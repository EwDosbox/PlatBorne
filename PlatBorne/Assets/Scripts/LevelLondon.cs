using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLondon : MonoBehaviour
{
    float londonTimer = 0;
    public Saves save;
    void Start()
    {
        PlayerPrefs.SetString("Level", "london");
        if (!PlayerPrefs.HasKey("Timer_London"))
        {
            PlayerPrefs.SetFloat("Timer_London", 0f);
            PlayerPrefs.Save();
        }
        else londonTimer = PlayerPrefs.GetFloat("Timer_London");
    }
    void Update()
    {
        londonTimer += Time.deltaTime;
        save.timer(londonTimer, 1);
    }
}
