namespace LogReader
{
    partial class LineParserOptionsDialog
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
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.radioGroupBox1 = new LogReader.RadioGroupBox();
            this.columnCount = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.separatorChar = new System.Windows.Forms.TextBox();
            this.csvParser = new System.Windows.Forms.RadioButton();
            this.singleLineParser = new System.Windows.Forms.RadioButton();
            this.radioGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.columnCount)).BeginInit();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(110, 178);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(15, 178);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // radioGroupBox1
            // 
            this.radioGroupBox1.Controls.Add(this.columnCount);
            this.radioGroupBox1.Controls.Add(this.label2);
            this.radioGroupBox1.Controls.Add(this.label1);
            this.radioGroupBox1.Controls.Add(this.separatorChar);
            this.radioGroupBox1.Controls.Add(this.csvParser);
            this.radioGroupBox1.Controls.Add(this.singleLineParser);
            this.radioGroupBox1.DataBindings.Add(new System.Windows.Forms.Binding("Selected", global::LogReader.Properties.Settings.Default, "LineParser_Type_Name", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.radioGroupBox1.Location = new System.Drawing.Point(12, 12);
            this.radioGroupBox1.Name = "radioGroupBox1";
            this.radioGroupBox1.Selected = global::LogReader.Properties.Settings.Default.LineParser_Type_Name;
            this.radioGroupBox1.Size = new System.Drawing.Size(173, 160);
            this.radioGroupBox1.TabIndex = 8;
            this.radioGroupBox1.TabStop = false;
            this.radioGroupBox1.Text = "Select columns view:";
            // 
            // columnCount
            // 
            this.columnCount.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::LogReader.Properties.Settings.Default, "LineParser_CSV_Columns", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.columnCount.Location = new System.Drawing.Point(107, 110);
            this.columnCount.Name = "columnCount";
            this.columnCount.Size = new System.Drawing.Size(34, 20);
            this.columnCount.TabIndex = 13;
            this.columnCount.Value = global::LogReader.Properties.Settings.Default.LineParser_CSV_Columns;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(45, 115);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Columns:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Separator:";
            // 
            // separatorChar
            // 
            this.separatorChar.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::LogReader.Properties.Settings.Default, "LineParser_CSV_Separator", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.separatorChar.Location = new System.Drawing.Point(107, 84);
            this.separatorChar.MaxLength = 1;
            this.separatorChar.Name = "separatorChar";
            this.separatorChar.Size = new System.Drawing.Size(34, 20);
            this.separatorChar.TabIndex = 10;
            this.separatorChar.Text = global::LogReader.Properties.Settings.Default.LineParser_CSV_Separator;
            // 
            // csvParser
            // 
            this.csvParser.AutoSize = true;
            this.csvParser.Location = new System.Drawing.Point(21, 56);
            this.csvParser.Name = "csvParser";
            this.csvParser.Size = new System.Drawing.Size(70, 17);
            this.csvParser.TabIndex = 9;
            this.csvParser.Tag = "1";
            this.csvParser.Text = "CSV style";
            this.csvParser.UseVisualStyleBackColor = true;
            this.csvParser.CheckedChanged += new System.EventHandler(this.csvParser_CheckedChanged);
            // 
            // singleLineParser
            // 
            this.singleLineParser.AutoSize = true;
            this.singleLineParser.Checked = true;
            this.singleLineParser.Location = new System.Drawing.Point(21, 24);
            this.singleLineParser.Name = "singleLineParser";
            this.singleLineParser.Size = new System.Drawing.Size(73, 17);
            this.singleLineParser.TabIndex = 8;
            this.singleLineParser.TabStop = true;
            this.singleLineParser.Tag = "0";
            this.singleLineParser.Text = "Single line";
            this.singleLineParser.UseVisualStyleBackColor = true;
            this.singleLineParser.CheckedChanged += new System.EventHandler(this.csvParser_CheckedChanged);
            // 
            // LineParserOptionsDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(197, 213);
            this.Controls.Add(this.radioGroupBox1);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LineParserOptionsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Columns";
            this.radioGroupBox1.ResumeLayout(false);
            this.radioGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.columnCount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private RadioGroupBox radioGroupBox1;
        private System.Windows.Forms.NumericUpDown columnCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox separatorChar;
        private System.Windows.Forms.RadioButton csvParser;
        private System.Windows.Forms.RadioButton singleLineParser;
    }
}