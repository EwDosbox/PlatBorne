using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{

    public Slider slider;
    public float speedToFill = 10;
    private int bossHP = 0;
    bool consistentDamage = false;
    bool sliderHealth = false;
    private float timer = 0;

    public void SetHP(int bossHP) { this.bossHP = bossHP; }
    public int GetHP() {  return this.bossHP; }
    public void Max(float maxHealth)
    {
        slider.maxValue = maxHealth;
        return;
    }
    
    private void Update()
    {
        if (consistentDamage && slider.value == slider.maxValue)
        {
            timer += Time.deltaTime;
            if (timer >= 1)
            {
                bossHP--;
                timer -= 1;
                if (bossHP == 0) consistentDamage = false;
            }
        }
        if (sliderHealth)
        {
            if (slider.value != slider.maxValue)
            {
                slider.value += speedToFill * Time.deltaTime;
            }
            else sliderHealth = false;
        }
        else slider.value = this.bossHP;
    }

    public void Slider()
    {
        sliderHealth = true;
        slider.value = float.MinValue;
        this.bossHP = 60;
    }

    public void LastPhase()
    {
        consistentDamage = true;
    }
}
