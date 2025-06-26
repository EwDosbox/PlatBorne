using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole_AttackDrillRain : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float acceleration;
    [SerializeField] float maxSpeed;
    [SerializeField] AudioSource sfx;
    [SerializeField] float maxDistance = 15f; // max distance at which sound is audible
    float currentSpeed = 1;
    float timer = 0;
    float timeToSpeed = 0.1f;
    GameObject player;

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
        player = FindAnyObjectByType<PlayerScript>().gameObject;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        rb.velocity = Vector2.down * currentSpeed;
    }

    void Update()
    {
        if (transform.position.y < -30f) Destroy(gameObject);
        //speed
        if (currentSpeed < maxSpeed)
        {
            timer += Time.deltaTime;
            if (timer > timeToSpeed) //the speed updates 10x per sec (instead of every frame)
            {
                timer -= timeToSpeed;
                currentSpeed += acceleration;
                rb.velocity += Vector2.down * currentSpeed;
            }            
        }
        //change volume        
        float distance = Vector2.Distance(transform.position, player.transform.position);
        float rawRatio = 1f - (distance / maxDistance);
        float volume = Mathf.Clamp(rawRatio * 0.1f, 0f, 0.1f);
        sfx.volume = volume;
    }
}