using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NeutralEnding : MonoBehaviour
{
    public AudioSource SFXTypewriter;
    public AudioSource Music;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("Cutscene_EndGameAct2");
        }
    }
IEnumerator TypeWriter(string text, AudioSource soundEffect, float timeBtwChars, float delayStart, float delayEnd, Text UIText, bool isFinishedText)
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
        isFinishedText = true;
    }
}
