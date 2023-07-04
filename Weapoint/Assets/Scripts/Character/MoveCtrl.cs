using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveCtrl : MonoBehaviour
{
    [Header("Effect Sound")]
    [SerializeField]
    private AudioClip swordAttackSound;
    [SerializeField]
    private AudioClip swordSkillSound;
    [SerializeField]
    private AudioClip daggerSkillSound;
    [SerializeField]
    private AudioClip chargeAxeSound;
    [SerializeField]
    private AudioClip endChargeAxeSound;
    [SerializeField]
    private AudioClip hitAxeSound;
    [SerializeField]
    private AudioClip bowAttackSound;
    [SerializeField]
    private AudioClip backStepSound;

    [Header("Animation")]
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private AnimationClip[] animations;

    [Header("Weapon")]
    [SerializeField]
    private SpriteRenderer WeaponRenderer;
    [SerializeField]
    private GameObject arrowPrefab;
    private float arrowSpawnOffset = 1.0f;

    private Rigidbody2D rb;
    private bool isFacingRight = false;
    private bool isJumping = false;
    private bool CanAttack = true;
    private bool CanSkill = true;
    private bool CanGetDamage = true;

    private float AttackCool;

    [Header("Player Stat")]
    [SerializeField]
    private string AttackType = "Sword";
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float attackSpeed = 2.5f;
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private float SkillCool = 5f;
    [SerializeField]
    private float maxHp = 100f;
    [SerializeField]
    private float currentHp = 100f;
    [SerializeField]
    private float power = 10f;
    [SerializeField]
    private int attackRange = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

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
            case "Axe":
                animator.SetTrigger("NormalAttack");
                AudioSource.PlayClipAtPoint(swordAttackSound, transform.position);
                break;
            case "Bow":
                animator.SetTrigger("BowAttack");
                StartCoroutine("BowTerm");
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
                StartCoroutine("backStep");

                break;
            case "Axe":
                ChargeAtk();
                break;
        }
    }
    
    IEnumerator backStep()
    {
        float stepDuration = 0.2f;
        float stepSpeed = 10f;
        animator.SetTrigger("BowSkill");
        AudioSource.PlayClipAtPoint(backStepSound,transform.position);
        Vector3 stepDirection = transform.right + transform.up * 0.6f ;
        float stepStartTime = Time.time;
        bool isShoot = false;
        while (Time.time - stepStartTime < stepDuration)
        {
            // 플레이어를 일정한 속도로 이동시킴
            if(!isShoot && Time.time - stepStartTime > 0.1f)
            {
                shootArrow(arrowSpawnOffset - 0.4f);
                shootArrow(arrowSpawnOffset - 0.2f);
                isShoot = true;
            }
            transform.position += stepDirection * stepSpeed * Time.deltaTime;
            yield return null;
        }

    }
    private void shootArrow(float arrowOffset)
    {
        Vector3 spawnPosition = transform.position + (transform.up*(arrowOffset-0.3f))+(transform.right * -1);
        GameObject arrowObject = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity);
        Arrow arrow = arrowObject.GetComponent<Arrow>();

        // 플레이어가 바라보는 방향에 따라 화살이 뒤집히도록 설정
        if (isFacingRight)
        {
            arrow.Flip();
        }
    }
    IEnumerator BowTerm()
    {
        yield return new WaitForSeconds(0.35f);
        AudioSource.PlayClipAtPoint(bowAttackSound, transform.position);
        shootArrow(arrowSpawnOffset);
    }
    Color curColor;
    private void ChargeAtk()
    {

        animator.SetTrigger("AxeCharge");
        curColor = WeaponRenderer.color;
        StartCoroutine("ChargeAtkCoroutine");
    }
    private IEnumerator ChargeAtkCoroutine()
    {
        float tmp1 = moveSpeed;
        float tmp2 = jumpForce;
        moveSpeed = 0f;
        jumpForce = 0f;
        float chargeValue = 1.0f;
        float WaitTime = 0.0f;
        bool chargeEnd = false;
        CanGetDamage = false;
        AudioSource.PlayClipAtPoint(chargeAxeSound, transform.position);
        while (!Input.GetKeyUp(KeyCode.LeftShift))
        {
            if (WaitTime > 1.5f)
            {
                break;
            }
            if(chargeValue > 0.0f)
            {
 
                chargeValue -= Time.deltaTime;
                ChangeAxeColor(new Color(curColor.r, chargeValue, chargeValue));
            }
            else if(chargeValue<=0.0f && !chargeEnd)
            {
                AudioSource.PlayClipAtPoint(endChargeAxeSound, transform.position);
                chargeEnd = true;
            }
            WaitTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        moveSpeed = tmp1;
        jumpForce = tmp2;
        StartCoroutine("AxeSkill");


    }
    IEnumerator AxeSkill()
    {
        animator.SetTrigger("AxeSkill");
        yield return new WaitForSeconds(0.2f);
        AudioSource.PlayClipAtPoint(hitAxeSound, transform.position);
        yield return new WaitForSeconds(0.15f);
        CanGetDamage = true;
        ChangeAxeColor(curColor);
    }
    private void ChangeAxeColor(Color color)
    {
        WeaponRenderer.color = color;
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
        CanGetDamage = false;
        // 플레이어를 일정한 속도로 돌진하도록 설정
        Vector3 dashDirection = transform.right * -1f;
        float dashStartTime = Time.time;
        while (Time.time - dashStartTime < dashDuration)
        {
            // 플레이어를 일정한 속도로 이동시킴
            transform.position += dashDirection * dashSpeed * Time.deltaTime;
            yield return null;
        }
        CanGetDamage = true;
        isDashing = false;
    }

    IEnumerator SkillCooltime()
    {
        GetComponent<SkillCooltime>().setCooltime(SkillCool);
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
    
    public void SetStat(int[] points)
    {
        maxHp = 20 + points[0];
        moveSpeed = 2.5f + 0.05f * points[1];
        power = 10f * (1f+(points[2] / 100f));
        attackRange = points[3] / 10;
        attackSpeed = 2.5f + 0.05f * points[4];
        currentHp = maxHp;
        AttackType = "Sword";
        if (points[0] >= 60)
        {

        }
        else if (points[1] >= 60)
        {
            
        }
        else if (points[2] >= 60)
        {
            AttackType = "Axe";
            SkillCool = 6f;
        }
        else if (points[3] >= 60)
        {
            AttackType = "Bow";
            SkillCool = 4f;
        }
        else if (points[4] >= 60)
        {
            AttackType = "Dagger";
            SkillCool = 3f;
        }
        animator.SetFloat("MovSpeed", moveSpeed / 5f);
        animator.SetFloat("atkSpeed", attackSpeed / 5f);

    }
    public float getMaxHp()
    {
        return maxHp;
    }
    public float getCurrentHp()
    {
        return currentHp;
    }
}