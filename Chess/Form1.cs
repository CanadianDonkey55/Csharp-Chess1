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

        // Contructor
        public ChessBoard(TableLayoutPanel chessBoardPanel)
        {
            // White always goes first
            IsWhiteTurn = true;

            // The table is equal to the table in the designer
            ChessBoardTable = chessBoardPanel;

            // Generates the board wuth all their squares
            GenerateBoard();

            // Places the pieces on their starting squares
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
            // The first value of the array is their row, the second is the column
            // The bool is if the piece is black or white (true = white, false = black)

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

        // Resets the colour of every square
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
            // The row and columns are equal to the row and column set in the ChessBoard class
            Row = row;
            Column = column;

            // If the square is black or white and creates each button based on a template
            IsBlack = colour;
            Button = SquareTemplate();
        }

        private Button SquareTemplate()
        {
            // Sets all of the button colours and makes the OnSquareClick method an event

            var square = new Button();

            square.FlatStyle = FlatStyle.Flat;
            this.Button = square;
            ResetColour();
            square.Dock = DockStyle.Fill;

            square.MouseDown += (sender, e) => OnSquareClick(e);
            return square;
        }

        private void ChangeColour(Button square, Color colour)
        {
            // Each square colour is equal to the colour set in the constructor

            square.BackColor = colour;
            square.FlatAppearance.MouseDownBackColor = colour;
            square.FlatAppearance.MouseOverBackColor = colour;
            square.FlatAppearance.BorderColor = colour;
        }

        public void ResetColour()
        {
            // If the bool IsBlack is true, the square colour is set to black, else it's white
            if (IsBlack)
            {
                ChangeColour(Button, Color.Black);
            }
            else
            {
                ChangeColour(Button, Color.White);
            }
        }

        private void OnSquareClick(MouseEventArgs e)
        {
            // If you left click a square, LeftClick(), if you right click a square RightClick()

            if (e.Button == MouseButtons.Left)
            {
                LeftClick();
            } 
            else if (e.Button == MouseButtons.Right)
            {
                RightClick();
            }
        }

        private void LeftClick()
        {
            // Calls the method to reset the colour of every square on the board back to either black or white
            ChessBoard.ResetAllSquareColours();

            // Goes through every square on the board and checks to find the one that is currently seleced
            // If it finds one, it gets all the legal moves of the piece on that square and sees if the currently selected square is one of that pieces legal moves
            // If it is, that piece is moved to this square and the method is returned
            // If no piece that is selected is found, the method continues
            foreach (var square in ChessBoard.Squares)
            {
                if (square.CurrentPiece != null && square.CurrentPiece.IsSelected)
                {
                    var legalMoves = square.CurrentPiece.GetLegalMoves(ChessBoard.Squares);
                    if (legalMoves.Contains(this))
                    {
                        Move(square);
                        return;
                    }
                }
            }
            
            // If the clicked square is not empty (ie, has a piece on it)
            if (CurrentPiece != null)
            {
                // If the currently seleccted piece is the same colour as the player turn
                if (ChessBoard.IsWhiteTurn != CurrentPiece.IsBlack)
                {
                    // If it finds another piece on your team is already selected, unselect that piece
                    foreach (var square in ChessBoard.Squares)
                    {
                        if (square.CurrentPiece != null && square.CurrentPiece.IsSelected)
                        {
                            square.CurrentPiece.IsSelected = false;
                            break;
                        }
                    }
                    // Select this piece and create a list of squares that are legal for the selected piece to move to
                    CurrentPiece.IsSelected = true;
                    var legalMoves = CurrentPiece.GetLegalMoves(ChessBoard.Squares);

                    // Highlights legal squares in either dark green or light green depending on if that square is black or white
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

                    // Highlights this square either dark or light yellow depending on if it's black or white
                    if (IsBlack)
                    {
                        ChangeColour(Button, Color.Goldenrod);
                    }
                    else
                    {
                        ChangeColour(Button, Color.Gold);
                    }

                    return;
                }

            }
        }

        private void RightClick()
        {
            // If you right click a square that is already red, reset that square back to it's original colour
            if (Button.BackColor == Color.Maroon || Button.BackColor == Color.Red)
            {
                ResetColour();
                return;
            }

            // Makes dark squares a darker red and light squares a lighter red
            if (IsBlack)
            {
                ChangeColour(Button, Color.Maroon);
            }
            else
            {
                ChangeColour(Button, Color.Red);
            }
        }

        private void Move(BoardSquare square)
        {
            // Stops pawns from moving multiple squares on subsequent turns
            if (square.CurrentPiece is Pawn pawn)
            {
                pawn.firstTurn = false;
            }
            
            // If the current space is not null, (if there is an enemy piece here) make it null
            if (CurrentPiece != null)
                
            {
                CurrentPiece = null;
            }

            // The current piece is the same as the piece on the old square
            // The piece on the old square is now on this square
            // The piece is no longer selected
            // The square's current piece is now null
            // Changes the turn
            CurrentPiece = square.CurrentPiece;
            square.CurrentPiece.CurrentBoardSquare = this;
            square.CurrentPiece.IsSelected = false;
            square.CurrentPiece = null;

            ChessBoard.IsWhiteTurn = !ChessBoard.IsWhiteTurn;
        }
    }

    #region Pieces
    public abstract class Piece
    {
        public bool IsBlack { get; set; }
        public BoardSquare CurrentBoardSquare { get; set; }
        public Image PieceImage { get; set; }
        public bool IsSelected { get; set; }

        protected Piece(BoardSquare startSquare, bool isBlack)
        {
            // The current board square is the square that this piece starts on, and the square's current piece is this piece
            CurrentBoardSquare = startSquare;
            CurrentBoardSquare.CurrentPiece = this;
            // If the piece is set to black in the ChessBoard class, it is black, and it is by default not selected
            IsBlack = isBlack;
            IsSelected = false;
        }

        // A list of legal moves for each piece
        public abstract List<BoardSquare> GetLegalMoves(BoardSquare[,] board);
    }

    public class Rook : Piece
    {
        public Rook(BoardSquare startSquare, bool isBlack) : base(startSquare, isBlack)
        {
            // Makes the image either the black or white version of this piece (applies to all pieces)
            if (IsBlack)
            {
                PieceImage = Image.FromFile(@"Resources\BlackPieces\BlackRook.png");
            }
            else
            {
                PieceImage = Image.FromFile(@"Resources\WhitePieces\WhiteRook.png");
            }

            // If there is an image on this piece, it's width and height are set to the width and height of the square, the board square's image is this piece image, and it's centered on the square
            if (PieceImage != null)
            {
                PieceImage = new Bitmap(PieceImage, CurrentBoardSquare.Button.Width, CurrentBoardSquare.Button.Height);

                CurrentBoardSquare.Button.Image = PieceImage;
                CurrentBoardSquare.Button.ImageAlign = ContentAlignment.MiddleCenter;
            }
        }

        public override List<BoardSquare> GetLegalMoves(BoardSquare[,] board)
        {
            // Row and column are the row and column that this piece is currently on
            // Makes a list of legal board square moves
            var row = CurrentBoardSquare.Row;
            var column = CurrentBoardSquare.Column;
            var legalMoves = new List<BoardSquare>();

            // Add the straight moves
            legalMoves.AddRange(Moves(board, row, column, 1, 0));
            legalMoves.AddRange(Moves(board, row, column, -1, 0));
            legalMoves.AddRange(Moves(board, row, column, 0, 1));
            legalMoves.AddRange(Moves(board, row, column, 0, -1));

            return legalMoves;
        }

        private List<BoardSquare> Moves(BoardSquare[,] board, int row, int column, int rowDir, int columnDir)
        {
            // List of board squares which will be the legal moves
            var straightMoves = new List<BoardSquare>();

            // r and c are the row or column that this piece is on, plus the direction that the piece can go (this method is called 4 times with each different direction
            var r = row + rowDir;
            var c = column + columnDir;

            // Goes until it reaches the edge of the board or finds another piece, and adds all those squares as legal moves
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
            // Returns a list of the legal moves
            return straightMoves;
        }
    }

    public class Bishop : Piece
    {
        public Bishop(BoardSquare startSquare, bool isBlack) : base(startSquare, isBlack)
        {
            if (IsBlack)
            {
                PieceImage = Image.FromFile(@"Resources\BlackPieces\BlackBishop.png");
            }
            else
            {
                PieceImage = Image.FromFile(@"Resources\WhitePieces\WhiteBishop.png");
            }


            if (PieceImage != null)
            {
                PieceImage = new Bitmap(PieceImage, CurrentBoardSquare.Button.Width, CurrentBoardSquare.Button.Height);

                CurrentBoardSquare.Button.Image = PieceImage;
                CurrentBoardSquare.Button.ImageAlign = ContentAlignment.MiddleCenter;
            }
        }

        // Exact same methods as the rook, but with neither rowDir or columnDir 0 so that this piece travels diagonally

        public override List<BoardSquare> GetLegalMoves(BoardSquare[,] board)
        {
            var row = CurrentBoardSquare.Row;
            var column = CurrentBoardSquare.Column;
            var legalMoves = new List<BoardSquare>();

            // Add the diagonal moves
            legalMoves.AddRange(Moves(board, row, column, 1, 1));
            legalMoves.AddRange(Moves(board, row, column, -1, 1));
            legalMoves.AddRange(Moves(board, row, column, 1, -1));
            legalMoves.AddRange(Moves(board, row, column, -1, -1));

            return legalMoves;
        }

        private List<BoardSquare> Moves(BoardSquare[,] board, int row, int column, int rowDir, int columnDir)
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
            if (IsBlack)
            {
                PieceImage = Image.FromFile(@"Resources\BlackPieces\BlackQueen.png");
            }
            else
            {
                PieceImage = Image.FromFile(@"Resources\WhitePieces\WhiteQueen.png");
            }


            if (PieceImage != null)
            {
                PieceImage = new Bitmap(PieceImage, CurrentBoardSquare.Button.Width, CurrentBoardSquare.Button.Height);

                CurrentBoardSquare.Button.Image = PieceImage;
                CurrentBoardSquare.Button.ImageAlign = ContentAlignment.MiddleCenter;
            }
        }

        // Both the bishop and rook moves are legal

        public override List<BoardSquare> GetLegalMoves(BoardSquare[,] board)
        {
            var row = CurrentBoardSquare.Row;
            var column = CurrentBoardSquare.Column;
            var legalMoves = new List<BoardSquare>();

            // Straight moves and diagonal moves
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
            if (IsBlack)
            {
                PieceImage = Image.FromFile(@"Resources\BlackPieces\BlackKing.png");
            }
            else
            {
                PieceImage = Image.FromFile(@"Resources\WhitePieces\WhiteKing.png");
            }


            if (PieceImage != null)
            {
                PieceImage = new Bitmap(PieceImage, CurrentBoardSquare.Button.Width, CurrentBoardSquare.Button.Height);

                CurrentBoardSquare.Button.Image = PieceImage;
                CurrentBoardSquare.Button.ImageAlign = ContentAlignment.MiddleCenter;
            }
        }

        // Same as the queen but with a limit of 1 square in every direction

        public override List<BoardSquare> GetLegalMoves(BoardSquare[,] board)
        {
            var row = CurrentBoardSquare.Row;
            var column = CurrentBoardSquare.Column;
            var legalMoves = new List<BoardSquare>();

            // All directions
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
            if (IsBlack)
            {
                PieceImage = Image.FromFile(@"Resources\BlackPieces\BlackPawn.png");
            }
            else
            {
                PieceImage = Image.FromFile(@"Resources\WhitePieces\WhitePawn.png");
            }


            if (PieceImage != null)
            {
                PieceImage = new Bitmap(PieceImage, CurrentBoardSquare.Button.Width, CurrentBoardSquare.Button.Height);

                CurrentBoardSquare.Button.Image = PieceImage;
                CurrentBoardSquare.Button.ImageAlign = ContentAlignment.MiddleCenter;
            }
        }

        public override List<BoardSquare> GetLegalMoves(BoardSquare[,] board)
        {
            // Gets the current row and column of this square 
            // Creates a list of all legal moves
            var row = CurrentBoardSquare.Row;
            var column = CurrentBoardSquare.Column;
            var legalMoves = new List<BoardSquare>();

            // Add the position of this piece
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
            if (IsBlack)
            {
                PieceImage = Image.FromFile(@"Resources\BlackPieces\DarkKnight.png");
            }
            else
            {
                PieceImage = Image.FromFile(@"Resources\WhitePieces\WhiteKnight.png");
            }


            if (PieceImage != null)
            {
                PieceImage = new Bitmap(PieceImage, CurrentBoardSquare.Button.Width, CurrentBoardSquare.Button.Height);

                CurrentBoardSquare.Button.Image = PieceImage;
                CurrentBoardSquare.Button.ImageAlign = ContentAlignment.MiddleCenter;
            }
        }

        public override List<BoardSquare> GetLegalMoves(BoardSquare[,] board)
        {
            var row = CurrentBoardSquare.Row;
            var column = CurrentBoardSquare.Column;
            var legalMoves = new List<BoardSquare>();

            // Add all four directions 
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