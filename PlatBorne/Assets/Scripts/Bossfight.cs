using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;
using UnitySceneManager;

[SerializeField] private AudioSource PreBossDialog;
[SerializeField] private AudioSource BossDamage01;
[SerializeField] private AudioSource BossDamage02;
[SerializeField] private AudioSource BossDamage03;
[SerializeField] private AudioSource BossDamage04;
[SerializeField] private AudioSource BossDeath01;
[SerializeField] private AudioSource HunterOOF;

public class NewBehaviourScript : MonoBehaviour
{
    public int phase = 1;
    bool attackIsGoing = false;
    public int timer = 0;
    int hunterHP = 3;
    int bossHP = 60;
    bool bossVunerable = false;
    double phaseTimer = 0;
    double Timer = 0;

    private void BossDeath()
    {
        BossDeath01.Play();
        Debug.Log("Boss Has Died");
    }
    private void BossAttackRushLeft()
    {

    }
    private void BossAttackRushRight()
    {

    }
    private void BossAttackFloorIsLava()
    {

    }
    private void BossAttackDagger()
    {

    }
    private void BossAttackSword()
    {

    }
    private void BossAttackLeechLeft()
    {

    }
    private void BossAttackLeechRight()
    {

    }
    private void BossAttackLeechBoth()
    {

    }
    private void PlayerDeath()
    {
        SceneManager.LoadScene("PlayerDeath");
        Debug.Log("Player Death Scene");
    }

    private void Start()
    {
        if (collision.gameObject.name == "Arena Start")
        {
            PreBossDialog.Play();
            Debug.Log("BossFight Start");
            attackIsGoing = true;
        }    
    }
    private void Update()
    {
        if (!attackIsGoing)
        {
            switch (phase)
            {
                case 1:
                    {
                        WaitForSeconds(4.0f);
                        if (collision.gameObject.name == "Boss Hitbox Left" && //je dál jak pùlka mapy//)
                        {
                            BossAttackRushLeft();
                        }
                        else if (collision.gameObject.name == "Boss Hitbox Right" &&//je dál jak pùlka mapy//)
                        {
                            BossAttackRushRight;
                        }
                        else if (collision.gameObject.name == "Boss Hitbox Down")) {
                            BossAttackFloorIsLava();
                        }
                        else
                        {
                            BossAttackDagger();
                        }
                    }
                case 2:
                    {
                        WaitForSeconds(3.0f);
                        if (collision.gameObject.name == "Boss Hitbox Left" && //je dál jak pùlka mapy//)
                        {
                            BossAttackRushLeft();
                        }
                        else if (collision.gameObject.name == "Boss Hitbox Right" &&//je dál jak pùlka mapy//)
                        {
                            BossAttackRushRight;
                        }
                        else if (collision.gameObject.name == "Boss Hitbox Right" || collision.gameObject.name == "Boss Hitbox Left" || collision.gameObject.name == "Boss Hitbox Up")
                        {
                            BossAttackSword();
                        }                   
                        else if (collision.gameObject.name == "Boss Hitbox Down")) {
                            BossAttackFloorIsLava();
                        }
                        else
                        {
                            
                        }
                    }
                case 3:
                    {
                        WaitForSeconds(2.0f);
                        if (collision.gameObject.name == "Boss Hitbox Left" && //je dál jak pùlka mapy//)
                        {
                            BossAttackRushLeft();
                        }
                        else if (collision.gameObject.name == "Boss Hitbox Right" &&//je dál jak pùlka mapy//)
                        {
                            BossAttackRushRight;
                        }
                        else if (collision.gameObject.name == "Boss Hitbox Right" || collision.gameObject.name == "Boss Hitbox Left" || collision.gameObject.name == "Boss Hitbox Up")
                        {
                            BossAttackSword();
                        }
                        else if (collision.gameObject.name == "Boss Hitbox Down")) {
                            BossAttackFloorIsLava();
                        }
                        else
                        {
                            BossAttackDagger();
                        }
                    }
                case 4:
                    {
                        WaitForSeconds(1.0f);
                        if (collision.gameObject.name == "Boss Hitbox Left" && //je dál jak pùlka mapy//)
                        {
                            BossAttackRushLeft();
                        }
                        else if (collision.gameObject.name == "Boss Hitbox Right" &&//je dál jak pùlka mapy//)
                        {
                            BossAttackRushRight;
                        }
                        //leeches
                        if (collision.gameObject.name)
                        {
                            BossAttackLeechRight()
                        }
                        else if (collision.gameObject.name == "Boss Hitbox Right")
                        {

                        }
                        else if (collision.gameObject.name == "Boss Hitbox Right" || collision.gameObject.name == "Boss Hitbox Left" || collision.gameObject.name == "Boss Hitbox Up")
                        {
                            BossAttackSword();
                        }
                        else if (collision.gameObject.name == "Boss Hitbox Down")) {
                            BossAttackFloorIsLava();
                        }
                        else
                        {
                            BossAttackDagger();
                        }
                    }
            }
        }
        if (PlayerVunerable)
        {
            WaitForSeconds(0.5f);
            PlayerVunerable = false;
        }
        if (bossVunerable) //Phase 1,2,3 Boss Damage
        {
            if (collision.gameObject.name == "Boss Hitbox")
            {
                bossVunerable = false;
                bossHP -= 20;
                switch (bossHP)
                {
                    case 40:
                        {
                            phase = 2;
                            BossDamage02.Play();
                            Debug.Log("Phase 2 Start");
                            break;
                        }
                    case 20:
                        {
                            phase = 3;
                            BossDamage03.Play();
                            Debug.Log("Phase 3 Start");
                            break;
                        }
                }
                if (bossHP == 0 && phase == 3)
                {
                    BossDamage04.Play();
                    Debug.Log("Phase 4 Start");
                    bossHP = 60;
                    phase = 4;
                }
            }
        }
        if (phase == 4 && (phaseTimer % 1000 = 0)) //Boss Damage Counter Phase 4
        {
            bossHP--;
            if (bossHP < 0) BossDeath();
        }
        if (collision.gameObject.name == "Boss Hitbox") //Hunter HP Damage + Death
        {
            hunterHP--;
            HunterOOF.Play();
            Debug.Log("Hunter has taken Damage");
            if (hunterHP == 0)
            {
                Debug.Log("Hunter has Died");
                PlayerDeath();
            }
        }

    }
}
