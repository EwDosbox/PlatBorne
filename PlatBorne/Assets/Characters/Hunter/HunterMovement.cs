using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HunterMovement : MonoBehaviour
{
    private Hunter_Input input = null;
    private Vector2 moveVector = Vector2.zero;
    private Rigidbody2D rb = null;
    public float moveSpeed;

    private void Awake()
    {
        input = new Hunter_Input();
        rb = GetComponent<Rigidbody2D>();

    }
    private void OnEnable()
    {
        input.Enable();
        input.Hunter.Movement.performed += OnMovementPerformed;
        input.Hunter.Movement.canceled += OnMovementCanceled;
    }
    private void OnDisable()
    {
        input.Disable();
        input.Hunter.Movement.performed -= OnMovementPerformed;
        input.Hunter.Movement.canceled -= OnMovementCanceled;
    }
    private void FixedUpdate()
    {
        rb.velocity = moveVector * moveSpeed;
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>();
    }
    private void OnMovementCanceled(InputAction.CallbackContext value)
    {
        moveVector = Vector2.zero;
    }
}
