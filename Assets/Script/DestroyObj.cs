using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObj : MonoBehaviour
{
    public float deleteTime = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        // 2秒後にオブジェクトを削除
        Destroy(gameObject, deleteTime);
    }

    // 他のオブジェクトと衝突した時に呼ばれる
    void OnCollisionEnter2D(Collision2D collision)
    {
        // 衝突したオブジェクトのタグが "StarBlock" なら削除
        if (collision.gameObject.tag == "StarBlock")
        {
            Debug.Log("スターブロックに衝突！！");
            Destroy(gameObject);
        } else if (collision.gameObject.tag == "Ground")
        {
            Debug.Log("地面に衝突！！");
            Destroy(gameObject);
        } else if (collision.gameObject.tag == "Enemy1")
        {
            Debug.Log("ワドルディに衝突！！");

            // Enemy1 にセットされた WanderingAI スクリプトを取得
            WanderingAI enemyAI = collision.gameObject.GetComponent<WanderingAI>();

            if (enemyAI != null)
            {
                // ダメージを与える（5ポイントのダメージ）
                enemyAI.TakeDamage(10);
            }

            // 自身のオブジェクトは衝突後に削除
            Destroy(gameObject);
        }
    }

}
