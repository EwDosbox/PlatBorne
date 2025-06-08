using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole_AttackDrillRain : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float speed;
    [SerializeField] AudioSource sfx;
    [SerializeField] float maxDistance = 15f; // max distance at which sound is audible
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
        rb.velocity = Vector2.down * speed;
    }

    void Update()
    {
        if (transform.position.y < -11f) Destroy(gameObject);
        //change volume        
        float distance = Vector2.Distance(transform.position, player.transform.position);
        float rawRatio = 1f - (distance / maxDistance);
        float volume = Mathf.Clamp(rawRatio * 0.1f, 0f, 0.1f);
        sfx.volume = volume;
    }
}