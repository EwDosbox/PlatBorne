using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Mole_LevelMove : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (PlayerPrefs.HasKey("Brecus_BeatenWithPussy") || PlayerPrefs.HasKey("Mole_BeatenWithPussy"))
            {
                SceneManager.LoadScene("Cutscene_BadEnding");
            }
            else SceneManager.LoadScene("Cutscene_GoodEnding");
        }
    }
}
