using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DashOrbScript : MonoBehaviour
{
    public GameObject dashOrb;
    public PlayerScript player;
    public GameObject levelMove;
    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if (collision.gameObject.CompareTag("Player"))
        {
            player.CanDash = true;
            levelMove.SetActive(true);
            Destroy(dashOrb);
        }
    }
}
