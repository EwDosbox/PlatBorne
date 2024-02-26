using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mole_UI : MonoBehaviour
{
    //UNITY
    public Text pussyMode;
    public Slider hpSlider;
    [SerializeField] float sliderSpeedToFill;
    [SerializeField] GameObject bossSlider;
    //SCRIPTS   
    Mole_Health health = new Mole_Health();
    private bool sliderStart = false;
    void Start()
    {
        
        if (PlayerPrefs.HasKey("PussyMode"))
        {
            pussyMode.text = "PussyMode Active";
        }
        else pussyMode.text = "";
    }
    void Update()
    {
        if (sliderStart)
        {
            if (hpSlider.value == hpSlider.maxValue) sliderStart = false;
            else
            {
                hpSlider.value += sliderSpeedToFill * Time.deltaTime;
            }
        }
        else
        {
            hpSlider.value = health.BossHealth;
        }
    }
    public void BossHPSliderStart() { sliderStart = true; }
    public void BossHPSliderDestroy() { bossSlider.SetActive(false); }
}
