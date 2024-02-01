using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugController : MonoBehaviour
{
    bool showConsole;
    bool showHelp = false;
    string input;
    //commands {
    public static DebugCommand ROSEBUD;
    public static DebugCommand HELP;
    //commands }
    public List<object> commandList;
    public PlayerHealth playerHealth;
    public void OnToggleConsole(InputValue value)
    {
        showConsole = !showConsole;
    }
    Vector2 scroll;
    private void OnGUI()
    {
        if (!showConsole) { return; }
        float y = 0;
        if (showHelp)
        {
            GUI.Box(new Rect(0, y, Screen.width, 100), "");
            Rect viewport = new Rect(0, 0, Screen.width - 30, 20 * commandList.Count);
            scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 90), scroll, viewport);
            for (int i = 0; i < commandList.Count; i++)
            {
                DebugCommandBase command = commandList[i] as DebugCommandBase;
                string label = $"{command.CommandFormat} - {command.CommandDescription}";
                Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);
                GUI.Label(labelRect, label);
            }
            GUI.EndScrollView();
            {
                y += 100;
            }
        }
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

        HELP = new DebugCommand("help", "Shows all commands", "help", () =>
        {
            showHelp = true;
        });

        commandList = new List<object>
        {
            ROSEBUD,
            HELP
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
