namespace GkHebKeyboard
{
    partial class frmPreferences
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
            this.cbFontSize = new System.Windows.Forms.ComboBox();
            this.labFontSizeLbl = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOkay = new System.Windows.Forms.Button();
            this.txtDefaultSaveLocation = new System.Windows.Forms.TextBox();
            this.btnSaveLocation = new System.Windows.Forms.Button();
            this.lblDefSaveLocation = new System.Windows.Forms.Label();
            this.grpBoxLanguage = new System.Windows.Forms.GroupBox();
            this.rbtnHebrew = new System.Windows.Forms.RadioButton();
            this.rbtnGreek = new System.Windows.Forms.RadioButton();
            this.lblPrefExplanation2 = new System.Windows.Forms.Label();
            this.lblPrefExplanation1 = new System.Windows.Forms.Label();
            this.dlgFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.grpBoxLanguage.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbFontSize
            // 
            this.cbFontSize.FormattingEnabled = true;
            this.cbFontSize.Items.AddRange(new object[] {
            "6",
            "8",
            "10",
            "11",
            "12",
            "14",
            "16",
            "18",
            "20",
            "24",
            "28",
            "30",
            "32",
            "36",
            "40",
            "48",
            "54",
            "60",
            "66",
            "72"});
            this.cbFontSize.Location = new System.Drawing.Point(364, 44);
            this.cbFontSize.Name = "cbFontSize";
            this.cbFontSize.Size = new System.Drawing.Size(62, 21);
            this.cbFontSize.TabIndex = 29;
            // 
            // labFontSizeLbl
            // 
            this.labFontSizeLbl.AutoSize = true;
            this.labFontSizeLbl.Location = new System.Drawing.Point(344, 20);
            this.labFontSizeLbl.Name = "labFontSizeLbl";
            this.labFontSizeLbl.Size = new System.Drawing.Size(145, 13);
            this.labFontSizeLbl.TabIndex = 28;
            this.labFontSizeLbl.Text = "Set the size of font at startup:";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(12, 232);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 27;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOkay
            // 
            this.btnOkay.Location = new System.Drawing.Point(419, 232);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(75, 23);
            this.btnOkay.TabIndex = 26;
            this.btnOkay.Text = "Okay";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // txtDefaultSaveLocation
            // 
            this.txtDefaultSaveLocation.BackColor = System.Drawing.SystemColors.Control;
            this.txtDefaultSaveLocation.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDefaultSaveLocation.Location = new System.Drawing.Point(59, 197);
            this.txtDefaultSaveLocation.Name = "txtDefaultSaveLocation";
            this.txtDefaultSaveLocation.ReadOnly = true;
            this.txtDefaultSaveLocation.Size = new System.Drawing.Size(377, 13);
            this.txtDefaultSaveLocation.TabIndex = 25;
            this.txtDefaultSaveLocation.Text = "None yet specified";
            // 
            // btnSaveLocation
            // 
            this.btnSaveLocation.Location = new System.Drawing.Point(320, 162);
            this.btnSaveLocation.Name = "btnSaveLocation";
            this.btnSaveLocation.Size = new System.Drawing.Size(75, 23);
            this.btnSaveLocation.TabIndex = 24;
            this.btnSaveLocation.Text = "Folder ...";
            this.btnSaveLocation.UseVisualStyleBackColor = true;
            this.btnSaveLocation.Click += new System.EventHandler(this.btnSaveLocation_Click);
            // 
            // lblDefSaveLocation
            // 
            this.lblDefSaveLocation.AutoSize = true;
            this.lblDefSaveLocation.Location = new System.Drawing.Point(39, 167);
            this.lblDefSaveLocation.Name = "lblDefSaveLocation";
            this.lblDefSaveLocation.Size = new System.Drawing.Size(275, 13);
            this.lblDefSaveLocation.TabIndex = 23;
            this.lblDefSaveLocation.Text = "Provide the default location for saving and retrieving text:";
            // 
            // grpBoxLanguage
            // 
            this.grpBoxLanguage.Controls.Add(this.rbtnHebrew);
            this.grpBoxLanguage.Controls.Add(this.rbtnGreek);
            this.grpBoxLanguage.Controls.Add(this.lblPrefExplanation2);
            this.grpBoxLanguage.Controls.Add(this.lblPrefExplanation1);
            this.grpBoxLanguage.Location = new System.Drawing.Point(42, 20);
            this.grpBoxLanguage.Name = "grpBoxLanguage";
            this.grpBoxLanguage.Size = new System.Drawing.Size(285, 114);
            this.grpBoxLanguage.TabIndex = 22;
            this.grpBoxLanguage.TabStop = false;
            this.grpBoxLanguage.Text = "Select Initial Language: ";
            // 
            // rbtnHebrew
            // 
            this.rbtnHebrew.AutoSize = true;
            this.rbtnHebrew.Location = new System.Drawing.Point(32, 86);
            this.rbtnHebrew.Name = "rbtnHebrew";
            this.rbtnHebrew.Size = new System.Drawing.Size(62, 17);
            this.rbtnHebrew.TabIndex = 3;
            this.rbtnHebrew.Text = "Hebrew";
            this.rbtnHebrew.UseVisualStyleBackColor = true;
            // 
            // rbtnGreek
            // 
            this.rbtnGreek.AutoSize = true;
            this.rbtnGreek.Checked = true;
            this.rbtnGreek.Location = new System.Drawing.Point(32, 60);
            this.rbtnGreek.Name = "rbtnGreek";
            this.rbtnGreek.Size = new System.Drawing.Size(54, 17);
            this.rbtnGreek.TabIndex = 2;
            this.rbtnGreek.TabStop = true;
            this.rbtnGreek.Text = "Greek";
            this.rbtnGreek.UseVisualStyleBackColor = true;
            // 
            // lblPrefExplanation2
            // 
            this.lblPrefExplanation2.AutoSize = true;
            this.lblPrefExplanation2.Location = new System.Drawing.Point(45, 39);
            this.lblPrefExplanation2.Name = "lblPrefExplanation2";
            this.lblPrefExplanation2.Size = new System.Drawing.Size(200, 13);
            this.lblPrefExplanation2.TabIndex = 1;
            this.lblPrefExplanation2.Text = "or Hebrew when you start the application";
            // 
            // lblPrefExplanation1
            // 
            this.lblPrefExplanation1.AutoSize = true;
            this.lblPrefExplanation1.Location = new System.Drawing.Point(25, 22);
            this.lblPrefExplanation1.Name = "lblPrefExplanation1";
            this.lblPrefExplanation1.Size = new System.Drawing.Size(253, 13);
            this.lblPrefExplanation1.TabIndex = 0;
            this.lblPrefExplanation1.Text = "Select whether you want the keyboard to use Greek";
            // 
            // dlgFolder
            // 
            this.dlgFolder.Description = "Specify the folder to which you will save text";
            // 
            // frmPreferences
            // 
            this.AcceptButton = this.btnOkay;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(506, 275);
            this.ControlBox = false;
            this.Controls.Add(this.cbFontSize);
            this.Controls.Add(this.labFontSizeLbl);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.txtDefaultSaveLocation);
            this.Controls.Add(this.btnSaveLocation);
            this.Controls.Add(this.lblDefSaveLocation);
            this.Controls.Add(this.grpBoxLanguage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmPreferences";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Preferences";
            this.grpBoxLanguage.ResumeLayout(false);
            this.grpBoxLanguage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbFontSize;
        private System.Windows.Forms.Label labFontSizeLbl;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOkay;
        private System.Windows.Forms.TextBox txtDefaultSaveLocation;
        private System.Windows.Forms.Button btnSaveLocation;
        private System.Windows.Forms.Label lblDefSaveLocation;
        private System.Windows.Forms.GroupBox grpBoxLanguage;
        private System.Windows.Forms.RadioButton rbtnHebrew;
        private System.Windows.Forms.RadioButton rbtnGreek;
        private System.Windows.Forms.Label lblPrefExplanation2;
        private System.Windows.Forms.Label lblPrefExplanation1;
        private System.Windows.Forms.FolderBrowserDialog dlgFolder;
    }
}