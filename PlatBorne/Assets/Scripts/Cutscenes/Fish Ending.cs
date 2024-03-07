using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class FishEnding : MonoBehaviour
{
    public AudioSource SFXTypewriter;
    public AudioSource music;
    public CanvasGroup canvasGroup;
    public Text mainText;
    public GameObject endingText;
    public CanvasGroup canvasGroupEnding;
    public CanvasGroup canvasHunterFish;
    private bool isFinished = false;
    void Start()
    {
        endingText.SetActive(false);
        music.Play();  
        StartCoroutine(MainTextAndShit());
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("Cutscene_EndGameAct2");
        }
    }
    IEnumerator MainTextAndShit()
    {
        StartCoroutine(TypeWriter("The lake is comforting. You stay and fall to slumber.",SFXTypewriter, 0.1f, 2, 0, mainText));
        while (!isFinished) yield return null;
        isFinished = false;
        StartCoroutine(TypeWriter("\nDreams of an easy life occupy your stressed mind.", SFXTypewriter, 0.1f, 2, 0, mainText));
        while (!isFinished) yield return null;
        isFinished = false;
        StartCoroutine(TypeWriter("\nThe Sanguine Moon no longer existing.", SFXTypewriter, 0.1f, 1, 0, mainText));
        while (!isFinished) yield return null;
        isFinished = false;
        StartCoroutine(TypeWriter("\nNo God nor authority over You...", SFXTypewriter, 0.1f, 1, 0, mainText));
        while (!isFinished) yield return null;
        isFinished = false;
        StartCoroutine(TypeWriter("\n\nYou wake up...Only water surrounds your changed body.", SFXTypewriter, 0.1f, 2, 0, mainText));
        while (!isFinished) yield return null;
        isFinished = false;
        StartCoroutine(TypeWriter("\nAll the fish swimming beside you nonchalantly.", SFXTypewriter, 0.1f, 1, 0, mainText));
        while (!isFinished) yield return null;
        isFinished = false;
        StartCoroutine(FadeInCanvas(canvasHunterFish, 10));
        StartCoroutine(TypeWriter("\n\"Have I become a fish?\"", SFXTypewriter, 0.1f, 1, 0, mainText));
        while (!isFinished) yield return null;
        isFinished = false;
        StartCoroutine(TypeWriter("\n\nYour fate is accepted shockingly fast.", SFXTypewriter, 0.1f, 2, 0, mainText));
        while (!isFinished) yield return null;
        isFinished = false;
        StartCoroutine(TypeWriter("\nYour prayers were heard not by the Moon, by someone good.", SFXTypewriter, 0.1f, 1, 0, mainText));
        while (!isFinished) yield return null;
        isFinished = false;
        StartCoroutine(TypeWriter("\nThe ever-burning hatred or the need to kill any monster has disperses...", SFXTypewriter, 0.1f, 1, 0, mainText));
        while (!isFinished) yield return null;
        isFinished = false;
        StartCoroutine(TypeWriter("\n\nJoy fills the void in You as you swim away with your new friends...", SFXTypewriter, 0.1f, 2, 1, mainText));
        while (!isFinished) yield return null;
        StartCoroutine(FadeOutCanvas(canvasGroup,1));
        yield return new WaitForSeconds(2);
        endingText.SetActive(true);
        yield return new WaitForSeconds(2);
        StartCoroutine(FadeOutCanvas(canvasGroupEnding, 1));
        StartCoroutine(FadeOutCanvas(canvasHunterFish, 1));
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Cutscene_EndGameAct2");
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

    IEnumerator FadeInCanvas(CanvasGroup fadeshit, float time)
    {
        float elapsedTime = 0f;
        float startAlpha = 0;

        while (elapsedTime < time)
        {
            float newAlpha = Mathf.Lerp(startAlpha, 1, elapsedTime / time);
            fadeshit.alpha = newAlpha;
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        canvasGroup.alpha = 1f;
    }
}
