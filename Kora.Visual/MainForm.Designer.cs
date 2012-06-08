namespace Kora.Visual
{
    partial class MainForm
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
            this.AddCheckButton = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.DeleteCheckButton = new System.Windows.Forms.RadioButton();
            this.SearchCheckButton = new System.Windows.Forms.RadioButton();
            this.SuccessorCheckButton = new System.Windows.Forms.RadioButton();
            this.MemoryCheckButton = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ControlCountLabel = new System.Windows.Forms.Label();
            this.ControlCountControl = new System.Windows.Forms.NumericUpDown();
            this.CountControl = new System.Windows.Forms.NumericUpDown();
            this.StartControl = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.StepControl = new System.Windows.Forms.NumericUpDown();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.RBTreeCheck = new System.Windows.Forms.CheckBox();
            this.VEBCheck = new System.Windows.Forms.CheckBox();
            this.XTrieDPHCheck = new System.Windows.Forms.CheckBox();
            this.YTrieDPHCheck = new System.Windows.Forms.CheckBox();
            this.XTrieStandardCheck = new System.Windows.Forms.CheckBox();
            this.YTrieStandardCheck = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ControlCountControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CountControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StepControl)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // AddCheckButton
            // 
            this.AddCheckButton.AutoSize = true;
            this.AddCheckButton.Checked = true;
            this.AddCheckButton.Location = new System.Drawing.Point(3, 3);
            this.AddCheckButton.Name = "AddCheckButton";
            this.AddCheckButton.Size = new System.Drawing.Size(79, 17);
            this.AddCheckButton.TabIndex = 0;
            this.AddCheckButton.TabStop = true;
            this.AddCheckButton.Text = "Dodawanie";
            this.AddCheckButton.UseVisualStyleBackColor = true;
            this.AddCheckButton.CheckedChanged += new System.EventHandler(this.RadioButtonCheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.flowLayoutPanel2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(105, 134);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Metoda";
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel2.Controls.Add(this.AddCheckButton);
            this.flowLayoutPanel2.Controls.Add(this.DeleteCheckButton);
            this.flowLayoutPanel2.Controls.Add(this.SearchCheckButton);
            this.flowLayoutPanel2.Controls.Add(this.SuccessorCheckButton);
            this.flowLayoutPanel2.Controls.Add(this.MemoryCheckButton);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(99, 115);
            this.flowLayoutPanel2.TabIndex = 5;
            // 
            // DeleteCheckButton
            // 
            this.DeleteCheckButton.AutoSize = true;
            this.DeleteCheckButton.Location = new System.Drawing.Point(3, 26);
            this.DeleteCheckButton.Name = "DeleteCheckButton";
            this.DeleteCheckButton.Size = new System.Drawing.Size(72, 17);
            this.DeleteCheckButton.TabIndex = 1;
            this.DeleteCheckButton.TabStop = true;
            this.DeleteCheckButton.Text = "Usuwanie";
            this.DeleteCheckButton.UseVisualStyleBackColor = true;
            this.DeleteCheckButton.CheckedChanged += new System.EventHandler(this.RadioButtonCheckedChanged);
            // 
            // SearchCheckButton
            // 
            this.SearchCheckButton.AutoSize = true;
            this.SearchCheckButton.Location = new System.Drawing.Point(3, 49);
            this.SearchCheckButton.Name = "SearchCheckButton";
            this.SearchCheckButton.Size = new System.Drawing.Size(93, 17);
            this.SearchCheckButton.TabIndex = 2;
            this.SearchCheckButton.TabStop = true;
            this.SearchCheckButton.Text = "Wyszukiwanie";
            this.SearchCheckButton.UseVisualStyleBackColor = true;
            this.SearchCheckButton.CheckedChanged += new System.EventHandler(this.RadioButtonCheckedChanged);
            // 
            // SuccessorCheckButton
            // 
            this.SuccessorCheckButton.AutoSize = true;
            this.SuccessorCheckButton.Location = new System.Drawing.Point(3, 72);
            this.SuccessorCheckButton.Name = "SuccessorCheckButton";
            this.SuccessorCheckButton.Size = new System.Drawing.Size(73, 17);
            this.SuccessorCheckButton.TabIndex = 3;
            this.SuccessorCheckButton.TabStop = true;
            this.SuccessorCheckButton.Text = "Następnik";
            this.SuccessorCheckButton.UseVisualStyleBackColor = true;
            this.SuccessorCheckButton.CheckedChanged += new System.EventHandler(this.RadioButtonCheckedChanged);
            // 
            // MemoryCheckButton
            // 
            this.MemoryCheckButton.AutoSize = true;
            this.MemoryCheckButton.Location = new System.Drawing.Point(3, 95);
            this.MemoryCheckButton.Name = "MemoryCheckButton";
            this.MemoryCheckButton.Size = new System.Drawing.Size(60, 17);
            this.MemoryCheckButton.TabIndex = 4;
            this.MemoryCheckButton.TabStop = true;
            this.MemoryCheckButton.Text = "Pamięć";
            this.MemoryCheckButton.UseVisualStyleBackColor = true;
            this.MemoryCheckButton.CheckedChanged += new System.EventHandler(this.RadioButtonCheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.AutoSize = true;
            this.groupBox2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox2.Controls.Add(this.tableLayoutPanel1);
            this.flowLayoutPanel4.SetFlowBreak(this.groupBox2, true);
            this.groupBox2.Location = new System.Drawing.Point(172, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(290, 123);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Parametry";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.ControlCountLabel, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.ControlCountControl, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.CountControl, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.StartControl, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.StepControl, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(284, 104);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // ControlCountLabel
            // 
            this.ControlCountLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ControlCountLabel.AutoSize = true;
            this.ControlCountLabel.Location = new System.Drawing.Point(3, 84);
            this.ControlCountLabel.Name = "ControlCountLabel";
            this.ControlCountLabel.Size = new System.Drawing.Size(149, 13);
            this.ControlCountLabel.TabIndex = 7;
            this.ControlCountLabel.Text = "Rozmiar zestawu kontrolnego:";
            this.ControlCountLabel.Visible = false;
            // 
            // ControlCountControl
            // 
            this.ControlCountControl.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ControlCountControl.Location = new System.Drawing.Point(161, 81);
            this.ControlCountControl.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.ControlCountControl.Name = "ControlCountControl";
            this.ControlCountControl.Size = new System.Drawing.Size(120, 20);
            this.ControlCountControl.TabIndex = 11;
            this.ControlCountControl.UseWaitCursor = true;
            this.ControlCountControl.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.ControlCountControl.Visible = false;
            // 
            // CountControl
            // 
            this.CountControl.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.CountControl.Location = new System.Drawing.Point(161, 55);
            this.CountControl.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.CountControl.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.CountControl.Name = "CountControl";
            this.CountControl.Size = new System.Drawing.Size(120, 20);
            this.CountControl.TabIndex = 10;
            this.CountControl.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // StartControl
            // 
            this.StartControl.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.StartControl.Location = new System.Drawing.Point(161, 3);
            this.StartControl.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.StartControl.Name = "StartControl";
            this.StartControl.Size = new System.Drawing.Size(120, 20);
            this.StartControl.TabIndex = 0;
            this.StartControl.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(152, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Rozmiar zbioru początkowego:";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Ilość kroków:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Rozmiar kroku:";
            // 
            // StepControl
            // 
            this.StepControl.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.StepControl.Location = new System.Drawing.Point(161, 29);
            this.StepControl.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.StepControl.Name = "StepControl";
            this.StepControl.Size = new System.Drawing.Size(120, 20);
            this.StepControl.TabIndex = 9;
            this.StepControl.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // groupBox3
            // 
            this.groupBox3.AutoSize = true;
            this.groupBox3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox3.Controls.Add(this.flowLayoutPanel3);
            this.groupBox3.Location = new System.Drawing.Point(108, 3);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(52, 157);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Struktury danych";
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.AutoSize = true;
            this.flowLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel3.Controls.Add(this.RBTreeCheck);
            this.flowLayoutPanel3.Controls.Add(this.VEBCheck);
            this.flowLayoutPanel3.Controls.Add(this.XTrieDPHCheck);
            this.flowLayoutPanel3.Controls.Add(this.YTrieDPHCheck);
            this.flowLayoutPanel3.Controls.Add(this.XTrieStandardCheck);
            this.flowLayoutPanel3.Controls.Add(this.YTrieStandardCheck);
            this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel3.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(3, 16);
            this.flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(46, 138);
            this.flowLayoutPanel3.TabIndex = 6;
            // 
            // RBTreeCheck
            // 
            this.RBTreeCheck.AutoSize = true;
            this.RBTreeCheck.Checked = true;
            this.RBTreeCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.RBTreeCheck.Location = new System.Drawing.Point(3, 3);
            this.RBTreeCheck.Name = "RBTreeCheck";
            this.RBTreeCheck.Size = new System.Drawing.Size(40, 17);
            this.RBTreeCheck.TabIndex = 7;
            this.RBTreeCheck.Text = "##";
            this.RBTreeCheck.UseVisualStyleBackColor = true;
            // 
            // VEBCheck
            // 
            this.VEBCheck.AutoSize = true;
            this.VEBCheck.Location = new System.Drawing.Point(3, 26);
            this.VEBCheck.Name = "VEBCheck";
            this.VEBCheck.Size = new System.Drawing.Size(40, 17);
            this.VEBCheck.TabIndex = 12;
            this.VEBCheck.Text = "##";
            this.VEBCheck.UseVisualStyleBackColor = true;
            // 
            // XTrieDPHCheck
            // 
            this.XTrieDPHCheck.AutoSize = true;
            this.XTrieDPHCheck.Location = new System.Drawing.Point(3, 49);
            this.XTrieDPHCheck.Name = "XTrieDPHCheck";
            this.XTrieDPHCheck.Size = new System.Drawing.Size(40, 17);
            this.XTrieDPHCheck.TabIndex = 8;
            this.XTrieDPHCheck.Text = "##";
            this.XTrieDPHCheck.UseVisualStyleBackColor = true;
            // 
            // YTrieDPHCheck
            // 
            this.YTrieDPHCheck.AutoSize = true;
            this.YTrieDPHCheck.Location = new System.Drawing.Point(3, 72);
            this.YTrieDPHCheck.Name = "YTrieDPHCheck";
            this.YTrieDPHCheck.Size = new System.Drawing.Size(40, 17);
            this.YTrieDPHCheck.TabIndex = 9;
            this.YTrieDPHCheck.Text = "##";
            this.YTrieDPHCheck.UseVisualStyleBackColor = true;
            // 
            // XTrieStandardCheck
            // 
            this.XTrieStandardCheck.AutoSize = true;
            this.XTrieStandardCheck.Checked = true;
            this.XTrieStandardCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.XTrieStandardCheck.Location = new System.Drawing.Point(3, 95);
            this.XTrieStandardCheck.Name = "XTrieStandardCheck";
            this.XTrieStandardCheck.Size = new System.Drawing.Size(40, 17);
            this.XTrieStandardCheck.TabIndex = 11;
            this.XTrieStandardCheck.Text = "##";
            this.XTrieStandardCheck.UseVisualStyleBackColor = true;
            // 
            // YTrieStandardCheck
            // 
            this.YTrieStandardCheck.AutoSize = true;
            this.YTrieStandardCheck.Checked = true;
            this.YTrieStandardCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.YTrieStandardCheck.Location = new System.Drawing.Point(3, 118);
            this.YTrieStandardCheck.Name = "YTrieStandardCheck";
            this.YTrieStandardCheck.Size = new System.Drawing.Size(40, 17);
            this.YTrieStandardCheck.TabIndex = 10;
            this.YTrieStandardCheck.Text = "##";
            this.YTrieStandardCheck.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.groupBox1);
            this.flowLayoutPanel1.Controls.Add(this.groupBox3);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(163, 160);
            this.flowLayoutPanel1.TabIndex = 5;
            // 
            // flowLayoutPanel4
            // 
            this.flowLayoutPanel4.AutoSize = true;
            this.flowLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel4.Controls.Add(this.flowLayoutPanel1);
            this.flowLayoutPanel4.Controls.Add(this.groupBox2);
            this.flowLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel4.Name = "flowLayoutPanel4";
            this.flowLayoutPanel4.Size = new System.Drawing.Size(655, 276);
            this.flowLayoutPanel4.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.AutoSize = true;
            this.button1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(0, 276);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(655, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Uruchom";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.ForwardClicked);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(655, 299);
            this.Controls.Add(this.flowLayoutPanel4);
            this.Controls.Add(this.button1);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ControlCountControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CountControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StepControl)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel4.ResumeLayout(false);
            this.flowLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton AddCheckButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton SearchCheckButton;
        private System.Windows.Forms.RadioButton DeleteCheckButton;
        private System.Windows.Forms.RadioButton SuccessorCheckButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox VEBCheck;
        private System.Windows.Forms.CheckBox XTrieStandardCheck;
        private System.Windows.Forms.CheckBox YTrieStandardCheck;
        private System.Windows.Forms.CheckBox YTrieDPHCheck;
        private System.Windows.Forms.CheckBox XTrieDPHCheck;
        private System.Windows.Forms.CheckBox RBTreeCheck;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.NumericUpDown StartControl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown ControlCountControl;
        private System.Windows.Forms.NumericUpDown CountControl;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown StepControl;
        private System.Windows.Forms.Label ControlCountLabel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.RadioButton MemoryCheckButton;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
        private System.Windows.Forms.Button button1;
    }
}

