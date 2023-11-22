using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public AudioSource src;
    public AudioClip srcOne;
    public AudioClip srcTwo;

    public Animator transitionAnim;
    public Animator sign;

    public GameObject panel;
    public Button continueButton;
    public Button newGameButton;

    public TMP_Text text;
    
    public bool InGame = false;
    public bool PreGameCutscene = false;

    private IEnumerator Start()
    {
        if (PlayerPrefs.HasKey("HasASavedGame"))
        {
            continueButton.interactable = true;
        }
        else continueButton.interactable = false;

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            SetMusicVolume();
        }

        if (PlayerPrefs.HasKey("SFXvolume"))
        {
            SetSFXVolume();
        }

        yield return new WaitForSeconds(1.1f);
        panel.SetActive(true);
        src.PlayOneShot(srcTwo);
        sign.SetTrigger("SignDown");
        text.enabled = true;
        yield return new WaitForSeconds(0.3f);
        text.gameObject.SetActive(true);
    }
    public void NewGame()
    {
        src.PlayOneShot(srcOne);
        StartCoroutine(_NewGame());
    }
    private IEnumerator _NewGame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("HasASavedGame", 1);
        PlayerPrefs.Save();
        InGame = true;
        transitionAnim.SetTrigger("Fade_End");
        yield return new WaitForSeconds(0.99f);
        SceneManager.LoadScene("PreGameCutscene");
        PlayerPrefs.SetInt("PreGameCutsceneSeen", 1);
        Debug.Log("Scene: PreGameCutscene");
    }

    public void GameContinue()
    {
        src.PlayOneShot(srcOne);
        StartCoroutine(_GameContinue());
    }

    private IEnumerator _GameContinue()
    {
        InGame = true;
        transitionAnim.SetTrigger("Fade_End");
        yield return new WaitForSeconds(0.99f);
        {
            if (PlayerPrefs.GetString("Level") == "bossfight")
            {
                SceneManager.LoadScene("Bossfight");
                Debug.Log("Scene: LevelBossfight");
            }
            else
            {
                SceneManager.LoadScene("LevelLondon");
                Debug.Log("Scene: LevelLondon");
            }
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
