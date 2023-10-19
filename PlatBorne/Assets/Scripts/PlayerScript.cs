using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private AudioSource Fell01;
    [SerializeField] private AudioSource Fell02;
    [SerializeField] private AudioSource Fell03;
    [SerializeField] private AudioSource Fell04;
    [SerializeField] private AudioSource Fell05;
    [SerializeField] private AudioSource Fell06;
    [SerializeField] private AudioSource Fell07;
    [SerializeField] private AudioSource Fell08;
    [SerializeField] private AudioSource Fell09;
    [SerializeField] private AudioSource Fell10;
    [SerializeField] private AudioSource Fell11;
    [SerializeField] private AudioSource Fell12;
    [SerializeField] private AudioSource Fell13;
    [SerializeField] private AudioSource Fell14;
    [SerializeField] private AudioSource Fell15;
    [SerializeField] private AudioSource Fell16;
    [SerializeField] private AudioSource Fell17;
    [SerializeField] private AudioSource Fell18;
    [SerializeField] private AudioSource Fell19;
    [SerializeField] private AudioSource Fell20;
    [SerializeField] private AudioSource Fell21;
    [SerializeField] private AudioSource Fell22;
    [SerializeField] int numberOfRandom;

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
    int playerFellRNG = 1;
    public bool playVoiceLine = false;

    void Randomize(int[] voiceLines)
    {
        System.Random rng = new System.Random();
        int random1, random2;
        for (int i = 0; i < numberOfRandom; i++)
        {
            random1 = rng.Next(1, voiceLines.Length - 1);
            random2 = rng.Next(1, voiceLines.Length - 1);
            int temp = voiceLines[random1];
            voiceLines[random1] = voiceLines[random2];
            voiceLines[random2] = voiceLines[random1];
        }
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
            switch (voiceLines[j])
            {
                case 1:
                    {
                        Fell01.Play();
                        break;
                        Debug.Log("Playing Audio - Fell01");
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
            if (j == voiceLines.Length - 1)
            {
                Randomize(voiceLines);
                j = 0;
            }
        }
    }
}
