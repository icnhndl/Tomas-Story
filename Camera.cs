using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private Transform hero;
    private Vector3 position;

    private void Awake()
    {
        if (hero==false)
        {
            hero = FindObjectOfType<Hero>().transform;
        }
    }
    private void Update()
    {
        pos = hero.position;
        position.z = -10f;
        position.y += 3f; 
        
        transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime);

    }
}
