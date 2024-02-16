using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

class EndgameCutscene : MonoBehaviour
{
    public Text mainText;
    int i = 0;
    string[] text =
            {
            "And with Brecus out of the picture, you travel further on your journey to Birmingham to send another monster where it belongs.",
            "\nYou don't know what the next day will bring,",
            "\nbut you sure are ready for it...",
            "\n*ACT II is being worked on*"
            };
    bool isFinishedText;
    public AudioSource soundEffect;
    public AudioSource music;
    public float timeBtwChars = 0.1f;
    public float delay = 2f;

    private void Start()
    {        
        i = 0;
        isFinishedText = false;
        StartCoroutine("TypeWriter");
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Debug.Log("Cutscene skipped");
            SceneManager.LoadScene("Cutscene_EndGameAct1");
            Debug.Log("Scene: Cutscene_EndGameStats");
        }
        if (isFinishedText && i < 4)
        {
            isFinishedText = false;            
            StartCoroutine("TypeWriter");
        }
        if (!music.isPlaying && music.time >= music.clip.length)
        {
            SceneManager.LoadScene("Cutscene_EndGameAct1");
            Debug.Log("Scene: Cutscene_EndGameStats");
        }
    }
    private IEnumerator TypeWriter()
    {
        soundEffect.Play();
        foreach (char c in text[i])
        {
            mainText.text += c;
            yield return new WaitForSeconds(timeBtwChars);
        }
        soundEffect.Stop();
        yield return new WaitForSeconds(delay);
        isFinishedText = true;
        i++;
    }
}
