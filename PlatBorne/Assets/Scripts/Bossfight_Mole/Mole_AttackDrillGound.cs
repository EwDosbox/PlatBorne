using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole_AttackDrillGround : MonoBehaviour
{
    Rigidbody2D rb;
    float timer;
    [SerializeField] float timeToSelfDestruct;
    [SerializeField] float speed;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") Destroy(gameObject);
    }
    void Awake()
    {
        transform.rotation = Quaternion.Euler(0,0,0);
        rb = GetComponent<Rigidbody2D>();
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