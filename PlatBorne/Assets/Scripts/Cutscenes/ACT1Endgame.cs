using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

class EndgameCutscene : MonoBehaviour
{
    public Text mainText;
    int i = 0;
    public CanvasGroup canvasText;
    string[] text =
            {
            "And with Brecus out of the picture, you travel further on your journey to Birmingham to send another monster where it belongs.",
            "\nYou don't know what the next day will bring,",
            "\nbut you sure are ready for it...",
            };
    bool isFinishedText;
    public AudioSource soundEffect;
    public AudioSource music;
    public float timeBtwChars = 0.1f;
    public float delay = 2f;

    private void Start()
    {
        i = 0;
        isFinishedText = false;
        StartCoroutine("TypeWriter");
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Debug.Log("Cutscene skipped");
            SceneManager.LoadScene("Cutscene_EndGameAct1");
            Debug.Log("Scene: Cutscene_EndGameStats");
        }
        if (isFinishedText && i < 3)
        {
            isFinishedText = false;
            StartCoroutine("TypeWriter");
        }
        else if (i == 3)
        {
            StartCoroutine(FadeOutCanvas(canvasText, 2));
            StartCoroutine(Wait(3));
        }
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
        fadeshit.alpha = 0f;
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("Cutscene_EndGameAct1");
    }    
}
