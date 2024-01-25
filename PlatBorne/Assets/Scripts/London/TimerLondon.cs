using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class LevelLondon : MonoBehaviour
{
    public Saves save;
    Stopwatch londonTimer = new Stopwatch();
    static public bool reachedTheEnd = false;
    void Start()
    {
        PlayerPrefs.SetString("Level", "london");
        if (PlayerPrefs.HasKey("Timer_London"))
        {
            londonTimer.Equals(save.timerLoad(1));
        }
        londonTimer.Start();
    }
    void Update()
    {
        save.timerSave(londonTimer, 1);
        if (reachedTheEnd)
        {
            londonTimer.Stop();
        }
    }
}
