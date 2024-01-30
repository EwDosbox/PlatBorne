using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugController : MonoBehaviour
{
    bool showConsole;
    string input;
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
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20, 20f), input);
    }
}
