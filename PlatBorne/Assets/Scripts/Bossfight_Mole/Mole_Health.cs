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
    PlayerHealth playerHealth;
    //PUBLIC//
    public bool bossDead = false;
    //PRIVATE//
    private bool bossStayInvincible = false;
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
    public bool BossStayInvincible //for changing phase
    {
        get { return bossStayInvincible; }
        set { bossStayInvincible = value; }
    }

    public void BossHit(bool weakspotDamage)
    {
        if (!BossInvincible || weakspotDamage)
        {            
            Debug.Log("Boss has taken a damage from player");      
            bossHealth -= playerDamage;
            BossInvincible = true;
            playerHealth.PlayerInvincible = true;
        }
        else
        {
            Debug.Log("Player has taken a damage from boss");
            playerHealth.PlayerDamage();
        }
    }

    private void Update()
    {
        if (!BossStayInvincible && BossInvincible)
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

    private void Awake()
    {
        playerHealth = FindAnyObjectByType<PlayerHealth>();
    }
}
