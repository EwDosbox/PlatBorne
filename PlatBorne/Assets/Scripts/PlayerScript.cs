using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerScript : MonoBehaviour
{
    public SpriteRenderer renderer;
    public Rigidbody2D rigidBody;
    public TilemapCollider2D levelWoods;
    public TilemapCollider2D levelLondon;
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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Fall hitbox" && playerWasInAir && !isPlayerInAir)
        {
            if (positionYWas < positionYIs)
            {
                playerFell++;
                playVoiceLine = true;
            }
        }
        else playerWasInAir = false;

        if (collision.gameObject.name == "Damage")
        {
            playerHP--;
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
            rigidBody.velocity = Vector2.left * movementSpeed + new Vector2(0, rigidBody.velocity.y);
            renderer.sprite = spritesWalk[0];
        }
        if (Input.GetKey(KeyCode.D) && !isPlayerInAir)
        {
            rigidBody.velocity = Vector2.right * movementSpeed + new Vector2(0, rigidBody.velocity.y);
            renderer.sprite = spritesWalk[1];
        }
        if (Input.GetKey(KeyCode.W) && !isPlayerInAir)
        {
            rigidBody.velocity = Vector2.up * jumpSpeed + new Vector2(rigidBody.velocity.x, 0);
        }
        
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
    }
    private bool DoesHunterTouchGround(Collider2D hunter, Collider2D level1, Collider2D level2)
    {
        bool doesHunterTouchLevel1 = hunter.IsTouching(level1.GetComponent<TilemapCollider2D>());
        bool doesHunterTouchLevel2 = hunter.IsTouching(level2.GetComponent<TilemapCollider2D>());
        bool doesHunterTouchGround = doesHunterTouchLevel1 || doesHunterTouchLevel2;
        return doesHunterTouchGround;
    }
}
