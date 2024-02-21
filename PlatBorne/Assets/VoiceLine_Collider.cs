using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceLine_Collider : MonoBehaviour
{
    public PlayerScript playerScript;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fall Hitbox"))
        {
            playerScript.touchedFallHitbox = true;
        }
    }
}
