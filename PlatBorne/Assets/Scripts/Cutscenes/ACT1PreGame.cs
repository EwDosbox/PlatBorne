using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine.InputSystem;

public class PreGameCutsceneScript : MonoBehaviour
{
    public Text mainText;
    public TMP_Text london;
    public TMP_Text date;
    int i = 0;
    string[] text =
            {
            "Hunters are special warriors that kill and destroy beasts that lie in this world.",
            "\nHunters must protect the remains of our former world.",
            "\nYou are one of the Hunters located in this destroyed place that every single human being left on this planet fears.",
            "\nThe city you call your home.",
            "\n\nBut there is another city, with worse monsters, bigger monsters,\nmonsters beyond human comprehension.",
            "\nYou have decided to travel there to free the city and his homeland of all evil.",
            "\n\nYou must travel to Birmingham and slay 'The Mole From The Lost City'.",
            "\nFor a Hoonter must hoont.",
            "\nBut to do that, you first need to escape your home, the wasteland named..."
            };
    bool isFinishedText;
    public AudioSource soundEffect;
    public AudioSource music;
    public AudioSource dramaticLondon;
    public CanvasGroup mainTextGroup;
    public CanvasGroup dateGroup;
    public CanvasGroup BIGTEXTGROUP;
    public float timeBtwChars;
    public float delay;
    public float delayOriginalName;
    public float delayFUCK;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        i = 0;
        isFinishedText = false;
        StartCoroutine("Wait");
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Debug.Log("Cutscene skipped");
            SceneManager.LoadScene("LevelLondon");
            Debug.Log("Scene: Level London");
        }
        if (isFinishedText && i < 9)
        {
            isFinishedText = false;
            StartCoroutine("TypeWriter");
        }
        else if (isFinishedText && i == 9) 
        {
            mainText.CrossFadeAlpha(0, 2f, true);
            isFinishedText = false;
            StartCoroutine("London");            
        }
        if (!dramaticLondon.isPlaying && dramaticLondon.time >= music.clip.length)
        {
            SceneManager.LoadScene("LevelLondon");
            Debug.Log("Scene: Level London");
        }
    }
    private IEnumerator London()
    {
        StartCoroutine(FadeOutCanvas(mainTextGroup));
        yield return new WaitForSeconds(delayFUCK);
        music.Stop();
        dramaticLondon.Play();
        mainTextGroup.alpha = 1f;
        yield return new WaitForSeconds(delayOriginalName);
        london.text = "LONDON";
        yield return new WaitForSeconds(delayOriginalName);
        date.text = "1872";
        StartCoroutine(FadeInCanvas(dateGroup));
        yield return new WaitForSeconds(delayFUCK);
    }

    private IEnumerator TypeWriter()
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

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(delay);
        isFinishedText = true;
    }

    IEnumerator FadeOutCanvas(CanvasGroup vocas)
    {
        float elapsedTime = 0f;
        float startAlpha = vocas.alpha;

        while (elapsedTime < 1)
        {
            float newAlpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / 1);
            vocas.alpha = newAlpha;
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        vocas.alpha = 0f;
        yield return new WaitForSeconds(1);
    }
    IEnumerator FadeInCanvas(CanvasGroup vocas)
    {
        float elapsedTime = 0f;
        float startAlpha = 0;

        while (elapsedTime < 1)
        {
            float newAlpha = Mathf.Lerp(startAlpha, 1f, elapsedTime / 1);
            vocas.alpha = newAlpha;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(1);
    }
}
