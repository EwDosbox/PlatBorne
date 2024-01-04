using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.ShaderData;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private AudioSource VLDeath01, VLDeath02, VLDeath03, VLDeath04, VLDeath05, VLDeath06, VLDeath07, VLDeath08, VLDeath09, VLDeath10, VLDeath11, VLDeath12, VLDeath13, VLDeath14, VLDeath15, VLDeath16, VLDeath17;
    public Text DialogueText;
    public Text pussyModeText;
    bool pussyModeActive = false;
    string dialogue;
    float waitingTimer = 0;
    int bossDeathCount;

    private void Start()
    {
        if (PlayerPrefs.HasKey("PussyMode")) 
        {
            int numberPussy = (PlayerPrefs.GetInt("PussyMode"));
            if (numberPussy == 1)
            {
                pussyModeActive = true;
                pussyModeText.text = "Press 'Space' to deactivate Pussy Mode\n(heal after each phase)";
            }
            else pussyModeText.text = "Press 'Space' to activate Pussy Mode\n(heal after each phase)";
        }
        int bossDeathCount = PlayerPrefs.GetInt("NumberOfDeath");
        switch (bossDeathCount)
        {
            case 1:
                {
                    dialogue = "First death is always best one.";
                    VLDeath01.Play();
                    Typing(dialogue, DialogueText);
                    Debug.Log("Play Voice Line Player Death01");
                    break;
                }
            case 2:
                {
                    dialogue = "You cannot escape Death.";
                    VLDeath02.Play();
                    Typing(dialogue, DialogueText);
                    Debug.Log("Play Voice Line Player Death02");
                    break;
                }
            case 3:
                {
                    dialogue = "3rd time the charm?";
                    VLDeath03.Play();
                    Typing(dialogue, DialogueText);
                    Debug.Log("Play Voice Line Player Death03");
                    break;
                }
            case 4:
                {
                    dialogue = "Die hunter and never come back.";
                    VLDeath04.Play();
                    Typing(dialogue, DialogueText);
                    Debug.Log("Play Voice Line Player Death04");
                    break;
                }
            case 5:
                {
                    dialogue = "Leave hunter, go back to drinking the blood of gods.";
                    VLDeath05.Play();
                    Typing(dialogue, DialogueText);
                    Debug.Log("Play Voice Line Player Death05");
                    break;
                }
            case 6:
                {
                    dialogue = "You are weak, cheeky hunter.";
                    VLDeath06.Play();
                    Typing(dialogue, DialogueText);
                    Debug.Log("Play Voice Line Player Death06");
                    break;
                }
            case 7:
                {
                    dialogue = "I have slain bigger than you.";
                    VLDeath07.Play();
                    Typing(dialogue, DialogueText);
                    Debug.Log("Play Voice Line Player Death07");
                    break;
                }
            case 8:
                {
                    dialogue = "First time is funny, 8 times is bloody annoying.";
                    VLDeath08.Play();
                    Typing(dialogue, DialogueText);
                    Debug.Log("Play Voice Line Player Death08");
                    break;
                }
            case 9:
                {
                    dialogue = "Are you even trying?";
                    VLDeath09.Play();
                    Typing(dialogue, DialogueText);
                    Debug.Log("Play Voice Line Player Death09");
                    break;
                }
            case 10:
                {
                    dialogue = "Dying 10 times must be an achievement.";
                    VLDeath10.Play();
                    Typing(dialogue, DialogueText);
                    Debug.Log("Play Voice Line Player Death10");
                    break;
                }
            case 11:
                {
                    dialogue = "Until we meet again.";
                    VLDeath11.Play();
                    Typing(dialogue, DialogueText);
                    Debug.Log("Play Voice Line Player Death11");
                    break;
                }
            case 12:
                {
                    dialogue = "You are not Kenough.";
                    VLDeath12.Play();
                    Typing(dialogue, DialogueText);
                    Debug.Log("Play Voice Line Player Death12");
                    break;
                }
            case 13:
                {
                    dialogue = "You are not My Little Pogchamp.";
                    VLDeath13.Play();
                    Typing(dialogue, DialogueText);
                    Debug.Log("Play Voice Line Player Death13");
                    break;
                }
            case 14:
                {
                    dialogue = "You need to kill me, Muppet, not yourself.";
                    VLDeath14.Play();
                    Typing(dialogue, DialogueText);
                    Debug.Log("Play Voice Line Player Death14");
                    break;
                }
            case 15:
                {
                    dialogue = "Skill issue.";
                    VLDeath15.Play();
                    Typing(dialogue, DialogueText);
                    Debug.Log("Play Voice Line Player Death15");
                    break;
                }
            case 16:
                {
                    dialogue = "The only thing I know for real You are dead again…how?!";
                    VLDeath16.Play();
                    Typing(dialogue, DialogueText);
                    Debug.Log("Play Voice Line Player Death16");
                    break;
                }
            default:
                {
                    dialogue = "...";
                    VLDeath17.Play();
                    Typing(dialogue, DialogueText);
                    Debug.Log("Play Voice Line Player Death17");
                    break;
                }
        }
    }
    private void Update()
    {
        waitingTimer += Time.deltaTime;
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("LevelBoss");
            Debug.Log("A hunter has respawned");
        }
        //text
        if (textType)
        {
            timerTextSpeed += Time.deltaTime;
            if (timerTextSpeed > textSpeed)
            {
                timerTextSpeed = 0;
                j++;
                Typing(dialogue, DialogueText);
            }
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
    //***
    public float textSpeed;
    private float timerTextSpeed = 0;
    private bool textType = false;
    private bool textReset = false;
    private int j = 0;
    //***
    private void TextReset()
    {
        textType = false;
        textReset = true;
        Debug.Log("Text Reset");
        j = 0;
    }

    private void Typing(string s, Text text)
    {
        textType = true;
        if (j == s.Length)
        {
            TextReset();
        }
        else text.text += s[j].ToString();
    }
}