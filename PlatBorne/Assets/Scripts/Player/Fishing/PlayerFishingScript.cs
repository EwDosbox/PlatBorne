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
    private FishInventory inventory;

    //************* PUBLIC
    public GameObject fishPrefab;
    public GameObject fishRainbowPrefab;
    public Saves save;
    public static Collider2D FishCatchArea
    {
        set
        {
            fishCatchArea = value;
        }
    }

    private void Awake()
    {
        hookRB = GetComponent<Rigidbody2D>();
        Collider2D[] colliders = FindObjectsOfType<Collider2D>();
        hookCatchArea = colliders.FirstOrDefault(collider => collider.name == "HookCatchArea");
        shouldHookMoveLeft = false;
        shouldHookMoveRight = false;
        holdTime = 0;
        isAlreadyCatching = false;
        catchingTimeNeeded = 1.5f;
        fish = new FishSpawnScript(fishPrefab);
        inventory = new FishInventory();
        fish.SpawnFish(inventory.NextFishColor);
    }
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            SceneManager.LoadScene("LevelBirmingham");
            save.PositionSave(6.57f, -3.82f);
        }
        //Movement
        if (shouldHookMoveLeft)
        {
            hookRB.AddRelativeForce(new Vector2(-(float)Equation(Time.time - holdTime), 0));
        }
        else if (shouldHookMoveRight)
        {
            hookRB.AddRelativeForce(new Vector2(+(float)Equation(Time.time - holdTime), 0));
        }
        //fish Catching logic
        if (Physics2D.IsTouching(hookCatchArea, fishCatchArea))
        {
            if (isAlreadyCatching)
            {
                if (Time.time - catchingTime > catchingTimeNeeded)
                {
                    //nespawnuje se rainbow fish
                    if (inventory.FishCatched == inventory.Count - 1)
                    {
                        inventory.CatchedFish();
                        Destroy(fish);
                        fish = new FishSpawnScript(fishRainbowPrefab);
                        fish.SpawnFish();// spawns rainbow one
                    }
                    else if (inventory.FishCatched == inventory.Count)
                    {
                        SceneManager.LoadScene("Cutsene_Ending Fish");
                    }
                    else
                    {
                        inventory.CatchedFish();
                        Destroy(fish);
                        fish = new FishSpawnScript(fishPrefab);
                        fish.SpawnFish(inventory.NextFishColor);//haze chybu
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
        else isAlreadyCatching = false;
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
}
