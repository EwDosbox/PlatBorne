using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole_AttackGroundFuckingSomething : MonoBehaviour
{
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth ph = FindAnyObjectByType<PlayerHealth>();
            ph.PlayerDamage();
            Destroy(transform.parent.gameObject);
        }
    }

    public void OnAnimationFinished()
    {
        Destroy(transform.parent.gameObject);
    }
}

