using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using UnityEngine;

[SerializeField] AudioSource VLDeath01;
[SerializeField] AudioSource VLDeath02;
[SerializeField] AudioSource VLDeath03;
[SerializeField] AudioSource VLDeath04;
[SerializeField] AudioSource VLDeath05;
[SerializeField] AudioSource VLDeath06;
[SerializeField] AudioSource VLDeath07;
[SerializeField] AudioSource VLDeath08;
[SerializeField] AudioSource VLDeath09;
[SerializeField] AudioSource VLDeath10;
[SerializeField] AudioSource VLDeath11;
[SerializeField] AudioSource VLDeath12;
[SerializeField] AudioSource VLDeath13;
[SerializeField] AudioSource VLDeath14;
[SerializeField] AudioSource VLDeath16;
[SerializeField] AudioSource VLDeath17;
[SerializeField] AudioSource VLDeath18;
[SerializeField] AudioSource VLDeath19;
[SerializeField] AudioSource VLDeath20;

public int BossDeathCount = 0;

public class NewBehaviourScript : MonoBehaviour
{
    public Animator transitionAnim;

    void Start()
    {
        BossDeathCount++;
        switch(BossDeathCount)
        {
            case 1:
                {
                    VLDeath.Play();
                    Debug.Log("Play Voice Line Player Death01");
                }
        }
    }
}
