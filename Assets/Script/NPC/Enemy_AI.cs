using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AI : MonoBehaviour
{
    public string Name;
    public float moveSpeed = 1f;             // 移動速度
    public float changeDirectionInterval = 2f; // 方向を変更する間隔
    protected float movementDirection;         // 移動方向（1: 右, -1: 左）
    protected Rigidbody2D rb;
    protected Animator anim;
    protected int _HP = 10;
    protected bool isBeingSucked = false;      // 吸引中かどうかのフラグ

    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        StartCoroutine(ChangeDirectionRoutine());
    }

    protected void Update()
    {
        if (!isBeingSucked)  // 吸引中でない場合にのみ移動
        {
            Move();
        } else
        {
            //Debug.Log("吸い込まれ中");
        }
    }

    protected void IS_Falling()
    {
        // 例えば、Y座標が -50 を下回った場合にキャラクターが落下したと判定
        if (transform.position.y <= -50)
        {
            _HP = 0;
            Debug.Log(Name + "が限界の高さまで落ちました。 Y座標：" + transform.position.y);
            CheckHP(); // HPが0になったら削除処理を実行
        }
    }


    protected void Move()
    {
        // 現在の移動方向に基づいてx軸のみで移動する
        rb.velocity = new Vector2(movementDirection * moveSpeed, rb.velocity.y);
        anim.SetInteger("Speed", 1);

        // 移動方向に基づいてキャラクターの向きを設定
        if (movementDirection > 0)
        {
            // 右向き
            transform.localScale = new Vector3(1, 1, 1);
        } else if (movementDirection < 0)
        {
            // 左向き
            transform.localScale = new Vector3(-1, 1, 1);
        }
        IS_Falling();
    }

    protected IEnumerator ChangeDirectionRoutine()
    {
        while (true)
        {
            // ランダムに右か左に移動方向を変更（1か-1を取得）
            movementDirection = GetRandomDirection();

            // 指定された時間待機
            yield return new WaitForSeconds(changeDirectionInterval);
        }
    }

    protected float GetRandomDirection()
    {
        // ランダムに右（1）か左（-1）を選択
        return Random.value > 0.5f ? 1f : -1f;
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        // 壁などに衝突した場合は方向を変える
        if (collision.gameObject.CompareTag("StarBlock"))
        {
            movementDirection = -movementDirection; // 方向を逆にする
        }
    }

    // ダメージを受ける処理
    public void TakeDamage(int damage)
    {
        _HP -= damage;
        Debug.Log("Enemy HP: " + _HP);
        CheckHP();
    }

    // HPが0以下になったらオブジェクトを削除
    protected void CheckHP()
    {
        if (_HP <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Enemy1 が破壊されました！");
        }
    }

    // 吸引される処理
    public void StartSuction(Transform player, float suikomiForce)
    {
        isBeingSucked = true;   // 吸引中のフラグを立てる
        StartCoroutine(SuctionCoroutine(suikomiForce,player));
    }

    protected IEnumerator SuctionCoroutine(float suikomiForce, Transform playerTransform)
    {

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
        
        Debug.Log("吸い込みしゅうりょう");
    }

    public void StopSuction()
    {
        isBeingSucked = false; // 吸い込みフラグを解除
        rb.velocity = Vector2.zero; // 吸引力を解除
        Debug.Log($"{Name} の吸い込みが解除されました。");
    }

}
