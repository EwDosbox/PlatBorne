using System.Runtime.CompilerServices;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;

public class PlayerInputScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Collider2D feet;
    public Saves save;

    static public bool CanMove
    {
        get;
        set;
    }

    private float jumpHeight;
    [SerializeField] private float maxJumpHeight;
    [SerializeField] private float minJumpHeight;
    [SerializeField] private float jumpModifier;
    private float jumpTime;

    private bool isPlayerFacingLeft;
    [SerializeField] private float movementSpeed;
    static public bool isPlayerInAir;
    private bool shouldWalkL = false;
    private bool shouldWalkR = false;
    private bool shouldJump;
    private bool jumpIsPressed;
    public AudioSource jumpSound;
    public AudioSource walkSound;// placeholder in Inspector; so it doesnt throw errors
    public AudioSource dashSound;// placeholder in Inspector; so it doesnt throw errors
    private bool isPlaying;
    private float time;
    public DebugController console;

    //Dash
    [SerializeField] public bool abilityToDash;
    private bool canDash;
    private float timeOfLastDash;
    [SerializeField] public float dashCooldown;
    private Vector2 velocityBeforeDash;
    private bool shouldDash;
    private bool dashing;
    private float dashStarted;
    private bool isMoving = false;
    [SerializeField] private float dashTimeLength;
    [SerializeField] private float dashVelocity;
    private Vector3 previousPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        jumpHeight = minJumpHeight;
        timeOfLastDash = 0;
        CanMove = true;
        previousPosition = transform.position;
    }

    public void AbilityToDash(bool Dash)
    {
        if (Dash) abilityToDash = true;
        else abilityToDash = false;
    }
    private void Update()
    {
        if (!isPlayerInAir && isMoving) walkSound.enabled = true;
        else walkSound.enabled = false;
        if (console.ShowConsole)
        {
            CanMove = false;
            if (!isPlayerInAir) rb.velocity = Vector3.zero;
        }
        if (previousPosition != transform.position) isMoving = true;
        else isMoving = false;
        previousPosition = transform.position;
    }
    private void FixedUpdate()
    {
        animator.SetBool("isJumpPreparing", jumpIsPressed);
        animator.SetBool("isPlayerInAir", isPlayerInAir);
        animator.SetBool("isHorizontalSpeedZero", rb.velocity.x != 0);
        animator.SetFloat("verticalSpeed", rb.velocity.y);
        animator.SetFloat("horizontalSpeed",rb.velocity.x);
        isPlayerInAir = !Physics2D.IsTouchingLayers(feet, groundLayer);
        if (CanMove)
        {
            if (shouldDash)
            {
                dashStarted = Time.time;
                velocityBeforeDash = rb.velocity;
                shouldDash = false;
                dashing = true;
            }
            else if (dashing)
            {
                if (isPlayerFacingLeft) rb.velocity = new Vector2(-dashVelocity, 0);
                else rb.velocity = new Vector2(+dashVelocity, 0);
                if ((Time.time - dashStarted) >= dashTimeLength)
                {
                    dashing = false;
                    rb.velocity = new Vector2(velocityBeforeDash.x, 0);
                }
            }
            else if (!isPlayerInAir)
            {
                if (!jumpIsPressed)
                {
                    if (shouldWalkR)
                    {
                        isPlayerFacingLeft = false;
                        transform.localScale = new Vector2(+0.19f, 0.19f);
                        rb.velocity = new Vector2(+movementSpeed, rb.velocity.y);
                    }
                    else if (shouldWalkL)
                    {
                        isPlayerFacingLeft = true;
                        transform.localScale = new Vector2(-0.19f, 0.19f);
                        rb.velocity = new Vector2(-movementSpeed, rb.velocity.y);
                    }
                    else
                    {
                        rb.velocity = new Vector2(0, rb.velocity.y);
                    }
                }
                else
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
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
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!console.ShowConsole)
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
                if (!isPlaying)
                {
                    jumpSound.Play();
                    isPlaying = true;
                }
            }
        }
    }
    public void WalkL(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            shouldWalkL = true;
            shouldWalkR = false;
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
        }
        if (context.canceled)
        {
            shouldWalkR = false;
        }
    }
    public void Dash(InputAction.CallbackContext context)
    {
        if (!console.ShowConsole)
        {
            if (context.performed)
            {
                if (abilityToDash && ((Time.time - timeOfLastDash) >= dashCooldown))
                {
                    timeOfLastDash = Time.time;
                    //dashSound.Play();
                    shouldDash = true;
                }
            }
        }
    }
}