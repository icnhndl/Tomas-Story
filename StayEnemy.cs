using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayEnemy : Entity
{
    private void Start()
    {
        lives = 1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject == Hero.Instance.gameObject)
        {
            Hero.Instance.GetDamage();
            lives--;
            Debug.Log("у червя" + lives);
        }

        if (lives < 1)
            Die();
    }
}
