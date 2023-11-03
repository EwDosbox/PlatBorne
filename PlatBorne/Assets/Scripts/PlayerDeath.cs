using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private AudioSource VLDeath01, VLDeath02, VLDeath03, VLDeath04, VLDeath05, VLDeath06, VLDeath07, VLDeath08, VLDeath09, VLDeath10, VLDeath11, VLDeath12, VLDeath13, VLDeath14, VLDeath15, VLDeath16, VLDeath17;
    //[SerializeField] float speed, waiting;
    public GameObject DialoguePanel;
    string dialogue;
    //public Text DialogueText;
    int bossDeathCount;

    private void Start()
    {
        /* dialogue[0] = "First death is always best one.";
         dialogue[1] = "You cannot escape Death.";
         dialogue[2] = "3rd time the charm?";
         dialogue[3] = "Die hunter and never come back.";
         dialogue[4] = "Leave hunter, go back to drinking the blood of gods.";
         dialogue[5] = "You are weak, cheeky hunter.";
         dialogue[6] = "I have slain bigger than you.";
         dialogue[7] = "First time is funny, 8 times is bloody annoying.";
         dialogue[8] = "Are you even trying?";
         dialogue[9] = "Dying 10 times must be an achievement.";
         dialogue[10] = "Until we meet again.";
         dialogue[11] = "You are not Kenough.";
         dialogue[12] = "You are not My Little Pogchamp.";
         dialogue[13] = "You need to kill me, Muppet, not yourself.";
         dialogue[14] = "Skill issue.";
         dialogue[15] = "The only thing I know for real You are dead again…how?!";
         dialogue[16] = "zzz";
        */
        //int bossDeathCount = PlayerPrefs.GetInt("PlayerDeath", 1);
        int bossDeathCount = PlayerPrefs.GetInt("NumberOfDeath",0);
        switch (bossDeathCount)
        {
            case 1:
                {
                    dialogue = "First death is always best one.";
                    VLDeath01.Play();
                    //StartCoroutine(Typing().ToString());
                    Debug.Log("Play Voice Line Player Death01");
                    break;
                }
            case 2:
                {
                    dialogue = "You cannot escape Death.";
                    VLDeath02.Play();
                    //StartCoroutine(Typing().ToString());
                    Debug.Log("Play Voice Line Player Death02");
                    break;
                }
            case 3:
                {
                    dialogue = "3rd time the charm?";
                    VLDeath03.Play();
                    //StartCoroutine(Typing().ToString());
                    Debug.Log("Play Voice Line Player Death03");
                    break;
                }
            case 4:
                {
                    dialogue = "Die hunter and never come back.";
                    VLDeath04.Play();
                   // StartCoroutine(Typing().ToString());
                    Debug.Log("Play Voice Line Player Death04");
                    break;
                }
            case 5:
                {
                    dialogue = "Leave hunter, go back to drinking the blood of gods.";
                    VLDeath05.Play();
                   // StartCoroutine(Typing().ToString());
                    Debug.Log("Play Voice Line Player Death05");
                    break;
                }
            case 6:
                {
                    VLDeath06.Play();
                   // StartCoroutine(Typing().ToString());
                    Debug.Log("Play Voice Line Player Death06");
                    break;
                }
            case 7:
                {
                    VLDeath07.Play();
                  //  StartCoroutine(Typing().ToString());
                    Debug.Log("Play Voice Line Player Death07");
                    break;
                }
            case 8:
                {
                    VLDeath08.Play();
                   // StartCoroutine(Typing().ToString());
                    Debug.Log("Play Voice Line Player Death08");
                    break;
                }
            case 9:
                {
                    VLDeath09.Play();
                    //StartCoroutine(Typing().ToString()); 
                    Debug.Log("Play Voice Line Player Death09");
                    break;
                }
            case 10:
                {
                    VLDeath10.Play();
                    //Typing();
                    Debug.Log("Play Voice Line Player Death10");
                    break;
                }
            case 11:
                {
                    VLDeath11.Play();
                   // StartCoroutine(Typing().ToString()); 
                    Debug.Log("Play Voice Line Player Death11");
                    break;
                }
            case 12:
                {
                    VLDeath12.Play();
                    //StartCoroutine(Typing().ToString());
                    Debug.Log("Play Voice Line Player Death12");
                    break;
                }
            case 13:
                {
                    VLDeath13.Play();
                    //StartCoroutine(Typing().ToString());
                    Debug.Log("Play Voice Line Player Death13");
                    break;
                }
            case 14:
                {
                    VLDeath14.Play();
                   // StartCoroutine(Typing().ToString());
                    Debug.Log("Play Voice Line Player Death14");
                    break;
                }
            case 15:
                {
                    VLDeath15.Play();
                   // StartCoroutine(Typing().ToString());
                    Debug.Log("Play Voice Line Player Death15");
                    break;
                }
            case 16:
                {
                    VLDeath16.Play();
                    //StartCoroutine(Typing().ToString());
                    Debug.Log("Play Voice Line Player Death16");
                    break;
                }
            case 17:
                {
                    VLDeath17.Play();
                    //StartCoroutine(Typing().ToString());
                    Debug.Log("Play Voice Line Player Death17");
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("LevelBoss");
            Debug.Log("A hunter has respawned");

        }
    }
}
