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
    public Saves save;
    //INSPECTOR//
    [Header("Audio")]
    [SerializeField] AudioSource SFXbossHit;
    [SerializeField] AudioSource SFXswitchPhase;
    [SerializeField] AudioSource SFXbossDeath;
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
    [SerializeField] Rigidbody2D colliderCharge;
    [Header("Settings")]
    public float timeToFirstAttack;
    public float bossChargeDelay;
    public float timeBetweenAttacksPhase1;
    public float timeBetweenAttacksPhase2;

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
    bool attackSpikesOn = false;
    bool bossCharge = false;
    bool nextAttack = false;
    Rigidbody2D rb;
    private void Start()
    {
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
        if (attackSpikesOn) timerAttackSpikes += Time.deltaTime;
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
                if (phase == 2 && bossCharge)
                {
                    Attack_MoleCharge();
                }
                else AttackChooser();
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
            SFXswitchPhase.Play();
            OSTPart1.Stop();
            OSTPart2.Play();
            if (bossHealth.pussyModeOn) playerHealth.PlayerHP = 3;
            //sprites
        }
        if (playerHealth.PlayerHP == 0) playerHealth.PlayerDeath(2); //Player Death
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!bossHealth.BossInvincible)
            {
                bossHealth.BossHit();
                SFXbossHit.Play();
            }
            else
            {
                playerHealth.PlayerDamage();
            }
        }
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
        Debug.Log("AttackChooser");
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
            if (timerAttackSpikes > 10)
            {
                timerAttackSpikes = 0;
                attackSpikesOn = true;
                Attack_Spikes();
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
            yield return new WaitForSeconds(2);
            for (int j = 1; j < 12; j += 2)
            {
                Vector2 position = new Vector2(-16.68f + (j * 3), 11.40f);
                Instantiate(prefabMoleRain, position, Quaternion.identity);
            }
            yield return new WaitForSeconds(2);
        }
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
        attackNumberDrillSide++;
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.5f);
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
            yield return new WaitForSeconds(2);
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
        if (colliderLeft)
        {
            rb.angularVelocity = 0;
            //animation
            yield return new WaitForSeconds(2); //charge time
            colliderCharge.velocity = Vector2.left * Time.deltaTime * 20;
            rb.position = new Vector2(-15.55f, -3.8f);
            while (colliderCharge.position.x < -15.55f) yield return null;
            colliderCharge.position = new Vector2(-15.55f, colliderCharge.position.y);
        }
        else
        {
            rb.angularVelocity = 0;
            //animation
            yield return new WaitForSeconds(2); //charge time
            colliderCharge.velocity = Vector2.left * Time.deltaTime * 20;
            rb.position = new Vector2(15.55f, -3.8f);
            while (colliderCharge.position.x < 15.55f) yield return null;
            colliderCharge.position = new Vector2(15.55f, colliderCharge.position.y);
        }
    }

    IEnumerator Attack_Rock()
    {
        Vector2 position;
        float y = -9;
        if (colliderMiddleLeft)
        {
            position = new Vector2(-13.24f, y);
            //animation
            yield return new WaitForSeconds(2);
            Instantiate(prefabROCK, position, Quaternion.identity);
        }
        else if (colliderMiddleMiddle)
        {
            position = new Vector2(0, y);
            //animation
            yield return new WaitForSeconds(2);
            Instantiate(prefabROCK, position, Quaternion.identity);
        }
        else //Middle Right
        {
            position = new Vector2(13.24f, y);
            //animation
            yield return new WaitForSeconds(2);
            Instantiate(prefabROCK, position, Quaternion.identity);
        }
    }

    private void Attack_Spikes()
    {
        if (colliderMiddleRight || colliderMiddleLeft)
        {
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
        attackIsGoing = false;
    }

    private void EndingDecider()
    {
        if (PlayerPrefs.HasKey("BeatenWithAPussyMode_Brecus") || PlayerPrefs.HasKey("BeatenWithAPussyMode_Mole"))
        {
            SceneManager.LoadScene("Cutscene_BadEnding");
        }
        else SceneManager.LoadScene("Cutscene_GoodEnding");
    }
}