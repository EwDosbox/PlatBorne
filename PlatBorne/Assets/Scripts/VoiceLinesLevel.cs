using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VoiceLinesLevel : MonoBehaviour
{
    public string[] subtitles;
    public bool voiceLinesBig;
    public string[] subtitlesBig;

    private AudioSource[] voiceLines;
    private AudioSource[] voiceLinesBigFall;

    private int[] randomNumbers;
    private int[] randomNumbersBig;

    private int index = 0;
    private int indexBig = 0;

    private int RNGnow = 0;
    private int RNGsaved = 2;

    private System.Random rng = new System.Random();
    private SubtitlesManager subtitlesManager;

    private void Start()
    {
        voiceLines = GetComponents<AudioSource>()
                     .OrderBy(v => v.gameObject.name)
                     .ToArray();

        subtitlesManager = FindAnyObjectByType<SubtitlesManager>();
        if (subtitlesManager == null) Debug.LogError("SubtitlesManager is not initialized!");
        if (voiceLines.Length == 0 || subtitles.Length == 0) Debug.LogError("VoiceLines or Subtitles are missing!");
        if (subtitles.Length != voiceLines.Length) Debug.LogError("Mismatch between subtitles and voiceLines!");

        randomNumbers = Enumerable.Range(0, voiceLines.Length).ToArray();
        Randomize();

        if (voiceLinesBig)
        {
            voiceLinesBigFall = GetComponentsInChildren<AudioSource>(true)
                                .Where(v => v.gameObject != this.gameObject)
                                .OrderBy(v => v.gameObject.name)
                                .ToArray();

            if (voiceLinesBigFall.Length == 0 || subtitlesBig.Length == 0) Debug.LogError("VoiceLinesBigFall or SubtitlesBig are missing!");
            if (subtitlesBig.Length != voiceLinesBigFall.Length) Debug.LogError("Mismatch between subtitlesBig and voiceLinesBigFall!");

            randomNumbersBig = Enumerable.Range(0, voiceLinesBigFall.Length).ToArray();
            RandomizeBig();
        }
    }

    private void Randomize()
    {
        for (int i = randomNumbers.Length - 1; i > 0; i--)
        {
            int j = rng.Next(0, i + 1);
            int temp = randomNumbers[i];
            randomNumbers[i] = randomNumbers[j];
            randomNumbers[j] = temp;
        }
    }

    private void RandomizeBig()
    {
        for (int i = randomNumbersBig.Length - 1; i > 0; i--)
        {
            int j = rng.Next(0, i + 1);
            int temp = randomNumbersBig[i];
            randomNumbersBig[i] = randomNumbersBig[j];
            randomNumbersBig[j] = temp;
        }
    }

    public void PlayVLFallen()
    {
        RNGnow++;
        if (RNGnow == RNGsaved)
        {
            RNGsaved = rng.Next(2, 6); // 2–5 inclusive
            RNGnow = 0;

            int idx = randomNumbers[index];
            AudioSource src = voiceLines[idx];
            AudioClip clip = src.clip;

            if (clip == null)
            {
                Debug.LogWarning("Missing audio clip in PlayVLFallen.");
                return;
            }

            subtitlesManager.Write(subtitles[idx], clip.length);
            src.Play();

            index++;
            if (index >= voiceLines.Length)
            {
                index = 0;
                randomNumbers = Enumerable.Range(0, voiceLines.Length).ToArray();
                Randomize();
            }
        }
    }

    public void PlayVLBigFall()
    {
        if (indexBig >= voiceLinesBigFall.Length)
        {
            indexBig = 0;
            randomNumbersBig = Enumerable.Range(0, voiceLinesBigFall.Length).ToArray();
            RandomizeBig();
        }

        int idx = randomNumbersBig[indexBig];
        AudioSource src = voiceLinesBigFall[idx];
        AudioClip clip = src.clip;

        if (clip == null)
        {
            Debug.LogWarning("Missing audio clip in PlayVLBigFall.");
            return;
        }

        subtitlesManager.Write(subtitlesBig[idx], clip.length);
        src.Play();

        indexBig++;
    }
}
