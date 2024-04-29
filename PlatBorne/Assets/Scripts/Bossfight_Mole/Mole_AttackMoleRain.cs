using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole_AttackMoleRain : MonoBehaviour
{
    private Rigidbody2D rb;
    private GameObject player;
    [SerializeField] private float timeToSelfDestruct;
    [SerializeField] private float acceleration;
    [SerializeField] private float maxSpeed;

    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) Destroy(gameObject);
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeToSelfDestruct)
        {
            Destroy(gameObject);
        }
        rb = GetComponent<Rigidbody2D>();
        if (rb.velocity.magnitude < maxSpeed) rb.velocity += Vector2.down * acceleration;
        else rb.velocity = Vector2.down * maxSpeed;
    }
}

