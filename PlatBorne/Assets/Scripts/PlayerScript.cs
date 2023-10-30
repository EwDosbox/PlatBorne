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

    public Sprite[] spritesIdle;
    public Sprite[] spritesWalk;

    bool isPlayerInAir;
    public int movementSpeed;
    public int jumpSpeed;
    double positionYWas;
    double positionYIs;
    bool playerWasInAir = false;
    public int playerFell = 0;
    public bool playVoiceLine = false;
    public bool bossHitboxRight = false;
    public bool bossHitboxLeft = false;
    public bool bossHitboxUp = false;
    public bool bossHitboxDown = false;
    public bool bossHitbox = false;
    public bool arenaStart = false;
    public int playerHP = 3;
    public bool playerInvincible = false;
    private float timer = 0;
    private bool isPlaying = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (playerWasInAir && !isPlayerInAir)
        {
            hunterDrop.Play();
            if (collision.gameObject.name == "Fall hitbox")
            {
                if (positionYWas < positionYIs)
                {
                    playerFell++;
                    playVoiceLine = true;
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
    }
    void Start()
    {
        renderer.sprite = spritesWalk[1];
    }
    void Update()
    {
        isPlayerInAir = !DoesHunterTouchGround(this.GetComponent<Collider2D>(), levelWoods, levelLondon);
        rigidBody.rotation = 0;
        if (Input.GetKey(KeyCode.A) && !isPlayerInAir)
        {
            rigidBody.velocity = Vector2.left * movementSpeed +
                                 new Vector2(0, rigidBody.velocity.y);
            renderer.sprite = spritesWalk[0];
        }
        if (Input.GetKey(KeyCode.D) && !isPlayerInAir)
        {
            rigidBody.velocity = Vector2.right * movementSpeed +
                                 new Vector2(0, rigidBody.velocity.y);
            renderer.sprite = spritesWalk[1];
        }
        if (Input.GetKey(KeyCode.W) && !isPlayerInAir)
        {
            rigidBody.velocity = Vector2.up * jumpSpeed +
                                 new Vector2(rigidBody.velocity.x, 0);
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
