using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class DebugCommandBase
{
    private string commandID;
    private string commandDescription;
    private string commandFormat;

    public string CommandID { get { return commandID; } }
    public string CommandDescription { get {  return commandDescription; } }
    public string CommandFormat { get {  return commandFormat; } }
    public DebugCommandBase(string id, string description, string format)
    {
        this.commandID = id;
        this.commandDescription = description;
        this.commandFormat = format;
    }
}

public class DebugCommand : DebugCommandBase
{
    private Action command;
    public DebugCommand(string id, string description, string format, Action command) : base(id, description, format)
    {
        this.command = command;
    }

    public void Invoke()
    {
        command.Invoke();
    }
}
