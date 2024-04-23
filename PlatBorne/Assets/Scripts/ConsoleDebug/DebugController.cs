using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DebugController : MonoBehaviour
{
    bool showConsole;
    bool showHelp = false;
    string input;
    public bool ShowConsole
    {
        get { return showConsole; } //only for reading
    }
    //commands {
    public static DebugCommand<bool> ROSEBUD;
    public static DebugCommand HELP;
    public static DebugCommand<string> KILL_BOSS;
    public static DebugCommand PLAYER_HEAL;
    public static DebugCommand<bool> PUSSYMODE;
    public static DebugCommand<string> TELEPORT;
    public static DebugCommand RESET_PREFS;
    public static DebugCommand<bool> DASH; 
    //commands }
    public List<object> commandListLevel;
    public List<object> commandListBoss;
    public PlayerHealth playerHealth;
    public PlayerInputScript playerInputScript;
    public Bossfight bossfight;
    public Saves save;
    public Mole_Bossfight moleBossfight;
    public float consoleLevelHeight = 95;
    public float consoleBossHeight = 170;
    public void OnToggleConsole(InputValue value)
    {
        showConsole = !showConsole;
        input = "";
        PlayerInputScript.CanMove = !PlayerInputScript.CanMove;
    }
    Vector2 scroll;
    private void OnGUI()
    {
        if (!showConsole) { return; }
        float y = 0;
        if (showHelp)
        {
            if (SceneManager.GetActiveScene().name == "LevelLondon" || SceneManager.GetActiveScene().name == "LevelBirmingham")
            {
                GUI.Box(new Rect(0, y, Screen.width, consoleLevelHeight), "");
                Rect viewport = new Rect(0, 0, Screen.width - 30, 20 * commandListLevel.Count);
                scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 80), scroll, viewport);
                for (int i = 0; i < commandListLevel.Count; i++)
                {
                    DebugCommandBase command = commandListLevel[i] as DebugCommandBase;
                    string label = $"{command.CommandFormat} - {command.CommandDescription}";
                    Rect labelRect = new Rect(5, 20 * i, viewport.width - 80, 20);
                    GUI.Label(labelRect, label);
                }
                GUI.EndScrollView();
                {
                    y += consoleLevelHeight;
                }
            }
            else
            {
                GUI.Box(new Rect(0, y, Screen.width, consoleBossHeight), "");
                Rect viewport = new Rect(0, 0, Screen.width - 30, 20 * commandListBoss.Count);
                scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 240), scroll, viewport);
                for (int i = 0; i < commandListBoss.Count; i++)
                {
                    DebugCommandBase command = commandListBoss[i] as DebugCommandBase;
                    string label = $"{command.CommandFormat} - {command.CommandDescription}";
                    Rect labelRect = new Rect(5, 20 * i, viewport.width - 240, 20);
                    GUI.Label(labelRect, label);
                }
                GUI.EndScrollView();
                {
                    y += consoleBossHeight;
                }
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
        ROSEBUD = new DebugCommand<bool>("rosebud", "Enables GodMode", "rosebud", (x) =>
        {
            if (x)
            {
                if (playerHealth != null) playerHealth.GodMode = true;
                PlayerPrefs.SetInt("GodMode", 1);
                PlayerPrefs.Save();
            }
            else
            {
                if (playerHealth != null) playerHealth.GodMode = false;
                PlayerPrefs.DeleteKey("GodMode");
                PlayerPrefs.Save();
            }
        });

        HELP = new DebugCommand("help", "Shows all commands", "help", () =>
        {
            showHelp = true;
        });

        KILL_BOSS = new DebugCommand<string>("kill_boss", "Kills boss", "kill_boss <value>", (x) =>
        {
            if (x == "brecus" && bossfight != null) StartCoroutine(bossfight.BossDeath());
            if (x == "mole") StartCoroutine(bossfight.BossDeath());
        });

        PLAYER_HEAL = new DebugCommand("player_heal", "Heals Hunter to full HP", "player_heal", () =>
        {
            if (playerHealth != null) playerHealth.PlayerHP = 3; 
        });

        PUSSYMODE = new DebugCommand<bool>("pussymode", "Enables/Disables pussy mode", "pussymode <value>", (x) =>
        {
            if (x)
            {
                PlayerPrefs.SetInt("PussyMode", 1);
                PlayerPrefs.Save();
                if (playerHealth != null) playerHealth.PussyMode = true;
            }
            else
            {
                PlayerPrefs.DeleteKey("PussyMode");
                PlayerPrefs.Save();
                if (playerHealth != null) playerHealth.PussyMode = false;
            }
        });


        TELEPORT = new DebugCommand<string>("teleport", "Teleports Player", "teleport <value>", (x) =>
        {
            if (x == "london") SceneManager.LoadScene("LevelLondon");
            else if (x == "brecus") SceneManager.LoadScene("LevelBoss");
            else if (x == "birmingham") SceneManager.LoadScene("LevelBirmingham");
            else if (x == "mole") SceneManager.LoadScene("LevelMole");
        });

        RESET_PREFS  = new DebugCommand("reset_prefs", "Resets all PlayerPrefs", "reset_prefs", () =>
        {
            if (save != null) save.NewGameSaveReset();
        });

        DASH = new DebugCommand<bool>("dash", "Enables/Disables Dash Ability", "dash <value>", (x) =>
        {
            if (x) playerInputScript.AbilityToDash(true);
            else playerInputScript.AbilityToDash(false);
        });

        commandListLevel = new List<object>
        {
            HELP,
            TELEPORT,
            DASH,
            RESET_PREFS
        };
        commandListBoss = new List<object>
        {
            HELP,
            ROSEBUD,
            KILL_BOSS,
            PLAYER_HEAL,
            DASH,
            PUSSYMODE,
            TELEPORT,
            RESET_PREFS,
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
        if (SceneManager.GetActiveScene().name == "LevelLondon" || SceneManager.GetActiveScene().name == "LevelBirmingham")
        {
            string[] properties = input.Split(' ');
            for (int i = 0; i < commandListLevel.Count; i++)
            {
                DebugCommandBase commandBase = commandListLevel[i] as DebugCommandBase;
                if (input.Contains(commandBase.CommandID))
                {
                    if (commandListLevel[i] as DebugCommand != null)
                    {
                        (commandListLevel[i] as DebugCommand).Invoke();
                    }
                    else if (commandListLevel[i] as DebugCommand<string> != null)
                    {
                        (commandListLevel[i] as DebugCommand<string>).Invoke(properties[1]);
                    }
                    else if (commandListLevel[i] as DebugCommand<bool> != null)
                    {
                        if (properties[1] == "true") (commandListLevel[i] as DebugCommand<bool>).Invoke(true);
                        else if (properties[1] == "false") (commandListLevel[i] as DebugCommand<bool>).Invoke(false);
                    }
                }
            }
        }
        else
        {
            string[] properties = input.Split(' ');
            for (int i = 0; i < commandListBoss.Count; i++)
            {
                DebugCommandBase commandBase = commandListBoss[i] as DebugCommandBase;
                if (input.Contains(commandBase.CommandID))
                {
                    if (commandListBoss[i] as DebugCommand != null)
                    {
                        (commandListBoss[i] as DebugCommand).Invoke();
                    }
                    else if (commandListBoss[i] as DebugCommand<string> != null)
                    {
                        (commandListBoss[i] as DebugCommand<string>).Invoke(properties[1]);
                    }
                    else if (commandListBoss[i] as DebugCommand<bool> != null)
                    {
                        if (properties[1] == "true") (commandListBoss[i] as DebugCommand<bool>).Invoke(true);
                        else if (properties[1] == "false") (commandListBoss[i] as DebugCommand<bool>).Invoke(false);
                    }
                }
            }
        }
    }
}
