using System;

namespace Chess
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var board = new ChessBoard(chessBoard);
        }

        private void Form1_Resize(object sender, System.EventArgs e)
        {
            // Sets variables to window width and height
            var width = this.ClientSize.Width;
            var height = this.ClientSize.Height;

            // The new board width and height are a percentage of the total window width and height
            var newBoardWidth = (int)(width * 0.7);
            var newBoardHeight = (int)(height * 0.7);

            // Ensures that the board stays a square
            // The width or height is only the smallest of the two dimensions
            if (newBoardWidth < newBoardHeight)
            {
                chessBoard.Width = newBoardWidth;
                chessBoard.Height = newBoardWidth;
            } 
            else
            {
                chessBoard.Width = newBoardHeight;
                chessBoard.Height = newBoardHeight;
            }

            // Sets the board centerpoint
            var chessBoardCenterPoint = new Point((width - chessBoard.Width) / 2, (height - chessBoard.Height) / 2);
            chessBoard.Location = chessBoardCenterPoint;
        }
    }

    public class ChessBoard
    {
        public BoardSquare[,] Squares { get; set; }

        private TableLayoutPanel ChessBoardTable { get; }
        public bool IsWhiteTurn { get; set; } = true;

        public ChessBoard(TableLayoutPanel chessBoardPanel)
        {
            IsWhiteTurn = true;
            ChessBoardTable = chessBoardPanel;
            GenerateBoard();
            GenerateStartingPieces();
        }

        private void GenerateBoard()
        {
            var rows = ChessBoardTable.RowCount;
            var columns = ChessBoardTable.ColumnCount;

            Squares = new BoardSquare[rows, columns];

            bool colInt = false;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    var square = new BoardSquare(r, c, colInt);
                    Squares[r, c] = square;
                    square.ChessBoard = this;

                    ChessBoardTable.Controls.Add(square.Button, c, r);
                    colInt = !colInt;
                }
                colInt = !colInt;
            }
        }

        private void GenerateStartingPieces()
        {
            // Rooks
            new Rook(Squares[0, 0], true);
            new Rook(Squares[0, 7], true);
            new Rook(Squares[7, 0], false);
            new Rook(Squares[7, 7], false);

            // Bishops
            new Bishop(Squares[0, 2], true);
            new Bishop(Squares[0, 5], true);
            new Bishop(Squares[7, 2], false);
            new Bishop(Squares[7, 5], false);

            // Queens
            new Queen(Squares[0, 3], true);
            new Queen(Squares[7, 3], false);

            // Kings
            new King(Squares[0, 4], true);
            new King(Squares[7, 4], false);

            // White Pawns
            new Pawn(Squares[6, 0], false);
            new Pawn(Squares[6, 1], false);
            new Pawn(Squares[6, 2], false);
            new Pawn(Squares[6, 3], false);
            new Pawn(Squares[6, 4], false);
            new Pawn(Squares[6, 5], false);
            new Pawn(Squares[6, 6], false);
            new Pawn(Squares[6, 7], false);

            // Black Pawns
            new Pawn(Squares[1, 0], true);
            new Pawn(Squares[1, 1], true);
            new Pawn(Squares[1, 2], true);
            new Pawn(Squares[1, 3], true);
            new Pawn(Squares[1, 4], true);
            new Pawn(Squares[1, 5], true);
            new Pawn(Squares[1, 6], true);
            new Pawn(Squares[1, 7], true);

            // Knights
            new Knight(Squares[0, 1], true);
            new Knight(Squares[0, 6], true);
            new Knight(Squares[7, 1], false);
            new Knight(Squares[7, 6], false);
        }

        public void ResetAllSquareColours()
        {
            for (var r = 0; r < Squares.GetLength(0); r++)
            {
                for (var c = 0; c < Squares.GetLength(1); c++)
                {
                    Squares[r, c].ResetColour();
                }
            }
        }
    }
         
    public class BoardSquare
    {
        public ChessBoard ChessBoard {  get; set; }
        public int Row {  get; set; }
        public int Column { get; set; }
        private bool IsBlack { get; set; }
        public Button Button { get; set; }
        public Piece CurrentPiece { get; set; }

        public BoardSquare(int row, int column, bool colour)
        {
            Row = row;
            Column = column;

            IsBlack = colour;
            Button = SquareTemplate(colour);
        }

        private Button SquareTemplate(bool isBlack)
        {
            var square = new Button();

            square.FlatStyle = FlatStyle.Flat;
            Color colour;

            if (isBlack)
            {
                colour = Color.Black;
            } 
            else
            {
                colour = Color.White;
            }

            square.Dock = DockStyle.Fill;

            ChangeColour(square, colour);

            square.MouseDown += (sender, e) => OnSquareClick();

            return square;
        }

        private void ChangeColour(Button square, Color colour)
        {
            square.BackColor = colour;
            square.FlatAppearance.MouseDownBackColor = colour;
            square.FlatAppearance.MouseOverBackColor = colour;
            square.FlatAppearance.BorderColor = colour;
        }

        public void ResetColour()
        {
            if (IsBlack)
            {
                ChangeColour(Button, Color.Black);
            }
            else
            {
                ChangeColour(Button, Color.White);
            }
        }

        private void OnSquareClick()
        {
            LeftClick();
        }

        private void LeftClick()
        {
            ChessBoard.ResetAllSquareColours();

            if (CurrentPiece != null)
            {
                var legalMoves = CurrentPiece.GetLegalMoves(ChessBoard.Squares);

                if (ChessBoard.IsWhiteTurn != CurrentPiece.IsBlack)
                {
                    foreach (var move in legalMoves)
                    {
                        move.Button.BackColor = Color.Green;
                        if (move.IsBlack)
                        {
                            move.ChangeColour(move.Button, Color.DarkGreen);
                        }
                        else
                        {
                            move.ChangeColour(move.Button, Color.LightGreen);
                        }
                    }

                    if (CurrentPiece.CurrentBoardSquare.IsBlack)
                    {
                        CurrentPiece.CurrentBoardSquare.ChangeColour(CurrentPiece.CurrentBoardSquare.Button, Color.Goldenrod);
                    }
                    else
                    {
                        CurrentPiece.CurrentBoardSquare.ChangeColour(CurrentPiece.CurrentBoardSquare.Button, Color.Gold);
                    }
                }

            }
            else
            {
                return;
            }
        }
    }

    #region Pieces
    public abstract class Piece
    {
        public bool IsBlack { get; set; }
        public BoardSquare CurrentBoardSquare { get; set; }
        public Image PieceImage { get; set; }

        protected Piece(BoardSquare startSquare, bool isBlack)
        {
            CurrentBoardSquare = startSquare;
            CurrentBoardSquare.CurrentPiece = this;
            IsBlack = isBlack;
        }

        public abstract List<BoardSquare> GetLegalMoves(BoardSquare[,] board);
    }

    public class Rook : Piece
    {
        public Rook(BoardSquare startSquare, bool isBlack) : base(startSquare, isBlack)
        {
            
        }

        public override List<BoardSquare> GetLegalMoves(BoardSquare[,] board)
        {
            var row = CurrentBoardSquare.Row;
            var column = CurrentBoardSquare.Column;
            var legalMoves = new List<BoardSquare>();

            legalMoves.AddRange(StraightMoves(board, row, column, 1, 0));
            legalMoves.AddRange(StraightMoves(board, row, column, -1, 0));
            legalMoves.AddRange(StraightMoves(board, row, column, 0, 1));
            legalMoves.AddRange(StraightMoves(board, row, column, 0, -1));

            return legalMoves;
        }

        private List<BoardSquare> StraightMoves(BoardSquare[,] board, int row, int column, int rowDir, int columnDir)
        {
            var straightMoves = new List<BoardSquare>();

            var r = row + rowDir;
            var c = column + columnDir;

            while (r >= 0 && r < board.GetLength(0) && c >= 0 && c < board.GetLength(1))
            {
                var square = board[r, c];

                if (square.CurrentPiece != null)
                {
                    if (square.CurrentPiece.IsBlack != this.IsBlack)
                    {
                        straightMoves.Add(square);
                    }
                    break;
                }

                straightMoves.Add(square);

                r += rowDir;
                c += columnDir;
            }
            return straightMoves;
        }
    }

    public class Bishop : Piece
    {
        public Bishop(BoardSquare startSquare, bool isBlack) : base(startSquare, isBlack)
        {

        }

        public override List<BoardSquare> GetLegalMoves(BoardSquare[,] board)
        {
            var row = CurrentBoardSquare.Row;
            var column = CurrentBoardSquare.Column;
            var legalMoves = new List<BoardSquare>();

            legalMoves.AddRange(DiagonalMoves(board, row, column, 1, 1));
            legalMoves.AddRange(DiagonalMoves(board, row, column, -1, 1));
            legalMoves.AddRange(DiagonalMoves(board, row, column, 1, -1));
            legalMoves.AddRange(DiagonalMoves(board, row, column, -1, -1));

            return legalMoves;
        }

        private List<BoardSquare> DiagonalMoves(BoardSquare[,] board, int row, int column, int rowDir, int columnDir)
        {
            var diagonalMoves = new List<BoardSquare>();

            var r = row + rowDir;
            var c = column + columnDir;

            while (r >= 0 && r < board.GetLength(0) && c >= 0 && c < board.GetLength(1))
            {
                var square = board[r, c];

                if (square.CurrentPiece != null)
                {
                    if (square.CurrentPiece.IsBlack != this.IsBlack)
                    {
                        diagonalMoves.Add(square);
                    }
                    break;
                }

                diagonalMoves.Add(square);

                r += rowDir;
                c += columnDir;
            }
            return diagonalMoves;
        }
    }

    public class Queen : Piece
    {
        public Queen(BoardSquare startSquare, bool isBlack) : base(startSquare, isBlack) 
        {

        }

        public override List<BoardSquare> GetLegalMoves(BoardSquare[,] board)
        {
            var row = CurrentBoardSquare.Row;
            var column = CurrentBoardSquare.Column;
            var legalMoves = new List<BoardSquare>();

            // Straight moves
            legalMoves.AddRange(Moves(board, row, column, 1, 0));
            legalMoves.AddRange(Moves(board, row, column, -1, 0));
            legalMoves.AddRange(Moves(board, row, column, 0, 1));
            legalMoves.AddRange(Moves(board, row, column, 0, -1));
            legalMoves.AddRange(Moves(board, row, column, 1, 1));
            legalMoves.AddRange(Moves(board, row, column, -1, 1));
            legalMoves.AddRange(Moves(board, row, column, 1, -1));
            legalMoves.AddRange(Moves(board, row, column, -1, -1));

            return legalMoves;
        }

        private List<BoardSquare> Moves(BoardSquare[,] board, int row, int column, int rowDir, int columnDir)
        {
            var moves = new List<BoardSquare>();

            var r = row + rowDir;
            var c = column + columnDir;

            while (r >= 0 && r < board.GetLength(0) && c >= 0 && c < board.GetLength(1))
            {
                var square = board[r, c];

                if (square.CurrentPiece != null)
                {
                    if (square.CurrentPiece.IsBlack != this.IsBlack)
                    {
                        moves.Add(square);
                    }
                    break;
                }

                moves.Add(square);

                r += rowDir;
                c += columnDir;
            }
            return moves;
        }
    }

    public class King : Piece
    {
        public King(BoardSquare startSquare, bool isBlack) : base(startSquare, isBlack)
        {
            
        }

        public override List<BoardSquare> GetLegalMoves(BoardSquare[,] board)
        {
            var row = CurrentBoardSquare.Row;
            var column = CurrentBoardSquare.Column;
            var legalMoves = new List<BoardSquare>();

            legalMoves.AddRange(Moves(board, row, column, 1, 0));
            legalMoves.AddRange(Moves(board, row, column, -1, 0));
            legalMoves.AddRange(Moves(board, row, column, 0, 1));
            legalMoves.AddRange(Moves(board, row, column, 0, -1));
            legalMoves.AddRange(Moves(board, row, column, 1, 1));
            legalMoves.AddRange(Moves(board, row, column, -1, 1));
            legalMoves.AddRange(Moves(board, row, column, 1, -1));
            legalMoves.AddRange(Moves(board, row, column, -1, -1));

            return legalMoves;
        }

        private List<BoardSquare> Moves(BoardSquare[,] board, int row, int column, int rowDir, int columnDir)
        {
            var moves = new List<BoardSquare>();

            var r = row + rowDir;
            var c = column + columnDir;

            if (r >= 0 && r < board.GetLength(0) && c >= 0 && c < board.GetLength(1))
            {
                var square = board[r, c];

                if (square.CurrentPiece == null || square.CurrentPiece.IsBlack != this.IsBlack)
                {
                    moves.Add(square);
                }
            }
            return moves;
        }
    }

    public class Pawn : Piece
    {
        public bool firstTurn = true;

        public Pawn(BoardSquare startSquare, bool isBlack) : base(startSquare, isBlack)
        {

        }

        public override List<BoardSquare> GetLegalMoves(BoardSquare[,] board)
        {
            var row = CurrentBoardSquare.Row;
            var column = CurrentBoardSquare.Column;
            var legalMoves = new List<BoardSquare>();

            legalMoves.AddRange(Moves(board, row, column));

            return legalMoves;
        }

        private List<BoardSquare> Moves(BoardSquare[,] board, int row, int column)
        {
            var moves = new List<BoardSquare>();
            int direction;

            if (this.IsBlack)
            {
                direction = 1;
            } 
            else
            {
                direction = -1;
            }

            var r = row + direction;
            var square = board[r, column];
            
            if (square.CurrentPiece == null)
            {
                moves.Add(square);
                if (firstTurn)
                {
                    r += direction;
                    square = board[r, column];
                    if (square.CurrentPiece == null)
                    {
                        moves.Add(square);
                    }
                }
            }
            return moves;
        }
    }

    public class Knight : Piece
    {
        public Knight(BoardSquare startSquare, bool isBlack) : base(startSquare, isBlack) 
        {

        }

        public override List<BoardSquare> GetLegalMoves(BoardSquare[,] board)
        {
            var row = CurrentBoardSquare.Row;
            var column = CurrentBoardSquare.Column;
            var legalMoves = new List<BoardSquare>();

            legalMoves.AddRange(Moves(board, row, column, 1, 0));
            legalMoves.AddRange(Moves(board, row, column, -1, 0));
            legalMoves.AddRange(Moves(board, row, column, 0, 1));
            legalMoves.AddRange(Moves(board, row, column, 0, -1));

            return legalMoves;
        }

        private List<BoardSquare> Moves(BoardSquare[,] board, int row, int column, int rowDir, int columnDir)
        {
            var moves = new List<BoardSquare>();

            int r = row + rowDir * 2;
            int c = column + columnDir * 2;

            // Goes out two spaces away from the knight
            if (r >= 0 && r < board.GetLength(0) && c >= 0 && c < board.GetLength(1))
            {
                var square = board[r, c];

                // Check for spaces above and below
                if (rowDir == 0) { 
                    if (r + 1 < board.GetLength(0))
                    {
                        var above = board[r + 1, c];
                        if (above.CurrentPiece == null || above.CurrentPiece.IsBlack != this.IsBlack)
                        {
                            moves.Add(above);
                        }
                    }
                    if (r - 1 >= 0)
                    {
                        var below = board[r - 1, c];
                        if (below.CurrentPiece == null || below.CurrentPiece.IsBlack != this.IsBlack)
                        {
                            moves.Add(below);
                        }
                    }
                }

                // Check for spaces to the left and right 
                if (columnDir == 0)
                {
                    if (c + 1 < board.GetLength(1))
                    {
                        var right = board[r, c + 1];
                        if (right.CurrentPiece == null || right.CurrentPiece.IsBlack != this.IsBlack)
                        {
                            moves.Add(right);
                        }
                    }
                    if (c - 1 >= 0)
                    {
                        var left = board[r, c - 1];
                        if (left.CurrentPiece == null || left.CurrentPiece.IsBlack != this.IsBlack)
                        {
                            moves.Add(left);
                        }
                    }
                }
            }
            return moves;
        }
    }
    #endregion
}