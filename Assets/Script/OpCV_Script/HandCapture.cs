using UnityEngine;
using OpenCvSharp;
using UnityEngine.UI;

public class HandCapture : MonoBehaviour
{
    public RawImage rawImage; // �f����\������UI
    public string cascadeFileName = "haarcascade_hand.xml"; // Haar Cascade�t�@�C����

    private WebCamTexture webCamTexture; // Web�J�����f��
    private CascadeClassifier cascadeClassifier; // Haar Cascade ���ފ�
    private Mat srcMat; // �J�����f���̌�Mat
    private Texture2D texture; // �\���p�̃e�N�X�`��

    void Start()
    {
        // Web�J�����̏�����
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length > 0)
        {
            webCamTexture = new WebCamTexture(devices[0].name, 640, 480, 30);
            webCamTexture.Play();
        }

        // Haar Cascade�̓ǂݍ���
        string cascadePath = System.IO.Path.Combine(Application.streamingAssetsPath, cascadeFileName);
        cascadeClassifier = new CascadeClassifier();
        if (!cascadeClassifier.Load(cascadePath))
        {
            Debug.LogError($"Haar Cascade�t�@�C���̓ǂݍ��݂Ɏ��s���܂����IPath: {cascadePath}");
        }

        // Mat�ƃe�N�X�`���̏�����
        srcMat = new Mat();
        texture = new Texture2D(640, 480, TextureFormat.RGBA32, false);
        rawImage.texture = texture;
    }

    void Update()
    {
        if (webCamTexture == null || !webCamTexture.isPlaying || webCamTexture.width <= 16)
            return;

        // WebCamTexture����Mat�ɕϊ�
        srcMat = OpenCvSharp.Unity.TextureToMat(webCamTexture);

        // Mat����̏ꍇ�̓G���[���o�͂��ďI��
        if (srcMat.Empty())
        {
            Debug.LogError("WebCamTexture����Mat�ւ̕ϊ��Ɏ��s���܂����I");
            return;
        }

        // �O���[�X�P�[���ϊ�
        Mat grayMat = new Mat();
        Cv2.CvtColor(srcMat, grayMat, ColorConversionCodes.BGR2GRAY);

        // Haar Cascade�Ŏ�����o
        OpenCvSharp.Rect[] hands = cascadeClassifier.DetectMultiScale(
            grayMat,
            scaleFactor: 1.1,
            minNeighbors: 6,
            flags: 0, // �t���O��0�Ŗ��Ȃ�
            minSize: new OpenCvSharp.Size(50, 50) // �ŏ����o�T�C�Y
        );

        // ���o���ꂽ��̏�������
        foreach (var hand in hands)
        {
            // ��̒��S���v�Z
            int centerX = hand.X + hand.Width / 2;
            int centerY = hand.Y + hand.Height / 2;

            // ����`��
            Cv2.Line(srcMat, new Point(centerX, centerY), new Point(centerX, centerY - 50), new Scalar(0, 255, 0), 2);

            // ��̋�`��`��
            Cv2.Rectangle(srcMat, hand, new Scalar(255, 0, 0), 2);

            // ��̒��S�ɓ_��`��
            Cv2.Circle(srcMat, new Point(centerX, centerY), 5, new Scalar(0, 255, 255), -1);
        }

        // Mat��Texture2D�ɕϊ����ĕ\��
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
