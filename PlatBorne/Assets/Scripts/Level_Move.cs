using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_Move : MonoBehaviour
{
    public string sceneName;
    public Animator transitionAnim;
    private bool leftTheZone = true;

    private IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && leftTheZone)
        {
            Debug.Log("Switching scene");
            leftTheZone = true;
            transitionAnim.SetTrigger("Fade_End");
            yield return new WaitForSeconds(0.99f);
            if (sceneName == "End")
            { //Ending Decider
                if (PlayerPrefs.HasKey("BeatenWithAPussyMode_Brecus") || PlayerPrefs.HasKey("BeatenWithAPussyMode_Mole"))
                {
                    SceneManager.LoadScene("Cutscene_BadEnding");
                }
                else SceneManager.LoadScene("Cutscene_GoodEnding");
            }
            else SceneManager.LoadScene(sceneName);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        leftTheZone = false;
    }
}
