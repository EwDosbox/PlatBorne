using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public SpriteRenderer renderer;
    public Rigidbody2D rigidBody;

    public Sprite[] spritesIdle;
    public Sprite[] spritesWalk;

    public int movementSpeed;
    // Start is called before the first frame update
    void Start()
    {
        renderer.sprite = spritesWalk[1];
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            rigidBody.velocity = Vector3.left * movementSpeed;
            renderer.sprite = spritesWalk[0];
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            rigidBody.velocity = Vector3.right * movementSpeed;
            renderer.sprite = spritesWalk[1];
        }
    }
}
