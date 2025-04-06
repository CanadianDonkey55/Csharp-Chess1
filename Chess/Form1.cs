namespace Chess
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ChessBoard board = new ChessBoard(chessBoard);
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
            Point chessBoardCenterPoint = new Point((width - chessBoard.Width) / 2, (height - chessBoard.Height) / 2);
            chessBoard.Location = chessBoardCenterPoint;
        }
    }

    public class ChessBoard
    {
        public BoardSquare[,] Squares { get; set; }

        public TableLayoutPanel ChessBoardTable { get; }

        public ChessBoard(TableLayoutPanel chessBoardPanel)
        {
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

        public void GenerateStartingPieces()
        {
            new Rook(Squares[0, 0], true);
            new Rook(Squares[0, 7], true);
            new Rook(Squares[7, 0], false);
            new Rook(Squares[7, 7], false);
        }
    }
         
    public class BoardSquare
    {
        public ChessBoard ChessBoard {  get; set; }
        public int Row {  get; set; }
        public int Column { get; set; }
        public bool Colour { get; set; }
        public Button Button { get; set; }
        public Piece CurrentPiece { get; set; }

        public BoardSquare(int row, int column, bool colour)
        {
            Row = row;
            Column = column;

            Colour = colour;
            Button = SquareTemplate(colour);
        }

        public Button SquareTemplate(bool backColour)
        {
            var square = new Button();

            square.FlatStyle = FlatStyle.Flat;
            Color colour;

            if (!backColour)
            {
                colour = Color.White;
            } 
            else
            {
                colour = Color.Black;
            }

            square.Dock = DockStyle.Fill;

            ChangeColour(square, colour);

            square.Click += (sender, e) => OnSquareClick();

            return square;
        }

        public void ChangeColour(Button square, Color colour)
        {
            square.BackColor = colour;
            square.FlatAppearance.MouseDownBackColor = colour;
            square.FlatAppearance.MouseOverBackColor = colour;
            square.FlatAppearance.BorderColor = colour;
        }

        public void OnSquareClick()
        {
            if (CurrentPiece != null)
            {
                var legalMoves = CurrentPiece.GetLegalMoves(ChessBoard.Squares);

                foreach (var move in legalMoves)
                {
                    move.Button.BackColor = Color.Green;
                    if (move.Colour)
                    {
                        move.ChangeColour(move.Button, Color.DarkGreen);
                    }
                    else
                    {
                        move.ChangeColour(move.Button, Color.LightGreen);
                    }
                }
            } 
            else
            {
                return;
            }
        }
    }

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
}