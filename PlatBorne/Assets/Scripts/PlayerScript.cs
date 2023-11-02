using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class PlayerScript : MonoBehaviour
{
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
    double positionYWas;
    double positionYIs;
    bool playerWasInAir = false;
    public int playerFell = 0;
    //*****
    static public bool playVoiceLine = false;
    static public bool bossHitboxRight = false;
    static public bool bossHitboxLeft = false;
    static public bool bossHitboxUp = false;
    static public bool bossHitboxDown = false;
    static public bool bossHitbox = false;
    //******
    public int playerHP = 3;
    public bool playerInvincible = false;
    private float timer = 0;
    private bool isPlaying = false;
    private float stairJumpSpeed = 0.009f;
    private bool touchedFallHitbox = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boss Hitbox"))
        {
            bossHitbox = true;
        }
        if (collision.gameObject.CompareTag("Damage"))
        {
            playerHP--;
            hunterDamage.Play();
            health.ChangeHealth(playerHP);
            playerInvincible = true;
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
        if (collision.gameObject.name == "Level Finish") SceneManager.LoadScene("LevelBoss");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Boss Hitbox Right")) bossHitboxRight = false;
        if (collision.gameObject.CompareTag("Boss Hitbox Left")) bossHitboxLeft = false;
        if (collision.gameObject.CompareTag("Boss Hitbox Down")) bossHitboxDown = false;
        if (collision.gameObject.CompareTag("Boss Hitbox Up")) bossHitboxUp = false;
        if (collision.gameObject.CompareTag("Boss Hitbox")) bossHitbox = false;
    }
    void Update()
    {
        isPlayerInAir = !DoesHunterTouchGround(hunterFeet, levelWoods, levelLondon);
        rigidBody.rotation = 0;
        //animator
        animator.SetFloat("Speed", Mathf.Abs(rigidBody.velocity.x));
        animator.SetBool("IsJumping", isPlayerInAir);
        //inputs
        if (Input.GetKey(KeyCode.A) && !isPlayerInAir && !Input.GetKey(KeyCode.W))
        {
            rigidBody.velocity = Vector2.left * movementSpeed +
                                 new Vector2(0, rigidBody.velocity.y);
        }
        if (Input.GetKey(KeyCode.D) && !isPlayerInAir && !Input.GetKey(KeyCode.W))
        {
            rigidBody.velocity = Vector2.right * movementSpeed +
                                 new Vector2(0, rigidBody.velocity.y);
        }
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !isPlayerInAir)
        {
            rigidBody.velocity = new Vector2(0, 0);
        }
        if (Input.GetKey(KeyCode.W) && !isPlayerInAir)
        {
            rigidBody.velocity = Vector2.zero;
            if (jumpSpeed < 10f) jumpSpeed += stairJumpSpeed;
        }
        //flipovani spritu
        if (((Input.GetKeyDown(KeyCode.A) && transform.localScale.x > 0) ||
            (Input.GetKeyDown(KeyCode.D) && transform.localScale.x < 0)) &&
            !isPlayerInAir)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            rigidBody.transform.localScale = scale;
        }

        if (Input.GetKeyUp(KeyCode.W) && !isPlayerInAir)
        {
            rigidBody.velocity = Vector2.up * jumpSpeed +
                                 new Vector2(rigidBody.velocity.x, 0);
            jumpSpeed = 5;
            //Stats
            int numberOfJump = PlayerPrefs.GetInt("NumberOfJumps", 0);
            numberOfJump++;
            PlayerPrefs.SetInt("NumberOfJumps", numberOfJump);
            PlayerPrefs.Save();
            //Stats
            if (!isPlaying)
            {
                hunterJump.Play();
                isPlaying = true;
            }
        }

        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && !isPlayerInAir && !Input.GetKey(KeyCode.W))
        {
            hunterWalk.enabled = true;
        }
        else hunterWalk.enabled = false;

        positionYIs = transform.position.y;
        if (!isPlayerInAir)
        {
            if (!touchedFallHitbox) positionYWas = positionYIs;
        }
        else
        {
            playerWasInAir = true;
        }

        if (isPlaying)
        {
            timer += Time.deltaTime;
            if (timer > 1) isPlaying = false;
        }
        else timer = 0;

        if (playerWasInAir && !isPlayerInAir && touchedFallHitbox)
        {
            if (positionYWas > positionYIs)
                {
                    Debug.Log("Player fell");
                positionYWas = positionYIs;
                    hunterDrop.Play();
                    playerFell++;
                    playVoiceLine = true;
                    //Stats
                    int numberOfFall = PlayerPrefs.GetInt("NumberOfFalls", 0);
                    numberOfFall++;
                    Debug.Log(numberOfFall);
                    PlayerPrefs.SetInt("NumberOfJumps", numberOfFall);
                    PlayerPrefs.Save();
                    //Stats
                }
            else
            {
                playerWasInAir = false;
                touchedFallHitbox = false;
            }
        }
    }
        private bool DoesHunterTouchGround(Collider2D hunter, Collider2D level1, Collider2D level2)
        {
            bool doesHunterTouchLevel1 = hunter.IsTouching(level1);
            bool doesHunterTouchLevel2 = hunter.IsTouching(level2);
            bool doesHunterTouchGround = doesHunterTouchLevel1 || doesHunterTouchLevel2;
            return doesHunterTouchGround;
        }
}
