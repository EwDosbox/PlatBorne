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
    [SerializeField] float speed = 2, waiting = 0.06f;
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
        DialoguePanel.SetActive(false);
        DialogueText = null;
    }

    IEnumerable Typing(string dialog)
    {

        foreach (char letter in dialogue.ToCharArray())
        {
            DialogueText.text += letter;
            yield return new WaitForSeconds(speed);
        }
        TextReset();
    }
}
