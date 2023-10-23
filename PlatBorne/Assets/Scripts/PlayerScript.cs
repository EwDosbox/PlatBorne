using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private AudioSource Fell01, Fell02, Fell03, Fell04, Fell05, Fell06, Fell07, Fell08, Fell09, Fell10, Fell11, Fell12, Fell13, Fell14, Fell15, Fell16, Fell17, Fell18, Fell19, Fell20, Fell21, Fell22;


    int j = 0;
    int[] voiceLinesArray = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22 };
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

    void Randomize(int[] voiceLines)
    {
        System.Random rng = new System.Random();
        int random1, random2;
        random1 = rng.Next(1, voiceLines.Length - 1);
        random2 = rng.Next(1, voiceLines.Length - 1);
        int temp = voiceLines[random1];
        voiceLines[random1] = voiceLines[random2];
        voiceLines[random2] = voiceLines[random1];
        return;
    }
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
        Randomize(voiceLinesArray);
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
        //VoiceLines London
        if (playVoiceLine)
        {
            switch (voiceLinesArray[j])
            {
                case 1:
                    {
                        Fell01.Play();                        
                        Debug.Log("Playing Audio - Fell01");
                        break;
                    }
                case 2:
                    {
                        Fell02.Play();
                        Debug.Log("Playing Audio - Fell02");
                        break;
                    }
                case 3:
                    {
                        Fell03.Play();
                        Debug.Log("Playing Audio - Fell03");
                        break;                        
                    }
                case 4:
                    {
                        Fell04.Play();
                        Debug.Log("Playing Audio - Fell04");
                        break;
                    }
                case 5:
                    {
                        Fell05.Play();
                        Debug.Log("Playing Audio - Fell05");
                        break;
                    }
                case 6:
                    {
                        Fell06.Play();
                        Debug.Log("Playing Audio - Fell06");
                        break;
                    }
                case 7:
                    {
                        Fell07.Play();
                        Debug.Log("Playing Audio - Fell07");
                        break;
                    }
                case 8:
                    {
                        Fell08.Play();
                        Debug.Log("Playing Audio - Fell08");
                        break;
                    }
                case 9:
                    {
                        Fell09.Play();
                        Debug.Log("Playing Audio - Fell09");
                        break;
                    }
                case 10:
                    {
                        Fell10.Play();
                        Debug.Log("Playing Audio - Fell10");
                        break;
                    }
                case 11:
                    {
                        Fell11.Play();
                        Debug.Log("Playing Audio - Fell11");
                        break;
                    }
                case 12:
                    {
                        Fell12.Play();
                        Debug.Log("Playing Audio - Fell12");
                        break;
                    }
                case 13:
                    {
                        Fell13.Play();
                        Debug.Log("Playing Audio - Fell13");
                        break;
                    }
                case 14:
                    {
                        Fell14.Play();
                        Debug.Log("Playing Audio - Fell14");
                        break;
                    }
                case 15:
                    {
                        Fell15.Play();
                        Debug.Log("Playing Audio - Fell15");
                        break;
                    }
                case 16:
                    {
                        Fell16.Play();
                        Debug.Log("Playing Audio - Fell16");
                        break;
                    }
                case 17:
                    {
                        Fell17.Play();
                        Debug.Log("Playing Audio - Fell17");
                        break;
                    }
                case 18:
                    {
                        Fell18.Play();
                        Debug.Log("Playing Audio - Fell18");
                        break;
                    }
                case 19:
                    {
                        Fell19.Play();
                        Debug.Log("Playing Audio - Fell19");
                        break;
                    }
                case 20:
                    {
                        Fell20.Play();
                        Debug.Log("Playing Audio - Fell20");
                        break;
                    }
                case 21:
                    {
                        Fell21.Play();
                        Debug.Log("Playing Audio - Fell21");
                        break;
                    }
                default:
                    {
                        Fell22.Play();
                        Debug.Log("Playing Audio - Fell22");
                        break;
                    }
            }
            j++;
            if (j == voiceLinesArray.Length - 1)
            {
                Randomize(voiceLinesArray);
                j = 0;
            }
        }
    }
}
