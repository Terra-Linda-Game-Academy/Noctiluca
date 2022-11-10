using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class DefaultCommands
{

    [ConsoleCommand("help", "gets list of all commands")]
    public static string Help(string input)
    {
        Debug.Log("Input: " + input);
        string output = "";
        if (input == "" || input == null)
        {
            for (int i = 0; i < DebugController.commandList.Count; i++)
            {

                DebugCommandBase command = DebugController.commandList[i] as DebugCommandBase;

                string line = $"{command.commandFormat} - {command.commandDescription}";

                Debug.Log(line);

                output += line + "\n";

            }
        }
        else
        {

            for (int i = 0; i < DebugController.commandList.Count; i++)
            {

                DebugCommandBase command = DebugController.commandList[i] as DebugCommandBase;

                if (command.commandId == input)
                {
                    string line = $"{command.commandFormat} - {command.commandDescription}";
                    output += line;
                    break;
                }

            }
        }

        return output;
    }

    [ConsoleCommand("cheats", "enables/disables cheat commands")]
    public static string Cheats(bool input)
    {
        DebugController.CheatsEnabled = input;
        return "cheats " + (input? "enabled":"disabled");
    }

    // [ConsoleCommand("parameterhelp", "gets specifics for parameter types")]
    // public static string Parameterhelp(string input)
    // {
    //     Type parameterType = Type.GetType(input);
    //     string format = DebugController.GetFormatOfParameter(parameterType);
    //     return format;
    // }

    [ConsoleCommand("disable", "disables selected gameobject")]
    public void DestroyGameobject(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
}