using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultCommands
{
    
    [ConsoleCommand("help", "gets list of all commands")]
    public static string Help(string input)
    {
        Debug.Log("Input: "+ input);
        string output = "";
        if(input == "" || input == null) {
            for (int i = 0; i < DebugController.commandList.Count; i++)
            {

                DebugCommandBase command = DebugController.commandList[i] as DebugCommandBase;

                string line = $"{command.commandFormat} - {command.commandDescription}";

                Debug.Log(line);

                output += line + "\n";

            }
        } else {
       
            for (int i = 0; i < DebugController.commandList.Count; i++)
            {

                DebugCommandBase command = DebugController.commandList[i] as DebugCommandBase;

                if(command.commandId==input) {
                    string line = $"{command.commandFormat} - {command.commandDescription}";
                    output += line;
                    break;
                }

            }
        }
        
        return output;
    }

    
}
