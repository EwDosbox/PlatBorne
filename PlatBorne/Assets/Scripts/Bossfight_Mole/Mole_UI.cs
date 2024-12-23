using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Mole_UI : MonoBehaviour
{
    // UNITY
    public CanvasGroup whiteScreen;
    public GameObject pussyMode;
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
    }

    void Update()
    {
        if (health.pussyModeOn)
        {
            pussyMode.SetActive(true);
        }
        else
        {
            pussyMode.SetActive(false);
        }

        if (hpSliderOn)
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
                    if (hpSlider.value >= hpSlider.maxValue) takeBossHealth = true;
                }
            }
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
