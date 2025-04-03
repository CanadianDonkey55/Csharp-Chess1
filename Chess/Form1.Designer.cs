namespace Chess
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            chessBoard = new TableLayoutPanel();
            SuspendLayout();
            // 
            // chessBoard
            // 
            chessBoard.ColumnCount = 8;
            chessBoard.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            chessBoard.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            chessBoard.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            chessBoard.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            chessBoard.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            chessBoard.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            chessBoard.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            chessBoard.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            chessBoard.Location = new Point(333, 10);
            chessBoard.Name = "chessBoard";
            chessBoard.RowCount = 8;
            chessBoard.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            chessBoard.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            chessBoard.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            chessBoard.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            chessBoard.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            chessBoard.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            chessBoard.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            chessBoard.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            chessBoard.Size = new Size(700, 700);
            chessBoard.TabIndex = 0;
            chessBoard.Paint += chessBoard_Paint;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1336, 722);
            Controls.Add(chessBoard);
            Name = "Form1";
            Text = "Chess";
            WindowState = FormWindowState.Maximized;
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel chessBoard;
    }
}
