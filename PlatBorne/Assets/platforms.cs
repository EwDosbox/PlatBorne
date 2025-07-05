using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platforms : MonoBehaviour
{
    PlayerScript player;
    CompositeCollider2D collider2D;
    void Awake()
    {
        collider2D = GetComponent<CompositeCollider2D>();
        StartCoroutine(Timer());             
    }

    IEnumerator Timer()
    {
        collider2D.isTrigger = true;
        yield return new WaitForSeconds(1);
        collider2D.isTrigger = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = FindFirstObjectByType<PlayerScript>();
        player.MovePlayer(0, 2);
    }
}
