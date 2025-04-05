namespace Chess
{
    public partial class Form1 : Form
    {
        readonly ChessBoard board;

        public Form1()
        {
            InitializeComponent();
            board = new ChessBoard(chessBoard);
        }

        private void Form1_Resize(object sender, System.EventArgs e)
        {
            var width = this.ClientSize.Width;
            var height = this.ClientSize.Height;

            var newBoardWidth = (int)(width * 0.7);
            var newBoardHeight = (int)(height * 0.7);

            if (newBoardWidth < newBoardHeight)
            {
                chessBoard.Width = newBoardWidth;
                chessBoard.Height = newBoardWidth;
            } else
            {
                chessBoard.Width = newBoardHeight;
                chessBoard.Height = newBoardHeight;
            }

            Point chessBoardCenterPoint = new Point((width - chessBoard.Width) / 2, (height - chessBoard.Height) / 2);
            chessBoard.Location = chessBoardCenterPoint;
        }
    }

    public class ChessBoard
    {
        public BoardSquare[,] squares { get; set; }

        private TableLayoutPanel chessBoard;

        public ChessBoard(TableLayoutPanel chessBoardPanel)
        {
            chessBoard = chessBoardPanel;
            GenerateBoard();
        }

        private void GenerateBoard()
        {
            var rows = chessBoard.RowCount;
            var columns = chessBoard.ColumnCount;

            squares = new BoardSquare[rows, columns];

            bool colInt = false;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    var square = new BoardSquare(r, c, colInt);
                    squares[r, c] = square;

                    chessBoard.Controls.Add(square.Button, c, r);
                    colInt = !colInt;
                }
                colInt = !colInt;
            }
        }
    }

    public class BoardSquare
    {
        public int Row {  get; set; }
        public int Column { get; set; }
        public bool Colour { get; set; }
        public Button Button { get; set; }

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
            square.BackColor = colour;
            
            square.FlatAppearance.MouseDownBackColor = colour;
            square.FlatAppearance.MouseOverBackColor = colour;
            square.FlatAppearance.BorderColor = colour;
            square.Click += (sender, e) => OnSquareClick();

            return square;
        }

        public void OnSquareClick()
        {
            MessageBox.Show($"You clicked square ({Row + 1}, {Column + 1})");
        }
    }

    public abstract class Pieces
    {
        private BoardSquare currentBoardSquare;


    }
}
