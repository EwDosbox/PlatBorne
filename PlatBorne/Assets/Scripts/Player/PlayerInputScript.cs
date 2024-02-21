using System.Threading;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;

public class PlayerInputScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    public Saves save;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Collider2D feet;

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

    //Dash
    [SerializeField] public bool abilityToDash;
    private bool canDash;
    private float timeOfLastDash;
    [SerializeField] public float dashCooldown;
    private Vector2 velocityBeforeDash;
    private bool shouldDash;
    private bool dashing;
    private float dashStarted;
    [SerializeField] private float dashTimeLength;
    [SerializeField] private float dashVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        jumpHeight = minJumpHeight;
        timeOfLastDash = 0;
        CanMove = true;
    }
    private void FixedUpdate()
    {
        animator.SetBool("isJumpPreparing", jumpIsPressed);
        animator.SetInteger("verticalSpeed", math.asint(rb.velocity.y));
        animator.SetInteger("horizontalSpeed",math.asint(rb.velocity.x));
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
                        rb.velocity = new Vector2(+movementSpeed, rb.velocity.y);
                        walkSound.Play();
                    }
                    else if (shouldWalkL)
                    {
                        isPlayerFacingLeft = true;
                        rb.velocity = new Vector2(-movementSpeed, rb.velocity.y);
                        walkSound.Play();
                    }
                    else
                    {
                        rb.velocity = new Vector2(0, rb.velocity.y);
                        walkSound.Stop();
                    }
                }
                else rb.velocity = new Vector2(0, rb.velocity.y);
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
        if (context.started)
        {
            jumpTime = Time.time;//when i pressed
            jumpIsPressed = true;
        }
        if (context.canceled)
        {
            shouldJump = true;
            jumpIsPressed = false;
            //save.PlayerJumped();
            //not created an instance (save = new Save()) so it throws an error
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
            if (abilityToDash && ((Time.time - timeOfLastDash) >= dashCooldown))
            {
                timeOfLastDash = Time.time;
                dashSound.Play();
                shouldDash = true;
            }
        }
    }
}