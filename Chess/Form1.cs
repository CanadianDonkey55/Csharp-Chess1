namespace Chess
{
    public partial class Form1 : Form
    {
        BoardSquare[,] squares;

        public Form1()
        {
            InitializeComponent();
            GenerateBoard();
        }

        private void Form1_Resize(object sender, System.EventArgs e)
        {
            var width = this.ClientSize.Width;
            var height = this.ClientSize.Height;

            var newBoardWidth = 
            var newBoardHeight = height - chessBoard.Height;

            chessBoard.Width = newBoardWidth;
            chessBoard.Height = newBoardHeight;

            Point chessBoardCenterPoint = new Point((width - chessBoard.Width) / 2, (height - chessBoard.Height) / 2);
            chessBoard.Location = chessBoardCenterPoint;
        }

        private void GenerateBoard()
        {
            var rows = chessBoard.RowCount;
            var columns = chessBoard.ColumnCount;

            squares = new BoardSquare[rows, columns];

            var colInt = 0;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    chessBoard.Controls.Add(new BoardSquare().SquareTemplate(colInt), c, r);
                    colInt++;
                }
                colInt--;
            }
        }
    }

    public class BoardSquare
    {
        public Button SquareTemplate(int backColour)
        {
            Button square = new Button();

            square.FlatStyle = FlatStyle.Flat;
            Color colour;

            if (backColour % 2 == 0)
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
            MessageBox.Show("");
        }
    }

    public abstract class Pieces
    {
        private BoardSquare currentBoardSquare;


    }
}
