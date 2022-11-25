using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private Transform hero;
    private Vector3 pos;

    private void Awake()
    {
        if (hero==false)
        {
        if(hero)
            hero = hero;
        else 
            hero = hero;
            hero = FindObjectOfType<Hero>().transform;
        }
    }
    private void Update()
    {
        pos = hero.position;
        pos.z = -10f;
        pos.y += 3f; 
        
        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime);

    }
}
