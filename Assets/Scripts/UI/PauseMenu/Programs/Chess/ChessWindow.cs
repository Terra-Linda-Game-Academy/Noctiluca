using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stockfish;
using Stockfish.NET;
using System.Threading;
using System.IO;
using System;
using System.Linq;
using UnityEngine.Events;

public enum ChessPieceType
{
    Pawn,
    Rook,
    Knight,
    Bishop,
    Queen,
    King
}

public enum GameOutcome
{
    BlackWin,
    WhiteWin,
    Stalemate,
    Repetition,
    Draw,
    InProgress
}

public enum ChessPieceColor
{
    White,
    Black
}

public class ChessPiece
{
    public ChessPieceType type;
    public ChessPieceColor color;
    public bool hasMoved = false;
    public bool isAlive = true;
    public int x;
    public int y;

    public void Move(int x, int y)
    {
        this.x = x;
        this.y = y;
        hasMoved = true;
    }

    public void Kill()
    {
        isAlive = false;
    }

    public ChessPiece(ChessPieceType type, ChessPieceColor color)
    {
        this.type = type;
        this.color = color;
    }

    public ChessPiece (ChessPiece piece)
    {
        type = piece.type;
        color = piece.color;
        hasMoved = piece.hasMoved;
        isAlive = piece.isAlive;
        x = piece.x;
        y = piece.y;
    }
}

public class ChessBoard {
    private ChessPiece[,] board = new ChessPiece[8, 8];
    public float squareSize = 50f;
    public float xOffset = 100f;
    public float yOffset = 100f;

    void UslessFunction()
    {
        
    }
    public ChessBoard() {
        OnCapture += (piece) => { Debug.Log("Captured "); };
        OnMove += (x, y, x1,y1, c, asd) => { Debug.Log("Moved "); };
        OnCheck += (color) => { Debug.Log("Check "); };
        OnCheckmate += (color) => { Debug.Log("Checkmate "); };
        OnStalemate += () => { Debug.Log("Stalemate "); };
        OnCastle += (color, side) => { Debug.Log("Castle "); };
        OnTurnEnd += (color) => { Debug.Log("Turn End "); };
    }

    public ChessPiece[,] GetBoard()
    {
        return board;
    }

    public ChessPiece GetPiece(int x, int y)
    {
        return board[x, y];
    }

    public void SetPiece(int x, int y, ChessPiece piece)
    {
        board[x, y] = piece;
    }

    public Vector2Int[] GetPieceMoves(ChessPiece piece, ChessPiece[,] board) {
        List<Vector2Int> moves = new List<Vector2Int>();
        switch (piece.type)
        {
            case ChessPieceType.Pawn:
                moves.AddRange(GetPawnMoves(piece, board));
                break;
            case ChessPieceType.Rook:
                moves.AddRange(GetRookMoves(piece,board));
                break;
            case ChessPieceType.Knight:
                moves.AddRange(GetKnightMoves(piece,board));
                break;
            case ChessPieceType.Bishop:
                moves.AddRange(GetBishopMoves(piece,board));
                break;
            case ChessPieceType.Queen:
                moves.AddRange(GetQueenMoves(piece,board));
                break;
            case ChessPieceType.King:
                moves.AddRange(GetKingMoves(piece,board));
                //add castling
                int castle = IsCastleLegal(piece.color);
                if (castle == 0)
                {
                    moves.Add(new Vector2Int(6, piece.y));
                }
                else if (castle == 1)
                {
                    moves.Add(new Vector2Int(2, piece.y));
                }
                else if (castle == 2)
                {
                    moves.Add(new Vector2Int(6, piece.y));
                    moves.Add(new Vector2Int(2, piece.y));
                }
                break;
        }
        return moves.ToArray();
    }

    public Vector2Int[] GetPawnMoves(ChessPiece piece, ChessPiece[,] board)
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        int direction = piece.color == ChessPieceColor.White ? -1 : 1;
        int x = piece.x;
        int y = piece.y;
        if (board[x, y + direction] == null)
        {
            moves.Add(new Vector2Int(x, y + direction));
            if (!piece.hasMoved && board[x, y + direction * 2] == null)
            {
                moves.Add(new Vector2Int(x, y + direction * 2));
            }
        }
        if (x > 0 && board[x - 1, y + direction] != null && board[x - 1, y + direction].color != piece.color)
        {
            moves.Add(new Vector2Int(x - 1, y + direction));
        }
        if (x < 7 && board[x + 1, y + direction] != null && board[x + 1, y + direction].color != piece.color)
        {
            moves.Add(new Vector2Int(x + 1, y + direction));
        }

        //add en passant
        if (EnPeasantSquare != new Vector2Int(-1,-1) && board[EnPeasantSquare.x, EnPeasantSquare.y] == null) {
            if (EnPeasantSquare.x == x - 1 && EnPeasantSquare.y == y + direction) {
                moves.Add(new Vector2Int(x - 1, y + direction));
            }
            else if (EnPeasantSquare.x == x + 1 && EnPeasantSquare.y == y + direction) {
                moves.Add(new Vector2Int(x + 1, y + direction));
            }
        }


        return moves.ToArray();
    }

    public Vector2Int[] GetRookMoves(ChessPiece piece, ChessPiece[,] board)
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        int x = piece.x;
        int y = piece.y;
        for (int i = x + 1; i < 8; i++)
        {
            if (board[i, y] == null)
            {
                moves.Add(new Vector2Int(i, y));
            }
            else
            {
                if (board[i, y].color != piece.color)
                {
                    moves.Add(new Vector2Int(i, y));
                }
                break;
            }
        }
        for (int i = x - 1; i >= 0; i--)
        {
            if (board[i, y] == null)
            {
                moves.Add(new Vector2Int(i, y));
            }
            else
            {
                if (board[i, y].color != piece.color)
                {
                    moves.Add(new Vector2Int(i, y));
                }
                break;
            }
        }
        for (int i = y + 1; i < 8; i++)
        {
            if (board[x, i] == null)
            {
                moves.Add(new Vector2Int(x, i));
            }
            else
            {
                if (board[x, i].color != piece.color)
                {
                    moves.Add(new Vector2Int(x, i));
                }
                break;
            }
        }
        for (int i = y - 1; i >= 0; i--)
        {
            if (board[x, i] == null)
            {
                moves.Add(new Vector2Int(x, i));
            }
            else
            {
                if (board[x, i].color != piece.color)
                {
                    moves.Add(new Vector2Int(x, i));
                }
                break;
            }
        }
        return moves.ToArray();
    }

    public Vector2Int[] GetKnightMoves(ChessPiece piece, ChessPiece[,] board)
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        int x = piece.x;
        int y = piece.y;
        if (x > 1)
        {
            if (y > 0 && (board[x - 2, y - 1] == null || board[x - 2, y - 1].color != piece.color))
            {
                moves.Add(new Vector2Int(x - 2, y - 1));
            }
            if (y < 7 && (board[x - 2, y + 1] == null || board[x - 2, y + 1].color != piece.color))
            {
                moves.Add(new Vector2Int(x - 2, y + 1));
            }
        }
        if (x < 6)
        {
            if (y > 0 && (board[x + 2, y - 1] == null || board[x + 2, y - 1].color != piece.color))
            {
                moves.Add(new Vector2Int(x + 2, y - 1));
            }
            if (y < 7 && (board[x + 2, y + 1] == null || board[x + 2, y + 1].color != piece.color))
            {
                moves.Add(new Vector2Int(x + 2, y + 1));
            }
        }
        if (y > 1)
        {
            if (x > 0 && (board[x - 1, y - 2] == null || board[x - 1, y - 2].color != piece.color))
            {
                moves.Add(new Vector2Int(x - 1, y - 2));
            }
            if (x < 7 && (board[x + 1, y - 2] == null || board[x + 1, y - 2].color != piece.color))
            {
                moves.Add(new Vector2Int(x + 1, y - 2));
            }
        }
        if (y < 6)
        {
            if (x > 0 && (board[x - 1, y + 2] == null || board[x - 1, y + 2].color != piece.color))
            {
                moves.Add(new Vector2Int(x - 1, y + 2));
            }
            if (x < 7 && (board[x + 1, y + 2] == null || board[x + 1, y + 2].color != piece.color))
            {
                moves.Add(new Vector2Int(x + 1, y + 2));
            }
        }
        return moves.ToArray();
    }

    public Vector2Int[] GetBishopMoves(ChessPiece piece, ChessPiece[,] board)
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        int x = piece.x;
        int y = piece.y;
        for (int i = 1; i < 8; i++)
        {
            if (x + i < 8 && y + i < 8)
            {
                if (board[x + i, y + i] == null)
                {
                    moves.Add(new Vector2Int(x + i, y + i));
                }
                else
                {
                    if (board[x + i, y + i].color != piece.color)
                    {
                        moves.Add(new Vector2Int(x + i, y + i));
                    }
                    break;
                }
            }
            else
            {
                break;
            }
        }
        for (int i = 1; i < 8; i++)
        {
            if (x - i >= 0 && y + i < 8)
            {
                if (board[x - i, y + i] == null)
                {
                    moves.Add(new Vector2Int(x - i, y + i));
                }
                else
                {
                    if (board[x - i, y + i].color != piece.color)
                    {
                        moves.Add(new Vector2Int(x - i, y + i));
                    }
                    break;
                }
            }
            else
            {
                break;
            }
        }
        for (int i = 1; i < 8; i++)
        {
            if (x + i < 8 && y - i >= 0)
            {
                if (board[x + i, y - i] == null)
                {
                    moves.Add(new Vector2Int(x + i, y - i));
                }
                else
                {
                    if (board[x + i, y - i].color != piece.color)
                    {
                        moves.Add(new Vector2Int(x + i, y - i));
                    }
                    break;
                }
            }
            else
            {
                break;
            }
        }
        for (int i = 1; i < 8; i++)
        {
            if (x - i >= 0 && y - i >= 0)
            {
                if (board[x - i, y - i] == null)
                {
                    moves.Add(new Vector2Int (x - i, y - i));
                }
                else
                {
                    if (board[x - i, y - i].color != piece.color)
                    {
                        moves.Add(new Vector2Int(x - i, y - i));
                    }
                    break;
                }
            }
            else
            {
                break;
            }
        }
        return moves.ToArray();
    }

    public Vector2Int[] GetQueenMoves(ChessPiece piece, ChessPiece[,] board)
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        moves.AddRange(GetBishopMoves(piece, board));
        moves.AddRange(GetRookMoves(piece, board));
        return moves.ToArray();
    }

    public Vector2Int[] GetKingMoves(ChessPiece piece, ChessPiece[,] board)
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        int x = piece.x;
        int y = piece.y;
        if (x > 0)
        {
            if (board[x - 1, y] == null || board[x - 1, y].color != piece.color)
            {
                moves.Add(new Vector2Int(x - 1, y));
            }
            if (y > 0 && (board[x - 1, y - 1] == null || board[x - 1, y - 1].color != piece.color))
            {
                moves.Add(new Vector2Int(x - 1, y - 1));
            }
            if (y < 7 && (board[x - 1, y + 1] == null || board[x - 1, y + 1].color != piece.color))
            {
                moves.Add(new Vector2Int(x - 1, y + 1));
            }
        }
        if (x < 7)
        {
            if (board[x + 1, y] == null || board[x + 1, y].color != piece.color)
            {
                moves.Add(new Vector2Int(x + 1, y));
            }
            if (y > 0 && (board[x + 1, y - 1] == null || board[x + 1, y - 1].color != piece.color))
            {
                moves.Add(new Vector2Int(x + 1, y - 1));
            }
            if (y < 7 && (board[x + 1, y + 1] == null || board[x + 1, y + 1].color != piece.color))
            {
                moves.Add(new Vector2Int(x + 1, y + 1));
            }
        }
        if (y > 0 && (board[x, y - 1] == null || board[x, y - 1].color != piece.color))
        {
            moves.Add(new Vector2Int(x, y - 1));
        }
        if (y < 7 && (board[x, y + 1] == null || board[x, y + 1].color != piece.color))
        {
            moves.Add(new Vector2Int(x, y + 1));
        }
        return moves.ToArray();
    }

    public bool InCheck(ChessPiece[,] currentBoard, ChessPieceColor color) {
        ChessPiece king = GetKing(currentBoard,color);
        if (king == null)
        {
            return false;
        }
        foreach (ChessPiece piece in currentBoard)
        {
            if (piece != null && piece.color != color && piece.type != ChessPieceType.King)
            {
                Vector2Int[] moves = GetPieceMoves(piece, currentBoard);
                foreach (Vector2Int move in moves)
                {
                    if (move.x == king.x && move.y == king.y)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public ChessPiece GetKing(ChessPiece[,] currentBoard, ChessPieceColor color)
    {
        foreach (ChessPiece piece in currentBoard)
        {
            if (piece != null && piece.color == color && piece.type == ChessPieceType.King)
            {
                return piece;
            }
        }
        return null;
    }

    public ChessPiece GetKing(ChessPieceColor color)
    {
        return GetKing(board, color);
    }

    public bool InCheck(ChessPieceColor color, int x, int y)
    {
        ChessPiece[,] newBoard = CopyBoard(board);
        ChessPiece king = GetKing(color);
        MovePiece(ref newBoard, color, king.x, king.y, x, y);
        return InCheck(board, color);
    }

    public bool InCheck(ChessPieceColor color)
    {
        return InCheck(board, color);
    }

    public bool InCheckmate(ChessPieceColor color)
    {
        if (!InCheck(color))
        {
            return false;
        }
        // ChessPiece king = GetKing(color);
        // Vector2Int[] kingMoves = GetKingMoves(king, board);
        // foreach (Vector2Int move in kingMoves)
        // {
        //     ChessPiece[,] newBoard = CopyBoard(board);
        //     MovePiece(ref newBoard, king.color, king.x, king.y, move.x, move.y);
        //     if (!InCheck(newBoard, color))
        //     {
        //         return false;
        //     }
        // }
        foreach (ChessPiece piece in board)
        {
            if (piece != null && piece.color == color)
            {
                List<Vector2Int> moves = GetLegalMoves(piece);
                // foreach (Vector2Int move in moves)
                // {
                //     ChessPiece[,] newBoard = CopyBoard(board);
                //     MovePiece(ref newBoard, piece.color, piece.x, piece.y, move.x, move.y);
                //     if (!InCheck(newBoard, color))
                //     {
                //         return false;
                //     }
                // }
                if (moves.Count > 0)
                {
                    return false;
                }
            }
        }
        return true;
    }


    public bool InStalemate(ChessPieceColor color)
    {
        if (InCheck(color))
        {
            return false;
        }
        foreach (ChessPiece piece in board)
        {
            if (piece != null && piece.color == color)
            {
                if(GetLegalMoves(piece).Count > 0)
                {
                    return false;
                }
            }
        }
        return true;
    }


    public bool InCheckmate()
    {
        return InCheckmate(ChessPieceColor.White) || InCheckmate(ChessPieceColor.Black);
    }

    public bool InStalemate()
    {
        return (InStalemate(ChessPieceColor.White) && turn==ChessPieceColor.Black) || (InStalemate(ChessPieceColor.Black) && turn==ChessPieceColor.White);
    }

    public GameOutcome GetGameOutcome()
    {
        if (InCheckmate(ChessPieceColor.White))
        {
            return GameOutcome.BlackWin;
        }
        if (InCheckmate(ChessPieceColor.Black))
        {
            return GameOutcome.WhiteWin;
        }
        if (InStalemate())
        {
            return GameOutcome.Stalemate;
        }
        return GameOutcome.InProgress;
    }

    public ChessPieceColor turn = ChessPieceColor.White; 


    public GameOutcome gameOutcome = GameOutcome.InProgress;
    public bool gameOver = false;




    public GameOutcome GetOutcome() {
        return gameOutcome;
    }

    public void EndTurn() {

        //check game outcome
        GameOutcome outcome = GetGameOutcome();

        SwitchTurn();
        ChessPieceColor otherColor = turn == ChessPieceColor.White ? ChessPieceColor.Black : ChessPieceColor.White;
        if(InCheck(turn)) {
            Debug.Log("Check!");
            OnCheck(turn);
        } else if(InCheck(otherColor)) {
            Debug.Log("Check!");
            OnCheck(otherColor);
        }

        

        
        
        //switch turn
        switch (outcome)
        {
            case GameOutcome.BlackWin:
                OnCheckmate(ChessPieceColor.Black);
                Debug.Log("Black wins!");
                gameOutcome = GameOutcome.BlackWin;
                gameOver = true;
                break;
            case GameOutcome.WhiteWin:
                OnCheckmate(ChessPieceColor.White);
                Debug.Log("White wins!");
                gameOutcome = GameOutcome.WhiteWin;
                gameOver = true;
                break;
            case GameOutcome.Stalemate:
                Debug.Log("Draw!");
                OnStalemate();
                gameOutcome = GameOutcome.Stalemate;
                gameOver = true;
                break;
            case GameOutcome.InProgress:
                OnTurnEnd(turn);
                break;
        }
        
    }

    public void SwitchTurn()
    {
        turn = turn == ChessPieceColor.White ? ChessPieceColor.Black : ChessPieceColor.White;
        //OnTurnSwitch(turn);
    }

    public bool MovePiece(ref ChessPiece[,] currentBoard, ChessPieceColor color, int x, int y, int newX, int newY)
    {
        
        if (x == newX && y == newY || (currentBoard[newX, newY] != null && currentBoard[newX, newY].color == color))
        {
            return false;
        }
        if (currentBoard[newX, newY] != null)
        {
            KillPiece(currentBoard, newX, newY);
        }
        currentBoard[x,y].Move(newX, newY);
        currentBoard[newX, newY] = currentBoard[x,y];
        currentBoard[x, y] = null;
        
        return true;
    }

    public void KillPiece(ChessPiece[,] currentBoard, int x, int y)
    {
        ChessPiece piece = currentBoard[x, y];
        piece.Kill();
        currentBoard[x, y] = null;
    }

    public bool MovePiece(int x, int y, int newX, int newY)
    {
        ChessPiece piece = board[x, y];
        return MovePiece(piece, newX, newY);
    }

    Vector2Int EnPeasantSquare = new Vector2Int(-1, -1);

    public bool MovePiece(ChessPiece piece, int newX, int newY, ChessPieceType promotionType = ChessPieceType.Pawn)
    {
        Debug.Log("Move piece");
        bool attacking = false;

        //if king is moved, check if it is castling
        if(piece.type == ChessPieceType.King && Mathf.Abs(piece.x - newX)>=2) {
            if(newX == 2) {
                Castle(1, piece.color);
                return true;
            } else if(newX == 6) {
                //castle left
                Castle(0, piece.color);
                return true;
            }
        }

        bool enPeasant = false;

        //check if en peasant, if piece moved diagonally to empty square, kill the piece behind it
        if(piece.type == ChessPieceType.Pawn && newX != piece.x && board[newX, newY] == null) {
            Debug.Log("En peasant");
            if(piece.color == ChessPieceColor.White) {
                KillPiece(newX, newY+1);
                attacking=true;
                enPeasant=true;
            } else if(piece.color == ChessPieceColor.Black) {
                KillPiece(newX, newY-1);
                attacking=true;
                enPeasant=true;
            }
        }


        //en passant if pawn moved 2 squares
        if(piece.type == ChessPieceType.Pawn && Mathf.Abs(newY - piece.y) == 2) {
            if(piece.color == ChessPieceColor.White)
                EnPeasantSquare = new Vector2Int(newX, newY+1);
            else if(piece.color == ChessPieceColor.Black) {
                EnPeasantSquare = new Vector2Int(newX, newY-1);
            }
        } else {
            EnPeasantSquare = new Vector2Int(-1, -1);
        }

        
        if(!enPeasant)
            if(piece.x == newX && piece.y == newY || (board[newX, newY]!= null && board[newX, newY].color == piece.color) || !GetLegalMoves(piece).Contains(new Vector2Int(newX, newY)))
                return false;
        
        
        if(!enPeasant && board[newX, newY] != null) {
            KillPiece(newX, newY);
            attacking=true;
        }

        bool promotion = false;

        if(piece.type == ChessPieceType.Pawn && (newY == 0 || newY == 7)) {
            if(promotionType == ChessPieceType.Pawn) {
                return false;
            }
            promotion = true;
            
        }
            
        

        Debug.Log("Moving " + piece.color+piece.type + " from " + piece.x + ", " + piece.y + " to " + newX + ", " + newY + ".");

        int x = piece.x;
        int y = piece.y;

        board[newX, newY] = piece;
        board[piece.x, piece.y] = null;
        piece.Move(newX, newY);

        OnMove(piece.color, x, y, newX, newY, promotion);

            
            

        //promotion
        if(promotion) {
            Promote(promotionType, piece.color, newX, newY);
        }

        EndTurn();
        
        return true;
    }

    public bool IsPromotion(ChessPiece piece, int newX, int newY) {
        if(piece.type == ChessPieceType.Pawn && (newY == 0 || newY == 7)) {
            return true;
        }
        return false;
    }

    public void Promote(ChessPieceType type, ChessPieceColor color, int x, int y) {
        board[x,y] = CreatePiece(type, board[x, y].color, x, y);
    }
        

    public ChessPiece CreatePiece(ChessPieceType type, ChessPieceColor color, int x, int y) {
        ChessPiece piece = new ChessPiece (type, color);
        piece.Move(x,y);
        return piece;
    }

    public void IrregularMove(ChessPiece piece, int newX, int newY)
    {
        board[newX, newY] = piece;
        board[piece.x, piece.y] = null;
        piece.Move(newX, newY);
    }

    //letter to type
    public ChessPieceType LetterToType(char letter) {
        switch((letter + "").ToLower().ToCharArray()[0]) {
            case 'k':
                return ChessPieceType.King;
            case 'q':
                return ChessPieceType.Queen;
            case 'r':
                return ChessPieceType.Rook;
            case 'b':
                return ChessPieceType.Bishop;
            case 'n':
                return ChessPieceType.Knight;
            case 'p':
                return ChessPieceType.Pawn;
            default:
                return ChessPieceType.Pawn;
        }
    }

    //type to letter
    public char TypeToLetter(ChessPieceType type) {
        switch(type) {
            case ChessPieceType.King:
                return 'K';
            case ChessPieceType.Queen:
                return 'Q';
            case ChessPieceType.Rook:
                return 'R';
            case ChessPieceType.Bishop:
                return 'B';
            case ChessPieceType.Knight:
                return 'N';
            case ChessPieceType.Pawn:
                return 'P';
            default:
                return 'P';
        }
    }

    // Example: Rxg7+ to a7g7
    public string PuzzleNotation(string notation, ChessPieceColor color)
    {
        Debug.Log("Puzzle notation: " + notation);

        notation = notation.Replace("x", "").Replace("+", "").Replace("#", "").Replace(" ", "").Trim();

        Debug.Log("New Puzzle notation: " + notation + ", Length: " + notation.Length);

        
        if(notation.Contains("=")) {
            Debug.Log("1.0");
            string endPos = notation.Substring(0, 2);
            string piece = notation.Split ('=')[1];

            Vector2Int pos = GetNotationCoordinates(endPos);
            ChessPiece pawn = FindPieces((piece) => (piece.type == LetterToType(notation[0]) && piece.color == turn && piece.type == ChessPieceType.Pawn && GetLegalMoves(piece).Contains(GetNotationCoordinates(endPos))))[0];
            return GetNotation(new Vector2Int(pawn.x, pawn.y)) + endPos + "=" +piece.ToUpper();
            
        }
        //if calsling
        else if (notation == "O-O") {
            Debug.Log("2.0");
            if(color == ChessPieceColor.White) {
                return "e1g1";
            } else {
                return "e8g8";
            }
        } else if (notation == "O-O-O") {
            Debug.Log("3.0");
            if(color == ChessPieceColor.White) {
                return "e1c1";
            } else {
                return "e8c8";
            }
        }
        else {
            if(notation.Length == 2) {
                Debug.Log("4.0");
                ChessPiece movePiece = FindPieces((piece) => (piece.color == turn&& piece.type==ChessPieceType.Pawn && GetLegalMoves(piece).Contains(GetNotationCoordinates(notation))))[0];
                return GetNotation(movePiece.x, movePiece.y) + notation;
            }
            else if(notation.Length == 3) {
                Debug.Log("5.0");
                string firstLetter = notation.Substring(0, 1);
                string endPos = notation.Substring(1, 2);
                bool isFirstCharNumber = int.TryParse(firstLetter, out int n);

                //check if first letter is capital
                if(firstLetter == firstLetter.ToUpper() && !isFirstCharNumber) {
                    Debug.Log("5.1");
                    Debug.Log("Notation: " + notation);
                    Debug.Log("Must be: " + LetterToType(firstLetter.ToCharArray()[0]) + ", " + color + ", " + GetNotationCoordinates(endPos));
                    
                    ChessPiece movePiece = FindPieces((piece) => (piece.type == LetterToType(notation[0]) && piece.color == turn && GetLegalMoves(piece).Contains(GetNotationCoordinates(endPos))))[0];
                    return GetNotation(movePiece.x, movePiece.y) + endPos;
                } else {
                    Debug.Log("5.2");
                    //if first letter is letter or number
                    List<ChessPiece>  possiblePieces = FindPieces((piece) => (piece.color == turn && GetLegalMoves(piece).Contains(GetNotationCoordinates(endPos))));
                    foreach(ChessPiece piece in possiblePieces) {
                        bool rightLocation = true;
                        if(isFirstCharNumber) {
                            //check if y is right
                            if(7-piece.y != int.Parse(firstLetter) - 1) {
                                rightLocation = false;
                            }
                            
                        } else {
                            //check if x is right
                            if(piece.x != firstLetter.ToLower().ToCharArray()[0] - 97) {
                                rightLocation = false;
                            }
                            
                        }
                        if(rightLocation) {
                            return GetNotation(piece.x, piece.y) + endPos;
                        }
                    }
                }
            } else if (notation.Length == 4) {
                Debug.Log("6.0");
                string firstLetter = notation.Substring(0, 1);
                string secondLetter = notation.Substring(1, 1);
                string endPos = notation.Substring(2, 2);
                bool isSecondCharNumber = int.TryParse(secondLetter, out int n);

                List<ChessPiece>  possiblePieces = FindPieces((piece) => (piece.type == LetterToType(firstLetter.ToCharArray()[0]) && piece.color == color && GetLegalMoves(piece).Contains(GetNotationCoordinates(endPos))));
                foreach(ChessPiece piece in possiblePieces) {
                    bool rightLocation = true;
                    if(isSecondCharNumber) {
                        int y =int.Parse(secondLetter) - 1;
                        Debug.Log("y: " + y + ", piece.y: " + (7-piece.y));
                        //check if y is right
                        if(7-piece.y != int.Parse(secondLetter) - 1) {
                            rightLocation = false;
                        }
                        
                    } else {
                        int x = secondLetter.ToLower().ToCharArray()[0] - 97;
                        Debug.Log("x: " + x + ", piece.x: " + piece.x);
                        //check if x is right
                        if(piece.x != secondLetter.ToLower().ToCharArray()[0] - 97) {
                            rightLocation = false;
                        }
                        
                    }
                    if(rightLocation) {
                        return GetNotation(piece.x, piece.y) + endPos;
                    }
                }
            }
        }
        Debug.Log("7.0");
        return "";
        
    }




    //Find piece FindPiece((piece) => (piece.x == 5))
    public List<ChessPiece> FindPieces(Func<ChessPiece, bool> predicate) {
        List<ChessPiece> pieces = new List<ChessPiece>();
        foreach(ChessPiece piece in board) {
            if(piece != null && predicate(piece)) {
                pieces.Add(piece);
            }
        }
        return pieces;
    }

    public void NotationMove(string notation, ChessPieceColor color) {

        Debug.Log("Notation: " + notation);
        // if (notation == "O-O")
        // {
        //     Castle(0, color); // Perform king-side castle
        // }
        // else if (notation == "O-O-O")
        // {
        //     Castle(1, color); // Perform queen-side castle
        // }
        //else 
        if (notation.Length == 4)
        {
            int startX = notation[0] - 'a'; // Convert letter to x-coordinate
            int startY = '8' - notation[1]; // Convert number to y-coordinate
            int endX = notation[2] - 'a'; // Convert letter to x-coordinate
            int endY = '8' - notation[3]; // Convert number to y-coordinate

            //e1g1 is king side castle for white
            //e1c1 is queen side castle for white
            //e8g8 is king side castle for black
            //e8c8 is queen side castle for black

            if(!GetKing(ChessPieceColor.White).hasMoved && startX == 4 && startY == 0 && endX == 6 && endY == 0) {
                Castle(0, ChessPieceColor.White);
                return;
            } else if(!GetKing(ChessPieceColor.White).hasMoved &&startX == 4 && startY == 0 && endX == 2 && endY == 0) {
                Castle(1, ChessPieceColor.White);
                return;
            } else if(!GetKing(ChessPieceColor.Black).hasMoved &&startX == 4 && startY == 7 && endX == 6 && endY == 7) {
                Castle(0, ChessPieceColor.Black);
                return;
            } else if(!GetKing(ChessPieceColor.Black).hasMoved &&startX == 4 && startY == 7 && endX == 2 && endY == 7) {
                Castle(1, ChessPieceColor.Black);
                return;
            }


            ChessPiece piece = GetPiece(startX, startY);
            MovePiece(piece, endX, endY);
        } else if (notation.Length == 5)//promotion 
        {
            int startX = notation[0] - 'a'; // Convert letter to x-coordinate
            int startY = '8' - notation[1]; // Convert number to y-coordinate
            int endX = notation[2] - 'a'; // Convert letter to x-coordinate
            int endY = '8' - notation[3]; // Convert number to y-coordinate
            char promotion = notation[4];
            ChessPieceType type = ChessPieceType.Pawn;
            switch(promotion) {
                case 'q':
                    type = ChessPieceType.Queen;
                    break;
                case 'r':
                    type = ChessPieceType.Rook;
                    break;
                case 'b':
                    type = ChessPieceType.Bishop;
                    break;
                case 'n':
                    type = ChessPieceType.Knight;
                    break;
            }
            Debug.Log("Promotion to " + type);
            ChessPiece piece = GetPiece(startX, startY);
            MovePiece(piece, endX, endY, type);
        }
        else
        {
            Debug.Log("Invalid algebraic notation!");
        }
    }

    public Vector2Int GetNotationCoordinates(string notation) { 
        int x = notation[0] - 'a'; // Convert letter to x-coordinate
        int y = '8' - notation[1]; // Convert number to y-coordinate
        return new Vector2Int(x, y);
    }

    //get notation from coordinates
    public string GetNotation(Vector2Int coordinates) {
        string notation = "";
        notation += (char)('a' + coordinates.x);
        notation += (char)('8' - coordinates.y);
        return notation;
    }

    public string GetNotation(int x, int y) {
        string notation = "";
        notation += (char)('a' + x);
        notation += (char)('8' - y);
        return notation;
    }


    public int IsCastleLegal(ChessPieceColor color) {
        // 0 = king-side, 1 = queen-side, -1 = illegal 2 = both
        int y = color == ChessPieceColor.White ? 7 : 0;
        int kingSide = 0;
        int queenSide = 0;
       
        for (int i = 5; i < 7; i++)
        {
            if (board[i, y] != null)
            {
                kingSide++;
            }
        }

        if (board[7, y] == null || board[7, y].type != ChessPieceType.Rook || board[7, y].color != color || board[7, y].hasMoved)
        {
            kingSide++;
        }
        if (board[4, y] == null || board[4, y].type != ChessPieceType.King || board[4, y].color != color || board[4, y].hasMoved)
        {
            kingSide++;
        }
        if (InCheck(color, 3, y))
        {
            kingSide++;
        }
        if (InCheck(color, 4, y))
        {
            kingSide++;
        }
        if (InCheck(color, 5, y))
        {
            kingSide++;
        }
        if (InCheck(color, 6, y))
        {
            kingSide++;
        }

       
        for (int i = 1; i < 4; i++)
        {
            if (board[i, y] != null)
            {
                queenSide++;
            }
        }

        if (board[0, y] == null || board[0, y].type != ChessPieceType.Rook || board[0, y].color != color || board[0, y].hasMoved)
        {
            queenSide++;
        }

        if (board[4, y] == null || board[4, y].type != ChessPieceType.King || board[4, y].color != color || board[4, y].hasMoved)
        {
            queenSide++;
        }

        if (InCheck(color, 3, y))
        {
            queenSide++;
        }

        if (InCheck(color, 4, y))
        {
            queenSide++;
        }

        if (InCheck(color, 3, y))
        {
            queenSide++;
        }

        if (InCheck(color, 2, y))
        {
            queenSide++;
        }

        if(queenSide == 0 && kingSide==0) {
            return 2;

        } else if (queenSide == 0) {
            return 1;
        } else if (kingSide == 0) {
            return 0;
        } else {
            return -1;
        }

    }

    public void Castle(int side, ChessPieceColor color)
    {
        int y = color == ChessPieceColor.White ? 7 : 0;
        int legal = IsCastleLegal(color);
        if(!(legal == 2 || legal == side)) 
            return;
        if (side == 0)
        {
            IrregularMove(GetPiece(4, y), 6, y);
            IrregularMove(GetPiece(7, y), 5, y);
        }
        else
        {
            IrregularMove(GetPiece(4, y), 2, y);
            IrregularMove(GetPiece(0, y), 3, y);
            
        }

        OnCastle(color, side);
        EndTurn();
    }



    public void KillPiece(int x, int y)
    {
        ChessPiece piece = board[x, y];
        OnCapture(piece.color);
        piece.Kill();
        board[x, y] = null;
    }

    //get legal moves
    public List<Vector2Int> GetLegalMoves(ChessPiece piece)
    {
        List<Vector2Int> legalMoves = new List<Vector2Int>();
        if(piece == null) {
            return legalMoves;
        }
        Vector2Int[] moves = GetPieceMoves(piece, board);

        foreach (Vector2Int move in moves)
        {
            ChessPiece[,] newBoard = CopyBoard(board);
            MovePiece(ref newBoard, piece.color, piece.x, piece.y, move.x, move.y);

            if (!InCheck(newBoard, piece.color))
            {
                ChessPiece otherKing = GetKing(GetOpponentColor(piece.color));
                //if king, check if in other king's attack range
                if (piece.type == ChessPieceType.King)
                {
                    if (Mathf.Abs(otherKing.x - move.x) <= 1 && Mathf.Abs(otherKing.y - move.y) <= 1)
                    {
                        continue;
                    }
                }
                legalMoves.Add(move);
            }
        }

        return legalMoves;
    }

    ChessPiece[] GetPiecesByColor(ChessPieceColor color)
    {
        List<ChessPiece> pieces = new List<ChessPiece>();

        foreach (ChessPiece piece in board)
        {
            if (piece != null && piece.color == color)
            {
                pieces.Add(piece);
            }
        }

        return pieces.ToArray();
    }

    ChessPieceColor GetOpponentColor (ChessPieceColor color)
    {
        return color == ChessPieceColor.White ? ChessPieceColor.Black : ChessPieceColor.White;
    }


    public ChessPiece[,] CopyBoard(ChessPiece[,] boardToCopy)
    {
        ChessPiece[,] newBoard = new ChessPiece[8, 8];
        for (int i = 0; i < boardToCopy.GetLength(0); i++)
        {
            for (int j = 0; j < boardToCopy.GetLength(1); j++)
            {
                if (boardToCopy[i, j] != null)
                {
                    newBoard[i, j] = new ChessPiece(boardToCopy[i, j]);
                }
            }
        }
        return newBoard;
    }

    public void LoadFEN(string fen)
    {

        FENInfo FENinfo = ReadFEN(fen);
        board = FENinfo.board;
        EnPeasantSquare = FENinfo.EnPeasantSquare;
        turn = FENinfo.turn;
        Debug.Log("Fen turen: " + turn);
        halfMoveClock = FENinfo.halfMoveClock;
        fullMoveNumber = FENinfo.fullMoveNumber;

    }

    public struct FENInfo
    {
        public ChessPiece[,] board;
        public Vector2Int EnPeasantSquare;
        public ChessPieceColor turn;
        public int halfMoveClock;
        public int fullMoveNumber;

        public FENInfo(ChessPiece[,] board, Vector2Int EnPeasantSquare, ChessPieceColor turn, int halfMoveClock, int fullMoveNumber)
        {
            this.board = board;
            this.EnPeasantSquare = EnPeasantSquare;
            this.turn = turn;
            this.halfMoveClock = halfMoveClock;
            this.fullMoveNumber = fullMoveNumber;
        }
    }


    
    public FENInfo ReadFEN(string fen) {
        //read FEN string and return a 2d array of chess pieces
        //https://en.wikipedia.org/wiki/Forsyth%E2%80%93Edwards_Notation
        string[] FENChunks = fen.Split(' ');

        string[] fenRows = FENChunks[0].Split('/');
        ChessPiece[,] tempBoard = new ChessPiece[8, 8];
        for (int i = 0; i < fenRows.Length; i++)
        {
            string row = fenRows[i];
            int x = 0;
            for (int j = 0; j < row.Length; j++)
            {
                char c = row[j];
                if (char.IsDigit(c))
                {
                    int space = (int)char.GetNumericValue(c);
                    for (int k = 0; k < space; k++)
                    {
                        tempBoard[x, i] = null;
                        x++;
                    }

                }
                else
                {
                    ChessPiece piece = null;
                    
                    switch (c)
                    {
                        case 'p':
                            piece = new ChessPiece(ChessPieceType.Pawn, ChessPieceColor.Black);
                            //set piece to have moved if it is not the second row
                            if (i != 1)
                            {
                                piece.hasMoved = true;
                            }
                            break;
                        case 'r':
                            piece = new ChessPiece(ChessPieceType.Rook, ChessPieceColor.Black);
                            break;
                        case 'n':
                            piece = new ChessPiece(ChessPieceType.Knight, ChessPieceColor.Black);
                            break;
                        case 'b':
                            piece = new ChessPiece(ChessPieceType.Bishop, ChessPieceColor.Black);
                            break;
                        case 'q':
                            piece = new ChessPiece(ChessPieceType.Queen, ChessPieceColor.Black);
                            break;
                        case 'k':
                            piece = new ChessPiece(ChessPieceType.King, ChessPieceColor.Black);
                            break;
                        case 'P':
                            piece = new ChessPiece(ChessPieceType.Pawn, ChessPieceColor.White);
                            //set piece to have moved if it is not the second row
                            if (i != 6)
                            {
                                piece.hasMoved = true;
                            }
                            break;
                        case 'R':
                            piece = new ChessPiece(ChessPieceType.Rook, ChessPieceColor.White);
                            break;
                        case 'N':
                            piece = new ChessPiece(ChessPieceType.Knight, ChessPieceColor.White);
                            break;
                        case 'B':
                            piece = new ChessPiece(ChessPieceType.Bishop, ChessPieceColor.White);
                            break;
                        case 'Q':
                            piece = new ChessPiece(ChessPieceType.Queen, ChessPieceColor.White);
                            break;
                        case 'K':
                            piece = new ChessPiece(ChessPieceType.King, ChessPieceColor.White);
                            break;
                    }
                    piece.x = x;
                    piece.y = i;
                    tempBoard[x, i] = piece;
                    x++;
                }
            }
        }
         ChessPieceColor turnColor = ChessPieceColor.White;
        if(FENChunks.Length > 1) {
            string turnTemp = FENChunks[1];
            turnColor = (turnTemp == "w") ? ChessPieceColor.White : ChessPieceColor.Black;
        }

        if(FENChunks.Length > 2) {
            string castling = FENChunks[2];
            if (castling != "-")
            {
                for (int i = 0; i < castling.Length; i++)
                {
                    char c = castling[i];
                    try {
                    switch (c)
                    {
                        case 'K':
                            if(tempBoard[7, 7] != null)
                                tempBoard[7, 7].hasMoved = false;
                            if(tempBoard[4, 7] != null)
                                tempBoard[4, 7].hasMoved = false;
                            break;
                        case 'Q':
                            if(tempBoard[0, 7] != null)
                                tempBoard[0, 7].hasMoved = false;
                            if(tempBoard[4, 7] != null)
                                tempBoard[4, 7].hasMoved = false;
                            break;
                        case 'k':
                            if(tempBoard[7, 0] != null)
                                tempBoard[7, 0].hasMoved = false;
                            if(tempBoard[4, 0] != null)
                                tempBoard[4, 0].hasMoved = false;
                            break;
                        case 'q':
                            if(tempBoard[0, 0] != null)
                                tempBoard[0, 0].hasMoved = false;
                            if(tempBoard[4, 0] != null)
                                tempBoard[4, 0].hasMoved = false;
                            break;
                    }
                    } catch (System.Exception e) {
                        Debug.Log(e);
                    }
                }
            }
        }

        Vector2Int enPeasantSquare = new Vector2Int(-1, -1);
        if(FENChunks.Length > 3 && FENChunks[3] != "-") {
            string enPeasant = FENChunks[3];
            enPeasantSquare = GetNotationCoordinates(enPeasant);
        }

        int halfMoveClock = 0;
        if(FENChunks.Length > 4)
            halfMoveClock = int.Parse(FENChunks[4]);

        int fullMoveNumber = 0;
        if(FENChunks.Length > 5)
            fullMoveNumber = int.Parse(FENChunks[5]);

        return new FENInfo(tempBoard, enPeasantSquare, turnColor, halfMoveClock, fullMoveNumber);


    }

    int fullMoveNumber = 0;
    int halfMoveClock = 0;

    public string GetFEN(ChessPiece[,] currentBoard) {
        //get FEN string from 2d array of chess pieces
        string fen = "";
        for (int i = 0; i < 8; i++)
        {
            int space = 0;
            for (int j = 0; j < 8; j++)
            {
                ChessPiece piece = currentBoard[j, i];
                if (piece == null)
                {
                    space++;
                }
                else
                {
                    if (space > 0)
                    {
                        fen += space;
                        space = 0;
                    }
                    switch (piece.type)
                    {
                        case ChessPieceType.Pawn:
                            fen += piece.color == ChessPieceColor.White ? "P" : "p";
                            break;
                        case ChessPieceType.Rook:
                            fen += piece.color == ChessPieceColor.White ? "R" : "r";
                            break;
                        case ChessPieceType.Knight:
                            fen += piece.color == ChessPieceColor.White ? "N" : "n";
                            break;
                        case ChessPieceType.Bishop:
                            fen += piece.color == ChessPieceColor.White ? "B" : "b";
                            break;
                        case ChessPieceType.Queen:
                            fen += piece.color == ChessPieceColor.White ? "Q" : "q";
                            break;
                        case ChessPieceType.King:
                            fen += piece.color == ChessPieceColor.White ? "K" : "k";
                            break;
                    }
                }
            }
            if (space > 0)
            {
                fen += space;
            }
            if (i < 7)
            {
                fen += "/";
            }
        }
        fen += " ";
        fen += turn == ChessPieceColor.White ? "w" : "b";
        fen += " ";
        string castling = "";

        //check each corner for a rook, if it is there, check if it has moved or the king has moved
        if (currentBoard[7, 7] != null && currentBoard[4, 7] != null && currentBoard[4, 7].type == ChessPieceType.King && (currentBoard[7, 7].type == ChessPieceType.Rook && !currentBoard[7, 7].hasMoved && !currentBoard[4, 7].hasMoved))
        {
            castling += "K";
        }
        if (currentBoard[0, 7] != null && currentBoard[4, 7] != null && currentBoard[4, 7].type == ChessPieceType.King && (currentBoard[0, 7].type == ChessPieceType.Rook && !currentBoard[0, 7].hasMoved && !currentBoard[4, 7].hasMoved))
        {
            castling += "Q";
        }
        if (currentBoard[7, 0] != null && currentBoard[4, 0] != null && currentBoard[4, 0].type == ChessPieceType.King && (currentBoard[7, 0].type == ChessPieceType.Rook && !currentBoard[7, 0].hasMoved && !currentBoard[4, 0].hasMoved))
        {
            castling += "k";
        }
        if (currentBoard[0, 0] != null && currentBoard[4, 0] != null && currentBoard[4, 0].type == ChessPieceType.King && (currentBoard[0, 0].type == ChessPieceType.Rook && !currentBoard[0, 0].hasMoved && !currentBoard[4, 0].hasMoved))
        {
            castling += "q";
        }
        if (castling == "")
        {
            castling = "-";
        }
        fen += castling;
        fen += " ";

        if (EnPeasantSquare.x == -1)
        {
            fen += "-";
        }
        else
        {
            fen += GetNotation(EnPeasantSquare.x, EnPeasantSquare.y);
        }

        fen += " ";
        fen += halfMoveClock;

        fen += " ";
        fen += fullMoveNumber;


        return fen;
    }


    //move, capture, check, checkmate

    public delegate void MoveEvent(ChessPieceColor color, int fromX, int fromY, int toX, int toY, bool promotion = false);
    public event MoveEvent OnMove;

    public delegate void CaptureEvent(ChessPieceColor color);
    public event CaptureEvent OnCapture;

    public delegate void CheckEvent(ChessPieceColor color);
    public event CheckEvent OnCheck;

    public delegate void CheckmateEvent(ChessPieceColor color);
    public event CheckmateEvent OnCheckmate;

    public delegate void StalemateEvent();
    public event StalemateEvent OnStalemate;
    //on turn end
    public delegate void TurnEndEvent(ChessPieceColor color);
    public event TurnEndEvent OnTurnEnd;

    //On Castle
    public delegate void CastleEvent(ChessPieceColor color, int side);
    public event CastleEvent OnCastle;




}

public enum MoveType
{
    Move,
    Capture,
    Castle,
    EnPassant,
    Promotion,
    Check,
    Checkmate,
    Stalemate,
    Invalid
}

public class ChessWindow : MonoBehaviour
{
    public ChessBoard board = null;

    private Texture2D whiteSquare;
    private Texture2D blackSquare;

    private Texture2D whitePawn;
    private Texture2D whiteRook;
    private Texture2D whiteKnight;
    private Texture2D whiteBishop;
    private Texture2D whiteQueen;
    private Texture2D whiteKing;

    private Texture2D blackPawn;
    private Texture2D blackRook;
    private Texture2D blackKnight;
    private Texture2D blackBishop;
    private Texture2D blackQueen;
    private Texture2D blackKing;

    private Texture2D circle;
    private Texture2D hollowCircle;

    private Texture2D white;

    private Texture2D triangle;

    private Texture2D closeButton;


    AudioClip moveSound;
    AudioClip captureSound;
    AudioClip checkSound;
    AudioClip checkmateSound;

    AudioClip castleSound;
    //AudioClip stalemateSound;

    TextAsset mateInFourPuzzles;
    TextAsset winningSacrificesPuzzles;
    TextAsset pinsPuzzles;

    Dictionary<string, string> puzzles = new Dictionary<string, string>();


    public UnityEvent OnOpen;
    public UnityEvent OnClose;

    float pieceScale = 0.9f;

    

    
    void LoadTextures() {
        
        whitePawn = Resources.Load<Texture2D>("chess/pieces/wp");
        whiteRook = Resources.Load<Texture2D>("chess/pieces/wr");
        whiteKnight = Resources.Load<Texture2D>("chess/pieces/wn");
        whiteBishop = Resources.Load<Texture2D>("chess/pieces/wb");
        whiteQueen = Resources.Load<Texture2D>("chess/pieces/wq");
        whiteKing = Resources.Load<Texture2D>("chess/pieces/wk");

        blackPawn = Resources.Load<Texture2D>("chess/pieces/bp");
        blackRook = Resources.Load<Texture2D>("chess/pieces/br");
        blackKnight = Resources.Load<Texture2D>("chess/pieces/bn");
        blackBishop = Resources.Load<Texture2D>("chess/pieces/bb");
        blackQueen = Resources.Load<Texture2D>("chess/pieces/bq");
        blackKing = Resources.Load<Texture2D>("chess/pieces/bk");

        circle = Resources.Load<Texture2D>("chess/circle");
        hollowCircle = Resources.Load<Texture2D>("chess/hollow_circle");

        closeButton = Resources.Load<Texture2D>("chess/close_button");

        //load sounds

        moveSound = Resources.Load<AudioClip>("chess/move");
        captureSound = Resources.Load<AudioClip>("chess/capture");
        checkSound = Resources.Load<AudioClip>("chess/check");
        checkmateSound = Resources.Load<AudioClip>("chess/checkmate");
        castleSound = Resources.Load<AudioClip>("chess/castle");

        //load puzzles
        mateInFourPuzzles = Resources.Load<TextAsset>("chess/mate_in_four");
        winningSacrificesPuzzles = Resources.Load<TextAsset>("chess/winning_sacrifices");
        pinsPuzzles = Resources.Load<TextAsset>("chess/pins");


        //create 1x1 white texture
        white = new Texture2D(1, 1);
        white.SetPixel(0, 0, Color.white);
        white.Apply();

        LoadPuzzles();
    }

    struct Arrow {
        public Vector2Int start;
        public Vector2Int end;
        public Color color;
        public Arrow(Vector2Int start, Vector2Int end, Color color) {
            this.start = start;
            this.end = end;
            this.color = color;
        }
    }

    List<Arrow> arrows = new List<Arrow>();

    private void LoadPuzzles() {
        /* mate in four puzzles file format
        [FEN]
        [Solution]
        [FEN]
        [Solution]
        */
        List<string> loadedPuzzles = new List<string>();
        loadedPuzzles.AddRange(mateInFourPuzzles.text.Split('\n'));
        loadedPuzzles.AddRange(winningSacrificesPuzzles.text.Split('\n'));
        loadedPuzzles.AddRange(pinsPuzzles.text.Split('\n'));

        string[] lines = string.Join("\n", loadedPuzzles).Split('\n');
        for(int i = 0; i < lines.Length; i += 2) {
            
            if(i == lines.Length - 1)
                break;

            string answer = lines[i + 1].Replace("  ", " ").Replace("   ", " ").Trim();
            if(!puzzles.ContainsKey(lines[i]))
                puzzles.Add(lines[i], answer);
            // string moveConversion = UpdateMoveFormat(lines[i], lines[i + 1]);
            // if(moveConversion != "") {
            //     puzzles.Add(lines[i], moveConversion);
            //      Debug.Log("Adding: " + lines[i] + " " + moveConversion);
            // }

           
        }
    }

    private string UpdateMoveFormat(string FEN, string movesStr) {
        string[] moves = movesStr.Split(' ');
        ChessBoard tempBoard = new ChessBoard();
        tempBoard.LoadFEN(FEN);
        List<string> trueMoves = new List<string>();
        foreach (string move in moves) {
            try {
            //if move length == 4 and the first letter is a lowercase letter, the second letter is a number, and the third letter is a lowercase letter and the fourth letter is a number
            if (move.Length == 4 && char.IsLower(move[0]) && char.IsDigit(move[1]) && char.IsLower(move[2]) && char.IsDigit(move[3])) {
                trueMoves.Add(move);
                continue;
            }

            string trueMove = tempBoard.PuzzleNotation(movesStr.Split(' ')[turnNumber], tempBoard.turn == ChessPieceColor.White ? ChessPieceColor.Black : ChessPieceColor.White);
            trueMoves.Add(trueMove);
            tempBoard.NotationMove(trueMove, tempBoard.turn);
            } catch (Exception e) {
               // Debug.Log(e);
                return "";
            }
        }

        //join moves with " "
        return string.Join(" ", trueMoves);

    }

    

    IStockfish stockfish;
    
    MoveType lastMoveType = MoveType.Invalid;

    
    Vector2Int lastMoveStart = new Vector2Int(-1, -1);
    Vector2Int lastMoveEnd = new Vector2Int(-1, -1);

    bool initialized = false;

    private bool m_DrawBoard = false;
    public bool DrawBoard
    {
        get { return m_DrawBoard; }
        set {
            if(m_DrawBoard==value) return;
            m_DrawBoard = value; 
            
            if(value) {
                OnOpen.Invoke();
            } else {
                OnClose.Invoke();
            }
        }
    }
    

    public void Initilize() {
        Initilize(true, 5);
    }

    public void Initilize(bool playAsWhite, int difficulty)
    {
        if(initialized) {
            DrawBoard = true;
            return;
        }

        DrawBoard = true;
        if(playAsWhite) {
            playerColor = ChessPieceColor.White;
            Debug.Log("Player is white");
        } else {
            playerColor = ChessPieceColor.Black;
        }

        //windows stockfish-win
        //mac stockfish-mac
        //linux stockfish-linux

        StartCoroutine(LoadBot(difficulty));

        board = new ChessBoard();
        LoadTextures();
        //board.LoadFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR");
        RestartGame();

        board.OnMove += (color, fromX, fromY, toX, toY, promotion) =>
        {
            if(lastMoveType == MoveType.Invalid)
                lastMoveType = MoveType.Move;

            lastMoveStart = new Vector2Int(fromX, fromY);
            lastMoveEnd = new Vector2Int(toX, toY);

            if(color == playerColor && playingPuzzles) {
                string playerMoveNotation = board.GetNotation(new Vector2Int(fromX, fromY)) + board.GetNotation(new Vector2Int(toX, toY));
                if(promotion) {
                    playerMoveNotation += "=" + board.TypeToLetter(board.GetPiece(toX,toY).type);
                }
                Debug.Log("Player move: " + playerMoveNotation);
                Debug.Log("Puzzle move: " + playerPuzzleMove + "("+playerPuzzleHint+")");
                if(playerMoveNotation != playerPuzzleMove && board.GetGameOutcome() == GameOutcome.InProgress) {
                    StartCoroutine(WrongMove());
                }
            }

            //AudioSource.PlayClipAtPoint(moveSound, Vector3.zero, 3f);
            
        };

        board.OnCapture += (color) =>
        {
            lastMoveType = MoveType.Capture;

            //AudioSource.PlayClipAtPoint(captureSound, Vector3.zero, 3f);
            
        };

        board.OnCastle += (color, side) =>
        {
            Debug.Log("Castle");
            lastMoveType = MoveType.Castle;
            //AudioSource.PlayClipAtPoint(checkSound, Vector3.zero, 3f);
        };

        board.OnCheck += (color) =>
        {
            Debug.Log("Check");
            lastMoveType = MoveType.Check;
            //AudioSource.PlayClipAtPoint(checkSound, Vector3.zero, 3f);
        };

        board.OnCheckmate += (color) =>
        {   
            lastMoveType = MoveType.Checkmate;
            AudioSource.PlayClipAtPoint(checkmateSound, Vector3.zero, 3f);
            
        };

        board.OnStalemate += () =>
        {
            lastMoveType = MoveType.Stalemate;
            AudioSource.PlayClipAtPoint(checkmateSound, Vector3.zero, 3f);
        };


        board.OnTurnEnd += (color) =>
        {
            // if(board.InCheckmate()) {
            //     AudioSource.PlayClipAtPoint(checkmateSound, Vector3.zero, 3f);
            // }
            if(lastMoveType == MoveType.Checkmate) {
                Debug.Log("Checkmate");
                //AudioSource.PlayClipAtPoint(checkmateSound, Vector3.zero, 3f);
            } else if (lastMoveType == MoveType.Check) {
                Debug.Log("Check");
                AudioSource.PlayClipAtPoint(checkSound, Vector3.zero, 3f);
            } else if (lastMoveType == MoveType.Capture) {
                Debug.Log("Capture");
                AudioSource.PlayClipAtPoint(captureSound, Vector3.zero, 3f);
            } else if (lastMoveType == MoveType.Castle) {
                Debug.Log("Castle");
                AudioSource.PlayClipAtPoint(castleSound, Vector3.zero, 3f);
            }else if (lastMoveType == MoveType.Move) {
                Debug.Log("Move");
                AudioSource.PlayClipAtPoint(moveSound, Vector3.zero, 3f);
            } else if (lastMoveType == MoveType.Invalid) {
                Debug.Log("Invalid");
            }
            lastMoveType = MoveType.Invalid;

            hintPos = new Vector2Int(-1, -1);
            hintPos2 = new Vector2Int(-1, -1);
            

            

            
            if(!wrongMove) {
                turnNumber++;
            }

            if(((turnNumber % 2) == 1) && playerColor == board.turn) {
                turnNumber++;
            }

            if(playingPuzzles && turnNumber >= puzzles[puzzleFEN].Split(' ').Length) {
                Debug.Log("Puzzle Solved");
                //should eventually be puzzle solved
                board.gameOver=true;
                //RandomPuzzle();
                return;
            }

            

            //create a thread to run the bot move
            if(color != playerColor) {
                

                if(playingPuzzles && !wrongMove) {
                    StartCoroutine(PuzzleMove());
                
                }
                    
                else
                    StartCoroutine(BotMove());
                

                
                
            } else {
                if(playingPuzzles && !wrongMove) {
                    try {
                    playerPuzzleMove = board.PuzzleNotation(puzzles[puzzleFEN].Split(' ')[turnNumber], playerColor==ChessPieceColor.White? ChessPieceColor.Black : ChessPieceColor.White);
                    } catch {
                        board.gameOver=true;
                        return;
                    }
                    playerPuzzleHint = puzzles[puzzleFEN].Split(' ')[turnNumber];
                }
                //if move was right
                //if(puzzles[puzzleFEN])
            }
            if(!wrongMove) {
                lastTurnFEN = board.GetFEN(board.GetBoard());
            }
            
            
        };

        
        initialized = true;
    }

    bool wrongMove = false;

    string playerPuzzleMove = "";
    string playerPuzzleHint = "";

    string lastTurnFEN = "";

    int turnNumber = 0;

    public bool playingPuzzles = false;
    string puzzleFEN = "";

    int gameNumber = 0;
    IEnumerator PuzzleMove() {
        if(!wrongMove) {
            bool worked = false;
            ChessPieceColor otherColor = playerColor == ChessPieceColor.White ? ChessPieceColor.Black : ChessPieceColor.White;
            string move = "";
            try {
                move = board.PuzzleNotation(puzzles[puzzleFEN].Split(' ')[turnNumber], otherColor);
                worked = true;
            } catch(Exception e) { 
                
                //find the forced move
                foreach (ChessPiece piece in board.GetBoard()) {
                    if(piece.color == otherColor) {
                        List<Vector2Int> moves = board.GetLegalMoves(piece);
                        if(moves.Count > 0) {
                            move = board.GetNotation(piece.x, piece.y) + board.GetNotation(moves[0]);
                            worked = true;
                            turnNumber--;
                            break;
                        }
                    }
                }
            }
            if(!worked) {
                board.gameOver=true;
                // bool w = false;
                // Debug.Log("Getting best move. Alt other");
                // GetBestMove((move) => {board.NotationMove(move, otherColor);});
                // yield return new WaitUntil(() => w);
                // Debug.Log("w: " + w);

                //playerPuzzleHint = playerPuzzleMove;
            } else {
                yield return new WaitForSeconds(0.5f);
                board.NotationMove(move, otherColor);
            }
            
        }
            
        

    }

    public string GetAllPuzzleMoves() {
        return puzzles[puzzleFEN];
    }

    IEnumerator WrongMove() {
        wrongMove= true;
        //set tile color to red and fade it back to normal
        Color originalLightColor = lightTile;
        Color originalDarkColor = darkTile;

        lightTile = new Color(1f, 0f, 0f, 1f);
        darkTile = new Color(0.75f, 0f, 0f, 1f);

        //fade back to normal
        for (float f = 0f; f <= 1f; f += 0.01f) {
            lightTile = Color.Lerp(lightTile, originalLightColor, f);
            darkTile = Color.Lerp(darkTile, originalDarkColor, f);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(0.5f);

        int turnNumberTemp = turnNumber;
        SetFEN(lastTurnFEN);
        turnNumber = turnNumberTemp--;
        wrongMove = false;
        

    }

    IEnumerator BotMove() {
        yield return new WaitUntil(() => botLoaded);
        int initialGameNumber = gameNumber;
        string move = "t";
        Thread thread = new Thread(() => {
            stockfish.SetFenPosition(board.GetFEN(board.GetBoard()));
            move = stockfish.GetBestMoveTime(500);
            Debug.Log("Move: "+ move);
        });
        thread.Start();
        yield return new WaitUntil(() => move != "t");
        Debug.Log("Done waiting");
        if(initialGameNumber == gameNumber)
            board.NotationMove(move, playerColor == ChessPieceColor.White ? ChessPieceColor.Black : ChessPieceColor.White);
        
        legalMoves = board.GetLegalMoves(selectedPiece);
        yield return null;
    }

    public void GetHint(Action<string> callback) {
        if(playingPuzzles) {
            callback(playerPuzzleHint);
            return;
        }
        StartCoroutine(GetBestMove(callback));
    }
    private IEnumerator GetBestMove(Action<string> callback) {
        yield return new WaitUntil(() => botLoaded);
        Debug.Log("Getting best move");
        string move = "t";
        int initialSkill = stockfish.SkillLevel;
        stockfish.SkillLevel = 20;
        Thread thread = new Thread(() => {
            stockfish.SetFenPosition(board.GetFEN(board.GetBoard()));
            move = stockfish.GetBestMoveTime(500);
            Debug.Log("Move: "+ move);
        });
        thread.Start();
        yield return new WaitUntil(() => move != "t");
        stockfish.SkillLevel = initialSkill;
        callback(move);
        yield return null;
    }

    IEnumerator LoadBot(int difficulty) {
        bool loaded = false;
        //find folder in application data path called cengines1155
        string dataPath = FindFolder(Application.dataPath, "cengines1155");

        //if editor, use the folder in the project
        if(Application.isEditor) {
            //one folder up from assets in the build folder
            string buildPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("/"));
            dataPath = FindFolder(buildPath, "cengines1155");
        }
            
        
        
        Thread thread = new Thread(() => {
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    stockfish = new Stockfish.NET.Core.StockfishClass(dataPath+ "/stockfish-win.exe");
                    break;
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                    stockfish = new Stockfish.NET.Core.StockfishClass(dataPath + "/stockfish-mac");
                    break;
                case RuntimePlatform.LinuxEditor:
                case RuntimePlatform.LinuxPlayer:
                    stockfish = new Stockfish.NET.Core.StockfishClass(dataPath + "/stockfish-linux");
                    break;
            }

            stockfish.SkillLevel = difficulty;
            loaded = true;
        });
        thread.Start();
        yield return new WaitUntil(() => loaded);
        botLoaded = true;
    }
    

    bool botLoaded = false;



    public string FindFolder(string path, string folderName)
    {
        // Check if the current directory contains the target folder
        string targetFolderPath = Path.Combine(path, folderName);
        if (Directory.Exists(targetFolderPath))
        {
            return targetFolderPath;
        }

        // Recursively search within each subdirectory
        string[] subdirectories = Directory.GetDirectories(path);
        foreach (string subdirectory in subdirectories)
        {
            string result = FindFolder(subdirectory, folderName);
            if (result != null)
            {
                return result;
            }
        }

        // Target folder not found
        return null;
    }

    

    public void SetChessPuzzle(string FEN, string solution) {
        stockfish.SkillLevel = 20;
        SetFEN(FEN, true);
        puzzleFEN = FEN;
        playingPuzzles = true;
        turnNumber = 0;
        playerPuzzleMove = board.PuzzleNotation(puzzles[puzzleFEN].Split(' ')[turnNumber], playerColor==ChessPieceColor.White? ChessPieceColor.Black : ChessPieceColor.White);
        playerPuzzleHint = puzzles[puzzleFEN].Split(' ')[turnNumber];
    }


    


    public void RandomPuzzle(int difficulty) {
        stockfish.SkillLevel = 20;
        int randomPuzzle = UnityEngine.Random.Range(0, puzzles.Count);
        string puzzleFEN = puzzles.Keys.ElementAt(randomPuzzle);
        SetChessPuzzle(puzzleFEN, "");
    }

    public void RandomPuzzle() {
        RandomPuzzle(5);
    }


    //Get Set fen
    public string GetFEN() {
        return board.GetFEN(board.GetBoard());
    }

     public void SetFEN(string fen, bool isPuzzle = false) {

        lastTurnFEN = fen;
        turnNumber = 0;
        board.LoadFEN(fen);

        hintPos = new Vector2Int(-1, -1);
        hintPos2 = new Vector2Int(-1, -1);

        


        board.gameOver = false;
        board.gameOutcome = GameOutcome.InProgress;
        lastMoveType = MoveType.Invalid;
        gameStarted = true;
       
        lastMoveStart = new Vector2Int(-1, -1);
        lastMoveEnd = new Vector2Int(-1, -1);


        legalMoves = new List<Vector2Int>();
        selectedPiece = null;
        gameNumber++;


        if(isPuzzle) 
            playerColor = board.turn;

        boardFlipped = playerColor == ChessPieceColor.Black;

        if(board.turn != playerColor) {
            if(isPuzzle)
                StartCoroutine(PuzzleMove());
            else
                StartCoroutine(BotMove());
            //Debug.Log("Bot move. Set FEN");
            
        }
       
    }

    ChessPieceColor playerColor = ChessPieceColor.White;


    string startingFEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 0";//"2Q3B1/k7/8/2K5/8/8/P7/8 w - - 0 0";

    bool isMousePressed = false;
    

    bool movingBoard = false;

    public float margin = 50f;
    
    Vector2 initialMousePos = Vector2.zero;
    Vector2 initialOffset = Vector2.zero;

    ChessPiece selectedPiece = null;

    List<Vector2Int> legalMoves = new List<Vector2Int>();

    public void MysteryMode() {
        int random = UnityEngine.Random.Range(0, 4);
        switch (random) {
            case 0:
                screenSaverMode = true;
                break;
            case 1:
                rainbow = true;
                break;
            case 2:
                pieceScale = UnityEngine.Random.Range(0.5f, 1.5f);
                break;
            case 3:
                pieceScale = UnityEngine.Random.Range(0.5f, 1.5f);
                screenSaverMode = true;
                rainbow = true;
                break;
        }

    }

    public void NormalMode() {
        screenSaverMode = false;
        rainbow = false;
        pieceScale = 0.9f;
        ResetTileColors();
    }


    void RestartGame() {
        Debug.Log("Restarting Game");
        
        board.gameOver = false;
        board.gameOutcome = GameOutcome.InProgress;
        lastMoveType = MoveType.Invalid;

        //set the player color to random
        if(gameNumber != 0)
            playerColor = (ChessPieceColor)UnityEngine.Random.Range(0, 2);

        boardFlipped = playerColor == ChessPieceColor.Black;
        lastMoveStart = new Vector2Int(-1, -1);
        lastMoveEnd = new Vector2Int(-1, -1);

        legalMoves = new List<Vector2Int>();
        selectedPiece = null;
        
        SetFEN(playingPuzzles? puzzleFEN : startingFEN, playingPuzzles);
        
        gameNumber++;
        turnNumber = 0;
        if(playingPuzzles) {
            playerPuzzleMove = board.PuzzleNotation(puzzles[puzzleFEN].Split(' ')[turnNumber], playerColor==ChessPieceColor.White? ChessPieceColor.Black : ChessPieceColor.White);
            playerPuzzleHint = puzzles[puzzleFEN].Split(' ')[turnNumber];
        }

        hintPos = new Vector2Int(-1, -1);
        hintPos2 = new Vector2Int(-1, -1);
    }

    bool boardFlipped = false;

    bool gameStarted = false;

    void Update() {
        if(!initialized || !DrawBoard)
            return;



        if(UnityEngine.Input.GetKeyDown(KeyCode.F)) {
            boardFlipped = !boardFlipped;
            Debug.Log("Flipped: " + boardFlipped);
        }
        margin = board.squareSize;

        if(screenSaverMode) {
            //move board based on velocity if hit edge of screen reverse velocity
            board.xOffset += screenSaverVelocity.x * Time.deltaTime * 50f;
            board.yOffset += screenSaverVelocity.y* Time.deltaTime * 50f;

            if(board.xOffset + board.squareSize * 8f > Screen.width) {
                screenSaverVelocity.x = -1;
            }//- margin/4f
            if(board.xOffset  < 0) {
                screenSaverVelocity.x = 1;
            } 
            if(board.yOffset + board.squareSize * 8f > Screen.height) {
                screenSaverVelocity.y = -1;
            }
            if(board.yOffset < 0) {
                screenSaverVelocity.y = 1;
            }

            
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.H)&&playingPuzzles) {
            Debug.Log("Hint");
            if(hintPos == new Vector2Int(-1, -1)) {
                hintPos = board.GetNotationCoordinates(playerPuzzleMove.Substring(0, 2));
            } else {
                hintPos2 = board.GetNotationCoordinates(playerPuzzleMove.Substring(2, 2));
            }

            Debug.Log("Hint: " + hintPos + " " + hintPos2);
        }

        if(playingPuzzles && !wrongMove && playerPuzzleMove.Length < 2) {
            playerPuzzleMove = board.PuzzleNotation(puzzles[puzzleFEN].Split(' ')[turnNumber], playerColor == ChessPieceColor.White ? ChessPieceColor.Black : ChessPieceColor.White);
            playerPuzzleHint = puzzles[puzzleFEN].Split(' ')[turnNumber];
            Debug.Log("Puzzle Move: " + playerPuzzleMove);
        }

    }


    bool mouseInPromotion = false;

    bool promotionActive = false;
    Vector2Int promotionTile = Vector2Int.zero;

    bool promotionSelelected = false;
    ChessPieceType promotionPiece = ChessPieceType.Queen;

    ChessPiece promotionPieceObject = null;
    Vector2Int promotionPieceTargetPos = Vector2Int.zero;

    void PromotionGUI(int x, int y) {
        //Set button style background to solid
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.normal.background = white;
        buttonStyle.hover.background = white;

        //create a box that has 4 buttons for the 4 pieces
        //if a button is pressed then set promotion piece to that piece
        //if right or escape is pressed then exit
        Rect promotionTile = GetTile(x, y);
        float popupMargin = margin/2f;
        Rect promotionBox = new Rect(promotionTile.x + promotionTile.width/2, promotionTile.y, promotionTile.width*4+popupMargin*5, promotionTile.height+popupMargin*2);
        //color dark black 0.9 opacity
        GUI.color = new Color(0f, 0f, 0f, 0.9f);
        GUI.Box(promotionBox, "", buttonStyle);
        GUI.color = Color.white;

        if(promotionBox.Contains(Event.current.mousePosition)) {
            mouseInPromotion = true;
        } else {
            mouseInPromotion = false;
        }

        

        Rect queenButton = new Rect(promotionTile.x + promotionTile.width/2+popupMargin, promotionTile.y+popupMargin, promotionTile.width, promotionTile.height);
        Rect rookButton = new Rect(promotionTile.x + promotionTile.width/2+popupMargin*2+promotionTile.width, promotionTile.y+popupMargin, promotionTile.width, promotionTile.height);
        Rect bishopButton = new Rect(promotionTile.x + promotionTile.width/2+popupMargin*3+promotionTile.width*2, promotionTile.y+popupMargin, promotionTile.width, promotionTile.height);
        Rect knightButton = new Rect(promotionTile.x + promotionTile.width/2+popupMargin*4+promotionTile.width*3, promotionTile.y+popupMargin, promotionTile.width, promotionTile.height);

        Texture2D queenTexture;
        if(playerColor == ChessPieceColor.White)
            queenTexture = whiteQueen;
        else
            queenTexture = blackQueen;
        if(GUI.Button(queenButton, queenTexture, buttonStyle)) {
            Debug.Log("Promoting to Queen");
            promotionPiece = ChessPieceType.Queen;
            promotionSelelected = true;
            promotionActive = false;
            board.MovePiece(promotionPieceObject, promotionPieceTargetPos.x, promotionPieceTargetPos.y, promotionPiece);
            selectedPiece = null;
            legalMoves = new List<Vector2Int>();
            mouseInPromotion = false;
            Debug.Log("Promotion Piece: " + promotionPiece);
            
        }

        Texture2D rookTexture;
        if(playerColor == ChessPieceColor.White)
            rookTexture = whiteRook;
        else
            rookTexture = blackRook;
        if(GUI.Button(rookButton, rookTexture, buttonStyle)) {
            promotionPiece = ChessPieceType.Rook;
            promotionSelelected = true;
            promotionActive = false;
            board.MovePiece(promotionPieceObject, promotionPieceTargetPos.x, promotionPieceTargetPos.y, promotionPiece);
            selectedPiece = null;
            legalMoves = new List<Vector2Int>();
            mouseInPromotion = false;
        }

        Texture2D bishopTexture;
        if(playerColor == ChessPieceColor.White)
            bishopTexture = whiteBishop;
        else
            bishopTexture = blackBishop;
        if(GUI.Button(bishopButton, bishopTexture, buttonStyle)) {
            promotionPiece = ChessPieceType.Bishop;
            promotionSelelected = true;
            promotionActive = false;
            board.MovePiece(promotionPieceObject, promotionPieceTargetPos.x, promotionPieceTargetPos.y, promotionPiece);
            selectedPiece = null;
            legalMoves = new List<Vector2Int>();
            mouseInPromotion = false;
        }

        Texture2D knightTexture;
        if(playerColor == ChessPieceColor.White)
            knightTexture = whiteKnight;
        else
            knightTexture = blackKnight;
        if(GUI.Button(knightButton, knightTexture, buttonStyle)) {
            promotionPiece = ChessPieceType.Knight;
            promotionSelelected = true;
            promotionActive = false;
            board.MovePiece(promotionPieceObject, promotionPieceTargetPos.x, promotionPieceTargetPos.y, promotionPiece);
            selectedPiece = null;
            legalMoves = new List<Vector2Int>();
            mouseInPromotion = false;
        }

        GUI.color = Color.white;




    }

    public void Close() {
        DrawBoard = false;
    }

    Color defaultLightTile = new Color(235f/255f, 236f/255f, 208f/255f);
    Color defaultDarkTile = new Color(119f/255f, 149f/255f, 86f/255f);

    public bool rainbow = false;

    Color lightTile = new Color(235f/255f, 236f/255f, 208f/255f);
    Color darkTile = new Color(119f/255f, 149f/255f, 86f/255f);

    public void ResetTileColors() {
        lightTile = defaultLightTile;
        darkTile = defaultDarkTile;
    }

    public bool screenSaverMode = false;
    Vector2 screenSaverVelocity = Vector2.one;

    Vector2Int hintPos = new Vector2Int(-1, -1);
    Vector2Int hintPos2 = new Vector2Int(-1, -1);
    
    Color hintDarkColor = new Color(73f/255f, 161f/255f, 126f/255f, 1f);
    Color hintLightColor = new Color(133f/255f, 205f/255f, 188f/255f, 1f);
    void OnGUI()
    {

        if(!initialized || !DrawBoard)
            return;

        if(rainbow) {
            //get rainbow spectrum with sin wave
            float r = Mathf.Sin(Time.time * 0.5f) * 0.5f + 0.5f;
            float g = Mathf.Sin(Time.time * 0.5f + 2) * 0.5f + 0.5f;
            float b = Mathf.Sin(Time.time * 0.5f + 4) * 0.5f + 0.5f;
            lightTile = new Color(r, g, b);

            r = Mathf.Sin(Time.time * 0.5f + 1) * 0.5f + 0.5f;
            g = Mathf.Sin(Time.time * 0.5f + 3) * 0.5f + 0.5f;
            b = Mathf.Sin(Time.time * 0.5f + 5) * 0.5f + 0.5f;
            darkTile = new Color(r, g, b);
        }



        
        //Dont know why this is needed but it is. Otherwise the game thinks the player is always white
        

        bool gameOver = board.gameOver;

        //check if f pressed and then print fen
        if (UnityEngine.Input.GetKey(KeyCode.LeftControl) && UnityEngine.Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log(board.GetFEN(board.GetBoard()));
        }

        

        //Debug.Log("Board Flipped: " + boardFlipped);
        

        bool mousePressedFrame = false;
        bool mouseReleasedFrame = false;
        if (UnityEngine.Input.GetMouseButtonDown(0) && !isMousePressed)
        {
            isMousePressed = true;
            mousePressedFrame = true;
        } else if (UnityEngine.Input.GetMouseButtonUp(0) && isMousePressed)
        {
            isMousePressed = false;
            mouseReleasedFrame = true;
        }

        //check if cntrl + r is pressed and reset the board
        if (UnityEngine.Input.GetKey(KeyCode.LeftControl) && UnityEngine.Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }

        Vector2 mousePos = new Vector2(UnityEngine.Input.mousePosition.x, Screen.height - UnityEngine.Input.mousePosition.y);

        // Create a surrounding box
        float boxSize = board.squareSize * 8;
        Vector2 boxPos = new Vector2(board.xOffset, board.yOffset);
        Rect boxRect = new Rect(boxPos.x, boxPos.y, boxSize, boxSize);
        Rect borderRect = new Rect(boxPos.x - margin / 2f, boxPos.y - margin / 2f, boxSize + margin, boxSize + margin);
        GUI.Box(borderRect, "");

        //close button
        Rect closeButtonRect = new Rect(borderRect.x+borderRect.width- margin*0.275f, borderRect.y + margin*0.125f, margin*0.25f, margin*0.25f);
        GUI.DrawTexture(closeButtonRect, closeButton);
        if(closeButtonRect.Contains(mousePos) && mousePressedFrame) {
            //are you sure you want to quit? warning
            Close();
        }

        // Variables to store initial mouse position and board offset


        // Detect if the mouse is over the border and inside the box
        if (borderRect.Contains(mousePos) && !boxRect.Contains(mousePos))
        {
            // Check if mouse button is pressed
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                movingBoard = true;
                initialMousePos = mousePos; // Store initial mouse position
                initialOffset = new Vector2(board.xOffset, board.yOffset); // Store initial board offset
            }


        }

        if (UnityEngine.Input.GetMouseButton(0) && movingBoard)
        {
            Vector2 delta = mousePos - initialMousePos; // Calculate the delta between current and initial mouse position
            board.xOffset = initialOffset.x + delta.x;
            board.yOffset = initialOffset.y + delta.y;
        }
        else if (UnityEngine.Input.GetMouseButtonUp(0))
        {
            movingBoard = false;
        }

        

        if(boxRect.Contains(mousePos) && !gameStarted) {
            
            gameStarted=true;

            // if(playerColor == ChessPieceColor.Black) {
            //     Debug.Log("Bot Move. Game Started");
            //     StartCoroutine(BotMove());
            // }
        }

        

        

        //draw chess board
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++) {
                int trueX = boardFlipped?(7-x):x;
                 int trueY = boardFlipped?(7-y):y;
                 
                int color = (x + y) % 2;
                if ((x + y) % 2 == 0)
                {
                    GUI.color = lightTile;
                }
                else
                {
                    GUI.color = darkTile;
                }
                if((lastMoveEnd.x == trueX && lastMoveEnd.y == trueY) || (lastMoveStart.x == trueX && lastMoveStart.y == trueY)) {
                    if(color == 1) {
                        GUI.color = new Color(((187f/119f)*darkTile.r*255f)/255f, ((203f/149f)*darkTile.g*255f)/255f, ((86f/208f)*darkTile.b*255f)/255f);
                        //GUI.color = new Color(187f/255f, 203f/255f, 43f/255f);
                    } else {
                        GUI.color = new Color(((247f/235f)*lightTile.r*255f)/255f, ((247f/236f)*lightTile.g*255f)/255f, ((105f/208f)*lightTile.b*255f)/255f);
                        //GUI.color = new Color(247f/255f, 247f/255f, 105f/255f);
                    }
                }

                bool selectedTile = false;

                if(selectedPiece != null && selectedPiece.x == trueX && selectedPiece.y == trueY) {
                    selectedTile = true;
                    if(color == 0) {
                        GUI.color = new Color(247f/255f, 247f/255f, 105f/255f);
                        
                    } else {
                        GUI.color = new Color(187f/255f, 203f/255f, 43f/255f);
                    }
                    
                    //GUI.DrawTexture(GetTile(selectedPiece), Texture2D.whiteTexture);
                    
                }

                float multiplier = selectedTile?1.5f:1f;
                if(new Vector2Int(trueX, trueY) == hintPos) {
                    
                    if(color ==1) {
                        GUI.color = hintDarkColor * multiplier;
                    } else {
                        GUI.color = hintLightColor * multiplier;
                    }
                } else if (new Vector2Int(trueX, trueY) == hintPos2) {
                    if(color ==1) {
                        GUI.color = hintDarkColor * multiplier;
                    } else {
                        GUI.color = hintLightColor * multiplier;
                    }
                }
                Rect tile = new Rect(x * board.squareSize + board.xOffset, y * board.squareSize + board.yOffset, board.squareSize, board.squareSize);
                GUI.DrawTexture(tile, Texture2D.whiteTexture);

               


                

                if(gameOver)
                    continue;

                //draw a circle if the tile is a legal move
                if(legalMoves.Contains(new Vector2Int(trueX, trueY))) {
                    GUI.color = new Color(0.3f, 0.3f, 0.3f, 0.5f);
                    if(board.GetBoard()[trueX, trueY] != null) {
                        GUI.DrawTexture(tile, hollowCircle);
                    } else {
                        Rect circleRect = new Rect(tile.x,  tile.y, tile.width, tile.height);
                        //scale the circle so that it is half the size of the tile
                        circleRect.x += circleRect.width / 3f;
                        circleRect.y += circleRect.height / 3f;
                        circleRect.width /= 3f;
                        circleRect.height /= 3f;
                        GUI.DrawTexture(circleRect, circle);
                    }
                    GUI.color = Color.white;
                    
                }


                
                
                //Detect if the mouse is over the tile
                if (tile.Contains(mousePos) && mousePressedFrame && !mouseInPromotion)
                {
                    Debug.Log("Clicked on tile " + x + ", " + y);
                    if(selectedPiece == null || !legalMoves.Contains(new Vector2Int(trueX,trueY))) {
                        selectedPiece = board.GetPiece(trueX, trueY);
                        promotionActive = false;
                        if(selectedPiece != null && selectedPiece.color != playerColor) {
                            selectedPiece = null;
                            legalMoves = new List<Vector2Int>();
                        }
                        legalMoves = board.GetLegalMoves(selectedPiece);
                    } else {
                        if(legalMoves.Contains(new Vector2Int(trueX, trueY)) && board.turn == playerColor ) {
                            bool isPromotion = board.IsPromotion(selectedPiece, trueX, trueY);
                            if(!isPromotion && board.MovePiece(selectedPiece, trueX, trueY)) {
                                Debug.Log("Moved piece");
                                gameStarted = true;
                                legalMoves = new List<Vector2Int>();
                                selectedPiece = null;
                                promotionActive = false;
                            }
                            if(isPromotion && !promotionActive) {
                                promotionActive = true;
                                promotionTile = new Vector2Int(trueX, trueY);
                                promotionPieceObject = selectedPiece;
                                promotionPieceTargetPos = new Vector2Int(trueX, trueY);
                            }
                            // if(board.IsPromotion(selectedPiece, x, trueY)) {
                            //     Debug.Log("Promotion");
                            //     //show promotion menu
                            //     promotionMenu = true;
                            //     promotionX = trueX;
                            //     promotionY = trueY;
                            // } else {
                            //     Debug.Log("Moved piece");
                            //     gameStarted = true;
                            //     legalMoves = new List<Vector2Int>();
                            //     selectedPiece = null;
                            //     board.MovePiece(selectedPiece, trueX, trueY);
                            // }
                            
                        }
                    }
                }

            }
        }

        

        

        GUI.color = Color.white;

        //draw pieces
        ChessPiece[,] pieces = board.GetBoard();
        for (int x = 0; x < 8; x++)
        {
            //int y = (boardFlipped?7:0); ((!boardFlipped && y < 8) || (boardFlipped && y >= 0)); y+=(boardFlipped?-1:1)
            for (int y = 0; y < 8; y++) {
                ChessPiece piece = pieces[x, y];
                if (piece != null)
                {
                    Texture2D texture = null;
                    switch (piece.type)
                    {
                        case ChessPieceType.Pawn:
                            if (piece.color == ChessPieceColor.White)
                            {
                                texture = whitePawn;
                            }
                            else
                            {
                                texture = blackPawn;
                            }
                            break;
                        case ChessPieceType.Rook:
                            if (piece.color == ChessPieceColor.White)
                            {
                                texture = whiteRook;
                            }
                            else
                            {
                                texture = blackRook;
                            }
                            break;
                        case ChessPieceType.Knight:
                            if (piece.color == ChessPieceColor.White)
                            {
                                texture = whiteKnight;
                            }
                            else
                            {
                                texture = blackKnight;
                            }
                            break;
                        case ChessPieceType.Bishop:
                            if (piece.color == ChessPieceColor.White)
                            {
                                texture = whiteBishop;
                            }
                            else
                            {
                                texture = blackBishop;
                            }
                            break;
                        case ChessPieceType.Queen:
                            if (piece.color == ChessPieceColor.White)
                            {
                                texture = whiteQueen;
                            }
                            else
                            {
                                texture = blackQueen;
                            }
                            break;
                        case ChessPieceType.King:
                            if (piece.color == ChessPieceColor.White)
                            {
                                texture = whiteKing;
                            }
                            else
                            {
                                texture = blackKing;
                            }
                            break;
                    }
                    //pieceScale
                    float trueX = (boardFlipped?(7-x):x) * board.squareSize+board.xOffset;
                    float trueY = (boardFlipped?(7-y):y) * board.squareSize+board.yOffset;
                    float pieceSize = (board.squareSize * pieceScale);
                    Rect pieceRect = new Rect(trueX + (board.squareSize - pieceSize) / 2f, trueY + (board.squareSize - pieceSize) / 2f, pieceSize, pieceSize);
                    
                    GUI.DrawTexture(pieceRect, texture);
                }
            }
            
        }

        if(UnityEngine.Input.GetMouseButtonDown(1) && !gameOver) {
            selectedPiece = null;
            legalMoves = new List<Vector2Int>();
        }


        foreach (Arrow arrow in arrows)
        {
            int trueStartX = (boardFlipped?(7-arrow.start.x):arrow.start.x);
            int trueStartY = (boardFlipped?(7-arrow.start.y):arrow.start.y);
            int trueEndX = (boardFlipped?(7-arrow.end.x):arrow.end.x);
            int trueEndY = (boardFlipped?(7-arrow.end.y):arrow.end.y);

            Vector2Int start = new Vector2Int(trueStartX, trueStartY);
            Vector2Int end = new Vector2Int(trueEndX, trueEndY);

            
            

            
        }
            

        if(promotionActive) {
            PromotionGUI(boardFlipped?(7-promotionTile.x):promotionTile.x, boardFlipped?(7-promotionTile.y):promotionTile.y);
            //if escape or right click is pressed, cancel promotion
            if(UnityEngine.Input.GetKeyDown(KeyCode.Escape) || UnityEngine.Input.GetMouseButtonDown(1)) {
                promotionActive = false;
                promotionTile = new Vector2Int(-1, -1);
            }
        }

        //if game is over, create a banner that says who won and a button to restart the game
        if(gameOver) {
            GUIStyle textStyle = new GUIStyle(GUI.skin.label);
            textStyle.fontSize = 45;
            textStyle.fontStyle = FontStyle.Bold;
            textStyle.alignment = TextAnchor.MiddleCenter;
            textStyle.normal.textColor = Color.white;
            textStyle.hover.textColor = Color.white;

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.label);
            buttonStyle.fontSize = 35;
            buttonStyle.fontStyle = FontStyle.Bold;
            buttonStyle.alignment = TextAnchor.MiddleCenter;
            buttonStyle.normal.textColor = Color.white;
            buttonStyle.hover.textColor = new Color(0.8f, 0.8f, 0.8f, 1f);

            GUI.color = new Color(0.3f, 0.3f, 0.3f, 0.65f);
            GUI.DrawTexture(boxRect, Texture2D.whiteTexture);
            GUI.color = Color.white;
            string message = "";
            //board.GetOutcome()
            if (board.GetOutcome() == GameOutcome.Stalemate)
            {
                message += "Game Over\nStalemate.";
            }
            else if (board.GetOutcome() == GameOutcome.WhiteWin)
            {
                message += "Game Over\nWhite Wins!";
            }
            else if (board.GetOutcome() == GameOutcome.BlackWin)
            {
                message += "Game Over\nBlack Wins!";
            } else if(board.GetOutcome() == GameOutcome.InProgress && playingPuzzles) {
                message += "Puzzle Solved!";
            }

            //Win rect is the boxRect with half the width
            Rect winRect = new Rect(boxRect.x, boxRect.y, boxRect.width, boxRect.height/2f);
            Rect restartButtonRect = new Rect(boxRect.x+ boxRect.width/3f, boxRect.y+boxRect.height/3f*1.8f, boxRect.width/3f, boxRect.height/7.5f);
            Rect puzzleButtonRect = new Rect(boxRect.x+ boxRect.width/4f, boxRect.y+boxRect.height/3f*2.3f, boxRect.width/2f, boxRect.height/7.5f);

            GUI.Label(winRect, message, textStyle);
            if(GUI.Button(restartButtonRect, "Restart", buttonStyle)) {
                RestartGame();
            }

            if(playingPuzzles && GUI.Button(puzzleButtonRect, "Next Puzzle", buttonStyle)) {
                RandomPuzzle();
            }
        }

        
        
    }

    public Rect GetTile(ChessPiece piece) {
        return GetTile(boardFlipped?7-piece.x:piece.x, boardFlipped?7-piece.y:piece.y);
    }

    public Rect GetTile(int x, int y) {
        return new Rect(x * board.squareSize + board.xOffset, y * board.squareSize + board.yOffset, board.squareSize, board.squareSize);
    }
    
    
}
