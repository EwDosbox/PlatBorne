using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoodEnding : MonoBehaviour
{
    public AudioSource SFXTypewriter;
    public AudioSource music;
    public AudioSource endingSFX;
    public CanvasGroup canvasGroup;
    public Text mainText;
    public GameObject endingText;
    public CanvasGroup canvasGroupEnding;
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
        StartCoroutine(TypeWriter("The everlasting slaughterer lies on the cold moist ground, you feel it...", SFXTypewriter, 0.1f, 2, 0, mainText));
        while (!isFinished) yield return null;
        isFinished = false;
        StartCoroutine(TypeWriter("\nThe ever-escaping euphoria of another day, for once it stayed here.", SFXTypewriter, 0.1f, 1, 0, mainText));
        while (!isFinished) yield return null;
        isFinished = false;
        StartCoroutine(TypeWriter("\nYou achieved the unachievable..", SFXTypewriter, 0.1f, 1, 0, mainText));
        while (!isFinished) yield return null;
        isFinished = false;
        StartCoroutine(TypeWriter("\n\nI did it, The dawn is nigh!\"", SFXTypewriter, 0.1f, 2, 0, mainText));
        while (!isFinished) yield return null;
        isFinished = false;
        StartCoroutine(TypeWriter("\nThe Moon is cleanses itself off the horrible, appalling, cruel crimson colour.", SFXTypewriter, 0.1f, 1, 0, mainText));
        while (!isFinished) yield return null;
        isFinished = false;
        StartCoroutine(TypeWriter("\nIndescribable joy, your heart is about to rupture. It is here.", SFXTypewriter, 0.1f, 1, 0, mainText));
        while (!isFinished) yield return null;
        isFinished = false;
        StartCoroutine(TypeWriter("\n\n\"Will I be the one to free it?\"", SFXTypewriter, 0.1f, 1, 0, mainText));
        while (!isFinished) yield return null;
        isFinished = false;
        StartCoroutine(TypeWriter("\nThe quest of cleansing The Great Kingdoms of Britain has been achieved by the Foolish Hoonter.", SFXTypewriter, 0.1f, 2, 0, mainText));
        while (!isFinished) yield return null;
        isFinished = false;
        yield return new WaitForSeconds(2);
        StartCoroutine(FadeOutCanvas(canvasGroup, 1));
        yield return new WaitForSeconds(2);
        endingText.SetActive(true);
        endingSFX.Play();
        yield return new WaitForSeconds(3);
        StartCoroutine(FadeOutCanvas(canvasGroupEnding, 1));
        yield return new WaitForSeconds(2);
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
}
