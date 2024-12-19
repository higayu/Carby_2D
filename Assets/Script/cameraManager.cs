using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraManager : MonoBehaviour
{

    //public GameObject target; // �Ǐ]����Ώۂ����߂�ϐ�
    public Transform target;
    Vector3 pos;              // �J�����̏����ʒu���L�����邽�߂̕ϐ�

    private BGMController bgmController;

    // Start is called before the first frame update
    void Start()
    {
        pos = Camera.main.gameObject.transform.position; //�J�����̏����ʒu��ϐ�pos�ɓ����

        bgmController = FindObjectOfType<BGMController>();

        if (bgmController == null)
        {
            Debug.LogError("BGMController��������܂���B");
        }
    }

 

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    bgmController.PauseBGM(); // P�L�[��BGM���ꎞ��~
        //}

        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    bgmController.ResumeBGM(); // R�L�[��BGM���ĊJ
        //}

        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    bgmController.StopBGM(); // S�L�[��BGM���~
        //}

        //------------------------------------------------------------------------------//

        Vector3 cameraPos = target.transform.position; // cameraPos�Ƃ����ϐ������A�Ǐ]����Ώۂ̈ʒu������

        if (target != null)
        {
            // ���炩�̏����i�Ⴆ�΁A�J�����̈ʒu���^�[�Q�b�g�ɍ��킹��j
        }
      
        // �����Ώۂ̉��ʒu��0��菬�����ꍇ
        if (target.transform.position.x < 0)
        {
            cameraPos.x = 0; // �J�����̉��ʒu��0������
        }

        // �����Ώۂ̏c�ʒu��0��菬�����ꍇ
        if (target.transform.position.y < 0)
        {
            cameraPos.y = -1;  // �J�����̏c�ʒu��0������
        }

        // �����Ώۂ̏c�ʒu��0���傫���ꍇ
        if (target.transform.position.y > 0)
        {
            cameraPos.y = target.transform.position.y;   // �J�����̏c�ʒu�ɑΏۂ̈ʒu������
        }

        cameraPos.z = -10; // �J�����̉��s���̈ʒu��-10������
        Camera.main.gameObject.transform.position = cameraPos; //�@�J�����̈ʒu�ɕϐ�cameraPos�̈ʒu������

    }
}