using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Mole_Health : MonoBehaviour
{
    //INSPECTOR//
    [Tooltip("MAIN")]
    [SerializeField] private int playerDamage = 10;
    [SerializeField] private float invincibilityTimer = 3;
    [Tooltip("OBJECTS")]
    //PUBLIC//
    public bool bossDead = false;
    //PRIVATE//
    private int bossHealth = 100;
    private bool bossInvincible = false;
    private float timer = 0;
    public int BossHealth
    {
        get { return bossHealth; }
        set 
        { 
            if (value <= 0 && value >= 100) bossHealth = value; 
        }
    }

    public void BossHit()
    {
        if (!bossInvincible)
        {
            bossHealth -= playerDamage;
            bossInvincible = true;
        }
        else
        {
            PlayerHealth playerHealth = new PlayerHealth();
            playerHealth.PlayerDamage();
        }
    }

    private void Update()
    {
        if (bossInvincible)
        {
            timer += Time.deltaTime;
            if (timer > invincibilityTimer)
            {
                bossInvincible = false;
            }
        }
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
