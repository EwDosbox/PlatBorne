using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole_Bossfight : MonoBehaviour
{
    //SCRIPTS//
    Mole_Health bossHealth = new Mole_Health();
    PlayerHealth playerHealth = new PlayerHealth();
    Mole_Backpack backpack = new Mole_Backpack();
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
    [SerializeField] float[] attackMoleRainPositionX;
    [SerializeField] float attackMoleRainPositionY = 0;
    [SerializeField] float attackMoleRainShift = 0;
    [SerializeField] float attackMoleRainFallSpeed = 0;
    [SerializeField] float attackMoleRainDelay = 0;
    //Shovel Rain - uses same stats as Drill Rain

    //PUBLIC//

    //PRIVATE//
    private float timer = 0;
    private bool timerOn = false;
    private bool bossStarted = false;
    private bool attackIsGoing = false;
    private int phase = 1;


    private void Start()
    {
        timer = save.timerLoad(4);
    }
    private void Update()
    {
        if (bossStarted)
        {
            if (!attackIsGoing)
            {

            }
            timer += Time.deltaTime;
            save.timerSave(4);
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
    IEnumerator AttackChooser()
    {
        attackIsGoing = true;
        if (phase == 1)
        {
            yield return new WaitForSeconds(timeForNextAttackPhase1);
        }
        else if(phase == 2)
        {
            yield return new WaitForSeconds(timeForNextAttackPhase2);
        }
    }

    //ATTACKS//
    //PHASE I
    IEnumerator Attack_DrillRain()
    {
        attackNumberDrillRain++;
        for (int i = 0; i < 10; i++)
        {

        }
    }

    private void Attack_SmallMolesRain()
    {
        attackNumberMoleRain++;
    }

    private void Attack_SideDrills()
    {
        attackNumberDrillSide++;
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

    }
}
