using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsScript : MonoBehaviour
{
    [SerializeField] GameObject controlsCanvas;

    private void OnTriggerEnter2D(Collider2D other)
    {
        controlsCanvas.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        controlsCanvas.SetActive(false);
    }
}
