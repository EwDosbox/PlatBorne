using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Saves : MonoBehaviour
{
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

    public void jumps(float numberOfJumps, int stage)
    {
        PlayerPrefs.SetFloat("NumberOfJumps_Act1", numberOfJumps);
        PlayerPrefs.Save();
    }

    public void falls(float number, int act)
    {
        if (act == 1)
        {
            PlayerPrefs.SetFloat("NumberOfFalls_London", number);
        }
        PlayerPrefs.Save();
    }

    public void timer(float time, int stage)
    {
        switch(stage)
        {
            case 1:
                {
                    PlayerPrefs.SetFloat("Timer_London", time);
                    break;
                }
            case 2:
                {
                    PlayerPrefs.SetFloat("Timer_Bricus", time);
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
}
