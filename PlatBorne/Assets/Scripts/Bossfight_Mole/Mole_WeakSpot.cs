using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole_WeakSpot : MonoBehaviour
{
    //Components
    Rigidbody2D rb;
    Mole_Bossfight bossfight;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {            
            bossfight.BossHitBeforeCharge();
            this.enabled = false;
        }
    }

    private void Awake()
    {
        bossfight = GetComponentInParent<Mole_Bossfight>();
        rb = GetComponent<Rigidbody2D>();
    }
}
