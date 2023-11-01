using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public AudioSource src;
    public AudioClip srcOne;

    public Animator transitionAnim;

    public bool InGame = false;
    public bool PreGameCutscene = false;

    private void Start()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            SetMusicVolume();
        }

        if (PlayerPrefs.HasKey("SFXvolume"))
        {
            SetSFXVolume();
        }
    }

    public void PlayGame()
    {
        src.PlayOneShot(srcOne);
        StartCoroutine(_PlayGame());
    }

    private IEnumerator _PlayGame()
    {
        InGame = true;
        transitionAnim.SetTrigger("Fade_End");
        yield return new WaitForSeconds(0.99f);
        if (!PlayerPrefs.HasKey("PreGame")) //If Player had already seen the cutscene or not
        {
            SceneManager.LoadScene("PreGame Cutscene");
            PlayerPrefs.SetInt("PreGame", 1);
            Debug.Log("Pre Game Cutscene activated");
        }
        else
        {
            SceneManager.LoadScene("LevelLondon");
            Debug.Log("Level London Activated");
        }
    }

    public void Settings()
    {
        src.PlayOneShot(srcOne);
        StartCoroutine(_Settings());
    }

    private IEnumerator _Settings()
    {
        transitionAnim.SetTrigger("Fade_End");
        yield return new WaitForSeconds(0.99f);
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

    public void MMenu()
    {
        src.PlayOneShot(srcOne);
        StartCoroutine(_MMenu());
    }


    private IEnumerator _MMenu()
    {
        transitionAnim.SetTrigger("Fade_End");
        yield return new WaitForSeconds(0.99f);
        SceneManager.LoadScene("MainMenu");
    }

    public void SetMusicVolume()
    {
        float volume = PlayerPrefs.GetFloat("MusicVolume");
        Debug.Log("Music: " + volume);
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }


    public void SetSFXVolume()
    {
        float volume = PlayerPrefs.GetFloat("SFXvolume");
        Debug.Log("SFX: " + volume);
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXvolume", volume);
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
