using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

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
    [SerializeField] AudioSource SFXbossHit;
    [SerializeField] AudioSource SFXbossHit02;
    [SerializeField] AudioSource PreBoss;
    [SerializeField] AudioSource SFXswitchPhase;
    [SerializeField] AudioSource SFXbossDeath;
    [SerializeField] AudioSource SFXbossDeathPussyMode;
    [Header("OST")]
    [SerializeField] AudioSource OSTPart0;
    [SerializeField] AudioSource OSTPart1;
    [SerializeField] AudioSource OSTPart2;
    [SerializeField] AudioSource OSTPartEND;
    [Header("Prefabs")]
    [SerializeField] GameObject prefabDrillGround;
    [SerializeField] GameObject prefabDrillRain;
    [SerializeField] GameObject prefabDrillSide;
    [SerializeField] GameObject prefabMoleRain;
    [SerializeField] GameObject prefabSpike;
    [SerializeField] GameObject platforms;
    [SerializeField] CanvasGroup platformsCanvasGroup;
    [SerializeField] GameObject prefabROCK;
    [SerializeField] GameObject prefabShovelRain;
    [SerializeField] GameObject prefabRockDirt;
    [Header("SettingsMain")]
    public float timeToFirstAttack;
    public float bossChargeDelay;
    public float timeBetweenAttacksPhase1;
    public float timeBetweenAttacksPhase2;
    public float moleRain_waitForNextWave;
    public float groundDrills_waitForNextDrill;
    public float shovelRain_waitForNextWave;
    public float moleCharge_TimeBeforeCharge;
    public float moleCharge_TimeBeforeIdle;
    public float moleCharge_Velocity;
    public float rock_ChargeTime;
    public GameObject doorsEnd;
    public GameObject UI_Player;
    public GameObject UI_Boss;
    //timers
    float timer = 0;
    float timerWaitForNextAttack = 0;
    float timerBossCharge = 0;
    float timerAttackSpikes = 0;

    bool timerOn = false;
    //boss attacks
    bool bossCanBossCharge = false;
    bool bossStarted = false;
    bool attackIsGoing = false;
    int phase = 0;
    bool attackSpikesActivate = false;
    bool bossNextAttackIsCharge = false;
    bool nextAttack = false;
    Rigidbody2D rb;
    //ANIMATOR
    Animator animator;
    private void Start()
    {
        //Initialize Components
        weakSpot = GetComponentInChildren<Mole_WeakSpot>();
        playerHealth = FindAnyObjectByType<PlayerHealth>();
        bossHealth = FindAnyObjectByType<Mole_Health>();
        //UI
        UI_Boss.SetActive(false);
        UI_Player.SetActive(false);
        doorsEnd.SetActive(false);
        subtitlesManager = FindFirstObjectByType<SubtitlesManager>();
        save = FindFirstObjectByType<Saves>();
        animator = GetComponent<Animator>();   
        PlayerPrefs.SetString("Level", "mole");
        platforms.SetActive(false);
        bossHealth.BossHealth = 100;
        playerHealth.PlayerHP = 3;
        timer = save.TimerLoad(4);
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {        
        if (attackSpikesActivate) timerAttackSpikes += Time.deltaTime;
        if (bossStarted)
        {
            //DELAY BETWEEN ATTACKS
            if (phase == 1 && !attackIsGoing)
            {
                if (timerWaitForNextAttack > timeBetweenAttacksPhase1) nextAttack = true;
                else timerWaitForNextAttack += Time.deltaTime;
            }
            else if (phase == 2 && !attackIsGoing)
            {
                if (timerWaitForNextAttack > timeBetweenAttacksPhase2) nextAttack = true;
                else timerWaitForNextAttack += Time.deltaTime;
            }
            //SPIKES
            if (timerAttackSpikes > 16 && phase == 2)
            {
                timerAttackSpikes = 0;
                Attack_Spikes();
            }
            //BOSS CHARGE
            if (timerBossCharge > bossChargeDelay)
            {
                timerBossCharge = 0;
                bossNextAttackIsCharge = true;
            }
            else if (bossCanBossCharge) timerBossCharge += Time.deltaTime;
            //ATTACK HANDLER
            if (nextAttack)
            {
                timerWaitForNextAttack = 0;
                nextAttack = false;
                AttackChooser();
            }         
            timer += Time.deltaTime;
            save.TimerSave(timer, 4);
        }
        //BOSS HP 
        if (bossHealth.BossHealth == 0)
        {
            StartCoroutine(BossDeath());
        }
        else if (bossHealth.BossHealth < 50 && phase == 1) StartCoroutine(ChangePhase());
        {
        }
        if (playerHealth.PlayerHP == 0) playerHealth.PlayerDeath(2); //Player Death
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && phase == 1)
        {
            if (!bossHealth.BossInvincible)
            {
                bossHealth.BossHit();
                if (Random.value < 0.5f) SFXbossHit.Play();
                else SFXbossHit02.Play();
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
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) //The Player has collided with boss while the boss if Charging (moving to the side)
        {
                Debug.Log("Player Collided with boss during a charge");
                if (playerScript.Position.x < rb.position.x) playerScript.MovePlayer(5, 0); //Move Right
                else playerScript.MovePlayer(-5, 0); //Move Left
            }
    }
    public IEnumerator StartBossFight()
    {
        Attack_SideDrills();
        UI_Boss.SetActive(true);
        UI_Player.SetActive(true);
        bossUI.BossHPSliderStart();
        playerHealth.StartHPUI();
        OSTPart0.Stop();
        PreBoss.Play();
        if (PlayerPrefs.HasKey("subtitles")) subtitlesManager.Write("At Last, you did it...took you long enough, now get your ass ready for a real fight!", PreBoss.clip.length);
        yield return new WaitForSeconds(PreBoss.clip.length);        
        OSTPart1.Play();
        phase = 1;
        bossStarted = true;
    }

    public IEnumerator ChangePhase()
    {
        OSTPart1.Stop();
        SFXswitchPhase.Play();
        if (PlayerPrefs.HasKey("subtitles")) subtitlesManager.Write("You wanna play? I will show you how we play with visitors in Birmingham!", SFXswitchPhase.clip.length);
        yield return new WaitForSeconds(SFXswitchPhase.clip.length);
        OSTPart2.Play();
        phase = 2;
        PlatformFadeIn();
        attackSpikesActivate = true;
        bossNextAttackIsCharge = true; //další útok je charge
        if (bossHealth.pussyModeOn) playerHealth.PlayerHP = 3;
    }

    public IEnumerator BossDeath()
    {
        OSTPart2.Stop();
        OSTPartEND.Play();
        if (transform.position.x <= 0) animator.SetBool("deathLeft", true);
        else animator.SetBool("deathRight", true);
        SFXbossDeath.Play();
        PlatformFadeOut();
        rb.angularVelocity = 0;
        bossUI.FadeOutEffect();
        float waitTime = 0;
        if (PlayerPrefs.HasKey("PussyMode"))
        {
            SFXbossDeath.Play();
            waitTime = SFXbossDeath.clip.length;
            if (PlayerPrefs.HasKey("subtitles")) subtitlesManager.Write("YOU CANNOT CHANGE WHAT HAS BEEN DONE! *screaming*", waitTime);
            PlayerPrefs.SetString("BeatenWithAPussyMode_Brecus", "real");
        }
        else
        {
            SFXbossDeathPussyMode.Play();
            waitTime = SFXbossDeathPussyMode.clip.length;
            if (PlayerPrefs.HasKey("subtitles")) subtitlesManager.Write("THE NIGHT WILL ALWAYS PERSIST! *screaming*", waitTime);
            PlayerPrefs.DeleteKey("BeatenWithAPussyMode_Brecus");
        }
        PlayerPrefs.Save();
        yield return new WaitForSeconds(waitTime);
        bossUI.BossHPSliderDestroy();
        doorsEnd.SetActive(true);
    }
    #region AttacksLogic
    string lastAttack = null;
    void AttackChooser()
    {
        attackIsGoing = true;
        if (phase == 1)
        {
            ChooseAttackPhase1();
        }
        else if (phase == 2)
        {
            ChooseAttackPhase2();
        }
    }

    private void ChooseAttackPhase1() //RNG, but one attack cannot be played twice in a row (Also works for phase2)
    {
        List<string> possibleAttacks = new List<string> { "MoleRain", "DrillRain", "DrillGround", "DrillSide" };
        // Remove the last attack to avoid repeating it
        if (possibleAttacks.Contains(lastAttack))
            possibleAttacks.Remove(lastAttack);

        int rng = Random.Range(0, possibleAttacks.Count);
        lastAttack = possibleAttacks[rng];

        switch (lastAttack)
        {
            case "MoleRain":
                StartCoroutine(Attack_MoleRain());
                break;
            case "DrillRain":
                Attack_DrillRain();
                break;
            case "DrillGround":
                StartCoroutine(Attack_GroundDrills());
                break;
            case "DrillSide":
                StartCoroutine(Attack_SideDrills());
                break;
        }
    }

    private void ChooseAttackPhase2()
    {
        List<string> possibleAttacks = new List<string> { "DrillSide", "MoleRain", "ShovelRain", "Rock" };
        if (bossNextAttackIsCharge) StartCoroutine(Attack_MoleCharge());
        else
        {
            int rng = Random.Range(0, possibleAttacks.Count);
            lastAttack = possibleAttacks[rng];
            if (possibleAttacks.Contains(lastAttack)) possibleAttacks.Remove(lastAttack);
            switch (lastAttack)
            {
                case "DrillSide":
                    StartCoroutine(Attack_SideDrills());
                    break;
                case "Rock":
                    StartCoroutine(Attack_Rock());
                    break;
                case "ShovelRain":
                    StartCoroutine(Attack_ShovelRain());
                    break;
                case "MoleRain":
                    StartCoroutine(Attack_MoleRain());
                    break;
            }
        }
    }
    #endregion
    #region Phase1Attacks
    public void Attack_DrillRain()
    {
        for (int i = 0; i < 8; i++)
        {
            Vector2 position = new Vector2(-16.50f + (i * 4.75f), 11);
            Instantiate(prefabDrillRain, position, Quaternion.identity);
        }
        attackIsGoing = false;
    }

    IEnumerator Attack_MoleRain()
    {
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 12; j += 2)
            {
                Vector2 position = new Vector2(-16.68f + (j * 3), 11.40f);
                Instantiate(prefabMoleRain, position, Quaternion.identity);
            }
            yield return new WaitForSeconds(moleRain_waitForNextWave);
            for (int j = 1; j < 12; j += 2)
            {
                Vector2 position = new Vector2(-16.68f + (j * 3), 11.40f);
                Instantiate(prefabMoleRain, position, Quaternion.identity);
            }
            yield return new WaitForSeconds(moleRain_waitForNextWave);
        }
        yield return new WaitForSeconds(2);
        attackIsGoing = false;
    }

    IEnumerator Attack_ShovelRain()
    {
        float attackMoleRainShift = 1.7f;
        float x = -17f;
        float y = 12.78f;
        for (int j = 0; j < 22; j += 2)
        {
            Vector2 position = new Vector2(x + (j * attackMoleRainShift), y);
            Instantiate(prefabShovelRain, position, Quaternion.identity);
            yield return new WaitForSeconds(0.25f);
        }
        yield return new WaitForSeconds(4);
        x = 18.7f;
        for (int j = 1; j < 23; j += 2)
        {
            Vector2 position = new Vector2(x - (j * attackMoleRainShift), y);
            Instantiate(prefabShovelRain, position, Quaternion.identity);
            yield return new WaitForSeconds(0.25f);
        }
        yield return new WaitForSeconds(2);
        attackIsGoing = false;
    }

    IEnumerator Attack_SideDrills()
    {
        Debug.Log("Attack_SideDrills");
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
                yield return new WaitForSeconds(1);
            }
        }
        else if (playerScript.Position.x > 6) // Right
        {
            for (int i = 3; i < 6; i++)
            {
                Instantiate(prefabDrillSide, position[i], Quaternion.identity);
                yield return new WaitForSeconds(1);
            }
        }
        else // Left
        {
            for (int i = 0; i < 3; i++)
            {
                Instantiate(prefabDrillSide, position[i], Quaternion.identity);
                yield return new WaitForSeconds(1);
            }
        }
        attackIsGoing = false;
    }
    #endregion
    #region Phase2Attacks
    IEnumerator Attack_GroundDrills()
    {
        if (playerScript.Position.x >= 0)
        {
            for (int i = 0; i < 8; i++)
            {
                yield return new WaitForSeconds(groundDrills_waitForNextDrill);
                Instantiate(prefabDrillGround, new Vector2(16.76f - (4.75f * i), -11), Quaternion.identity); //right            
            }
        }
        else
        {
            for (int i = 0; i < 8; i++)
            {
                yield return new WaitForSeconds(groundDrills_waitForNextDrill);
                Instantiate(prefabDrillGround, new Vector2(-16.76f + (4.75f * i), -11), Quaternion.identity); //left        
            }            
        }
        attackIsGoing = false;
    }
    IEnumerator Attack_MoleCharge()
    {
        bossNextAttackIsCharge = false;
        Debug.Log("Attack: MoleCharge");
        bossCanBossCharge = false;
        if (playerScript.Position.x > transform.position.x) //right (player je vpravo od Bosse)
        {
            animator.SetTrigger("readyChargeRight");
            yield return new WaitForSeconds(moleCharge_TimeBeforeCharge); //charge time
            animator.SetBool("chargingRight", true);
            rb.velocity = Vector2.right * moleCharge_Velocity;
            while (rb.velocity != Vector2.zero)
            {
                if (rb.position.x > 15.55f)
                {
                    rb.velocity = Vector2.zero;
                }
                else yield return null;
            }       
            animator.SetBool("chargingRight", false);
            attackIsGoing = false;
            yield return new WaitForSeconds(moleCharge_TimeBeforeIdle); //V cyklu, protoze mi mrdalo s MovePlayer
            animator.SetTrigger("stopChargeRight"); //Pøes trigger, protože jsem ho chtìl vyzkoušet
        }
        else //left
        {
            animator.SetTrigger("readyChargeLeft");
            yield return new WaitForSeconds(moleCharge_TimeBeforeCharge); //charge time
            animator.SetBool("chargingLeft", true);                                                            
            rb.velocity = Vector2.left * moleCharge_Velocity;
            while (rb.velocity != Vector2.zero) //charge az do urcityho mista
            {
                if (rb.position.x < -15.55f)
                {
                    rb.velocity = Vector2.zero;                    
                }
                else yield return null;
            }
            bossNextAttackIsCharge = false;            
            animator.SetBool("chargingLeft", false);
            attackIsGoing = false;
            yield return new WaitForSeconds(moleCharge_TimeBeforeIdle); //V cyklu, protoze mi mrdalo s MovePlayer
            animator.SetTrigger("stopChargeLeft");
        }
        bossCanBossCharge = true; //Boss charge utok zapnut
    }

    IEnumerator Attack_Rock()
    {
        Vector2 position;
        float y = -9;
        if (playerScript.Position.x < -6.67) //left
        {
            position = new Vector3(-13.24f, y, -1);
            Instantiate(prefabRockDirt, new Vector3(position.x, -4.62f, 15), Quaternion.identity);
            yield return new WaitForSeconds(rock_ChargeTime);
            PlayerPrefs.SetString("rockAttack", "left"); //Im not sorry
            PlayerPrefs.Save();
            Instantiate(prefabROCK, position, Quaternion.identity);            
        }
        else if (playerScript.Position.x > 6.67) //right
        {
            position = new Vector3(13.24f, y, -1);
            Instantiate(prefabRockDirt, new Vector3(position.x, -4.62f, 15), Quaternion.identity);
            yield return new WaitForSeconds(rock_ChargeTime);
            PlayerPrefs.SetString("rockAttack", "right");
            PlayerPrefs.Save();
            Instantiate(prefabROCK, position, Quaternion.identity);
        }
        else // middle
        {
            position = new Vector3(0, y, -1);
            Instantiate(prefabRockDirt, new Vector3(position.x, -4.62f, 15), Quaternion.identity);
            yield return new WaitForSeconds(rock_ChargeTime);
            PlayerPrefs.SetString("rockAttack", "middle");
            PlayerPrefs.Save();
            Instantiate(prefabROCK, position, Quaternion.identity);
        }
        attackIsGoing = false;
    }

    private void Attack_Spikes()
    {
        Debug.Log("Spikes");
        Vector2[] position = new Vector2[6];
        float y = -2.62f;
        position[0] = new Vector2(-8.2544f, y);
        position[1] = new Vector2(8.2544f, y);
        position[2] = new Vector2(-6.4772f, y);
        position[3] = new Vector2(6.4772f, y);
        position[4] = new Vector2(-4.701f, y);
        position[5] = new Vector2(4.701f, y);
        for (int i = 0; i < 6; i++)
        {
            Instantiate(prefabSpike, position[i], Quaternion.identity);
        }
    }

    IEnumerator PlatformFadeIn()
    {
        platformsCanvasGroup.alpha = 0f;
        platforms.SetActive(true);
        while (platformsCanvasGroup.alpha < 1f)
        {
            platformsCanvasGroup.alpha += 0.01f;
            yield return new WaitForSeconds(0.01f / 1);
        }
        platformsCanvasGroup.alpha = 1f;
    }

    IEnumerator PlatformFadeOut()
    {
        platformsCanvasGroup.alpha = 1f;
        platforms.SetActive(true);
        while (platformsCanvasGroup.alpha < 1f)
        {
            platformsCanvasGroup.alpha += 0.01f;
            yield return new WaitForSeconds(0.01f / 1);
        }
        platforms.SetActive(false);
        platformsCanvasGroup.alpha = 0f;
    }
    #endregion
    public void BossHitBeforeCharge()
    {
        bossHealth.BossHit();
        bossNextAttackIsCharge = false;
        animator.SetBool("chargeRight", false);
        animator.SetBool("chargeLeft", false);
    }
}