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
        if (PlayerPrefs.GetString("Level") == "fish")
        {
            playerScript.PlayerLoadCustomMovement(5.24f, -3.75f, 0);
        }
        PlayerPrefs.SetString("Level", "birmingham");
        levelTimer.Equals(save.TimerLoad(3));
    }

    void Update()
    {
        //delete later
        if (Input.GetKeyDown(KeyCode.Numlock))
        {
            playerScript.PlayerLoadCustomMovement(3.92f, 64.34f, 0);
        }
        //end here
        levelTimer += Time.deltaTime;
        save.TimerSave(levelTimer, 3);
    }
}
