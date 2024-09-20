using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Mole_Bossfight : MonoBehaviour
{
    [Header("Scipts")]
    public Mole_Health bossHealth;
    public PlayerHealth playerHealth;
    public Mole_WeakSpot weakSpot;
    public Mole_UI bossUI;
    Saves save;
    SubtitlesManager subtitlesManager;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] PlayerScript playerScript;
    //INSPECTOR//
    [Header("Audio")]
    [SerializeField] AudioSource SFXbossHit;
    [SerializeField] AudioSource PreBoss;
    [SerializeField] AudioSource SFXswitchPhase;
    [SerializeField] AudioSource SFXbossDeath;
    [SerializeField] AudioSource SFXbossDeathPussyMode;
    [SerializeField] AudioSource OSTPart1;
    [SerializeField] AudioSource OSTPart2;
    [Header("Prefabs")]
    [SerializeField] GameObject prefabDrillGround;
    [SerializeField] GameObject prefabDrillRain;
    [SerializeField] GameObject prefabDrillSide;
    [SerializeField] GameObject prefabMoleRain;
    [SerializeField] GameObject prefabSpike;
    [SerializeField] GameObject prefabPlatforms;
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

    private bool colliderRight = false;
    private bool colliderLeft = false;
    private bool colliderMiddleRight = false;
    private bool colliderMiddleLeft = false;
    private bool colliderMiddleMiddle = false;
    private bool colliderPlatform = false;
    private bool colliderGround = false;

    public bool ColliderRight
    {
        set
        {
            colliderRight = value;
        }
    }
    public bool ColliderLeft
    {
        set
        {
            colliderLeft = value;
        }
    }
    public bool ColliderMiddleRight
    {
        set
        {
            colliderMiddleRight = value;
        }
    }
    public bool ColliderMiddleLeft
    {
        set
        {
            colliderMiddleLeft = value;
        }
    }
    public bool ColliderMiddleMiddle
    {
        set
        {
            colliderMiddleMiddle = value;
        }
    }
    public bool ColliderPlatform
    {
        set
        {
            colliderPlatform = value;
        }
    }
    public bool ColliderGround
    {
        set
        {
            colliderGround = value;
        }
    }
    //timers
    float timer = 0;
    float timerWaitForNextAttack = 0;
    float timerBossCharge = 0;
    float timerAttackSpikes = 0;

    bool timerOn = false;
    bool bossCanBossCharge = false;
    bool bossStarted = false;
    bool attackIsGoing = false;
    int phase = 0;
    bool attackSpikesActivate = false;
    bool bossCharge = false;
    bool nextAttack = false;
    bool isPlayerCollidingWithBossDuringCharge = false;
    Rigidbody2D rb;
    BoxCollider2D boxCollider2D;
    //ANIMATOR
    Animator animator;
    private void Start()
    {
        subtitlesManager = FindFirstObjectByType<SubtitlesManager>();
        save = FindFirstObjectByType<Saves>();
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        bossUI.FadeOutEffect();
        PlayerPrefs.SetString("Level", "mole");
        prefabPlatforms.SetActive(false);
        bossHealth.BossHealth = 100;   
        playerHealth.PlayerHP = 3;
        timer = save.TimerLoad(4);
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(StartBossFight());
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
            //BOSS CHARGE
            if (timerBossCharge > bossChargeDelay)
            {
                timerBossCharge = 0;
                bossCharge = true;
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
        else if (bossHealth.BossHealth < 50 && phase == 1) //Change Phase
        {
            phase = 2;
            attackSpikesActivate = true;
            //SFXswitchPhase.Play();
            //OSTPart1.Stop();
            //OSTPart2.Play();
            prefabPlatforms.SetActive(true);
            bossCharge = true; //další útok je charge
            if (bossHealth.pussyModeOn) playerHealth.PlayerHP = 3;
            //sprites
        }
        if (playerHealth.PlayerHP == 0) playerHealth.PlayerDeath(2); //Player Death
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && phase == 1)
        {
            if (!bossHealth.BossInvincible)
            {
                bossHealth.BossHit();
                //SFXbossHit.Play();
            }
            else
            {
                playerHealth.PlayerDamage();
            }
        }
        if (phase == 2) playerHealth.PlayerDamage();
    }
    public IEnumerator StartBossFight()
    {      
        //OSTPart1.Play();
        bossUI.BossHPSliderStart();
        playerHealth.StartHPUI();
        yield return new WaitForSeconds(timeToFirstAttack);
        phase = 1;
        bossStarted = true;
    }

    public IEnumerator BossDeath()
    {
        if (transform.position.x <= 0) animator.SetBool("deathLeft", true);
        else animator.SetBool("deathRight", true);
        SFXbossDeath.Play();
        rb.angularVelocity = 0;
        bossUI.FadeInEffect();
        yield return new WaitForSeconds(4);
        bossUI.BossHPSliderDestroy();
        if (PlayerPrefs.HasKey("PussyMode"))
        {
            PlayerPrefs.SetString("BeatenWithAPussyMode_Brecus", "real");
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.DeleteKey("BeatenWithAPussyMode_Brecus");
            PlayerPrefs.Save();
        }
        EndingDecider();
    }

    //Attack variables
    int attackNumberShovelRain = 0;
    int attackNumberMoleRain = 0;
    int attackNumberDrillSide = 0;
    int attackNumberDrillGround = 0;
    int attackNumberDrillRain = 0;
    int attackNumberRock = 0;
    string lastAttack = null;
    void AttackChooser()
    {
        attackIsGoing = true;
        if (phase == 1)
        {
            //if same
            if ((attackNumberMoleRain == attackNumberDrillRain) && (attackNumberMoleRain == attackNumberDrillGround) || ((attackNumberDrillGround + attackNumberDrillRain + attackNumberMoleRain) / 3) < attackNumberDrillSide + 2)
            {
                switch (UnityEngine.Random.Range(1, 4))
                {
                    case 1:
                        lastAttack = "MoleRain";
                        attackNumberMoleRain++;
                        StartCoroutine(Attack_MoleRain());
                        break;
                    case 2:
                        lastAttack = "DrillRain";
                        attackNumberDrillRain++;
                        Attack_DrillRain();
                        break;
                    case 3:
                        lastAttack = "DrillGround";
                        attackNumberDrillGround++;
                        StartCoroutine(Attack_GroundDrills());
                        break;
                }
            }
            else if (((attackNumberDrillRain + attackNumberDrillGround) / 2) > attackNumberMoleRain && lastAttack != "MoleRain")
            {
                lastAttack = "MoleRain";
                attackNumberMoleRain++;
                StartCoroutine(Attack_MoleRain());
            }
            else if ((attackNumberMoleRain + attackNumberDrillGround / 2) > attackNumberDrillRain && (lastAttack != "MoleRain" || lastAttack != "DrillRain"))
            {
                lastAttack = "DrillRain";
                attackNumberDrillRain++;
                Attack_DrillRain();
            }
            else if (((attackNumberDrillRain + attackNumberDrillSide) / 2) > attackNumberDrillGround && lastAttack != "DrillGround")
            {
                lastAttack = "DrillGround";
                attackNumberDrillGround++;
                StartCoroutine(Attack_GroundDrills());
            }
            else //DrillSide
            {
                lastAttack = "DrillSide";
                attackNumberDrillSide++;
                Attack_SideDrills();
            }
        }
        else if (phase == 2)
        {
            if (timerAttackSpikes > 16)
            {
                timerAttackSpikes = 0;
                Attack_Spikes();
                AttackChooser();
            }
            if (bossCharge)
            {
                StartCoroutine(Attack_MoleCharge());
            }
            if (colliderMiddleLeft && colliderMiddleRight && lastAttack != "ShovelRain")
            {
                lastAttack = "ShovelRain";
                attackNumberShovelRain++;
                StartCoroutine(Attack_ShovelRain());
            }
            else if (lastAttack != "Rock")
            {
                lastAttack = "Rock";
                attackNumberRock++;
                StartCoroutine(Attack_Rock());
            }
            else if (lastAttack != "ShovelRain" || lastAttack != "MoleRain")
            {
                lastAttack = "MoleRain";
                attackNumberMoleRain++;
                StartCoroutine(Attack_MoleRain());
            }
            else //drillSide
            {
                lastAttack = "DrillSide";
                attackNumberDrillSide++;
                Attack_SideDrills();
            }            
        }
    }

    //ATTACKS//
    //PHASE I
    public void Attack_DrillRain()
    {
        attackNumberDrillRain++;
        for (int i = 0; i < 11; i++)
        {
            Vector2 position = new Vector2(-16.50f + (i * 3.30f), 11);
            Instantiate(prefabDrillRain, position, Quaternion.identity);
        }
        attackIsGoing = false;
    }

    IEnumerator Attack_MoleRain()
    {
        attackNumberMoleRain++;
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

    private void Attack_SideDrills()
    {
        attackNumberDrillSide++;
        Vector2[] position = new Vector2[6];
        position[0] = new Vector2(-19.5f, -5.72f);
        position[1] = new Vector2(-19.5f, -3.4f);
        position[2] = new Vector2(-19.5f, -1.08f);
        //right wall
        position[3] = new Vector2(19.5f, -5.72f);
        position[4] = new Vector2(19.5f, -3.4f);
        position[5] = new Vector2(19.5f, -1.08f);
        if (colliderMiddleMiddle)
        {
            for (int i = 0; i > 6; i++)
            {
                Instantiate(prefabDrillSide, position[i], Quaternion.identity);
            }
        }
        else if (colliderMiddleRight)
        {
            for (int i = 0; i > 3; i++)
            {
                Instantiate(prefabDrillSide, position[i], Quaternion.identity);
            }
        }
        else
        {
            for (int i = 3; i > 6; i++)
            {
                Instantiate(prefabDrillSide, position[i], Quaternion.identity);
            }
        }                
        attackIsGoing = false;
    }
    //PHASE II
    IEnumerator Attack_GroundDrills()
    {
        attackNumberDrillGround++;
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(groundDrills_waitForNextDrill);
            if (colliderRight) Instantiate(prefabDrillGround, new Vector2(16.76f - (3 * i), -11), Quaternion.identity); //right
            else Instantiate(prefabDrillGround, new Vector2(-16.76f + (3 * i), -11), Quaternion.identity); //left
        }
        attackIsGoing = false;
    }

    IEnumerator Attack_ShovelRain()
    {
        attackNumberShovelRain++;
        for (int i = 0; i < 16; i++)
        {
            int attackMoleRainShift = 2;
            float x = -16.68f;
            for (int j = 0; j < 16; j += 2)
            {
                Vector2 position = new Vector2(x, -16.68f + (j * attackMoleRainShift));
                Instantiate(prefabShovelRain, position, Quaternion.identity);
            }
            yield return new WaitForSeconds(shovelRain_waitForNextWave);
            for (int j = 1; j < 16; j += 2)
            {
                Vector2 position = new Vector2(x, -16.68f + (j * attackMoleRainShift));
                Instantiate(prefabShovelRain, position, Quaternion.identity);
            }
        }
        attackIsGoing = false;
    }

    IEnumerator Attack_MoleCharge()
    {
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
                    if (playerScript.Position.x > 14) playerScript.MovePlayer(-5, 0); //hrac bude posunut aby nebyl v Mole
                }
                else yield return null;
            }
            bossCharge = false;            
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
                    if (playerScript.Position.x < -14) playerScript.MovePlayer(5, 0);
                }
                else yield return null;
            }
            bossCharge = false;            
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
            Instantiate(prefabRockDirt, new Vector2(position.x, -4.62f), Quaternion.identity);
            yield return new WaitForSeconds(rock_ChargeTime);
            PlayerPrefs.SetString("rockAttack", "left"); //Im not sorry
            PlayerPrefs.Save();
            Instantiate(prefabROCK, position, Quaternion.identity);            
        }
        else if (playerScript.Position.x > 6.67) //right
        {
            position = new Vector3(13.24f, y, -1);
            Instantiate(prefabRockDirt, new Vector2(position.x, -4.62f), Quaternion.identity);
            yield return new WaitForSeconds(rock_ChargeTime);
            PlayerPrefs.SetString("rockAttack", "right");
            PlayerPrefs.Save();
            Instantiate(prefabROCK, position, Quaternion.identity);
        }
        else // middle
        {
            position = new Vector3(0, y, -1);
            Instantiate(prefabRockDirt, new Vector2(position.x, -4.62f), Quaternion.identity);
            yield return new WaitForSeconds(rock_ChargeTime);
            PlayerPrefs.SetString("rockAttack", "middle");
            PlayerPrefs.Save();
            Instantiate(prefabROCK, position, Quaternion.identity);
        }

        attackIsGoing = false;
    }

    private void Attack_Spikes()
    {
        Debug.Log("SPikes");
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

    private void EndingDecider()
    {
        if (PlayerPrefs.HasKey("BeatenWithAPussyMode_Brecus") || PlayerPrefs.HasKey("BeatenWithAPussyMode_Mole"))
        {
            SceneManager.LoadScene("Cutscene_BadEnding");
        }
        else SceneManager.LoadScene("Cutscene_GoodEnding");
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerHealth.PlayerDamage();
        }
    }

    public void BossHitWhileCharge()
    {
        bossCharge = false;
        animator.SetBool("chargeRight", false);
        animator.SetBool("chargeLeft" , false);
    }
}