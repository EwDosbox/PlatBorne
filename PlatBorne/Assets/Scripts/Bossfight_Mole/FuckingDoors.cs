using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FuckingDoors : MonoBehaviour
{
    bool playerHasMoved = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!playerHasMoved)
        {
            if (collision.tag == "Player")
            {
                playerHasMoved = true;
                if (SceneManager.GetActiveScene().name == "LevelMole")
                {
                    CameraMoleScript cam = FindAnyObjectByType<CameraMoleScript>();
                    Collider2D collider2D = GetComponent<Collider2D>();
                    collider2D.isTrigger = false;
                    cam.ChangeCamPosition();
                }
                PlayerScript playerScript = FindAnyObjectByType<PlayerScript>();
                playerScript.MovePlayer(2f, 0f);
            }
        }
    }
}
