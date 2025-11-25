namespace PDFiller
{
    partial class ProductViewForm
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
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.GroupBox groupBox2;
            System.Windows.Forms.Label label3;
            this.resetBtn = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.saveBtn = new System.Windows.Forms.Button();
            this.loadBtn = new System.Windows.Forms.Button();
            this.productPictureBox = new System.Windows.Forms.PictureBox();
            this.productNameTextBox = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.productPnTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.productTypeComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            groupBox2 = new System.Windows.Forms.GroupBox();
            label3 = new System.Windows.Forms.Label();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.productPictureBox)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label1.Location = new System.Drawing.Point(77, 404);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(37, 19);
            label1.TabIndex = 9;
            label1.Text = "PN: ";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label2.Location = new System.Drawing.Point(77, 440);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(53, 19);
            label2.TabIndex = 10;
            label2.Text = "Name: ";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(this.resetBtn);
            groupBox2.Controls.Add(this.button5);
            groupBox2.Controls.Add(this.saveBtn);
            groupBox2.Controls.Add(this.loadBtn);
            groupBox2.Location = new System.Drawing.Point(532, 12);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(193, 307);
            groupBox2.TabIndex = 12;
            groupBox2.TabStop = false;
            groupBox2.Text = "Options";
            // 
            // resetBtn
            // 
            this.resetBtn.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resetBtn.ForeColor = System.Drawing.SystemColors.ControlText;
            this.resetBtn.Location = new System.Drawing.Point(35, 246);
            this.resetBtn.Name = "resetBtn";
            this.resetBtn.Size = new System.Drawing.Size(130, 45);
            this.resetBtn.TabIndex = 10;
            this.resetBtn.Text = "Undo changes";
            this.resetBtn.UseVisualStyleBackColor = true;
            this.resetBtn.Click += new System.EventHandler(this.resetBtn_Click);
            // 
            // button5
            // 
            this.button5.Enabled = false;
            this.button5.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button5.Location = new System.Drawing.Point(35, 182);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(130, 45);
            this.button5.TabIndex = 9;
            this.button5.Text = "Mark Used";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // saveBtn
            // 
            this.saveBtn.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveBtn.Location = new System.Drawing.Point(35, 47);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(130, 51);
            this.saveBtn.TabIndex = 6;
            this.saveBtn.Text = "Save Changes";
            this.saveBtn.UseVisualStyleBackColor = true;
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // loadBtn
            // 
            this.loadBtn.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadBtn.Location = new System.Drawing.Point(35, 113);
            this.loadBtn.Name = "loadBtn";
            this.loadBtn.Size = new System.Drawing.Size(130, 51);
            this.loadBtn.TabIndex = 5;
            this.loadBtn.Text = "Load Image";
            this.loadBtn.UseVisualStyleBackColor = true;
            this.loadBtn.Click += new System.EventHandler(this.loadBtn_Click);
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Enabled = false;
            label3.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label3.Location = new System.Drawing.Point(77, 476);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(42, 19);
            label3.TabIndex = 15;
            label3.Text = "Type:";
            // 
            // productPictureBox
            // 
            this.productPictureBox.Location = new System.Drawing.Point(38, 19);
            this.productPictureBox.Name = "productPictureBox";
            this.productPictureBox.Size = new System.Drawing.Size(396, 357);
            this.productPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.productPictureBox.TabIndex = 1;
            this.productPictureBox.TabStop = false;
            // 
            // productNameTextBox
            // 
            this.productNameTextBox.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.productNameTextBox.Location = new System.Drawing.Point(134, 436);
            this.productNameTextBox.Name = "productNameTextBox";
            this.productNameTextBox.Size = new System.Drawing.Size(236, 26);
            this.productNameTextBox.TabIndex = 2;
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.ForeColor = System.Drawing.Color.Red;
            this.button4.Location = new System.Drawing.Point(35, 100);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(130, 45);
            this.button4.TabIndex = 7;
            this.button4.Text = "Delete Product";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // productPnTextBox
            // 
            this.productPnTextBox.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.productPnTextBox.Location = new System.Drawing.Point(134, 400);
            this.productPnTextBox.Name = "productPnTextBox";
            this.productPnTextBox.Size = new System.Drawing.Size(236, 26);
            this.productPnTextBox.TabIndex = 8;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.productTypeComboBox);
            this.groupBox1.Controls.Add(label3);
            this.groupBox1.Controls.Add(label2);
            this.groupBox1.Controls.Add(this.productPictureBox);
            this.groupBox1.Controls.Add(label1);
            this.groupBox1.Controls.Add(this.productNameTextBox);
            this.groupBox1.Controls.Add(this.productPnTextBox);
            this.groupBox1.Location = new System.Drawing.Point(33, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(473, 509);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Product";
            // 
            // productTypeComboBox
            // 
            this.productTypeComboBox.Enabled = false;
            this.productTypeComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.productTypeComboBox.FormattingEnabled = true;
            this.productTypeComboBox.Items.AddRange(new object[] {
            "N/A",
            "Topper",
            "Tricou",
            "Other.."});
            this.productTypeComboBox.Location = new System.Drawing.Point(134, 472);
            this.productTypeComboBox.Name = "productTypeComboBox";
            this.productTypeComboBox.Size = new System.Drawing.Size(236, 28);
            this.productTypeComboBox.TabIndex = 16;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Controls.Add(this.button4);
            this.groupBox3.Location = new System.Drawing.Point(532, 337);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 184);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Dangerous";
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.Red;
            this.button1.Location = new System.Drawing.Point(35, 40);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(130, 45);
            this.button1.TabIndex = 8;
            this.button1.Text = "Mark Unused";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // ProductViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(737, 533);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Name = "ProductViewForm";
            this.Text = "ProductViewForm";
            this.Load += new System.EventHandler(this.ProductViewForm_Load);
            groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.productPictureBox)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox productPictureBox;
        private System.Windows.Forms.TextBox productNameTextBox;
        private System.Windows.Forms.Button loadBtn;
        private System.Windows.Forms.Button saveBtn;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox productPnTextBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox productTypeComboBox;
        private System.Windows.Forms.Button resetBtn;
    }
}