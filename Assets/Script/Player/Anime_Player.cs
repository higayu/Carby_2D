using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Anime_Player : MonoBehaviour
{


    public const int speed = 5; // �ʏ�̈ړ����x
    public float jumpForce = 5f; // �W�����v��
    public float dashSpeedMultiplier = 2f; // �_�b�V�����̑��x�{��
    public int dashDuration = 10; // �_�b�V���̎������ԃt���[����

    public const float hoverGravityScale = 0.5f; // �z�o�����O���[�h���̏d�̓X�P�[��
    public const float normalGravityScale = 1f; // �ʏ펞�̏d�̓X�P�[��

    private Animator anim = null;
    private Rigidbody2D rb = null;
    private bool isGround = false; // �n�ʂɂ��邩�ǂ���
    private float realSpeed = speed; // ���ۂ̑��x

    public Vector2 boxSize = new Vector2(2f, 1f);// ��`�̃T�C�Y�i������傫���A�c������������j
    public float suikomiForce = 0.1f; // �z�����݂̗�
    private bool isSucking = false; // �z�����ݒ����ǂ����������t���O
    private bool isHoubaru = false;//���̒��ɕ��ɓ����Ă�����
    public float closeDistance = 1f; // �ڐG�Ƃ݂Ȃ�����
    private int Suikomi_Count = 0;

    private bool isFacingRight = true;

    public AudioSource audioSource;// AudioSource�R���|�[�l���g
    public AudioClip suikomiSound; // �z�����ݎ��̉����N���b�v
    public AudioClip hobaringSound; // �z�o�����O�̉����N���b�v
    public AudioClip starSound; // ����f���o���̉����N���b�v

    public float power = 100f;//�X�^�[�̔��ˑ��x
    public GameObject cannonBall;//�X�^�[�I�u�W�F�N�g
    public Transform shootPoint;//�X�^�[�̔��ˌ�


    #region -------------------�y �h�A �z------------------
    private bool isTouchingDoor = false; // �h�A�ƐڐG���Ă��邩�ǂ����������t���O
    private string Door_Name;
    #endregion ----------------------------------------------

    #region //------------------------------�ystart���\�b�h�z---------------------------------------------//
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Null�`�F�b�N
        if (anim == null)
        {
            Debug.LogError("Animator�R���|�[�l���g��������܂���B");
        }

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D�R���|�[�l���g��������܂���B");
        }

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("AudioSource�R���|�[�l���g��������܂���B");
        }

        if (suikomiSound == null)
        {
            Debug.LogError("�z�����݉����N���b�v���ݒ肳��Ă��܂���B");
        }
    }
    #endregion //---------------------------------------------------------------------------//

    // Update is called once per frame
    #region  ----------�y �A�b�v�f�[�g�C�x���g �z---------------------------------------------
    void Update()
    {

        #region  ----------�y �I�u�W�F�N�g�̗L�� �z---------------------------------------------
        if (anim == null || rb == null || audioSource == null || audioSource == null)
        {
            return; // �K�v�ȃR���|�[�l���g��������Ȃ��ꍇ�͏����𒆒f
        }
        #endregion  ----------�y  �z---------------------------------------------


        #region  ----------�y �ړ����� �z---------------------------------------------
        // ���͂��擾
        float horizontalKey = Input.GetAxis("Horizontal");
        float xSpeed = 0.0f;

        if (horizontalKey > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            anim.SetFloat("Speed", 1);
            xSpeed = realSpeed;
            //Debug.Log("�E�Ɉړ�");

        } else if (horizontalKey < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            anim.SetFloat("Speed", 1);
            xSpeed = -(float)speed;
          //  Debug.Log("���Ɉړ�");
        } else
        {
            anim.SetFloat("Speed", 0);
            xSpeed = 0.0f;
            realSpeed = speed; // ��~���ɑ��x��ʏ�ɖ߂�
        }

        isFacingRight = transform.localScale.x > 0;

        //if (isFacingRight)
        //{
        //    Debug.Log("�E����");
        //} else
        //{
        //    Debug.Log("������");
        //}


        //Debug.Log("�X�s�[�h : " + xSpeed);
        // ���ړ��̑��x��ݒ�
        rb.velocity = new Vector2(xSpeed, rb.velocity.y);
        //Debug.Log("�ړ���̏���" + rb.velocity);

        #endregion  ----------�y�ړ����� �z---------------------------------------------


        #region  ----------�y �W�����v���� �z---------------------------------------------
        // �X�y�[�X�L�[�������ꂽ�ꍇ�A���L�����N�^�[���n�ʂɂ���ꍇ�ɃW�����v
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            anim.SetInteger("Jump", 1);
           // Debug.Log("�W�����v�m��I�I�I isGround: " + isGround);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        } else if (Input.GetKeyDown(KeyCode.Space) && !isGround && !isHoubaru)
        {

            Debug.Log(anim.GetInteger("Jump") + ": �W�����vGET");

            if(anim.GetInteger("Jump") == 2)
            {
                rb.gravityScale = hoverGravityScale; // �z�o�����O���[�h���̏d�̓X�P�[����ݒ�
               // Debug.Log("�z�o�����O���[�h�p���I�I�I");
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                audioSource.PlayOneShot(hobaringSound);
            }
            else
            {
                anim.SetInteger("Jump", 2);
               // Debug.Log("�z�o�����O���[�h�m��I�I�I isGround: " + isGround);
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                audioSource.PlayOneShot(hobaringSound);
            }

        }
        else if(!isGround && anim.GetInteger("Jump") == 0)
        {
            anim.SetInteger("Jump", 1);
        }
        #endregion  ----------�y �W�����v���������z---------------------------------------------


        #region  ----------�y �z�����ݏ��� �z---------------------------------------------
        // �z�����݃{�^����������Ă���Ԃɋz�����ݍU�����p��
        if (Input.GetKey(KeyCode.S) && !isSucking && !isHoubaru)
        {
            isSucking = true;
            Debug.Log("S�L�[");
            //Debug.Log("S�L�[�������͂���");
            Suikomi_Count = 0;
            anim.SetInteger("suikomi", 1);
            audioSource.PlayOneShot(suikomiSound);
            SuikomiStarBlock();
            //StartCoroutine(SuckStarBlock());

        }
        else if (Input.GetKey(KeyCode.S) && isSucking && !isHoubaru)
        {
            Debug.Log("�z�����݌p��");
            SuikomiStarBlock();
            isSucking = true;
            Suikomi_Count++;
        }
        // �z�����݉�������
        else if (Input.GetKeyUp(KeyCode.S))
        {
            if (isSucking && !isHoubaru)
            {
                Debug.Log("�z�����݂�����");
                anim.SetInteger("suikomi", 0);

                // �z�����ݔ͈͓��̓G�̋z�����݃t���O������
                StopSuctionEnemies();
            }
            isSucking = false;
        }
        #endregion  ----------�y �z�����ݏ������� �z---------------------------------------------



        #region  ----------�y �f���o�� �z---------------------------------------------
        if (Input.GetKeyDown(KeyCode.V) && isHoubaru)
        {
            Shoot(isFacingRight);
            anim.SetInteger("suikomi", 0);
            isSucking = false;
            isHoubaru = false;
        }
        #endregion  ----------�y �f���o������ �z---------------------------------------------


        if (isTouchingDoor && Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log("�h�A�̑O�ŏ���L�[����");
            // �h�A�Ɋ֘A���鏈�������s
            SoundEffect.Instance.Door_Sound();
            Door_Method();
        }
    }
    #endregion --------------------------------------------------------------------------


    #region
    void StopSuctionEnemies()
    {
        Vector2 boxCenter = (Vector2)transform.position + (isFacingRight ? Vector2.right : Vector2.left) * 1f;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0f);

        foreach (Collider2D collider in colliders)
        {
            // �z���Ώۂ�"Enemy1"�iWanderingAI�����I�u�W�F�N�g�j�ł���ꍇ
            string[] SuikomiTags = { "Enemy1", "Enemy2", "StarBlock" };

            if (SuikomiTags.Contains(collider.tag))
            {
                {
                    Enemy_AI enemy = collider.GetComponent<Enemy_AI>();
                    if (enemy != null)
                    {
                        enemy.StopSuction(); // �z�����݉�����ʒm
                    }
                }
            }
        }
    }
    #endregion

    #region --------------------------�y�f���o���@���\�b�h�z------------------------------------
    void Shoot(bool isFacingRight)
    {

        if (isFacingRight)
        {
            Debug.Log("�V���b�g�I�I");
            audioSource.PlayOneShot(starSound);
            GameObject newBullet = Instantiate(cannonBall, shootPoint.position, Quaternion.identity) as GameObject;
            newBullet.GetComponent<Rigidbody2D>().AddForce(Vector3.right * power);
        } else
        {
            Debug.Log("�V���b�g�I�I�i�������j");
            audioSource.PlayOneShot(starSound);
            GameObject newBullet = Instantiate(cannonBall, shootPoint.position, Quaternion.identity) as GameObject;
            // �������ɗ͂�������
            newBullet.GetComponent<Rigidbody2D>().AddForce(Vector3.left * power);
            // �����e�̃X�v���C�g�𔽓]�������ꍇ
            Vector3 bulletScale = newBullet.transform.localScale;
            bulletScale.x *= -1; // ���E���]
            newBullet.transform.localScale = bulletScale;
        }

    }
    #endregion -------------------------------------------------------------------------------


    #region ----------------------------�y�z�����݁@���\�b�h�z----------------------------------------
    void SuikomiStarBlock()
    {
        Debug.Log("�z�����ݍU�������s�I�F " + Suikomi_Count + "��");

        // �z�����ݔ͈͂̒��S�_���v���C���[�̑O���ɐݒ�
        Vector2 boxCenter = (Vector2)transform.position + (isFacingRight ? Vector2.right : Vector2.left) * 1f;

        // �z�����ݔ͈͓��̃R���C�_�[���擾
        Collider2D[] colliders = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0f);

        bool objectFound = false;

        foreach (Collider2D collider in colliders)
        {
            // �z���Ώۂ�"Enemy1"�iWanderingAI�����I�u�W�F�N�g�j�ł���ꍇ
            string[] SuikomiTags = { "Enemy1", "Enemy2", "StarBlock" };

            if (SuikomiTags.Contains(collider.tag))
            {
                objectFound = true;

                Rigidbody2D targetRb = collider.GetComponent<Rigidbody2D>();
                if (targetRb != null)
                {
                    // �v���C���[�̈ʒu�Ɍ������ċz�����ޗ͂�������
                    Vector2 direction = (transform.position - collider.transform.position).normalized;

                    // �z���͂�NPC�ɓK�p
                    //targetRb.velocity = direction * suikomiForce;
                    OnTriggerEnter2D(collider);

                    // �v���C���[�Ƃ̋����𑪒�
                    float distance = Vector2.Distance(transform.position, collider.transform.position);
                    if (distance < closeDistance && Suikomi_Count > 10)
                    {
                        Debug.Log(collider.name + " ���z�����񂾁I");
                        isSucking = false;
                        anim.SetInteger("suikomi", 2);
                        isHoubaru = true;
                        Debug.Log("�z�����݊��� �ق��΂郂�[�h�ڍs " + anim.GetInteger("suikomi"));
                        Destroy(collider.gameObject); // NPC���폜
                    }
                }
            }
        }

        if (!objectFound)
        {
            Debug.Log("�͈͓���Enemy1��������܂���I");
        }
    }
    #endregion ---------------------------------------------------------------------------------------

    #region---- NPC����������z�����J�n�����
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy1"))
        {
            Enemy_AI enemy = other.GetComponent<Enemy_AI>();
            if (enemy != null)
            {
                enemy.StartSuction(transform,suikomiForce); // �v���C���[��Transform��n��
            }
        }
        else if(other.CompareTag("StarBlock"))
        {
            Block block = other.GetComponent<Block>();
            if (block != null)
            {
                block.StartSuction(transform, suikomiForce); // �v���C���[��Transform��n��
            }
        }

        if (other.CompareTag("Door"))
        {
            isTouchingDoor = true;
            // Door �N���X���擾
            Door door = other.GetComponent<Door>();
            if (door != null)
            {
                Door_Name = door.Scene_name; // Door �N���X�̃v���p�e�B���g�p
                Debug.Log("�h�A��Scene��"+ Door_Name);
            }
        }
    }
    #endregion ---------------------------------------------------------------------------------------

    #region
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Door"))
        {
            isTouchingDoor = false;
        }
    }
    #endregion

    public void Door_Method()
    {
        SceneManager.LoadScene(Door_Name);
    }

    #region --- �z�����ݍU���͈̔͂����o���i�f�o�b�O�p�j

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // �z�����ݔ͈͂̋�`�̒��S
        Vector2 boxCenter = (Vector2)transform.position + (isFacingRight ? Vector2.right : Vector2.left) * 1.9f;

        // ��`�͈̔͂�`��
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }
    #endregion ------------------------------------
    //----------------------------------------------------------------------------------//

    #region //-----------------------------�y�n�ʂ̓����蔻��z----------------------------------------------//
    // �n�ʂɐڐG�����Ƃ��̏���
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
            Debug.Log("�n�ʂɒ��n���܂����B");
            anim.SetInteger("Jump", 0);
            rb.gravityScale = normalGravityScale; 
        }
    }

    // �n�ʂ��痣�ꂽ�Ƃ��̏���
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = false;
            Debug.Log("�n�ʂ��痣��܂����B");
        }
    }
    #endregion //---------------------------------------------------------------------------//

}
