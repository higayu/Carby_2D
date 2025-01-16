using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraManager : MonoBehaviour
{
    public Transform target;
    Vector3 pos;

    public BGMController bgmController;

    // カメラのY軸オフセット（対象を画面の下に配置する距離を指定）
    public float verticalOffset = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        pos = Camera.main.gameObject.transform.position; // カメラの初期位置を変数posに入れる

        bgmController = FindObjectOfType<BGMController>();

        if (bgmController == null)
        {
            Debug.LogError("BGMControllerが見つかりません。");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraPos = target.transform.position; // cameraPosという変数を作り、追従する対象の位置を入れる

        if (target != null)
        {
            // 特にここでは対象を追従する処理がメインになる
        }

        // 対象のX軸が0より小さい場合、カメラのX軸位置を制限
        if (target.transform.position.x < 0)
        {
            cameraPos.x = 0;
        }

        // Y軸の位置を調整 (反転してカメラを上にずらし、対象が下に見えるようにする)
        cameraPos.y = target.transform.position.y - verticalOffset;

        // カメラの奥行きを固定
        cameraPos.z = -10;

        // カメラの位置を更新
        Camera.main.gameObject.transform.position = cameraPos;
    }
}
