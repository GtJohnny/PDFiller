namespace PDFiller
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
            System.Windows.Forms.GroupBox groupBox3;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.GroupBox groupBox6;
            System.Windows.Forms.Label label6;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.openPdfCheck = new System.Windows.Forms.CheckBox();
            this.PrintCheck = new System.Windows.Forms.CheckBox();
            this.autoFillCheck = new System.Windows.Forms.CheckBox();
            this.autoFillBtn = new System.Windows.Forms.Button();
            this.tabControlMenu = new System.Windows.Forms.TabControl();
            this.AutoFillPage = new System.Windows.Forms.TabPage();
            this.filePage = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.button12 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.mergeFillButton = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.excelButton = new System.Windows.Forms.Button();
            this.unzippedButton = new System.Windows.Forms.Button();
            this.zipButton = new System.Windows.Forms.Button();
            this.ConfigPage = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.emagBtn = new System.Windows.Forms.Button();
            this.SamedayBtn = new System.Windows.Forms.Button();
            this.CelBtn = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.workButton = new System.Windows.Forms.Button();
            this.rootButton = new System.Windows.Forms.Button();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.rootTextBox = new System.Windows.Forms.TextBox();
            this.excelPathBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.zipPathBox = new System.Windows.Forms.TextBox();
            this.zipLabel = new System.Windows.Forms.Label();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.excelTab = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.chromiumWebBrowser1 = new CefSharp.WinForms.ChromiumWebBrowser();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.summaryGridView = new System.Windows.Forms.DataGridView();
            this.topperColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qntColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qntCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.numeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.excelGridView = new System.Windows.Forms.DataGridView();
            groupBox3 = new System.Windows.Forms.GroupBox();
            label3 = new System.Windows.Forms.Label();
            groupBox6 = new System.Windows.Forms.GroupBox();
            label6 = new System.Windows.Forms.Label();
            groupBox3.SuspendLayout();
            groupBox6.SuspendLayout();
            this.tabControlMenu.SuspendLayout();
            this.AutoFillPage.SuspendLayout();
            this.filePage.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.ConfigPage.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.excelTab.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.summaryGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.excelGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(this.openPdfCheck);
            groupBox3.Controls.Add(this.PrintCheck);
            groupBox3.Controls.Add(this.autoFillCheck);
            groupBox3.Dock = System.Windows.Forms.DockStyle.Bottom;
            groupBox3.Location = new System.Drawing.Point(3, 490);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new System.Drawing.Size(206, 180);
            groupBox3.TabIndex = 0;
            groupBox3.TabStop = false;
            groupBox3.Text = "Options";
            // 
            // openPdfCheck
            // 
            this.openPdfCheck.AutoSize = true;
            this.openPdfCheck.Checked = true;
            this.openPdfCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.openPdfCheck.Font = new System.Drawing.Font("Times New Roman", 14.25F);
            this.openPdfCheck.Location = new System.Drawing.Point(11, 121);
            this.openPdfCheck.Name = "openPdfCheck";
            this.openPdfCheck.Size = new System.Drawing.Size(185, 25);
            this.openPdfCheck.TabIndex = 8;
            this.openPdfCheck.Text = "Open pdf in browser";
            this.openPdfCheck.UseVisualStyleBackColor = true;
            // 
            // PrintCheck
            // 
            this.PrintCheck.AutoSize = true;
            this.PrintCheck.Checked = true;
            this.PrintCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.PrintCheck.Font = new System.Drawing.Font("Times New Roman", 14.25F);
            this.PrintCheck.Location = new System.Drawing.Point(11, 78);
            this.PrintCheck.Name = "PrintCheck";
            this.PrintCheck.Size = new System.Drawing.Size(150, 25);
            this.PrintCheck.TabIndex = 7;
            this.PrintCheck.Text = "Also print on fill";
            this.PrintCheck.UseVisualStyleBackColor = true;
            // 
            // autoFillCheck
            // 
            this.autoFillCheck.AutoSize = true;
            this.autoFillCheck.Checked = true;
            this.autoFillCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoFillCheck.Font = new System.Drawing.Font("Times New Roman", 14.25F);
            this.autoFillCheck.Location = new System.Drawing.Point(11, 34);
            this.autoFillCheck.Name = "autoFillCheck";
            this.autoFillCheck.Size = new System.Drawing.Size(161, 25);
            this.autoFillCheck.TabIndex = 6;
            this.autoFillCheck.Text = "Autofill on launch";
            this.autoFillCheck.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = System.Windows.Forms.DockStyle.Top;
            label3.Location = new System.Drawing.Point(100, 100);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(100, 23);
            label3.TabIndex = 10;
            label3.Text = "AAAAAAAAAAAAAAAAAA";
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(this.autoFillBtn);
            groupBox6.Dock = System.Windows.Forms.DockStyle.Top;
            groupBox6.Location = new System.Drawing.Point(3, 3);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new System.Drawing.Size(206, 528);
            groupBox6.TabIndex = 3;
            groupBox6.TabStop = false;
            groupBox6.Text = "Auto";
            // 
            // autoFillBtn
            // 
            this.autoFillBtn.BackColor = System.Drawing.SystemColors.ControlLight;
            this.autoFillBtn.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.Control;
            this.autoFillBtn.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.autoFillBtn.Location = new System.Drawing.Point(28, 245);
            this.autoFillBtn.Name = "autoFillBtn";
            this.autoFillBtn.Size = new System.Drawing.Size(151, 39);
            this.autoFillBtn.TabIndex = 8;
            this.autoFillBtn.Text = "AutoFill";
            this.autoFillBtn.UseVisualStyleBackColor = false;
            this.autoFillBtn.Click += new System.EventHandler(this.autoFillBtn_Click);
            // 
            // label6
            // 
            label6.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label6.Location = new System.Drawing.Point(7, 118);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(97, 23);
            label6.TabIndex = 5;
            label6.Text = "Root path:";
            // 
            // tabControlMenu
            // 
            this.tabControlMenu.Controls.Add(this.AutoFillPage);
            this.tabControlMenu.Controls.Add(this.filePage);
            this.tabControlMenu.Controls.Add(this.ConfigPage);
            this.tabControlMenu.Font = new System.Drawing.Font("Monotype Corsiva", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControlMenu.Location = new System.Drawing.Point(629, 0);
            this.tabControlMenu.Name = "tabControlMenu";
            this.tabControlMenu.SelectedIndex = 0;
            this.tabControlMenu.Size = new System.Drawing.Size(220, 704);
            this.tabControlMenu.TabIndex = 3;
            // 
            // AutoFillPage
            // 
            this.AutoFillPage.Controls.Add(groupBox3);
            this.AutoFillPage.Controls.Add(groupBox6);
            this.AutoFillPage.Location = new System.Drawing.Point(4, 27);
            this.AutoFillPage.Name = "AutoFillPage";
            this.AutoFillPage.Padding = new System.Windows.Forms.Padding(3);
            this.AutoFillPage.Size = new System.Drawing.Size(212, 673);
            this.AutoFillPage.TabIndex = 2;
            this.AutoFillPage.Text = "AutoFill";
            this.AutoFillPage.UseVisualStyleBackColor = true;
            // 
            // filePage
            // 
            this.filePage.Controls.Add(this.groupBox5);
            this.filePage.Controls.Add(this.groupBox4);
            this.filePage.Location = new System.Drawing.Point(4, 27);
            this.filePage.Name = "filePage";
            this.filePage.Padding = new System.Windows.Forms.Padding(3);
            this.filePage.Size = new System.Drawing.Size(212, 673);
            this.filePage.TabIndex = 0;
            this.filePage.Text = "Files";
            this.filePage.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.button12);
            this.groupBox5.Controls.Add(this.button7);
            this.groupBox5.Controls.Add(this.mergeFillButton);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox5.Location = new System.Drawing.Point(3, 391);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(206, 279);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Work";
            // 
            // button12
            // 
            this.button12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button12.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.Control;
            this.button12.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button12.Location = new System.Drawing.Point(35, 186);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(137, 45);
            this.button12.TabIndex = 6;
            this.button12.Text = "Manual Work";
            this.button12.UseVisualStyleBackColor = false;
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button7.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.Control;
            this.button7.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button7.Location = new System.Drawing.Point(35, 116);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(137, 45);
            this.button7.TabIndex = 7;
            this.button7.Text = "Print";
            this.button7.UseVisualStyleBackColor = false;
            // 
            // mergeFillButton
            // 
            this.mergeFillButton.BackColor = System.Drawing.SystemColors.ControlLight;
            this.mergeFillButton.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.Control;
            this.mergeFillButton.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mergeFillButton.Location = new System.Drawing.Point(35, 48);
            this.mergeFillButton.Name = "mergeFillButton";
            this.mergeFillButton.Size = new System.Drawing.Size(137, 45);
            this.mergeFillButton.TabIndex = 6;
            this.mergeFillButton.Text = "Merge&&Fill";
            this.mergeFillButton.UseVisualStyleBackColor = false;
            this.mergeFillButton.Click += new System.EventHandler(this.mergeFillButton_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.excelButton);
            this.groupBox4.Controls.Add(this.unzippedButton);
            this.groupBox4.Controls.Add(this.zipButton);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox4.Location = new System.Drawing.Point(3, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(206, 390);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Files";
            this.groupBox4.Enter += new System.EventHandler(this.groupBox4_Enter);
            // 
            // excelButton
            // 
            this.excelButton.BackColor = System.Drawing.SystemColors.ControlLight;
            this.excelButton.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.Control;
            this.excelButton.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.excelButton.Location = new System.Drawing.Point(25, 84);
            this.excelButton.Name = "excelButton";
            this.excelButton.Size = new System.Drawing.Size(156, 45);
            this.excelButton.TabIndex = 5;
            this.excelButton.Text = "Select Excel File";
            this.excelButton.UseVisualStyleBackColor = false;
            this.excelButton.Click += new System.EventHandler(this.excelButton_Click);
            // 
            // unzippedButton
            // 
            this.unzippedButton.BackColor = System.Drawing.SystemColors.ControlLight;
            this.unzippedButton.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.Control;
            this.unzippedButton.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.unzippedButton.Location = new System.Drawing.Point(25, 262);
            this.unzippedButton.Name = "unzippedButton";
            this.unzippedButton.Size = new System.Drawing.Size(156, 45);
            this.unzippedButton.TabIndex = 2;
            this.unzippedButton.Text = "Select Unzipped";
            this.unzippedButton.UseVisualStyleBackColor = false;
            this.unzippedButton.Click += new System.EventHandler(this.unzippedButton_Click);
            // 
            // zipButton
            // 
            this.zipButton.BackColor = System.Drawing.SystemColors.ControlLight;
            this.zipButton.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.Control;
            this.zipButton.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zipButton.Location = new System.Drawing.Point(25, 173);
            this.zipButton.Name = "zipButton";
            this.zipButton.Size = new System.Drawing.Size(156, 45);
            this.zipButton.TabIndex = 4;
            this.zipButton.Text = "Select Zip Archive";
            this.zipButton.UseVisualStyleBackColor = false;
            this.zipButton.Click += new System.EventHandler(this.zipButton_Click);
            // 
            // ConfigPage
            // 
            this.ConfigPage.Controls.Add(this.groupBox1);
            this.ConfigPage.Controls.Add(this.groupBox2);
            this.ConfigPage.Location = new System.Drawing.Point(4, 27);
            this.ConfigPage.Name = "ConfigPage";
            this.ConfigPage.Padding = new System.Windows.Forms.Padding(3);
            this.ConfigPage.Size = new System.Drawing.Size(212, 673);
            this.ConfigPage.TabIndex = 1;
            this.ConfigPage.Text = "Config";
            this.ConfigPage.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.emagBtn);
            this.groupBox1.Controls.Add(this.SamedayBtn);
            this.groupBox1.Controls.Add(this.CelBtn);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(3, 377);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(206, 293);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sites";
            // 
            // emagBtn
            // 
            this.emagBtn.BackColor = System.Drawing.SystemColors.ControlLight;
            this.emagBtn.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.Control;
            this.emagBtn.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.emagBtn.Location = new System.Drawing.Point(36, 46);
            this.emagBtn.Name = "emagBtn";
            this.emagBtn.Size = new System.Drawing.Size(135, 43);
            this.emagBtn.TabIndex = 9;
            this.emagBtn.Text = "Open Emag";
            this.emagBtn.UseVisualStyleBackColor = false;
            this.emagBtn.Click += new System.EventHandler(this.emagBtn_Click);
            // 
            // SamedayBtn
            // 
            this.SamedayBtn.BackColor = System.Drawing.SystemColors.ControlLight;
            this.SamedayBtn.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.Control;
            this.SamedayBtn.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.SamedayBtn.Location = new System.Drawing.Point(36, 203);
            this.SamedayBtn.Name = "SamedayBtn";
            this.SamedayBtn.Size = new System.Drawing.Size(135, 43);
            this.SamedayBtn.TabIndex = 8;
            this.SamedayBtn.Text = "Open Sameday";
            this.SamedayBtn.UseVisualStyleBackColor = false;
            this.SamedayBtn.Click += new System.EventHandler(this.SamedayBtn_Click);
            // 
            // CelBtn
            // 
            this.CelBtn.BackColor = System.Drawing.SystemColors.ControlLight;
            this.CelBtn.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.Control;
            this.CelBtn.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.CelBtn.Location = new System.Drawing.Point(35, 124);
            this.CelBtn.Name = "CelBtn";
            this.CelBtn.Size = new System.Drawing.Size(137, 43);
            this.CelBtn.TabIndex = 7;
            this.CelBtn.Text = "Open Cel.ro";
            this.CelBtn.UseVisualStyleBackColor = false;
            this.CelBtn.Click += new System.EventHandler(this.CelBtn_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.workButton);
            this.groupBox2.Controls.Add(this.rootButton);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(206, 383);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Paths";
            // 
            // workButton
            // 
            this.workButton.BackColor = System.Drawing.SystemColors.ControlLight;
            this.workButton.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.Control;
            this.workButton.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.workButton.Location = new System.Drawing.Point(36, 229);
            this.workButton.Name = "workButton";
            this.workButton.Size = new System.Drawing.Size(135, 52);
            this.workButton.TabIndex = 6;
            this.workButton.Text = "Select Work Directory";
            this.workButton.UseVisualStyleBackColor = false;
            this.workButton.Click += new System.EventHandler(this.workButton_Click);
            // 
            // rootButton
            // 
            this.rootButton.BackColor = System.Drawing.SystemColors.ControlLight;
            this.rootButton.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.Control;
            this.rootButton.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.rootButton.Location = new System.Drawing.Point(36, 101);
            this.rootButton.Name = "rootButton";
            this.rootButton.Size = new System.Drawing.Size(135, 52);
            this.rootButton.TabIndex = 3;
            this.rootButton.Text = "Select Root Directory";
            this.rootButton.UseVisualStyleBackColor = false;
            this.rootButton.Click += new System.EventHandler(this.rootButton_Click);
            // 
            // groupBox8
            // 
            this.groupBox8.BackColor = System.Drawing.SystemColors.Window;
            this.groupBox8.Controls.Add(this.rootTextBox);
            this.groupBox8.Controls.Add(label6);
            this.groupBox8.Controls.Add(this.excelPathBox);
            this.groupBox8.Controls.Add(this.label7);
            this.groupBox8.Controls.Add(this.zipPathBox);
            this.groupBox8.Controls.Add(this.zipLabel);
            this.groupBox8.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox8.Font = new System.Drawing.Font("Monotype Corsiva", 12F, System.Drawing.FontStyle.Italic);
            this.groupBox8.Location = new System.Drawing.Point(0, 706);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(849, 151);
            this.groupBox8.TabIndex = 5;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Paths";
            this.groupBox8.Enter += new System.EventHandler(this.groupBox8_Enter);
            // 
            // rootTextBox
            // 
            this.rootTextBox.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rootTextBox.Location = new System.Drawing.Point(110, 117);
            this.rootTextBox.Name = "rootTextBox";
            this.rootTextBox.ReadOnly = true;
            this.rootTextBox.Size = new System.Drawing.Size(733, 25);
            this.rootTextBox.TabIndex = 6;
            // 
            // excelPathBox
            // 
            this.excelPathBox.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.excelPathBox.Location = new System.Drawing.Point(110, 70);
            this.excelPathBox.Name = "excelPathBox";
            this.excelPathBox.ReadOnly = true;
            this.excelPathBox.Size = new System.Drawing.Size(733, 25);
            this.excelPathBox.TabIndex = 3;
            this.excelPathBox.DoubleClick += new System.EventHandler(this.excelPathBox_DoubleClick);
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(7, 71);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(97, 23);
            this.label7.TabIndex = 2;
            this.label7.Text = "Excel File:";
            // 
            // zipPathBox
            // 
            this.zipPathBox.Font = new System.Drawing.Font("Times New Roman", 11.25F);
            this.zipPathBox.Location = new System.Drawing.Point(110, 21);
            this.zipPathBox.Name = "zipPathBox";
            this.zipPathBox.ReadOnly = true;
            this.zipPathBox.Size = new System.Drawing.Size(733, 25);
            this.zipPathBox.TabIndex = 1;
            this.zipPathBox.DoubleClick += new System.EventHandler(this.zipPathBox_DoubleClick);
            // 
            // zipLabel
            // 
            this.zipLabel.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zipLabel.Location = new System.Drawing.Point(7, 22);
            this.zipLabel.Name = "zipLabel";
            this.zipLabel.Size = new System.Drawing.Size(81, 23);
            this.zipLabel.TabIndex = 0;
            this.zipLabel.Text = "Zip File:";
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.summaryGridView);
            this.tabPage5.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage5.Location = new System.Drawing.Point(4, 26);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(622, 674);
            this.tabPage5.TabIndex = 3;
            this.tabPage5.Text = "Summary";
            // 
            // excelTab
            // 
            this.excelTab.Controls.Add(this.excelGridView);
            this.excelTab.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.excelTab.Location = new System.Drawing.Point(4, 26);
            this.excelTab.Name = "excelTab";
            this.excelTab.Padding = new System.Windows.Forms.Padding(3);
            this.excelTab.Size = new System.Drawing.Size(622, 674);
            this.excelTab.TabIndex = 2;
            this.excelTab.Text = "ExcelPreview";
            this.excelTab.UseVisualStyleBackColor = true;
            this.excelTab.Click += new System.EventHandler(this.excelTab_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.chromiumWebBrowser1);
            this.tabPage3.Location = new System.Drawing.Point(4, 26);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(622, 674);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "MergedPreview";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // chromiumWebBrowser1
            // 
            this.chromiumWebBrowser1.ActivateBrowserOnCreation = false;
            this.chromiumWebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chromiumWebBrowser1.Location = new System.Drawing.Point(3, 3);
            this.chromiumWebBrowser1.Name = "chromiumWebBrowser1";
            this.chromiumWebBrowser1.Size = new System.Drawing.Size(616, 668);
            this.chromiumWebBrowser1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.textBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(622, 674);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "WorkflowStatus";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(3, 3);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(616, 668);
            this.textBox1.TabIndex = 0;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage1);
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.excelTab);
            this.tabControl2.Controls.Add(this.tabPage5);
            this.tabControl2.Font = new System.Drawing.Font("Monotype Corsiva", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(630, 704);
            this.tabControl2.TabIndex = 4;
            this.tabControl2.SelectedIndexChanged += new System.EventHandler(this.tabControl2_SelectedIndexChanged);
            // 
            // summaryGridView
            // 
            this.summaryGridView.AllowUserToAddRows = false;
            this.summaryGridView.AllowUserToDeleteRows = false;
            this.summaryGridView.AllowUserToResizeColumns = false;
            this.summaryGridView.AllowUserToResizeRows = false;
            this.summaryGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.summaryGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.summaryGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.topperColumn,
            this.qntColumn});
            this.summaryGridView.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.summaryGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.summaryGridView.Location = new System.Drawing.Point(0, 0);
            this.summaryGridView.Name = "summaryGridView";
            this.summaryGridView.ReadOnly = true;
            this.summaryGridView.ShowCellErrors = false;
            this.summaryGridView.Size = new System.Drawing.Size(622, 674);
            this.summaryGridView.TabIndex = 9;
            // 
            // topperColumn
            // 
            this.topperColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.topperColumn.HeaderText = "Topper";
            this.topperColumn.Name = "topperColumn";
            this.topperColumn.ReadOnly = true;
            // 
            // qntColumn
            // 
            this.qntColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.qntColumn.HeaderText = "Quantity";
            this.qntColumn.Name = "qntColumn";
            this.qntColumn.ReadOnly = true;
            this.qntColumn.Width = 80;
            // 
            // qntCol
            // 
            this.qntCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.qntCol.HeaderText = "Quantity";
            this.qntCol.MinimumWidth = 20;
            this.qntCol.Name = "qntCol";
            this.qntCol.ReadOnly = true;
            this.qntCol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.qntCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.qntCol.Width = 61;
            // 
            // nameCol
            // 
            this.nameCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.nameCol.HeaderText = "Topper";
            this.nameCol.Name = "nameCol";
            this.nameCol.ReadOnly = true;
            this.nameCol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.nameCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // numeCol
            // 
            this.numeCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.numeCol.HeaderText = "Name";
            this.numeCol.Name = "numeCol";
            this.numeCol.ReadOnly = true;
            this.numeCol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.numeCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.numeCol.Width = 44;
            // 
            // excelGridView
            // 
            this.excelGridView.AllowUserToAddRows = false;
            this.excelGridView.AllowUserToDeleteRows = false;
            this.excelGridView.AllowUserToResizeColumns = false;
            this.excelGridView.AllowUserToResizeRows = false;
            this.excelGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.excelGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.excelGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.numeCol,
            this.nameCol,
            this.qntCol});
            this.excelGridView.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.excelGridView.Location = new System.Drawing.Point(0, 0);
            this.excelGridView.Name = "excelGridView";
            this.excelGridView.ReadOnly = true;
            this.excelGridView.RowTemplate.ReadOnly = true;
            this.excelGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.excelGridView.ShowCellErrors = false;
            this.excelGridView.Size = new System.Drawing.Size(622, 674);
            this.excelGridView.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(849, 857);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.tabControlMenu);
            this.Controls.Add(this.tabControl2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PDFiller";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox6.ResumeLayout(false);
            this.tabControlMenu.ResumeLayout(false);
            this.AutoFillPage.ResumeLayout(false);
            this.filePage.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ConfigPage.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.excelTab.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.summaryGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.excelGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControlMenu;
        private System.Windows.Forms.TabPage filePage;
        private System.Windows.Forms.Button zipButton;
        private System.Windows.Forms.Button unzippedButton;
        private System.Windows.Forms.TabPage ConfigPage;
        private System.Windows.Forms.Button workButton;
        private System.Windows.Forms.Button rootButton;
        private System.Windows.Forms.Button CelBtn;
        private System.Windows.Forms.Button emagBtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button mergeFillButton;
        private System.Windows.Forms.Button excelButton;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.TabPage AutoFillPage;
        private System.Windows.Forms.Button autoFillBtn;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button SamedayBtn;
        internal System.Windows.Forms.CheckBox autoFillCheck;
        internal System.Windows.Forms.CheckBox PrintCheck;
        internal System.Windows.Forms.CheckBox openPdfCheck;
        internal System.Windows.Forms.TextBox zipPathBox;
        internal System.Windows.Forms.Label zipLabel;
        internal System.Windows.Forms.TextBox excelPathBox;
        internal System.Windows.Forms.TextBox rootTextBox;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage excelTab;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabControl tabControl2;
        internal System.Windows.Forms.TextBox textBox1;
        private CefSharp.WinForms.ChromiumWebBrowser chromiumWebBrowser1;
        private System.Windows.Forms.DataGridView summaryGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn topperColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn qntColumn;
        private System.Windows.Forms.DataGridView excelGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn numeCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn qntCol;
    }
}

