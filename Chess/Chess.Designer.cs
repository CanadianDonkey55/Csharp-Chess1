namespace Chess
{
    partial class Chess
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
            label1 = new Label();
            newGameButton = new Button();
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
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(1121, 24);
            label1.Name = "label1";
            label1.Size = new Size(0, 15);
            label1.TabIndex = 1;
            // 
            // newGameButton
            // 
            newGameButton.BackColor = Color.FromArgb(0, 64, 0);
            newGameButton.Location = new Point(1073, 28);
            newGameButton.Name = "newGameButton";
            newGameButton.Size = new Size(121, 51);
            newGameButton.TabIndex = 2;
            newGameButton.Text = "New Game";
            newGameButton.UseVisualStyleBackColor = false;
            newGameButton.Click += newGameButton_Click;
            // 
            // Chess
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.WindowFrame;
            ClientSize = new Size(1336, 722);
            Controls.Add(newGameButton);
            Controls.Add(label1);
            Controls.Add(chessBoard);
            Name = "Chess";
            Text = "Chess";
            WindowState = FormWindowState.Maximized;
            Resize += Form1_Resize;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel chessBoard;
        public Label label1;
        private Button newGameButton;
    }
}
