using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{

    public Slider slider;
    public float speedToFill = 10;
    private bool start = false;
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
        start = true;
        slider.value = timer * speedToFill;
        if (slider.maxValue == slider.value) start = false;
    }

    private void Update()
    {
        if (start) timer += Time.deltaTime;
        if (start) BossStart();
    }
}
