using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;

public class PlayerInputScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    private float jumpHeight;
    [SerializeField] private float maxJumpHeight;
    [SerializeField] private float minJumpHeight;
    [SerializeField] private float jumpModifier;
    private float jumpTime;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Collider2D feet;

    [SerializeField] private float movementSpeed;
    static public bool isPlayerInAir;
    private bool shouldWalkL = false;
    private bool shouldWalkR = false;
    private bool shouldJump;

    private bool jumpIsPressed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        jumpHeight = minJumpHeight;
    }
    private void FixedUpdate()
    {
        animator.SetBool("IsJumping", isPlayerInAir);
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        isPlayerInAir = !Physics2D.IsTouchingLayers(feet, groundLayer);
        if (!isPlayerInAir)
        {
            if (!jumpIsPressed)
            {
                if (shouldWalkR) rb.velocity = new Vector2(+movementSpeed, rb.velocity.y);
                else if (shouldWalkL) rb.velocity = new Vector2(-movementSpeed, rb.velocity.y);
                else rb.velocity = new Vector2(0, rb.velocity.y);
            }
            if (jumpIsPressed) rb.velocity = new Vector2(0, rb.velocity.y);
            if (shouldJump)
            {// behaves like a weird stopwatch
                jumpHeight = minJumpHeight + (Time.time - jumpTime) * jumpModifier;
                if (jumpHeight >= maxJumpHeight) jumpHeight = maxJumpHeight;
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
                shouldJump = false;
            }
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            jumpTime = Time.time;//when i pressed
            jumpIsPressed = true;
        }
        if (context.canceled)
        {
            shouldJump = true;
            jumpIsPressed = false;
        }
        //hunterWalk.enabled = false;
    }
    public void WalkL(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            shouldWalkL = true;
            shouldWalkR = false;
            if (!isPlayerInAir) transform.localScale = new Vector2(-0.19f, 0.19f);
        }
        if (context.canceled)
        {
            shouldWalkL = false;
        }
    }
    public void WalkR(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            shouldWalkL = false;
            shouldWalkR = true;
            if (!isPlayerInAir) transform.localScale = new Vector2(+0.19f, 0.19f);
        }
        if (context.canceled)
        {
            shouldWalkR = false;
        }
    }
    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Dash");
        }
    }
}
/*

if (Input.GetKeyUp(KeyCode.W))
{
    numberOfJumps++;
    save.Jumps(numberOfJumps, 1);
    PlayerPrefs.Save();
    //Sound
    if (!isPlaying)
    {
        hunterJump.Play();
        isPlaying = true;
    }
}
if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.W))
{
    rb.velocity = new Vector2(-walkSpeed, rb.velocity.y);
}
if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W))
{
    rb.velocity = new Vector2(walkSpeed, rb.velocity.y);
}
if (!Input.anyKey)
{
    rb.velocity = new Vector2(0, rb.velocity.y);
}*/