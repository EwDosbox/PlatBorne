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
    public GameObject pauseMenu;

    public int phase = 1;
    bool bossInvincible = true;
    float timer = 0f;
    bool timerOn = false;
    float phaseTimer = 0f;
    float invincibilityTimerBoss = 0f;
    float invincibilityTimerPlayer = 0f;
    bool playerInvincible = false;
    private float timerBetweenAttacks = 0f; //Dependent on attackIsGoingOn
    //****************
    static public bool bossfightStarted = false;
    static public bool playerPlayDamage = false;
    static public bool attackIsGoingOn = false; //When attack is finished, bool will go to false. If false, the timer will activate and depenting on phase will start another attack when the time is right
    private float bossfightTimer;
    private bool godMode = false;
    private bool pussyMode = false;
    //****************
    int attackNumberRush = 0;
    int attackNumberDagger = 0;
    int attackNumberFloorIsLava = 0;
    int attackNumberLeech = 0;
    int attackNumberSword = 0;
    //****************
    public BoxCollider2D bounds;

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
        PlayerPrefs.SetFloat("BossTimer", timer);
        PlayerPrefs.Save();
        SceneManager.LoadScene("EndgameCutscene");
        Debug.Log("Boss Has Died");
    }
    public void PlayerDeath()
    {
        int ded = PlayerPrefs.GetInt("NumberOfDeath", 0);
        ded++;
        PlayerPrefs.SetInt("NumberOfDeath", ded);
        float gameTimer = PlayerPrefs.GetFloat("GameTimer");
        gameTimer += timer;
        PlayerPrefs.SetFloat("GameTimer", gameTimer);
        PlayerPrefs.Save();
        Debug.Log("Hunter has Died");
        SceneManager.LoadScene("PlayerDeath");
    }

    private void Start()
    {
        bounds.isTrigger = true;
        PlayerPrefs.SetString("Level", "bossfight");
        PlayerPrefs.Save();
        //Absolute fucking reset
        bossfightStarted = false;
        bossHealthBar.SetHP(60);
        bossHealthBar.Slider();
        playerHealth.SetHP(3);
        phase = 1;
        phaseTimer = 0;
        timerOn = false;
        timer = 0f;
    }
    private void Update()
    {
        if (bossfightStarted && phase == 4 && bossHealthBar.GetHP() == 0) BossDeath();
        if (playerHealth.GetHP() == 0 && bossfightStarted) PlayerDeath();
        if (bossInvincible) text.text = "Boss Invincible";
        else text.text = "Boss Vunerable";
        if (pauseMenu.active) timerOn = false;
        else if (bossfightStarted) timerOn = true;
        if (bossfightStarted)
        {
            //VIKTOR
            if (timer >= 0.25f) bounds.isTrigger = false;
            //VIKTOR
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
            if (!bossInvincible && phase <= 3 && PlayerScript.bossHitbox) //Phase 1,2,3 Boss Damage
            {
                    bossInvincible = true;
                    playerInvincible = true;
                    bossHealthBar.SetHP(bossHealthBar.GetHP() - 20);
                playerHealth.SetHP(3);
                    switch (bossHealthBar.GetHP())
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
                    if (bossHealthBar.GetHP() == 0 && phase == 3)
                    {
                        BossDamage03.Play();
                        Debug.Log("Phase 4 Start");
                        bossHealthBar.Slider();
                        bossHealthBar.SetHP(60);
                        bossHealthBar.LastPhase();
                        phase = 4;
                        OSTLoop.enabled = false;
                        OSTPhase4.enabled = true;
                    }
            }
            if ((PlayerScript.bossHitbox && bossInvincible) || PlayerScript.bossDamage)
            {
                if (!playerInvincible && !godMode)
                {
                    playerInvincible = true;
                    playerPlayDamage = true;
                    playerHealth.SetHP(playerHealth.GetHP() - 1);
                    playerHealth.SetGodMode(godMode);
                    Debug.Log("Hunter has taken Damage");
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
            phase = 1;
            bossfightStarted = true;
            attackIsGoingOn = false;
            if (PlayerPrefs.HasKey("GodMode")) godMode = bool.Parse(PlayerPrefs.GetString("GodMode"));
            else godMode = false;
            //testing
            godMode = true;
            bossHealthBar.Slider();
            playerHealth.PlayerStart();
            PreBossDialog.Play();
            OSTLoop.enabled = true;
            if (PlayerPrefs.HasKey("BossfightTimer")) bossfightTimer = PlayerPrefs.GetFloat("BossfightTimer");
            else bossfightTimer = 0;
        }
    }
}