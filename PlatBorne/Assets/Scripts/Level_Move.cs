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
        Debug.Log("Trigget entered");

        if (other.tag == "Player" && leftTheZone)
        {
            Debug.Log("Switching scene");
            leftTheZone = true;
            transitionAnim.SetTrigger("Fade_End");
            yield return new WaitForSeconds(0.99f);
            SceneManager.LoadScene(sceneName);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        leftTheZone = false;
    }
}
