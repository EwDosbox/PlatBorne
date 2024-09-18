using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class VoiceLinesLevel : MonoBehaviour
{
    //VoiceLines
    public string[] subtitles;
    public bool voiceLinesBig;
    public string[] subtitlesBig;
    AudioSource[] voiceLines;    
    private int[] randomNumbers;
    private int index = 0;
    private int RNGnow = 0;
    private int RNGsaved = 2;
    System.Random rng = new System.Random();
    //VoiceLinesBigFall
    AudioSource[] voiceLinesBigFall;
    private int[] randomNumbersBig;
    int indexBig = 0;

    SubtitlesManager subtitlesManager;
    void Randomize()
    {
        int random1, random2, bucket;
        for (int i = 0; i < 1000; i++)
        {
            random1 = rng.Next(0, randomNumbers.Length);
            random2 = rng.Next(0, randomNumbers.Length);
            bucket = randomNumbers[random1];
            randomNumbers[random1] = randomNumbers[random2];
            randomNumbers[random2] = bucket;
        }
    }

    void RandomizeBig() //fuck it, another one
    {
        int random1, random2, bucket;
        for (int i = 0; i < 100; i++)
        {
            random1 = rng.Next(0, randomNumbersBig.Length);
            random2 = rng.Next(0, randomNumbersBig.Length);
            bucket = randomNumbersBig[random1];
            randomNumbersBig[random1] = randomNumbersBig[random2];
            randomNumbersBig[random2] = bucket;
        }
    }
    
    void Start()
    {
        subtitlesManager = FindAnyObjectByType<SubtitlesManager>();
        voiceLines = GetComponents<AudioSource>();
        randomNumbers = new int[voiceLines.Length];
        for (int i = 0; i < voiceLines.Length; i++)
        {
            randomNumbers[i] = i;
        }
        Randomize();
        if (voiceLinesBig)
        {
            voiceLinesBigFall = GetComponentsInChildren<AudioSource>();            
            randomNumbersBig = new int[voiceLinesBigFall.Length];
            for (int i = 0; i < voiceLinesBigFall.Length; i++)
            {
                randomNumbersBig[i] = i;
            }
            RandomizeBig();
        }        
    }
    public void PlayVLFallen()
    {
        RNGnow++;
        if (RNGnow == RNGsaved)
        {
            Debug.Log("PlayFall");
            RNGsaved = rng.Next(2, 4 + 1);
            RNGnow = 0;
            subtitlesManager.Write(subtitles[randomNumbers[index]], voiceLines[randomNumbers[index]].clip.length);
            voiceLines[randomNumbers[index]].Play();
            index++;
            if (index == voiceLines.Length - 1)
            {
                index = 0;
                Randomize();
            }
        }
    }

    public void PlayVLBigFall()
    {
        Debug.Log(indexBig);
        subtitlesManager.Write(subtitlesBig[randomNumbersBig[indexBig]], voiceLinesBigFall[randomNumbersBig[indexBig]].clip.length);
        voiceLinesBigFall[randomNumbersBig[indexBig]].Play();
        indexBig++;
        if (indexBig == voiceLinesBigFall.Length - 1)
        {
            indexBig = 0;
            RandomizeBig();
        }
    }
}
