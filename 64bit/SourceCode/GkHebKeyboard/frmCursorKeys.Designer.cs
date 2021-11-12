
namespace GkHebKeyboard
{
    partial class frmCursorKeys
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCursorKeys));
            this.pnlBase = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.pboxKeys = new System.Windows.Forms.PictureBox();
            this.pnlBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pboxKeys)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBase
            // 
            this.pnlBase.Controls.Add(this.btnClose);
            this.pnlBase.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBase.Location = new System.Drawing.Point(0, 238);
            this.pnlBase.Name = "pnlBase";
            this.pnlBase.Size = new System.Drawing.Size(366, 30);
            this.pnlBase.TabIndex = 2;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(147, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pboxKeys
            // 
            this.pboxKeys.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pboxKeys.Image = ((System.Drawing.Image)(resources.GetObject("pboxKeys.Image")));
            this.pboxKeys.InitialImage = null;
            this.pboxKeys.Location = new System.Drawing.Point(13, 10);
            this.pboxKeys.Name = "pboxKeys";
            this.pboxKeys.Size = new System.Drawing.Size(341, 222);
            this.pboxKeys.TabIndex = 3;
            this.pboxKeys.TabStop = false;
            // 
            // frmCursorKeys
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(366, 268);
            this.Controls.Add(this.pboxKeys);
            this.Controls.Add(this.pnlBase);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmCursorKeys";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Physical Keys for Cursor movement";
            this.pnlBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pboxKeys)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBase;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.PictureBox pboxKeys;
    }
}