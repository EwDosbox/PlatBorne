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
    public static DebugCommand<bool> ROSEBUD;
    public static DebugCommand HELP;
    public static DebugCommand<string> KILL_BOSS;
    public static DebugCommand PLAYER_HEAL;
    public static DebugCommand<bool> PUSSYMODE;
    public static DebugCommand<string> TELEPORT;
    public static DebugCommand RESET_PREFS;
    //commands }
    public List<object> commandList;
    public PlayerHealth playerHealth;
    public Bossfight bossfight;
    public Saves save;
    public Mole_Bossfight moleBossfight;
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
            GUI.Box(new Rect(0, y, Screen.width, 150), "");
            Rect viewport = new Rect(0, 0, Screen.width - 30, 20 * commandList.Count);
            scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 140), scroll, viewport);
            for (int i = 0; i < commandList.Count; i++)
            {
                DebugCommandBase command = commandList[i] as DebugCommandBase;
                string label = $"{command.CommandFormat} - {command.CommandDescription}";
                Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);
                GUI.Label(labelRect, label);
            }
            GUI.EndScrollView();
            {
                y += 150;
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
            }
            else
            {
                if (playerHealth != null) playerHealth.GodMode = false;
                PlayerPrefs.DeleteKey("GodMode");
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
                if (playerHealth != null) playerHealth.PussyMode = true;
            }
            else
            {
                PlayerPrefs.DeleteKey("PussyMode");
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

        commandList = new List<object>
        {
            ROSEBUD,
            HELP,
            KILL_BOSS,
            PLAYER_HEAL, 
            PUSSYMODE,
            TELEPORT,
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
        string[] properties = input.Split(' ');
        for (int i = 0; i < commandList.Count; i++)
        {
            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;
            if (input.Contains(commandBase.CommandID))
            {
                if (commandList[i] as DebugCommand != null)
                {
                    (commandList[i] as DebugCommand).Invoke();
                }
                else if (commandList[i] as DebugCommand<string> != null)
                {
                    (commandList[i] as DebugCommand<string>).Invoke(properties[1]);
                }
                else if (commandList[i] as DebugCommand<bool> != null)
                {
                    if (properties[1] == "true") (commandList[i] as DebugCommand<bool>).Invoke(true);
                    else if (properties[1] == "false") (commandList[i] as DebugCommand<bool>).Invoke(false);
                }
            }            
        }
    }
}
