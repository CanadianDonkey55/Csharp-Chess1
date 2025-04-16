using System;
using System.ComponentModel;

namespace Chess
{
    public partial class Chess : Form
    {
        ChessBoard ChessBoard;
        public Chess()
        {
            InitializeComponent();
            ChessBoard = new ChessBoard(chessBoard);
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

            //foreach (var square in ChessBoard.Squares)
            //{
            //    square.CurrentPiece.PieceImage = new Bitmap(square.CurrentPiece.PieceImage, square.Button.Width, square.Button.Height);
            //}
        }
    }

    public class PawnPromoteScreen
    {
        public ChessBoard ChessBoard { get; set; }
        public BoardSquare CurrentBoardSquare { get; set; }
        public bool IsBlack { get; set; }
        public Panel panel;
        private String pieceColour;

        public PawnPromoteScreen(ChessBoard chessBoard, BoardSquare currentBoardSquare, bool isBlack)
        {
            ChessBoard = chessBoard;
            IsBlack = isBlack;
            panel = new Panel();

            if (IsBlack)
            {
                pieceColour = "Black";
            }
            else
            {
                pieceColour = "White";
            }
            DisplayPanel();
            CurrentBoardSquare = currentBoardSquare;
        }

        private void DisplayPanel()
        {
            Image knightImage = Image.FromFile(@"Resources\" + pieceColour + @"Pieces\" + pieceColour + "Knight.png");
            Image queenImage = Image.FromFile(@"Resources\" + pieceColour + @"Pieces\" + pieceColour + "Queen.png");
            Image rookImage = Image.FromFile(@"Resources\" + pieceColour + @"Pieces\" + pieceColour + "Rook.png");
            Image bishopImage = Image.FromFile(@"Resources\" + pieceColour + @"Pieces\" + pieceColour + "Bishop.png");

            knightImage = new Bitmap(knightImage, ChessBoard.Squares[0, 0].Button.Width, ChessBoard.Squares[0, 0].Button.Height);
            queenImage = new Bitmap(queenImage, ChessBoard.Squares[0, 0].Button.Width, ChessBoard.Squares[0, 0].Button.Height);
            rookImage = new Bitmap(rookImage, ChessBoard.Squares[0, 0].Button.Width, ChessBoard.Squares[0, 0].Button.Height);
            bishopImage = new Bitmap(bishopImage, ChessBoard.Squares[0, 0].Button.Width, ChessBoard.Squares[0, 0].Button.Height);

            // Create the buttons
            Button queenButton = new()
            {
                Location = new Point(0, 0),
                Image = queenImage,
                ImageAlign = ContentAlignment.MiddleCenter,
                Size = queenImage.Size
            };
            Button rookButton = new()
            {
                Location = new Point(queenButton.Width, 0),
                Image = rookImage,
                ImageAlign = ContentAlignment.MiddleCenter,
                Size = queenImage.Size
            };
            Button bishopButton = new()
            {
                Location = new Point(rookButton.Width * 2, 0),
                Image = bishopImage,
                ImageAlign = ContentAlignment.MiddleCenter,
                Size = queenImage.Size
            };
            Button knightButton = new()
            {
                Location = new Point(bishopButton.Width * 3, 0),
                Image = knightImage,
                ImageAlign = ContentAlignment.MiddleCenter,
                Size = queenImage.Size
            };

            BoardSquare square = ChessBoard.Squares[0, 0];
            panel.BackColor = Color.White;
            panel.Width = square.Button.Width * 4;
            panel.Height = square.Button.Height;

            panel.Controls.Add(queenButton);
            panel.Controls.Add(rookButton);
            panel.Controls.Add(bishopButton);
            panel.Controls.Add(knightButton);

            queenButton.MouseDown += (sender, e) => PieceButton("queen");
            rookButton.MouseDown += (sender, e) => PieceButton("rook");
            knightButton.MouseDown += (sender, e) => PieceButton("knight");
            bishopButton.MouseDown += (sender, e) => PieceButton("bishop");
        }

        public void PieceButton(String pieceType)
        {
            CurrentBoardSquare.CurrentPiece = null;
            switch (pieceType.ToLower())
            {
                case "queen":
                    CurrentBoardSquare.CurrentPiece = new Queen(CurrentBoardSquare, IsBlack);
                    break;
                case "rook":
                    CurrentBoardSquare.CurrentPiece = new Rook(CurrentBoardSquare, IsBlack, null);
                    break;
                case "bishop":
                    CurrentBoardSquare.CurrentPiece = new Bishop(CurrentBoardSquare, IsBlack);
                    break;
                case "knight":
                    CurrentBoardSquare.CurrentPiece = new Knight(CurrentBoardSquare, IsBlack);
                    break;
            }

            CurrentBoardSquare.Button.Image = CurrentBoardSquare.CurrentPiece.PieceImage;
            CurrentBoardSquare.CurrentPiece.CurrentBoardSquare = CurrentBoardSquare;
            panel.Parent.Controls.Remove(panel);
            ChessBoard.Promoting = false;
            ChessBoard.IsWhiteTurn = !ChessBoard.IsWhiteTurn;
        }
    }

    public class ChessBoard
    {
        public bool Promoting { get; set; } = false;
        public BoardSquare[,] Squares { get; set; }

        public TableLayoutPanel ChessBoardTable { get; }
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

            bool isDark = false;

            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    var square = new BoardSquare(row, column, isDark);
                    Squares[row, column] = square;
                    square.ChessBoard = this;

                    ChessBoardTable.Controls.Add(square.Button, column, row);
                    isDark = !isDark;
                }
                isDark = !isDark;
            }
        }

        private void GenerateStartingPieces()
        {
            // The first value of the array is their row, the second is the column
            // The bool is if the piece is black or white (true = white, false = black)

            // Rooks
            new Rook(Squares[0, 0], true, false);
            new Rook(Squares[0, 7], true, true);
            new Rook(Squares[7, 0], false, false);
            new Rook(Squares[7, 7], false, true);

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
            for (var row = 0; row < Squares.GetLength(0); row++)
            {
                for (var column = 0; column < Squares.GetLength(1); column++)
                {
                    Squares[row, column].ResetColour();
                }
            }
        }
    }

    [ToolboxItem(true)]
    public class BoardSquare
    {
        public ChessBoard ChessBoard { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        private bool IsDark { get; set; }
        public Button Button { get; set; }
        public Piece? CurrentPiece { get; set; }
        public bool IsCastleableSquare { get; set; } = false;

        public BoardSquare(int row, int column, bool colour)
        {
            // The row and columns are equal to the row and column set in the ChessBoard class
            Row = row;
            Column = column;

            // If the square is black or white and creates each button based on a template
            IsDark = colour;
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
            if (IsDark)
            {
                ChangeColour(Button, Color.SaddleBrown);
            }
            else
            {
                ChangeColour(Button, Color.BurlyWood);
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

            // Only do everything else if there is no pawn promoting
            if (!ChessBoard.Promoting)
            {
                // Goes through every square on the board and checks to find the one that is currently seleced
                // If it finds one, it gets all the legal moves of the piece on that square and sees if the currently selected square is one of that pieces legal moves
                // If it is, that piece is moved to this square and the method is returned
                // If no piece that is selected is found, the method continues
                foreach (var square in ChessBoard.Squares)
                {
                    if (square.CurrentPiece != null && square.CurrentPiece.IsSelected)
                    {
                        var legalMoves = square.CurrentPiece.InCheckLegalMoves(ChessBoard.Squares);
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
                        var legalMoves = CurrentPiece.InCheckLegalMoves(ChessBoard.Squares);

                        // Highlights legal squares in either dark green or light green depending on if that square is black or white
                        foreach (var move in legalMoves)
                        {
                            move.Button.BackColor = Color.Green;
                            if (move.IsDark)
                            {
                                move.ChangeColour(move.Button, Color.DarkGreen);
                            }
                            else
                            {
                                move.ChangeColour(move.Button, Color.LightGreen);
                            }
                        }

                        // Highlights this square either dark or light yellow depending on if it's black or white
                        if (IsDark)
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
            if (IsDark)
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

            MovePieces(square, this);
            PawnPromotion(square);
            CheckForCheck(square);
            Checkmate(square);
            Castling(square);

            // Changes the turn
            if (!ChessBoard.Promoting)
            {
                ChessBoard.IsWhiteTurn = !ChessBoard.IsWhiteTurn;
            }
        }

        private void MovePieces(BoardSquare originalSquare, BoardSquare newSquare)
        {
            // The current piece is the same as the piece on the old square
            // The piece on the old square is now on this square
            // The piece is no longer selected
            // The square's current piece is now null
            newSquare.CurrentPiece = originalSquare.CurrentPiece;
            originalSquare.CurrentPiece.CurrentBoardSquare = newSquare;
            originalSquare.CurrentPiece.IsSelected = false;
            originalSquare.CurrentPiece = null;

            // Swap the new piece spot's image with the original piece spot's image
            newSquare.Button.Image = originalSquare.Button.Image;
            originalSquare.Button.Image = null;
        }

        private void PawnPromotion(BoardSquare square)
        {
            // If the current piece is a pawn and it's at the end of the board, allow it to promote
            if (CurrentPiece is Pawn whitePawn && !whitePawn.IsBlack && Row == 0)
            {
                whitePawn.Promote();
            }
            else if (CurrentPiece is Pawn blackPawn && blackPawn.IsBlack && Row == 7)
            {
                blackPawn.Promote();
            }
        }

        private void CheckForCheck(BoardSquare square)
        {
            // Check if the team king is in check
            var legalMoves = CurrentPiece.InCheckLegalMoves(ChessBoard.Squares);
            foreach (var move in legalMoves)
            {
                if (move.CurrentPiece == CurrentPiece.GetEnemyKing())
                {
                    CurrentPiece.GetEnemyKing().InCheck = true;
                    break;
                }
                else
                {
                    CurrentPiece.GetEnemyKing().InCheck = false;
                }
            }
        }

        private void Checkmate(BoardSquare square)
        {
            // Check for checkmate
            if (CurrentPiece.GetEnemyKing().InCheck && CurrentPiece.GetEnemyKing().InCheckLegalMoves(ChessBoard.Squares).Count <= 0)
            {
                bool checkmate = false;

                foreach (var space in ChessBoard.Squares)
                {
                    if (space.CurrentPiece != null && space.CurrentPiece.IsBlack != CurrentPiece.IsBlack && space.CurrentPiece.InCheckLegalMoves(ChessBoard.Squares).Count <= 0)
                    {
                        checkmate = true;
                    }
                    else if (space.CurrentPiece != null && space.CurrentPiece.IsBlack != CurrentPiece.IsBlack)
                    {
                        checkmate = false;
                        break;
                    }
                }

                if (checkmate)
                {
                    MessageBox.Show("Checkmate");
                }
            }
        }

        private void Castling(BoardSquare square)
        {
            // Swap pieces when castling
            if (IsCastleableSquare)
            {
                if (Column == 2)
                {
                    MovePieces(ChessBoard.Squares[Row, 0], ChessBoard.Squares[Row, 3]);
                    IsCastleableSquare = false;
                }
                else if (Column == 6)
                {
                    MovePieces(ChessBoard.Squares[Row, 7], ChessBoard.Squares[Row, 5]);
                    IsCastleableSquare = false;
                }
            }

            // If the current piece is a king or rook, set the HasMoved variable to true
            if (CurrentPiece is King king && !king.HasMoved)
            {
                king.HasMoved = true;
            }
            else if (CurrentPiece is Rook rook && !rook.HasMoved)
            {
                rook.HasMoved = true;
            }
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

        // Check each legal move to see if the king is still in check
        public List<BoardSquare> InCheckLegalMoves(BoardSquare[,] board)
        {
            var validMoves = new List<BoardSquare>();
            var legalMoves = GetLegalMoves(board);

            foreach (var move in legalMoves)
            {
                // Save the current state
                var originalSquare = CurrentBoardSquare;
                var originalPieceOnTarget = move.CurrentPiece;

                // Simulate the move
                move.CurrentPiece = this;
                CurrentBoardSquare.CurrentPiece = null;
                CurrentBoardSquare = move;

                // Check if the king is in check
                if (!KingInCheck())
                {
                    validMoves.Add(move);
                }

                // Revert the move
                CurrentBoardSquare = originalSquare;
                originalSquare.CurrentPiece = this;
                move.CurrentPiece = originalPieceOnTarget;
            }

            //if (this is King currentKing && validMoves.Count == 0)
            //{
            //    currentKing.InCheck = true;
            //    MessageBox.Show("King in check");
            //}
            //else if (this is King currentKingNotInCheck)
            //{
            //    currentKingNotInCheck.InCheck = false;
            //    MessageBox.Show("King not in check");
            //}

            return validMoves;
        }


        public bool KingInCheck()
        {
            // Find the king's position  
            BoardSquare? kingSquare = null;
            foreach (var square in CurrentBoardSquare.ChessBoard.Squares)
            {
                if (square.CurrentPiece is King king && king.IsBlack == this.IsBlack)
                {
                    kingSquare = square;
                    break;
                }
            }

            // Ensure kingSquare is not null before proceeding  
            if (kingSquare == null)
            {
                return false; // If no king is found, assume not in check  
            }

            // Check if any opposing piece can attack the king  
            foreach (var square in CurrentBoardSquare.ChessBoard.Squares)
            {
                if (square.CurrentPiece != null && square.CurrentPiece.IsBlack != this.IsBlack)
                {
                    var opponentMoves = square.CurrentPiece.GetLegalMoves(CurrentBoardSquare.ChessBoard.Squares);
                    if (opponentMoves.Contains(kingSquare))
                    {
                        return true; // King is in check  
                    }
                }
            }
            return false; // King is not in check  
        }

        public King GetTeamKing()
        {
            foreach (var square in CurrentBoardSquare.ChessBoard.Squares)
            {
                if (square.CurrentPiece is King king && king.IsBlack == this.IsBlack)
                {
                    return king;
                }
            }
            return null;
        }

        public King GetEnemyKing()
        {
            foreach (var square in CurrentBoardSquare.ChessBoard.Squares)
            {
                if (square.CurrentPiece is King king && king.IsBlack != this.IsBlack)
                {
                    return king;
                }
            }
            return null;
        }
    }

    public class Rook : Piece
    {
        public bool HasMoved { get; set; } = false;
        public bool? OnRight { get; }

        public Rook(BoardSquare startSquare, bool isBlack, bool? onRight) : base(startSquare, isBlack)
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

            OnRight = onRight;
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
        public bool HasMoved { get; set; } = false;
        public bool InCheck { get; set; } = false;

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
            // Check for legal castling moves and add them to the list of legal moves
            moves.AddRange(Castle(moves, board));

            return moves;
        }

        private List<BoardSquare> Castle(List<BoardSquare> moves, BoardSquare[,] board)
        {
            // Searches every square on the board for a rook that is the same colour and has not moved
            foreach (var square in CurrentBoardSquare.ChessBoard.Squares)
            {
                if (square.CurrentPiece is Rook rook && rook.IsBlack == this.IsBlack && !rook.HasMoved && !HasMoved)
                {
                    // If the rook is on the right side of the board, check if the squares between the king and rook are empty
                    if (rook.OnRight == true && board[square.Row, square.Column - 1].CurrentPiece == null
                        && board[square.Row, square.Column - 2].CurrentPiece == null)
                    {
                        moves.Add(board[square.Row, square.Column - 1]);
                        board[square.Row, square.Column - 1].IsCastleableSquare = true;
                    }
                    // If the rook is on the left side of the board, check if the squares between the king and rook are empty
                    else if (rook.OnRight == false && board[square.Row, square.Column + 1].CurrentPiece == null 
                        && board[square.Row, square.Column + 2].CurrentPiece == null
                        && board[square.Row, square.Column + 3].CurrentPiece == null)
                    {
                        moves.Add(board[square.Row, square.Column + 2]);
                        board[square.Row, square.Column + 2].IsCastleableSquare = true;
                    }
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

            // Checks one space in front of the pawn
            var r = row + direction;
            if (r >= 0 && r < board.GetLength(0))
            {
                var square = board[r, column];
                if (square.CurrentPiece == null)
                {
                    moves.Add(square);

                    // Allow the pawn to move two squares on its first turn
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
            }

            // Check diagonal spaces to see if they have enemy pieces, if they do, make them a legal move
            r = row + direction;
            if (r >= 0 && r < board.GetLength(0))
            {
                if (column + 1 < board.GetLength(1))
                {
                    var diagonalSquare = board[r, column + 1];
                    if (diagonalSquare.CurrentPiece != null && diagonalSquare.CurrentPiece.IsBlack != IsBlack)
                    {
                        moves.Add(diagonalSquare);
                    }
                }

                if (column - 1 >= 0)
                {
                    var diagonalSquare = board[r, column - 1];
                    if (diagonalSquare.CurrentPiece != null && diagonalSquare.CurrentPiece.IsBlack != IsBlack)
                    {
                        moves.Add(diagonalSquare);
                    }
                }
            }
            return moves;
        }

        // Creates a new promote screen near the pawn that is promoting
        public void Promote()
        {
            CurrentBoardSquare.ChessBoard.Promoting = true;

            PawnPromoteScreen promoteScreen =  new PawnPromoteScreen(CurrentBoardSquare.ChessBoard, CurrentBoardSquare, IsBlack);
            CurrentBoardSquare.ChessBoard.ChessBoardTable.Parent.Controls.Add(promoteScreen.panel);
            if (IsBlack)
            {
                promoteScreen.IsBlack = true;
            }
            else
            {
                promoteScreen.IsBlack = false;
            }
            promoteScreen.panel.Location = new Point(CurrentBoardSquare.Button.Location.X, CurrentBoardSquare.Button.Location.Y);
        }
    }

    public class Knight : Piece
    {
        public Knight(BoardSquare startSquare, bool isBlack) : base(startSquare, isBlack) 
        {
            if (IsBlack)
            {
                PieceImage = Image.FromFile(@"Resources\BlackPieces\BlackKnight.png");
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