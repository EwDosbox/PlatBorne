using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MouseVisibilty : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public bool turnOnMenu = false;
    public bool startInLockedState = false;
    public bool startInInvisibleState = false;
    private void Start()
    {
        if (startInLockedState) Cursor.lockState = CursorLockMode.Locked;
        if (startInInvisibleState) Cursor.visible = false;
    }

    private void Update()
    {
        if (turnOnMenu)
        {
            if (pauseMenu.activeInHierarchy == false && settingsMenu.activeInHierarchy == false)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}
