using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultCommands
{
    
    [ConsoleCommand("help", "gets list of all commands")]
    public string Help(string input)
    {
        string output = "";
        for (int i = 0; i < DebugController.commandList.Count; i++)
        {

            DebugCommandBase command = DebugController.commandList[i] as DebugCommandBase;

            string line = $"{command.commandFormat} - {command.commandDescription}";

            output += line + "\n";

        }
        output += "---";
        return output;
    }

    
}
