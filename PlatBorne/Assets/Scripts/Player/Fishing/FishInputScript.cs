using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class FishInputScript : MonoBehaviour
{
    [SerializeField] private PlayerFishingScript playerFishingScript;

    public void HookL(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerFishingScript.HoldTime = Time.time;
            playerFishingScript.ShouldHookMoveLeft = true;
            playerFishingScript.ShouldHookMoveRight = false;
        }
        else if (context.canceled)
        {
            playerFishingScript.HoldTime = 0;
            playerFishingScript.ShouldHookMoveLeft = false;
            playerFishingScript.ShouldHookMoveRight = false;
        }
    }
    public void HookR(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerFishingScript.HoldTime = Time.time;
            playerFishingScript.ShouldHookMoveLeft = false;
            playerFishingScript.ShouldHookMoveRight = true;
        }
        else if (context.canceled)
        {
            playerFishingScript.HoldTime = 0;
            playerFishingScript.ShouldHookMoveLeft = false;
            playerFishingScript.ShouldHookMoveRight = false;
        }
    }
    public void Back(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PlayerPrefs.SetInt("wasFishing", 1);
            PlayerPrefs.Save();
            SceneManager.LoadScene("LevelBirmingham");
        }
    }
}
