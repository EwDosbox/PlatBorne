using System.Collections;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class PlayerHealth : MonoBehaviour
{
    public GameObject[] hp;
    public GameObject[] hpGodMode;
    public AudioSource hunterGodMode;
    public AudioSource hunterDamage01;
    private bool godMode;
    private bool pussyMode;
    private bool playerInvincible;
    private bool uiStart = false;
    private bool uiEnd = false;
    private float timer = 0;
    private int playerHP = 3;
    private float timerInvincibility = 0;

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
        if (SceneManager.GetActiveScene().buildIndex == 3 || SceneManager.GetActiveScene().buildIndex == 7) //bossfights
        {

            if (PlayerPrefs.HasKey("GodMode"))
            {
                int numberGod = (PlayerPrefs.GetInt("GodMode"));
                if (numberGod == 1)
                {
                    godMode = true;
                }
                else godMode = false;
            }
            foreach (GameObject temp in hp)
            {
                temp.SetActive(false);
            }
            foreach (GameObject temp in hpGodMode)
            {
                temp.SetActive(false);
            }
        }
    }
    public void PlayerDamage()
    {
        if (!playerInvincible && !godMode)
        {
            playerHP--;
            HunterDamageVL();
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
        if (PlayerInvincible)
        {
            timerInvincibility += Time.deltaTime;
            if (timerInvincibility > 1f)
            {
                PlayerInvincible = false;
            }
        }
        else timerInvincibility = 0;
        if (uiStart && !uiEnd)
        {
            timer += Time.deltaTime;
            if (timer > 2 && playerHP >= 1)
            {
                if (godMode) hpGodMode[0].SetActive(true);
                hp[0].SetActive(true);
            }
            if (timer > 4 && playerHP >= 2)
            {
                if (godMode) hpGodMode[1].SetActive(true);
                hp[1].SetActive(true);
            }
            if (timer > 6 && playerHP == 3)
            {                
                if (godMode) hpGodMode[2].SetActive(true);
                hp[2].SetActive(true);
                uiEnd = true;
            }            
            else if (timer > 6) uiEnd = true;
        }
        else if (uiEnd)
        {
            if (playerHP >= 1)
            {
                if (godMode) hpGodMode[0].SetActive(true);
                else hpGodMode[0].SetActive(false);
                hp[0].SetActive(true);
            }
            else
            {
                if (godMode) hpGodMode[0].SetActive(false);
                hp[0].SetActive(false);
            }
            if (playerHP >= 2)
            {
                if (godMode) hpGodMode[1].SetActive(true);
                else hpGodMode[1].SetActive(false);
                hp[1].SetActive(true);
            }
            else
            {
                if (godMode) hpGodMode[1].SetActive(false);
                hp[1].SetActive(false);
            }
            if (playerHP >= 3)
            {
                if (godMode) hpGodMode[2].SetActive(true);
                else hpGodMode[2].SetActive(false);
                hp[2].SetActive(true);
            }
            else
            {
                if (godMode) hpGodMode[2].SetActive(false);
                hp[2].SetActive(false);
            }
        }
    }

    void HunterDamageVL()
    {
        hunterDamage01.Play(); //dont ask
    }
}
