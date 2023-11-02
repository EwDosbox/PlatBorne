using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private AudioSource hunterDamage;
    [SerializeField] private AudioSource hunterDrop;
    [SerializeField] private AudioSource hunterJump;
    [SerializeField] private AudioSource hunterWalk;

    public SpriteRenderer renderer;
    public Rigidbody2D rigidBody;
    public CompositeCollider2D levelWoods;
    public CompositeCollider2D levelLondon;
    public PlayerHealth health;
    public Collider2D hunterFeet;

    public Sprite[] spritesIdle;
    public Sprite[] spritesWalk;

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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (playerWasInAir && !isPlayerInAir)
        {
            if (collision.gameObject.name == "Fall hitbox")
            {
                if (positionYWas < positionYIs)
                {
                    hunterDrop.Play();
                    playerFell++;
                    playVoiceLine = true;
                    //Stats
                    int numberOfFall = PlayerPrefs.GetInt("NumberOfFalls", 0);
                    numberOfFall++;
                    PlayerPrefs.SetInt("NumberOfJumps", numberOfFall);
                    PlayerPrefs.Save();
                    //Stats
                }
            }
            else playerWasInAir = false;
        }

        if (collision.gameObject.name == "Damage")
        {
            playerHP--;
            hunterDamage.Play();
            health.ChangeHealth(playerHP);
            playerInvincible = true;
        }
        if (collision.gameObject.name == "Boss Hitbox Right") bossHitboxRight = true;
        else if (collision.gameObject.name == "Boss Hitbox Left") bossHitboxLeft = true;
        if (collision.gameObject.name == "Boss Hitbox Down") bossHitboxDown = true;
        else if (collision.gameObject.name == "Boss Hitbox Right") bossHitboxUp = true;
        Debug.Log(bossHitboxLeft);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Boss Hitbox Right") bossHitboxRight = false;
        else if (collision.gameObject.name == "Boss Hitbox Left") bossHitboxLeft = false;
        if (collision.gameObject.name == "Boss Hitbox Down") bossHitboxDown = false;
        else if (collision.gameObject.name == "Boss Hitbox Right") bossHitboxUp = false;
    }
    void Start()
    {
        renderer.sprite = spritesWalk[1];
    }
    void Update()
    {
        isPlayerInAir = !DoesHunterTouchGround(hunterFeet, levelWoods, levelLondon);
        rigidBody.rotation = 0;
        if (Input.GetKey(KeyCode.A) && !isPlayerInAir && !Input.GetKeyUp(KeyCode.W))
        {
            rigidBody.velocity = Vector2.left * movementSpeed +
                                 new Vector2(0, rigidBody.velocity.y);
            renderer.sprite = spritesWalk[0];
        }
        if (Input.GetKey(KeyCode.D) && !isPlayerInAir && !Input.GetKeyUp(KeyCode.W))
        {
            rigidBody.velocity = Vector2.right * movementSpeed +
                                 new Vector2(0, rigidBody.velocity.y);
            renderer.sprite = spritesWalk[1];
        }
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !isPlayerInAir)
        {
            rigidBody.velocity = new Vector2(0, 0);
        }
        if (Input.GetKey(KeyCode.W) && !isPlayerInAir)
        {
            if (jumpSpeed < 10f) jumpSpeed += stairJumpSpeed;
        }
        if(Input.GetKeyUp(KeyCode.W) &&  !isPlayerInAir)
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

        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && !isPlayerInAir)
        {
            hunterWalk.enabled = true;
        }
        else hunterWalk.enabled = false;

        if (!isPlayerInAir)
        {
            positionYIs = transform.position.y;
            positionYWas = positionYIs;
        }
        else
        {
            playerWasInAir = true;
            positionYIs = transform.position.y;
        }

        if (isPlaying)
        {
            timer += Time.deltaTime;
            if (timer > 1) isPlaying = false;
        }
        else timer = 0;
    }
    private bool DoesHunterTouchGround(Collider2D hunter, Collider2D level1, Collider2D level2)
    {
        bool doesHunterTouchLevel1 = hunter.IsTouching(level1);
        bool doesHunterTouchLevel2 = hunter.IsTouching(level2);
        bool doesHunterTouchGround = doesHunterTouchLevel1 || doesHunterTouchLevel2;
        return doesHunterTouchGround;
    }
}
