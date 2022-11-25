using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;
using UnityEngine.UI;
public class Hero : Entity 
{
    [SerializeField] private float speed = 3f; // ñêîðîñòü äâèæåíèÿ
    [SerializeField] private int health; // ñêîðîñòü äâèæåíèÿ
    [SerializeField] private float jumpForce = 8; // ñèëà ïðûæêà
    private bool isGrounded = false;

    [SerializeField] private Image[] hearts;

    [SerializeField] private Sprite aliveHeart;
    [SerializeField] private Sprite deadHeart; 
 
    public bool isAttacking1 = false;
    public bool isRecharged1 = true;

    public bool isAttacking2 = false;
    public bool isRecharged2 = true;

    public Transform attackPos;
    public float attackRange;
    public LayerMask enemy;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    private States State
    {
        get { return (States)anim.GetInteger("state"); }
        set { anim.SetInteger("state", (int)value); }
    } 

    public static Hero Instance { get; set; }

    private void Awake()
    {
        lives = 5;
        health = lives;
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        isRecharged1 = true;
        isRecharged2 = true;
    }

    private void FixedUpdate()
    {
        CheckGround();
    }
    int counter = 0;
    private void Update()
    {
        if (!isAttacking1 && !isAttacking2 && isGrounded && State != States.die)
            State = States.idle;
        if (!isAttacking1 && !isAttacking2 && Input.GetButton("Horizontal") && State != States.die)
            Walk();
        if (!isAttacking1 && !isAttacking2 && Input.GetButton("Horizontal") && Input.GetButton("Horizontal1") && State != States.die)
            Run();
        if (!isAttacking1 && !isAttacking2 && isGrounded && Input.GetButtonDown("Jump") && State != States.die)
            Jump();
        if (Input.GetButtonDown("Fire1") && !isAttacking2)
            Attack1();
        if (Input.GetButtonDown("Fire2") && !isAttacking1 && State != States.die)
            Attack2();

        if (!(health <= lives))
        {
            health = lives;
        }
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
                hearts[i].sprite = aliveHeart;
            else
                hearts[i].sprite = deadHeart;

            if (i < lives)
                hearts[i].enabled = true;
            else
                hearts[i].enabled = false;
        }
        if (health < 1)
        {
            if (counter == 0)
            {
                counter++;
                State = States.death;
            }
            else
                State = States.die;
        }
    }

    private IEnumerator AttackAnimation1()
    {
        yield return new WaitForSeconds(1.1f);
        isAttacking1 = false;
    }
    private IEnumerator AttackCoolDown1()
    {
        yield return new WaitForSeconds(1.2f);
        isRecharged1 = true;
    }

    private IEnumerator AttackAnimation2()
    {
        yield return new WaitForSeconds(0.45f);
        isAttacking2 = false;
    }
    private IEnumerator AttackCoolDown2()
    {
        yield return new WaitForSeconds(0.6f);
        isRecharged2 = true;
    }

    public void Attack1()
    {
        if (isGrounded && isRecharged1)
        {
            State = States.attack1;
            isAttacking1 = true;
            isRecharged1 = false;

            StartCoroutine(AttackAnimation1());
            StartCoroutine(AttackCoolDown1());
        }
    }
    public void Attack2()
    {
        if (isGrounded && isRecharged2)
        {
            State = States.attack2;
            isAttacking2 = true;
            isRecharged2 = false;

            StartCoroutine(AttackAnimation2());
            StartCoroutine(AttackCoolDown2());
        }
    }
    public void OnAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemy);

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].GetComponent<Entity>().GetDamage();
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
    private void Walk()
    {
        if (isGrounded) State = States.walk;

        Vector3 dir = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
        sprite.flipX = dir.x < 0.0f;
    }
    private void Run()
    {
        if (isGrounded) State = States.run;

        Vector3 dir = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, (speed) * Time.deltaTime);
        sprite.flipX = dir.x < 0.0f;
    }
    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }
    private void CheckGround()
    {
        if (!isGrounded) State = States.jump;

        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        isGrounded = collider.Length > 1;
    }
    public override void GetDamage()
    {
        health -= 1;
      
    }
}

public enum States
{
    idle,
    walk,
    jump,
    attack1,
    attack2,
    run,
    death,
    die
}
