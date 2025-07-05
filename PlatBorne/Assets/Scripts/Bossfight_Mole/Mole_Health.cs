using UnityEngine;
using Debug = UnityEngine.Debug;

public class Mole_Health : MonoBehaviour
{
    //INSPECTOR//
    [Tooltip("MAIN")]
    [SerializeField] private int playerDamage = 10;
    [Tooltip("OBJECTS")]
    PlayerHealth playerHealth;
    Mole_Bossfight mole;
    //PUBLIC//
    public bool bossDead = false;
    //PRIVATE//
    private bool bossStayInvincible = false;
    private int bossHealth = 100;
    private bool bossInvincible = true;
    public int BossHealth
    {
        get { return bossHealth; }
        set 
        { 
            if (value <= 0 && value >= 100) bossHealth = value; 
        }
    }
    public bool BossInvincible
    {
        get { return bossInvincible; }
        set
        { 
            bossInvincible = value;
            mole.ChangeSpriteInvincible(value);
        }
    }
    public bool BossStayInvincible //for changing phase
    {
        get { return bossStayInvincible; }
        set 
        { 
            bossStayInvincible = value;
        }
    }

    public void BossHit(bool weakspotDamage)
    {
        if (!BossInvincible || weakspotDamage)
        {            
            Debug.Log("Boss has taken a damage from player");      
            bossHealth -= playerDamage;
            BossInvincible = true;
            playerHealth.PlayerInvincible = true;
        }
        else
        {
            Debug.Log("Player has taken a damage from boss");
            playerHealth.PlayerDamage();
        }
    }

    public void BossDeath()
    {
        Mole_UI ui = new Mole_UI();
        ui.BossHPSliderDestroy();
        BossInvincible = false;
        bossHealth = 0;
        bossDead = true;
    }

    private void Awake()
    {
        playerHealth = FindAnyObjectByType<PlayerHealth>();
        mole = FindAnyObjectByType<Mole_Bossfight>();
    }
}
