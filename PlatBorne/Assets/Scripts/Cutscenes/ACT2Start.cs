using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using System.Drawing;
using System;
using Color = UnityEngine.Color;

public class ACT2Start : MonoBehaviour
{
    public Text mainText;
    int i = 0;
    string[] text =
            {
            "Only blood-thirsty monsters and chaos was left at the outskirts of London, a living hell...",
            "\nThis desolate place was once full of glamour and happiness.",
            "\n'Reduced to Ash and Darkness' you think to yourself.",
            "\nFear the Old Mole, Hoonter!"
            };
    bool isFinishedText;
    public AudioSource soundEffect;
    public AudioSource music;
    public float timeBtwChars;
    public float delay;
    public CanvasGroup canvasGroup;

    private void Start()
    {
        i = 0;
        isFinishedText = false;
        StartCoroutine("Wait");
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Debug.Log("Cutscene skipped");
            SceneManager.LoadScene("LevelBirmingham");
        }
        if (isFinishedText && i < 4)
        {
            isFinishedText = false;
            StartCoroutine(TypeWriter());
        }
        else if (isFinishedText && i == 4) StartCoroutine(FadeOutCanvas());
    }
    IEnumerator TypeWriter()
    {
        soundEffect.Play();
        foreach (char c in text[i])
        {
            mainText.text += c;
            yield return new WaitForSeconds(timeBtwChars);
        }
        soundEffect.Stop();
        yield return new WaitForSeconds(delay);
        isFinishedText = true;
        i++;
    }
    IEnumerator FadeOutCanvas()
    {
        float elapsedTime = 0f;
        float startAlpha = canvasGroup.alpha;

        while (elapsedTime < 1)
        {
            float newAlpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / 1);
            canvasGroup.alpha = newAlpha;
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        canvasGroup.alpha = 0f;
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("LevelBirmingham");
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(delay);
        isFinishedText = true;
    }
}
