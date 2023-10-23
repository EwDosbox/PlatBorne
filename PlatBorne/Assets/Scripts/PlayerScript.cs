using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public SpriteRenderer renderer;
    public Rigidbody2D rigidBody;
    public Collider2D collider;

    public Sprite[] spritesIdle;
    public Sprite[] spritesWalk;

    public int movementSpeed;
    public int jumpSpeed;
    bool isPlayerInAir = false;
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

        if (collision.gameObject.name == "Arena Start") arenaStart = true;
        if (collision.gameObject.name == "Boss Hitbox Right") bossHitboxRight = true;
        else if (collision.gameObject.name == "Boss Hitbox Left") bossHitboxLeft = true;
        if (collision.gameObject.name == "Boss Hitbox Down") bossHitboxDown = true;
        else if (collision.gameObject.name == "Boss Hitbox Right") bossHitboxUp = true;
    }
    void Start()
    {
        renderer.sprite = spritesWalk[1];
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics2D.IsTouching(collider, this.GetComponent<Collider2D>())) isPlayerInAir = false;
        else isPlayerInAir = true;
        rigidBody.angularVelocity = 0;
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
}
