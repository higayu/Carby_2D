using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anime_Player : MonoBehaviour
{


    public const int speed = 5; // 通常の移動速度
    public float jumpForce = 5f; // ジャンプ力
    public float dashSpeedMultiplier = 2f; // ダッシュ時の速度倍率
    public int dashDuration = 10; // ダッシュの持続時間フレーム数

    public const float hoverGravityScale = 0.5f; // ホバリングモード時の重力スケール
    public const float normalGravityScale = 1f; // 通常時の重力スケール

    private Animator anim = null;
    private Rigidbody2D rb = null;
    private bool isGround = false; // 地面にいるかどうか
    private float realSpeed = speed; // 実際の速度

    public Vector2 boxSize = new Vector2(2f, 1f);// 矩形のサイズ（横幅を大きく、縦を小さくする）
    public float suikomiForce = 0.1f; // 吸い込みの力
    private bool isSucking = false; // 吸い込み中かどうかを示すフラグ
    private bool isHoubaru = false;//口の中に物に入っている状態
    public float closeDistance = 1f; // 接触とみなす距離
    private int Suikomi_Count = 0;

    private bool isFacingRight = true;

    public AudioSource audioSource;// AudioSourceコンポーネント
    public AudioClip suikomiSound; // 吸い込み時の音声クリップ
    public AudioClip hobaringSound; // ホバリングの音声クリップ
    public AudioClip starSound; // 星を吐き出すの音声クリップ

    public float power = 100f;//スターの発射速度
    public GameObject cannonBall;//スターオブジェクト
    public Transform shootPoint;//スターの発射口

    #region //------------------------------【startメソッド】---------------------------------------------//
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Nullチェック
        if (anim == null)
        {
            Debug.LogError("Animatorコンポーネントが見つかりません。");
        }

        if (rb == null)
        {
            Debug.LogError("Rigidbody2Dコンポーネントが見つかりません。");
        }

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("AudioSourceコンポーネントが見つかりません。");
        }

        if (suikomiSound == null)
        {
            Debug.LogError("吸い込み音声クリップが設定されていません。");
        }
    }
    #endregion //---------------------------------------------------------------------------//

    // Update is called once per frame
    #region  ----------【 アップデートイベント 】---------------------------------------------
    void Update()
    {

        #region  ----------【 オブジェクトの有無 】---------------------------------------------
        if (anim == null || rb == null || audioSource == null || audioSource == null)
        {
            return; // 必要なコンポーネントが見つからない場合は処理を中断
        }
        #endregion  ----------【  】---------------------------------------------


        #region  ----------【 移動処理 】---------------------------------------------
        // 入力を取得
        float horizontalKey = Input.GetAxis("Horizontal");
        float xSpeed = 0.0f;

        if (horizontalKey > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            anim.SetFloat("Speed", 1);
            xSpeed = realSpeed;
            //Debug.Log("右に移動");

        } else if (horizontalKey < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            anim.SetFloat("Speed", 1);
            xSpeed = -(float)speed;
          //  Debug.Log("左に移動");
        } else
        {
            anim.SetFloat("Speed", 0);
            xSpeed = 0.0f;
            realSpeed = speed; // 停止時に速度を通常に戻す
        }

        isFacingRight = transform.localScale.x > 0;

        //if (isFacingRight)
        //{
        //    Debug.Log("右向き");
        //} else
        //{
        //    Debug.Log("左向き");
        //}


        //Debug.Log("スピード : " + xSpeed);
        // 横移動の速度を設定
        rb.velocity = new Vector2(xSpeed, rb.velocity.y);
        //Debug.Log("移動後の処理" + rb.velocity);

        #endregion  ----------【移動末尾 】---------------------------------------------


        #region  ----------【 ジャンプ処理 】---------------------------------------------
        // スペースキーが押された場合、かつキャラクターが地面にいる場合にジャンプ
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            anim.SetInteger("Jump", 1);
           // Debug.Log("ジャンプ確定！！！ isGround: " + isGround);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        } else if (Input.GetKeyDown(KeyCode.Space) && !isGround && !isHoubaru)
        {

            Debug.Log(anim.GetInteger("Jump") + ": ジャンプGET");

            if(anim.GetInteger("Jump") == 2)
            {
                rb.gravityScale = hoverGravityScale; // ホバリングモード時の重力スケールを設定
               // Debug.Log("ホバリングモード継続！！！");
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                audioSource.PlayOneShot(hobaringSound);
            }
            else
            {
                anim.SetInteger("Jump", 2);
               // Debug.Log("ホバリングモード確定！！！ isGround: " + isGround);
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                audioSource.PlayOneShot(hobaringSound);
            }

        } else
        {

        }
        #endregion  ----------【 ジャンプ処理末尾】---------------------------------------------


        #region  ----------【 吸い込み処理 】---------------------------------------------
        // 吸い込みボタンが押されている間に吸い込み攻撃を継続
        if (Input.GetKey(KeyCode.S) && !isSucking && !isHoubaru)
        {
            isSucking = true;
            Debug.Log("Sキー");
            //Debug.Log("Sキーを押しはじめ");
            Suikomi_Count = 0;
            anim.SetInteger("suikomi", 1);
            audioSource.PlayOneShot(suikomiSound);
            SuikomiStarBlock();
            //StartCoroutine(SuckStarBlock());

        }
        else if (Input.GetKey(KeyCode.S) && isSucking && !isHoubaru)
        {
            Debug.Log("吸い込み継続");
            SuikomiStarBlock();
            isSucking = true;
            Suikomi_Count++;
        }
        else if(Input.GetKeyUp(KeyCode.S))
        {
            if (isSucking && !isHoubaru)
            {
                Debug.Log("吸い込みを解除");
                anim.SetInteger("suikomi", 0);
            }
            isSucking = false;
        }
        #endregion  ----------【 吸い込み処理末尾 】---------------------------------------------



        #region  ----------【 吐き出す 】---------------------------------------------
        if (Input.GetKeyDown(KeyCode.V) && isHoubaru)
        {
            Shoot(isFacingRight);
            anim.SetInteger("suikomi", 0);
            isSucking = false;
            isHoubaru = false;
        }
        #endregion  ----------【 吐き出す末尾 】---------------------------------------------
    }
    #endregion --------------------------------------------------------------------------


    #region --------------------------【吐き出す　メソッド】------------------------------------
    void Shoot(bool isFacingRight)
    {

        if (isFacingRight)
        {
            Debug.Log("ショット！！");
            audioSource.PlayOneShot(starSound);
            GameObject newBullet = Instantiate(cannonBall, shootPoint.position, Quaternion.identity) as GameObject;
            newBullet.GetComponent<Rigidbody2D>().AddForce(Vector3.right * power);
        } else
        {
            Debug.Log("ショット！！（左向き）");
            audioSource.PlayOneShot(starSound);
            GameObject newBullet = Instantiate(cannonBall, shootPoint.position, Quaternion.identity) as GameObject;
            // 左向きに力を加える
            newBullet.GetComponent<Rigidbody2D>().AddForce(Vector3.left * power);
            // もし弾のスプライトを反転したい場合
            Vector3 bulletScale = newBullet.transform.localScale;
            bulletScale.x *= -1; // 左右反転
            newBullet.transform.localScale = bulletScale;
        }

    }
    #endregion -------------------------------------------------------------------------------


    #region ----------------------------【吸い込み　メソッド】----------------------------------------
    void SuikomiStarBlock()
    {
        Debug.Log("吸い込み攻撃を実行！： " + Suikomi_Count + "回");

        // 吸い込み範囲の中心点をプレイヤーの前方に設定
        Vector2 boxCenter = (Vector2)transform.position + (isFacingRight ? Vector2.right : Vector2.left) * 1f;

        // 吸い込み範囲内のコライダーを取得
        Collider2D[] colliders = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0f);

        bool objectFound = false;

        foreach (Collider2D collider in colliders)
        {
            // 吸引対象が"Enemy1"（WanderingAIを持つオブジェクト）である場合
            if (collider.CompareTag("Enemy1"))
            {
                objectFound = true;

                Rigidbody2D targetRb = collider.GetComponent<Rigidbody2D>();
                if (targetRb != null)
                {
                    // プレイヤーの位置に向かって吸い込む力を加える
                    Vector2 direction = (transform.position - collider.transform.position).normalized;

                    // 吸引力をNPCに適用
                    //targetRb.velocity = direction * suikomiForce;
                    OnTriggerEnter2D(collider);

                    // プレイヤーとの距離を測定
                    float distance = Vector2.Distance(transform.position, collider.transform.position);
                    if (distance < closeDistance && Suikomi_Count > 10)
                    {
                        Debug.Log(collider.name + " を吸い込んだ！");
                        isSucking = false;
                        anim.SetInteger("suikomi", 2);
                        isHoubaru = true;
                        Debug.Log("吸い込み完了 ほうばるモード移行 " + anim.GetInteger("suikomi"));
                        Destroy(collider.gameObject); // NPCを削除
                    }
                }
            }
        }

        if (!objectFound)
        {
            Debug.Log("範囲内にEnemy1が見つかりません！");
        }
    }

    // プレイヤーの吸引範囲内にNPCが入ったら吸引を開始する例
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy1"))
        {
            Enemy_AI enemy = other.GetComponent<Enemy_AI>();
            if (enemy != null)
            {
                enemy.StartSuction(transform); // プレイヤーのTransformを渡す
            }
        }
    }



    #endregion ---------------------------------------------------------------------------------------

    // 吸い込み攻撃の範囲を視覚化（デバッグ用）

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // 吸い込み範囲の矩形の中心
        Vector2 boxCenter = (Vector2)transform.position + (isFacingRight ? Vector2.right : Vector2.left) * 1.9f;

        // 矩形の範囲を描画
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }

    //----------------------------------------------------------------------------------//



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
