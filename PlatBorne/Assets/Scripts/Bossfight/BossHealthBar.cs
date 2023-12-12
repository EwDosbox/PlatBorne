using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{

    public Slider slider;
    public float speedToFill = 10;
    private bool isFilled = false;
    private int bossHP = 0;

    public void SetHP(int bossHP) { this.bossHP = bossHP; }
    public int GetHP() {  return this.bossHP; }
    public void Max(float maxHealth)
    {
        slider.maxValue = maxHealth;
        return;
    }
    
    private void Update()
    {
        slider.value = bossHP;
    }

    public void Slider()
    {
        for(float timer = 0; !isFilled; timer = Time.deltaTime) 
        {
            slider.value = timer * speedToFill;
            if (slider.maxValue == slider.value) isFilled = true;            
        }        
    }

    public void LastPhase()
    {
        for (float timer = 0; bossHP != 0; timer = Time.deltaTime)
        {
            if (timer >= 1) bossHP--;
        }
    }
}
