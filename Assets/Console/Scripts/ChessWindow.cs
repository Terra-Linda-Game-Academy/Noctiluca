using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stockfish;
using Stockfish.NET;
using System.Threading;

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
    public float xOffset,yOffset = 0f;

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

    public Vector2Int[] GetPieceMoves(ChessPiece piece) {
        List<Vector2Int> moves = new List<Vector2Int>();
        switch (piece.type)
        {
            case ChessPieceType.Pawn:
                moves.AddRange(GetPawnMoves(piece));
                break;
            case ChessPieceType.Rook:
                moves.AddRange(GetRookMoves(piece));
                break;
            case ChessPieceType.Knight:
                moves.AddRange(GetKnightMoves(piece));
                break;
            case ChessPieceType.Bishop:
                moves.AddRange(GetBishopMoves(piece));
                break;
            case ChessPieceType.Queen:
                moves.AddRange(GetQueenMoves(piece));
                break;
            case ChessPieceType.King:
                moves.AddRange(GetKingMoves(piece));
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

    public Vector2Int[] GetPawnMoves(ChessPiece piece)
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
        return moves.ToArray();
    }

    public Vector2Int[] GetRookMoves(ChessPiece piece)
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

    public Vector2Int[] GetKnightMoves(ChessPiece piece)
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

    public Vector2Int[] GetBishopMoves(ChessPiece piece)
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

    public Vector2Int[] GetQueenMoves(ChessPiece piece)
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        moves.AddRange(GetBishopMoves(piece));
        moves.AddRange(GetRookMoves(piece));
        return moves.ToArray();
    }

    public Vector2Int[] GetKingMoves(ChessPiece piece)
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
        ChessPiece king = null;
        foreach (ChessPiece piece in currentBoard)
        {
            if (piece != null && piece.color == color && piece.type == ChessPieceType.King)
            {
                king = piece;
                break;
            }
        }
        if (king == null)
        {
            return false;
        }
        foreach (ChessPiece piece in currentBoard)
        {
            if (piece != null && piece.color != color && piece.type != ChessPieceType.King)
            {
                Vector2Int[] moves = GetPieceMoves(piece);
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

    public ChessPiece GetKing(ChessPieceColor color)
    {
        foreach (ChessPiece piece in board)
        {
            if (piece != null && piece.color == color && piece.type == ChessPieceType.King)
            {
                return piece;
            }
        }
        return null;
    }

    public bool InCheck(ChessPieceColor color, int x, int y)
    {
        ChessPiece[,] newBoard = CopyBoard(board);
        ChessPiece king = GetKing(color);
        MovePiece(newBoard, color, king.x, king.y, x, y);
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
        ChessPiece king = GetKing(color);
        Vector2Int[] kingMoves = GetKingMoves(king);
        foreach (Vector2Int move in kingMoves)
        {
            ChessPiece[,] newBoard = CopyBoard(board);
            MovePiece(newBoard, king.color, king.x, king.y, move.x, move.y);
            if (!InCheck(newBoard, color))
            {
                return false;
            }
        }
        foreach (ChessPiece piece in board)
        {
            if (piece != null && piece.color == color)
            {
                Vector2Int[] moves = GetPieceMoves(piece);
                foreach (Vector2Int move in moves)
                {
                    ChessPiece[,] newBoard = CopyBoard(board);
                    MovePiece(newBoard, piece.color, piece.x, piece.y, move.x, move.y);
                    if (!InCheck(newBoard, color))
                    {
                        return false;
                    }
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
                Vector2Int[] moves = GetPieceMoves(piece);
                foreach (Vector2Int move in moves)
                {
                    ChessPiece[,] newBoard = CopyBoard(board);
                    MovePiece(newBoard, piece.color, piece.x, piece.y, move.x, move.y);
                    if (!InCheck(newBoard, color))
                    {
                        return false;
                    }
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
        return InStalemate(ChessPieceColor.White) || InStalemate(ChessPieceColor.Black);
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
            return GameOutcome.Draw;
        }
        return GameOutcome.InProgress;
    }

    public ChessPieceColor turn = ChessPieceColor.White; 

    public void EndTurn() {
        SwitchTurn();
        ChessPieceColor otherColor = turn == ChessPieceColor.White ? ChessPieceColor.Black : ChessPieceColor.White;
        if(InCheck(turn)) {
            Debug.Log("Check!");
            OnCheck(turn);
        } else if(InCheck(otherColor)) {
            Debug.Log("Check!");
            OnCheck(otherColor);
        }

        

        //check game outcome
        GameOutcome outcome = GetGameOutcome();
        
        //switch turn
        switch (outcome)
        {
            case GameOutcome.BlackWin:
                OnCheckmate(ChessPieceColor.Black);
                Debug.Log("Black wins!");
                break;
            case GameOutcome.WhiteWin:
                OnCheckmate(ChessPieceColor.White);
                Debug.Log("White wins!");
                break;
            case GameOutcome.Draw:
                Debug.Log("Draw!");
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

    public bool MovePiece(ChessPiece[,] currentBoard, ChessPieceColor color, int x, int y, int newX, int newY)
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

    public bool MovePiece(ChessPiece piece, int newX, int newY)
    {

        //if king is moved, check if it is castling
        if(piece.type == ChessPieceType.King) {
            if(newX == 2) {
                Castle(1, piece.color);
                return true;
            } else if(newX == 6) {
                //castle left
                Castle(0, piece.color);
                return true;
            }
        }

        if(piece.x == newX && piece.y == newY || (board[newX, newY]!= null && board[newX, newY].color == piece.color) || !GetLegalMoves(piece).Contains(new Vector2Int(newX, newY)))
            return false;
        
        bool attacking = false;
        if(board[newX, newY] != null) {
            KillPiece(newX, newY);
            attacking=true;
        }
            
        

        Debug.Log("Moving " + piece.type + " from " + piece.x + ", " + piece.y + " to " + newX + ", " + newY + ".");

        int x = piece.x;
        int y = piece.y;

        board[newX, newY] = piece;
        board[piece.x, piece.y] = null;
        piece.Move(newX, newY);

        if(!attacking)
            OnMove(piece.color, x, y, newX, newY);

        EndTurn();
        
        return true;
    }

    public void IrregularMove(ChessPiece piece, int newX, int newY)
    {
        board[newX, newY] = piece;
        board[piece.x, piece.y] = null;
        piece.Move(newX, newY);
    }

    public void NotationMove(string notation, ChessPieceColor color) {
        if (notation == "O-O")
        {
            Castle(0, color); // Perform king-side castle
        }
        else if (notation == "O-O-O")
        {
            Castle(1, color); // Perform queen-side castle
        }
        else if (notation.Length == 4)
        {
            int startX = notation[0] - 'a'; // Convert letter to x-coordinate
            int startY = '8' - notation[1]; // Convert number to y-coordinate
            int endX = notation[2] - 'a'; // Convert letter to x-coordinate
            int endY = '8' - notation[3]; // Convert number to y-coordinate

            ChessPiece piece = GetPiece(startX, startY);
            MovePiece(piece, endX, endY);
        }
        else
        {
            Debug.Log("Invalid algebraic notation!");
        }
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
        Vector2Int[] moves = GetPieceMoves(piece);

        foreach (Vector2Int move in moves)
        {
            ChessPiece[,] newBoard = CopyBoard(board);
            MovePiece(newBoard, piece.color, piece.x, piece.y, move.x, move.y);

            if (!InCheck(newBoard, piece.color))
            {
                bool isBlockingCheck = false;

                // Check if the move blocks a check
                if (InCheck(board, piece.color))
                {
                    isBlockingCheck = true;
                }


                // Include the move in the list of legal moves
                if (isBlockingCheck)
                {
                    
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
        board = ReadFEN(fen);
    }
    
    public ChessPiece[,] ReadFEN(string fen) {
        //read FEN string and return a 2d array of chess pieces
        //https://en.wikipedia.org/wiki/Forsyth%E2%80%93Edwards_Notation
        string[] fenRows = fen.Split('/');
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
        return tempBoard;


    }

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
        return fen;
    }


    //move, capture, check, checkmate

    public delegate void MoveEvent(ChessPieceColor color, int fromX, int fromY, int toX, int toY);
    public event MoveEvent OnMove;

    public delegate void CaptureEvent(ChessPieceColor color);
    public event CaptureEvent OnCapture;

    public delegate void CheckEvent(ChessPieceColor color);
    public event CheckEvent OnCheck;

    public delegate void CheckmateEvent(ChessPieceColor color);
    public event CheckmateEvent OnCheckmate;
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

    public Texture2D whiteSquare;
    public Texture2D blackSquare;

    public Texture2D whitePawn;
    public Texture2D whiteRook;
    public Texture2D whiteKnight;
    public Texture2D whiteBishop;
    public Texture2D whiteQueen;
    public Texture2D whiteKing;

    public Texture2D blackPawn;
    public Texture2D blackRook;
    public Texture2D blackKnight;
    public Texture2D blackBishop;
    public Texture2D blackQueen;
    public Texture2D blackKing;

    public Texture2D circle;


    AudioClip moveSound;
    AudioClip captureSound;
    AudioClip checkSound;
    AudioClip checkmateSound;

    AudioClip castleSound;
    //AudioClip stalemateSound;

    
    void LoadTextures() {
        whitePawn = Resources.Load<Texture2D>("chess/Chess_plt60");
        whiteRook = Resources.Load<Texture2D>("chess/Chess_rlt60");
        whiteKnight = Resources.Load<Texture2D>("chess/Chess_nlt60");
        whiteBishop = Resources.Load<Texture2D>("chess/Chess_blt60");
        whiteQueen = Resources.Load<Texture2D>("chess/Chess_qlt60");
        whiteKing = Resources.Load<Texture2D>("chess/Chess_klt60");

        blackPawn = Resources.Load<Texture2D>("chess/Chess_pdt60");
        blackRook = Resources.Load<Texture2D>("chess/Chess_rdt60");
        blackKnight = Resources.Load<Texture2D>("chess/Chess_ndt60");
        blackBishop = Resources.Load<Texture2D>("chess/Chess_bdt60");
        blackQueen = Resources.Load<Texture2D>("chess/Chess_qdt60");
        blackKing = Resources.Load<Texture2D>("chess/Chess_kdt60");

        circle = Resources.Load<Texture2D>("chess/circle");

        //load sounds

        moveSound = Resources.Load<AudioClip>("chess/move");
        captureSound = Resources.Load<AudioClip>("chess/capture");
        checkSound = Resources.Load<AudioClip>("chess/check");
        checkmateSound = Resources.Load<AudioClip>("chess/checkmate");
        castleSound = Resources.Load<AudioClip>("chess/castle");

    }

    IStockfish stockfish;
    
    MoveType lastMoveType = MoveType.Invalid;
    public void Initilize(int difficulty)
    {

        //in resources folder
        stockfish = new Stockfish.NET.Core.Stockfish(Application.dataPath + "/resources/chess/stockfish-win.exe");
        stockfish.SkillLevel = difficulty;

        board = new ChessBoard();
        LoadTextures();
        //board.LoadFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR");
        board.LoadFEN("rnbqk2r/ppp2ppp/4p3/2b5/2p1nP2/3P1N2/PPP3PP/RNBQ1RK1");

        board.OnMove += (color, fromX, fromY, toX, toY) =>
        {
            if(lastMoveType == MoveType.Invalid)
                lastMoveType = MoveType.Move;

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

        board.OnTurnEnd += (color) =>
        {
            if(lastMoveType == MoveType.Checkmate) {
                Debug.Log("Checkmate");
                AudioSource.PlayClipAtPoint(checkmateSound, Vector3.zero, 3f);
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
            //create a thread to run the bot move
            if(color != playerColor)
                StartCoroutine(BotMove());
            

            
        };

        
        
    }

    IEnumerator BotMove() {
        string move = "t";
        Thread thread = new Thread(() => {
            stockfish.SetFenPosition(board.GetFEN(board.GetBoard()));
            move = stockfish.GetBestMoveTime(500);
            Debug.Log("Move: "+ move);
        });
        thread.Start();
        yield return new WaitUntil(() => move != "t");
        Debug.Log("Done waiting");
        board.NotationMove(move, ChessPieceColor.Black);
        yield return null;
    }

    public void Awake() {
        Initilize(5);
    }

    //Get Set fen
    public string GetFEN() {
        return board.GetFEN(board.GetBoard());
    }

    public void SetFEN(string fen) {
        board.LoadFEN(fen);
    }


    public ChessPieceColor playerColor = ChessPieceColor.White;




    bool isMousePressed = false;
    

    bool movingBoard = false;

    public float margin = 50f;
    
    Vector2 initialMousePos = Vector2.zero;
    Vector2 initialOffset = Vector2.zero;

    ChessPiece selectedPiece = null;

    List<Vector2Int> legalMoves = new List<Vector2Int>();

    void OnGUI()
    {

        //check if f pressed and then print fen
        if (UnityEngine.Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log(board.GetFEN(board.GetBoard()));
        }

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
            board.LoadFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR");
        }

        Vector2 mousePos = new Vector2(UnityEngine.Input.mousePosition.x, Screen.height - UnityEngine.Input.mousePosition.y);

        // Create a surrounding box
        float boxSize = board.squareSize * 8;
        Vector2 boxPos = new Vector2(board.xOffset, board.yOffset);
        Rect boxRect = new Rect(boxPos.x, boxPos.y, boxSize, boxSize);
        Rect borderRect = new Rect(boxPos.x - margin / 2f, boxPos.y - margin / 2f, boxSize + margin, boxSize + margin);
        GUI.Box(borderRect, "");

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



        //draw chess board
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++) {
                if ((x + y) % 2 == 0)
                {
                    GUI.color = new Color(235f/255f, 236f/255f, 208f/255f);
                }
                else
                {
                    GUI.color = new Color(119f/255f, 149f/255f, 86f/255f);
                }
                Rect tile = new Rect(x * board.squareSize + board.xOffset, y * board.squareSize + board.yOffset, board.squareSize, board.squareSize);
                GUI.DrawTexture(tile, Texture2D.whiteTexture);


                //draw a circle if the tile is a legal move
                if(legalMoves.Contains(new Vector2Int(x, y))) {
                    GUI.color = new Color(0.3f, 0.3f, 0.3f, 0.5f);
                    Rect circleRect = new Rect(tile.x, tile.y, tile.width, tile.height);
                    //scale the circle so that it is half the size of the tile
                    circleRect.x += circleRect.width / 4f;
                    circleRect.y += circleRect.height / 4f;
                    circleRect.width /= 2f;
                    circleRect.height /= 2f;
                    //draw a circle
                    GUI.DrawTexture(circleRect, circle);
                }
                


                //Detect if the mouse is over the tile
                if (tile.Contains(mousePos) && mousePressedFrame)
                {
                    Debug.Log("Clicked on tile " + x + ", " + y);
                    if(selectedPiece == null || !legalMoves.Contains(new Vector2Int(x, y))) {
                        selectedPiece = board.GetPiece(x, y);
                        if(selectedPiece.color != playerColor)
                            selectedPiece = null;
                        legalMoves = board.GetLegalMoves(selectedPiece);
                    } else {
                        if(legalMoves.Contains(new Vector2Int(x, y)) && board.turn == ChessPieceColor.White && board.MovePiece(selectedPiece, x, y) ) {
                            Debug.Log("Moved piece");
                            legalMoves = new List<Vector2Int>();
                            selectedPiece = null;
                        }
                    }
                }

            }
        }

        if(selectedPiece != null) {
            Color highlightColor = new Color(0.7f, 0.7f, 0.9f, 0.9f);
            GUI.color = highlightColor;
            
            GUI.DrawTexture(GetTile(selectedPiece), Texture2D.whiteTexture);
        }

        GUI.color = Color.white;

        //draw pieces
        ChessPiece[,] pieces = board.GetBoard();
        for (int x = 0; x < 8; x++)
        {
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
                    GUI.DrawTexture(new Rect(x * board.squareSize+board.xOffset, y * board.squareSize+board.yOffset, board.squareSize, board.squareSize), texture);
                }
            }
            
        }

         if(UnityEngine.Input.GetMouseButtonDown(1)) {
            selectedPiece = null;
        }
        
    }

    public Rect GetTile(ChessPiece piece) {
        return GetTile(piece.x, piece.y);
    }

    public Rect GetTile(int x, int y) {
        return new Rect(x * board.squareSize + board.xOffset, y * board.squareSize + board.yOffset, board.squareSize, board.squareSize);
    }
    
    
}
