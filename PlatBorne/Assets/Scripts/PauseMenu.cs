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
        StartCoroutine(_Pause());
    }

    private IEnumerator _Pause()
    {
        yield return new WaitForSeconds(srcOne.length);
        pauseMenu.SetActive(true);
        GameIsPaused = true;
    }

    public void Resume()
    {
        src.PlayOneShot(srcOne);
        StartCoroutine(_Resume());
    }

    private IEnumerator _Resume()
    {
        yield return new WaitForSeconds(srcOne.length);
        pauseMenu.SetActive(false);
        GameIsPaused = false;
    }

    public void Settings()
    {
        src.PlayOneShot(srcOne);
        StartCoroutine(_Settings());
    }

    private IEnumerator _Settings()
    {
        yield return new WaitForSeconds(srcOne.length);
        IsInSettings = true;
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void ReturnFromSettings()
    {
        src.PlayOneShot(srcOne);
        StartCoroutine(_ReturnFromSettings());
    }

    private IEnumerator _ReturnFromSettings()
    {
        yield return new WaitForSeconds(srcOne.length);
        IsInSettings = false;
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(true);
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
}
