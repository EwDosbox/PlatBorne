using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerDeath : MonoBehaviour
{
    public Animator transitionAnim;
    [SerializeField] private AudioSource VLDeath01, VLDeath02, VLDeath03, VLDeath04, VLDeath05, VLDeath06, VLDeath07, VLDeath08, VLDeath09, VLDeath10, VLDeath11, VLDeath12, VLDeath13, VLDeath14, VLDeath15, VLDeath16, VLDeath17;
    [SerializeField] float speed, waiting;
    public GameObject DialoguePanel;
    string[] dialogue;
    public Text DialogueText;
    int bossDeathCount = 0;

    private void Start()
    {
        string[] dialogue = new string[17];
        int BossDeathCount = PlayerPrefs.GetInt("PlayerDeath", 0);
        BossDeathCount++;
        PlayerPrefs.SetInt("PlayerDeath", BossDeathCount);
        PlayerPrefs.Save();
        switch (BossDeathCount)
        {
            case 1:
                {
                    VLDeath01.Play();
                    Typing();
                    Debug.Log("Play Voice Line Player Death01");
                    break;
                }
            case 2:
                {
                    VLDeath02.Play();
                    Typing();
                    Debug.Log("Play Voice Line Player Death02");
                    break;
                }
            case 3:
                {
                    VLDeath03.Play();
                    Typing();
                    Debug.Log("Play Voice Line Player Death03");
                    break;
                }
            case 4:
                {
                    VLDeath04.Play();
                    Typing();
                    Debug.Log("Play Voice Line Player Death04");
                    break;
                }
            case 5:
                {
                    VLDeath05.Play();
                    Typing();
                    Debug.Log("Play Voice Line Player Death05");
                    break;
                }
            case 6:
                {
                    VLDeath06.Play();
                    Typing();
                    Debug.Log("Play Voice Line Player Death06");
                    break;
                }
            case 7:
                {
                    VLDeath07.Play();
                    Typing();
                    Debug.Log("Play Voice Line Player Death07");
                    break;
                }
            case 8:
                {
                    VLDeath08.Play();
                    Typing();
                    Debug.Log("Play Voice Line Player Death08");
                    break;
                }
            case 9:
                {
                    VLDeath09.Play();
                    Typing();
                    Debug.Log("Play Voice Line Player Death09");
                    break;
                }
            case 10:
                {
                    VLDeath10.Play();
                    Typing();
                    Debug.Log("Play Voice Line Player Death10");
                    break;
                }
            case 11:
                {
                    VLDeath11.Play();
                    Typing();
                    Debug.Log("Play Voice Line Player Death11");
                    break;
                }
            case 12:
                {
                    VLDeath12.Play();
                    Typing();
                    Debug.Log("Play Voice Line Player Death12");
                    break;
                }
            case 13:
                {
                    VLDeath13.Play();
                    Typing();
                    Debug.Log("Play Voice Line Player Death13");
                    break;
                }
            case 14:
                {
                    VLDeath14.Play();
                    Typing();
                    Debug.Log("Play Voice Line Player Death14");
                    break;
                }
            case 15:
                {
                    VLDeath15.Play();
                    Typing();
                    Debug.Log("Play Voice Line Player Death15");
                    break;
                }
            case 16:
                {
                    VLDeath16.Play();
                    Typing();
                    Debug.Log("Play Voice Line Player Death16");
                    break;
                }
            case 17:
                {
                    VLDeath17.Play();
                    Typing();
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
    IEnumerable Typing()
    {
            foreach (char letter in dialogue[bossDeathCount - 1].ToCharArray())
            {
                DialogueText.text += letter;
                yield return new WaitForSeconds(speed);
            }
    }
}
