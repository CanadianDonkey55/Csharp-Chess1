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

            Button button = new Button();
            button.Text = "1";
            button.Dock = DockStyle.Fill;
            button.BackColor = Color.Black;
            button.Visible = true;

            label1.Text = height.ToString();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    chessBoard.Controls.Add(new BoardSquare { Text = x.ToString() + ", " + y.ToString(), Dock = DockStyle.Fill, BackColor = Color.Black, ForeColor = Color.White }, x, y);
                }
            }
        }
    }

    public class BoardSquare : Button
    {
        public void NewSquare()
        {
            
        }
    }
}
