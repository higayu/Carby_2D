using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;

public class Block : MonoBehaviour
{
    public bool isBeingSucked = false;  // 吸い込まれている状態かどうか
    protected Rigidbody2D rb;
    protected string Name = "ブロック";

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("スターブロックにぶつかったよ！");
        }
    }

    // 吸い込まれている場合、ターゲットの位置に向かって移動する
    public void MoveTowardsTarget(Vector3 targetPosition)
    {
        // 吸い込まれる位置に向かって移動
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 5f);
    }

    // 吸引される処理
    public void StartSuction(Transform player, float suikomiForce)
    {
        if (player == null)
        {
            Debug.LogError("Player is null!");
            return;
        }
        if (suikomiForce == null)
        {
            Debug.LogError("SuikomiForce is null!");
            return;
        }
        isBeingSucked = true;   // 吸引中のフラグを立てる
        StartCoroutine(SuctionCoroutine(suikomiForce,player));
    }

    protected IEnumerator SuctionCoroutine(float suikomiForce,Transform playerTransform)
    {
        if (playerTransform == null)
        {
            Debug.LogError("Player is null in SuctionCoroutine!");
            yield break;
        }
        // 吸引力を計算
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        rb.velocity = direction * suikomiForce;
        // プレイヤーに一定距離まで近づいたら吸引完了
        if (Vector2.Distance(playerTransform.position, transform.position) < 0.5f)
        {
            Debug.Log("吸い込み完了！");
            Destroy(gameObject); // NPCを削除
            yield break;
        }
        yield return null;  // 次のフレームまで待機
    }

    public void StopSuction()
    {
        isBeingSucked = false; // 吸い込みフラグを解除
        rb.velocity = Vector2.zero; // 吸引力を解除
        Debug.Log($"{Name} の吸い込みが解除されました。");
    }

}
