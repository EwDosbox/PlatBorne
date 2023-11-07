using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossFightVoiceLines : MonoBehaviour
{
    [SerializeField] private AudioSource BossDamage01, BossDamage02, BossDamage03, PreBossDialog;
    private string dialogue = null;
    public GameObject DialoguePanel;
    public Text DialogueText;
    private int j = 0;
    //***
    public float textSpeed;
    public float waitingTime;
    private float timerTextSpeed = 0;
    private bool textType = false;
    private bool textReset = false;
    private float timerTextReset = 0;
    //***

    public void PlayBossDamage01()
    {
        DialoguePanel.SetActive(true);
        dialogue = "Nothing but a scratch!";
        BossDamage01.Play();
        Typing(dialogue);
    }
    public void PlayBossDamage02()
    {
        DialoguePanel.SetActive(true);
        dialogue = "You’re starting to annoy me, Hunter!";
        BossDamage02.Play();
        Typing(dialogue);
    }

    public void PlayBossDamage03()
    {
        DialoguePanel.SetActive(true);
        dialogue = "I‘m gonna sink you like Americans have sank our bloody delicious tea!";
        BossDamage03.Play();
        Typing(dialogue);
    }

    public void PlayPreBossDialog()
    {
        DialoguePanel.SetActive(true);
        dialogue = "So, you finally did it. Well Now its time to see if you really got what it takes to escape this bloodhole of a city...";
        PreBossDialog.Play();
        Typing(dialogue);
    }
    private void TextReset()
    {
        textType = false;
        textReset = true;
        if (timerTextReset > waitingTime)
        {
            timerTextReset = 0;
            Debug.Log("Text Reset");
            DialoguePanel.SetActive(false);
            DialogueText.text = null;
            j = 0;
        }
    }

    private void Typing(string s)
    {
        textType = true;
        DialoguePanel.SetActive(true);
        Debug.Log(dialogue.Length);
        if (j == s.Length)
        {
            TextReset();
        }
        else DialogueText.text += s[j];
    }

    private void Update()
    {
        if (textType)
        {
            timerTextSpeed += Time.deltaTime;
            Debug.Log(timerTextSpeed);
            if (timerTextSpeed > textSpeed)
            {
                timerTextSpeed = 0;
                j++;
                Typing(dialogue);
            }
        }

        if (textReset)
        {
            timerTextReset += Time.deltaTime;
            TextReset();
        }
    }
}

