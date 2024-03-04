using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;

public class Saves : MonoBehaviour
{
    public void NewGameSaveReset()
    {
        //MAIN
        PlayerPrefs.DeleteKey("Level");
        PlayerPrefs.DeleteKey("GodMode");
        PlayerPrefs.DeleteKey("PussyMode");
        //ACT I
        PlayerPrefs.DeleteKey("NumberOfFalls_London");
        PlayerPrefs.DeleteKey("NumberOfJumps_Act1");
        PlayerPrefs.DeleteKey("Timer_London");
        PlayerPrefs.DeleteKey("Timer_Brecus");
        PlayerPrefs.DeleteKey("HunterPositionY_London");
        PlayerPrefs.DeleteKey("HunterPositionX_London");
        PlayerPrefs.DeleteKey("RNGSaved");
        PlayerPrefs.DeleteKey("RNGNow");
        PlayerPrefs.DeleteKey("HasASavedGame");
        PlayerPrefs.DeleteKey("LondonVoiceLinesJ");
        PlayerPrefs.DeleteKey("LondonVoiceLinesArray");
        PlayerPrefs.DeleteKey("Brecus_BeatenWithPussy");
        //ACT II
        PlayerPrefs.DeleteKey("Mole_BeatenWithPussy");
        PlayerPrefs.DeleteKey("HunterPositionX_Birmingham");
        PlayerPrefs.DeleteKey("HunterPositionY_Birmingham");
        PlayerPrefs.DeleteKey("NumberOfFalls_Birmingham");
        PlayerPrefs.DeleteKey("NumberOfJumps_Act2");
        PlayerPrefs.DeleteKey("Timer_Birmingham");
        PlayerPrefs.DeleteKey("Timer_Mole");
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
            case "birmingham":
                {
                    PlayerPrefs.SetFloat("HunterPositionX_Birmingham", positionX);
                    PlayerPrefs.SetFloat("HunterPositionY_Birmingham", positionY);
                    break;
                }
            default: break;
        }
        PlayerPrefs.Save();
    }
    public Vector3 LoadMovement()
    {
        float positionX = 0;
        float positionY = 0;
        switch (PlayerPrefs.GetString("Level"))
        {
            case "london":
                {
                    if (PlayerPrefs.HasKey("HunterPositionX_London"))
                    {
                        positionY = PlayerPrefs.GetFloat("HunterPositionY_London");
                        positionX = PlayerPrefs.GetFloat("HunterPositionX_London");                        
                        
                    }
                    break;
                }
            case "birmingham":
                {
                    if (PlayerPrefs.HasKey("HunterPositionX_Birmingham"))
                    {
                        positionY = PlayerPrefs.GetFloat("HunterPositionY_Birmingham");
                        positionX = PlayerPrefs.GetFloat("HunterPositionX_Birmingham");
                    }
                    break;
                }
            default: break;          
        }
        return new Vector3(positionX, positionY, 1);
    }
    public void PlayerFell()
    {
        if (PlayerPrefs.GetString("Level") == "london" || PlayerPrefs.GetString("Level") == "brecus")
        {
            int xxx = PlayerPrefs.GetInt("NumberOfFalls_Act1");
            xxx++;
            PlayerPrefs.SetInt("NumberOfFalls_Act1", xxx);
        }
        else
        {
            int xxx = PlayerPrefs.GetInt("NumberOfFalls_Act2");
            xxx++;
            PlayerPrefs.SetInt("NumberOfFalls_Act2", xxx);
        }
        PlayerPrefs.Save();
    }

    public void TimerSave(float timer, int stage)
    {
        if (timer != 0)
        {
            switch (stage)
            {
                case 1:
                    {
                        PlayerPrefs.SetFloat("Timer_London", timer);
                        break;
                    }
                case 2:
                    {
                        PlayerPrefs.SetFloat("Timer_Brecus", timer);
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
    }

    public float TimerLoad(int stage)
    {
        switch (stage)
        {
            case 1:
                {
                    return PlayerPrefs.GetFloat("Timer_London");
                }
            case 2:
                {
                    return PlayerPrefs.GetFloat("Timer_Brecus");
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

    public void PlayerJumped()
    {
        if (PlayerPrefs.GetString("Level") == "london" || PlayerPrefs.GetString("Level") == "brecus")
        {
            int xxx = PlayerPrefs.GetInt("NumberOfJumps_Act1");
            xxx++;
            PlayerPrefs.SetInt("NumberOfJumps_Act1", xxx);            
        }
        else
        {
            int xxx = PlayerPrefs.GetInt("NumberOfJumps_Act2");
            xxx++;
            PlayerPrefs.SetInt("NumberOfJumps_Act2", xxx);
        }
        PlayerPrefs.Save();
    }
}
