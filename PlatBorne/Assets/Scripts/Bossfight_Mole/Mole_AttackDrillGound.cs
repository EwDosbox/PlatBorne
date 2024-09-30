using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole_AttackDrillGround : MonoBehaviour
{
    Rigidbody2D rb;
    float timer;
    [SerializeField] float timeToSelfDestruct;
    [SerializeField] float speed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth ph = FindAnyObjectByType<PlayerHealth>();
            ph.PlayerDamage();
            Destroy(gameObject);
        }
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.rotation = Quaternion.Euler(0,0,0);        
        rb.velocity = Vector2.up * speed;
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