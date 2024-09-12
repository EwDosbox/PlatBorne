using System.Collections;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossFightVoiceLines : MonoBehaviour
{
    public GameObject smallBlackBox;
    public GameObject largeBlackBox;
    public GameObject canvas;
    public CanvasGroup canvasGroup;
    public TMP_Text text;
    bool stop = false;
    bool currentlyWriting = false;

    private float textSpeed;
    public float fadeOutSpeed;
    private string mainSubtitles;

    private Coroutine writingCoroutine;

    public void Write(string subtitles, float audioDuration)
    {
        if (PlayerPrefs.HasKey("Subtitles"))
        {
            if (currentlyWriting) //Collision
            {
                StopCoroutine(writingCoroutine);
                RestartGameObject();
            }
            text.text = null;
            mainSubtitles = subtitles;
            currentlyWriting = true;
            textSpeed = audioDuration / mainSubtitles.Length;
            writingCoroutine = StartCoroutine(Writing());
            smallBlackBox.SetActive(true);
        }
    }

    public void Update()
    {
        if (text.preferredHeight > 40)
        {
            smallBlackBox.SetActive(false);
            largeBlackBox.SetActive(true);
        }
    }

    IEnumerator Writing()
    {
        canvas.SetActive(true);
        foreach (char c in mainSubtitles)
        {
            if (stop)
            {
                stop = false;
                RestartGameObject();
                StopCoroutine(writingCoroutine);
                yield break; 
            }

            text.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        //Fade out
        for (float i = 1; i > 0; i -= 0.01f)
        {
            if (stop)
            {
                stop = false;
                RestartGameObject();
                StopCoroutine(writingCoroutine);
                yield break;
            }
            canvasGroup.alpha = i;
            yield return new WaitForSeconds(fadeOutSpeed / 100);
        }

        RestartGameObject();
    }

    private void RestartGameObject()
    {
        text.text = null;
        smallBlackBox.SetActive(false);
        largeBlackBox.SetActive(false);
        canvas.SetActive(false);
        canvasGroup.alpha = 1;
        currentlyWriting = false;
    }
}


