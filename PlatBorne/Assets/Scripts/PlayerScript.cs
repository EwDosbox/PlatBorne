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

    public GameObject level;
    bool isPlayerInAir = false;
    // Start is called before the first frame update
    void Start()
    {
        renderer.sprite = spritesWalk[1];
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics2D.IsTouching(collider, GetComponent<Collider2D>())) isPlayerInAir = false;
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
        if (Input.GetKeyDown(KeyCode.W) && !isPlayerInAir)
        {
            rigidBody.velocity = Vector2.up * jumpSpeed + new Vector2(rigidBody.velocity.x, 0);
        }
    }

}
