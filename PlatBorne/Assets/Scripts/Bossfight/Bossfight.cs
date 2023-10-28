using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Bossfight : MonoBehaviour
{
    [SerializeField] private AudioSource PreBossDialog;
    [SerializeField] private AudioSource BossDamage01;
    [SerializeField] private AudioSource BossDamage02;
    [SerializeField] private AudioSource BossDamage03;
    [SerializeField] private AudioSource BossDamage04;
    [SerializeField] private AudioSource BossDeath01;
    [SerializeField] private AudioSource HunterOOF;

    public BossHealthBar bossHealthBar;
    public PlayerHealth playerHealth;
    public BossAttacks bossAttacks;
    public PlayerScript player;
    public BossFightVoiceLines voiceLines;

    public int phase = 1;
    bool attackIsGoing = false;
    public int playerHP = 3;
    public int bossHP = 60;
    bool bossInvincible = true;
    float timer = 0f;
    bool timerOn = true;
    float phaseTimer = 0f;
    float invincibilityTimerBoss = 0f;
    float invincibilityTimerPlayer = 0f;
    bool playerInvincible = false;

    public bool bossHitboxRight = false;
    public bool bossHitboxLeft = false;
    public bool bossHitboxUp = false;
    public bool bossHitboxDown = false;
    public bool bossHitbox = false;
    public bool bossfightStarted = false;


    private void BossDeath()
    {
        BossDeath01.Play();
        Debug.Log("Boss Has Died");
    }
    private void PlayerDeath()
    {
        SceneManager.LoadScene("PlayerDeath");
        Debug.Log("Player Death Scene");
    }

    private void Start()
    {
        //Debug
    }
    private void Update()
    {
        if (bossHitboxLeft && !bossfightStarted) //Start of Bossfight - UI inicialization
        {
            bossfightStarted = true;
            bossHealthBar.BossStart();
            playerHealth.PlayerStart();
            voiceLines.PlayPreBossDialog();
        }

        if (bossfightStarted)
        {
            //ÈASOVAÈ
            if (timerOn)
            {
                timer += Time.fixedDeltaTime;
                Debug.Log(timer);
                if (bossInvincible) invincibilityTimerBoss += Time.fixedDeltaTime;
                if (playerInvincible) invincibilityTimerPlayer += Time.fixedDeltaTime;
                if (phase == 4) phaseTimer += Time.fixedDeltaTime;
            }

            if (invincibilityTimerBoss >= 60 && !attackIsGoing)
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
                    bossHealthBar.Set(bossHP); //UI
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
            else if (phase == 4 && (phaseTimer % 2 == 0 || phaseTimer % 2 != 0))
            {
                bossHP--;
                bossHealthBar.Set(bossHP); //UI
                if (bossHP <= 0) BossDeath();
            }
            if (bossHitbox && bossInvincible)
            {
                playerHP--;
                playerHealth.ChangeHealth(playerHP); //UI
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
}
