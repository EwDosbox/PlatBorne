using System;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;


public class PlayerScript : MonoBehaviour
{
    public Saves save;
    [SerializeField] private AudioSource hunterDamage;
    [SerializeField] private AudioSource hunterDrop;

    public Animator animator;
    public PlayerHealth health;

    //Movement Variables
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask pushRightLayer;
    [SerializeField] private Collider2D hunterFeet;
    //Movement Variables

    float positionYWas;
    float positionYIs;
    bool playerWasInAir = false;
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
    public bool touchedFallHitbox = false;
    private bool playWalking = false;
    public VoiceLinesLevel voiceLines;
    //FISH
    public bool wasFishing = false;
    public void SetFishing(bool wasFishing) { this.wasFishing = wasFishing; }
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
        if (PlayerPrefs.HasKey("wasFishing"))
        {
            rb.transform.position = save.LoadMovementAfterFish();
            PlayerPrefs.DeleteKey("wasFishing");
            PlayerPrefs.Save();
        }
        else rb.transform.position = save.LoadMovement(transform.position);
    }
    void Update()
    {
        //stairs will now push!
        if(Physics2D.IsTouchingLayers(hunterFeet, pushRightLayer)) rb.position = new Vector2(rb.position.x + 0.01f, rb.position.y);
        //inputs
        if (!PlayerInputScript.isPlayerInAir)
        {
            //fell
            if (playerWasInAir && touchedFallHitbox && (positionYWas > transform.position.y) && (math.abs(positionYWas - positionYIs) > 2.5))
            {
                hunterDrop.Play();
                voiceLines.PlayVL = true;
                playerWasInAir = false;
                touchedFallHitbox = false;
                save.PlayerFell();
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
        }
        //damage
        if (Bossfight.playerPlayDamage && !health.GodMode)
        {
            hunterDamage.Play();
            Bossfight.playerPlayDamage = false;
        }
        save.PositionSave(transform.position.x, transform.position.y);
    }
}
