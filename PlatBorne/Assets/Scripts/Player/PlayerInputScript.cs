using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;

public class HunterInputScript : MonoBehaviour
{
    private HunterInput input;
    private Rigidbody2D rb;

    private float jumpHeight;
    [SerializeField] private float maxJumpHeight;
    [SerializeField] private float minJumpHeight;
    [SerializeField] private float jumpModifier;
    private float jumpTime;

    public bool isPlayerInAir;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Collider2D feet;

    [SerializeField] private float movementSpeed;

    private void Awake()
    {
        input = new HunterInput();
        rb = GetComponent<Rigidbody2D>();
        jumpHeight = minJumpHeight;
    }

    private void FixedUpdate()
    {
        isPlayerInAir = Physics2D.IsTouchingLayers(feet, groundLayer);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(isPlayerInAir)
        {
            if (context.started)
            {
                jumpTime = Time.time;//when i pressed
            }
            if(context.canceled) 
            {// behaves like a weird stopwatch
                jumpHeight = minJumpHeight + (Time.time - jumpTime) * jumpModifier;
                if(jumpHeight >= maxJumpHeight) jumpHeight = maxJumpHeight;
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            }
            //hunterWalk.enabled = false;
        }
    }
    public void Walk(InputAction.CallbackContext context)
    {
        if(isPlayerInAir)
        {
            if (context.started)
            {
                rb.velocity = new Vector2(context.ReadValue<Vector2>().x * movementSpeed, rb.velocity.y);
            }
            if (context.canceled)
            {
                rb.velocity = Vector2.zero;
            }
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