using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugController : MonoBehaviour
{
    bool showConsole;
    string input;
    //commands {
    public static DebugCommand ROSEBUD;
    //commands }
    public List<object> commandList;
    public PlayerHealth playerHealth;
    public void OnToggleConsole(InputValue value)
    {
        showConsole = !showConsole;
    }

    private void OnGUI()
    {
        if (!showConsole) { return; }
        float y = 0;
        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        GUI.SetNextControlName("console");
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);
        GUI.FocusControl("console");
    }

    private void Awake()
    {
        ROSEBUD = new DebugCommand("rosebud", "Enables GodMode", "rosebud", () =>
        {
            playerHealth.SetGodMode(true);
        });

        commandList = new List<object>
        {
            ROSEBUD
        };
    }

    public void OnSubmit(InputValue value)
    {
        if (showConsole)
        {
            HandleInput();
            input = "";
        }
    }

    private void HandleInput()
    {
        for (int i = 0; i < commandList.Count; i++)
        {
            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;
            if (input.Contains(commandBase.CommandID))
            {
                if (commandList[i] as DebugCommand != null)
                {
                    (commandList[i] as DebugCommand).Invoke();
                }
            }            
        }
    }
}
