using System.Threading;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;

public class PlayerInputScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    public Saves save;

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
    public AudioSource jumpSound;
    public AudioSource walkSound;
    public AudioSource dashSound;
    private bool isPlaying;
    private float time;

    //Dash
    static public bool CanDash;
    private Vector2 velocityBeforeDash;
    private bool shouldDash;
    private bool dashing;
    private float dashStarted;
    [SerializeField] private float dashLength;
    [SerializeField] private float dashDistance;

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
        if (shouldDash)
        {
            dashStarted = Time.time;
            velocityBeforeDash = rb.velocity;
            rb.velocity = new Vector2(dashDistance, 0);
            shouldDash = false;
        }
        else if (dashing)
        {
            rb.velocity = new Vector2(dashDistance, 0);
            if((Time.time - dashStarted) >= dashLength)
            {
                dashing = false;
                rb.velocity = velocityBeforeDash;
            }
        }
        else if (!isPlayerInAir)
        {
            if (!jumpIsPressed)
            {
                if (shouldWalkR)
                {
                    rb.velocity = new Vector2(+movementSpeed, rb.velocity.y);
                    walkSound.Play();
                }
                else if (shouldWalkL)
                {
                    rb.velocity = new Vector2(-movementSpeed, rb.velocity.y);
                    walkSound.Play();
                }
                else
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    walkSound.Stop();
                }
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
        if (isPlaying) //timer for a jump SFX
        {
            time += Time.deltaTime;
            if (time > 0.5f)
            {
                isPlaying = false;
                time = 0;
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
            save.PlayerJumped();
            //Sound
            if (!isPlaying)
            {
                jumpSound.Play();
                isPlaying = true;
            }
        }
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
            walkSound.Stop();
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
            if (CanDash)
            {
                dashSound.Play();
                shouldDash = true;
            }
        }
    }
}