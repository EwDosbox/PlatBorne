using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

class Cutscene_EndGameStats : MonoBehaviour
{
    public Text[] mainText;
    public Text[] dataText;
    public Text endingText;
    public AudioSource soundEffect;
    public CanvasGroup canvasGroup;
    public int numberOfStrings;
    string[] writerText =
    {
        "Number of Falls:",
        "Number of Jumps:",
        "Number of Deaths (Brecus):",
        "Time (London):",
        "Time (Brecus):"
    };
    string[] writerData;
    int i = 0;

    [SerializeField] float delayBeforeStart;
    [SerializeField] float timeBtwChars;
    [SerializeField] float delayBeforeData;
    [SerializeField] float delayBeforeText;
    bool isFinishedText = false;
    bool isFinishedData = false;

    // Use this for initialization
    void Start()
    {
        writerData = new string[numberOfStrings];
        writerData[0] = PlayerPrefs.GetInt("NumberOfFalls_London").ToString();
        writerData[1] = PlayerPrefs.GetInt("NumberOfJumps_Act1").ToString();
        writerData[2] = PlayerPrefs.GetInt("NumberOfDeath").ToString();
        writerData[3] = TimeConvertor(PlayerPrefs.GetFloat("Timer_London"));
        writerData[4] = TimeConvertor(PlayerPrefs.GetFloat("Timer_Brecus"));
        StartCoroutine(Ending("ACT I Finished"));
    }

    private void Update()
    {
        if (i < numberOfStrings)
        {
            if (isFinishedData)
            {
                isFinishedData = false;
                StartCoroutine("TypeWriterText");
            }
            else if (isFinishedText)
            {
                isFinishedText = false;
                StartCoroutine("TypeWriterData");
            }
        }
        else
        {
            StartCoroutine(FadeOutCanvas(canvasGroup, 2));
        }
        //continue
        if (Input.anyKeyDown) SceneManager.LoadScene("Cutscene_ACT2Start");
    }
    private IEnumerator TypeWriterText()
    {
        soundEffect.Play();
        foreach (char c in writerText[i])
        {
            mainText[i].text += c;
            yield return new WaitForSeconds(timeBtwChars);
        }
        soundEffect.Stop();
        yield return new WaitForSeconds(delayBeforeData);
        isFinishedText = true;
    }
    IEnumerator TypeWriterData()
    {
        soundEffect.Play();
        foreach (char c in writerData[i])
        {
            dataText[i].text += c;
            yield return new WaitForSeconds(timeBtwChars);
        }
        soundEffect.Stop();
        yield return new WaitForSeconds(delayBeforeText);
        isFinishedData = true;
        i++;        
    }

    IEnumerator Ending(string ending)
    {
        yield return new WaitForSeconds(delayBeforeStart);
        soundEffect.Play();
        foreach (char c in ending)
        {
            endingText.text += c;
            yield return new WaitForSeconds(timeBtwChars);
        }
        soundEffect.Stop();
        yield return new WaitForSeconds(delayBeforeText);
        isFinishedData = true;
    }

    public static string TimeConvertor(float totalSeconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(totalSeconds);
        return time.ToString("hh':'mm':'ss'.'ffff");
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
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Cutscene_ACT2Start");

    }

}
