using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TEMPORARY : MonoBehaviour
{
    public AudioSource SFXTypewriter;
    public CanvasGroup canvasGroup;
    public Text mainText;
    private bool isFinished = false;
    void Start()
    {
        StartCoroutine(MainTextAndShit());
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("Cutscene_Credits");
        }
    }
    IEnumerator MainTextAndShit()
    {
        StartCoroutine(TypeWriter("Mole Bossfight is currently under development...", SFXTypewriter, 0.1f, 2, 0, mainText));
        while (!isFinished) yield return null;
        isFinished = false;
        StartCoroutine(TypeWriter("Things to be added in Platborne v2.1", SFXTypewriter, 0.1f, 2, 0, mainText));
        while (!isFinished) yield return null;
        isFinished = false;
        StartCoroutine(TypeWriter("-Changed OST for Brecus Bossfight (OG: Astron [YT])", SFXTypewriter, 0.1f, 1, 0, mainText));
        while (!isFinished) yield return null;
        isFinished = false;
        StartCoroutine(TypeWriter("-Mole Bossfight", SFXTypewriter, 0.1f, 1, 0, mainText));
        while (!isFinished) yield return null;
        isFinished = false;
        StartCoroutine(TypeWriter("-SFX Rework", SFXTypewriter, 0.1f, 1, 0, mainText));
        while (!isFinished) yield return null;
        isFinished = false;
        StartCoroutine(TypeWriter("-Better Teleport Console", SFXTypewriter, 0.1f, 1, 0, mainText));
        while (!isFinished) yield return null;
        isFinished = false;
        StartCoroutine(TypeWriter("And More...", SFXTypewriter, 0.1f, 1, 0, mainText));
        while (!isFinished) yield return null;
        isFinished = false;
        yield return new WaitForSeconds(2);
        StartCoroutine(FadeOutCanvas(canvasGroup,1));
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Cutscene_Credits");
    }

    IEnumerator TypeWriter(string text, AudioSource soundEffect, float timeBtwChars, float delayStart, float delayEnd, Text UIText)
    {
        yield return new WaitForSeconds(delayStart);
        soundEffect.Play();
        foreach (char c in text)
        {
            UIText.text += c;
            yield return new WaitForSeconds(timeBtwChars);
        }
        soundEffect.Stop();
        yield return new WaitForSeconds(delayEnd);
        isFinished = true;
    }

    IEnumerator FadeOutCanvas(CanvasGroup fadeshit, float time)
    {
        float elapsedTime = 0f;
        float startAlpha = fadeshit.alpha;

        while (elapsedTime < time)
        {
            float newAlpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / time);
            fadeshit.alpha = newAlpha;
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        canvasGroup.alpha = 0f;
    }
}
