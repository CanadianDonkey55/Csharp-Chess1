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

            Button button = new Button();
            button.Text = "1";
            button.Dock = DockStyle.Fill;
            button.Visible = true;
            chessBoard.Controls.Add(button, height, width);

            label1.Text = height.ToString();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    
                }
            }
        }
    }
}
