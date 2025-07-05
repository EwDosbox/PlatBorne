using JetBrains.Annotations;
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
        get { return showConsole; } // read-only
    }

    // Commands
    public static DebugCommand<bool> ROSEBUD;
    public static DebugCommand HELP;
    public static DebugCommand<string> KILL_BOSS;
    public static DebugCommand PLAYER_HEAL;
    public static DebugCommand<bool> PUSSYMODE;
    public static DebugCommand<string> TELEPORT;
    public static DebugCommand RESET_PREFS;
    public static DebugCommand<bool> DASH;

    // Cached references
    PlayerHealth playerHealth;
    PlayerInputScript playerInputScript;
    Saves save;

    // Other
    string feedbackMessage = "";
    private Coroutine clearFeedbackCoroutine;
    public List<object> commandListLevel;
    public List<object> commandListBoss;
    public float consoleLevelHeight = 95;
    public float consoleBossHeight = 170;
    public float y;

    Vector2 scroll;

    private void Awake()
    {
        playerInputScript = FindObjectOfType<PlayerInputScript>();
        playerHealth = FindObjectOfType<PlayerHealth>();
        save = FindObjectOfType<Saves>();

        ROSEBUD = new DebugCommand<bool>("rosebud", "Enables GodMode", "rosebud <true/false>", (x) =>
        {
            if (playerHealth == null) playerHealth = FindObjectOfType<PlayerHealth>();
            if (playerHealth != null)
                playerHealth.GodMode = x;

            if (x)
                PlayerPrefs.SetInt("GodMode", 69);
            else
                PlayerPrefs.DeleteKey("GodMode");

            PlayerPrefs.Save();
            CommandCorrectAnswer();
        });

        HELP = new DebugCommand("help", "Shows all commands", "help", () =>
        {
            showHelp = true;
        });

        KILL_BOSS = new DebugCommand<string>("kill_boss", "Kills boss", "kill_boss <bossname>", (x) =>
        {
            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                Bossfight bossfight = FindObjectOfType<Bossfight>();
                if (bossfight != null)
                {
                    StartCoroutine(bossfight.BossDeath());
                    CommandCorrectAnswer();
                }
                else feedbackMessage = "Bossfight not found.";
            }
            else if (SceneManager.GetActiveScene().buildIndex == 7)
            {
                Mole_Bossfight moleBossfight = FindObjectOfType<Mole_Bossfight>();
                if (moleBossfight != null)
                {
                    StartCoroutine(moleBossfight.BossDeath());
                    CommandCorrectAnswer();
                }
                else feedbackMessage = "Mole bossfight not found.";
            }
            else feedbackMessage = "No boss fight in this scene.";
        });

        PLAYER_HEAL = new DebugCommand("player_heal", "Heals Hunter to full HP", "player_heal", () =>
        {
            if (playerHealth == null) playerHealth = FindObjectOfType<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.PlayerHP = 3;
                CommandCorrectAnswer();
            }
            else feedbackMessage = "PlayerHealth not found.";
        });

        PUSSYMODE = new DebugCommand<bool>("pussymode", "Enables/Disables pussy mode", "pussymode <true/false>", (x) =>
        {
            PlayerPrefs.SetInt("PussyMode", x ? 1 : 0);
            PlayerPrefs.Save();

            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                Bossfight bossfight = FindObjectOfType<Bossfight>();
                if (bossfight != null)
                {
                    bossfight.SetPussyMode(x);
                    CommandCorrectAnswer();
                }
                else feedbackMessage = "Bossfight not found.";
            }
            else if (SceneManager.GetActiveScene().buildIndex == 7)
            {
                Mole_Bossfight moleBossfight = FindObjectOfType<Mole_Bossfight>();
                if (moleBossfight != null)
                {
                    moleBossfight.SetPussyMode(x);
                    CommandCorrectAnswer();
                }
                else feedbackMessage = "Mole bossfight not found.";
            }
            else feedbackMessage = "No boss fight in this scene.";
        });

        TELEPORT = new DebugCommand<string>("teleport", "Teleports Player", "teleport <london/brecus/birmingham/lunae>", (x) =>
        {
            if (x == "london") SceneManager.LoadScene("LevelLondon");
            else if (x == "brecus") SceneManager.LoadScene("LevelBoss");
            else if (x == "birmingham") SceneManager.LoadScene("LevelBirmingham");
            else if (x == "lunae") SceneManager.LoadScene("LevelMole");
            else feedbackMessage = $"Unknown teleport location '{x}'.";
        });

        RESET_PREFS = new DebugCommand("reset_prefs", "Resets all PlayerPrefs", "reset_prefs", () =>
        {
            if (save != null)
            {
                save.NewGameSaveReset();
                CommandCorrectAnswer();
            }
            else feedbackMessage = "Save system not found.";
        });

        DASH = new DebugCommand<bool>("dash", "Enables/Disables Dash Ability", "dash <true/false>", (x) =>
        {
            if (playerInputScript == null) playerInputScript = FindObjectOfType<PlayerInputScript>();
            if (playerInputScript != null)
            {
                playerInputScript.AbilityToDash(x);
                CommandCorrectAnswer();
            }
            else feedbackMessage = "PlayerInputScript not found.";
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

    public void OnToggleConsole(InputValue value)
    {
        showConsole = !showConsole;
        input = "";
        if (playerInputScript != null)
            PlayerInputScript.CanMove = !PlayerInputScript.CanMove;
    }

    private void OnGUI()
    {
        if (!showConsole) return;
        y = 0;

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
                y += consoleLevelHeight;
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
                y += consoleBossHeight;
            }
        }

        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        GUI.SetNextControlName("console");
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);
        GUI.FocusControl("console");
        GUI.Label(new Rect(10f, y + 30f, Screen.width - 20f, 20f), feedbackMessage);
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

        if (properties.Length == 0 || string.IsNullOrWhiteSpace(properties[0]))
            return;

        string inputCommand = properties[0];

        bool isLevelScene = SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 12;
        List<object> commandList = isLevelScene ? commandListLevel : commandListBoss;

        foreach (var cmdObj in commandList)
        {
            DebugCommandBase commandBase = cmdObj as DebugCommandBase;
            if (commandBase == null) continue;

            if (commandBase.CommandID == inputCommand)
            {
                if (cmdObj is DebugCommand cmdNoArg)
                {
                    cmdNoArg.Invoke();
                    CommandCorrectAnswer();
                    return;
                }
                else if (cmdObj is DebugCommand<string> cmdString)
                {
                    if (properties.Length > 1)
                    {
                        cmdString.Invoke(properties[1]);
                        CommandCorrectAnswer();
                    }
                    else
                    {
                        feedbackMessage = $"Command '{inputCommand}' requires an argument.";
                    }
                    return;
                }
                else if (cmdObj is DebugCommand<bool> cmdBool)
                {
                    if (properties.Length > 1)
                    {
                        if (bool.TryParse(properties[1], out bool boolArg))
                        {
                            cmdBool.Invoke(boolArg);
                            CommandCorrectAnswer();
                        }
                        else feedbackMessage = $"Invalid argument for '{inputCommand}': expected true/false.";
                    }
                    else
                    {
                        feedbackMessage = $"Command '{inputCommand}' requires an argument.";
                    }
                    return;
                }
            }
        }

        feedbackMessage = $"Unknown command: {inputCommand}";
    }

    public void CommandCorrectAnswer()
    {
        if (clearFeedbackCoroutine != null)
            StopCoroutine(clearFeedbackCoroutine);

        feedbackMessage = "Command Done";
        clearFeedbackCoroutine = StartCoroutine(ClearFeedbackMessageAfterDelay());
    }

    private IEnumerator ClearFeedbackMessageAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        feedbackMessage = "";
        clearFeedbackCoroutine = null;
    }
}
