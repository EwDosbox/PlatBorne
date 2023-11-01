using UnityEngine;

public class VoiceLineLondon : MonoBehaviour
{
    [SerializeField] private AudioSource Fell01;
    [SerializeField] private AudioSource Fell02;
    [SerializeField] private AudioSource Fell03;
    [SerializeField] private AudioSource Fell04;
    [SerializeField] private AudioSource Fell05;
    [SerializeField] private AudioSource Fell06;
    [SerializeField] private AudioSource Fell07;
    [SerializeField] private AudioSource Fell08;
    [SerializeField] private AudioSource Fell09;
    [SerializeField] private AudioSource Fell10;
    [SerializeField] private AudioSource Fell11;
    [SerializeField] private AudioSource Fell12;
    [SerializeField] private AudioSource Fell13;
    [SerializeField] private AudioSource Fell14;
    [SerializeField] private AudioSource Fell15;
    [SerializeField] private AudioSource Fell16;
    [SerializeField] private AudioSource Fell17;
    [SerializeField] private AudioSource Fell18;
    [SerializeField] private AudioSource Fell19;
    [SerializeField] private AudioSource Fell20;
    [SerializeField] private AudioSource Fell21;
    [SerializeField] private AudioSource Fell22;
    int numberOfRandom;
    int j = 0;
    int[] voiceLines = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22 };
    void Randomize(int[] voiceLines)
    {
        System.Random rng = new System.Random();
        int random1, random2, bucket; //Ani slovo Vikore
        for (int i = 0; i < numberOfRandom;i++)
        {
            random1 = rng.Next(1, voiceLines.Length - 1);
            random2 = rng.Next(1, voiceLines.Length - 1);
            bucket = voiceLines[random1];
            voiceLines[random1] = voiceLines[random2];
            voiceLines[random2] = voiceLines[random1];
        }
        Debug.Log(voiceLines);
        return;
    }

    private void Start()
    {
        Randomize(voiceLines);
    }
    void Update()
    {
        PlayerScript playerScript = GetComponent<PlayerScript>();
        bool playVoiceLine = playerScript.playVoiceLine;
        if (playVoiceLine)
        {
            switch (voiceLines[j])
            {
                case 1:
                    {
                        Fell01.Play();
                        break;
                    }
                case 2:
                    {
                        Fell02.Play();
                        break;
                    }
                case 3:
                    {
                        Fell03.Play();
                        break;
                    }
                case 4:
                    {
                        Fell04.Play();
                        break;
                    }
                case 5:
                    {
                        Fell05.Play();
                        break;
                    }
                case 6:
                    {
                        Fell06.Play();
                        break;
                    }
                case 7:
                    {
                        Fell07.Play();
                        break;
                    }
                case 8:
                    {
                        Fell08.Play();
                        break;
                    }
                case 9:
                    {
                        Fell09.Play();
                        break;
                    }
                case 10:
                    {
                        Fell10.Play();
                        break;
                    }
                case 11:
                    {
                        Fell11.Play();
                        break;
                    }
                case 12:
                    {
                        Fell12.Play();
                        break;
                    }
                case 13:
                    {
                        Fell13.Play();
                        break;
                    }
                case 14:
                    {
                        Fell14.Play();
                        break;
                    }
                case 15:
                    {
                        Fell15.Play();
                        break;
                    }
                case 16:
                    {
                        Fell16.Play();
                        break;
                    }
                case 17:
                    {
                        Fell17.Play();
                        break;
                    }
                case 18:
                    {
                        Fell18.Play();
                        break;
                    }
                case 19:
                    {
                        Fell19.Play();
                        break;
                    }
                case 20:
                    {
                        Fell20.Play();
                        break;
                    }
                case 21:
                    {
                        Fell21.Play();
                        break;
                    }
                default:
                    {
                        Fell22.Play();
                        break;
                    }
            }
            j++;
            if (j == voiceLines.Length - 1)
            {
                Randomize(voiceLines);
                j = 0;
            }
        }
    }
}