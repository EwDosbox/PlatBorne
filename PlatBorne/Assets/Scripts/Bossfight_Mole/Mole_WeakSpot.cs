using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole_WeakSpot : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool backpackOn = false;
    public bool BackpackOn
    { set { backpackOn = value; } }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Mole_Health moleHealth = new Mole_Health();
        if (collision.gameObject.CompareTag("Player") && backpackOn)
        {
            moleHealth.BossHit();
        }
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
}
