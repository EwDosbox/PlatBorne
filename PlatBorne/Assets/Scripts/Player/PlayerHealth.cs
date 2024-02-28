using System.Collections;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Image hp1;
    public Image hp2;
    public Image hp3;
    public Image hp1_GodMode;
    public Image hp2_GodMode;
    public Image hp3_GodMode;
    public AudioSource hunterGodMode;
    private int playerHP = 3;
    private bool godMode;
    private bool pussyMode;
    private bool playerInvincible;

    IEnumerator InvincibleWait()
    {
        yield return new WaitForSeconds(3);
        playerInvincible = true;
    }

    public bool GodMode
    {
        set 
        {             
            if (value == true)
            {
                hunterGodMode.Play();
            }
            godMode = value;
            HPChanged();
        }
        get { return godMode; }
    }

    public bool PussyMode
    {
        set { pussyMode = value; }
        get { return godMode;  }
    }
    public int PlayerHP
    {
        set 
        {
            if (value <= 3 && value >= 0 && !godMode)
            {
                playerHP = value;
                HPChanged();
            }
        }
        get { return playerHP; }
    }

    public bool PlayerInvincible
    {
        get { return playerInvincible; }
        set 
        { 
            if (value == true) StartCoroutine(InvincibleWait());
            playerInvincible = value; 
        }
    }
    private void Start()
    {
        if (PlayerPrefs.HasKey("PussyMode"))
        {
            int numberPussy = (PlayerPrefs.GetInt("PussyMode"));
            if (numberPussy == 1)
            {
                pussyMode = true;
            }
            else pussyMode = false;
        }

        if (PlayerPrefs.HasKey("GodMode"))
        {
            int numberGod = (PlayerPrefs.GetInt("GodMode"));
            if (numberGod == 1)
            {
                godMode = true;
            }
            else godMode = false;
        }
        hp1.enabled = false;
        hp2.enabled = false;
        hp3.enabled = false;
        hp1_GodMode.enabled = false;
        hp2_GodMode.enabled = false;
        hp3_GodMode.enabled = false;
    }
    public void PlayerDamage()
    {
        if (!playerInvincible)
        {
            playerHP--;
            PlayerInvincible = true;
            HPChanged();
        }
    }

    public void PlayerDeath(int boss)
    {
        if (boss == 1)
        {
            int ded = PlayerPrefs.GetInt("NumberOfDeath", 0);
            ded++;
            PlayerPrefs.SetInt("NumberOfDeath", ded);
            SceneManager.LoadScene("PlayerDeath");
        }
        else if (boss == 2)
        {
            int ded = PlayerPrefs.GetInt("NumberOfDeath_Mole", 0);
            ded++;
            PlayerPrefs.SetInt("NumberOfDeath_Mole", ded);
            SceneManager.LoadScene("PlayerDeath");
        }
    }
    public IEnumerator PlayerHPStart()
    {
        playerHP = 3;
        yield return new WaitForSeconds(2f);
        if (godMode) hp1_GodMode.enabled = true;
        else hp1.enabled = true;
        yield return new WaitForSeconds(2f);
        if (godMode) hp2_GodMode.enabled = true;
        else hp2.enabled = true;
        yield return new WaitForSeconds(2f);
        if (godMode) hp3_GodMode.enabled = true;
        else hp3.enabled = true;
    }

    private void HPChanged()
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
                        hp1_GodMode.enabled = false;
                        hp2_GodMode.enabled = false;
                        hp3_GodMode.enabled = false;
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
                        hp1_GodMode.enabled = false;
                        hp2_GodMode.enabled = false;
                        hp3_GodMode.enabled = false;
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
                        hp1_GodMode.enabled = false;
                        hp2_GodMode.enabled = false;
                        hp3_GodMode.enabled = false;
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
                        hp1_GodMode.enabled = false;
                        hp2_GodMode.enabled = false;
                        hp3_GodMode.enabled = false;
                        hp1.enabled = false;
                        hp2.enabled = false;
                        hp3.enabled = false;
                    }
                    break;
                }
        }
    }
}
