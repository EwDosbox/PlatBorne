using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{

    public Slider slider;
    [SerializeField] private float speedToFill = 10; //60/5 = 12
    private bool start = true;
    private float timer = 0;

    public void Max(float maxHealth)
    {
        slider.maxValue = maxHealth;
        return;
    }
    
    public void Set(float health)
    {
        slider.value = health;
    }

    public void BossStart()
    {
        slider.value = timer * speedToFill;
        if (slider.maxValue == slider.value) start = false;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (start) BossStart();
    }
}
