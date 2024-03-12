using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class BossAttacks : MonoBehaviour
{
    public Bossfight bossfight;
    public PlayerScript player;
    public GameObject leech;
    public GameObject lava;
    public GameObject dagger;
    public GameObject sword;
    public Rigidbody2D rb;
    public GameObject warningLeft;
    public GameObject warningRight;
    [SerializeField] private AudioSource BossScream;
    //IN UNITY
    public float leechAttackBetween;
    public float leechAttackDifference;
    public float leechAttackMaxSpawnRate;
    [SerializeField] public float timerBetweenDaggerAttack = 1;
    //IN UNITY
    Vector3 daggerVector = new Vector3();
    public int phase;
    public bool bossHitboxRight = false;
    public bool bossHitboxLeft = false;
    public bool bossHitboxUp = false;
    public bool bossHitboxDown = false;
    public bool bossHitbox = false;
    private float leechTimer = 0;
    private int leechAttackWhere = 0; //0 = LeftRight, 1 = RightLeft, 2 = Both
    private bool leechAttack = false;
    private float daggerTimer = 0;
    private bool daggerTimerIsOn = false;
    private int leechAttackHappened = 0;
    private bool swordAttackTimer = false;
    private float timerSwordAttack = 0;
    private float warningTimer = 0f;
    private bool boolWarningTimer = false;
    private bool swordAttackIsLeft = true;
    private int[] daggerPosition;
    private bool bossAttackDaggerFirstTime = true;
    private int daggerIndex = 0;
    public void BossAttackFloorIsLava()
    {
        Bossfight.attackIsGoingOn = true;
        Vector3 position = new Vector3(0, -15f, 0.8f); //z = BUDE V POPØEDÍ
        Instantiate(lava, position, Quaternion.identity);
        return;
    }//done
    public void BossAttackDagger()
    {
        if (bossAttackDaggerFirstTime)
        {
            Bossfight.attackIsGoingOn = true;
            int random = UnityEngine.Random.Range(1, 5);
            switch (random)
            {
                default:
                    daggerPosition = new int[6] { 2, 1, 3, 5, 4, 6 };
                    break;
                case 2:
                    daggerPosition = new int[6] { 1, 4, 2, 5, 3, 6 };
                    break;
                case 3:
                    daggerPosition = new int[6] { 1, 3, 5, 4, 2, 6 };
                    break;
                case 4:
                    daggerPosition = new int[6] { 6, 3, 5, 1, 4, 2 };
                    break;
            }
        }
        daggerTimerIsOn = true;
        if (daggerIndex < daggerPosition.Length)
        {
            if (daggerTimer > timerBetweenDaggerAttack)
            {
                switch (daggerPosition[daggerIndex])
                {
                    case 1:
                        daggerVector = new Vector3(20f, 0f, 3);
                        break;
                    case 2:
                        daggerVector = new Vector3(20, -3.76f, 3);
                        break;
                    case 3:
                        daggerVector = new Vector3(20f, -7.50f, 3);
                        break;
                    case 4:
                        daggerVector = new Vector3(-20f, 0f, 3);
                        break;
                    case 5:
                        daggerVector = new Vector3(-20f, -3.76f, 3);
                        break;
                    case 6:
                        daggerVector = new Vector3(-20f, -7.50f, 3);
                        break;
                }
                Instantiate(dagger, daggerVector, Quaternion.identity);
                daggerIndex++;
                daggerTimer -= timerBetweenDaggerAttack;
            }
        }
        else
        {
            daggerTimerIsOn = false;
            Bossfight.attackIsGoingOn = false;
            daggerIndex = 0;
        }
    }//done
    public void BossAttackSwordLeft()
    {
        swordAttackIsLeft = true;
        Bossfight.attackIsGoingOn = true;
        boolWarningTimer = true;
        warningLeft.SetActive(true);
        if (warningTimer > 2.5f)
        {
            warningLeft.SetActive(false);
            boolWarningTimer = false;
            Vector3 position = new Vector3(-21f, -1.3f, 3);
            Instantiate(sword, position, quaternion.identity);
        }
    }
    public void BossAttackSwordRight()
    {
        swordAttackIsLeft = false;
        Bossfight.attackIsGoingOn = true;
        boolWarningTimer = true;
        warningRight.SetActive(true);
        if (warningTimer > 2.5f)
        {
            warningRight.SetActive(false);
            boolWarningTimer = false;
            Vector3 position = new Vector3(21f, -1.3f, 3);
            Debug.Log("BossSwordRight");
            Instantiate(sword, position, quaternion.identity);
        }
    }
    public void BossAttackSwordBoth(bool bothAtTheSameTime, bool leftFirst, float timeBetweenAttacks)
    {
        if (bothAtTheSameTime)
        {
            BossAttackSwordLeft();
            BossAttackSwordRight();
        }
        else if (leftFirst)
        {
            swordAttackTimer = true;
            BossAttackSwordLeft();
            if (timerSwordAttack > timeBetweenAttacks) BossAttackSwordRight();
            swordAttackTimer = false;
            return;
        }
        else
        {
            swordAttackTimer = true;
            BossAttackSwordRight();
            if (timerSwordAttack > timeBetweenAttacks) BossAttackSwordLeft();
            swordAttackTimer = false;
            return;
        }

    }
    public void BossAttackLeechLeft()
    {
        Bossfight.attackIsGoingOn = true;
        leechAttackWhere = 0;
        leechAttack = true;
        if (leechTimer > leechAttackBetween)
        {
            Vector3 position = new Vector3(-16.88f + (leechAttackHappened * 1.7f), 11.5f, 3);
            Instantiate(leech, position, Quaternion.identity);
            leechAttackHappened++;
            leechTimer = 0;
            if (leechAttackBetween > leechAttackMaxSpawnRate) leechAttackBetween -= leechAttackDifference;
            else leechAttackBetween = leechAttackMaxSpawnRate;
        }
        else if (leechAttackHappened > 17)//attackEnd
        {
            leechTimer = 0;
            leechAttackBetween = 0.5f;
            leechAttackHappened = 0;
            leechAttack = false;
            Bossfight.attackIsGoingOn = false;
            Debug.Log("Leeches End");
        }
    }//done
    public void BossAttackLeechRight()
    {
        Bossfight.attackIsGoingOn = true;
        leechAttack = true;
        leechAttackWhere = 1;
        if (leechTimer > leechAttackBetween)
        {
            Vector3 position = new Vector3(16.78f - (leechAttackHappened * 1.7f), 11.5f, 3);
            Instantiate(leech, position, Quaternion.identity);
            leechAttackHappened++;
            leechTimer = 0;
            if (leechAttackBetween > leechAttackMaxSpawnRate) leechAttackBetween -= leechAttackDifference;
            else leechAttackBetween = leechAttackMaxSpawnRate;
        }
        else if (leechAttackHappened > 17)//attackEnd
        {
            leechTimer = 0;
            leechAttackBetween = 0.5f;
            leechAttackHappened = 0;
            leechAttack = false;
            Bossfight.attackIsGoingOn = false;
            Debug.Log("Leeches End");
        }
    }//done
    public void BossAttackLeechBoth()
    {
        Bossfight.attackIsGoingOn = true;
        leechAttack = true;
        leechAttackWhere = 2;
        if (leechTimer > leechAttackBetween)
        {
            Debug.Log("Right");
            Vector3 position = new Vector3(16.78f - (leechAttackHappened * 1.7f), 11.5f, 3);
            Instantiate(leech, position, Quaternion.identity);
            Vector3 position2 = new Vector3(-16.78f + (leechAttackHappened * 1.7f), 11.5f, 3);
            Instantiate(leech, position2, Quaternion.identity);
            leechAttackHappened++;
            leechTimer = 0;
            if (leechAttackBetween > 0.10f) leechAttackBetween -= 0.08f;
        }
        else if (leechAttackHappened > 8)//attackEnd
        {
            leechTimer = 0;
            leechAttackBetween = 1;
            leechAttackHappened = 0;
            leechAttack = false;
            Bossfight.attackIsGoingOn = false;
            Debug.Log("Leeches End");
        }
    }//done
    /*public void PhaseAttack() //rushes the player, screams before ***needs Vorm***
    {
        BossScream.Play();
        //animation for screaming
        StartCoroutine(WaitForAudio());

    }

    private IEnumerator WaitForAudio()
    {
        while (BossScream.isPlaying)
        {
            yield return null;
        }
    }*/
    private void Update()
    {
        if (swordAttackTimer) timerSwordAttack += Time.deltaTime;

        if (daggerTimerIsOn)
        {
            daggerTimer += Time.deltaTime;
            bossAttackDaggerFirstTime = false;
            BossAttackDagger();
        }
        else
        {
            daggerTimer = 0;
            bossAttackDaggerFirstTime = true;
        }

        if (leechAttack)
        {
            leechTimer += Time.deltaTime;
            if (leechAttackWhere == 0) BossAttackLeechLeft();
            else if (leechAttackWhere == 1) BossAttackLeechRight();
            else BossAttackLeechBoth();
        }
        if (boolWarningTimer)
        {
            warningTimer += Time.deltaTime;
            if (warningTimer > 2f) if (swordAttackIsLeft) BossAttackSwordLeft();
                                   else BossAttackSwordRight();
        }
        else warningTimer = 0;
    }
    }

