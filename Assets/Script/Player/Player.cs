using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Vector2 initialPosition;
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;


    // Start is called before the first frame update
    void Start()
    {
        //// ゲームオブジェクトの位置を初期位置に設定
        //transform.position = initialPosition;

        // 例えば、初期位置を (0, 0) に設定
        transform.position = new Vector2(21, -2);
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //// 入力を取得
        //movement.x = Input.GetAxis("Horizontal");
        //movement.y = Input.GetAxis("Vertical");
    }


    void FixedUpdate()
    {
        // Rigidbody2Dを使って移動
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

    }
}
