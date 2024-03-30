using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBirmingham : MonoBehaviour
{
    float levelTimer = 0;
    public Saves save;
    public PlayerScript playerScript;
    void Start()
    {
        PlayerPrefs.SetString("Level", "birmingham");
        levelTimer.Equals(save.TimerLoad(3));
    }

    void Update()
    {
        levelTimer += Time.deltaTime;
        save.TimerSave(levelTimer, 3);
    }
}
