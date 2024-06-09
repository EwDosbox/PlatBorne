using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Mole_Health : MonoBehaviour
{
    //INSPECTOR//
    [Tooltip("MAIN")]
    [SerializeField] private int playerDamage = 10;
    [SerializeField] private float invincibilityTimer = 5;
    [Tooltip("OBJECTS")]
    //PUBLIC//
    public bool bossDead = false;
    public bool pussyModeOn = false;
    //PRIVATE//
    private int bossHealth = 100;
    private bool bossInvincible = true;
    private float timer = 0;
    public int BossHealth
    {
        get { return bossHealth; }
        set 
        { 
            if (value <= 0 && value >= 100) bossHealth = value; 
        }
    }
    public bool BossInvincible
    {
        get { return bossInvincible; }
        set { bossInvincible = value; }
    }

    public void BossHit()
    {
        if (!bossInvincible)
        {
            bossHealth -= playerDamage;
            BossInvincible = true;
        }
        else
        {
            PlayerHealth playerHealth = new PlayerHealth();
            playerHealth.PlayerDamage();
        }
    }

    private void Update()
    {
        if (BossInvincible)
        {
            timer += Time.deltaTime;
            if (timer > invincibilityTimer)
            {
                bossInvincible = false;
            }
        }
        else timer = 0;
    }

    public void BossDeath()
    {
        Mole_UI ui = new Mole_UI();
        ui.BossHPSliderDestroy();
        bossInvincible = false;
        bossHealth = 0;
        bossDead = true;
    }
}
