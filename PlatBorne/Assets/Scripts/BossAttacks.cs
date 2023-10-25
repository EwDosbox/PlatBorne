using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttacks : MonoBehaviour
{
    public Bossfight bossfight;
    public PlayerScript player;
    public GameObject[] leech;
    public GameObject[] lava;
    [SerializeField] private AudioSource BossScream;

    public int phase;
    public bool bossHitboxRight = false;
    public bool bossHitboxLeft = false;
    public bool bossHitboxUp = false;
    public bool bossHitboxDown = false;
    public bool bossHitbox = false;
    public bool bossfightStarted = false;
    private void BossAttackRushPlayer()
    {

    }
    private void BossAttackFloorIsLava()
    {
        Transform lava = GetComponent<Transform>();
        for (float i = -15; i != - 7; i += (float)0.2)
        {
            lava.transform.position = new Vector2(lava.transform.position.x,i);
            new WaitForSeconds((float)0.2);
        }
        return;
    }
    private void BossAttackDagger()
    {

    }
    private void BossAttackSword()
    {

    }
    private void BossAttackLeechLeft()
    {
        for (int i = 0; i < leech.Length - 3; i++)
        {
            Rigidbody rb = leech[i].GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.WakeUp();
                new WaitForSeconds((float)0.05 + Mathf.Log(i));
            }
        }
        LeechReset();
    }
    private void BossAttackLeechRight()
    {
        for (int i = leech.Length; i > 3; i--)
        {
            Rigidbody rb = leech[i].GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.WakeUp();
                new WaitForSeconds((float)0.05 + Mathf.Log(i));
            }
        }
        LeechReset();
    }
    private void BossAttackLeechBoth()
    {
        for (int i = 0; i < (leech.Length / 2) - 2; i++)
        {
            Rigidbody rb = leech[i].GetComponent<Rigidbody>();
            Rigidbody rb2 = leech[leech.Length - i - 1].GetComponent<Rigidbody>();
            if (rb != null && rb2 != null)
            {
                rb.isKinematic = false;
                rb2.isKinematic = false;
                rb.WakeUp();
                rb2.WakeUp();
                new WaitForSeconds((float)0.05 + Mathf.Log(i));
            }
        }
        LeechReset();
        return;
    }
    private void BossAttackChoose()
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

    private void PhaseAttack() //rushes the player, screams before
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

    private void LeechReset()
    {
        foreach (GameObject name in leech)
        {
            Rigidbody rb = name.GetComponent<Rigidbody>();
            rb.Sleep();
            name.transform.position = new Vector2(name.transform.position.x, (float)-559.36);
        }
        return;
    }
}
