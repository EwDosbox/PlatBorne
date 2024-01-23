using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsScript : MonoBehaviour
{
    [SerializeField] GameObject controlsCanvas;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hunter is inside the collider");
        controlsCanvas.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Hunter is outside the collider");
        controlsCanvas.SetActive(false);
    }
}
