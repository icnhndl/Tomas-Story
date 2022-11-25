using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingMonster : Entity
{
    private double speed = 0.1f;
    private Vector3 direection; 
    private SpriteRenderer sprite;
    private Rigidbody2D rb;

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        lives = 10;
        direction = transform.right;
    }

    private void Update()
    {
        Move();
    }
    private void Move()
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.1f + transform.right * direction.x * 0.7f, 0.1f);
        if (colliders.Length > 0) direction *= -1;

        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, Time.deltaTime);
        sprite.flipX = direction.x > 0.0f;



    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Hero.Instance.gameObject)
        {
            Hero.Instance.GetDamage();
        } 
    }
}

