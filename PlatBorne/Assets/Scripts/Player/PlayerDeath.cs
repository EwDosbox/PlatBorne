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
    int bossDeathCount;
    float waitingTimer = 0;
    

    private void Start()
    {
        VLBrecusDeath = GetComponentsInChildren<AudioSource>(true)
                                .Where(v => v.gameObject.name  == "Brecus")  // Exclude parent
                                .OrderBy(v => v.gameObject.name)  // Sort by name
                                .ToArray();
        VLMoleDeath = GetComponentsInChildren<AudioSource>(true)
                                .Where(v => v.gameObject.name == "Mole")  // Exclude parent
                                .OrderBy(v => v.gameObject.name)  // Sort by name
                                .ToArray();
        if (PlayerPrefs.HasKey("PussyMode")) 
        {
            int numberPussy = (PlayerPrefs.GetInt("PussyMode"));
            if (numberPussy == 1)
            {
                pussyModeActive = true;
                pussyModeText.text = "Press 'Space' to deactivate Pussy Mode\n(heal after each phase)";
            }
        }
        if (PlayerPrefs.GetString("Level") == "bricus")
        {
            int bossDeathCount = PlayerPrefs.GetInt("NumberOfDeath");
            if (bossDeathCount == 0 || bossDeathCount == VLBrecusDeath.Length)
            {
                VLBrecusDeath[VLBrecusDeath.Length - 1].Play();
                StartCoroutine(TypeWriterText(textBrecus[VLBrecusDeath.Length - 1], 0.1f, dialogueText));
            }
            else
            {
                VLBrecusDeath[bossDeathCount - 1].Play();
                StartCoroutine(TypeWriterText(textBrecus[bossDeathCount - 1], 0.1f, dialogueText));
            }
        }
        else if (PlayerPrefs.GetString("Level") == "mole")
        {
            int bossDeathCount = PlayerPrefs.GetInt("NumberOfDeath");
            if (bossDeathCount == 0 || bossDeathCount == VLBrecusDeath.Length)
            {
                VLBrecusDeath[VLBrecusDeath.Length - 1].Play();
                StartCoroutine(TypeWriterText(textBrecus[VLBrecusDeath.Length - 1], 0.1f, dialogueText));
            }
            else
            {
                VLBrecusDeath[bossDeathCount - 1].Play();
                StartCoroutine(TypeWriterText(textBrecus[bossDeathCount - 1], 0.1f, dialogueText));
            }
        }
        else StartCoroutine(TypeWriterText("Damn Mike Wazowski You Broke My Code...How Did you Do That?", 0.1f, dialogueText));
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
    private IEnumerator TypeWriterText(string writerText, float timeBtwChars, Text UIMainText)
    {
        foreach (char c in writerText)
        {
            UIMainText.text += c;
            yield return new WaitForSeconds(timeBtwChars);
        }
    }
}