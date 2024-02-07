using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterInputScript : MonoBehaviour
{
    public void Jump()
    {
        //hunterWalk.enabled = false;
        Debug.Log("Jump");
    }
    public void Walk()
    {
        //hunterWalk.enabled = true;
        Debug.Log("Walk");
    }
    public void Dash()
    {
        Debug.Log("Dash");
    }
}
/*
if (Input.GetKey(KeyCode.W))
{
    rb.velocity = new Vector2(0, 0);
    jumpHeight += Time.deltaTime * jumpModifier;
    if (jumpHeight > maxJumpHeight)
    {
        jumpHeight = maxJumpHeight;
    }
}
if (Input.GetKeyUp(KeyCode.W))
{
    rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
    jumpHeight = minJumpHeight;
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