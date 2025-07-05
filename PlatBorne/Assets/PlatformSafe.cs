using System.Collections;
using UnityEngine;

public class PlatformSafe : MonoBehaviour
{
    BoxCollider2D collider;
    void OnEnable()
    {
        collider = GetComponent<BoxCollider2D>();
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerScript input = collision.GetComponent<PlayerScript>();
        if (input != null) input.MovePlayer(0, 1, true);
    }
}
