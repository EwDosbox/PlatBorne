using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{

    public Slider slider;

    public void Max(int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
        return;
    }
    
    public void Set(int health)
    {
        slider.value = health;
    }

    public void BossStart(int maxHealth)
    {
        slider.maxValue = maxHealth;
        for (double i = 0; i < maxHealth; i += 0.05)
        {
            slider.value = (float)i;
            new WaitForSeconds((float)i);    
        }
        return;
    }
}
