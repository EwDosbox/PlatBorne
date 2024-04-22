using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIFadeOutEffect : MonoBehaviour
{
    public CanvasGroup uiCanvas;
    float fadeOutElapsedTime = 0;
    public float fadeTime = 5;
    bool fadeOut = false;
    public bool FadeOut
    {
        set { fadeOut = value; }
    }
    void Update()
    {
        if (fadeOut)
        {
            if (uiCanvas.alpha > 0f)
            {
                float currentAlpha = Mathf.Lerp(1, 0, fadeOutElapsedTime / fadeTime);
                uiCanvas.alpha = currentAlpha;
                fadeOutElapsedTime += Time.deltaTime;
            }
            else
            {
                uiCanvas.alpha = 0f;
                fadeOut = false; //Optimalization
            }
        }        
    }
}
