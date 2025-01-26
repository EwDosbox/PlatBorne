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
        StartCoroutine(SettingsLoad());
        yield return new WaitForSeconds(1.1f);
        panel.SetActive(true);
        src.PlayOneShot(srcTwo);
        sign.SetTrigger("SignDown");
        text.enabled = true;
        yield return new WaitForSeconds(0.3f);
        text.gameObject.SetActive(true);
    }
    private IEnumerator SettingsLoad()
    {
        if (PlayerPrefs.HasKey("Vsync")) QualitySettings.vSyncCount = 1;
        else QualitySettings.vSyncCount = 0;

        if (PlayerPrefs.HasKey("FullScreen")) Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        else Screen.fullScreenMode = FullScreenMode.Windowed;

        if (PlayerPrefs.HasKey("Level"))
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
        NewGameSaveReset();
        PlayerPrefs.SetInt("HasASavedGame", 1);
        PlayerPrefs.Save();
        InGame = true;
        transitionAnim.SetTrigger("Fade_End");
        yield return new WaitForSeconds(0.9f);
        SceneManager.LoadScene("Cutscene_ACT1Start");
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
            case "birmingham":
                {
                    SceneManager.LoadScene("LevelBirmingham");
                    Debug.Log("Scene: LevelBirmingham");
                    break;
                }
            case "mole":
                {
                    SceneManager.LoadScene("LevelMole");
                    Debug.Log("Scene: LevelMole");
                    break;
                }
            case "fish":
                {
                    SceneManager.LoadScene("Fishing");
                    break;
                }
            default:
                {
                    SceneManager.LoadScene("LevelLondon");
                    Debug.LogError("Could not load a Save");
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

    public void NewGameSaveReset()
    {
        //MAIN
        PlayerPrefs.DeleteKey("Level");
        PlayerPrefs.DeleteKey("GodMode");
        PlayerPrefs.DeleteKey("PussyMode");
        //ACT I
        PlayerPrefs.DeleteKey("NumberOfFalls_London");
        PlayerPrefs.DeleteKey("NumberOfJumps_Act1");
        PlayerPrefs.DeleteKey("Timer_London");
        PlayerPrefs.DeleteKey("Timer_Brecus");
        PlayerPrefs.DeleteKey("HunterPositionY_London");
        PlayerPrefs.DeleteKey("HunterPositionX_London");
        PlayerPrefs.DeleteKey("RNGSaved");
        PlayerPrefs.DeleteKey("RNGNow");
        PlayerPrefs.DeleteKey("HasASavedGame");
        PlayerPrefs.DeleteKey("LondonVoiceLinesJ");
        PlayerPrefs.DeleteKey("LondonVoiceLinesArray");
        PlayerPrefs.DeleteKey("Brecus_BeatenWithPussy");
        //ACT II
        PlayerPrefs.DeleteKey("Mole_BeatenWithPussy");
        PlayerPrefs.DeleteKey("HunterPositionX_Birmingham");
        PlayerPrefs.DeleteKey("HunterPositionY_Birmingham");
        PlayerPrefs.DeleteKey("NumberOfFalls_Birmingham");
        PlayerPrefs.DeleteKey("NumberOfJumps_Act2");
        PlayerPrefs.DeleteKey("Timer_Birmingham");
        PlayerPrefs.DeleteKey("Timer_Mole");
    }
    public void London()
    {
        src.PlayOneShot(srcOne);
        StartCoroutine(_London());
    }

    public IEnumerator _London()
    {
        NewGameSaveReset();
        PlayerPrefs.SetString("Level", "london");
        PlayerPrefs.Save();
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
        NewGameSaveReset();
        PlayerPrefs.SetString("Level", "bricus");
        PlayerPrefs.Save();
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
        NewGameSaveReset();
        PlayerPrefs.SetString("Level", "birmingham");
        PlayerPrefs.Save();
        InGame = true;
        transitionAnim.SetTrigger("Fade_End");
        yield return new WaitForSeconds(0.9f);
        SceneManager.LoadScene("LevelBirmingham");
    }

    public void Mole()
    {
        src.PlayOneShot(srcOne);
        StartCoroutine(_Mole());
    }

    public IEnumerator _Mole()
    {
        NewGameSaveReset();
        PlayerPrefs.SetString("Level", "mole");
        PlayerPrefs.Save();
        InGame = true;
        transitionAnim.SetTrigger("Fade_End");
        yield return new WaitForSeconds(0.9f);
        SceneManager.LoadScene("LevelMole");
    }

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
