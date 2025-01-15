using UnityEngine;
using OpenCvSharp;
using UnityEngine.UI;

public class HandCapture : MonoBehaviour
{
    public RawImage rawImage; // 映像を表示するUI
    public string cascadeFileName = "haarcascade_hand.xml"; // Haar Cascadeファイル名

    private WebCamTexture webCamTexture; // Webカメラ映像
    private CascadeClassifier cascadeClassifier; // Haar Cascade 分類器
    private Mat srcMat; // カメラ映像の元Mat
    private Texture2D texture; // 表示用のテクスチャ

    void Start()
    {
        // Webカメラの初期化
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length > 0)
        {
            webCamTexture = new WebCamTexture(devices[0].name, 640, 480, 30);
            webCamTexture.Play();
        }

        // Haar Cascadeの読み込み
        string cascadePath = System.IO.Path.Combine(Application.streamingAssetsPath, cascadeFileName);
        cascadeClassifier = new CascadeClassifier();
        if (!cascadeClassifier.Load(cascadePath))
        {
            Debug.LogError($"Haar Cascadeファイルの読み込みに失敗しました！Path: {cascadePath}");
        }

        // Matとテクスチャの初期化
        srcMat = new Mat();
        texture = new Texture2D(640, 480, TextureFormat.RGBA32, false);
        rawImage.texture = texture;
    }

    void Update()
    {
        if (webCamTexture == null || !webCamTexture.isPlaying || webCamTexture.width <= 16)
            return;

        // WebCamTextureからMatに変換
        srcMat = OpenCvSharp.Unity.TextureToMat(webCamTexture);

        // Matが空の場合はエラーを出力して終了
        if (srcMat.Empty())
        {
            Debug.LogError("WebCamTextureからMatへの変換に失敗しました！");
            return;
        }

        // グレースケール変換
        Mat grayMat = new Mat();
        Cv2.CvtColor(srcMat, grayMat, ColorConversionCodes.BGR2GRAY);

        // Haar Cascadeで手を検出
        OpenCvSharp.Rect[] hands = cascadeClassifier.DetectMultiScale(
            grayMat,
            scaleFactor: 1.1,
            minNeighbors: 6,
            flags: 0, // フラグは0で問題なし
            minSize: new OpenCvSharp.Size(50, 50) // 最小検出サイズ
        );

        // 検出された手の情報を処理
        foreach (var hand in hands)
        {
            // 手の中心を計算
            int centerX = hand.X + hand.Width / 2;
            int centerY = hand.Y + hand.Height / 2;

            // 線を描画
            Cv2.Line(srcMat, new Point(centerX, centerY), new Point(centerX, centerY - 50), new Scalar(0, 255, 0), 2);

            // 手の矩形を描画
            Cv2.Rectangle(srcMat, hand, new Scalar(255, 0, 0), 2);

            // 手の中心に点を描画
            Cv2.Circle(srcMat, new Point(centerX, centerY), 5, new Scalar(0, 255, 255), -1);
        }

        // MatをTexture2Dに変換して表示
        rawImage.texture = OpenCvSharp.Unity.MatToTexture(srcMat, texture);
    }

    private void OnDestroy()
    {
        if (webCamTexture != null)
        {
            webCamTexture.Stop();
        }

        if (cascadeClassifier != null)
        {
            cascadeClassifier.Dispose();
        }
    }
}
