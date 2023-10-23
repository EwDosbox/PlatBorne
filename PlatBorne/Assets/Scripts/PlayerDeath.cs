using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public Animator transitionAnim;
    [SerializeField] private AudioSource VLDeath01, VLDeath02, VLDeath03, VLDeath04, VLDeath05, VLDeath06, VLDeath07, VLDeath08, VLDeath09, VLDeath10, VLDeath11, VLDeath12, VLDeath13, VLDeath14, VLDeath15, VLDeath16, VLDeath17, VLDeath18;

    public int BossDeathCount = 0;

    private void Start()
    {
        BossDeathCount++;
        switch (BossDeathCount)
        {
            case 1:
                {
                    VLDeath01.Play();
                    Debug.Log("Play Voice Line Player Death01");
                    break;
                }
            case 2:
                {
                    VLDeath02.Play();
                    Debug.Log("Play Voice Line Player Death02");
                    break;
                }
            case 3:
                {
                    VLDeath03.Play();
                    Debug.Log("Play Voice Line Player Death03");
                    break;
                }
            case 4:
                {
                    VLDeath04.Play();
                    Debug.Log("Play Voice Line Player Death04");
                    break;
                }
            case 5:
                {
                    VLDeath05.Play();
                    Debug.Log("Play Voice Line Player Death05");
                    break;
                }
            case 6:
                {
                    VLDeath06.Play();
                    Debug.Log("Play Voice Line Player Death06");
                    break;
                }
            case 7:
                {
                    VLDeath07.Play();
                    Debug.Log("Play Voice Line Player Death07");
                    break;
                }
            case 8:
                {
                    VLDeath08.Play();
                    Debug.Log("Play Voice Line Player Death08");
                    break;
                }
            case 9:
                {
                    VLDeath09.Play();
                    Debug.Log("Play Voice Line Player Death09");
                    break;
                }
            case 10:
                {
                    VLDeath10.Play();
                    Debug.Log("Play Voice Line Player Death10");
                    break;
                }
            case 11:
                {
                    VLDeath11.Play();
                    Debug.Log("Play Voice Line Player Death11");
                    break;
                }
            case 12:
                {
                    VLDeath12.Play();
                    Debug.Log("Play Voice Line Player Death12");
                    break;
                }
            case 13:
                {
                    VLDeath13.Play();
                    Debug.Log("Play Voice Line Player Death13");
                    break;
                }
            case 14:
                {
                    VLDeath14.Play();
                    Debug.Log("Play Voice Line Player Death14");
                    break;
                }
            case 15:
                {
                    VLDeath15.Play();
                    Debug.Log("Play Voice Line Player Death15");
                    break;
                }
            case 16:
                {
                    VLDeath16.Play();
                    Debug.Log("Play Voice Line Player Death16");
                    break;
                }
            case 17:
                {
                    VLDeath17.Play();
                    Debug.Log("Play Voice Line Player Death17");
                    break;
                }
            default:
                {
                    VLDeath18.Play();
                    Debug.Log("Play Voice Line Player Death Default");
                    break;
                }
        }
    }
}
