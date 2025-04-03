namespace Chess
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void chessBoard_Paint(object sender, PaintEventArgs e)
        {
            var height = chessBoard.RowCount;
            var width = chessBoard.ColumnCount;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Button button = new Button { Text = "1"};
                    chessBoard.Controls.Add(button, x, y);
                }
            }
        }
    }
}
