using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Mole_UI : MonoBehaviour
{
    //UNITY
    public CanvasGroup blackScreen;
    public CanvasGroup whiteScreen;
    public GameObject pussyMode;
    public Slider hpSlider;
    [SerializeField] GameObject bossSlider;
    public float timeToFadeIn;
    public float timeToFadeOut;
    //SCRIPTS   
    [SerializeField] Mole_Health health;
    [SerializeField] PlayerHealth playerHealth;
    //private
    bool fadeOut = false;
    bool fadeIn = false;
    bool takeBossHealth = false;
    float timer = 0f;
    void Start()
    {
        hpSlider.value = hpSlider.minValue;
    }
    void Update()
    {
        if (health.pussyModeOn)
        {
            pussyMode.SetActive(true);
        }
        else pussyMode.SetActive(false);
        if (hpSlider.enabled)
        {
            if (takeBossHealth) hpSlider.value = health.BossHealth;
            else
            {
                hpSlider.value = hpSlider.value + Time.deltaTime * 16.667f;
                if (hpSlider.value == hpSlider.maxValue) takeBossHealth = true;
            }
        }
        if (fadeIn)
        {
            if (whiteScreen.alpha < 1f)
            {
                float newAlpha = 0 + (timer / timeToFadeIn);
                timer += Time.deltaTime;
                whiteScreen.alpha = newAlpha;
            }
            else fadeIn = false;
        }
        if (fadeOut)
        {
            if (blackScreen.alpha != 0)
            {
                float newAlpha = 1 - (timer / timeToFadeOut);
                timer += Time.deltaTime;
                blackScreen.alpha = newAlpha;
            }
            else fadeOut = false;
        }
    }
    public void BossHPSliderStart() 
    {
        hpSlider.enabled = true;
    }
    public void BossHPSliderDestroy() { hpSlider.enabled = false; }

    public void FadeOutEffect()
    {
        timer = 0f;
        blackScreen.alpha = 1f;
        fadeOut = true;
    }

    public void FadeInEffect()
    {
        timer = 0f;
        whiteScreen.alpha = 0f;
        fadeIn = true;
    }

}
