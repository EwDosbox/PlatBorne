using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoadSystem
{
    GameObject player;
    public static void PlayerSave(string nameOfCurrentLevel, float playerPositionX, float playerPositionY, float londonTimer)
    {
        PlayerPrefs.SetString("CurrentLevel", nameOfCurrentLevel);
        PlayerPrefs.SetFloat("PlayerPositionX", playerPositionX);
        PlayerPrefs.SetFloat("PlayerPositionY", playerPositionY);
        PlayerPrefs.SetFloat("LondonTimer", londonTimer);
    }
    public static void PlayerSave(string nameOfCurrentLevel, float playerPositionX, float playerPositionY, float bossFightTimer, int PlayerHP)
    {
        PlayerPrefs.SetString("CurrentLevel", nameOfCurrentLevel);
        PlayerPrefs.SetFloat("PlayerPositionX", playerPositionX);
        PlayerPrefs.SetFloat("PlayerPositionY", playerPositionY);
        PlayerPrefs.SetFloat("BossFightTimer", bossFightTimer);
    }

    public static void PlayerLoad(out string currentLevel, out float playerPositionX, out float playerPositionY)
    {
        currentLevel = PlayerPrefs.GetString("CurrentLevel");
        playerPositionX = PlayerPrefs.GetFloat("PlayerPositionX");
        playerPositionY = PlayerPrefs.GetFloat("PlayerPositionY");
    }
}
