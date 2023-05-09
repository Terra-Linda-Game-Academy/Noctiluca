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
            for (int i = 0; i < ConsoleController.commandList.Count; i++)
            {

                ConsoleCommandHolder command = ConsoleController.commandList[i] as ConsoleCommandHolder;

                string line = $"{command.commandFormat} - {command.commandDescription}";

                Debug.Log(line);

                output += line + "\n";

            }
        }
        else
        {

            for (int i = 0; i < ConsoleController.commandList.Count; i++)
            {

                ConsoleCommandHolder command = ConsoleController.commandList[i] as ConsoleCommandHolder;

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
        ConsoleController.CheatsEnabled = input;
        return "cheats " + (input? "enabled":"disabled");
    }


    [ConsoleCommand("disable", "disables selected gameobject", true)]
    public void DestroyGameobject(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }


    [ConsoleCommand("chess", "chess", false)]
    public void Chess()
    {
        GameObject chess = new GameObject("Chess");
        ChessWindow chessWindow = chess.AddComponent<ChessWindow>();
        chessWindow.Initilize();
    }
    

}
