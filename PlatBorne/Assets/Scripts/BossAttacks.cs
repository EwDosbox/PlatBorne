using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttacks : MonoBehaviour
{
    public Bossfight bossfight;
    public PlayerScript player;
    public GameObject leech;
    public GameObject lava;
    public GameObject dagger;
    [SerializeField] private AudioSource BossScream;

    public int phase;
    public bool bossHitboxRight = false;
    public bool bossHitboxLeft = false;
    public bool bossHitboxUp = false;
    public bool bossHitboxDown = false;
    public bool bossHitbox = false;
    public bool bossfightStarted = false;
    private float daggerTimer = 0;
    private float leechTimer = 0;
    private int leechAttackWhere = 0; //0 = LeftRight, 1 = RightLeft, 2 = Both
    private bool daggerAttack = false;
    private int daggerAttackHappened = 0;
    private bool leechAttack = false;
    private int leechAttackHappened = 0;
    private float leechAttackBetween = 1;

    public void BossAttackRushPlayer()
    {

    }//***needs Vorm***
    public void BossAttackFloorIsLava()
    {
        Transform lava = GetComponent<Transform>();
        for (float i = -15; i != - 7; i += (float)0.2)
        {
            lava.transform.position = new Vector2(lava.transform.position.x,i);
            new WaitForSeconds((float)0.2);
        }
        return;
    }
    public void BossAttackDagger()
    {
        daggerAttack = true;
        if (daggerTimer > 1 && daggerAttackHappened < 6)
        {
            Vector3 position = new Vector3(20, -3.76f, 0);
            switch (daggerAttackHappened)
            {
                case 1: 
                    position = new Vector3(20f, 0f, 0);
                    break;
                case 2:
                    position = new Vector3(20f, -7.50f, 0);
                    break;
                case 3:
                    position = new Vector3(-20f, -3.76f, 0);
                    break;
                case 4:
                    position = new Vector3(-20f, 0f, 0);
                    break;
                case 5:
                    position = new Vector3(-20f, -7.50f, 0);
                    break;
            }
            Instantiate(dagger, position, Quaternion.identity);
            daggerAttackHappened++;
            daggerTimer = 0;
        }
        else if (daggerAttackHappened > 6)//attackEnd
        {
            daggerTimer = 0;
            daggerAttackHappened = 0;
            daggerAttack = false;
            Debug.Log("Daggers End");
        }
    } //done
    public void BossAttackSword()
    {

    }
    public void BossAttackLeechLeft()
    {
        leechAttackWhere = 0;
        leechAttack = true;
        if (leechTimer > leechAttackBetween)
        {
            Vector3 position = new Vector3(-16.88f + (leechAttackHappened * 1.7f), 11.5f, 0);
            Instantiate(leech, position, Quaternion.identity);
            leechAttackHappened++;
            leechTimer = 0;
            if (leechAttackBetween > 0.10f) leechAttackBetween -= 0.04f;
        }
        else if (leechAttackHappened > 17)//attackEnd
        {
            leechTimer = 0;
            leechAttackBetween = 1;
            leechAttackHappened = 0;
            leechAttack = false;
            Debug.Log("Leeches End");
        }
    }//done
    public void BossAttackLeechRight()
    {
        leechAttack = true;
        leechAttackWhere = 1;
        if (leechTimer > leechAttackBetween)
        {
            Debug.Log("Right");
            Vector3 position = new Vector3(16.78f - (leechAttackHappened * 1.7f), 11.5f, 0);
            Instantiate(leech, position, Quaternion.identity);
            leechAttackHappened++;
            leechTimer = 0;
            if (leechAttackBetween > 0.10f) leechAttackBetween -= 0.04f;
        }
        else if (leechAttackHappened > 17)//attackEnd
        {
            leechTimer = 0;
            leechAttackBetween = 1;
            leechAttackHappened = 0;
            leechAttack = false;
            Debug.Log("Leeches End");
        }
    }//done
    public void BossAttackLeechBoth()
    {
        leechAttack = true;
        leechAttackWhere = 2;
        if (leechTimer > leechAttackBetween)
        {
            Debug.Log("Right");
            Vector3 position = new Vector3(16.78f - (leechAttackHappened * 1.7f), 11.5f, 0);
            Instantiate(leech, position, Quaternion.identity);
            Vector3 position2 = new Vector3(-16.78f + (leechAttackHappened * 1.7f), 11.5f, 0);
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
            Debug.Log("Leeches End");
        }
    }//done
    public void BossAttackChoose()
    {
        switch (phase)
        {
            case 1:
                {
                    new WaitForSeconds(5 - phase);
                    if (bossHitboxRight)
                    {
                        BossAttackRushPlayer(); // + boss na leve strane
                    }
                    else if (bossHitboxLeft) // + boss na prave strane
                    {
                        BossAttackRushPlayer();
                    }
                    else if (bossHitboxDown)
                    {
                        BossAttackFloorIsLava();
                    }
                    else
                    {
                        BossAttackDagger();
                    }
                    break;
                }
            case 2:
                {
                    new WaitForSeconds(5 - phase);
                    if (bossHitboxRight) // + boss na leve strane
                    {
                        BossAttackRushPlayer();
                    }
                    else if (bossHitboxLeft) // + boss na prave strane
                    {
                        BossAttackRushPlayer();
                    }
                    else if (bossHitboxLeft)
                    {
                        BossAttackSword();
                    }
                    else
                    {
                        BossAttackDagger();
                    }
                    break;
                }
            case 3:
                {
                    new WaitForSeconds(5 - phase);
                    if (bossHitboxRight) // + boss na leve strane
                    {
                        BossAttackRushPlayer();
                    }
                    else if (bossHitboxLeft) // + boss na prave strane
                    {
                        BossAttackRushPlayer();
                    }
                    else if (bossHitboxLeft)
                    {
                        BossAttackSword();
                    }
                    else if (bossHitboxDown)
                    {
                        BossAttackFloorIsLava();
                    }
                    else
                    {
                        BossAttackDagger();
                    }
                    break;
                }
            case 4:
                {
                    new WaitForSeconds(5 - phase);
                    if (bossHitboxRight) // + boss na leve strane
                    {
                        BossAttackRushPlayer();
                    }
                    else if (bossHitboxLeft) // + boss na prave strane
                    {
                        BossAttackRushPlayer();
                    }
                    else if (bossHitboxLeft)
                    {
                        BossAttackSword();
                    }
                    else if (bossHitboxDown)
                    {
                        BossAttackFloorIsLava();
                    }
                    else
                    {
                        BossAttackDagger();
                    }
                    // BossAttackLeech();
                    break;
                }
        }
    }

    public void PhaseAttack() //rushes the player, screams before ***needs Vorm***
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
    }
    private void Update()
    {
        if (daggerAttack)
        {
            daggerTimer += Time.deltaTime;
            BossAttackDagger();
        }

        if (leechAttack)
        {
            leechTimer += Time.deltaTime;
            if (leechAttackWhere == 0) BossAttackLeechLeft();
            else if (leechAttackWhere == 1) BossAttackLeechRight();
            else BossAttackLeechBoth();
        }
    }
}
