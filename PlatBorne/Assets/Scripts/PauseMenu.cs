using System.Collections;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject settingsMenu;

    public AudioSource buttonClick;
    public static bool GameIsPaused = false;
    public static bool IsInSettings = false;

    Saves save;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                if (!IsInSettings)
                {
                    Resume();
                }
                else
                {
                    ReturnFromSettings();
                }
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        buttonClick.Play();
        _Pause();
    }

    private void _Pause()
    {
        pauseMenu.SetActive(true);
        GameIsPaused = true;
    }

    public void Resume()
    {
        buttonClick.Play();
        _Resume();
    }

    private void _Resume()
    {
        pauseMenu.SetActive(false);
        GameIsPaused = false;
    }

    public void Settings()
    {
        buttonClick.Play();
        _Settings();
    }

    public void _Settings()
    {
        IsInSettings = true;
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void ReturnFromSettings()
    {
        buttonClick.Play();
        _ReturnFromSettings();
    }

    private void _ReturnFromSettings()
    {
        IsInSettings = false;
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void Quit()
    {
        buttonClick.Play();
        _Quit();
    }

    private void _Quit()
    {
        Debug.Log("Application has quit");
        Application.Quit();
    }

    private void Start()
    {
        save = FindFirstObjectByType<Saves>();     
    }
}
