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


    static ChessWindow chessWindow = null;

    [ConsoleCommand("chess", "chess (1-20)", false)]
    public string Chess(int difficulty)
    {
        if(chessWindow != null) {
            return "Only one chess game can be run at a time.";
        }

        //limit difficulty to 1-20
        if (difficulty < 1)
        {
            difficulty = 1;
        }
        else if (difficulty > 20)
        {
            difficulty = 20;
        }
        GameObject chess = new GameObject("Chess");
        chessWindow = chess.AddComponent<ChessWindow>();
        chessWindow.Initilize(difficulty);
        return "Succesfuly started chess.";
        //chessWindow.Initilize();
    }

    [ConsoleCommand("chess_close", "chess fen (fen)", false)]
    public string ChessClose()
    {
        if(chessWindow == null)
        {
            return "No chess game running";
        }
        chessWindow.Close();
        chessWindow = null;
        return "Succesfuly closed chess.";
    }

    [ConsoleCommand("chess_fen_get", "chess_fen_get", false)]
    public string ChessFENGet()
    {
        if(chessWindow == null)
        {
            return "No chess game running";
        }

        return "FEN: "+chessWindow.GetFEN();
    }

    [ConsoleCommand("chess_fen_set", "chess_fen_set (fen)", false)]
    public string ChessFENSet(string fen)
    {
        if(chessWindow == null)
        {
            return "No chess game running";
        }
        chessWindow.SetFEN(fen);
        return "FEN set to: "+fen;
    }

    [ConsoleCommand("chess_square_size", "chess_square_size (size)", false)]
    public string SetSquareSize(float size)
    {
        if(chessWindow == null)
        {
            return "No chess game running";
        }
        chessWindow.board.squareSize = size;
        return "Sqaure size set to: "+size;
    }

}
