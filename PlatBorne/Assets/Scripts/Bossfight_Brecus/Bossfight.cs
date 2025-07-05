using System.Collections;
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
    [SerializeField] AudioSource sfxInvicDown;
    [SerializeField] AudioSource sfxInvicUp;
    [SerializeField] SpriteRenderer bossSprite;
    [SerializeField] GameObject DashOrb;
    [SerializeField] AudioSource OSTPhase0;
    [SerializeField] AudioSource[] OST;
    [SerializeField] AudioSource[] OSTIntermezzo;

    public Animator animator;
    PlayerHealth playerHealth;
    public BossAttacks attack;
    public GameObject player;
    public GameObject levelMove;
    public GameObject boss;
    Rigidbody2D rb;
    public Text pussyModeOn;
    public GameObject pauseMenu;
    public GameObject UI_BossHP;
    public GameObject UI_PlayerHP;
    public Saves save;
    public int phase = 1;
    public GameObject moveNextLevel;
    bool bossInvincible = true;
    float globalTimer;
    float phaseTimer = 0f;
    float invincibilityTimerBoss = 0f;
    float invincibilityTimerPlayer = 0f;
    static bool playerInvincible = false;
    float timerBetweenAttacks = 0f;
    public UIFadeOutEffect fadeOut;
    static public bool bossfightStarted = false;
    static public bool playerPlayDamage = false;
    static public bool attackIsGoingOn = false;
    bool bossIsDead = false;
    int attackNumberDagger = 0;
    int attackNumberFloorIsLava = 0;
    int attackNumberLeech = 0;
    int attackNumberSword = 0;
    public BoxCollider2D bounds;
    public bool pussyModeActive = false;
    SubtitlesManager subtitlesManager;

    public Slider slider;
    public float speedToFill = 10;
    private int bossHP = 0;
    bool consistentDamage = false;
    private float timer = 0;

    private bool BossInvincible
    {
        get { return bossInvincible; }
        set
        {
            bossInvincible = value;
            ChangeSpriteInvincible(value);
        }
    }

    private int ResetVariables()
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
        animator.SetBool("Boss Death", true);
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
        bossHP = 0;
        enabled = false;
        fadeOut.FadeOut = true;
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
        globalTimer = save.TimerLoad(2);
        levelMove.SetActive(false);
        bounds.isTrigger = true;
        UI_BossHP.SetActive(false);
        UI_PlayerHP.SetActive(false);
        PlayerPrefs.SetString("Level", "bricus");
        PlayerPrefs.Save();
        bossfightStarted = false;
        phase = 1;
        phaseTimer = 0;
        bossHP = 60;
        animator.SetBool("isInvincible", true);
    }

    private void Update()
    {
        pussyModeOn.gameObject.SetActive(pussyModeActive);
        if (player.transform.position.x > 1.5) bossSprite.flipX = true;
        else if (player.transform.position.x < -1.5) bossSprite.flipX = false;

        if (bossfightStarted)
        {
            bounds.isTrigger = false;
            globalTimer += Time.deltaTime;
            save.TimerSave(globalTimer, 2);
            if (BossInvincible) invincibilityTimerBoss += Time.deltaTime;
            else invincibilityTimerBoss = 0f;
            if (playerInvincible) invincibilityTimerPlayer += Time.deltaTime;
            else invincibilityTimerPlayer = 0f;
            if (phase == 4) phaseTimer += Time.deltaTime;
            else phaseTimer = 0f;
            if (!attackIsGoingOn) timerBetweenAttacks += Time.deltaTime;
            else timerBetweenAttacks = 0f;

            if (invincibilityTimerBoss >= 30)
            {
                invincibilityTimerBoss = 0;
                BossInvincible = false;
            }
            if (invincibilityTimerPlayer > 2)
            {
                playerInvincible = false;
                invincibilityTimerPlayer = 0;
            }
            if (!BossInvincible && PlayerScript.bossHitbox)
            {
                BossInvincible = true;
                playerInvincible = true;
                if (playerHealth.PussyMode) playerHealth.PlayerHP = 3;
                switch (bossHP)
                {
                    case 60:
                        bossHP = 40;
                        slider.value = 40;
                        phase = 2;
                        BossDamage01.Play();
                        if (PlayerPrefs.HasKey("Subtitles")) subtitlesManager.Write("Tis But A Scratch!", BossDamage01.clip.length);
                        StartCoroutine(MusicManager(1));
                        break;
                    case 40:
                        bossHP = 20;
                        slider.value = 20;
                        phase = 3;
                        BossDamage02.Play();
                        if (PlayerPrefs.HasKey("Subtitles")) subtitlesManager.Write("You‘re starting to annoy me, Hunter!", BossDamage02.clip.length);
                        StartCoroutine(MusicManager(2));
                        break;
                    case 20:
                        bossHP = 1;
                        slider.value = 1;
                        StartCoroutine(MusicManager(3));
                        BossDamage03.Play();
                        if (PlayerPrefs.HasKey("Subtitles")) subtitlesManager.Write("I‘m gonna sink you like Americans have sank our bloody delicious tea!", BossDamage03.clip.length);                        
                        bossHP = 60;
                        StartCoroutine(SliderDrain());
                        phase = 4;
                        animator.SetBool("isInvincible", false);                        
                        break;
                }
            }

            if (playerHealth.PlayerHP == 0) PlayerDeath();
            if ((PlayerScript.bossHitbox && BossInvincible) || PlayerScript.bossDamage)
            {
                playerHealth.PlayerDamage();
                if (playerHealth.PlayerHP <= 0) PlayerDeath();
                PlayerScript.bossHitbox = false;
                PlayerScript.bossDamage = false;
            }
            switch (phase)
            {
                case 1:
                    if (timerBetweenAttacks > 6 - phase)
                    {
                        if (PlayerScript.bossHitboxDown && attackNumberFloorIsLava < 1)
                        {
                            attack.BossAttackFloorIsLava();
                            attackNumberFloorIsLava = ResetVariables();
                        }
                        else
                        {
                            attack.BossAttackDagger();
                            attackNumberDagger = ResetVariables();
                        }
                    }
                    break;
                case 2:
                    if (timerBetweenAttacks > 6 - phase)
                    {
                        if (PlayerScript.bossHitboxDown && attackNumberFloorIsLava < 1)
                        {
                            attack.BossAttackFloorIsLava();
                            attackNumberFloorIsLava = ResetVariables();
                        }
                        else if (PlayerScript.bossHitboxUp && attackNumberDagger < 1)
                        {
                            attack.BossAttackDagger();
                            attackNumberDagger = ResetVariables();
                        }
                        else if (attackNumberLeech < 1)
                        {
                            if (PlayerScript.bossHitboxLeft) attack.BossAttackLeechLeft();
                            else attack.BossAttackLeechRight();
                            attackNumberLeech = ResetVariables();
                        }
                        else
                        {
                            attack.BossAttackDagger();
                            attackNumberDagger = ResetVariables();
                        }
                    }
                    break;
                case 3:
                    if (timerBetweenAttacks > 6 - phase)
                    {
                        if (PlayerScript.bossHitboxDown && attackNumberFloorIsLava < 1)
                        {
                            attack.BossAttackFloorIsLava();
                            attackNumberFloorIsLava = ResetVariables();
                        }
                        else if (PlayerScript.bossHitboxUp && attackNumberDagger < 1)
                        {
                            attack.BossAttackDagger();
                            attackNumberDagger = ResetVariables();
                        }
                        else
                        {
                            if (PlayerScript.bossHitboxLeft) attack.BossAttackSwordLeft();
                            else attack.BossAttackSwordRight();
                            attackNumberSword = ResetVariables();
                        }
                    }
                    break;
                case 4:
                    if (timerBetweenAttacks > 6 - phase)
                    {
                        if (attackNumberSword < 1 && attackNumberLeech < 1)
                        {
                            if (PlayerScript.bossHitboxRight) attack.BossAttackSwordBoth(false, false, 1f);
                            else attack.BossAttackSwordBoth(false, true, 1f);
                            attackNumberSword = ResetVariables();
                        }
                        else if (attackNumberLeech < 1)
                        {
                            if (bossHP <= 20) attack.BossAttackLeechBoth();
                            else if (PlayerScript.bossHitboxRight) attack.BossAttackLeechRight();
                            else attack.BossAttackLeechLeft();
                            attackNumberLeech = ResetVariables();
                        }
                        else if (PlayerScript.bossHitboxDown && attackNumberFloorIsLava < 1)
                        {
                            attack.BossAttackFloorIsLava();
                            attackNumberFloorIsLava = ResetVariables();
                        }
                        else
                        {
                            attack.BossAttackDagger();
                            attackNumberDagger = ResetVariables();
                        }
                    }
                    break;
            }
        }
    }

    public void StartBossfight() //starts when the camera is moved
    {
        StartCoroutine(StartBossfightCoroutine());
    }

    IEnumerator StartBossfightCoroutine()
    {
        UI_BossHP.SetActive(true);
        UI_PlayerHP.SetActive(true);
        PussyModeManager();
        playerHealth.StartHPUI();
        StartCoroutine(SliderFillUp());
        if (PlayerPrefs.HasKey("brecusStart"))
        {
            if (PlayerPrefs.GetInt("brecusStart") != 0) //short intro
            {
                phase = 1;                
                bossfightStarted = true;
                attackIsGoingOn = false;
                StartCoroutine(MusicManager(0));
            }
        }
        else
        {
            MusicManagerTurnOffMusic(); //long intro
            PreBossDialog.Play();
            if (PlayerPrefs.HasKey("Subtitles")) subtitlesManager.Write("So, you finally did it. Well Now its time to see if you really got what it takes to escape this bloodhole of a city.", PreBossDialog.clip.length);
            yield return new WaitForSeconds(PreBossDialog.clip.length + 0.5f);
            StartCoroutine(MusicManager(0));
            phase = 1;
            bossfightStarted = true;
            attackIsGoingOn = false;
            PlayerPrefs.SetInt("brecusStart", 1);
            PlayerPrefs.Save();
        }        
    }

    IEnumerator MusicManager(int phase)
    {
        MusicManagerTurnOffMusic();
        if (phase != 0)
        {
            OSTIntermezzo[phase - 1].Play();
            while (OSTIntermezzo[phase - 1].isPlaying)
            {
                yield return null;
            }
        }
        OST[phase].Play();
    }

    void MusicManagerTurnOffMusic()
    {
        OSTPhase0.Stop();
        foreach (AudioSource ost in OST) ost.Stop();
        foreach (AudioSource ost in OSTIntermezzo) ost.Stop();
    }

    private void PussyModeManager()
    {
        if (!PlayerPrefs.HasKey("BrecusFirstTime"))
        {
            PlayerPrefs.SetInt("BrecusFirstTime", 1);
            if (PlayerPrefs.HasKey("PussyMode")) PlayerPrefs.SetInt("PussyMode", 0);
            PlayerPrefs.Save();
        }
        pussyModeActive = PlayerPrefs.HasKey("PussyMode") && PlayerPrefs.GetInt("PussyMode") != 0;
    }

    public void SetPussyMode(bool state)
    {
        pussyModeActive = state;
    }

    public void ChangeSpriteInvincible(bool isInvic)
    {
        if (phase == 4)  return; //ignore  on phase 4
        if (isInvic)
        {
            sfxInvicUp.Play();
            animator.SetBool("isInvincible", true);
        }
        else
        {
            sfxInvicDown.Play();
            animator.SetBool("isInvincible", false);
        }
    }

    IEnumerator SliderFillUp(float speed = 0.1f) //normal fill up  speed is 6 seconds
    {
        while(slider.value != slider.maxValue)
        {
            yield return new WaitForSeconds(speed);
            slider.value++;
        }
    }

    IEnumerator SliderDrain()
    {

        yield return SliderFillUp(0.016f); //fills in 1 sec
        slider.value = 60;
        while (slider.value != 0)
        {
            yield return new WaitForSeconds(1f);
            slider.value--;
        }
        StartCoroutine(BossDeath());
    }
}
