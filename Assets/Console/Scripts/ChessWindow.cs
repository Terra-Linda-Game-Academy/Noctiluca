using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChessPieceType
{
    Pawn,
    Rook,
    Knight,
    Bishop,
    Queen,
    King
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

    public void MovePiece(int x, int y, int newX, int newY)
    {
        ChessPiece piece = board[x, y];
        piece.Move(newX, newY);
        board[newX, newY] = piece;
        board[x, y] = null;
    }

    public void KillPiece(int x, int y)
    {
        ChessPiece piece = board[x, y];
        piece.Kill();
        board[x, y] = null;
    }


    public void LoadFEN(string fen)
    {
        board = ReadFEN(fen);
    }
    
    public ChessPiece[,] ReadFEN(string fen) {
        //read FEN string and return a 2d array of chess pieces
        //https://en.wikipedia.org/wiki/Forsyth%E2%80%93Edwards_Notation
        string[] fenRows = fen.Split('/');
        for (int i = 0; i < fenRows.Length; i++)
        {
            string row = fenRows[i];
            int x = 0;
            for (int j = 0; j < row.Length; j++)
            {
                char c = row[j];
                if (char.IsDigit(c))
                {
                    x += (int)char.GetNumericValue(c);
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
                    board[x, i] = piece;
                    x++;
                }
            }
        }
        return board;


    }
}

public class ChessWindow : MonoBehaviour
{
    public ChessBoard board = new ChessBoard();

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

    }
    
    public void Initilize()
    {
        LoadTextures();
        board.LoadFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR");
    }

    Vector2 mouseReletivePosition;

    public float margin = 25f;

    void OnGUI()
    {
        //create a surrounding box
        float boxSize = board.squareSize*8;
        Vector2 boxPos = new Vector2(board.xOffset, board.yOffset);
        Rect boxRect = new Rect(boxPos.x,boxPos.y, boxSize, boxSize);
        Rect borderRect = new Rect(boxPos.x-margin/2f,boxPos.y-margin/2f, boxSize + margin, boxSize + margin);
        GUI.Box(borderRect, "");
        //detct if the mouse is inside the box. Specify UnityEngine.Input
        if (borderRect.Contains(UnityEngine.Input.mousePosition) && !boxRect.Contains(UnityEngine.Input.mousePosition))
        {
            //if mouse pressed
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                //calculate the mouse position relative to the box
                mouseReletivePosition = new Vector2(UnityEngine.Input.mousePosition.x - board.xOffset, UnityEngine.Input.mousePosition.y - board.yOffset);
            }
            else if (UnityEngine.Input.GetMouseButton(0))
            {
                //calculate the mouse position relative to the box
                Vector2 mouseReletivePosition2 = new Vector2(UnityEngine.Input.mousePosition.x - board.xOffset, UnityEngine.Input.mousePosition.y - board.yOffset);
                //calculate the difference between the two positions
                Vector2 delta = mouseReletivePosition2 - mouseReletivePosition;
                //move the board by the difference
                board.xOffset += delta.x;
                board.yOffset += delta.y;
                //update the mouse position
                mouseReletivePosition = mouseReletivePosition2;
            }

            
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
                GUI.DrawTexture(new Rect(x * board.squareSize + board.xOffset, y * board.squareSize + board.yOffset, board.squareSize, board.squareSize), Texture2D.whiteTexture);
            }
        }

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


    }
    
}
