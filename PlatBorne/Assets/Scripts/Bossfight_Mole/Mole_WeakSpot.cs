using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole_WeakSpot : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool backpackOn = false;
    [SerializeField] Mole_Health health;
    [SerializeField] Mole_Bossfight bossfight;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            health.BossHit();
            bossfight.BossHitWhileCharge();
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
}
