using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;


public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private AudioSource PreBossDialog;
    [SerializeField] private AudioSource BossDamage01;
    [SerializeField] private AudioSource BossDamage02;
    [SerializeField] private AudioSource BossDamage03;
    [SerializeField] private AudioSource BossDamage04;
    [SerializeField] private AudioSource BossDeath01;
    [SerializeField] private AudioSource HunterOOF;

    public int phase = 1;
    bool attackIsGoing = false;
    int playerHP = 3;
    int bossHP = 60;
    bool bossInvincible = true;
    float timerSecondary = 0f;
    bool timerSecondaryOn = false;
    float timer = 0f;
    bool timerOn = true;
    float phaseTimer = 0f;
    float invincibilityTimerBoss = 0f;
    float invincibilityTimerPlayer = 0f;
    float[] invincibilityPhaseTimerBoss = { 50, 60, 45 };
    bool playerInvincible = false;

    bool bossHitboxRight = false;
    bool bossHitboxLeft = false;
    bool bossHitboxUp = false;
    bool bossHitboxDown = false;
    bool bossHitbox = false;
    bool arenaStart = false;

    private void BossDeath()
    {
        BossDeath01.Play();
        Debug.Log("Boss Has Died");
    }
    private void BossAttackRushPlayer()
    {

    }
    private void BossAttackFloorIsLava()
    {

    }
    private void BossAttackDagger()
    {

    }
    private void BossAttackSword()
    {

    }
    private void BossAttackLeechLeft()
    {

    }
    private void BossAttackLeechRight()
    {

    }
    private void BossAttackLeechBoth()
    {

    }
    private void PlayerDeath()
    {
        SceneManager.LoadScene("PlayerDeath");
        Debug.Log("Player Death Scene");
    }

    private void TimerSecondaryReset()
    {
        timerSecondary = 0;
        timerSecondaryOn = false;
    }
    private void BossAttackChoose()
    {
        switch (phase)
        {
            case 1:
                {
                    timerSecondaryOn = true;
                    if (timerSecondary >= 4) //wait 4s
                    {
                        TimerSecondaryReset();
                        if (bossHitboxRight) // + boss na leve strane
                        {
                            BossAttackRushPlayer();
                        }
                        else if (bossHitboxLeft) // + boss na prave strane
                        {
                            BossAttackRushPlayer();
                        }
                        else if (bossHitboxDown)
                        {
                            BossAttackFloorIsLava();
                        }
                        else
                        {
                            BossAttackDagger();
                        }
                    }
                    break;
                }
            case 2:
                {
                    timerSecondaryOn = true;
                    if (timerSecondary >= 3) //wait 3s
                    {
                        TimerSecondaryReset();
                        if (bossHitboxRight) // + boss na leve strane
                        {
                            BossAttackRushPlayer();
                        }
                        else if (bossHitboxLeft) // + boss na prave strane
                        {
                            BossAttackRushPlayer();
                        }
                        else if (bossHitboxLeft)
                        {
                            BossAttackSword();
                        }
                        else
                        {
                            BossAttackDagger();
                        }
                    }
                    break;
                }
            case 3:
                {
                    if (timerSecondary >= 2) //wait 2s
                    {
                        TimerSecondaryReset();
                        if (bossHitboxRight) // + boss na leve strane
                        {
                            BossAttackRushPlayer();
                        }
                        else if (bossHitboxLeft) // + boss na prave strane
                        {
                            BossAttackRushPlayer();
                        }
                        else if (bossHitboxLeft)
                        {
                            BossAttackSword();
                        }
                        else if (bossHitboxDown)
                        {
                            BossAttackFloorIsLava();
                        }
                        else
                        {
                            BossAttackDagger();
                        }
                    }
                    break;
                }
            case 4:
                {
                    if (timerSecondary >= 1) //wait 1s
                    {
                        TimerSecondaryReset();
                        if (bossHitboxRight) // + boss na leve strane
                        {
                            BossAttackRushPlayer();
                        }
                        else if (bossHitboxLeft) // + boss na prave strane
                        {
                            BossAttackRushPlayer();
                        }
                        else if (bossHitboxLeft)
                        {
                            BossAttackSword();
                        }
                        else if (bossHitboxDown)
                        {
                            BossAttackFloorIsLava();
                        }
                        else
                        {
                            BossAttackDagger();
                        }
                        // BossAttackLeech();
                    }
                    break;
                }
        }
    }

    private void Start()
    {
        PlayerScript playerScript = GetComponent<PlayerScript>();
        bool bossHitboxRight = playerScript.bossHitboxRight;
        bool bossHitboxLeft = playerScript.bossHitboxLeft;
        bool bossHitboxUp = playerScript.bossHitboxUp;
        bool bossHitboxDown = playerScript.bossHitboxDown;
        bool bossHitbox = playerScript.bossHitbox;
        bool arenaStart = playerScript.arenaStart;
    }
    private void Update()
    {
        //ÈASOVAÈ
        if (timerOn)
        {
            timer += Time.fixedDeltaTime;
            if (timerSecondaryOn) timerSecondary += Time.fixedDeltaTime;
            if (bossInvincible) invincibilityTimerBoss += Time.fixedDeltaTime;
            if (playerInvincible) invincibilityTimerPlayer += Time.fixedDeltaTime;
            if (phase == 4) phaseTimer += Time.fixedDeltaTime;
        }

        if (invincibilityTimerBoss > invincibilityPhaseTimerBoss[phase - 1])
        {
            invincibilityTimerBoss = 0;
            bossInvincible = false;
        }
        if (invincibilityTimerPlayer > 1)
        {
            playerInvincible = false;
            invincibilityTimerPlayer = 0;
        }

        if (!bossInvincible && phase <= 3) //Phase 1,2,3 Boss Damage
        {
            if (bossHitbox) 
            {
                bossInvincible = true;
                playerInvincible = true;
                bossHP -= 20;
                switch (bossHP)
                {
                    case 40:
                        {
                            phase = 2;
                            BossDamage02.Play();
                            Debug.Log("Phase 2 Start");
                            break;
                        }
                    case 20:
                        {
                            phase = 3;
                            BossDamage03.Play();
                            Debug.Log("Phase 3 Start");
                            break;
                        }
                }
                if (bossHP == 0 && phase == 3)
                {
                    BossDamage04.Play();
                    Debug.Log("Phase 4 Start");
                    bossHP = 60;
                    phase = 4;
                }
            }
        }
        else if (phase == 4 && ((phaseTimer * 1000) % 1000 == 0)) //Boss Damage Counter Phase 4
        {
            bossHP--;
            if (bossHP <= 0) BossDeath();
        }
        if (bossHitbox && bossInvincible)
        {
            playerHP--;
            HunterOOF.Play();
            Debug.Log("Hunter has taken Damage");
            if (playerHP == 0)
            {
                Debug.Log("Hunter has Died");
                PlayerDeath();
            }
        }

    }
}
