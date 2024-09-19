using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VoiceLinesLevel : MonoBehaviour
{
    public string[] subtitles;
    public bool voiceLinesBig;
    public string[] subtitlesBig;
    AudioSource[] voiceLines;
    private int[] randomNumbers;
    private int index = 0;
    private int RNGnow = 0;
    private int RNGsaved = 2;
    System.Random rng = new System.Random();
    AudioSource[] voiceLinesBigFall;
    private int[] randomNumbersBig;
    int indexBig = 0;
    SubtitlesManager subtitlesManager;

    void Randomize()
    {
        for (int i = randomNumbers.Length - 1; i > 0; i--)
        {
            int j = rng.Next(0, i + 1);
            int temp = randomNumbers[i];
            randomNumbers[i] = randomNumbers[j];
            randomNumbers[j] = temp;
        }
    }

    void RandomizeBig()
    {
        for (int i = randomNumbersBig.Length - 1; i > 0; i--)
        {
            int j = rng.Next(0, i + 1);
            int temp = randomNumbersBig[i];
            randomNumbersBig[i] = randomNumbersBig[j];
            randomNumbersBig[j] = temp;
        }
    }

    void Start()
    {
        voiceLines = GetComponents<AudioSource>()
                     .OrderBy(v => v.gameObject.name)  // Sort by name
                     .ToArray();

        subtitlesManager = FindAnyObjectByType<SubtitlesManager>();
        if (subtitlesManager == null) Debug.LogError("subtitlesManager is not initialized!");
        if (voiceLines.Length == 0) Debug.LogError("VoiceLines or Subtitles are missing!");

        randomNumbers = new int[voiceLines.Length];
        for (int i = 0; i < voiceLines.Length; i++)
        {
            randomNumbers[i] = i;
        }
        Randomize();

        if (voiceLinesBig)
        {
            voiceLinesBigFall = GetComponentsInChildren<AudioSource>(true)
                                .Where(v => v.gameObject != this.gameObject)  // Exclude parent
                                .OrderBy(v => v.gameObject.name)  // Sort by name
                                .ToArray();

            if (voiceLinesBigFall.Length == 0) Debug.LogError("VoiceLinesBigFall or SubtitlesBig are missing!");

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
            RNGsaved = rng.Next(2, 5);
            RNGnow = 0;            
            subtitlesManager.Write(subtitles[randomNumbers[index]], voiceLines[randomNumbers[index]].clip.length);
            voiceLines[randomNumbers[index]].Play();
            index++;
            if (index >= voiceLines.Length)
            {
                index = 0;
                Randomize();
            }
        }
    }


    public void PlayVLBigFall()
    {
        if (indexBig >= voiceLinesBigFall.Length)
        {
            indexBig = 0;
            RandomizeBig();
        }
        subtitlesManager.Write(subtitlesBig[randomNumbersBig[indexBig]], voiceLinesBigFall[randomNumbersBig[indexBig]].clip.length);
        voiceLinesBigFall[randomNumbersBig[indexBig]].Play();
        indexBig++;
    }
}
