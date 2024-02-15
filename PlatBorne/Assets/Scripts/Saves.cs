using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;

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
        else PlayerPrefs.SetFloat("NumberOfFalls_Birmingham", number);
        PlayerPrefs.Save();
    }

    public void timerSave(float timer, int stage)
    {
        switch(stage)
        {
            case 1:
                {
                    PlayerPrefs.SetFloat("Timer_London", timer);
                    break;
                }
            case 2:
                {
                    PlayerPrefs.SetFloat("Timer_Bricus", timer);
                    break;
                }
            case 3:
                {
                    PlayerPrefs.SetFloat("Timer_Birmingham", timer);
                    break;
                }
            case 4: 
                {
                    PlayerPrefs.SetFloat("Timer_Mole", timer);
                    break;
                }
        }
        PlayerPrefs.Save();
    }

    public float timerLoad(int stage)
    {
        switch (stage)
        {
            case 1:
                {
                    return PlayerPrefs.GetFloat("Timer_London");
                }
            case 2:
                {
                    return PlayerPrefs.GetFloat("Timer_Bricus");
                }
            case 3:
                {
                    return PlayerPrefs.GetFloat("Timer_Birmingham");
                }
            case 4:
                {
                    return PlayerPrefs.GetFloat("Timer_Mole");
                }
        }
        return 0;
    }
}
