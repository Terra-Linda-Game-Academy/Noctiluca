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


    public static ChessWindow chessWindow = null;

    [ConsoleCommand("chess", "chess (playAsWhite) (difficulty 1-20)", false)]
    public string Chess(bool playAsWhite, int difficulty)
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
        chessWindow.Initilize(playAsWhite, difficulty);
        return "Succesfuly started chess.";
        //chessWindow.Initilize();
    }

    [ConsoleCommand("chess_puzzle", "puzzle (1-20)", false)]
    public string ChessPuzzle()
    {
        if(chessWindow == null)
        {
            return "Chess must be running";
        }


        // //limit difficulty to 1-20
        // if (difficulty < 1)
        // {
        //     difficulty = 1;
        // }
        // else if (difficulty > 20)
        // {
        //     difficulty = 20;
        // }
        chessWindow.RandomPuzzle();
        return "Succesfuly started a puzzle.";
        //chessWindow.Initilize();
    }

    //chess_hint GetBestMove
    [ConsoleCommand("chess_hint", "chess_hint", false)]
    public string ChessHint()
    {
        if(chessWindow == null)
        {
            return "No chess game running";
        }
        chessWindow.GetHint((hint) => {ConsoleController.Instance.AddToConsoleLog("Best move is: " + hint);});
        return chessWindow.playingPuzzles?"":"Finding best move...";
    }

    //allpuzll moves
    [ConsoleCommand("chess_all_moves", "chess_all_moves", false)]
    public string ChessAllMoves()
    {
        if(chessWindow == null)
        {
            return "No chess game running";
        }
        return chessWindow.GetAllPuzzleMoves();
    }


    [ConsoleCommand("chess_screen_saver_mode", "chess_screen_saver_mode", false)]
    public string ChessRainbow(bool screenSaverMode)
    {
        if(chessWindow == null)
        {
            return "No chess game running";
        }
        chessWindow.screenSaverMode = screenSaverMode;
        return "Succesfuly set chess screen saver mode to "+screenSaverMode+".";
    }

    [ConsoleCommand("chess_rainbow", "chess_rainbow", false)]
    public string ChessRainbow()
    {
        if(chessWindow == null)
        {
            return "No chess game running";
        }
        chessWindow.rainbow = !chessWindow.rainbow;
        return "Succesfuly "+ (chessWindow.rainbow?"rainbowed":"de-rainbowed")+" chess.";
    }

    [ConsoleCommand("chess_color_reset", "chess_color_reset", false)]
    public string ChessColorReset()
    {
        if(chessWindow == null)
        {
            return "No chess game running";
        }
        chessWindow.ResetTileColors();
        return "Succesfuly reset chess color.";
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
