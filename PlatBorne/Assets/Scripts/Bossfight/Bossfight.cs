using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Bossfight : MonoBehaviour
{
    [SerializeField] private AudioSource PreBossDialog;
    [SerializeField] private AudioSource BossDamage01;
    [SerializeField] private AudioSource BossDamage02;
    [SerializeField] private AudioSource BossDamage03;
    [SerializeField] private AudioSource BossDeath01;
    [SerializeField] private AudioSource OSTLoop;
    [SerializeField] private AudioSource OSTPhase4;

    public BossFightVoiceLines voiceLines;
    public BossHealthBar bossHealthBar;
    public PlayerHealth playerHealth;
    public BossAttacks attack;
    public GameObject player;
    private Rigidbody2D rb;
    public Text text;

    public int phase = 1;
    bool attackIsGoing = false;
    static public int playerHP = 3;
    public int bossHP = 60;
    bool bossInvincible = true;
    float timer = 0f;
    bool timerOn = true;
    float phaseTimer = 0f;
    float invincibilityTimerBoss = 0f;
    float invincibilityTimerPlayer = 0f;
    bool playerInvincible = false;
    private bool attackTimerBool = false;
    private float attackTimer = 5f;

    bool bossHitboxRight;
    bool bossHitboxLeft;
    bool bossHitboxUp;
    bool bossHitboxDown;
    bool bossHitbox;
    static public bool bossfightStarted = false;
    //****************
    int attackNumberRush = 0;
    int attackNumberDagger = 0;
    int attackNumberFloorIsLava = 0;
    int attackNumberLeech = 0;
    int attackNumberSword = 0;
    //****************

    private int resetPromenych()
    {
        attackNumberRush = 0;
        attackNumberDagger = 0;
        attackNumberFloorIsLava = 0;
        attackNumberLeech = 0;
        attackNumberSword = 0;
        return 1;
    }    
    private void BossDeath()
    {
        BossDeath01.Play();
        PlayerPrefs.SetFloat("Boss Time", timer);
        PlayerPrefs.Save();
        SceneManager.LoadScene("EndgameCutscene");
        Debug.Log("Boss Has Died");
    }
    public void PlayerDeath()
    {
        playerInvincible = true;
        int ded = PlayerPrefs.GetInt("NumberOfDeath", 0);
        ded++;
        PlayerPrefs.SetInt("NumberOfDeath", ded);
        PlayerPrefs.Save();
        Debug.Log("Hunter has Died");
        SceneManager.LoadScene("PlayerDeath");
    }

    private void Start()
    {
        voiceLines.PlayBossDamage01();
    }
    private void Update()
    {
        if (PlayerHealth.playerDeath) PlayerDeath(); 
        if (bossInvincible) text.text = "Boss Invincible";
        else text.text = "Boss Vunerable";
        if (PlayerScript.bossHitboxLeft && !bossfightStarted) //Start of Bossfight - UI inicialization
        {
            phase = 1;
            timerOn = true;
            bossfightStarted = true;
            attackTimerBool = true;
            bossHealthBar.BossStart();
            playerHealth.PlayerStart();
            PreBossDialog.Play();
            OSTLoop.enabled = true;
        }

        if (bossfightStarted)
        {
            //ÈASOVAÈ
            if (timerOn)
            {
                timer += Time.deltaTime;
                if (bossInvincible) invincibilityTimerBoss += Time.deltaTime;
                else invincibilityTimerBoss = 0f;
                if (playerInvincible) invincibilityTimerPlayer += Time.deltaTime;
                else invincibilityTimerPlayer = 0f;
                if (phase == 4) phaseTimer += Time.deltaTime;
                else phaseTimer = 0f;
                if (attackTimerBool) attackTimer += Time.deltaTime;
                else attackTimer = 0f;
            }

            if (invincibilityTimerBoss >= 60)
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
                if (PlayerScript.bossHitbox)
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
                                BossDamage01.Play();
                                Debug.Log("Phase 2 Start");
                                break;
                            }
                        case 20:
                            {
                                phase = 3;
                                BossDamage02.Play();
                                Debug.Log("Phase 3 Start");
                                break;
                            }
                    }
                    if (bossHP == 0 && phase == 3)
                    {
                        BossDamage03.Play();
                        Debug.Log("Phase 4 Start");
                        bossHP = 60;
                        phase = 4;
                        OSTLoop.enabled = false;
                        OSTPhase4.enabled = true;
                    }
                }
            }
            else if (phase == 4 && phaseTimer >= 1)
            {
                phaseTimer = 0;
                bossHP--;
                bossHealthBar.Set(bossHP); //UI
                if (bossHP <= 0) BossDeath();
            }
            if (PlayerScript.bossHitbox && bossInvincible)
            {
                playerInvincible = true;
                PlayerScript.bossHitbox = false;
                playerHP--;
                playerHealth.ChangeHealth(); //UI
                Debug.Log("Hunter has taken Damage");
                if (playerHP == 0)
                {
                    PlayerDeath();
                }
            }

            switch (phase)
            {
                case 1:
                    {
                        if (!bossInvincible)
                        {
                            //attack.PhaseAttack();
                        }
                        else if (attackTimer > 10)
                        {
                                /*if ((bossHitboxDown && rb.position.x > 6.60) && attackNumberRush < 1) //RushRight
                                {
                                    attack.BossAttackRushPlayer(true);
                                    attackNumberRush = resetPromenych();
                                }
                                else if ((bossHitboxDown && rb.position.x < -6.60) && attackNumberRush < 1) //RushLeft
                                {
                                    attack.BossAttackRushPlayer(false);
                                    attackNumberRush = attackNumberRush = resetPromenych();
                                }*/
                                if (PlayerScript.bossHitboxDown && attackNumberFloorIsLava < 1)
                                {
                                    Debug.Log("Lava");
                                    attack.BossAttackFloorIsLava();
                                    attackNumberFloorIsLava = resetPromenych();
                                }
                                else
                                {
                                    Debug.Log("Dagger");
                                    attack.BossAttackDagger();
                                    attackNumberDagger = resetPromenych();
                                }
                            attackTimer = 0f;
                        }                            
                        }
                        break;
                case 2:
                {
                    if (attackTimer > 8)
                    {
                        //if (!bossInvincible) attack.PhaseAttack();
                        //else
                        {
                            /*if ((bossHitboxDown && rb.position.x > 6.60) && attackNumberRush < 1) //RushRight
                            {
                                attack.BossAttackRushPlayer(true);
                                attackNumberRush = resetPromenych();
                            }
                            else if ((bossHitboxDown && rb.position.x < -6.60) && attackNumberRush < 1) //RushLeft
                            {
                                attack.BossAttackRushPlayer(false);
                                attackNumberRush = resetPromenych();
                            }*/
                            if (PlayerScript.bossHitboxDown && attackNumberFloorIsLava < 1)
                            {
                                attack.BossAttackFloorIsLava();
                                attackNumberFloorIsLava = resetPromenych();
                            }
                            else if (attackNumberLeech < 1)
                            {
                                if (PlayerScript.bossHitboxLeft) attack.BossAttackLeechLeft();
                                else
                                {
                                    attack.BossAttackLeechRight();
                                    attackNumberLeech = resetPromenych();
                                }
                            }
                            else
                            {
                                attack.BossAttackDagger();
                                attackNumberDagger = resetPromenych();
                            }
                        }
                        attackTimer = 0f;
                    }
                    break;
                }
            case 3:
                {
                    if (attackTimer > 8)
                    {
                        if (!bossInvincible) attack.PhaseAttack();
                        else
                        {
                            /*if ((bossHitboxDown && rb.position.x > 6.60) && attackNumberRush < 1) //RushRight
                            {
                                attack.BossAttackRushPlayer(true);
                                attackNumberRush = resetPromenych();
                            }
                            else if ((bossHitboxDown && rb.position.x < -6.60) && attackNumberRush < 1) //RushLeft
                            {
                                attack.BossAttackRushPlayer(false);
                                attackNumberRush = resetPromenych();
                            }*/
                            if (attackNumberSword < 1 && PlayerScript.bossHitboxLeft)
                            {
                                attack.BossAttackSwordLeft();
                                attackNumberSword = resetPromenych();
                            }
                            else if (attackNumberSword < 1 && PlayerScript.bossHitboxRight)
                            {
                                attack.BossAttackSwordRight();
                                attackNumberSword = resetPromenych();
                            }
                            else if (PlayerScript.bossHitboxDown && attackNumberFloorIsLava < 1)
                            {
                                attack.BossAttackFloorIsLava();
                                attackNumberFloorIsLava = resetPromenych();
                            }
                            else
                            {
                                attack.BossAttackDagger();
                                attackNumberDagger = resetPromenych();
                            }
                        }
                        attackTimer = 0f;
                    }
                    break;
                }
            case 4:
                {
                    if (attackTimer > 6)
                    {
                        if (attackNumberSword < 1 && (PlayerScript.bossHitboxLeft || PlayerScript.bossHitboxRight))
                        {
                            attack.BossAttackSwordBoth(true, false, 0);
                            attackNumberSword = resetPromenych();
                        }
                        else if (PlayerScript.bossHitboxDown && attackNumberFloorIsLava < 1)
                        {
                            attack.BossAttackFloorIsLava();
                            attackNumberFloorIsLava = resetPromenych();
                        }
                        else if (attackNumberDagger > 1)
                        {
                            attack.BossAttackDagger();
                            attackNumberDagger = resetPromenych();
                        }
                        else
                        {
                            if (phaseTimer % 2 == 0) attack.BossAttackLeechLeft();
                            else if (phaseTimer % 3 == 0) attack.BossAttackLeechRight();
                            else attack.BossAttackLeechBoth();
                            attackNumberSword = resetPromenych();
                        }
                        attackTimer = 0f;
                    }
                    break;
                }
            }
        }
    }
}
