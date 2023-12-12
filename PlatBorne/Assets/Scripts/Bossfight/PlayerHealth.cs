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
    private int playerHP = 0;
    private bool godMode = false;
    private bool pussyMode = false;

    public void SetHP(int playerHP) { this.playerHP = playerHP; }
    public int GetHP() { return this.playerHP; }
    public void SetGodMode(bool godMode) {  this.godMode = godMode; }
    public void SetPussyMode(bool pussyMode) {  this.pussyMode = pussyMode; }

    public void PlayerStart()
    {
        for (float timer = 0; timer < 5; timer += Time.deltaTime)
        {
            if (godMode)
            {
                if (timer > 1) hp1_GodMode.enabled = true;
                if (timer > 3) hp2_GodMode.enabled = true;
                if (timer > 5) hp3_GodMode.enabled = true;
            }
            else
            {
                if (timer > 1) hp1.enabled = true;
                if (timer > 3) hp2.enabled = true;
                if (timer > 5) hp3.enabled = true;
            }
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
        switch (playerHP)
        {
            default:
                {
                    if (godMode)
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
                    if (godMode)
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
                    if (godMode)
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
                    if (godMode)
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
}
