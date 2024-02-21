using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] GameObject mainMenuBackground;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject actSelectorMenu;

    public AudioSource src;
    public AudioClip srcOne;
    public AudioClip srcTwo;

    public Animator transitionAnim;
    public Animator sign;

    public GameObject panel;
    public Button continueButton;
    public Button newGameButton;
    public Saves save;

    public TMP_Text text;
    
    public bool InGame = false;
    public bool PreGameCutscene = false;
    public bool IsInSettings = false;
    public bool actMenuOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsInSettings)
            {
                ReturnFromSettings();
            }
            else if (actMenuOpen)
            {
                CloseActMenu();
            }
        }
    }

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
        Debug.Log("cedula spadla");
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
        save.NewGameSaveReset();
        PlayerPrefs.SetInt("HasASavedGame", 1);
        PlayerPrefs.Save();
        InGame = true;
        transitionAnim.SetTrigger("Fade_End");
        yield return new WaitForSeconds(0.9f);
        SceneManager.LoadScene("Cutscene_StartGame");
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
        yield return new WaitForSeconds(0.9f);
        Debug.Log(PlayerPrefs.GetString("Level"));
        switch (PlayerPrefs.GetString("Level"))
        {
            case "bricus":
                {
                    SceneManager.LoadScene("LevelBoss");
                    Debug.Log("Scene: LevelBossfight");
                    break;
                }
            case "london":
                {
                    SceneManager.LoadScene("LevelLondon");
                    Debug.Log("Scene: LevelLondon");
                    break;
                }
            default:
                {
                    SceneManager.LoadScene("LevelLondon");
                    Debug.Log("Scene: LevelLondon");
                    break;             
                }
        }
    }

    public void Settings()
    {
        src.PlayOneShot(srcOne);
        IsInSettings = true;
        mainMenuBackground.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void ReturnFromSettings()
    {
        mainMenuBackground.SetActive(true);
        IsInSettings = false;
        src.PlayOneShot(srcOne);
        settingsMenu.SetActive(false);
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
        yield return new WaitForSeconds(0.9f);
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

    public void London()
    {
        src.PlayOneShot(srcOne);
        StartCoroutine(_London());
    }

    public IEnumerator _London()
    {
        InGame = true;
        transitionAnim.SetTrigger("Fade_End");
        yield return new WaitForSeconds(0.9f);
        SceneManager.LoadScene("LevelLondon");
    }

    public void Brecus()
    {
        src.PlayOneShot(srcOne);
        StartCoroutine(_Brecus());
    }

    public IEnumerator _Brecus()
    {
        InGame = true;
        transitionAnim.SetTrigger("Fade_End");
        yield return new WaitForSeconds(0.9f);
        SceneManager.LoadScene("LevelBoss");
    }

    public void Birmingham()
    {
        src.PlayOneShot(srcOne);
        StartCoroutine(_Birmingham());
    }
    public IEnumerator _Birmingham()
    {
        InGame = true;
        transitionAnim.SetTrigger("Fade_End");
        yield return new WaitForSeconds(0.9f);
        SceneManager.LoadScene("LevelBirmingham");
    }

    //public void Mole()
    //{
    //    src.PlayOneShot(srcOne);
    //    StartCoroutine(_Mole());
    //}

    //public IEnumerator _Mole()
    //{
    //    InGame = true;
    //    transitionAnim.SetTrigger("Fade_End");
    //    yield return new WaitForSeconds(0.9f);
    //    SceneManager.LoadScene("LevelMole");
    //}

    public void OpenActMenu()
    {
        src.PlayOneShot(srcOne);
        actMenuOpen = true;
        actSelectorMenu.SetActive(true);
    }

    public void CloseActMenu()
    {
        src.PlayOneShot(srcOne);
        actMenuOpen = false;
        actSelectorMenu.SetActive(false);
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
