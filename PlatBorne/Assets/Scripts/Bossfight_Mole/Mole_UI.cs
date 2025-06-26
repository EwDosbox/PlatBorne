using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//The fuck is even this script man, I must have been high while  writing a new script just for this

public class Mole_UI : MonoBehaviour 
{
    // UNITY
    public CanvasGroup whiteScreen;
    public Slider hpSlider;
    bool hpSliderOn = false;
    [SerializeField] GameObject bossSlider;

    // SCRIPTS   
    [SerializeField] Mole_Health health;
    [SerializeField] PlayerHealth playerHealth;

    // private
    bool takeBossHealth = false;

    void Start()
    {
        hpSlider.value = hpSlider.minValue;
        StartCoroutine(AnimateBossHpSlider());
    }

    IEnumerator AnimateBossHpSlider()
    {
        while (hpSliderOn)
        {
            if (hpSlider.enabled)
            {
                if (takeBossHealth)
                {
                    hpSlider.value = health.BossHealth;
                }
                else
                {
                    hpSlider.value += Time.deltaTime * 16.667f;
                    if (hpSlider.value >= hpSlider.maxValue)
                    {
                        hpSlider.value = hpSlider.maxValue;
                        takeBossHealth = true;
                    }
                }
            }
            yield return null;
        }
    }


    public void BossHPSliderStart()
    {
        hpSlider.enabled = true;
        hpSliderOn = true;
    }

    public void BossHPSliderDestroy()
    {
        hpSlider.enabled = false;
        hpSliderOn = false;
    }

    public void FadeOutEffect(float timeToFadeOut)
    {
        StartCoroutine(FadeOutCoroutine(timeToFadeOut));
    }

    private IEnumerator FadeOutCoroutine(float timeToFadeOut)
    {
        float elapsed = 0f;
        whiteScreen.alpha = 0f;

        while (elapsed < timeToFadeOut)
        {
            whiteScreen.alpha = Mathf.Lerp(0f, 1f, elapsed / timeToFadeOut);
            elapsed += Time.deltaTime;
            yield return null;
        }

        whiteScreen.alpha = 1f;
    }
}
