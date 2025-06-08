using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private int playerHP = 3;
    private float timerInvincibility = 0;
    private float timeToUnInvincible = 1f;

    public bool GodMode
    {
        set 
        {             
            if (value == true && !godMode) //do not play sound when GodMode is already on
            {
                PlayerPrefs.SetInt("GodMode", 69);
                godMode = true;
                hunterGodMode.Play();
            }
            else
            {
                PlayerPrefs.SetInt("GodMode", 0);
                godMode = false;
            }                
            PlayerPrefs.Save();
            UpdateHearts();
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
            UpdateHearts();
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
                if (numberGod == 0) GodMode = false;
                else GodMode = true;
            }
            ResetHearts();
        }
        else this.gameObject.SetActive(false);
    }
    public void PlayerDamage()
    {
        if (!playerInvincible && !godMode)
        {
            PlayerHP--;
            HunterDamageVL();
            PlayerInvincible = true;
        }
    }

    public void SetInvincibleTimer(float time)
    {
        timeToUnInvincible = time;
        playerInvincible = true;
    }

    public void PlayerDeath(int boss)
    {
        if (boss == 1)
        {
            int ded = PlayerPrefs.GetInt("NumberOfDeath", 0);
            ded++;
            PlayerPrefs.SetInt("NumberOfDeath", ded);
        }
        else if (boss == 2)
        {
            int ded = PlayerPrefs.GetInt("NumberOfDeath_Mole", 0);
            ded++;
            PlayerPrefs.SetInt("NumberOfDeath_Mole", ded);
        }
        PlayerPrefs.Save();
        SceneManager.LoadScene("PlayerDeath");
    }

    private void Update()
    {
        if (PlayerInvincible)
        {
            timerInvincibility += Time.deltaTime;
            if (timerInvincibility > timeToUnInvincible)
            {
                PlayerInvincible = false;
                timeToUnInvincible = 1f;
            }
        }
        else timerInvincibility = 0;        
    }

    void HunterDamageVL()
    {
        hunterDamage01.Play(); //dont ask
    }

    public void StartHPUI()
    {
        StartCoroutine(HPStart());
    }

    IEnumerator HPStart()
    {
        yield return new WaitForSeconds(2f);
        if (playerHP >= 1)
        {
            if (godMode) hpGodMode[0].SetActive(true);
            hp[0].SetActive(true);
        }

        yield return new WaitForSeconds(2f);
        if (playerHP >= 2)
        {
            if (godMode) hpGodMode[1].SetActive(true);
            hp[1].SetActive(true);
        }

        yield return new WaitForSeconds(2f);
        if (playerHP >= 3)
        {
            if (godMode) hpGodMode[2].SetActive(true);
            hp[2].SetActive(true);
        }
    }

    private void UpdateHearts()
    {
        ResetHearts();
        if (playerHP >= 1)
        {
            if (godMode) hpGodMode[0].SetActive(true);
            hp[0].SetActive(true);
        }
        if (playerHP >= 2)
        {
            if (godMode) hpGodMode[1].SetActive(true);
            hp[1].SetActive(true);
        }
        if (playerHP >= 3)
        {
            if (godMode) hpGodMode[2].SetActive(true);
            hp[2].SetActive(true);
        }
    }

    private void ResetHearts()
    {
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
