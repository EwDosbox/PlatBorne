using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class DashOrb : MonoBehaviour
{
    public PlayerInputScript player;
    public Bossfight brecus;
    public GameObject moveNextLevel;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.abilityToDash = true;
            moveNextLevel.SetActive(true);
            Destroy(gameObject);
        }
    }
}
