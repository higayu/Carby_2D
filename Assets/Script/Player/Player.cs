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
        //// �Q�[���I�u�W�F�N�g�̈ʒu�������ʒu�ɐݒ�
        //transform.position = initialPosition;

        // �Ⴆ�΁A�����ʒu�� (0, 0) �ɐݒ�
        transform.position = new Vector2(21, -2);
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //// ���͂��擾
        //movement.x = Input.GetAxis("Horizontal");
        //movement.y = Input.GetAxis("Vertical");
    }


    void FixedUpdate()
    {
        // Rigidbody2D���g���Ĉړ�
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

    }
}
