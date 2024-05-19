using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole_WeakSpot : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool backpackOn = false;
    [SerializeField] Mole_Health health;
    public bool BackpackOn
    { set { backpackOn = value; } }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && backpackOn)
        {
            health.BossHit();
        }
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
}
