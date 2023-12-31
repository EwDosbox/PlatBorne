using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    //***********KONAMI CHEAT CODE*************
    private static readonly KeyCode[] konamiCode =
        { KeyCode.UpArrow, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.DownArrow,
          KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.LeftArrow, KeyCode.RightArrow,
          KeyCode.B, KeyCode.A };
    private int konamiIndex = 0;
    private void KonamiCheatCodeTriggered()
    {
        SceneManager.LoadScene("LevelBoss");
        konamiIndex = 0;
    }
    //***********KONAMI CHEAT CODE*************
    [SerializeField] private AudioSource hunterDamage;
    [SerializeField] private AudioSource hunterDrop;
    [SerializeField] private AudioSource hunterJump;
    [SerializeField] private AudioSource hunterWalk;

    public Animator animator;
    public Rigidbody2D rigidBody;
    public CompositeCollider2D levelWoods;
    public CompositeCollider2D levelLondon;
    public PlayerHealth health;
    public Collider2D hunterFeet;

    bool isPlayerInAir;
    public int movementSpeed;
    public float jumpSpeed;
    float positionYWas;
    float positionYIs;
    bool playerWasInAir = false;
    public int playerFell = 0;
    //*****
    static public bool playVoiceLine = false;
    static public bool bossHitboxRight = false;
    static public bool bossHitboxLeft = false;
    static public bool bossHitboxUp = false;
    static public bool bossHitboxDown = false;
    static public bool bossHitbox = false;
    static public bool bossDamage = false;
    //******
    public bool playerInvincible = false;
    private float timer = 0;
    private bool isPlaying = false;
    private bool touchedFallHitbox = false;
    public float londonTimer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boss Hitbox"))
        {
            bossHitbox = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boss Hitbox"))
        {
            bossHitbox = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Boss Hitbox Right")) bossHitboxRight = true;
        if (collision.gameObject.CompareTag("Boss Hitbox Left")) bossHitboxLeft = true;
        if (collision.gameObject.CompareTag("Boss Hitbox Down")) bossHitboxDown = true;
        if (collision.gameObject.CompareTag("Boss Hitbox Up")) bossHitboxUp = true;
        if (collision.gameObject.CompareTag("Boss Hitbox")) bossHitbox = true;
        if (collision.gameObject.CompareTag("Fall Hitbox")) touchedFallHitbox = true;
        if (collision.gameObject.CompareTag("LevelLondon_Finish"))
        {
            PlayerPrefs.SetFloat("GameTimer", PlayerPrefs.GetFloat("LondonTimer"));
            PlayerPrefs.Save();
            SceneManager.LoadScene("LevelBoss");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Damage")) bossDamage = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Boss Hitbox Right")) bossHitboxRight = false;
        if (collision.gameObject.CompareTag("Boss Hitbox Left")) bossHitboxLeft = false;
        if (collision.gameObject.CompareTag("Boss Hitbox Down")) bossHitboxDown = false;
        if (collision.gameObject.CompareTag("Boss Hitbox Up")) bossHitboxUp = false;
        if (collision.gameObject.CompareTag("Boss Hitbox")) bossHitbox = false;
        if (collision.gameObject.CompareTag("Damage")) bossDamage = false;
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("Level"))
        {
            if (PlayerPrefs.GetString("Level") == "london")
            {
                if (PlayerPrefs.HasKey("HunterPositionX"))
                {
                    float poziceX = PlayerPrefs.GetFloat("HunterPositionX");
                    float poziceY = PlayerPrefs.GetFloat("HunterPositionY");
                    rigidBody.transform.position = new Vector3(poziceX, poziceY, 0.79f);
                }
            }
        }
        PlayerPrefs.SetString("Level", "london");
        if (!PlayerPrefs.HasKey("LondonTimer"))
        {
            PlayerPrefs.SetFloat("LondonTimer", 0f);
            PlayerPrefs.Save();
        }
    }
    void Update()
    {
        rigidBody.rotation = 0;
        //animator
        animator.SetFloat("Speed", Mathf.Abs(rigidBody.velocity.x));
        animator.SetBool("IsJumping", isPlayerInAir);
        //pouze pokud se dotyka zeme
        if (hunterFeet.IsTouching(levelLondon) ||
           hunterFeet.IsTouching(levelWoods))
        {
            isPlayerInAir = false;
        }
        else isPlayerInAir = true;
        //inputs
        if (!isPlayerInAir)
        {
            if (Input.GetKey(KeyCode.A))
            {
                rigidBody.velocity = new Vector2(-1 * movementSpeed, rigidBody.velocity.y);
            }
            if (Input.GetKey(KeyCode.D))
            {
                rigidBody.velocity = new Vector2(1 * movementSpeed, rigidBody.velocity.y);
            }
            if (Input.GetKey(KeyCode.W))
            {
                rigidBody.velocity = new Vector2(0, 0);
                if (jumpSpeed < 10f)
                {
                    if (Bossfight.bossfightStarted) jumpSpeed += Time.deltaTime * 8;
                    else jumpSpeed += Time.deltaTime * 4; //Double jump speed
                }
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpSpeed);
                if (Bossfight.bossfightStarted) jumpSpeed = 7f;
                else jumpSpeed = 5f;
                //Stats
                float numberOfJumps = PlayerPrefs.GetFloat("NumberOfJumps", 0f);
                PlayerPrefs.SetFloat("NumberOfJumps", numberOfJumps);
                PlayerPrefs.Save();
                //Sound
                if (!isPlaying)
                {
                    hunterJump.Play();
                    isPlaying = true;
                }
            }
            //no momentum
            if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W))
            {
                rigidBody.velocity = Vector2.zero;
            }
            //Flipovani spritu
            if (((Input.GetKey(KeyCode.A) && transform.localScale.x > 0) ||
                 (Input.GetKey(KeyCode.D) && transform.localScale.x < 0)) &&
                !(Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)))
            {
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                rigidBody.transform.localScale = scale;
            }
            //Sound
            if (((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))) && !Input.GetKey(KeyCode.W))
            {
                hunterWalk.enabled = true;
            }
            else hunterWalk.enabled = false;
            //fell
            if (playerWasInAir && touchedFallHitbox && (positionYWas > transform.position.y) && (math.abs(positionYWas - positionYIs) > 2.5))
            {
                hunterDrop.Play();
                playerFell++;
                playVoiceLine = true;
                playerWasInAir = false;
                touchedFallHitbox = false;
                //save
                float numberOfFalls = PlayerPrefs.GetFloat("NumberOfFalls", 0);
                numberOfFalls++;
                PlayerPrefs.SetFloat("NumberOfFalls", numberOfFalls);
                PlayerPrefs.Save();
            }
            else
            {
                positionYWas = transform.position.y;
                playerWasInAir = false;
                touchedFallHitbox = false;
            }
        }
        else
        {
            playerWasInAir = true;
            hunterWalk.enabled = false;
        }
        //sound vyskoceni
        if (isPlaying)
        {
            timer += Time.deltaTime;
            if (timer > 1) isPlaying = false;
        }
        else timer = 0;
        //damage
        if (Bossfight.playerPlayDamage)
        {
            hunterDamage.Play();
            Bossfight.playerPlayDamage = false;
        }
        //Save
        PlayerPrefs.SetFloat("HunterPositionX", transform.position.x);
        PlayerPrefs.SetFloat("HunterPositionY", transform.position.y);
        londonTimer = PlayerPrefs.GetFloat("LondonTimer");
        londonTimer += Time.deltaTime;
        PlayerPrefs.SetFloat("LondonTimer", londonTimer);
        PlayerPrefs.Save();
        //****KONAMI CODE****
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(konamiCode[konamiIndex]))
            {
                konamiIndex++;
                if (konamiIndex == konamiCode.Length)
                {
                    KonamiCheatCodeTriggered();
                }
            }
            else konamiIndex = 0;
        }
    }
}
