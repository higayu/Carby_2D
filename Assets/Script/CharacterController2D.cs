using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    public float speed = 5f;
    private Animator animator;
    private Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float move = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(move * speed, rb.velocity.y);
        rb.velocity = movement;

        // Animator��Speed�p�����[�^�[��ݒ�
        animator.SetFloat("Speed", Mathf.Abs(move));

        // �L�����N�^�[�̌�����ύX
        if (move > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        } else if (move < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
