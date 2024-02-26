using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Mole_Bossfight : MonoBehaviour
{
    //SCRIPTS//
    Mole_Health bossHealth = new Mole_Health();
    PlayerHealth playerHealth = new PlayerHealth();
    Mole_WeakSpot weakSpot = new Mole_WeakSpot();
    Mole_UI bossUI = new Mole_UI();
    Saves save = new Saves();
    //INSPECTOR//
    [Tooltip("Audio")]
    [SerializeField] AudioClip SFXbossHit;
    [SerializeField] AudioClip SFXswitchPhase;
    [SerializeField] AudioClip SFXbossDeath;
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
    //Mole Rain
    [SerializeField] float attackMoleRainPositionX;
    [SerializeField] int attackMoleRainCycles;
    [SerializeField] float attackMoleRainPositionY = 0;
    [SerializeField] float attackMoleRainShift = 0;
    [SerializeField] float attackMoleRainFallSpeed = 0;
    [SerializeField] float attackMoleRainDelay = 0;
    [SerializeField] int attackMoleRainNumberOfSpawns = 0;
    //Mole Charge
    [SerializeField] float attackMoleChargeSpeed;
    [SerializeField] float attackMoleChargeStuntTime;
    float attackMoleChargeTimer;
    //Rock
    [SerializeField] float[] attackRockPositionX;
    [SerializeField] float attackRockPositionY = 0;
    [SerializeField] float attackRockDelay = 0;
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
    //PUBLIC//

    //COLLIDERS//
    private bool colliderRight = false;
    private bool colliderLeft = false;
    private bool colliderMiddleRight = false;
    private bool colliderMiddleLeft = false;
    private bool colliderMiddleMiddle = false;
    private bool colliderPlatform = false;
    private bool colliderGround = false;

    public bool ColliderRight {set {colliderRight = value; } }
    public bool ColliderLeft {set {colliderLeft = value; } }
    public bool ColliderMiddleRight {set {colliderMiddleRight = value; } }
    public bool ColliderMiddleLeft {set {colliderMiddleLeft = value; } }
    public bool ColliderMiddleMiddle {set {colliderMiddleMiddle = value; } }
    public bool ColliderPlatform {set {colliderPlatform = value; } }
    public bool ColliderGround {set {colliderGround = value; } }
    //PRIVATE//
    private float timer = 0;
    private bool timerOn = false;
    private bool bossStarted = false;
    private bool attackIsGoing = false;
    private int phase = 1;
    Rigidbody2D rb;


    private void Awake()
    {
        timer = save.TimerLoad(4);
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (bossStarted)
        {
            if (!attackIsGoing)
            {

            }
            timer += Time.deltaTime;
            save.TimerSave(timer,4);
        }
    }

    public void StartBossFight()
    {
        bossStarted = true;
    }
    //Attack variables
    int attackNumberShovel = 0;
    int attackNumberMoleRain = 0;
    int attackNumberDrillSide = 0;
    int attackNumberDrillGround = 0;
    int attackNumberDrillRain = 0;
    int attackNumberSpikes = 0;
    int attackNumberRock = 0;
    string lastAttack = null;
    IEnumerator AttackChooser()
    {
        attackIsGoing = true;
        if (phase == 1)
        {
            yield return new WaitForSeconds(timeForNextAttackPhase1);
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
        else if(phase == 2)
        {
            yield return new WaitForSeconds(timeForNextAttackPhase2);
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
        for (int i = 0; i < attackMoleRainCycles; i++)
        {
            for (int j = 0; j < attackMoleRainNumberOfSpawns; j += 2)
            {
                Vector2 position = new Vector2(attackMoleRainPositionX + (j * attackMoleRainShift), attackMoleRainPositionY);
                Instantiate(prefabMoleRain, position, Quaternion.identity);
            }
            yield return new WaitForSeconds(attackMoleRainDelay);
            for (int j= 1; j < attackMoleRainNumberOfSpawns; j += 2)
            {
                Vector2 position = new Vector2(attackMoleRainPositionX + (j * attackMoleRainShift), attackMoleRainPositionY);
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
    private void Attack_GroundDrills()
    {
        attackNumberDrillGround++;
    }

    private void Attack_ShovelRain()
    {
        attackNumberShovel++;
    }

    private void Attack_MoleCharge()
    {
        if (colliderLeft)
        {
            rb.velocity += Vector2.left * attackMoleChargeSpeed * Time.deltaTime;
        }
        else 
        {
            rb.velocity += Vector2.right * attackMoleChargeSpeed * Time.deltaTime;
        }
    }

    /*IEnumerator Attack_Rock()
    {
        Vector2 position;
        if (colliderMiddleLeft)
        {
            position = new Vector2();
        }
        else if (colliderMiddleMiddle)
        {
            position = new Vector2();   
        }
        else //Middle Right
        {
            position = new Vector2();
        }
    }*/
}
