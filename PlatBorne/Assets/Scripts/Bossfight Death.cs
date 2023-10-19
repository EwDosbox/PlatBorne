using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using UnityEngine;

[SerializeField] private AudioSource VLDeath01;
[SerializeField] private AudioSource VLDeath02;
[SerializeField] private AudioSource VLDeath03;
[SerializeField] private AudioSource VLDeath04;
[SerializeField] private AudioSource VLDeath05;
[SerializeField] private AudioSource VLDeath06;
[SerializeField] private AudioSource VLDeath07;
[SerializeField] private AudioSource VLDeath08;
[SerializeField] private AudioSource VLDeath09;
[SerializeField] private AudioSource VLDeath10;
[SerializeField] private AudioSource VLDeath11;
[SerializeField] private AudioSource VLDeath12;
[SerializeField] private AudioSource VLDeath13;
[SerializeField] private AudioSource VLDeath14;
[SerializeField] private AudioSource VLDeath15;
[SerializeField] private AudioSource VLDeath16;
[SerializeField] private AudioSource VLDeath17;
[SerializeField] private AudioSource VLDeath18;
[SerializeField] private AudioSource VLDeath19;
[SerializeField] private AudioSource VLDeath20;

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
