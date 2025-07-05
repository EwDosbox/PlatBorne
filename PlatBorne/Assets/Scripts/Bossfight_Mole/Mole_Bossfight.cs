using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class Mole_Bossfight : MonoBehaviour
{
    [Header("Scipts")]
    Mole_Health bossHealth;
    PlayerHealth playerHealth;
    Mole_WeakSpot weakSpot;
    public Mole_UI bossUI;
    Saves save;
    SubtitlesManager subtitlesManager;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] PlayerScript playerScript;
    //INSPECTOR//
    [Header("Audio")]
    [SerializeField] AudioSource sfxBossHit01;
    [SerializeField] AudioSource sfxBossHit02;
    [SerializeField] AudioSource sfxDrillStartUp;
    [SerializeField] AudioSource sfxDrillCharge;
    [SerializeField] AudioSource sfxInvicDown;
    [SerializeField] AudioSource sfxInvicUp;
    [SerializeField] AudioSource vlPreBoss;
    [SerializeField] AudioSource vlSwitchPhase;
    [SerializeField] AudioSource vlBossDeathNormal;
    [SerializeField] AudioSource vlBossDeathPussy;
    [Header("OST")]
    [SerializeField] AudioSource OSTPart0;
    [SerializeField] AudioSource OSTPart1;
    [SerializeField] AudioSource OSTPart2;
    [SerializeField] AudioSource OSTPartEnd;
    [Header("Prefabs")]
    [SerializeField] GameObject prefabDrillGround;
    [SerializeField] GameObject prefabDrillRain;
    [SerializeField] GameObject prefabDrillSide;
    [SerializeField] GameObject prefabMoleRain;
    [SerializeField] GameObject prefabSpike;
    [SerializeField] GameObject platforms;
    [SerializeField] TilemapRenderer platformsTileMapRenderer;
    [SerializeField] GameObject prefabROCK;
    [SerializeField] GameObject prefabShovelRain;
    [SerializeField] GameObject prefabRockDirt;
    [Header("SettingsMain")]
    public float timeToFirstAttack;
    public float bossChargeDelay;
    public float timeBetweenAttacksPhase1;
    public float timeBetweenAttacksPhase2;
    public float moleRainWaitForNextWave;
    public float groundDrillWaitForNextDrill;
    public float moleChargeTimeBeforeCharge;
    public float moleChargeVelocity;
    public float rockChargeTime;
    public float drillSideWaitForAnotherTimer;
    public float waitTimeToTransitionToCutscene;
    private bool changePhaseHasPlayed = false;
    private bool bossDeathHasPlayed = false;
    private bool pussyModeActive = false;
    private bool bossWaitingForDamageWeakspot = false;
    public bool BossWaitingForDamageWeakspot
    {
        get {  return bossWaitingForDamageWeakspot; }
    }
    private bool bossWaitingForDamageWeakspotOnRight = false;
    //timers
    float globalTimer = 0;
    float timerWaitForNextAttack = 0;
    float timerBossCharge = 0;
    float timerAttackSpikes = 0;
    bool playerHasTakenDamageInCharge = false;
    float timeBossLastDamage = 0;
    public float timeBossInvincible = 15;
    //boss attacks
    float spikesTimeNext = 0;
    int numberOfDrillAttacks = 1;
    bool bossIsCharging = false;
    bool bossCanChargeTimer = false;
    bool bossfightIsRunning = false;
    bool attackIsGoing = false;
    int phase = 0;
    bool attackSpikesActivate = false;
    bool bossNextAttackIsCharge = false;
    bool isNextAttackReady = false;
    bool canAttack = true;
    private AudioSource currentVoiceLine = null;
    Rigidbody2D rb;
    //ANIMATOR
    Animator animator;
    [Header("GameObjects")]
    [SerializeField] GameObject pussyModeGameObject;
    [SerializeField] GameObject UI_Player;
    [SerializeField] GameObject UI_Boss;
    private void Start()
    {
        //Initialize Components
        weakSpot = GetComponentInChildren<Mole_WeakSpot>();
        playerHealth = FindAnyObjectByType<PlayerHealth>();
        bossHealth = FindAnyObjectByType<Mole_Health>();
        subtitlesManager = FindFirstObjectByType<SubtitlesManager>();
        save = FindFirstObjectByType<Saves>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spikesTimeNext = Random.Range(15, 30);
        //UI
        PussyModeManager();
        PlayerPrefs.SetString("Level", "mole");
        PlayerPrefs.Save();
        UI_Boss.SetActive(false);
        UI_Player.SetActive(false);
        platforms.SetActive(false);
        bossHealth.BossHealth = 100;
        playerHealth.PlayerHP = 3;
        globalTimer = save.TimerLoad(4);
        MusicManager(MusicEnum.OSTPart0);
    }
    private void Update()
    {
        Debug.Log("Boss invic:" + bossHealth.BossInvincible);
        if (bossfightIsRunning)
        {
            #region DelayBetweenAttacks
            if (phase == 1 && !attackIsGoing)
            {
                if (timerWaitForNextAttack > timeBetweenAttacksPhase1) isNextAttackReady = true;
                else timerWaitForNextAttack += Time.deltaTime;
            }
            else if (phase == 2 && !attackIsGoing)
            {
                if (timerWaitForNextAttack > timeBetweenAttacksPhase2) isNextAttackReady = true;
                else timerWaitForNextAttack += Time.deltaTime;
            }
            #endregion

            #region Spikes
            if (attackSpikesActivate)
            {
                timerAttackSpikes += Time.deltaTime;
                if (timerAttackSpikes > spikesTimeNext && phase == 2)
                {
                    timerAttackSpikes = 0;
                    spikesTimeNext = Random.Range(15, 30);
                    Attack_Spikes();
                }
            }
            #endregion

            #region BOSSCHARGE
            if (timerBossCharge > bossChargeDelay)
            {
                timerBossCharge = 0;
                bossNextAttackIsCharge = true;
            }
            else if (bossCanChargeTimer) timerBossCharge += Time.deltaTime;
            #endregion

            #region ATTACK HANDLER
            if (canAttack)
            {
                if (isNextAttackReady)
                {
                    timerWaitForNextAttack = 0;
                    isNextAttackReady = false;
                    ChooseAttack(phase);
                }
            }
            else
            {
                timerWaitForNextAttack = 0;
                isNextAttackReady = false;
            }
            #endregion


            #region Health Manager
            if (playerHealth.PlayerHP == 0) playerHealth.PlayerDeath(2); //Player Death
            if (bossHealth.BossHealth <= 0 && !bossDeathHasPlayed)  //One Time Thing
            {
                bossDeathHasPlayed = true;
                bossfightIsRunning = false;
                StartCoroutine(BossDeath());
            }
            else if (bossHealth.BossHealth < 50 && phase == 1 && !changePhaseHasPlayed)  //One Time Thing
            {
                changePhaseHasPlayed = true;
                StartCoroutine(ChangePhase());
            }
            #endregion

            globalTimer += Time.deltaTime;            
            if (bossHealth.BossInvincible && phase == 1 && globalTimer - timeBossLastDamage > timeBossInvincible) //first phase Invic timer
            {
                timeBossLastDamage = globalTimer;
                bossHealth.BossInvincible = false;
            }
            save.TimerSave(globalTimer, 4);
            pussyModeGameObject.SetActive(pussyModeActive);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!bossfightIsRunning) return; //Do not damage anybody until the boss is finished talking
        if (collision.gameObject.CompareTag("Player") && phase == 1)
        {            
            if (!bossHealth.BossInvincible) //Yes it does the check twice because of the SFX
            {
                if (Random.value < 0.5f) sfxBossHit01.Play();
                else sfxBossHit02.Play();
                bossHealth.BossHit(false);                   
            }
            else
            {
                playerHealth.PlayerDamage();
            }
        }
        if (phase == 2)
        {
            playerHealth.PlayerDamage();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !playerHasTakenDamageInCharge && bossIsCharging)
        {
            Debug.Log("The player was damaged from charge");
            playerHealth.PlayerDamage();
            playerHasTakenDamageInCharge = true;
        }
    }

    #region AudioManagement

    enum VoiceLinesEnum
    {
        voiceLinePreBoss,
        voiceLinesSwitchPhase,
        voiceLineDeathNormal,
        voiceLineDeathPussyMode
    }

    enum MusicEnum
    {
        OSTPart0,
        OSTPart1,
        OSTPart2,
        OSTPartEnd,
        StopAll
    }

    private void VoiceLinesManager(VoiceLinesEnum voiceLine)
    {
        AudioSource clipToPlay = null;
        string subtitleText = "";
        if (currentVoiceLine != null && currentVoiceLine.isPlaying) //Stops currently playing VL
        {
            currentVoiceLine.Stop();
        }
        switch (voiceLine)
        {
            case VoiceLinesEnum.voiceLinePreBoss:
                clipToPlay = vlPreBoss;
                subtitleText = "At Last, you did it...took you long enough, now get your ass ready for a real fight!";
                break;
            case VoiceLinesEnum.voiceLinesSwitchPhase:
                clipToPlay = vlSwitchPhase;
                subtitleText = "You wanna play? I will show you how we play with visitors in Birmingham!";
                break;
            case VoiceLinesEnum.voiceLineDeathNormal:
                clipToPlay = vlBossDeathNormal;
                subtitleText = "YOU CANNOT CHANGE WHAT HAS BEEN DONE! *screaming*";
                break;
            case VoiceLinesEnum.voiceLineDeathPussyMode:
                clipToPlay = vlBossDeathPussy;
                subtitleText = "THE NIGHT WILL ALWAYS PERSIST! *screaming*";
                break;
        }
        if (clipToPlay != null)
        {
            clipToPlay.Play();
            currentVoiceLine = clipToPlay; 
        }
        //Substitles
        if (PlayerPrefs.HasKey("subtitles") && !string.IsNullOrEmpty(subtitleText))
        {
            subtitlesManager.Write(subtitleText, clipToPlay.clip.length);
        }
    }

    private void MusicManager(MusicEnum music)
    {
        OSTPart0.Stop();
        OSTPart1.Stop();
        OSTPart2.Stop();
        OSTPartEnd.Stop();
        if (music == MusicEnum.StopAll) return;
        switch (music)
        {
            case MusicEnum.OSTPart0:
                OSTPart0.Play();
                break;
            case MusicEnum.OSTPart1:
                OSTPart1.Play();
                break;
            case MusicEnum.OSTPart2:
                OSTPart2.Play();
                break;
            case MusicEnum.OSTPartEnd:
                OSTPartEnd.Play();
                break;
        }
    }
    #endregion

    #region BossfightStates
    public IEnumerator StartBossFight()
    {        
        UI_Boss.SetActive(true);
        UI_Player.SetActive(true);
        bossUI.BossHPSliderStart();
        pussyModeGameObject.SetActive(pussyModeActive);
        playerHealth.StartHPUI();
        if (PlayerPrefs.HasKey("moleStart"))
        {
            if (PlayerPrefs.GetInt("moleStart") != 0)
            {
                MusicManager(MusicEnum.OSTPart1);
                phase = 1;
                bossfightIsRunning = true;
            }
        }
        else //first time hearing fighting boss
        {
            MusicManager(MusicEnum.StopAll);
            VoiceLinesManager(VoiceLinesEnum.voiceLinePreBoss);
            yield return new WaitForSeconds(vlPreBoss.clip.length + 0.5f);
            MusicManager(MusicEnum.OSTPart1);
            phase = 1;
            bossfightIsRunning = true;
            PlayerPrefs.SetInt("moleStart", 1);
        }
    }

    public IEnumerator ChangePhase()
    {
        animator.SetBool("isInvincible", false);
        bossHealth.BossStayInvincible = true;
        playerHealth.PlayerInvincible = true;
        canAttack = false;
        Debug.Log("Attack: ChangePhase");
        MusicManager(MusicEnum.StopAll);
        VoiceLinesManager(VoiceLinesEnum.voiceLinesSwitchPhase);
        yield return new WaitForSeconds(vlSwitchPhase.clip.length);
        MusicManager(MusicEnum.OSTPart2);
        phase = 2;
        playerHealth.PlayerInvincible = false;
        StartCoroutine(PlatformFadeIn(1));
        attackSpikesActivate = true;
        StartCoroutine(Attack_MoleCharge());
        if (pussyModeActive) playerHealth.PlayerHP = 3;
    }

    public IEnumerator BossDeath()
    {
        Debug.Log("BossDeath");
        MusicManager(MusicEnum.OSTPartEnd);
        #region Animator
        //Reset All Animator
        animator.SetBool("chargingRight", false);
        animator.SetBool("chargingLeft", false);
        if (transform.position.x <= 0) animator.SetBool("deathLeft", true);
        else animator.SetBool("deathRight", true);
        #endregion
        StartCoroutine(PlatformFadeOut(1));
        rb.angularVelocity = 0;
        float waitTime = 0;
        if (PlayerPrefs.HasKey("PussyMode"))
        {
            VoiceLinesManager(VoiceLinesEnum.voiceLineDeathPussyMode);
            waitTime = vlBossDeathPussy.clip.length;
            VoiceLinesManager(VoiceLinesEnum.voiceLineDeathPussyMode);
            PlayerPrefs.DeleteKey("BeatenWithAPussyMode_Brecus");
        }
        else
        {
            VoiceLinesManager(VoiceLinesEnum.voiceLineDeathNormal);
            waitTime = vlBossDeathNormal.clip.length;
            VoiceLinesManager(VoiceLinesEnum.voiceLineDeathNormal);
            PlayerPrefs.SetString("BeatenWithAPussyMode_Brecus", "real");
        }
        bossUI.FadeOutEffect(waitTimeToTransitionToCutscene - waitTime);
        PlayerPrefs.Save();
        yield return new WaitForSeconds(waitTimeToTransitionToCutscene);
        bossUI.BossHPSliderDestroy();
        //Ending Decider
        if (PlayerPrefs.HasKey("BeatenWithAPussyMode_Brecus") || PlayerPrefs.HasKey("BeatenWithAPussyMode_Mole"))
        {
            SceneManager.LoadScene(9);
        }
        else SceneManager.LoadScene(10);
    }
    #endregion

    #region AttacksLogic
    private BossAttack lastAttack = BossAttack.MoleRain;

    private enum BossAttack
    {
        MoleRain,
        DrillRain,
        DrillGround,
        DrillSide,
        ShovelRain,
        Rock
    }


    private void ChooseAttack(int phase)
    {
        attackIsGoing = true;
        List<BossAttack> possibleAttacks;

        if (phase == 1)
        {
            possibleAttacks = new List<BossAttack> {
            BossAttack.MoleRain,
            BossAttack.DrillRain,
            BossAttack.DrillGround,
            BossAttack.DrillSide
        };
        }
        else if (phase == 2)
        {
            if (bossNextAttackIsCharge)
            {
                StartCoroutine(Attack_MoleCharge());
                return;
            }

            possibleAttacks = new List<BossAttack> {
            BossAttack.DrillSide,
            BossAttack.MoleRain,
            BossAttack.ShovelRain,
            BossAttack.Rock
        };
        }
        else
        {
            Debug.LogWarning("Unknown boss phase: " + phase);
            return;
        }

        possibleAttacks.Remove(lastAttack);
        if (possibleAttacks.Count == 0) possibleAttacks.Add(lastAttack);

        lastAttack = possibleAttacks[Random.Range(0, possibleAttacks.Count)];

        switch (lastAttack)
        {
            case BossAttack.MoleRain: StartCoroutine(Attack_MoleRain()); break;
            case BossAttack.DrillRain: StartCoroutine(Attack_DrillRain()); break;
            case BossAttack.DrillGround: StartCoroutine(Attack_GroundDrills()); break;
            case BossAttack.DrillSide: StartCoroutine(Attack_SideDrills()); break;
            case BossAttack.ShovelRain: StartCoroutine(Attack_ShovelRain()); break;
            case BossAttack.Rock: MainRockAttack(); break;
            default: Debug.LogWarning("Unhandled attack: " + lastAttack); break;
        }
    }



    #endregion

    #region Phase1Attacks
    IEnumerator Attack_DrillRain()
    {
        Debug.Log("Attack: Drill Rain");
        for (int j = 0; j < numberOfDrillAttacks; j++)
        {
            if (j % 2 == 0)
            {
                for (int i = 0; i < 8; i++)
                {
                    Vector2 position = new Vector2(-16.50f + (i * 4.75f), 11);
                    Instantiate(prefabDrillRain, position, Quaternion.identity);
                }
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    Vector2 position = new Vector2(-16.50f + (i * 2.375f), 11);
                    Instantiate(prefabDrillRain, position, Quaternion.identity);
                }
            }
            yield return new WaitForSeconds(1f);
        }
        numberOfDrillAttacks++;
        attackIsGoing = false;
    }

    IEnumerator Attack_MoleRain()
    {
        Debug.Log("Attack: MoleRain");
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 12; j += 2)
            {
                Vector2 position = new Vector2(-16.68f + (j * 3), 11.40f);
                Instantiate(prefabMoleRain, position, Quaternion.identity);
            }
            yield return new WaitForSeconds(moleRainWaitForNextWave);
            for (int j = 1; j < 12; j += 2)
            {
                Vector2 position = new Vector2(-16.68f + (j * 3), 11.40f);
                Instantiate(prefabMoleRain, position, Quaternion.identity);
            }
            yield return new WaitForSeconds(moleRainWaitForNextWave);
        }
        attackIsGoing = false;
    }

    IEnumerator Attack_ShovelRain()
    {
        Debug.Log("Attack: ShovelRain");
        float attackMoleRainShift = 2.4f;
        float fromX = -17f;
        float y = 12.78f;
        for (float currentX = fromX; currentX < Mathf.Abs(fromX); currentX += attackMoleRainShift)
        {
            Vector2 position = new Vector2(currentX,  y);
            Instantiate(prefabShovelRain, position, Quaternion.identity);
            yield return new WaitForSeconds(0.25f);
        }
        yield return new WaitForSeconds(2);       
        attackIsGoing = false;
    }

    IEnumerator Attack_SideDrills()
    {
        Debug.Log("Attack: SideDrills");
        Vector2[] position = new Vector2[6];
        // left wall
        position[0] = new Vector2(-19.5f, -5.72f);
        position[1] = new Vector2(-19.5f, -3.4f);
        position[2] = new Vector2(-19.5f, -1.08f);
        // right wall
        position[3] = new Vector2(19.5f, -5.72f);
        position[4] = new Vector2(19.5f, -3.4f);
        position[5] = new Vector2(19.5f, -1.08f);

        if (playerScript.Position.x > -6 && playerScript.Position.x < 6) // Middle
        {
            for (int i = 0; i < 6; i++)
            {
                Instantiate(prefabDrillSide, position[i], Quaternion.identity);
                yield return new WaitForSeconds(drillSideWaitForAnotherTimer);
            }
        }
        else if (playerScript.Position.x > 6) // Right
        {
            for (int i = 3; i < 6; i++)
            {
                Instantiate(prefabDrillSide, position[i], Quaternion.identity);
                yield return new WaitForSeconds(drillSideWaitForAnotherTimer);
            }
        }
        else // Left
        {
            for (int i = 0; i < 3; i++)
            {
                Instantiate(prefabDrillSide, position[i], Quaternion.identity);
                yield return new WaitForSeconds(drillSideWaitForAnotherTimer);
            }
        }
        attackIsGoing = false;
    }
    #endregion

    #region Phase2Attacks
    IEnumerator Attack_GroundDrills()
    {
        Debug.Log("Attack: GroundDrills");
        if (playerScript.Position.x >= 0)
        {
            for (int i = 0; i < 8; i++)
            {
                yield return new WaitForSeconds(groundDrillWaitForNextDrill);
                Instantiate(prefabDrillGround, new Vector2(16.76f - (4.75f * i), -11), Quaternion.identity); //right            
            }
        }
        else
        {
            for (int i = 0; i < 8; i++)
            {
                yield return new WaitForSeconds(groundDrillWaitForNextDrill);
                Instantiate(prefabDrillGround, new Vector2(-16.76f + (4.75f * i), -11), Quaternion.identity); //left        
            }            
        }
        attackIsGoing = false;
    }
    IEnumerator Attack_MoleCharge()
    {
        bossNextAttackIsCharge = false;
        Debug.Log("Attack: MoleCharge");
        bossCanChargeTimer = false;
        if (playerScript.Position.x > transform.position.x) //player is right away from boss
        {
            animator.SetTrigger("readyChargeRight");
            sfxDrillStartUp.Play();
            yield return new WaitForSeconds(sfxDrillStartUp.clip.length - 0.1f); //Waits for the sfx to finish playing
            sfxDrillCharge.Play();
            animator.SetBool("chargingRight", true);            
            rb.velocity = Vector2.right * moleChargeVelocity;
            bossIsCharging = true;
            while (rb.velocity != Vector2.zero) //Charge to the right side of the screen
            {
                if (rb.position.x > 15.55f)
                {
                    rb.velocity = Vector2.zero;
                }
                else yield return null;
            }
            sfxDrillCharge.Stop();
            bossIsCharging = false;
            canAttack = true; //Turns back the attacks (except the charge - only for the first charge of the fight)
            playerHasTakenDamageInCharge = false; //reset
            MovePlayerIfInsideOfMole();            
            animator.SetBool("chargingRight", false);
            attackIsGoing = false;
            bossWaitingForDamageWeakspot = true;
            bossWaitingForDamageWeakspotOnRight = true;
        }
        else //left
        {
            animator.SetTrigger("readyChargeLeft");
            sfxDrillStartUp.Play();
            yield return new WaitForSeconds(sfxDrillStartUp.clip.length - 0.1f); //Waits for the sfx to finish playing
            sfxDrillCharge.Play();
            animator.SetBool("chargingLeft", true);            
            rb.velocity = Vector2.left * moleChargeVelocity;
            bossIsCharging = true;
            while (rb.velocity != Vector2.zero) //Charge to the left side of the screen
            {
                if (rb.position.x < -15.55f)
                {
                    rb.velocity = Vector2.zero;                    
                }
                else yield return null;
            }
            sfxDrillCharge.Stop();
            canAttack = true; //Turns back the attacks (except the charge - only for the first charge of the fight)
            bossIsCharging = false;
            playerHasTakenDamageInCharge = false; //reset
            MovePlayerIfInsideOfMole();
            animator.SetBool("chargingLeft", false);
            attackIsGoing = false;
            bossWaitingForDamageWeakspot = true;
            bossWaitingForDamageWeakspotOnRight = false;
        }        
    }

    public void MainRockAttack()
    {
        if (playerScript.Position.x > -6.67f && playerScript.Position.x < 6.67f) // middle
        {
            if (Random.Range(0, 2) == 1) // 50/50 chance
            {
                StartCoroutine(RockAttack(0));
                StartCoroutine(RockAttack(1));
            }
            else
            {
                StartCoroutine(RockAttack(1));
                StartCoroutine(RockAttack(2));
            }
        }
        else //left and right
        {
            StartCoroutine(RockAttack(0));
            StartCoroutine(RockAttack(2));
        }
    }

    /// <summary>
    /// 0 = left; 1 = middle; 2 = right
    /// </summary>
    /// <param name="where">Basic input is  1</param>
    /// <returns></returns>
    IEnumerator RockAttack(int where)
    {
        Vector2 position;
        float y = -9;
        if (where == 0) position = new Vector3(-13.24f, y, -1);
        else if (where == 2) position = new Vector3(13.24f, y, -1);
        else position = new Vector3(0, y, -1);

        Instantiate(prefabRockDirt, new Vector3(position.x, -4.62f, 15), Quaternion.identity);
        yield return new WaitForSeconds(rockChargeTime);
        Instantiate(prefabROCK, position, Quaternion.identity);

        attackIsGoing = false;
    }

    private void Attack_Spikes()
    {
        Debug.Log("AttackSpikesStart");
        float y = -2.62f;
        float offset = 2.45f;

        for (int i = 0; i < 3; i++)
        {
            float temp = -10.94f + i * offset;
            Instantiate(prefabSpike, new Vector2(temp, y) , Quaternion.identity);
            Instantiate(prefabSpike, new Vector2(Mathf.Abs(temp), y), Quaternion.identity);
        }
        attackSpikesActivate = false;
    }


    IEnumerator PlatformFadeIn(float fadeDuration = 2)
    {
        Debug.Log("PlatformFadeIn");
        if (platformsTileMapRenderer == null)
        {
            Debug.LogError("TilemapRenderer is not assigned!");
            yield break;
        }
        platforms.SetActive(true);
        float elapsedTime = 0f;
        Color startColor = platformsTileMapRenderer.material.color;
        startColor.a = 0f; //a == alpha
        platformsTileMapRenderer.material.color = startColor;
        Color targetColor = startColor;
        targetColor.a = 1f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);
            platformsTileMapRenderer.material.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }
        platformsTileMapRenderer.material.color = targetColor; //just to make sure
    }


    IEnumerator PlatformFadeOut(float fadeDuration = 2)
    {
        Debug.Log("PlatformFadeOutn");
        if (platformsTileMapRenderer == null)
        {
            Debug.LogError("TilemapRenderer is not assigned!");
            yield break;
        }
        platforms.SetActive(true);
        float elapsedTime = 0f;
        Color startColor = platformsTileMapRenderer.material.color;
        startColor.a = 1f; //a == alpha
        platformsTileMapRenderer.material.color = startColor;
        Color targetColor = startColor;
        targetColor.a = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);
            platformsTileMapRenderer.material.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }
        platformsTileMapRenderer.material.color = targetColor; //just to make sure
    }

    #endregion
    public void BossHitWeakspot()
    {
        bossWaitingForDamageWeakspot = false;
        bossHealth.BossHit(true);        
        playerHealth.SetInvincibleTimer(3f);
        bossNextAttackIsCharge = false;
        bossCanChargeTimer = true; //Turn on the timer
        if (bossWaitingForDamageWeakspotOnRight) animator.SetTrigger("stopChargeRight");
        else animator.SetTrigger("stopChargeLeft");
        animator.SetBool("chargingRight", false);
        animator.SetBool("chargingLeft", false);
    }
    private void MovePlayerIfInsideOfMole()
    {
        if (playerScript.transform.position.x < -14) //PlayerOnLeft
        {
            playerScript.MovePlayer(5, 0); //Move Right
            if (!playerHasTakenDamageInCharge) playerHealth.PlayerDamage();
            Debug.Log("CheckIfPlayerIsInMole Move Right");
        }
        if (playerScript.transform.position.x > 14) //PlayerOnRight
        {
            playerScript.MovePlayer(-5, 0); //Move Left
            if (!playerHasTakenDamageInCharge) playerHealth.PlayerDamage();
            Debug.Log("CheckIfPlayerIsInMole Move Left");
        }
    }

    private void FirstTimeBossChecker()
    {
        if (!PlayerPrefs.HasKey("MoleFirstTime") || PlayerPrefs.GetInt("MoleFirstTime") == 0) //Boss will be set on normal difficulty the first time
        {
            PlayerPrefs.SetInt("MoleFirstTime", 1);
            if (PlayerPrefs.HasKey("PussyMode")) PlayerPrefs.SetInt("PussyMode", 0);
            PlayerPrefs.Save();
            Debug.Log("PussyMode Reset");
        }
    }

    private void PussyModeManager()
    {
        if (pussyModeGameObject == null) Debug.LogError("Could not find PussyModeText");
        else
        {
            FirstTimeBossChecker();
            if (PlayerPrefs.HasKey("PussyMode") && PlayerPrefs.GetInt("PussyMode") != 0) pussyModeActive = true;
            else pussyModeActive = false;
        }
    }

    public void SetPussyMode(bool state)
    {
        pussyModeActive = state;
    }

    public void ChangeSpriteInvincible(bool isInvic)
    {
        if (isInvic)
        {
            //Un-drop Invincibility
            sfxInvicUp.Play();
            animator.SetBool("isInvincible", true);            
        }
        else
        {
            //drop Invincibility
            sfxInvicDown.Play();
            animator.SetBool("isInvincible", false);
        }
    }
}