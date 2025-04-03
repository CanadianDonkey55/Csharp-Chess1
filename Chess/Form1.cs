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
            //chessBoard.Controls.Add(button, height, width);

            label1.Text = height.ToString();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    chessBoard.Controls.Add(new Button { Text = x.ToString() + ", " + y.ToString(), Dock = DockStyle.Fill, BackColor = Color.Black, ForeColor = Color.White }, x, y);
                }
            }
        }

        private void chessBoard_Paint(object sender, PaintEventArgs e)
        {
            
        }
    }

    public class BoardSquare : Button
    {
        
    }
}
