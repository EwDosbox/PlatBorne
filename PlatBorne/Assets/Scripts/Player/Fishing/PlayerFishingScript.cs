using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerFishingScript : MonoBehaviour
{
    //************* PRIVATE
    private Rigidbody2D hookRB;
    //Movement
    private bool shouldHookMoveLeft;
    private bool shouldHookMoveRight;

    private float holdTime;
    //fish Catching Logic
    private static Collider2D fishCatchArea;
    private Collider2D hookCatchArea;

    private bool isAlreadyCatching;

    private float catchingTime;
    private float catchingTimeNeeded;

    private FishSpawnScript fish;
    private FishInventory inventory; // Reference to FishInventory
    private SubtitlesManager subtitlesManager;

    //************* PUBLIC
    public GameObject fishPrefab;
    public GameObject fishRainbowPrefab;
    public AudioSource[] fishVoiceLine;
    public static Collider2D FishCatchArea
    {
        set
        {
            fishCatchArea = value;
        }
    }

    private void Awake()
    {
        subtitlesManager = FindFirstObjectByType<SubtitlesManager>();
        hookRB = GetComponent<Rigidbody2D>();
        Collider2D[] colliders = FindObjectsOfType<Collider2D>();
        hookCatchArea = colliders.FirstOrDefault(collider => collider.name == "HookCatchArea");
        shouldHookMoveLeft = false;
        shouldHookMoveRight = false;
        holdTime = 0;
        isAlreadyCatching = false;
        catchingTimeNeeded = 1.5f;

        // Find the existing FishInventory in the scene
        inventory = FindObjectOfType<FishInventory>();

        // Check if inventory was found
        if (inventory == null)
        {
            Debug.LogError("FishInventory not found in the scene!");
            return; // Exit if no inventory is found to avoid further errors
        }

        fish = new FishSpawnScript(fishPrefab);
        fish.SpawnFish(inventory.NextFishColor);

        // Random voice line
        if (PlayerPrefs.GetInt("wasFishing") == 1)
        {
            switch (Random.Range(0, 3))
            {
                case 0:
                    if (PlayerPrefs.HasKey("Subtitles"))
                        subtitlesManager.Write("Yeah sure, go fishing, we have all the time in the world...", fishVoiceLine[0].clip.length);
                    fishVoiceLine[0].Play();
                    break;
                case 1:
                    if (PlayerPrefs.HasKey("Subtitles"))
                        subtitlesManager.Write("What makes you think a little fishing could help you?", fishVoiceLine[1].clip.length);
                    fishVoiceLine[1].Play();
                    break;
                case 2:
                    if (PlayerPrefs.HasKey("Subtitles"))
                        subtitlesManager.Write("You're so desperate that you go fishing? That's kinda fishy", fishVoiceLine[2].clip.length);
                    fishVoiceLine[2].Play();
                    break;
            }
        }
    }

    private void FixedUpdate()
    {
        //Movement
        if (shouldHookMoveLeft)
        {
            hookRB.AddRelativeForce(new Vector2(-(float)Equation(Time.time - holdTime), 0));
        }
        else if (shouldHookMoveRight)
        {
            hookRB.AddRelativeForce(new Vector2(+(float)Equation(Time.time - holdTime), 0));
        }

        // Fish Catching logic
        if (Physics2D.IsTouching(hookCatchArea, fishCatchArea))
        {
            if (isAlreadyCatching)
            {
                if (Time.time - catchingTime > catchingTimeNeeded)
                {
                    // Handle catching logic
                    if (inventory.FishCatched == inventory.Count - 1)
                    {
                        inventory.CatchedFish();
                        Destroy(fish);
                        fish = new FishSpawnScript(fishRainbowPrefab);
                        fish.SpawnFish(); // Spawns rainbow fish
                    }
                    else if (inventory.FishCatched == inventory.Count)
                    {
                        SceneManager.LoadScene("Cutscene_Ending Fish");
                    }
                    else
                    {
                        inventory.CatchedFish();
                        Destroy(fish);
                        fish = new FishSpawnScript(fishPrefab);
                        fish.SpawnFish(inventory.NextFishColor);
                    }
                    isAlreadyCatching = false;
                    catchingTime = Time.time;
                }
                isAlreadyCatching = true;
            }
            else
            {
                catchingTime = Time.time;
                isAlreadyCatching = true;
            }
        }
        else
        {
            isAlreadyCatching = false;
        }
    }

    private double Equation(float time)
    {
        return 4 + (1 - 4) / (1 + Mathf.Pow(time / 0.5f, 26));
    }

    // Input Actions
    public void HookL(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            holdTime = Time.time;
            shouldHookMoveLeft = true;
            shouldHookMoveRight = false;
        }
        if (context.canceled)
        {
            holdTime = 0;
            shouldHookMoveLeft = false;
            shouldHookMoveRight = false;
        }
    }

    public void HookR(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            holdTime = Time.time;
            shouldHookMoveLeft = false;
            shouldHookMoveRight = true;
        }
        if (context.canceled)
        {
            holdTime = 0;
            shouldHookMoveLeft = false;
            shouldHookMoveRight = false;
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerPrefs.SetInt("wasFishing", 1);
            PlayerPrefs.Save();
            SceneManager.LoadScene("LevelBirmingham");
        }
    }
}
