using OpenCvSharp;
using OpenCvSharp.Demo;
using UnityEngine;

public class Hand : WebCamera
{
    public string cascadeFileName = "haarcascade_hand.xml"; // �茟�o�pHaar Cascade�t�@�C����
    public GameObject HandObject; // ��̓����ɘA������I�u�W�F�N�g
    public RectTransform canvasRectTransform; // Canvas��RectTransform

    private CascadeClassifier cascadeHands;

    protected override void Awake()
    {
        base.Awake();

        // �f�o�C�X���X�g���擾
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length == 0)
        {
            Debug.LogError("�J�����f�o�C�X��������܂���I");
            return;
        }

        Debug.Log("�g�p�\�ȃJ�����f�o�C�X:");
        foreach (var device in devices)
        {
            Debug.Log($"�f�o�C�X��: {device.name}, FrontFacing: {device.isFrontFacing}");
        }

        // ���z�J�������X�L�b�v���A�����J������D��
        string selectedDeviceName = null;
        foreach (var device in devices)
        {
            if (!device.name.Contains("Virtual Camera")) // ���z�J�����𖳎�
            {
                selectedDeviceName = device.name;
                break;
            }
        }

        if (string.IsNullOrEmpty(selectedDeviceName))
        {
            Debug.LogError("�����J������������܂���I�ŏ��̃f�o�C�X���g�p���܂��B");
            selectedDeviceName = devices[0].name; // �t�H�[���o�b�N�Ƃ��čŏ��̃f�o�C�X���g�p
        }

        Debug.Log($"�I�����ꂽ�f�o�C�X: {selectedDeviceName}");
        DeviceName = selectedDeviceName;

        // Haar Cascade�t�@�C���̃p�X
        string cascadePath = System.IO.Path.Combine(Application.streamingAssetsPath, cascadeFileName);

        // ���ފ�̏�����
        cascadeHands = new CascadeClassifier();
        if (!cascadeHands.Load(cascadePath))
        {
            Debug.LogError("Haar Cascade�t�@�C���̃��[�h�Ɏ��s���܂����I");
            throw new System.Exception($"Failed to load Haar Cascade file: {cascadePath}");
        }
        Debug.Log("Cascade classifier loaded successfully.");
    }

    protected override bool ProcessTexture(WebCamTexture input, ref Texture2D output)
    {
        Debug.Log("ProcessTexture�J�n");

        // WebCamTexture�̉𑜓x���m�F
        Debug.Log($"WebCamTexture�𑜓x: {input.width}x{input.height}");

        // WebCamTexture ���� Mat �ɕϊ�
        Mat image = OpenCvSharp.Unity.TextureToMat(input);

        // �J�����f���𐅕����]
        Cv2.Flip(image, image, FlipMode.Y);
        Debug.Log("�J�����f���𐅕����]");

        // �O���[�X�P�[���ϊ�
        Mat gray = image.CvtColor(ColorConversionCodes.BGR2GRAY);

        // �q�X�g�O�����ϓ���
        Cv2.EqualizeHist(gray, gray);

        // �茟�o
        OpenCvSharp.Rect[] detectedHands = cascadeHands.DetectMultiScale(gray, 1.1, 6);
        Debug.Log($"���o���ꂽ��̐�: {detectedHands.Length}");

        foreach (var hand in detectedHands)
        {
            Debug.Log($"���o���ꂽ��: X={hand.X}, Y={hand.Y}, Width={hand.Width}, Height={hand.Height}");

            // ��̒��S���W���v�Z
            float centerX = hand.TopLeft.X + hand.Width / 2f;
            float centerY = hand.TopLeft.Y + hand.Height / 2f;
            Debug.Log($"��̒��S: X={centerX}, Y={centerY}");

            // �r���[�|�[�g���W�n�ɕϊ�
            Vector2 viewportPos = new Vector2(centerX / gray.Width, 1 - centerY / gray.Height);

            // Canvas���̍��W�ɕϊ�
            Vector3 worldPosition = new Vector3(
                viewportPos.x * canvasRectTransform.rect.width,
                viewportPos.y * canvasRectTransform.rect.height,
                HandObject.transform.position.z // Z���W�͌Œ�
            );

            Debug.Log($"HandObject�̐V�����ʒu: X={worldPosition.x}, Y={worldPosition.y}");

            // HandObject�̈ʒu���X�V
            HandObject.transform.position = worldPosition;
        }

        // Mat �� Texture2D �ɕϊ����ďo��
        output = OpenCvSharp.Unity.MatToTexture(image);
        Debug.Log("ProcessTexture����");
        return true;
    }
}
