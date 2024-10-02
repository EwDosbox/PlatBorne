using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Mole_UI : MonoBehaviour
{
    //UNITY
    public CanvasGroup whiteScreen;
    public GameObject pussyMode;
    public Slider hpSlider;
    bool hpSliderOn = false;
    [SerializeField] GameObject bossSlider;
    public float timeToFadeOut;
    //SCRIPTS   
    [SerializeField] Mole_Health health;
    [SerializeField] PlayerHealth playerHealth;
    //private
    bool fadeOut = false;
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
        if (hpSliderOn)
        {
            if (hpSlider.enabled)
            {
                if (takeBossHealth) hpSlider.value = health.BossHealth;
                else
                {
                    hpSlider.value = hpSlider.value + Time.deltaTime * 16.667f;
                    if (hpSlider.value == hpSlider.maxValue) takeBossHealth = true;
                }
            }
        }
        if (fadeOut)
        {
            if (whiteScreen.alpha < 1f)
            {
                float newAlpha = 0 + (timer / timeToFadeOut);
                timer += Time.deltaTime;
                whiteScreen.alpha = newAlpha;
            }
            else fadeOut = false;
        }
    }
    public void BossHPSliderStart() 
    {
        hpSlider.enabled = true;
        hpSliderOn = true;
    }
    public void BossHPSliderDestroy() { hpSlider.enabled = false; hpSliderOn = false; }


    public void FadeOutEffect()
    {
        timer = 0f;
        whiteScreen.alpha = 0f;
        fadeOut = true;
    }

}
