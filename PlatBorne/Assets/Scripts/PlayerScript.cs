using System;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public Saves save;
    //***********KONAMI CHEAT CODE*************
    private static readonly KeyCode[] konamiCode =
        { KeyCode.UpArrow, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.DownArrow,
          KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.LeftArrow, KeyCode.RightArrow,
          KeyCode.B, KeyCode.A };
    private int konamiIndex = 0;
    private void KonamiCheatCodeTriggered()
    {
        save.PositionSave(transform.position.x, transform.position.y);
        SceneManager.LoadScene("LevelBoss");
        konamiIndex = 0;
    }
    //***********KONAMI CHEAT CODE*************
    [SerializeField] private AudioSource hunterDamage;
    [SerializeField] private AudioSource hunterDrop;
    [SerializeField] private AudioSource hunterJump;
    [SerializeField] private AudioSource hunterWalk;

    public Animator animator;
    public PlayerHealth health;

    //Movement Variables
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Collider2D hunterFeet;
    [SerializeField] private int walkSpeed;
    [SerializeField] private float maxJumpHeight;
    [SerializeField] private float minJumpHeight;
    [SerializeField] private float jumpModifier;
    private bool isPlayerInAir;
    private float jumpHeight;
    //Movement Variables
    //DASH variables
    [SerializeField] public bool canDash;
    private Stopwatch stopwatchDash;
    private TimeSpan timeDash;
    //DASH variables

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
    //stats
    private float numberOfJumps = 0;
    private float numberOfFalls = 0;

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
            save.timer(timer, 1);
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
        jumpHeight = minJumpHeight;
        if (PlayerPrefs.GetInt("CanDash", 0) == 0) canDash = false;
        else canDash = true;
        timeDash = new TimeSpan(0, 0, 2);
        stopwatchDash = new Stopwatch();
        stopwatchDash.Start();
        LoadMovement();
    }
    void Update()
    {
        //animator
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        animator.SetBool("IsJumping", isPlayerInAir);
        //pouze pokud se dotyka zeme
        if (Physics2D.IsTouchingLayers(hunterFeet, groundLayer)) isPlayerInAir = false;
        else isPlayerInAir = true;
        //inputs
        if (!isPlayerInAir)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rb.velocity = new Vector2(0, 0);
                jumpHeight += Time.deltaTime * jumpModifier;
                if (jumpHeight > maxJumpHeight)
                {
                    jumpHeight = maxJumpHeight;
                }
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
                jumpHeight = minJumpHeight;
                numberOfJumps++;
                save.jumps(numberOfJumps, 1);
                //Sound
                if (!isPlaying)
                {
                    hunterJump.Play();
                    isPlaying = true;
                }
            }
            if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.W))
            {
                rb.velocity = new Vector2(-walkSpeed, rb.velocity.y);
            }
            if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W))
            {
                rb.velocity = new Vector2(walkSpeed, rb.velocity.y);
            }
            if (!Input.anyKey)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            //Flipovani spritu
            if (((Input.GetKey(KeyCode.A) && transform.localScale.x > 0) ||
                 (Input.GetKey(KeyCode.D) && transform.localScale.x < 0)) &&
                !(Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)))
            {
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                rb.transform.localScale = scale;
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
                numberOfFalls++;
                save.falls(numberOfFalls, 1);
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
        if (stopwatchDash.Elapsed > timeDash) 
        {
            if (Input.GetKey(KeyCode.LeftShift) && canDash)
            {
                Dash();
                stopwatchDash.Restart();
            }
        }
        else
        {
            stopwatchDash.Start();
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
        save.PositionSave(transform.position.x, transform.position.y);
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

    private void LoadMovement()
    {        
        switch (PlayerPrefs.GetString("Level"))
        {
            case "london":
                {
                    if (PlayerPrefs.HasKey("HunterPositionX_London"))
                    {
                        float poziceY = PlayerPrefs.GetFloat("HunterPositionY_London");
                        float poziceX = PlayerPrefs.GetFloat("HunterPositionX_London");
                        UnityEngine.Debug.Log(poziceX + " " + poziceY);
                        rb.transform.position = new Vector3(poziceX, poziceY, 0.79f);
                    }
                    break;
                }
            case "bricus":
                {
                    break;
                }
        }        
    }
    private void Dash()
    {
        UnityEngine.Debug.Log("dash");
    }
}
