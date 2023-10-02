using System.Collections;
using System.Security.Authentication.ExtendedProtection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource src;
    public AudioClip srcOne;

    public bool InGame = false;
    
    public void PlayGame()
    {
        src.PlayOneShot(srcOne);
        StartCoroutine(_PlayGame());
    }

    private IEnumerator _PlayGame()
    {
        InGame = true;
        yield return new WaitForSeconds(srcOne.length);
        SceneManager.LoadScene("SampleScene");
    }

    public void Settings()
    {
        src.PlayOneShot(srcOne);
        StartCoroutine(_Settings());
    }

    private IEnumerator _Settings()
    {
        yield return new WaitForSeconds(srcOne.length);
        SceneManager.LoadScene("Settings");
    }

    public void Quit()
    {
        src.PlayOneShot(srcOne);
        StartCoroutine(_Quit());
    }

    private IEnumerator _Quit()
    {
        yield return new WaitForSeconds(srcOne.length);
        Debug.Log("Application has quit");
        Application.Quit();
    }

    public void ReturnFromSettings()
    {
        src.PlayOneShot(srcOne);
        StartCoroutine(_Return());
    }

    private IEnumerator _Return()
    {
        yield return new WaitForSeconds(srcOne.length);
        if (InGame)
        {
            SceneManager.LoadScene("SampleScene");
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void MMenu()
    {
        src.PlayOneShot(srcOne);
        StartCoroutine(_MMenu());
    }


    private IEnumerator _MMenu()
    {
        yield return new WaitForSeconds(srcOne.length);
        SceneManager.LoadScene("MainMenu");
    }


    //*****************************************stary, prvni kod*************************
    //public void GoToScene(string sceneName)
    //{
    //    SceneManager.LoadScene(sceneName);
    //}

    //public void QuitApp()
    //{
    //    Application.Quit();
    //    Debug.Log("Application has quit");
    //}
}
