using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCtrl : MonoBehaviour
{
    [SerializeField]
    private AudioClip swordAttackSound;
    [SerializeField]
    private AudioClip swordSkillSound;
    [SerializeField]
    private AudioClip daggerSkillSound;
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private AnimationClip[] animations;
    private Rigidbody2D rb;
    private bool isFacingRight = false;
    private bool isJumping = false;
    private bool CanAttack = true;
    private bool CanSkill = true;
    private bool CanGetDamage = true;
    private string AttackType = "Sword";
    private float AttackCool;

    public float moveSpeed = 5f;
    public float attackSpeed = 2.5f;
    public float jumpForce = 5f;
    public float SkillCool = 5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator.SetFloat("MovSpeed", moveSpeed / 5f);
        animator.SetFloat("atkSpeed", attackSpeed / 5f);
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        bool isMoving = Mathf.Abs(moveX) > 0.01f;
        if (isMoving)
        {
            animator.SetBool("IsRunning", true);
            rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);
        }
        else
        {
            animator.SetBool("IsRunning", false);
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        if ((isFacingRight && moveX < 0) || (!isFacingRight && moveX > 0))
        {
            Flip();
        }

        if (Input.GetKeyDown(KeyCode.W) && !isJumping)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isJumping = true;
        }
        if (Input.GetKeyDown(KeyCode.Space) && CanAttack)
        {
            StartCoroutine("AttackCooltime");
            Attack();

        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && CanSkill)
        {
            StartCoroutine("SkillCooltime");
            Skill();
        }
    }

    private void Attack()
    {
        switch (AttackType)
        {
            case "Sword":
                animator.SetTrigger("NormalAttack");
                AudioSource.PlayClipAtPoint(swordAttackSound, transform.position);
                break;
            case "Dagger":
                animator.SetTrigger("NormalAttack");
                AudioSource.PlayClipAtPoint(swordAttackSound, transform.position);
                break;

        }
    }
    private void Skill()
    {
        switch (AttackType)
        {
            case "Sword":
                animator.SetTrigger("NormalSkill");
                AudioSource.PlayClipAtPoint(swordSkillSound, transform.position);
                break;
            case "Dagger":
                Dash();
                AudioSource.PlayClipAtPoint(daggerSkillSound, transform.position);
                break;
            case "Bow":
                break;
        }
    }
    private bool isDashing = false;
    private float dashSpeed = 50f;
    private float dashDuration = 0.1f;

    private void Dash()
    {
        if (!isDashing)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    private IEnumerator DashCoroutine()
    {
        isDashing = true;

        // 플레이어를 일정한 속도로 돌진하도록 설정
        Vector3 dashDirection = transform.right * -1f;
        float dashStartTime = Time.time;
        Debug.Log(transform.right);
        while (Time.time - dashStartTime < dashDuration)
        {
            // 플레이어를 일정한 속도로 이동시킴
            transform.position += dashDirection * dashSpeed * Time.deltaTime;
            yield return null;
        }

        isDashing = false;
    }

    IEnumerator SkillCooltime()
    {
        CanSkill = false;
        yield return new WaitForSeconds(SkillCool);
        CanSkill = true;
    }
    IEnumerator AttackCooltime()
    {
        CanAttack = false;
        yield return new WaitForSeconds(0.01f);
        AttackCool = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(AttackCool);
        CanAttack = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}