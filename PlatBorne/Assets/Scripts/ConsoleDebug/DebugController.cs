using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DebugController : MonoBehaviour
{
    bool showConsole;
    bool showHelp = false;
    string input;
    //commands {
    public static DebugCommand ROSEBUD;
    public static DebugCommand HELP;
    public static DebugCommand KILL_BRECUS;
    public static DebugCommand KILL_MOLE;
    public static DebugCommand PLAYER_HEAL;
    public static DebugCommand PUSSYMODE_TRUE;
    public static DebugCommand PUSSYMODE_FALSE;
    public static DebugCommand TELEPORT_LONDON;
    public static DebugCommand TELEPORT_CHURCH;
    public static DebugCommand TELEPORT_BIRMINGHAM;
    public static DebugCommand RESET_PREFS;
    //commands }
    public List<object> commandList;
    public PlayerHealth playerHealth;
    public Bossfight bossfight;
    public Saves save;
    public void OnToggleConsole(InputValue value)
    {
        showConsole = !showConsole;
        PlayerInputScript.CanMove = !PlayerInputScript.CanMove;
    }
    Vector2 scroll;
    private void OnGUI()
    {
        if (!showConsole) { return; }
        float y = 0;
        if (showHelp)
        {
            GUI.Box(new Rect(0, y, Screen.width, 250), "");
            Rect viewport = new Rect(0, 0, Screen.width - 30, 20 * commandList.Count);
            scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 240), scroll, viewport);
            for (int i = 0; i < commandList.Count; i++)
            {
                DebugCommandBase command = commandList[i] as DebugCommandBase;
                string label = $"{command.CommandFormat} - {command.CommandDescription}";
                Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);
                GUI.Label(labelRect, label);
            }
            GUI.EndScrollView();
            {
                y += 250;
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
            playerHealth.GodMode = true;
        });

        HELP = new DebugCommand("help", "Shows all commands", "help", () =>
        {
            showHelp = true;
        });

        KILL_BRECUS = new DebugCommand("kill_brecus", "Kills boss Brecus", "kill_brecus", () =>
        {
            StartCoroutine(bossfight.BossDeath());
        });

        KILL_MOLE = new DebugCommand("kill_mole", "Kills boss Mole", "kill_mole", () =>
        {
            Debug.Log("Mole Died");
        });

        PLAYER_HEAL = new DebugCommand("player_heal", "Heals Hunter to full HP", "player_heal", () =>
        {
            playerHealth.PlayerHP = 3; 
        });

        PUSSYMODE_TRUE = new DebugCommand("pussymode_true", "Enables Pussy Mode (for bosses only)", "pussymode_true", () =>
        {
            PlayerPrefs.SetInt("PussyMode", 1);
        });

        PUSSYMODE_FALSE = new DebugCommand("pussymode_false", "Disables Pussy Mode (for bosses only)", "pussymode_false", () =>
        {
            PlayerPrefs.DeleteKey("PussyMode");
        });

        TELEPORT_LONDON = new DebugCommand("teleport_london", "Teleports Player to London", "teleport_london", () =>
        {
            SceneManager.LoadScene("LevelLondon");
        });

        TELEPORT_CHURCH = new DebugCommand("teleport_church", "Teleports Player to Church (Bricus)", "teleport_church", () =>
        {
            SceneManager.LoadScene("LevelBoss");
        });

        TELEPORT_BIRMINGHAM = new DebugCommand("teleport_birmingham", "Teleports Player to Birmingham", "teleport_birmingham", () =>
        {
            Debug.Log("Work In Progress");
        });

        RESET_PREFS  = new DebugCommand("reset_prefs", "Resets all PlayerPrefs", "reset_prefs", () =>
        {
            save.NewGameSaveReset();
        });

        commandList = new List<object>
        {
            ROSEBUD,
            HELP, //Bigger UI, cannot move
            KILL_BRECUS, 
            KILL_MOLE, 
            PLAYER_HEAL, 
            PUSSYMODE_TRUE, //UI CHANGE
            PUSSYMODE_FALSE, //UI CHANGE
            TELEPORT_LONDON, 
            TELEPORT_CHURCH,
            TELEPORT_BIRMINGHAM, 
            RESET_PREFS
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
