using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Controller : MonoBehaviour
{

    public float speed = 2f;
    public LayerMask groundLayer; // 地面と判定するレイヤー
    private Rigidbody2D rb;

    public float jumpForce = 5f; // ジャンプ力

    public float detectionRadius = 5f; // プレイヤー検出半径
    public float fleeDistance = 3f; // プレイヤーから逃げる距離
    private Animator anim;
    private bool isGround = false; // 地面にいるかどうか
    public float normalGravityScale = 1f; // 通常の重力スケー


    private GameObject player;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (anim == null)
        {
            Debug.LogError("Animatorコンポーネントが見つかりません。");
        }

        if (rb == null)
        {
            Debug.LogError("Rigidbody2Dコンポーネントが見つかりません。");
        }

        // プレイヤーを探す
        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {

        Move();
        DetectPlayer();
    }



    private void Move()
    {
        // NPCの基本的なパトロール移動などを実装
        anim.SetFloat("Speed", Mathf.Abs(speed));
        rb.velocity = new Vector2(speed, rb.velocity.y);

        // 壁や端で方向を変える処理などを追加
        if (ShouldChangeDirection())
        {
            speed = -speed; // 反転
            Flip();
        }
    }

    private bool ShouldChangeDirection()
    {
        // 壁に当たる、または地面の端に到達したら方向を変える
        // 地面の端を判定するためにRaycastを使用
        Vector2 rayOrigin = transform.position; // NPCの現在位置からRaycastを発射
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 0.1f, groundLayer);

        // Raycastが地面にヒットしなかった場合、または前方に壁がある場合に方向を変更
        bool isAtEdge = hit.collider == null;

        // 前方にRaycastを打ち、壁に衝突するかどうかをチェック
        RaycastHit2D wallHit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, 0.1f, groundLayer);

        return isAtEdge || wallHit.collider != null; // 地面がない場合または壁に当たる場合に方向を変更
    }


    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1; // 反転
        transform.localScale = localScale;
    }

    private void DetectPlayer()
    {
        // プレイヤーを検出して、一定距離以内なら逃げる、攻撃するなどの行動を実装
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

            if (distanceToPlayer < detectionRadius)
            {
                if (distanceToPlayer < fleeDistance)
                {
                    // プレイヤーから逃げる
                    FleeFromPlayer();
                    if( anim.GetInteger("Speed") == 0)
                    {
                        anim.SetInteger("Speed", 1);
                    }

                } 
                else if (distanceToPlayer >6f) 
                {
                    // プレイヤーに向かって攻撃や近づく動作を実装
                    ApproachPlayer();
                    if (anim.GetInteger("Speed") == 0)
                    {
                        anim.SetInteger("Speed", 1);
                    }
                }
                else
                {
                    if (anim.GetInteger("Speed") == 1)
                    {
                        anim.SetInteger("Speed", 0);
                    }
                }
            }
        }
    }

    private void FleeFromPlayer()
    {
        Debug.Log("プレイヤーから逃げる！");
        // プレイヤーと反対方向に移動
        Vector2 fleeDirection = (transform.position - player.transform.position).normalized;
        rb.velocity = fleeDirection * speed;
    }

    private void ApproachPlayer()
    {
        Debug.Log("プレイヤーに近づく！");
        // プレイヤーに向かって移動
        Vector2 approachDirection = (player.transform.position - transform.position).normalized;
        rb.velocity = approachDirection * speed;
    }

    #region //-----------------------------【地面の当たり判定】----------------------------------------------//
    // 地面に接触したときの処理
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
            Debug.Log("地面に着地しました。");
            anim.SetInteger("Jump", 0);
            rb.gravityScale = normalGravityScale;
        }
    }

    // 地面から離れたときの処理
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = false;
            Debug.Log("地面から離れました。");
        }
    }
    #endregion //---------------------------------------------------------------------------//
}
