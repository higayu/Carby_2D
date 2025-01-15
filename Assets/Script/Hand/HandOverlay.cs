using OpenCvSharp;
using OpenCvSharp.Demo;
using UnityEngine;
using UnityEngine.UI;

public class HandOverlay : WebCamera
{
    public TextAsset handCascadeFile; // 手検出用のHaar Cascadeファイル
    public GameObject handObject; // 手の位置に合わせて移動するオブジェクト
    public RectTransform canvasRectTransform; // CanvasのRectTransform

    private CascadeClassifier cascadeHands;

    protected override void Awake()
    {
        base.Awake();

        // Haar Cascadeの初期化
        FileStorage storageHands = new FileStorage(handCascadeFile.text, FileStorage.Mode.Read | FileStorage.Mode.Memory);
        cascadeHands = new CascadeClassifier();
        if (!cascadeHands.Read(storageHands.GetFirstTopLevelNode()))
        {
            throw new System.Exception("Failed to load hand cascade classifier");
        }
        Debug.Log("Hand cascade classifier loaded successfully.");
    }

    protected override bool ProcessTexture(WebCamTexture input, ref Texture2D output)
    {
        // カメラ映像をMatに変換
        Mat image = OpenCvSharp.Unity.TextureToMat(input);

        // 映像をグレースケールに変換
        Mat gray = image.CvtColor(ColorConversionCodes.BGR2GRAY);

        // ヒストグラム均等化（顔検出の精度を向上）
        Mat equalizeHistMat = new Mat();
        Cv2.EqualizeHist(gray, equalizeHistMat);

        // 手の検出
        OpenCvSharp.Rect[] detectedHands = cascadeHands.DetectMultiScale(gray, 1.1, 6);
        Debug.Log($"検出された手の数: {detectedHands.Length}");

        foreach (var hand in detectedHands)
        {
            // 手の中心座標を計算
            float centerX = hand.TopLeft.X + hand.Width / 2f;
            float centerY = hand.TopLeft.Y + hand.Height / 2f;

            Debug.Log($"手の中心: X={centerX}, Y={centerY}");

            // カメラの映像からビューポート座標系に変換
            Vector2 viewportPos = new Vector2(centerX / gray.Width, 1 - centerY / gray.Height);

            // ビューポート座標をCanvas内の座標に変換
            Vector3 canvasPos = new Vector3(
                viewportPos.x * canvasRectTransform.sizeDelta.x,
                viewportPos.y * canvasRectTransform.sizeDelta.y,
                0 // Z軸は平面上のため0
            );

            Debug.Log($"Canvas上の手の位置: X={canvasPos.x}, Y={canvasPos.y}");

            // 手オブジェクトの位置を更新
            handObject.GetComponent<RectTransform>().anchoredPosition = canvasPos;
        }

        // 画像をTexture2Dに戻して出力
        output = OpenCvSharp.Unity.MatToTexture(image);
        return true;
    }
}
