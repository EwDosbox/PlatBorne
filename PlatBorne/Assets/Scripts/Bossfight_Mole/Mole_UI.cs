using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Mole_UI : MonoBehaviour
{
    //UNITY
    public Text pussyMode;
    public Slider hpSlider;
    [SerializeField] GameObject bossSlider;
    private bool takeBossHealth = false;
    //SCRIPTS   
    [SerializeField] Mole_Health health;
    void Start()
    {
        if (PlayerPrefs.HasKey("PussyMode"))
        {
            pussyMode.text = "PussyMode Active";
        }
        else pussyMode.text = "";
        hpSlider.value = hpSlider.minValue;
    }
    void Update()
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
    public void BossHPSliderStart() 
    {
        hpSlider.enabled = true;
    }
    public void BossHPSliderDestroy() { hpSlider.enabled = false; }
}
