using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mole_UI : MonoBehaviour
{
    //UNITY
    public Text pussyMode;
    public Slider hpSlider;
    [SerializeField] float sliderSpeedToFill = 20;
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
        if (takeBossHealth) hpSlider.value = health.BossHealth;        
    }
    public IEnumerator BossHPSliderStart() 
    {
        int value = 0;
        while(hpSlider.value != hpSlider.maxValue)
        {
            value++;
            hpSlider.value = value;
            yield return new WaitForSeconds(0.05f);
        }
        takeBossHealth = true;
    }
    public void BossHPSliderDestroy() { hpSlider.enabled = false; }
}
