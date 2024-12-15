using System;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;


public class PlayerScript : MonoBehaviour
{
    Saves save;
    [SerializeField] private AudioSource hunterDamage;
    [SerializeField] private AudioSource hunterDrop01;
    [SerializeField] private AudioSource hunterDrop02;
    PlayerHealth playerHealth;

    public Animator animator;
    PlayerInputScript playerInputScript;

    //Movement Variables
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask pushRightLayer;
    [SerializeField] private Collider2D hunterFeet;
    //Movement Variables

    float positionYWas;
    bool playerWasInAir = false;
    float distanceOfFall = 0;
    //*****
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
    bool hunterIsDown = false;
    public VoiceLinesLevel voiceLines;
    //FISH
    public bool wasFishing = false;
    public void SetFishing(bool wasFishing) { this.wasFishing = wasFishing; }
    public Vector2 Position
    {
        get { return new Vector2(transform.position.x, transform.position.y); }
    }
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
        if (collision.CompareTag("Damage") && SceneManager.GetActiveScene().name != "MoleBossfight") playerHealth.PlayerDamage();
        if (collision.CompareTag("Boss Hitbox Right")) bossHitboxRight = true;
        if (collision.CompareTag("Boss Hitbox Left")) bossHitboxLeft = true;
        if (collision.CompareTag("Boss Hitbox Down")) bossHitboxDown = true;
        if (collision.CompareTag("Boss Hitbox")) bossHitbox = true;
        if (collision.CompareTag("Fall Hitbox")) touchedFallHitbox = true;        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Damage")) bossDamage = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Boss Hitbox Right")) bossHitboxRight = false;
        if (collision.CompareTag("Boss Hitbox Left")) bossHitboxLeft = false;
        if (collision.CompareTag("Boss Hitbox Down")) bossHitboxDown = false;
        if (collision.CompareTag("Boss Hitbox Up")) bossHitboxUp = false;
        if (collision.CompareTag("Boss Hitbox")) bossHitbox = false;
        if (collision.CompareTag("Damage")) bossDamage = false;
    }
    private void Start()
    {
        //Components
        save = FindAnyObjectByType<Saves>();
        if (save == null) Debug.LogError("ERROR: Save is Null");
        playerHealth = FindAnyObjectByType<PlayerHealth>(FindObjectsInactive.Include);
        if (playerHealth == null) Debug.LogError("ERROR: PlayerHealth is Null");
        playerInputScript = GetComponent<PlayerInputScript>();
        if (playerInputScript == null) Debug.LogError("ERROR: PlayerInputScript is Null");

        positionYWas = transform.position.y;
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
        if (hunterIsDown && Input.anyKey)
        {
            hunterIsDown = false;
            animator.SetBool("bigFall", false);
        }
        //stairs will now push!
        if(Physics2D.IsTouchingLayers(hunterFeet, pushRightLayer)) rb.position = new Vector2(rb.position.x + 0.01f, rb.position.y);

        // Check if the player is on the ground
        if (!PlayerInputScript.isPlayerInAir)
        {
            playerInputScript.GroundDashReset();
            // Handle transition from air to ground
            if (playerWasInAir)
            {
                playerWasInAir = false;
                // Calculate fall distance
                float distanceOfFall = Mathf.Abs(positionYWas - transform.position.y);
                if (touchedFallHitbox && (positionYWas > transform.position.y) && distanceOfFall > 1f)
                {
                    if (UnityEngine.Random.Range(0f, 1f) < 0.5f) hunterDrop01.Play();
                    else hunterDrop02.Play();
                    if (distanceOfFall >= 10f)
                    {
                        voiceLines.PlayVLBigFall();
                        animator.SetBool("bigFall", true);
                        hunterIsDown = true;
                    }
                    else
                    {
                        voiceLines.PlayVLFallen();
                    }
                    save.PlayerFell();
                }
                touchedFallHitbox = false;
            }
            else positionYWas = transform.position.y;
        }
        else
        {
            playerWasInAir = true;
        }


        //damage
        if (Bossfight.playerPlayDamage && !playerHealth.GodMode)
        {
            hunterDamage.Play();
            Bossfight.playerPlayDamage = false;
        }
        save.PositionSave(transform.position.x, transform.position.y);
    }

    //For Mole Bossfight
    public void MovePlayer(float x, float y)
    {
        transform.position = new Vector3(transform.position.x + x, transform.position.y + y, 1);
    }
}
