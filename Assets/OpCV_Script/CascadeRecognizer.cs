using OpenCvSharp;
using OpenCvSharp.Demo;
using UnityEngine;

public class CascadeRecognizer : WebCamera
{
    public string cascadeFileName = "haarcascade_hand.xml"; // 手検出用Haar Cascadeファイル名
    public GameObject character; // キャラクターオブジェクト
    public RectTransform canvasRectTransform; // CanvasのRectTransform

    private CascadeClassifier cascadeHands;

    protected override void Awake()
    {
        base.Awake();

        // デバイスリストを取得
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length == 0)
        {
            Debug.LogError("カメラデバイスが見つかりません！");
            return;
        }

        Debug.Log("使用可能なカメラデバイス:");
        foreach (var device in devices)
        {
            Debug.Log($"デバイス名: {device.name}, FrontFacing: {device.isFrontFacing}");
        }

        // 仮想カメラをスキップし、物理カメラを優先
        string selectedDeviceName = null;
        foreach (var device in devices)
        {
            if (!device.name.Contains("Virtual Camera")) // 仮想カメラを無視
            {
                selectedDeviceName = device.name;
                break;
            }
        }

        if (string.IsNullOrEmpty(selectedDeviceName))
        {
            Debug.LogError("物理カメラが見つかりません！最初のデバイスを使用します。");
            selectedDeviceName = devices[0].name; // フォールバックとして最初のデバイスを使用
        }

        Debug.Log($"選択されたデバイス: {selectedDeviceName}");
        DeviceName = selectedDeviceName;

        // Haar Cascadeファイルのパス
        string cascadePath = System.IO.Path.Combine(Application.streamingAssetsPath, cascadeFileName);

        // 分類器の初期化
        cascadeHands = new CascadeClassifier();
        if (!cascadeHands.Load(cascadePath))
        {
            Debug.LogError("Haar Cascadeファイルのロードに失敗しました！");
            throw new System.Exception($"Failed to load Haar Cascade file: {cascadePath}");
        }
        Debug.Log("Cascade classifier loaded successfully.");
    }

    protected override bool ProcessTexture(WebCamTexture input, ref Texture2D output)
    {
        Debug.Log("ProcessTexture開始");

        // WebCamTextureの解像度を確認
        Debug.Log($"WebCamTexture解像度: {input.width}x{input.height}");

        // WebCamTexture から Mat に変換
        Mat image = OpenCvSharp.Unity.TextureToMat(input);

        // グレースケール変換
        Mat gray = image.CvtColor(ColorConversionCodes.BGR2GRAY);

        // ヒストグラム均等化
        Cv2.EqualizeHist(gray, gray);

        // 手検出
        OpenCvSharp.Rect[] detectedHands = cascadeHands.DetectMultiScale(gray, 1.1, 6);
        Debug.Log($"検出された手の数: {detectedHands.Length}");

        foreach (var hand in detectedHands)
        {
            Debug.Log($"検出された手: X={hand.X}, Width={hand.Width}");

            // 手の中心座標を計算
            float centerX = hand.TopLeft.X + hand.Width / 2f;
            Debug.Log($"手の中心X: {centerX}");

            // 映像を3分割
            float leftBoundary = gray.Width / 3f;
            float rightBoundary = 2 * gray.Width / 3f;

            Debug.Log($"映像分割: 左={leftBoundary}, 右={rightBoundary}");

            // 手の位置に応じてキャラクターの移動を決定
            if (centerX < leftBoundary)
            {
                // 左側に手がある場合
                Debug.Log("手が左側にあります。キャラクターを左に移動。");
                character.transform.position += new Vector3(-1f, 0, 0); // 左に移動
            } else if (centerX > rightBoundary)
            {
                // 右側に手がある場合
                Debug.Log("手が右側にあります。キャラクターを右に移動。");
                character.transform.position += new Vector3(1f, 0, 0); // 右に移動
            } else
            {
                // 中央に手がある場合
                Debug.Log("手が中央にあります。キャラクターを停止。");
            }
        }

        // Mat を Texture2D に変換して出力
        output = OpenCvSharp.Unity.MatToTexture(image);
        Debug.Log("ProcessTexture完了");
        return true;
    }
}
