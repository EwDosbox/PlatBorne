using System;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerDeath : MonoBehaviour
{
    private AudioSource[] VLBrecusDeath;
    private AudioSource[] VLMoleDeath;
    [SerializeField]  string[] textBrecus;
    [SerializeField]  string[] textMole;
    public Text dialogueText;
    public Text pussyModeText;
    bool pussyModeActive = false;
    int bossDeathCount = 0;
    float waitingTimer = 0;
    private Coroutine currentCoroutine;

    private void Start()
    {
        try
        {
            VLBrecusDeath = transform.Find("Brecus").GetComponentsInChildren<AudioSource>();
            VLMoleDeath = transform.Find("Mole").GetComponentsInChildren<AudioSource>();
            if (VLBrecusDeath == null || VLMoleDeath == null) throw new NotImplementedException();
            if (PlayerPrefs.HasKey("PussyMode"))
            {
                int numberPussy = (PlayerPrefs.GetInt("PussyMode"));
                if (numberPussy == 1)
                {
                    pussyModeActive = true;
                    pussyModeText.text = "Press 'Space' to deactivate Pussy Mode\n(heal after each phase)";
                }
            }
            string level = PlayerPrefs.GetString("Level");
            if (level == "bricus")
            {
                bossDeathCount = PlayerPrefs.GetInt("NumberOfDeath");
                Debug.Log("bossDeathCount: " + bossDeathCount);
                if (bossDeathCount != 0 && bossDeathCount < VLBrecusDeath.Length)
                {
                    VLBrecusDeath[bossDeathCount - 1].Play();
                    TypeWriterText(textBrecus[bossDeathCount - 1], 0.1f, dialogueText);
                }
                else
                {
                    VLBrecusDeath.Last().Play();
                    TypeWriterText(textBrecus.Last(), 0.1f, dialogueText);
                }
            }
            else if (level == "mole")
            {
                bossDeathCount = PlayerPrefs.GetInt("NumberOfDeath_Mole");
                Debug.Log("bossDeathCount: " + bossDeathCount);
                if (bossDeathCount != 0 && bossDeathCount < VLMoleDeath.Length)
                {
                    VLMoleDeath[bossDeathCount - 1].Play();
                    TypeWriterText(textMole[bossDeathCount - 1], 0.1f, dialogueText);
                }
                else
                {
                    VLMoleDeath.Last().Play();
                    TypeWriterText(textMole.Last(), 0.1f, dialogueText);
                }
            }
            else throw new NotImplementedException();
        }
        catch (NotImplementedException) //Gotta use this beacuse I aint doing my own exception
        {
            TypeWriterText("Damn Mike Wazowski You Broke My Code...How Did you Do That?", 0.1f, dialogueText);
        }
    }
    private void Update()
    {
        waitingTimer += Time.deltaTime;
        if (Input.GetKey(KeyCode.Escape))
        {
            Debug.Log(PlayerPrefs.GetString("Level"));
            if (PlayerPrefs.GetString("Level") == "mole") SceneManager.LoadScene("LevelMole");
            else if (PlayerPrefs.GetString("Level") == "bricus") SceneManager.LoadScene("LevelBoss");
        }              
        if (Input.GetKey(KeyCode.Space) && waitingTimer > 1)
        {
            if (pussyModeActive)
            {
                pussyModeActive = false;
                pussyModeText.text = "Pussy Mode Deactivated";
                PlayerPrefs.SetInt("PussyMode", 0);
                PlayerPrefs.Save();
            }
            else
            {
                pussyModeActive = true;
                pussyModeText.text = "Pussy Mode Activated";
                PlayerPrefs.SetInt("PussyMode", 1);
                PlayerPrefs.Save();
            }
            waitingTimer = 0;
        }
    }
    private IEnumerator TypeWriter(string writerText, float timeBtwChars, Text UIMainText)
    {
        foreach (char c in writerText)
        {
            UIMainText.text += c;
            yield return new WaitForSeconds(timeBtwChars);
        }
    }

    private void TypeWriterText(string writerText, float timeBtwChars, Text UIMainText)
    {
        if (currentCoroutine == null)
        {
            currentCoroutine = StartCoroutine(TypeWriter(writerText, timeBtwChars, UIMainText));
            Debug.Log("Playing Message");
        }
        else
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
            UIMainText.text = ""; // Reset the text
        }        
    }

    
}