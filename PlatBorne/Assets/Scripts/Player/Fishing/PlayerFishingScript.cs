using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

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


    //************* PUBLIC
    public GameObject fishPrefab;
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
        FishInventory.RandomizeColors();
        fish.SpawnFish(FishInventory.NextFishColor);
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
        //fish Catching logic
        if (Physics2D.IsTouching(hookCatchArea, fishCatchArea))
        {
            if (isAlreadyCatching)
            {
                if (Time.time - catchingTime > catchingTimeNeeded)
                {
                    FishInventory.CatchedFish();
                    Destroy(fish);
                    fish = new FishSpawnScript(fishPrefab);
                    fish.SpawnFish(FishInventory.NextFishColor);
                    isAlreadyCatching = false;
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
