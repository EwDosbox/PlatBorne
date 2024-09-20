using System.Collections;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject settingsMenu;

    public AudioSource src;
    public AudioClip srcOne;

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
        src.PlayOneShot(srcOne);
        _Pause();
    }

    private void _Pause()
    {
        pauseMenu.SetActive(true);
        GameIsPaused = true;
    }

    public void Resume()
    {
        src.PlayOneShot(srcOne);
        _Resume();
    }

    private void _Resume()
    {
        pauseMenu.SetActive(false);
        GameIsPaused = false;
    }

    public void Settings()
    {
        src.PlayOneShot(srcOne);
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
        src.PlayOneShot(srcOne);
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
        src.PlayOneShot(srcOne);
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
