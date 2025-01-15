using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public string Scene_name;

    private void OnCollisionStay2D(Collision2D collision)
    {
        //collisionに相手側の情報が格納される。
        // Debug.Log(collision.gameObject.name);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //コライダーが当たったら最初に呼ばれる
        //collisionに相手側の情報が格納される。
       // Debug.Log(collision.gameObject.name);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //コライダーが離れた時に呼ばれる
       // Debug.Log(collision.gameObject.name);
    }

    //////////////////////
    ////  Trigger系　////
    ////////////////////
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //コライダーが当たったら最初に呼ばれる
        //collisionに相手側の情報が格納される。
        //Debug.Log("ぶつかっているオブジェクト名："+collision.name);
        //Debug.Log("ドアに接触");

        //if (Input.GetKeyDown(KeyCode.UpArrow))//上矢印キーの入力
        //{
        //    destination();
        //}
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        //コライダーが当たっていると継続して呼ばれる
        //Debug.Log(collision.name);
        //Debug.Log("ぶつかっているオブジェクト名：" + collision.name);


        //if (Input.GetKeyDown(KeyCode.UpArrow))//上矢印キーの入力
        //{
        //    Debug.Log("ドアの前で上矢印キー入力");
        //    destination();
        //}

        //if (collision.gameObject.CompareTag("Player")) // Player タグで判定
        //{
        //    Debug.Log("ぶつかっているオブジェクト名：" + collision.name);

        //    if (Input.GetKeyDown(KeyCode.UpArrow)) // 上矢印キーの入力
        //    {
        //        Debug.Log("ドアの前で上矢印キー入力");
        //        destination();
        //    }

        //    if (Input.GetKeyDown(KeyCode.LeftArrow))
        //    {
        //        Debug.Log("ドアの前で左矢印キー入力");
        //    }

        //    if (Input.GetKeyDown(KeyCode.RightArrow))
        //    {
        //        Debug.Log("ドアの前で右矢印キー入力");
        //    }
        //}

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //コライダーがが離れた時に呼ばれる
        //Debug.Log(collision.name);
    }


}
