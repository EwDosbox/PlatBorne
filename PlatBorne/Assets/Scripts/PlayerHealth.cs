using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Image hp1;
    public Image hp2;
    public Image hp3;

    public void ChangeHealth(int health)
    {
        switch(health)
        {
            default:
                {
                    hp1.enabled = true;
                    hp2.enabled = true;
                    hp3.enabled = true;
                    break;
                }
            case 2:
                {
                    hp1.enabled = true;
                    hp2.enabled = true;
                    hp3.enabled = false;
                    break;
                }
            case 1:
                {
                    hp1.enabled = true;
                    hp2.enabled = false;
                    hp3.enabled = false;
                    break;
                }
            case 0:
                {
                    hp1.enabled = false;
                    hp2.enabled = false;
                    hp3.enabled = false;
                    break;
                }
        }
    }

    public void PlayerStart()
    {
        new WaitForSeconds(1);
        hp1.enabled = true;
        new WaitForSeconds(1);
        hp2.enabled = true;
        new WaitForSeconds(1);
        hp3.enabled = true;
    }
}
