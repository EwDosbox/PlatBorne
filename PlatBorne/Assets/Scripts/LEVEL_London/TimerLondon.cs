using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class LevelLondon : MonoBehaviour
{
    public Saves save;
    private float londonTimer;
    static public bool reachedTheEnd = false;
    void Start()
    {
        PlayerPrefs.SetString("Level", "london");
        londonTimer.Equals(save.TimerLoad(1));
    }
    void Update()
    {
        londonTimer += Time.deltaTime;
        if (!reachedTheEnd)
        {
            save.TimerSave(londonTimer, 1);
        }
    }
}
