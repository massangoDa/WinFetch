namespace WIn_UUP_Iso
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
            richTextBox1 = new RichTextBox();
            comboBox1 = new ComboBox();
            label1 = new Label();
            button1 = new Button();
            comboBox3 = new ComboBox();
            label3 = new Label();
            label4 = new Label();
            comboBox4 = new ComboBox();
            groupBox1 = new GroupBox();
            button2 = new Button();
            button4 = new Button();
            groupBox2 = new GroupBox();
            button5 = new Button();
            label5 = new Label();
            progressBar1 = new ProgressBar();
            label2 = new Label();
            button3 = new Button();
            ファイルToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1 = new MenuStrip();
            pictureBox1 = new PictureBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = Color.FromArgb(51, 51, 51);
            richTextBox1.BorderStyle = BorderStyle.None;
            richTextBox1.Location = new Point(437, 35);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.ScrollBars = RichTextBoxScrollBars.None;
            richTextBox1.Size = new Size(435, 500);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "";
            // 
            // comboBox1
            // 
            comboBox1.BackColor = Color.LightGray;
            comboBox1.Font = new Font("Yu Gothic UI", 20F);
            comboBox1.FormattingEnabled = true;
            comboBox1.ItemHeight = 37;
            comboBox1.Items.AddRange(new object[] { "amd64", "arm64" });
            comboBox1.Location = new Point(201, 16);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(173, 45);
            comboBox1.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Yu Gothic UI", 20F);
            label1.Location = new Point(15, 19);
            label1.Name = "label1";
            label1.Size = new Size(180, 37);
            label1.TabIndex = 3;
            label1.Text = "アーキテクチャ：";
            // 
            // button1
            // 
            button1.BackColor = Color.DimGray;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatAppearance.MouseOverBackColor = Color.FromArgb(55, 55, 55);
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Yu Gothic UI", 20F);
            button1.ForeColor = Color.White;
            button1.Location = new Point(281, 118);
            button1.Name = "button1";
            button1.Size = new Size(93, 50);
            button1.TabIndex = 6;
            button1.Text = "検索";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // comboBox3
            // 
            comboBox3.BackColor = Color.LightGray;
            comboBox3.Enabled = false;
            comboBox3.FormattingEnabled = true;
            comboBox3.Location = new Point(15, 63);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(368, 23);
            comboBox3.TabIndex = 7;
            comboBox3.Text = "ビルドバージョンの選択";
            comboBox3.SelectedIndexChanged += comboBox3_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Yu Gothic UI", 15F);
            label3.Location = new Point(18, 209);
            label3.Name = "label3";
            label3.Size = new Size(0, 28);
            label3.TabIndex = 8;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Yu Gothic UI", 20F);
            label4.Location = new Point(53, 70);
            label4.Name = "label4";
            label4.Size = new Size(142, 37);
            label4.TabIndex = 9;
            label4.Text = "バージョン：";
            // 
            // comboBox4
            // 
            comboBox4.BackColor = Color.LightGray;
            comboBox4.Font = new Font("Yu Gothic UI", 20F);
            comboBox4.FormattingEnabled = true;
            comboBox4.ItemHeight = 37;
            comboBox4.Items.AddRange(new object[] { "Windows 11", "Windows 10" });
            comboBox4.Location = new Point(201, 67);
            comboBox4.Name = "comboBox4";
            comboBox4.Size = new Size(173, 45);
            comboBox4.TabIndex = 10;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(comboBox4);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(button1);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(comboBox1);
            groupBox1.Location = new Point(12, 27);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(394, 180);
            groupBox1.TabIndex = 11;
            groupBox1.TabStop = false;
            // 
            // button2
            // 
            button2.BackColor = Color.DimGray;
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatAppearance.MouseOverBackColor = Color.FromArgb(55, 55, 55);
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Yu Gothic UI", 20F);
            button2.ForeColor = Color.White;
            button2.Location = new Point(252, 485);
            button2.Name = "button2";
            button2.Size = new Size(154, 50);
            button2.TabIndex = 12;
            button2.Text = "ダウンロード";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // button4
            // 
            button4.BackColor = Color.DimGray;
            button4.FlatAppearance.BorderSize = 0;
            button4.FlatAppearance.MouseOverBackColor = Color.FromArgb(55, 55, 55);
            button4.FlatStyle = FlatStyle.Flat;
            button4.Font = new Font("Yu Gothic UI", 16F);
            button4.ForeColor = Color.White;
            button4.Location = new Point(129, 485);
            button4.Name = "button4";
            button4.Size = new Size(105, 50);
            button4.TabIndex = 16;
            button4.Text = "ISO作成";
            button4.UseVisualStyleBackColor = false;
            button4.Click += button4_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(button5);
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(progressBar1);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(comboBox3);
            groupBox2.Location = new Point(12, 213);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(394, 266);
            groupBox2.TabIndex = 17;
            groupBox2.TabStop = false;
            // 
            // button5
            // 
            button5.BackColor = Color.DimGray;
            button5.FlatAppearance.BorderSize = 0;
            button5.FlatAppearance.MouseOverBackColor = Color.FromArgb(55, 55, 55);
            button5.FlatStyle = FlatStyle.Flat;
            button5.Font = new Font("Yu Gothic UI", 12F);
            button5.ForeColor = Color.White;
            button5.Location = new Point(201, 92);
            button5.Name = "button5";
            button5.Size = new Size(182, 29);
            button5.TabIndex = 12;
            button5.Text = "最新バージョンを指定する";
            button5.UseVisualStyleBackColor = false;
            button5.Click += button5_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Yu Gothic UI", 20F);
            label5.Location = new Point(15, 157);
            label5.Name = "label5";
            label5.Size = new Size(241, 37);
            label5.TabIndex = 11;
            label5.Text = "ダウンロード進捗状況";
            // 
            // progressBar1
            // 
            progressBar1.BackColor = Color.LightGray;
            progressBar1.ForeColor = Color.Blue;
            progressBar1.Location = new Point(15, 209);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(368, 23);
            progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.TabIndex = 10;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("メイリオ", 20F);
            label2.Location = new Point(15, 19);
            label2.Name = "label2";
            label2.Size = new Size(207, 41);
            label2.TabIndex = 9;
            label2.Text = "バージョン一覧";
            // 
            // button3
            // 
            button3.BackColor = Color.DimGray;
            button3.FlatAppearance.BorderSize = 0;
            button3.FlatAppearance.MouseOverBackColor = Color.FromArgb(55, 55, 55);
            button3.FlatStyle = FlatStyle.Flat;
            button3.Font = new Font("Yu Gothic UI", 12F);
            button3.ForeColor = Color.White;
            button3.Location = new Point(12, 485);
            button3.Name = "button3";
            button3.Size = new Size(70, 50);
            button3.TabIndex = 18;
            button3.Text = "リセット";
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // ファイルToolStripMenuItem
            // 
            ファイルToolStripMenuItem.Checked = true;
            ファイルToolStripMenuItem.CheckState = CheckState.Indeterminate;
            ファイルToolStripMenuItem.Name = "ファイルToolStripMenuItem";
            ファイルToolStripMenuItem.Size = new Size(60, 23);
            ファイルToolStripMenuItem.Text = "使い方";
            ファイルToolStripMenuItem.Click += ファイルToolStripMenuItem_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = Color.Gray;
            menuStrip1.Font = new Font("Yu Gothic UI", 10F);
            menuStrip1.Items.AddRange(new ToolStripItem[] { ファイルToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(884, 27);
            menuStrip1.TabIndex = 19;
            menuStrip1.Text = "menuStrip1";
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImage = WinFetch.Properties.Resources.a_Photoroom2;
            pictureBox1.Location = new Point(540, 358);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(435, 219);
            pictureBox1.TabIndex = 20;
            pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Gray;
            ClientSize = new Size(884, 561);
            Controls.Add(button3);
            Controls.Add(button4);
            Controls.Add(groupBox2);
            Controls.Add(button2);
            Controls.Add(richTextBox1);
            Controls.Add(groupBox1);
            Controls.Add(menuStrip1);
            Controls.Add(pictureBox1);
            MainMenuStrip = menuStrip1;
            MaximumSize = new Size(900, 600);
            MinimumSize = new Size(900, 600);
            Name = "Form1";
            Text = "Form1";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RichTextBox richTextBox1;
        private ComboBox comboBox1;
        private Label label1;
        private Button button1;
        private ComboBox comboBox3;
        private Label label3;
        private Label label4;
        private ComboBox comboBox4;
        private GroupBox groupBox1;
        private Button button2;
        private Button button4;
        private GroupBox groupBox2;
        private Button button3;
        private Label label2;
        private Label label5;
        private ProgressBar progressBar1;
        private ToolStripMenuItem ファイルToolStripMenuItem;
        private MenuStrip menuStrip1;
        private Button button5;
        private PictureBox pictureBox1;
    }
}
