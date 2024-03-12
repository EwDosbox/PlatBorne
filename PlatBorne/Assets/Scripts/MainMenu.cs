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
        SettingsLoad();
        yield return new WaitForSeconds(1.1f);
        panel.SetActive(true);
        src.PlayOneShot(srcTwo);
        sign.SetTrigger("SignDown");
        text.enabled = true;
        yield return new WaitForSeconds(0.3f);
        text.gameObject.SetActive(true);
    }
    private void SettingsLoad()
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
        }
    }

    public void Settings()
    {
        src.PlayOneShot(srcOne);
        mainMenuBackground.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void ReturnFromSettings()
    {
        mainMenuBackground.SetActive(true);
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
