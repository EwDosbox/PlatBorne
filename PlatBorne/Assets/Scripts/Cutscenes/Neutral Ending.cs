using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NeutralEnding : MonoBehaviour
{
    public AudioSource SFXTypewriter;
    public AudioSource music;
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
        StartCoroutine(TypeWriter("The everlasting slaughterer lies on the cold moist ground, yet you feel...", SFXTypewriter, 0.1f, 2, 0, mainText));
        while (!isFinished) yield return null;
        isFinished = false;
        StartCoroutine(TypeWriter("\nNothing. Void still fills your heart and soul...", SFXTypewriter, 0.1f, 1, 0, mainText));
        while (!isFinished) yield return null;
        isFinished = false;
        StartCoroutine(TypeWriter("\n\"Where is the bliss, the delight of the new Moon?\"", SFXTypewriter, 0.1f, 2, 0, mainText));
        while (!isFinished) yield return null;
        isFinished = false;
        StartCoroutine(TypeWriter("\nThere is none.", SFXTypewriter, 0.1f, 1, 0, mainText));
        while (!isFinished) yield return null;
        isFinished = false;
        StartCoroutine(TypeWriter("\n\nThe moon still shines with its flaming red abhorrent colour", SFXTypewriter, 0.1f, 1, 0, mainText));
        while (!isFinished) yield return null;
        isFinished = false;
        StartCoroutine(TypeWriter("\n\"It's... still bloody? Do I have to kill more? What is there left for me to kill?\"", SFXTypewriter, 0.1f, 2, 0, mainText));
        while (!isFinished) yield return null;
        isFinished = false;
        StartCoroutine(TypeWriter("\n\"There is none\" rings in your ears.", SFXTypewriter, 0.1f, 1, 0, mainText));
        while (!isFinished) yield return null;
        isFinished = false;
        StartCoroutine(TypeWriter("\n\nThe Moon is singing words of horrors particularly for the Lonely Hoonter.", SFXTypewriter, 0.1f, 1, 0, mainText));
        while (!isFinished) yield return null;
        isFinished = false;
        StartCoroutine(TypeWriter("\n\"Unholy Moon! Ooooh You wretched gory Moon! Why do you torture me! I do what you please...\"", SFXTypewriter, 0.1f, 2, 0, mainText));
        while (!isFinished) yield return null;
        isFinished = false;
        StartCoroutine(TypeWriter("\n\"Yet it's all for naught!\"", SFXTypewriter, 0.1f, 1, 0, mainText));
        while (!isFinished) yield return null;
        isFinished = false;
        StartCoroutine(TypeWriter("\n\"...What now?\"", SFXTypewriter, 0.1f, 2, 0, mainText));
        while (!isFinished) yield return null;
        isFinished = false;
        StartCoroutine(TypeWriter("\nThe ringing and singing suddenly died off. Only You remain.", SFXTypewriter, 0.1f, 1, 0, mainText));
        while (!isFinished) yield return null;
        isFinished = false;
        StartCoroutine(TypeWriter("\nNo more London, Birminghway not even the wretched Mole.", SFXTypewriter, 0.1f, 1, 0, mainText));
        while (!isFinished) yield return null;
        isFinished = false;
        StartCoroutine(TypeWriter("\n\nOnly now you feel the true loneliness.", SFXTypewriter, 0.1f, 2, 0, mainText));
        while (!isFinished) yield return null;
        isFinished = false;
        yield return new WaitForSeconds(2);
        StartCoroutine(FadeOutCanvas(canvasGroup, 1));
        yield return new WaitForSeconds(2);
        endingText.SetActive(true);
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
