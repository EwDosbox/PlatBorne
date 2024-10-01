using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicVolSlider;
    [SerializeField] private Slider SFXVolSlider;

    [SerializeField] Toggle subtitlesToggle;
    [SerializeField] Toggle vsyncToggle;
    [SerializeField] Toggle fullscreenToggle;

    public TMPro.TMP_Dropdown resolutionDropdown;

    public AudioSource src;

    Resolution[] resolutions;

    public Animator transitionAnim;

    void Start()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            LoadMusicVolume();
        }
        else
        {
            SetMusicVolume();
        }

        if (PlayerPrefs.HasKey("SFXvolume"))
        {
            LoadSFXvolume();
        }
        else
        {
            SetSFXVolume();
        }
        //Toggles
        if (PlayerPrefs.HasKey("Subtitles"))
        {
            subtitlesToggle.isOn = true;
        }
        else subtitlesToggle.isOn = false;

        if (PlayerPrefs.HasKey("Vsync"))
        {
            vsyncToggle.isOn = true;
        }
        else vsyncToggle.isOn = false;

        if (PlayerPrefs.HasKey("FullScreen"))
        {
            fullscreenToggle.isOn = true;
        }
        else fullscreenToggle.isOn = false;

        resolutions = Screen.resolutions;

        if (resolutionDropdown != null)
        {
            resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();

            int currentResolutionIndex = 0;
            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height + " @ " + resolutions[i].refreshRateRatio + "hz";
                options.Add(option);

                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }
    }

    public void GoBack()
    {
        PauseMenu menu = FindAnyObjectByType<PauseMenu>();
        menu.ReturnFromSettings();
    }

    public void SetResolution(int resolutionIndex)
    {
        src.Play();
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetMusicVolume()
    {
        float volume = musicVolSlider.value;
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void LoadMusicVolume()
    {
        float volume = PlayerPrefs.GetFloat("MusicVolume");
        SetMusicVolume(volume);
    }

    public void SetSFXVolume()
    {
        float volume = SFXVolSlider.value;
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXvolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXvolume", volume);
    }

    public void LoadSFXvolume()
    {
        float volume = PlayerPrefs.GetFloat("SFXvolume");
        SetSFXVolume(volume);
    }

    public void SetFullscreen()
    {
        src.Play();
        Screen.fullScreen = fullscreenToggle.isOn;

        if (fullscreenToggle.isOn)
        {
            PlayerPrefs.SetInt("FullScreen", 1);
        }
        else
        {
            PlayerPrefs.DeleteKey("FullScreen");
        }

        PlayerPrefs.Save();
    }


    public void MMenu()
    {
        src.Play();
        StartCoroutine(_MMenu());
    }


    private IEnumerator _MMenu()
    {
        transitionAnim.SetTrigger("Fade_End");
        yield return new WaitForSeconds(0.99f);
        SceneManager.LoadScene("MainMenu");
    }

    public void SetVsync()
    {
        src.Play();
        if (vsyncToggle.isOn)
        {
            QualitySettings.vSyncCount = 1;
            PlayerPrefs.SetInt("Vsync", 1);
        }
        else
        {
            QualitySettings.vSyncCount = 0;
            PlayerPrefs.DeleteKey("Vsync");
        }
        PlayerPrefs.Save();
    }

    public void SetSubtitles()
    {
        src.Play();
        if (subtitlesToggle.isOn)
        {
            PlayerPrefs.SetInt("Subtitles", 1);
        }
        else
        {
            PlayerPrefs.DeleteKey("Subtitles");
        }
        PlayerPrefs.Save();
    }

}
