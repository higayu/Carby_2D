using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraManager : MonoBehaviour
{

    //public GameObject target; // 追従する対象を決める変数
    public Transform target;
    Vector3 pos;              // カメラの初期位置を記憶するための変数

    private BGMController bgmController;

    // Start is called before the first frame update
    void Start()
    {
        pos = Camera.main.gameObject.transform.position; //カメラの初期位置を変数posに入れる

        bgmController = FindObjectOfType<BGMController>();

        if (bgmController == null)
        {
            Debug.LogError("BGMControllerが見つかりません。");
        }
    }

 

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    bgmController.PauseBGM(); // PキーでBGMを一時停止
        //}

        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    bgmController.ResumeBGM(); // RキーでBGMを再開
        //}

        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    bgmController.StopBGM(); // SキーでBGMを停止
        //}

        //------------------------------------------------------------------------------//

        Vector3 cameraPos = target.transform.position; // cameraPosという変数を作り、追従する対象の位置を入れる

        if (target != null)
        {
            // 何らかの処理（例えば、カメラの位置をターゲットに合わせる）
        }
      
        // もし対象の横位置が0より小さい場合
        if (target.transform.position.x < 0)
        {
            cameraPos.x = 0; // カメラの横位置に0を入れる
        }

        // もし対象の縦位置が0より小さい場合
        if (target.transform.position.y < 0)
        {
            cameraPos.y = -1;  // カメラの縦位置に0を入れる
        }

        // もし対象の縦位置が0より大きい場合
        if (target.transform.position.y > 0)
        {
            cameraPos.y = target.transform.position.y;   // カメラの縦位置に対象の位置を入れる
        }

        cameraPos.z = -10; // カメラの奥行きの位置に-10を入れる
        Camera.main.gameObject.transform.position = cameraPos; //　カメラの位置に変数cameraPosの位置を入れる

    }
}