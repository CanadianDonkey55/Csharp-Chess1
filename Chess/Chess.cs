using System;
using System.ComponentModel;

namespace Chess
{
    public partial class Chess : Form
    {
        readonly ChessBoard ChessBoard;
        public Chess()
        {
            InitializeComponent();
            ChessBoard = new ChessBoard(chessBoard);
            newGameButton.ForeColor = Color.White;
        }

        private void Form1_Resize(object sender, System.EventArgs e)
        {
            // Sets variables to window width and height
            var width = this.ClientSize.Width;
            var height = this.ClientSize.Height;

            // The new board width and height are a percentage of the total window width and height
            var newBoardWidth = width * 0.7;
            var newBoardHeight = height * 0.7;

            // Ensures that the board stays a square
            // The width or height is only the smallest of the two dimensions
            if (newBoardWidth < newBoardHeight)
            {
                chessBoard.Width = ((int)newBoardWidth);
                chessBoard.Height = ((int)newBoardWidth);
            }
            else
            {
                chessBoard.Width = ((int)newBoardHeight);
                chessBoard.Height = ((int)newBoardHeight);
            }

            // Sets the board centerpoint
            var chessBoardCenterPoint = new Point((width - chessBoard.Width) / 2, (height - chessBoard.Height) / 2);
            chessBoard.Location = chessBoardCenterPoint;

            // Resize the image on each square
            foreach (var square in ChessBoard.Squares)
            {
                if (square.CurrentPiece == null) continue;
                if (square.CurrentPiece.PieceImage == null) continue;
                square.Button.Image = new Bitmap(square.CurrentPiece.PieceImage, square.Button.Width, square.Button.Height);
            }
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            foreach (var square in ChessBoard.Squares)
            {
                square.CurrentPiece = null;
                square.Button.Image = null;
                ChessBoard.IsWhiteTurn = true;
            }
            ChessBoard.GenerateStartingPieces();
            ChessBoard.ResetAllSquares();
        }
    }

    public class PawnPromoteScreen
    {
        public ChessBoard ChessBoard { get; set; }
        public BoardSquare CurrentBoardSquare { get; set; }
        public bool IsBlack { get; set; }
        public Panel panel;
        private readonly String pieceColour;

        public PawnPromoteScreen(ChessBoard chessBoard, BoardSquare currentBoardSquare, bool isBlack)
        {
            // Gets a reference to the chessboard
            ChessBoard = chessBoard;
            // Gets a reference to if the current promoting piece is black
            IsBlack = isBlack;
            // Creates a new panel to display the screen
            panel = new Panel();

            // The string will be used to get the location of the piece image
            if (IsBlack)
            {
                pieceColour = "Black";
            }
            else
            {
                pieceColour = "White";
            }
            // Displays the screen
            DisplayPanel();
            // Gets a reference to the current square to position the screen better
            CurrentBoardSquare = currentBoardSquare;
        }

        private void DisplayPanel()
        {
            // Gets the location of each of the piece images
            Image knightImage = Image.FromFile(@"Resources\" + pieceColour + @"Pieces\" + pieceColour + "Knight.png");
            Image queenImage = Image.FromFile(@"Resources\" + pieceColour + @"Pieces\" + pieceColour + "Queen.png");
            Image rookImage = Image.FromFile(@"Resources\" + pieceColour + @"Pieces\" + pieceColour + "Rook.png");
            Image bishopImage = Image.FromFile(@"Resources\" + pieceColour + @"Pieces\" + pieceColour + "Bishop.png");

            // Creates the image as the size 
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

            // Gets a random square to use for reference (doesn't matter which so I use 0, 0)
            BoardSquare square = ChessBoard.Squares[0, 0];
            panel.BackColor = Color.White;
            // Needs space for four images, so makes the panel the width of four squares and the height of one
            panel.Width = square.Button.Width * 4;
            panel.Height = square.Button.Height;

            // Adds each button as a control to the panel
            panel.Controls.Add(queenButton);
            panel.Controls.Add(rookButton);
            panel.Controls.Add(bishopButton);
            panel.Controls.Add(knightButton);

            // Assigns each button an event
            queenButton.MouseDown += (sender, e) => PieceButton("queen");
            rookButton.MouseDown += (sender, e) => PieceButton("rook");
            knightButton.MouseDown += (sender, e) => PieceButton("knight");
            bishopButton.MouseDown += (sender, e) => PieceButton("bishop");
        }

        public void PieceButton(String pieceType)
        {
            // Removes the current piece
            CurrentBoardSquare.CurrentPiece = null;

            // Checks for each piece type and creates the new piece on the current square depending on which button was pressed
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

            // Creates the image and assigns the current board square as this piece's square
            if (CurrentBoardSquare.CurrentPiece == null) return;
            CurrentBoardSquare.Button.Image = CurrentBoardSquare.CurrentPiece.PieceImage;
            CurrentBoardSquare.CurrentPiece.CurrentBoardSquare = CurrentBoardSquare;
            
            // Resets the square
            ChessBoard.Promoting = false;
            // Switches turn
            ChessBoard.IsWhiteTurn = !ChessBoard.IsWhiteTurn;

            // Removes the panel
            if (panel.Parent == null) return;
            panel.Parent.Controls.Remove(panel);
        }
    }

    public class ChessBoard
    {
        public bool Promoting { get; set; } = false;
        public BoardSquare[,] Squares { get; set; }
        public BoardSquare? PreviousMove { get; set; }
        public TableLayoutPanel ChessBoardTable { get; set; }
        public bool IsWhiteTurn { get; set; } = true;

        // Contructor
        public ChessBoard(TableLayoutPanel chessBoardPanel)
        {
            // White always goes first
            IsWhiteTurn = true;

            // The table is equal to the table in the designer
            ChessBoardTable = chessBoardPanel;

            // Generates the board wuth all their squares
            Squares = GenerateBoard();

            // Places the pieces on their starting squares
            GenerateStartingPieces();
        }

        private BoardSquare[,] GenerateBoard()
        {
            var rows = ChessBoardTable.RowCount;
            var columns = ChessBoardTable.ColumnCount;

            var squares = new BoardSquare[rows, columns];

            bool isDark = false;

            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    var square = new BoardSquare(row, column, isDark, this);
                    squares[row, column] = square;
                    square.ChessBoard = this;

                    ChessBoardTable.Controls.Add(square.Button, column, row);
                    isDark = !isDark;
                }
                isDark = !isDark;
            }

            return squares;
        }

        public void GenerateStartingPieces()
        {
            // The first value of the array is their row, the second is the column
            // The bool is if the piece is black or white (true = white, false = black)

            // Rooks
            // The second bool is to show if the rook starts on the left (false) or right (true)
            _ = new Rook(Squares[0, 0], true, false);
            _ = new Rook(Squares[0, 7], true, true);
            _ = new Rook(Squares[7, 0], false, false);
            _ = new Rook(Squares[7, 7], false, true);

            // Bishops
            _ = new Bishop(Squares[0, 2], true);
            _ = new Bishop(Squares[0, 5], true);
            _ = new Bishop(Squares[7, 2], false);
            _ = new Bishop(Squares[7, 5], false);

            // Queens
            _ = new Queen(Squares[0, 3], true);
            _ = new Queen(Squares[7, 3], false);

            // Kings
            _ = new King(Squares[0, 4], true);
            _ = new King(Squares[7, 4], false);

            // White Pawns
            _ = new Pawn(Squares[6, 0], false);
            _ = new Pawn(Squares[6, 1], false);
            _ = new Pawn(Squares[6, 2], false);
            _ = new Pawn(Squares[6, 3], false);
            _ = new Pawn(Squares[6, 4], false);
            _ = new Pawn(Squares[6, 5], false);
            _ = new Pawn(Squares[6, 6], false);
            _ = new Pawn(Squares[6, 7], false);

            // Black Pawns
            _ = new Pawn(Squares[1, 0], true);
            _ = new Pawn(Squares[1, 1], true);
            _ = new Pawn(Squares[1, 2], true);
            _ = new Pawn(Squares[1, 3], true);
            _ = new Pawn(Squares[1, 4], true);
            _ = new Pawn(Squares[1, 5], true);
            _ = new Pawn(Squares[1, 6], true);
            _ = new Pawn(Squares[1, 7], true);

            // Knights
            _ = new Knight(Squares[0, 1], true);
            _ = new Knight(Squares[0, 6], true);
            _ = new Knight(Squares[7, 1], false);
            _ = new Knight(Squares[7, 6], false);
        }

        // Resets each square back to default
        public void ResetAllSquares()
        {
            foreach (var square in Squares)
            {
                square.IsCastleableSquare = false;
                square.IsEnPassantSquare = false;
                square.ResetColour();
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
        public bool IsEnPassantSquare { get; set; } = false;

        public BoardSquare(int row, int column, bool colour, ChessBoard chessBoard)
        {
            ChessBoard = chessBoard;

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

            var square = new Button
            {
                FlatStyle = FlatStyle.Flat
            };
            this.Button = square;
            ResetColour();
            square.Dock = DockStyle.Fill;

            square.MouseDown += (sender, e) => OnSquareClick(e);
            return square;
        }

        private static void ChangeColour(Button square, Color colour)
        {
            // Each square colour is equal to the colour set in the constructor

            square.BackColor = colour;
            square.FlatAppearance.MouseDownBackColor = colour;
            square.FlatAppearance.MouseOverBackColor = colour;
            square.FlatAppearance.BorderColor = colour;
        }

        public void ResetColour()
        {
            // Makes each square either dark or light
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
            ChessBoard.ResetAllSquares();

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
                                ChangeColour(move.Button, Color.DarkGreen);
                            }
                            else
                            {
                                ChangeColour(move.Button, Color.LightGreen);
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
            PawnPromotion();
            CheckForCheck();
            Checkmate();
            Castling();
            EnPassant();

            // Keeps track of the previous move
            ChessBoard.PreviousMove = null;
            ChessBoard.PreviousMove = this;

            // Changes the turn
            if (!ChessBoard.Promoting)
            {
                ChessBoard.IsWhiteTurn = !ChessBoard.IsWhiteTurn;
            }
        }

        private static void MovePieces(BoardSquare originalSquare, BoardSquare newSquare)
        {
            // Ensure originalSquare.CurrentPiece is not null before accessing its properties
            if (originalSquare.CurrentPiece != null)
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
        }

        private void PawnPromotion()
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

        private void CheckForCheck()
        {
            if (CurrentPiece == null)
            {
                return;
            }

            // Check if the team king is in check  
            var legalMoves = CurrentPiece.InCheckLegalMoves(ChessBoard.Squares);
            foreach (var move in legalMoves)
            {
                var enemyKing = CurrentPiece.GetEnemyKing();
                if (enemyKing != null && move.CurrentPiece == enemyKing)
                {
                    enemyKing.InCheck = true;
                    break;
                }
                else if (enemyKing != null)
                {
                    enemyKing.InCheck = false;
                }
            }
        }

        private void Checkmate()
        {
            if (CurrentPiece == null)
            {
                return;
            }

            // Get the enemy king and ensure it's not null before accessing its properties  
            var enemyKing = CurrentPiece.GetEnemyKing();
            if (enemyKing != null && enemyKing.InCheck && enemyKing.InCheckLegalMoves(ChessBoard.Squares).Count <= 0)
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

                // Shows a message displaying who won  
                if (checkmate)
                {
                    if (ChessBoard.IsWhiteTurn)
                    {
                        MessageBox.Show("White wins!");
                    }
                    else
                    {
                        MessageBox.Show("Black wins!");
                    }
                }
            }
        }

        private void Castling()
        {
            // Move pieces to their new locations when castling
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

        private void EnPassant()
        {
            if (CurrentPiece == null)
            {
                return;
            }

            // Removes the piece on he square behind the pawn after doing en passant
            if (IsEnPassantSquare)
            {
                if (CurrentPiece.IsBlack)
                {
                    ChessBoard.Squares[Row - 1, Column].CurrentPiece = null;
                    ChessBoard.Squares[Row - 1, Column].Button.Image = null;
                } 
                else if (!CurrentPiece.IsBlack)
                {
                    ChessBoard.Squares[Row + 1, Column].CurrentPiece = null;
                    ChessBoard.Squares[Row + 1, Column].Button.Image = null;
                }

                IsEnPassantSquare = false;
            }
        }
    }

    #region Pieces
    public abstract class Piece
    {
        public bool IsBlack { get; set; }
        public BoardSquare CurrentBoardSquare { get; set; }
        public Image? PieceImage { get; set; }
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

        public King? GetEnemyKing()
        {
            // Checks each square for a king of the opposite colour as this piece
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

            // Only checks one square in each direction
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

            // If en passant is possible, add it as a legal move
            var enPassantSquare = EnPassantSquare(board);
            if (enPassantSquare != null)
            {
                legalMoves.Add(enPassantSquare);
            }

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
                        square = board[r + direction, column];
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
            if (CurrentBoardSquare.ChessBoard.ChessBoardTable.Parent == null)
            {
                return;
            }

            CurrentBoardSquare.ChessBoard.Promoting = true;

            PawnPromoteScreen promoteScreen =  new(CurrentBoardSquare.ChessBoard, CurrentBoardSquare, IsBlack);
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

        private BoardSquare? EnPassantSquare(BoardSquare[,] board)
        {
            // If there hasn't already been a move, return null
            if (CurrentBoardSquare.ChessBoard.PreviousMove == null)
            {
                return null;
            }

            // If the previous move was a pawn's first turn, it's not the same color as this pawn, both pawns are on the same row, they are on columns directly next to each other,
            // and it's one of the two rows the en passant can happen on
            if (CurrentBoardSquare.ChessBoard.PreviousMove.CurrentPiece is Pawn pawn && !pawn.firstTurn && pawn.IsBlack != IsBlack && CurrentBoardSquare.Row == pawn.CurrentBoardSquare.Row &&
                (pawn.CurrentBoardSquare.Column == CurrentBoardSquare.Column + 1 || pawn.CurrentBoardSquare.Column == CurrentBoardSquare.Column - 1) &&
                (pawn.CurrentBoardSquare.Row == 3 || pawn.CurrentBoardSquare.Row == 4))
            {
                // Keeps track of the direction you will pass in (1 = right, -1 = left)
                int passDirection;
                // Keeps track of the direction the pawn is moving in (1 = down, -1 = up)
                int moveDirection;
                if (IsBlack)
                {
                    moveDirection = 1;
                    // Can't en passant if the pawn isn't far enough forward
                    if (pawn.CurrentBoardSquare.Row == 3)
                    {
                        return null;
                    }
                }
                else
                {
                    moveDirection = -1;
                    // Can't en passant if the pawn isn't far enough forward
                    if (pawn.CurrentBoardSquare.Row == 4)
                    {
                        return null;
                    }
                }

                // Checks where the opposing pawn is to check which direction to pass from
                if (pawn.CurrentBoardSquare.Column > CurrentBoardSquare.Column)
                {
                    passDirection = 1;
                }
                else
                {
                    passDirection = -1;
                }

                var enPassantSquare = board[CurrentBoardSquare.Row + moveDirection, CurrentBoardSquare.Column + passDirection];
                // Marks it as an EnPassantSquare for logic later
                enPassantSquare.IsEnPassantSquare = true;
                return enPassantSquare;
            }
            // If it doesn't meet all of those conditions, return no squares
            else
            {
                return null;
            }
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