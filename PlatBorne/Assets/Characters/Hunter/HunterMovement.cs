using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class HunterMovement : MonoBehaviour
{
    //VITAL
    private Hunter_Input input = null;
    private Rigidbody2D rb = null;

    //moveSpeed
    [SerializeField] private float moveSpeed;

    //jump
    [SerializeField] private float maxJumpHeight;
    [SerializeField] private float minJumpHeight;
    [SerializeField] private float jumpModifier;
    private float jumpHeight = 0;
    private Stopwatch jumpStopwatch;

    //playerInAir
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Collider2D hunterFeet;
    private bool isPlayerInAir;

    private void Update()
    {
        isPlayerInAir = Physics2D.IsTouchingLayers(hunterFeet, groundLayer);
        UnityEngine.Debug.Log(isPlayerInAir);
    }
    private void Awake()
    {
        jumpStopwatch = new Stopwatch();
        input = new Hunter_Input();
        rb = GetComponent<Rigidbody2D>();

    }
    private void OnEnable()
    {
        if(!isPlayerInAir)
        {
            input.Enable();
            input.Hunter.Movement.performed += OnMovementPerformed;
            input.Hunter.Movement.canceled += OnMovementCanceled;
            input.Hunter.Jump.performed += OnJumpPerformed;
            input.Hunter.Jump.canceled += OnJumpCanceled;
        }
    }
    private void OnDisable()
    {
        if (!isPlayerInAir)
        {
            input.Disable();
            input.Hunter.Movement.performed -= OnMovementPerformed;
            input.Hunter.Movement.canceled -= OnMovementCanceled;
            input.Hunter.Jump.performed -= OnJumpPerformed;
            input.Hunter.Jump.canceled -= OnJumpCanceled;
        }
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        rb.velocity = value.ReadValue<Vector2>() * moveSpeed;
    }
    private void OnMovementCanceled(InputAction.CallbackContext value)
    {
        rb.velocity = value.ReadValue<Vector2>();
    }
    private void OnJumpPerformed(InputAction.CallbackContext value)
    {
        jumpStopwatch.Start();
    }
    private void OnJumpCanceled(InputAction.CallbackContext value)
    {
        jumpHeight = minJumpHeight + (float) jumpStopwatch.Elapsed.TotalSeconds * jumpModifier;
        if (jumpHeight > maxJumpHeight)
        {
            jumpHeight = maxJumpHeight;
        }
        jumpStopwatch = new Stopwatch();
        rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        jumpHeight = 0;
    }
}
