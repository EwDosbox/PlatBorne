using System.Collections;
using TMPro;
using UnityEngine;

public class SubtitlesManager : MonoBehaviour
{
    public GameObject smallBlackBox;
    public RectTransform smallBlackBoxRect;
    public GameObject canvas;
    public RectTransform textRect;
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
            if (currentlyWriting)
            {
                StopCoroutine(writingCoroutine);
                RestartGameObject();
            }
            text.text = "";
            mainSubtitles = subtitles;
            currentlyWriting = true;
            textSpeed = audioDuration / mainSubtitles.Length;
            writingCoroutine = StartCoroutine(Writing());
            smallBlackBox.SetActive(true);
        }
    }

    IEnumerator Writing()
    {
        canvas.SetActive(true);
        foreach (char c in mainSubtitles)
        {
            if (stop)
            {
                RestartGameObject();
                yield break;
            }

            // Update the size of the box based on the text size
            smallBlackBoxRect.sizeDelta = new Vector2(text.preferredWidth + Screen.width * 0.05f, text.preferredHeight + 10);
            text.text += c;

            yield return new WaitForSeconds(textSpeed);
        }

        // Fade out
        for (float i = 1; i > 0; i -= 0.01f)
        {
            if (stop)
            {
                RestartGameObject();
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
        canvas.SetActive(false);
        canvasGroup.alpha = 1;
        currentlyWriting = false;
    }

    private void Start()
    {
        UpdateBoxPosition();
    }

    private void UpdateBoxPosition()
    {
        smallBlackBoxRect.anchoredPosition = new Vector2(0, Screen.height * 0.1f - Screen.height * 0.5f);
        textRect.anchoredPosition = new Vector2(0, Screen.height * 0.1f - Screen.height * 0.5f);
        textRect.sizeDelta = new Vector2(Screen.width, textRect.sizeDelta.y);
    }
}
