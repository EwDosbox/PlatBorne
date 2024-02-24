using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFishingScript : MonoBehaviour
{
    private Rigidbody2D hookRB;

    private bool shouldHookMoveLeft;
    private bool shouldHookMoveRight;

    private float holdTime;

    private void Awake()
    {
        hookRB = GetComponent<Rigidbody2D>();
        shouldHookMoveLeft = false;
        shouldHookMoveRight = false;
        holdTime = 0;
    }
    private void FixedUpdate()
    {
        if (shouldHookMoveLeft)
        {
            hookRB.AddRelativeForce(new Vector2(-(float)Equation(Time.time - holdTime), 0));
        }
        else if (shouldHookMoveRight)
        {
            hookRB.AddRelativeForce(new Vector2(+(float)Equation(Time.time - holdTime), 0));
        }
    }

    private double Equation(float time)
    {
        return 4 + (1 - 4) / (1 + math.pow(time / 0.5, 26));
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
