namespace SpreadsheetGUI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newSpreadsheetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.applicationInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spreadsheetPanel1 = new SS.SpreadsheetPanel();
            this.CellNameTextBox = new System.Windows.Forms.TextBox();
            this.CellValueTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.CellContentTextBox = new System.Windows.Forms.TextBox();
            this.InputLabel = new System.Windows.Forms.Label();
            this.ValueLabel = new System.Windows.Forms.Label();
            this.ContentChangeButton = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.UsernameLabel = new System.Windows.Forms.Label();
            this.IPLabel = new System.Windows.Forms.Label();
            this.UsernameTextBox = new System.Windows.Forms.TextBox();
            this.IPTextBox = new System.Windows.Forms.TextBox();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.NetworkInfoLabel = new System.Windows.Forms.Label();
            this.consoleTextBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.NetworkInputLabel = new System.Windows.Forms.Label();
            this.InputEnterButton = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(781, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newSpreadsheetToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.openToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newSpreadsheetToolStripMenuItem
            // 
            this.newSpreadsheetToolStripMenuItem.Name = "newSpreadsheetToolStripMenuItem";
            this.newSpreadsheetToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.newSpreadsheetToolStripMenuItem.Text = "New Spreadsheet";
            this.newSpreadsheetToolStripMenuItem.Click += new System.EventHandler(this.newSpreadsheetToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.applicationInfoToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // applicationInfoToolStripMenuItem
            // 
            this.applicationInfoToolStripMenuItem.Name = "applicationInfoToolStripMenuItem";
            this.applicationInfoToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.applicationInfoToolStripMenuItem.Text = "Application Info";
            this.applicationInfoToolStripMenuItem.Click += new System.EventHandler(this.applicationInfoToolStripMenuItem_Click);
            // 
            // spreadsheetPanel1
            // 
            this.spreadsheetPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spreadsheetPanel1.Location = new System.Drawing.Point(0, 67);
            this.spreadsheetPanel1.Name = "spreadsheetPanel1";
            this.spreadsheetPanel1.Size = new System.Drawing.Size(525, 372);
            this.spreadsheetPanel1.TabIndex = 1;
            this.spreadsheetPanel1.Load += new System.EventHandler(this.spreadsheetPanel1_Load);
            // 
            // CellNameTextBox
            // 
            this.CellNameTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.CellNameTextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellNameTextBox.Location = new System.Drawing.Point(12, 41);
            this.CellNameTextBox.Multiline = true;
            this.CellNameTextBox.Name = "CellNameTextBox";
            this.CellNameTextBox.ReadOnly = true;
            this.CellNameTextBox.Size = new System.Drawing.Size(40, 20);
            this.CellNameTextBox.TabIndex = 2;
            // 
            // CellValueTextBox
            // 
            this.CellValueTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.CellValueTextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellValueTextBox.Location = new System.Drawing.Point(79, 41);
            this.CellValueTextBox.Multiline = true;
            this.CellValueTextBox.Name = "CellValueTextBox";
            this.CellValueTextBox.ReadOnly = true;
            this.CellValueTextBox.Size = new System.Drawing.Size(133, 20);
            this.CellValueTextBox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 4;
            // 
            // CellContentTextBox
            // 
            this.CellContentTextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellContentTextBox.Location = new System.Drawing.Point(258, 41);
            this.CellContentTextBox.Name = "CellContentTextBox";
            this.CellContentTextBox.Size = new System.Drawing.Size(204, 23);
            this.CellContentTextBox.TabIndex = 6;
            // 
            // InputLabel
            // 
            this.InputLabel.AutoSize = true;
            this.InputLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InputLabel.Location = new System.Drawing.Point(218, 44);
            this.InputLabel.Name = "InputLabel";
            this.InputLabel.Size = new System.Drawing.Size(38, 15);
            this.InputLabel.TabIndex = 5;
            this.InputLabel.Text = "Input:";
            // 
            // ValueLabel
            // 
            this.ValueLabel.AutoSize = true;
            this.ValueLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ValueLabel.Location = new System.Drawing.Point(58, 44);
            this.ValueLabel.Name = "ValueLabel";
            this.ValueLabel.Size = new System.Drawing.Size(15, 15);
            this.ValueLabel.TabIndex = 7;
            this.ValueLabel.Text = "=";
            // 
            // ContentChangeButton
            // 
            this.ContentChangeButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ContentChangeButton.Location = new System.Drawing.Point(468, 40);
            this.ContentChangeButton.Name = "ContentChangeButton";
            this.ContentChangeButton.Size = new System.Drawing.Size(43, 23);
            this.ContentChangeButton.TabIndex = 8;
            this.ContentChangeButton.Text = "Enter";
            this.ContentChangeButton.UseVisualStyleBackColor = true;
            this.ContentChangeButton.Click += new System.EventHandler(this.ContentChangeButton_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "sprd";
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "sprd";
            // 
            // UsernameLabel
            // 
            this.UsernameLabel.AutoSize = true;
            this.UsernameLabel.Location = new System.Drawing.Point(92, 7);
            this.UsernameLabel.Name = "UsernameLabel";
            this.UsernameLabel.Size = new System.Drawing.Size(58, 13);
            this.UsernameLabel.TabIndex = 9;
            this.UsernameLabel.Text = "Username:";
            this.UsernameLabel.Click += new System.EventHandler(this.UsernameLabel_Click);
            // 
            // IPLabel
            // 
            this.IPLabel.AutoSize = true;
            this.IPLabel.Location = new System.Drawing.Point(259, 5);
            this.IPLabel.Name = "IPLabel";
            this.IPLabel.Size = new System.Drawing.Size(20, 13);
            this.IPLabel.TabIndex = 10;
            this.IPLabel.Text = "IP:";
            this.IPLabel.Click += new System.EventHandler(this.IPLabel_Click);
            // 
            // UsernameTextBox
            // 
            this.UsernameTextBox.Location = new System.Drawing.Point(153, 2);
            this.UsernameTextBox.Name = "UsernameTextBox";
            this.UsernameTextBox.Size = new System.Drawing.Size(100, 20);
            this.UsernameTextBox.TabIndex = 11;
            this.UsernameTextBox.TextChanged += new System.EventHandler(this.UsernameTextBox_TextChanged);
            // 
            // IPTextBox
            // 
            this.IPTextBox.Location = new System.Drawing.Point(282, 2);
            this.IPTextBox.Name = "IPTextBox";
            this.IPTextBox.Size = new System.Drawing.Size(100, 20);
            this.IPTextBox.TabIndex = 12;
            this.IPTextBox.TextChanged += new System.EventHandler(this.IPTextBox_TextChanged);
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(388, 2);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(74, 22);
            this.ConnectButton.TabIndex = 13;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // NetworkInfoLabel
            // 
            this.NetworkInfoLabel.AutoSize = true;
            this.NetworkInfoLabel.Location = new System.Drawing.Point(531, 50);
            this.NetworkInfoLabel.Name = "NetworkInfoLabel";
            this.NetworkInfoLabel.Size = new System.Drawing.Size(105, 13);
            this.NetworkInfoLabel.TabIndex = 15;
            this.NetworkInfoLabel.Text = "Network Info Display";
            // 
            // consoleTextBox1
            // 
            this.consoleTextBox1.BackColor = System.Drawing.SystemColors.WindowText;
            this.consoleTextBox1.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.consoleTextBox1.ForeColor = System.Drawing.SystemColors.Window;
            this.consoleTextBox1.Location = new System.Drawing.Point(534, 67);
            this.consoleTextBox1.Multiline = true;
            this.consoleTextBox1.Name = "consoleTextBox1";
            this.consoleTextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.consoleTextBox1.Size = new System.Drawing.Size(225, 126);
            this.consoleTextBox1.TabIndex = 16;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(534, 231);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(225, 20);
            this.textBox2.TabIndex = 17;
            // 
            // NetworkInputLabel
            // 
            this.NetworkInputLabel.AutoSize = true;
            this.NetworkInputLabel.Location = new System.Drawing.Point(534, 212);
            this.NetworkInputLabel.Name = "NetworkInputLabel";
            this.NetworkInputLabel.Size = new System.Drawing.Size(74, 13);
            this.NetworkInputLabel.TabIndex = 18;
            this.NetworkInputLabel.Text = "Network Input";
            // 
            // InputEnterButton
            // 
            this.InputEnterButton.Location = new System.Drawing.Point(534, 258);
            this.InputEnterButton.Name = "InputEnterButton";
            this.InputEnterButton.Size = new System.Drawing.Size(75, 23);
            this.InputEnterButton.TabIndex = 19;
            this.InputEnterButton.Text = "Enter";
            this.InputEnterButton.UseVisualStyleBackColor = true;
            this.InputEnterButton.Click += new System.EventHandler(this.InputEnterButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(781, 451);
            this.Controls.Add(this.InputEnterButton);
            this.Controls.Add(this.NetworkInputLabel);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.consoleTextBox1);
            this.Controls.Add(this.NetworkInfoLabel);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.IPTextBox);
            this.Controls.Add(this.UsernameTextBox);
            this.Controls.Add(this.IPLabel);
            this.Controls.Add(this.UsernameLabel);
            this.Controls.Add(this.ContentChangeButton);
            this.Controls.Add(this.ValueLabel);
            this.Controls.Add(this.InputLabel);
            this.Controls.Add(this.CellContentTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CellValueTextBox);
            this.Controls.Add(this.CellNameTextBox);
            this.Controls.Add(this.spreadsheetPanel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "new1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newSpreadsheetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem applicationInfoToolStripMenuItem;
        private SS.SpreadsheetPanel spreadsheetPanel1;
        private System.Windows.Forms.TextBox CellNameTextBox;
        private System.Windows.Forms.TextBox CellValueTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox CellContentTextBox;
        private System.Windows.Forms.Label InputLabel;
        private System.Windows.Forms.Label ValueLabel;
        private System.Windows.Forms.Button ContentChangeButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label UsernameLabel;
        private System.Windows.Forms.Label IPLabel;
        private System.Windows.Forms.TextBox UsernameTextBox;
        private System.Windows.Forms.TextBox IPTextBox;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.Label NetworkInfoLabel;
        private System.Windows.Forms.TextBox consoleTextBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label NetworkInputLabel;
        private System.Windows.Forms.Button InputEnterButton;
    }
}

