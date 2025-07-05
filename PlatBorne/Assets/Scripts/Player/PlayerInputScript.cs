using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInputScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Collider2D feet;
    Saves save;
    bool bossMovement = false;

    static public bool CanMove { get; set; }

    private float jumpHeight;
    [SerializeField] private float maxJumpHeight;
    [SerializeField] private float minJumpHeight;
    [SerializeField] private float jumpModifier;
    private float jumpTime;

    private bool isPlayerFacingLeft;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float movementSpeedBoss;
    static public bool isPlayerInAir;
    private bool shouldWalkL = false;
    private bool shouldWalkR = false;
    private bool shouldJump;
    private bool wantsToJump = false;
    private bool jumpIsPressed;
    private bool hasJumped;
    public AudioSource jumpSound;
    private bool isPlaying;
    private float time;
    DebugController console;
    float dashTimeWait = 1;
    float dashTime = 1;

    // Dash
    [SerializeField] public bool abilityToDash;
    [SerializeField] AudioSource dashSFX;
    bool groundDashReset = true;
    private Vector2 velocityBeforeDash;
    private bool canDash = false;
    private bool dashing;
    private float dashStarted;
    private bool isMoving = false;
    [SerializeField] AudioSource walkSound;
    [SerializeField] private float dashTimeLength;
    [SerializeField] private float dashVelocity;
    private Vector3 previousPosition;

    private void Awake()
    {
        save = FindFirstObjectByType<Saves>();
        console = FindFirstObjectByType<DebugController>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (SceneManager.GetActiveScene().name == "LevelBoss" || SceneManager.GetActiveScene().name == "LevelMole")
        {
            maxJumpHeight *= 1.3f;
            jumpModifier *= 1.3f;
            movementSpeed = movementSpeedBoss;
        }

        jumpHeight = minJumpHeight;
        CanMove = true;
        previousPosition = transform.position;
    }

    public void AbilityToDash(bool dash)
    {
        abilityToDash = dash;
    }

    private void Update()
    {
        walkSound.enabled = (!isPlayerInAir && isMoving);

        if (console.ShowConsole)
        {
            CanMove = false;
            if (!isPlayerInAir) rb.velocity = Vector3.zero;
        }

        isMoving = previousPosition != transform.position;
        previousPosition = transform.position;
    }

    private void FixedUpdate()
    {
        dashTime += Time.deltaTime;

        animator.SetBool("isJumpPreparing", jumpIsPressed);
        animator.SetBool("isPlayerInAir", isPlayerInAir);
        animator.SetBool("isHorizontalSpeedZero", rb.velocity.x != 0);
        animator.SetFloat("verticalSpeed", rb.velocity.y);
        animator.SetFloat("horizontalSpeed", rb.velocity.x);

        isPlayerInAir = !Physics2D.IsTouchingLayers(feet, groundLayer);
        if (!isPlayerInAir)
        {
            hasJumped = false; // Reset jump lockout on landing
        }

        if (CanMove)
        {
            if (canDash)
            {
                dashStarted = Time.time;
                velocityBeforeDash = rb.velocity;
                canDash = false;
                dashing = true;
            }
            else if (dashing)
            {
                rb.velocity = new Vector2(isPlayerFacingLeft ? -dashVelocity : dashVelocity, 0);
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
                {
                    Debug.Log("jump: " + (Time.time - jumpTime));
                    jumpHeight = minJumpHeight + (Time.time - jumpTime) * jumpModifier;
                    if (jumpHeight >= maxJumpHeight) jumpHeight = maxJumpHeight;
                    rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
                    shouldJump = false;
                }
            }

            if (isPlaying)
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
        if (console.ShowConsole) return;

        if (context.started && !isPlayerInAir && !hasJumped)
        {
            jumpTime = Time.time;
            wantsToJump = true;
            jumpIsPressed = true;
        }

        if (context.canceled && !isPlayerInAir && !hasJumped && wantsToJump)
        {
            wantsToJump = false;
            shouldJump = true;
            jumpIsPressed = false;
            hasJumped = true;
            save.PlayerJumped();

            if (!isPlaying)
            {
                jumpSound.Play();
                isPlaying = true;
            }
        }
    }

    public void WalkL(InputAction.CallbackContext context)
    {
        if (context.started) { shouldWalkL = true; shouldWalkR = false; }
        if (context.canceled) { shouldWalkL = false; }
    }

    public void WalkR(InputAction.CallbackContext context)
    {
        if (context.started) { shouldWalkR = true; shouldWalkL = false; }
        if (context.canceled) { shouldWalkR = false; }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (!console.ShowConsole && context.performed)
        {
            if (abilityToDash && groundDashReset && dashTime > dashTimeWait)
            {
                dashTime = 0;
                groundDashReset = false;
                dashSFX.Play();
                canDash = true;
            }
        }
    }

    public void GroundDashReset()
    {
        groundDashReset = true;
    }
}
