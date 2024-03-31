using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class VoiceLinesLevel : MonoBehaviour
{
    public AudioSource[] voiceLines;
    private int[] randomNumbers;
    private int index = 0;
    private int RNGnow = 0;
    private int RNGsaved = 2;
    System.Random rng = new System.Random();
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
    // Start is called before the first frame update
    void Start()
    {
        randomNumbers = new int[voiceLines.Length];
        for (int i = 0; i < voiceLines.Length;i++)
        {
            randomNumbers[i] = i;
        }
        Randomize();
    }
    public void PlayVLFallen()
    {
        RNGnow++;
        if (RNGnow == RNGsaved)
        {
            RNGsaved = rng.Next(2, 4 + 1);
            RNGnow = 0;
            voiceLines[randomNumbers[index]].Play();
            index++;
            if (index == voiceLines.Length - 1)
            {
                index = 0;
                Randomize();
            }
        }
    }
}
