using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [SerializeField] Toggle subtitlesToggle;
    [SerializeField] Toggle vsyncToggle;
    [SerializeField] Toggle fullscreenToggle;

    public TMPro.TMP_Dropdown resolutionDropdown;
    public AudioSource src;

    Resolution[] resolutions;
    public Animator transitionAnim;

    void Start()
    {
        if (PlayerPrefs.HasKey("MusicVolume")) LoadMusicVolume();
        else SetMusicVolume(0.5f); // default volume

        if (PlayerPrefs.HasKey("SFXvolume")) LoadSFXVolume();
        else SetSFXVolume(0.5f); // default volume

        subtitlesToggle.isOn = PlayerPrefs.HasKey("Subtitles");
        vsyncToggle.isOn = PlayerPrefs.HasKey("Vsync");
        fullscreenToggle.isOn = PlayerPrefs.HasKey("FullScreen");

        resolutions = Screen.resolutions;
        resolutions = Screen.resolutions
                    .GroupBy(r => new { r.width, r.height }) // Prevent duplicates
                    .Select(g => g.First())
                    .ToArray();

        if (resolutionDropdown != null)
        {
            resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();
            int currentResolutionIndex = 0;

            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;
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
        menu.CloseSettingsMenu();
    }

    public void SetResolution(int resolutionIndex)
    {
        src.Play();
        Resolution resolution = resolutions[resolutionIndex];
        if (PlayerPrefs.HasKey("FullScreen")) Screen.SetResolution(resolution.width, resolution.height, true, 60);
        else Screen.SetResolution(resolution.width, resolution.height, false, 60);
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat("Music", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f);
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float volume)
    {
        musicSlider.value = volume;
        audioMixer.SetFloat("Music", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f);
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    public void LoadMusicVolume()
    {
        float volume = PlayerPrefs.GetFloat("MusicVolume");
        musicSlider.value = volume;
        audioMixer.SetFloat("Music", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f);
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        audioMixer.SetFloat("SFX", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f);
        PlayerPrefs.SetFloat("SFXvolume", volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume)
    {
        sfxSlider.value = volume;
        audioMixer.SetFloat("SFX", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f);
        PlayerPrefs.SetFloat("SFXvolume", volume);
        PlayerPrefs.Save();
    }

    public void LoadSFXVolume()
    {
        float volume = PlayerPrefs.GetFloat("SFXvolume");
        sfxSlider.value = volume;
        audioMixer.SetFloat("SFX", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f);
    }

    public void SetFullscreen()
    {
        src.Play();
        Screen.fullScreen = fullscreenToggle.isOn;
        if (fullscreenToggle.isOn) PlayerPrefs.SetInt("FullScreen", 1);
        else PlayerPrefs.DeleteKey("FullScreen");
        PlayerPrefs.Save();
    }

    public void SetVsync()
    {
        src.Play();
        if (vsyncToggle.isOn) QualitySettings.vSyncCount = 1;
        else QualitySettings.vSyncCount = 0;

        if (vsyncToggle.isOn) PlayerPrefs.SetInt("Vsync", 1);
        else PlayerPrefs.DeleteKey("Vsync");

        PlayerPrefs.Save();
    }

    public void SetSubtitles()
    {
        src.Play();
        if (subtitlesToggle.isOn) PlayerPrefs.SetInt("Subtitles", 1);
        else PlayerPrefs.DeleteKey("Subtitles");
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
}
