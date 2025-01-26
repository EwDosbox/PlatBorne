using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class Bossfight : MonoBehaviour
{
    [SerializeField] AudioSource PreBossDialog;
    [SerializeField] AudioSource BossDamage01;
    [SerializeField] AudioSource BossDamage02;
    [SerializeField] AudioSource BossDamage03;
    [SerializeField] AudioSource BossDeath01;
    [SerializeField] AudioSource SFXVunerability;
    [SerializeField] SpriteRenderer bossSprite;
    [SerializeField] GameObject DashOrb;
    [SerializeField] AudioSource OSTPhase0;
    [SerializeField] AudioSource[] OST;
    [SerializeField] AudioSource[] OSTIntermezzo;

    public Animator bossAnimation;
    public BossHealthBar bossHealthBar;
    PlayerHealth playerHealth;
    public BossAttacks attack;
    public GameObject player;
    public GameObject levelMove;
    public GameObject boss;
    Rigidbody2D rb;
    public Text text;
    public Text pussyModeOn;
    public GameObject pauseMenu;
    public GameObject UI_BossHP;
    public GameObject UI_PlayerHP;
    public Saves save;
    public int phase = 1;
    public GameObject moveNextLevel;
    bool bossInvincible = true;
    float timer;
    bool timerOn = false;
    float phaseTimer = 0f;
    float invincibilityTimerBoss = 0f;
    float invincibilityTimerPlayer = 0f;
    static bool playerInvincible = false;
    float timerBetweenAttacks = 0f; //Dependent on attackIsGoingOn
    public UIFadeOutEffect fadeOut;
    //BossAttacks
    static public bool bossfightStarted = false;
    static public bool playerPlayDamage = false;
    static public bool attackIsGoingOn = false; //When attack is finished, bool will go to false. If false, the timer will activate and depenting on phase will start another attack when the time is right
    bool bossIsDead = false;    
    int attackNumberDagger = 0;
    int attackNumberFloorIsLava = 0;
    int attackNumberLeech = 0;
    int attackNumberSword = 0;
    //****************
    public BoxCollider2D bounds;
    public bool pussyModeActive = false;
    SubtitlesManager subtitlesManager;
    private int resetPromenych()
    {
        attackNumberDagger = 0;
        attackNumberFloorIsLava = 0;
        attackNumberLeech = 0;
        attackNumberSword = 0;
        return 1;
    }
    public IEnumerator BossDeath()
    {
        MusicManagerTurnOffMusic();
        bossIsDead = true;
        BossDeath01.Play();
        bossAnimation.SetBool("Boss Death", true);
        if (pussyModeActive)
        {
            PlayerPrefs.SetString("BeatenWithAPussyMode_Brecus", "real");
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.DeleteKey("BeatenWithAPussyMode_Brecus");
            PlayerPrefs.Save();
        }
        yield return new WaitForSeconds(1.78f);
        Destroy(boss);
        DashOrb.SetActive(true);
        text.text = "Boss Is Dead";
        bossHealthBar.SetHP(0);
        bossHealthBar.enabled = false;
        fadeOut.FadeOut = true; //UI Fade Out effect
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
        playerHealth = FindFirstObjectByType<PlayerHealth>();
        subtitlesManager = FindAnyObjectByType<SubtitlesManager>();
        if (subtitlesManager == null) Debug.LogError("Could Not Find subtitles Manager in Brecus Bossfight");
        timer = save.TimerLoad(2);
        levelMove.SetActive(false);
        bounds.isTrigger = true;
        UI_BossHP.SetActive(false);
        UI_PlayerHP.SetActive(false);
        PlayerPrefs.SetString("Level", "bricus");
        PlayerPrefs.Save();
        bossfightStarted = false;
        phase = 1;
        phaseTimer = 0;
        timerOn = false;
    }
    private void Update()
    {
        pussyModeOn.gameObject.SetActive(pussyModeActive);
        if (bossInvincible) text.text = "Boss Is Invincible";
        else text.text = "Boss Is Vunerable";
        if (pauseMenu.activeInHierarchy) timerOn = false;
        else if (bossfightStarted) timerOn = true;
        //Boss Sprite Flip
        if (player.transform.position.x > 1.5) bossSprite.flipX = true;
        else if (player.transform.position.x < -1.5) bossSprite.flipX = false;
        //****//
        #region BossFightMain
        if (bossfightStarted)
        {
            #region Timers
            bounds.isTrigger = false;
            //ÈASOVAÈ
            if (timerOn)
            {
                timer += Time.deltaTime;
                save.TimerSave(timer, 2);
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
                SFXVunerability.Play();
                bossInvincible = false;
            }
            if (invincibilityTimerPlayer > 2) //seconds of invincibility after damage
            {
                playerInvincible = false;
                invincibilityTimerPlayer = 0;
            }
            #endregion
            #region BossDamage
            if (phase == 4 && bossHealthBar.GetHP() == 0) StartCoroutine(BossDeath());            
            if (!bossInvincible && PlayerScript.bossHitbox)
            {
                bossInvincible = true;
                playerInvincible = true;
                if (playerHealth.PussyMode) playerHealth.PlayerHP = 3;
                switch (bossHealthBar.GetHP())
                {
                    case 60:
                        {
                            bossHealthBar.SetHP(40);
                            phase = 2;
                            BossDamage01.Play();
                            if (PlayerPrefs.HasKey("Subtitles")) subtitlesManager.Write("Tis But A Scratch!", BossDamage01.clip.length);
                            StartCoroutine(MusicManager(1));
                            break;
                        }
                    case 40:
                        {
                            bossHealthBar.SetHP(20);
                            phase = 3;
                            BossDamage02.Play();
                            if (PlayerPrefs.HasKey("Subtitles")) subtitlesManager.Write("You‘re starting to annoy me, Hunter!", BossDamage02.clip.length);
                            StartCoroutine(MusicManager(2));
                            break;
                        }
                    case 20:
                        {
                            bossHealthBar.SetHP(1);
                            BossDamage03.Play();
                            if (PlayerPrefs.HasKey("Subtitles")) subtitlesManager.Write("I‘m gonna sink you like Americans have sank our bloody delicious tea!", BossDamage03.clip.length);
                            bossHealthBar.Slider();
                            bossHealthBar.SetHP(60);
                            bossHealthBar.LastPhase();
                            phase = 4;
                            text.text = "Survive";
                            StartCoroutine(MusicManager(3));
                            break;
                        }
                }
            }
            else if (bossHealthBar.GetHP() == 0 && phase == 4)
            {
                StartCoroutine(BossDeath());
            }
            #endregion
            #region PlayerDamage
            if (playerHealth.PlayerHP == 0) PlayerDeath();
            if ((PlayerScript.bossHitbox && bossInvincible) || PlayerScript.bossDamage)
            {
                playerHealth.PlayerDamage();
                if (playerHealth.PlayerHP <= 0) PlayerDeath();
                PlayerScript.bossHitbox = false;
                PlayerScript.bossDamage = false;
            }
            #endregion
            switch (phase)
            {
                case 1:
                    {
                        if (timerBetweenAttacks > 6 - phase) //doba èekání na další útok
                        {
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
        #endregion

        #region Start of Bossfight - UI inicialization (once)
        else if (PlayerScript.bossHitboxLeft && !bossfightStarted)
        {
            UI_BossHP.SetActive(true);
            UI_PlayerHP.SetActive(true);
            phase = 1;
            bossfightStarted = true;
            attackIsGoingOn = false;
            PussyModeManager();
            bossHealthBar.Slider();
            playerHealth.StartHPUI();
            StartCoroutine(DIALOGWaitingLine("preboss"));
        }
        #endregion
    }

    //DIALOG doesnt overlap with Music
    IEnumerator DIALOGWaitingLine(string name) 
    {
        switch(name)
        {
            case "preboss":
                {
                    MusicManagerTurnOffMusic();
                    PreBossDialog.Play();
                    if (PlayerPrefs.HasKey("Subtitles")) subtitlesManager.Write("So, you finally did it. Well Now its time to see if you really got what it takes to escape this bloodhole of a city.", PreBossDialog.clip.length);
                    yield return new WaitForSeconds(PreBossDialog.clip.length);
                    StartCoroutine(MusicManager(0));
                    break;
                }
        }        
    }

    //MUSIC
    IEnumerator MusicManager(int phase) //Phase 0 Starts automatically
    {        
        if (phase != 0) //There are 4 phase OST but only 3 Intermezzos starting at phase 2
        {
            OSTIntermezzo[phase].Play();
            yield return new WaitForSeconds(OSTIntermezzo[phase - 1].clip.length); 
        }
        OST[phase].Play();
    }

    void MusicManagerTurnOffMusic()
    {
        OSTPhase0.Stop();
        foreach (AudioSource ost in OST)
        {
            ost.Stop();
        }
        foreach (AudioSource ost in OSTIntermezzo)
        {
            ost.Stop();
        }
    }

    private void PussyModeManager()
    {
        if (!PlayerPrefs.HasKey("BrecusFirstTime")) //Boss will be set on normal difficulty the first time
        {
            PlayerPrefs.SetInt("BrecusFirstTime", 1);
            if (PlayerPrefs.HasKey("PussyMode")) PlayerPrefs.SetInt("PussyMode", 0);
            PlayerPrefs.Save();
        }

        if (PlayerPrefs.HasKey("PussyMode") && PlayerPrefs.GetInt("PussyMode") != 0) pussyModeActive = true;
        else pussyModeActive = false;
    }

    public void SetPussyMode(bool state)
    {
        pussyModeActive = state;
    }
}