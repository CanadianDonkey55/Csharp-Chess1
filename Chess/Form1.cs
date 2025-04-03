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

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var colInt = 0;
                    var divXY = (x + 1) / (y + 1);

                    if (divXY %  2 == 0)
                    {
                        colInt = 1;
                    } else
                    {
                        colInt = 0;
                    }
                    

                    chessBoard.Controls.Add(new BoardSquare().SquareTemplate(colInt), x, y);
                }
            }
        }
    }

    public class BoardSquare : Button
    {
        protected Control control = new Control();

        public Control SquareTemplate(int backColour)
        {
            Color colour = new Color();

            if (backColour == 0)
            {
                colour = Color.White;
            } else if (backColour == 1)
            {
                colour = Color.Black;
            }

                control.Dock = DockStyle.Fill;
            control.BackColor = colour;
            return control;
        }
    }
}
