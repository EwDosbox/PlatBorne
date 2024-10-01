using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole_AttackGroundFuckingSomething : MonoBehaviour
{
    Rigidbody2D rb;
    float timer = 0;
    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();    
        animator = GetComponent<Animator>();
        string godWhyUnity = PlayerPrefs.GetString("rockAttack");
        if (godWhyUnity == "left") animator.SetTrigger("rockLeft");
        else if (godWhyUnity == "right") animator.SetTrigger("rockRight");
        else animator.SetTrigger("rockMiddle");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth ph = FindAnyObjectByType<PlayerHealth>();
            ph.PlayerDamage();
            Destroy(gameObject);
        }
    }

   
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 3)
        {
            Destroy(gameObject);
        }
    }
}

