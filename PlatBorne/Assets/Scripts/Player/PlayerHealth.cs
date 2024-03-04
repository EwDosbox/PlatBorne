using System.Collections;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class PlayerHealth : MonoBehaviour
{
    public GameObject hp1;
    public GameObject hp2;
    public GameObject hp3;
    public GameObject hp1_GodMode;
    public GameObject hp2_GodMode;
    public GameObject hp3_GodMode;
    public AudioSource hunterGodMode;
    private bool godMode;
    private bool pussyMode;
    private bool playerInvincible;
    private bool uiStart = false;
    private bool uiEnd = false;
    private float timer;
    private int playerHP = 3;
    private float timerInvincibility;

    public bool GodMode
    {
        set 
        {             
            if (value == true)
            {
                hunterGodMode.Play();
            }
            godMode = value;
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
            if (value <= 3 && value >= 0)
            {
                playerHP = value;
            }
        }
        get { return playerHP; }
    }

    public bool PlayerInvincible
    {
        get { return playerInvincible; }
        set { playerInvincible = value; }
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
        hp1.SetActive(false);
        hp2.SetActive(false);
        hp3.SetActive(false);
        hp1_GodMode.SetActive(false);
        hp2_GodMode.SetActive(false);
        hp3_GodMode.SetActive(false);
    }
    public void PlayerDamage()
    {
        if (!playerInvincible)
        {
            playerHP--;
            PlayerInvincible = true;
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

    public void StartHPUI()
    {
        uiStart = true;
    }

    private void Update()
    {
        if (playerInvincible)
        {
            timerInvincibility += Time.deltaTime;
            if (timerInvincibility > 2)
            {
                playerInvincible = false;
                timerInvincibility = 0;
            }
        }
        if (uiStart)
        {
            timer += Time.deltaTime;
            if (timer > 2)
            {
                if (godMode) hp1_GodMode.SetActive(true);
                else hp1.SetActive(true);
            }
            if (timer > 4)
            {
                if (godMode) hp2_GodMode.SetActive(true);
                else hp2.SetActive(true);
            }
            if (timer > 6)
            {                
                if (godMode) hp3_GodMode.SetActive(true);
                else hp3.SetActive(true);
                uiEnd = true;
            }            
        } //HP START
        else if (uiEnd)
        {
            switch (playerHP)
            {
                default:
                    {
                        if (godMode)
                        {
                            hp1_GodMode.SetActive(true);
                            hp2_GodMode.SetActive(true);
                            hp3_GodMode.SetActive(true);
                        }
                        else
                        {
                            hp1_GodMode.SetActive(true);
                            hp2_GodMode.SetActive(true);
                            hp3_GodMode.SetActive(true);
                            hp1.SetActive(true);
                            hp2.SetActive(true);
                            hp3.SetActive(true);
                        }
                        break;
                    }
                case 2:
                    {
                        if (godMode)
                        {
                            hp1_GodMode.SetActive(true);
                            hp2_GodMode.SetActive(true);
                            hp3_GodMode.SetActive(false);
                        }
                        else
                        {
                            hp1_GodMode.SetActive(false);
                            hp2_GodMode.SetActive(false);
                            hp3_GodMode.SetActive(false);
                            hp1.SetActive(true);
                            hp2.SetActive(true);
                            hp3.SetActive(false);
                        }
                        break;
                    }
                case 1:
                    {
                        if (godMode)
                        {
                            hp1_GodMode.SetActive(true);
                            hp2_GodMode.SetActive(false);
                            hp3_GodMode.SetActive(false);
                        }
                        else
                        {
                            hp1_GodMode.SetActive(false);
                            hp2_GodMode.SetActive(false);
                            hp3_GodMode.SetActive(false);
                            hp1.SetActive(true);
                            hp2.SetActive(false);
                            hp3.SetActive(false);
                        }
                        break;
                    }
                case 0:
                    {
                        if (godMode)
                        {
                            hp1_GodMode.SetActive(false);
                            hp2_GodMode.SetActive(false);
                            hp3_GodMode.SetActive(false);
                        }
                        else
                        {
                            hp1_GodMode.SetActive(false);
                            hp2_GodMode.SetActive(false);
                            hp3_GodMode.SetActive(false);
                            hp1.SetActive(false);
                            hp2.SetActive(false);
                            hp3.SetActive(false);
                        }
                        break;
                    }
            }
        }
    }
}
