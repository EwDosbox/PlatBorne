using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class Mole_Bossfight : MonoBehaviour
{
    //SCRIPTS//
    [SerializeField] Mole_Health bossHealth;
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] Mole_WeakSpot weakSpot;
    [SerializeField] Mole_UI bossUI;
    [SerializeField] Saves save;
    //INSPECTOR//
    [Tooltip("Audio")]
    [SerializeField] AudioSource SFXbossHit;
    [SerializeField] AudioSource SFXswitchPhase;
    [SerializeField] AudioSource SFXbossDeath;
    [SerializeField] AudioSource OSTPart1;
    [SerializeField] AudioSource OSTPart2;
    [Tooltip("GameObjects")]
    [SerializeField] GameObject[] platforms;
    [SerializeField] GameObject drill;
    [SerializeField] GameObject shovel;
    [SerializeField] GameObject smallMole;
    [Tooltip("Data")]
    [SerializeField] float timeForNextAttackPhase1 = 6;
    [SerializeField] float timeForNextAttackPhase2 = 4;
    [SerializeField] int attackDrillRainNumberOfSpawns = 0;
    //Drill Rain
    [SerializeField] float attackDrillRainPositionX = 0;
    [SerializeField] float attackDrillRainPositionY = 0;
    [SerializeField] float attackDrillRainShift = 0;
    [SerializeField] float attackDrillRainSpeed = 0;
    //Drill Ground
    [SerializeField] float attackDrillGroundPositionX = 0;
    [SerializeField] float attackDrillGroundPositionY = 0;
    [SerializeField] float attackDrillGroundShift = 0;
    [SerializeField] float attackDrillGroundSpeed = 0;
    //Drill Side
    [SerializeField] float attackDrillSidePositionX = 0;
    [SerializeField] float[] attackDrillSidePositionY;
    [SerializeField] float attackDrillSideShift = 0;
    [SerializeField] float attackDrillSideSpeed = 0;
    //Mole Charge
    [SerializeField] float attackMoleChargeSpeed;
    [SerializeField] float attackMoleChargeStuntTime;
    float attackMoleChargeTimer;
    [SerializeField] float attackRockDelay = 0;
    [SerializeField] float attackSpikesDelay = 0;
    [SerializeField] float attackMoleRainDelay = 0;
    //Shovel Rain - uses same stats as Drill Rain

    //PREFABS//
    [SerializeField] GameObject prefabDrillGround;
    [SerializeField] GameObject prefabDrillRain;
    [SerializeField] GameObject prefabDrillSide;
    [SerializeField] GameObject prefabMoleRain;
    [SerializeField] GameObject prefabSpike;
    [SerializeField] GameObject prefabPlatforms;
    [SerializeField] GameObject prefabROCK;
    [SerializeField] GameObject prefabShovelRain;
    //OTHER
    [SerializeField] GameObject levelMove;
    //PUBLIC//

    //COLLIDERS//
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
    //PRIVATE//
    private float timer = 0;
    private bool timerOn = false;
    private bool bossStarted = false;
    private bool attackIsGoing = false;
    private int phase = 1;
    private bool attackSpikesOn = false;
    private float attackSpikesTimer = 0;
    private float bossChargeTimer = 0;
    private float bossChargeDelay = 15;
    private bool bossCharge = false;
    Rigidbody2D rb;

    private void Awake()
    {
        levelMove.SetActive(false);
        timer = save.TimerLoad(4);
        rb = GetComponent<Rigidbody2D>();
        PlayerPrefs.SetString("Level", "mole");
        StartCoroutine(bossUI.BossHPSliderStart());
        StartBossFight();
        phase = 1;
    }
    private void FixedUpdate()
    {
        if (attackSpikesOn) attackSpikesTimer += Time.deltaTime;
        if (bossStarted)
        {
            if (bossCharge)
                if (!attackIsGoing)
                {
                    if (bossChargeTimer > bossChargeDelay && phase == 2)
                    {
                        bossChargeTimer = 0;
                        Attack_MoleCharge();
                    }
                    AttackChooser();
                }
            timer += Time.deltaTime;
            save.TimerSave(timer, 4);
        }
        if (bossHealth.BossHealth == 0)
        {
            BossDeath();
            SFXbossDeath.Play();
        }
        else if (bossHealth.BossHealth < 50)
        {
            phase = 2;
            SFXswitchPhase.Play();
            OSTPart1.Stop();
            OSTPart2.Play();
            if (playerHealth.PussyMode) playerHealth.PlayerHP = 3;
            //sprites
        }
        if (playerHealth.PlayerHP == 0) playerHealth.PlayerDeath(2);
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BossStop"))
        {
            rb.angularVelocity = 0;
            if (rb.position.x < -50)
            {
                //boss is stuck in right
            }
            else
            {
                //boss is stuck in left
            }
        }
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
    public void StartBossFight()
    {
        bossStarted = true;
        bossHealth.BossHealth = 100;
        OSTPart1.Play();
        bossUI.BossHPSliderStart();
        playerHealth.PlayerHPStart();
    }

    IEnumerator BossDeath()
    {
        rb.angularVelocity = 0;
        yield return new WaitForSeconds(2);
        bossUI.BossHPSliderDestroy();
        Destroy(rb);
        levelMove.SetActive(true);
    }


    //Attack variables
    int attackNumberShovelRain = 0;
    int attackNumberMoleRain = 0;
    int attackNumberDrillSide = 0;
    int attackNumberDrillGround = 0;
    int attackNumberDrillRain = 0;
    int attackNumberRock = 0;
    string lastAttack = null;
    IEnumerator AttackChooser()
    {
        attackIsGoing = true;
        if (phase == 1)
        {
            yield
            return new WaitForSeconds(timeForNextAttackPhase1);
            //if same
            if ((attackNumberMoleRain == attackNumberDrillRain) && (attackNumberMoleRain == attackNumberDrillGround) || ((attackNumberDrillGround + attackNumberDrillRain + attackNumberMoleRain) / 3) < attackNumberDrillSide + 2)
            {
                switch (UnityEngine.Random.Range(1, 4))
                {
                    case 1:
                        lastAttack = "MoleRain";
                        attackNumberMoleRain++;
                        Attack_MolesRain();
                        break;
                    case 2:
                        lastAttack = "DrillRain";
                        attackNumberDrillRain++;
                        Attack_DrillRain();
                        break;
                    case 3:

                        lastAttack = "DrillGround";
                        attackNumberDrillGround++;
                        Attack_GroundDrills();
                        break;
                }
            }
            else if (((attackNumberDrillRain + attackNumberDrillGround) / 2) > attackNumberMoleRain && lastAttack != "MoleRain")
            {
                lastAttack = "MoleRain";
                attackNumberMoleRain++;
                Attack_MolesRain();
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
                Attack_GroundDrills();
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
            yield
            return new WaitForSeconds(timeForNextAttackPhase2);
            if (colliderMiddleLeft && colliderMiddleRight && lastAttack != "ShovelRain")
            {
                lastAttack = "ShovelRain";
                attackNumberShovelRain++;
                Attack_ShovelRain();
            }
            else if (lastAttack != "Rock")
            {
                lastAttack = "Rock";
                attackNumberRock++;
                Attack_Rock();
            }
            else if (lastAttack != "ShovelRain" || lastAttack != "MoleRain")
            {
                lastAttack = "MoleRain";
                attackNumberMoleRain++;
                Attack_MolesRain();
            }
            else //drillSide
            {
                lastAttack = "DrillSide";
                attackNumberDrillSide++;
                Attack_SideDrills();
            }
            if (attackSpikesTimer > attackSpikesDelay)
            {
                attackSpikesTimer = 0;
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
        for (int i = 0; i < attackDrillRainNumberOfSpawns; i++)
        {
            Vector2 position = new Vector2(attackDrillRainPositionX + (i * attackDrillRainShift), attackDrillRainPositionY);
            Instantiate(prefabDrillRain, position, Quaternion.identity);
        }
        attackIsGoing = false;
    }

    IEnumerator Attack_MolesRain()
    {
        attackNumberMoleRain++;
        for (int i = 0; i < 16; i++)
        {
            int attackMoleRainShift = 2;
            float x = -16.68f;
            for (int j = 0; j < 16; j += 2)
            {
                Vector2 position = new Vector2(x, -16.68f + (j * attackMoleRainShift));
                Instantiate(prefabMoleRain, position, Quaternion.identity);
            }
            yield return new WaitForSeconds(attackMoleRainDelay);
            for (int j = 1; j < 16; j += 2)
            {
                Vector2 position = new Vector2(x, -16.68f + (j * attackMoleRainShift));
                Instantiate(prefabMoleRain, position, Quaternion.identity);
            }
        }
        attackIsGoing = false;
    }

    private void Attack_SideDrills()
    {
        attackNumberDrillSide++;
        Vector2[] position = new Vector2[4];
        position[0] = new Vector2(attackDrillSidePositionX, attackDrillGroundPositionY);
        position[1] = new Vector2(attackDrillSidePositionX + attackDrillSideShift, attackDrillGroundPositionY);
        position[2] = new Vector2(attackDrillSidePositionX, -attackDrillGroundPositionY);
        position[3] = new Vector2(attackDrillSidePositionX + attackDrillSideShift, -attackDrillGroundPositionY);
        for (int i = 0; i > 4; i++)
        {
            Instantiate(prefabDrillSide, position[i], Quaternion.identity);
        }
        attackIsGoing = false;
    }
    //PHASE II
    IEnumerator Attack_GroundDrills()
    {
        attackNumberDrillSide++;
        for (int i = 0; i > 20; i++)
        {
            yield
            return new WaitForSeconds(attackDrillGroundSpeed);
            if (colliderRight) Instantiate(prefabDrillSide, new Vector2(attackDrillGroundPositionX + (attackDrillGroundShift * i), attackDrillGroundPositionY), Quaternion.identity); //right
            else Instantiate(prefabDrillSide, new Vector2(attackDrillGroundPositionX + (attackDrillGroundShift * i), attackDrillGroundPositionY), Quaternion.identity); //left
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
            yield return new WaitForSeconds(attackMoleRainDelay);
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
            yield return new WaitForSeconds(attackMoleChargeTimer);
            rb.velocity = Vector2.left * attackMoleChargeSpeed * Time.deltaTime;
        }
        else
        {
            yield return new WaitForSeconds(attackMoleChargeTimer);
            rb.velocity += Vector2.right * attackMoleChargeSpeed * Time.deltaTime;
        }
    }

    private void Attack_Rock()
    {
        Vector2 position;
        float y = -9;
        if (colliderMiddleLeft)
        {
            position = new Vector2(-13.24f, y);
            //animation
            Instantiate(prefabROCK, position, Quaternion.identity);
        }
        else if (colliderMiddleMiddle)
        {
            position = new Vector2(0, y);
            //animation
            Instantiate(prefabROCK, position, Quaternion.identity);
        }
        else //Middle Right
        {
            position = new Vector2(13.24f, y);
            //animation
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
}