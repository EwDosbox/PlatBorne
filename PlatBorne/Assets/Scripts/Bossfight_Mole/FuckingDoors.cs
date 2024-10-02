using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuckingDoors : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            CameraMoleScript cam = FindAnyObjectByType<CameraMoleScript>();
            Collider2D collider2D = GetComponent<Collider2D>();
            collider2D.isTrigger = false;
            cam.ChangeCamPosition();
        }
    }
}
