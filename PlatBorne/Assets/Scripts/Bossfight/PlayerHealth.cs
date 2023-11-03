using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Image hp1;
    public Image hp2;
    public Image hp3;
    private float timer = 0f;
    static public bool playerDeath = false;
    private bool kemoMilujuANenavidimTimery = false;
    public Bossfight bossfight;
    public void ChangeHealth()
    {
        switch(Bossfight.playerHP)
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
                    playerDeath = true;
                    break;
                }
        }
    }

    public void PlayerStart()
    {
        kemoMilujuANenavidimTimery = true;
        if (timer > 1) hp1.enabled = true;
        if (timer > 5) hp2.enabled = true;
        if (timer > 9) 
        {
            hp3.enabled = true;
            kemoMilujuANenavidimTimery = false;
        }
    }

    private void Start()
    {
        hp1.enabled = false;
        hp2.enabled = false;
        hp3.enabled = false;
    }

    private void Update()
    {
        if (kemoMilujuANenavidimTimery)
        {
            timer += Time.deltaTime;
            PlayerStart();
        }
    }
}
