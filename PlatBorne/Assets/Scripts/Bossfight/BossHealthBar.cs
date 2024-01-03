using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{

    public Slider slider;
    public float speedToFill = 10;
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
        slider.value = this.bossHP;
    }

    public void Slider()
    {
        for (float timer = 0; slider.value != slider.maxValue; timer += Time.deltaTime) 
        {
            slider.value = timer * speedToFill;           
        }        
    }

    public void LastPhase()
    {
        for (float timer = 0; this.bossHP != 0; timer = Time.deltaTime)
        {
            if (timer >= 1) this.bossHP--;
        }
    }
}
