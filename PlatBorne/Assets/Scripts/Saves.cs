using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class Saves : MonoBehaviour
{
    public void NewGameSaveReset()
    {
        PlayerPrefs.DeleteKey("NumberOfFalls_London");
        PlayerPrefs.DeleteKey("NumberOfJumps_Act1");
        PlayerPrefs.DeleteKey("Timer_London");
        PlayerPrefs.DeleteKey("Timer_Bricus");
        PlayerPrefs.DeleteKey("HunterPositionY_London");
        PlayerPrefs.DeleteKey("HunterPositionX_London");
        PlayerPrefs.DeleteKey("Level");
        PlayerPrefs.DeleteKey("GodMode");
        PlayerPrefs.DeleteKey("PussyMode");
        PlayerPrefs.DeleteKey("RNGSaved");
        PlayerPrefs.DeleteKey("RNGNow");
        PlayerPrefs.DeleteKey("HasASavedGame");
        PlayerPrefs.DeleteKey("LondonVoiceLinesJ");
        PlayerPrefs.DeleteKey("LondonVoiceLinesArray");
    }
    public void PositionSave(float positionX, float positionY)
    {
        switch (PlayerPrefs.GetString("Level"))
        {
            case "london":
                {
                    PlayerPrefs.SetFloat("HunterPositionX_London", positionX);
                    PlayerPrefs.SetFloat("HunterPositionY_London", positionY);
                    break;
                }
            case "bricus":
                {
                    break;
                }
        }
        PlayerPrefs.Save();
    }

    public void Jumps(float numberOfJumps, int stage)
    {
        PlayerPrefs.SetFloat("NumberOfJumps_Act1", numberOfJumps);
        PlayerPrefs.Save();
    }

    public void Falls(float number, int act)
    {
        if (act == 1)
        {
            PlayerPrefs.SetFloat("NumberOfFalls_London", number);
        }
        PlayerPrefs.Save();
    }

    public void timerSave(Stopwatch timer, int stage)
    {
        TimeSpan time = timer.Elapsed;
        string sTime = time.Hours.ToString() + ":" + time.Minutes.ToString() + ":" + time.Seconds.ToString();
        switch(stage)
        {
            case 1:
                {
                    PlayerPrefs.SetString("Timer_London", sTime);
                    break;
                }
            case 2:
                {
                    PlayerPrefs.SetString("Timer_Bricus", sTime);
                    break;
                }
            case 3:
                {
                    break;
                }
            case 4: 
                {
                    break;
                }
        }
        PlayerPrefs.Save();
    }

    public Stopwatch timerLoad(int stage)
    {
        Stopwatch time = new Stopwatch();
        string sTime = null;
        switch (stage)
        {
            case 1:
                {
                    sTime = PlayerPrefs.GetString("Timer_London");
                    break;
                }
            case 2:
                {
                    sTime = PlayerPrefs.GetString("Timer_Bricus");
                    break;
                }
            case 3:
                {
                    break;
                }
            case 4:
                {
                    break;
                }
        }
        time.Equals(TimeSpan.Parse(sTime));
        return time;
    }
}
