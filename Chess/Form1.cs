namespace Chess
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            GenerateBoard();
        }

        private void GenerateBoard()
        {
            var height = chessBoard.RowCount;
            var width = chessBoard.ColumnCount;

            var colInt = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    chessBoard.Controls.Add(new BoardSquare().SquareTemplate(colInt), x, y);
                    colInt++;
                }
                colInt--;
            }
        }
    }

    public class BoardSquare : Button
    {
        public Button SquareTemplate(int backColour)
        {
            Button square = new Button();
            Color colour = new Color();

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
            square.Click += (sender, e) => OnSquareClick();
            return square;
        }

        public void OnSquareClick()
        {
            Application.Exit();
        }
    }
}
