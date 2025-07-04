using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public Vector3 LoadMovement(Vector3 beforeMovement)
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "LevelLondon": return new Vector3(PlayerPrefs.GetFloat("HunterPositionX_London", -14.6f), PlayerPrefs.GetFloat("HunterPositionY_London", -67.84f), 0);
            case "LevelBoss": return new Vector3(-31.0946f, -11.26171f, 1);
            case "LevelBirmingham": return new Vector3(PlayerPrefs.GetFloat("HunterPositionX_Birmingham", -37.87f), PlayerPrefs.GetFloat("HunterPositionY_Birmingham", -4.28f), 0);
            case "LevelMole": return new Vector3(-37.12f, -5.78f, 1);
            default: return beforeMovement;
        }
    }

    public Vector3 LoadMovementAfterFish() //pain
    {
        return new Vector3(4.44f, -3.846775f, 0);
    }

    public void PlayerFell()
    {
        if (PlayerPrefs.GetString("Level") == "london")
        {
            int xxx = PlayerPrefs.GetInt("NumberOfFalls_Act1");
            xxx++;
            PlayerPrefs.SetInt("NumberOfFalls_Act1", xxx);
        }
        else if (PlayerPrefs.GetString("Level") == "birmingham")
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
        if (PlayerPrefs.GetString("Level") == "london" || PlayerPrefs.GetString("Level") == "bricus")
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

    public void FishInventory(bool resetInventory)
    {
        if (resetInventory) PlayerPrefs.DeleteKey("FishInventory");
        else
        {
            int temp = PlayerPrefs.GetInt("FishInventory", 0);
            PlayerPrefs.SetInt("FishInventory", temp++);
        }
        PlayerPrefs.Save();
    }

    public int FishInventory()
    {
        return PlayerPrefs.GetInt("FishInventory", 0);
    }
}
