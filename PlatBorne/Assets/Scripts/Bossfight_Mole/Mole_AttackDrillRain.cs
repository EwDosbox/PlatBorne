using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole_AttackDrillRain : MonoBehaviour
{
    Rigidbody2D rb;
    float timer;
    [SerializeField] float timeToSelfDestruct;
    [SerializeField] float speed;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.down * speed;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeToSelfDestruct)
        {
            Destroy(gameObject);
        }
    }
}