using System.Diagnostics;
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
    [SerializeField] private SpriteRenderer bossSprite;
    [SerializeField] private GameObject DashOrb;

    public BossFightVoiceLines voiceLines;
    public BossHealthBar bossHealthBar;
    public PlayerHealth playerHealth;
    public BossAttacks attack;
    public GameObject player;
    private Rigidbody2D rb;
    public Text text;
    public Text pussyModeOn;
    public GameObject pauseMenu;
    public GameObject UI_BossHP;
    public GameObject UI_PlayerHP;
    public Saves save;
    public int phase = 1;
    bool bossInvincible = true;
    Stopwatch timer = new Stopwatch();
    bool timerOn = false;
    float phaseTimer = 0f;
    float invincibilityTimerBoss = 0f;
    float invincibilityTimerPlayer = 0f;
    static bool playerInvincible = false;
    private float timerBetweenAttacks = 0f; //Dependent on attackIsGoingOn
    //****************
    static public bool bossfightStarted = false;
    static public bool playerPlayDamage = false;
    static public bool attackIsGoingOn = false; //When attack is finished, bool will go to false. If false, the timer will activate and depenting on phase will start another attack when the time is right
    private float bossfightTimer;
    static public bool godMode = false;
    private bool pussyMode = false;
    private bool bossIsDead = false;
    //****************
    int attackNumberDagger = 0;
    int attackNumberFloorIsLava = 0;
    int attackNumberLeech = 0;
    int attackNumberSword = 0;
    //****************
    public BoxCollider2D bounds;

    private int resetPromenych()
    {
        attackNumberDagger = 0;
        attackNumberFloorIsLava = 0;
        attackNumberLeech = 0;
        attackNumberSword = 0;
        return 1;
    }
    private void BossDeath()
    {
        bossIsDead = true;
        Destroy(bossSprite); //tohle se pak musí upravit jak bude aniamce
        Instantiate(DashOrb);
        text.text = "Boss Is Dead";
        BossDeath01.Play();
    }
    public void PlayerDeath()
    {
        int ded = PlayerPrefs.GetInt("NumberOfDeath", 0);
        ded++;
        PlayerPrefs.SetInt("NumberOfDeath", ded);
        SceneManager.LoadScene("PlayerDeath");
    }


    private void Start()
    {
        bounds.isTrigger = true;
        UI_BossHP.active = false;
        UI_PlayerHP.active = false;
        PlayerPrefs.SetString("Level", "bricus");
        PlayerPrefs.Save();
        //Absolute fucking reset
        bossfightStarted = false;
        phase = 1;
        phaseTimer = 0;
        timerOn = false;
        timer = save.timerLoad(2);
        if (PlayerPrefs.GetInt("PussyMode") > 0) pussyMode = true;
        else pussyMode = false;
    }
    private void Update()
    {
        if (bossfightStarted && phase == 4 && bossHealthBar.GetHP() == 0) BossDeath();
        if (playerHealth.GetHP() == 0 && bossfightStarted) PlayerDeath();
        if (bossInvincible) text.text = "Boss Is Invincible";
        else text.text = "Boss Is Vunerable";
        if (pauseMenu.active) timerOn = false;
        else if (bossfightStarted) timerOn = true;
        //Boss Sprite Flip
        if (player.transform.position.x > 1.5) bossSprite.flipX = true;
        else if (player.transform.position.x < -1.5) bossSprite.flipX = false;
        //****//
        if (bossfightStarted)
        {
            bounds.isTrigger = false;
            //ÈASOVAÈ
            if (timerOn)
            {
                save.timerSave(timer, 2);
                if (bossInvincible) invincibilityTimerBoss += Time.deltaTime;
                else invincibilityTimerBoss = 0f;
                if (playerInvincible) invincibilityTimerPlayer += Time.deltaTime;
                else invincibilityTimerPlayer = 0f;
                if (phase == 4) phaseTimer += Time.deltaTime;
                else phaseTimer = 0f;
                if (!attackIsGoingOn) timerBetweenAttacks += Time.deltaTime;
                else timerBetweenAttacks = 0f;
            }

            if (invincibilityTimerBoss >= 60)
            {
                invincibilityTimerBoss = 0;
                bossInvincible = false;
            }
            if (invincibilityTimerPlayer > 2) //seconds of invincibility after damage
            {
                playerInvincible = false;
                invincibilityTimerPlayer = 0;
            }
            //Boss Damage
            if (!bossInvincible && PlayerScript.bossHitbox)
            {
                bossInvincible = true;
                playerInvincible = true;
                if (pussyMode) playerHealth.SetHP(3);
                switch (bossHealthBar.GetHP())
                {
                    case 60:
                        {
                            bossHealthBar.SetHP(40);
                            phase = 2;
                            BossDamage01.Play();
                            break;
                        }
                    case 40:
                        {
                            bossHealthBar.SetHP(20);
                            phase = 3;
                            BossDamage02.Play();
                            break;
                        }
                    case 20:
                        {
                            bossHealthBar.SetHP(1);
                            BossDamage03.Play();
                            bossHealthBar.Slider();
                            bossHealthBar.SetHP(60);
                            bossHealthBar.LastPhase();
                            phase = 4;
                            text.text = "Survive";
                            OSTLoop.enabled = false;
                            OSTPhase4.enabled = true;
                            break;
                        }
                }
            }
            else if (bossHealthBar.GetHP() == 0 && phase == 4)
            {
                BossDeath();
            }
            //Player Damage
            if ((PlayerScript.bossHitbox && bossInvincible) || PlayerScript.bossDamage)
            {
                if (!playerInvincible && !godMode)
                {
                    playerInvincible = true;
                    playerPlayDamage = true;
                    playerHealth.SetHP(playerHealth.GetHP() - 1);
                    playerHealth.SetGodMode(godMode);
                    if (playerHealth.GetHP() == 0)
                    {
                        PlayerDeath();
                    }
                }
                PlayerScript.bossHitbox = false;
                PlayerScript.bossDamage = false;
            }
            switch (phase)
            {
                case 1:
                    {
                        if (timerBetweenAttacks > 6 - phase) //doba èekání na další útok
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
                                attack.BossAttackFloorIsLava();
                                attackNumberFloorIsLava = resetPromenych();
                            }
                            else
                            {
                                attack.BossAttackDagger();
                                attackNumberDagger = resetPromenych();
                            }
                        }
                    }
                    break;
                case 2:
                    {
                        if (timerBetweenAttacks > 6 - phase)
                        {
                            {
                                /*if (((PlayerScript.bossHitboxDown && rb.position.x > 6.60) && attackNumberRush < 1) //RushRight
                                {
                                    attack.BossAttackRushPlayer(true);
                                    attackNumberRush = resetPromenych();
                                }
                                else if (((PlayerScript.bossHitboxDown && rb.position.x < -6.60) && attackNumberRush < 1) //RushLeft
                                {
                                    attack.BossAttackRushPlayer(false);
                                    attackNumberRush = resetPromenych();
                                }*/
                                if (PlayerScript.bossHitboxDown && attackNumberFloorIsLava < 1)
                                {
                                    attack.BossAttackFloorIsLava();
                                    attackNumberFloorIsLava = resetPromenych();
                                }
                                else if (PlayerScript.bossHitboxUp && attackNumberDagger < 1)
                                {
                                    attack.BossAttackDagger();
                                    attackNumberDagger = resetPromenych();
                                }
                                else if (attackNumberLeech < 1)
                                {
                                    if (PlayerScript.bossHitboxLeft) attack.BossAttackLeechLeft();
                                    else attack.BossAttackLeechRight();
                                    attackNumberLeech = resetPromenych();
                                }
                                else
                                {
                                    attack.BossAttackDagger();
                                    attackNumberDagger = resetPromenych();
                                }
                            }
                        }
                        break;
                    }
                case 3:
                    {
                        if (timerBetweenAttacks > 6 - phase)
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
                                else if (PlayerScript.bossHitboxUp && attackNumberDagger < 1)
                                {
                                    attack.BossAttackDagger();
                                    attackNumberDagger = resetPromenych();
                                }
                                else 
                                {
                                    if (PlayerScript.bossHitboxLeft) attack.BossAttackSwordLeft();
                                    else attack.BossAttackSwordRight();
                                    attackNumberSword = resetPromenych();
                                }
                            }
                        break;
                    }
                case 4:
                    {
                        if (timerBetweenAttacks > 6 - phase)
                        {
                            if (attackNumberSword < 1 && attackNumberLeech < 1)
                            {
                                if (PlayerScript.bossHitboxRight) attack.BossAttackSwordBoth(false, false, 1f);
                                else attack.BossAttackSwordBoth(false, true, 1f);
                                attackNumberSword = resetPromenych();
                                }
                            else if (attackNumberLeech < 1)
                                {
                                if (bossHealthBar.GetHP() <= 20) attack.BossAttackLeechBoth();
                                else if (PlayerScript.bossHitboxRight) attack.BossAttackLeechRight();
                                else attack.BossAttackLeechLeft();
                                attackNumberLeech = resetPromenych();
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
                        break;
                    }
            }
        }
        //start of bossfight
        else if (PlayerScript.bossHitboxLeft && !bossfightStarted) //Start of Bossfight - UI inicialization
        {
            UI_BossHP.active = true;
            UI_PlayerHP.active = true;
            phase = 1;
            bossfightStarted = true;
            attackIsGoingOn = false;
            if (PlayerPrefs.GetInt("GodMode") > 0) godMode = true;
            else godMode = false;
            if (pussyMode) pussyModeOn.gameObject.SetActive(true);
            else pussyModeOn.gameObject.SetActive(false);
            bossHealthBar.Slider();
            playerHealth.PlayerStart();
            PreBossDialog.Play();
            timer.Start();
            OSTLoop.enabled = true;
            if (PlayerPrefs.HasKey("Timer_Bricus")) bossfightTimer = PlayerPrefs.GetFloat("Timer_Bricus");
            else bossfightTimer = 0;
        }
    }
}