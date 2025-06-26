using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] AudioSource buttonClick;

    public static bool GameIsPaused { get; private set; } = false;
    public static bool IsInSettings { get; private set; } = false;

    void Start()
    {
        ApplySavedAudioSettings();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!GameIsPaused) ShowPauseMenu();
            else if (IsInSettings) CloseSettingsMenu();
            else ResumeGame();
        }
    }

    void ApplySavedAudioSettings()
    {
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        audioMixer.SetFloat("Music", Mathf.Log10(Mathf.Clamp(musicVolume, 0.0001f, 1f)) * 20f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXvolume", 0.5f);
        audioMixer.SetFloat("SFX", Mathf.Log10(Mathf.Clamp(sfxVolume, 0.0001f, 1f)) * 20f);
    }

    public void ShowPauseMenu()
    {
        PlayClick();
        pauseMenu.SetActive(true);
        settingsMenu.SetActive(false);
        GameIsPaused = true;
        IsInSettings = false;
    }

    public void ResumeGame()
    {
        PlayClick();
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        GameIsPaused = false;
        IsInSettings = false;
    }

    public void OpenSettingsMenu()
    {
        PlayClick();
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
        IsInSettings = true;
    }

    public void CloseSettingsMenu()
    {
        PlayClick();
        pauseMenu.SetActive(true);
        settingsMenu.SetActive(false);
        IsInSettings = false;
    }

    public void ReturnToMainMenu()
    {
        PlayClick();
        GameIsPaused = false;
        IsInSettings = false;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        PlayClick();
        Debug.Log("Application has quit");
        Application.Quit();
    }

    void PlayClick()
    {
        if (buttonClick != null && !buttonClick.isPlaying) buttonClick.Play();
    }
}
