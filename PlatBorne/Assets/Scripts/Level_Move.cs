using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_Move : MonoBehaviour
{
    public string sceneName;
    public Animator transitionAnim;

    private IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigget entered");

        if (other.tag == "Player")
        {
            Debug.Log("Switching scene");
            transitionAnim.SetTrigger("Fade_End");
            yield return new WaitForSeconds(0.99f);
            SceneManager.LoadScene(sceneName);
        }
    }
}
