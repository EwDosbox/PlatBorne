using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LondonDialogue : MonoBehaviour
{
    [SerializeField] private AudioSource Fell01, Fell02, Fell03, Fell04, Fell05, Fell06, Fell07, Fell08, Fell09, Fell10, Fell11, Fell12, Fell13, Fell14, Fell15, Fell16, Fell17, Fell18, Fell19, Fell20, Fell21, Fell22;
    int j = 0;
    int[] voiceLinesArray;
    int RNGsaved = 0;
    int RNGnow = 0;
    public int minimum;
    public int maximum;

    void Randomize(int[] voiceLines)
    {
        System.Random rng = new System.Random();
        for (int i = 0; i < 250; i++)
        {
            int random1 = rng.Next(0, voiceLines.Length - 1);
            int random2 = rng.Next(0, voiceLines.Length - 1);
            int temp = voiceLines[random1];
            voiceLines[random1] = voiceLines[random2];
            voiceLines[random2] = temp;
        }
        string saved = null;
        for (int i = 0; i < voiceLines.Length; i++)
        {
            saved += voiceLines[i].ToString();
            if (i < voiceLines.Length - 1)saved += ",";
        }
        PlayerPrefs.SetString("LondonVoiceLinesArray", saved);
        PlayerPrefs.Save();
        return;
    }
    void Start()
    {
        voiceLinesArray = new int[21];
        if (PlayerPrefs.HasKey("LondonVoiceLinesArray"))
        {
            string vlTemp = PlayerPrefs.GetString("LondonVoiceLinesArray");
            string[] arrayVLChar = vlTemp.Split(",");
            for(int i = 0;i < arrayVLChar.Length;i++) voiceLinesArray[i] = int.Parse(arrayVLChar[i]);
            RNGnow = PlayerPrefs.GetInt("RNGNow");
            RNGsaved = PlayerPrefs.GetInt("RNGSaved");
            j = PlayerPrefs.GetInt("LondonVoiceLinesJ");
        }
        else
        { 
            for (int i = 0; i < voiceLinesArray.Length; i++)
            {
                voiceLinesArray[i] = i;
            }
            Randomize(voiceLinesArray);
        }
    }
    void Update()
    {
        if (PlayerScript.playVoiceLine)
        {
            if (RNGnow == RNGsaved)
            {
                System.Random rng = new System.Random();
                RNGsaved = rng.Next(minimum, maximum + 1);
                RNGnow = 0;
                switch (voiceLinesArray[j])
                {
                    case 1:
                        {
                            Fell01.Play();
                            Debug.Log("Playing Audio - Fell01");
                            break;
                        }
                    case 2:
                        {
                            Fell02.Play();
                            Debug.Log("Playing Audio - Fell02");
                            break;
                        }
                    case 3:
                        {
                            Fell03.Play();
                            Debug.Log("Playing Audio - Fell03");
                            break;
                        }
                    case 4:
                        {
                            Fell04.Play();
                            Debug.Log("Playing Audio - Fell04");
                            break;
                        }
                    case 5:
                        {
                            Fell05.Play();
                            Debug.Log("Playing Audio - Fell05");
                            break;
                        }
                    case 6:
                        {
                            Fell06.Play();
                            Debug.Log("Playing Audio - Fell06");
                            break;
                        }
                    case 7:
                        {
                            Fell07.Play();
                            Debug.Log("Playing Audio - Fell07");
                            break;
                        }
                    case 8:
                        {
                            Fell08.Play();
                            Debug.Log("Playing Audio - Fell08");
                            break;
                        }
                    case 9:
                        {
                            Fell09.Play();
                            Debug.Log("Playing Audio - Fell09");
                            break;
                        }
                    case 10:
                        {
                            Fell10.Play();
                            Debug.Log("Playing Audio - Fell10");
                            break;
                        }
                    case 11:
                        {
                            Fell11.Play();
                            Debug.Log("Playing Audio - Fell11");
                            break;
                        }
                    case 12:
                        {
                            Fell12.Play();
                            Debug.Log("Playing Audio - Fell12");
                            break;
                        }
                    case 13:
                        {
                            Fell13.Play();
                            Debug.Log("Playing Audio - Fell13");
                            break;
                        }
                    case 14:
                        {
                            Fell14.Play();
                            Debug.Log("Playing Audio - Fell14");
                            break;
                        }
                    case 15:
                        {
                            Fell15.Play();
                            Debug.Log("Playing Audio - Fell15");
                            break;
                        }
                    case 16:
                        {
                            Fell16.Play();
                            Debug.Log("Playing Audio - Fell16");
                            break;
                        }
                    case 17:
                        {
                            Fell17.Play();
                            Debug.Log("Playing Audio - Fell17");
                            break;
                        }
                    case 18:
                        {
                            Fell18.Play();
                            Debug.Log("Playing Audio - Fell18");
                            break;
                        }
                    case 19:
                        {
                            Fell19.Play();
                            Debug.Log("Playing Audio - Fell19");
                            break;
                        }
                    case 20:
                        {
                            Fell20.Play();
                            Debug.Log("Playing Audio - Fell20");
                            break;
                        }
                    case 21:
                        {
                            Fell21.Play();
                            Debug.Log("Playing Audio - Fell21");
                            break;
                        }
                    default:
                        {
                            Fell22.Play();
                            Debug.Log("Playing Audio - Fell22");
                            break;
                        }
                }
                j++;
                PlayerScript.playVoiceLine = false;
                if (j == voiceLinesArray.Length - 1)
                {
                    Randomize(voiceLinesArray);
                    j = 0;
                }
                PlayerPrefs.SetInt("RNGSaved", RNGsaved);
                PlayerPrefs.SetInt("RNGNow", RNGnow);
                PlayerPrefs.SetInt("LondonVoiceLinesJ", j);
                PlayerPrefs.Save();
            }
            else
            {
                RNGnow++;
                PlayerPrefs.SetInt("RNGnow", RNGnow);
                PlayerPrefs.Save();
            }
        }
    }
}
