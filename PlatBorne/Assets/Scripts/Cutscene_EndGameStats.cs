using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class Cutscene_EndGameStats : MonoBehaviour
{
    public Text[] mainText;
    public Text[] dataText;
    public int numberOfStrings;
    string[] writerText =
    {
        "Ending:",
        "Number of Falls:",
        "Number of Jumps:",
        "Number of Deaths (Bricus):",
        "Time (London):",
        "Time (Bricus)"
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
        writerData[0] = "An Ending";
        writerData[1] = PlayerPrefs.GetInt("NumberOfFalls_London").ToString();
        writerData[2] = PlayerPrefs.GetInt("NumberOfJumps_Act1").ToString();
        writerData[3] = PlayerPrefs.GetInt("NumberOfDeath").ToString();
        writerData[4] = PlayerPrefs.GetFloat("Timer_London").ToString();
        writerData[5] = PlayerPrefs.GetFloat("Timer_Bricus").ToString();
        StartCoroutine("Wait");
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
        //continue
        if (Input.anyKeyDown) SceneManager.LoadScene("MainMenu");
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(delayBeforeStart);
        isFinishedData = true;
    }
    IEnumerator TypeWriterText()
    {
        foreach (char c in writerText[i])
        {
            mainText[i].text += c;
            yield return new WaitForSeconds(timeBtwChars);
        }
        yield return new WaitForSeconds(delayBeforeData);
        isFinishedText = true;
    }
    IEnumerator TypeWriterData()
    {
        foreach (char c in writerData[i])
        {
            dataText[i].text += c;
            yield return new WaitForSeconds(timeBtwChars);
        }
        yield return new WaitForSeconds(delayBeforeText);
        isFinishedData = true;
        i++;
    }
}
