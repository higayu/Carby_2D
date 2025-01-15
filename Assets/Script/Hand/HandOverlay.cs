using OpenCvSharp;
using OpenCvSharp.Demo;
using UnityEngine;
using UnityEngine.UI;

public class HandOverlay : WebCamera
{
    public TextAsset handCascadeFile; // �茟�o�p��Haar Cascade�t�@�C��
    public GameObject handObject; // ��̈ʒu�ɍ��킹�Ĉړ�����I�u�W�F�N�g
    public RectTransform canvasRectTransform; // Canvas��RectTransform

    private CascadeClassifier cascadeHands;

    protected override void Awake()
    {
        base.Awake();

        // Haar Cascade�̏�����
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
        // �J�����f����Mat�ɕϊ�
        Mat image = OpenCvSharp.Unity.TextureToMat(input);

        // �f�����O���[�X�P�[���ɕϊ�
        Mat gray = image.CvtColor(ColorConversionCodes.BGR2GRAY);

        // �q�X�g�O�����ϓ����i�猟�o�̐��x������j
        Mat equalizeHistMat = new Mat();
        Cv2.EqualizeHist(gray, equalizeHistMat);

        // ��̌��o
        OpenCvSharp.Rect[] detectedHands = cascadeHands.DetectMultiScale(gray, 1.1, 6);
        Debug.Log($"���o���ꂽ��̐�: {detectedHands.Length}");

        foreach (var hand in detectedHands)
        {
            // ��̒��S���W���v�Z
            float centerX = hand.TopLeft.X + hand.Width / 2f;
            float centerY = hand.TopLeft.Y + hand.Height / 2f;

            Debug.Log($"��̒��S: X={centerX}, Y={centerY}");

            // �J�����̉f������r���[�|�[�g���W�n�ɕϊ�
            Vector2 viewportPos = new Vector2(centerX / gray.Width, 1 - centerY / gray.Height);

            // �r���[�|�[�g���W��Canvas���̍��W�ɕϊ�
            Vector3 canvasPos = new Vector3(
                viewportPos.x * canvasRectTransform.sizeDelta.x,
                viewportPos.y * canvasRectTransform.sizeDelta.y,
                0 // Z���͕��ʏ�̂���0
            );

            Debug.Log($"Canvas��̎�̈ʒu: X={canvasPos.x}, Y={canvasPos.y}");

            // ��I�u�W�F�N�g�̈ʒu���X�V
            handObject.GetComponent<RectTransform>().anchoredPosition = canvasPos;
        }

        // �摜��Texture2D�ɖ߂��ďo��
        output = OpenCvSharp.Unity.MatToTexture(image);
        return true;
    }
}
