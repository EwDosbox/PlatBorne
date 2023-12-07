using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Image hp1;
    public Image hp2;
    public Image hp3;
    public Image hp1_GodMode;
    public Image hp2_GodMode;
    public Image hp3_GodMode;
    public Bossfight bossfight;
    private float timer = 0f;
    private bool kemoMilujuANenavidimTimery = false;
    public void ChangeHealth(bool isGodModeActive)
    {
        Debug.Log(Bossfight.godMode);
        switch (Bossfight.playerHP)
        {
            default:
                {
                    if (Bossfight.godMode)
                    {
                        hp1_GodMode.enabled = true;
                        hp2_GodMode.enabled = true;
                        hp3_GodMode.enabled = true;
                    }
                    else
                    {
                        hp1.enabled = true;
                        hp2.enabled = true;
                        hp3.enabled = true;
                    }
                    break;
                }
            case 2:
                {
                    if (Bossfight.godMode)
                    {
                        hp1_GodMode.enabled = true;
                        hp2_GodMode.enabled = true;
                        hp3_GodMode.enabled = false;
                    }
                    else
                    {
                        hp1.enabled = true;
                        hp2.enabled = true;
                        hp3.enabled = false;
                    }
                    break;
                }
            case 1:
                {
                    if (Bossfight.godMode)
                    {
                        hp1_GodMode.enabled = true;
                        hp2_GodMode.enabled = false;
                        hp3_GodMode.enabled = false;
                    }
                    else
                    {
                        hp1.enabled = true;
                        hp2.enabled = false;
                        hp3.enabled = false;
                    }
                    break;
                }
            case 0:
                {
                    if (Bossfight.godMode)
                    {
                        hp1_GodMode.enabled = false;
                        hp2_GodMode.enabled = false;
                        hp3_GodMode.enabled = false;
                    }
                    else
                    {
                        hp1.enabled = false;
                        hp2.enabled = false;
                        hp3.enabled = false;
                    }
                    break;
                }
        }
    }

    public void PlayerStart()
    {
        kemoMilujuANenavidimTimery = true;
        Debug.Log("GodMode:" + Bossfight.godMode);
        if (Bossfight.godMode)
        {
            if (timer > 1) hp1_GodMode.enabled = true;
            if (timer > 3) hp2_GodMode.enabled = true;
            if (timer > 5)
            {
                hp3_GodMode.enabled = true;
                kemoMilujuANenavidimTimery = false;
            }
        }
        else
        {
            if (timer > 1) hp1.enabled = true;
            if (timer > 3) hp2.enabled = true;
            if (timer > 5)
            {
                hp3.enabled = true;
                kemoMilujuANenavidimTimery = false;
            }
        }
    }
    public void GodMode()
    {
        if (Bossfight.godMode)
        {
            if (hp1.enabled) hp1_GodMode.enabled = true;
            if (hp2.enabled) hp2_GodMode.enabled = true;
            if (hp3.enabled) hp3_GodMode.enabled = true;
        }
        else
        {
            hp1_GodMode.enabled = false;
            hp2_GodMode.enabled = false;
            hp3_GodMode.enabled = false;
        }
    }
    private void Start()
    {
        hp1.enabled = false;
        hp2.enabled = false;
        hp3.enabled = false;
        hp1_GodMode.enabled = false;
        hp2_GodMode.enabled = false;
        hp3_GodMode.enabled = false;
    }

    private void Update()
    {
        if (kemoMilujuANenavidimTimery)
        {
            timer += Time.deltaTime;
            PlayerStart();

        }
    }
}
