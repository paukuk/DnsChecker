namespace DnsChecker
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
            this.UploadButton = new System.Windows.Forms.Button();
            this.MainFormLabel = new System.Windows.Forms.Label();
            this.InfoGroupBox = new System.Windows.Forms.GroupBox();
            this.InfoGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // UploadButton
            // 
            this.UploadButton.Location = new System.Drawing.Point(67, 117);
            this.UploadButton.Name = "UploadButton";
            this.UploadButton.Size = new System.Drawing.Size(102, 35);
            this.UploadButton.TabIndex = 0;
            this.UploadButton.Text = "Select excel file";
            this.UploadButton.UseVisualStyleBackColor = true;
            this.UploadButton.Click += new System.EventHandler(this.UploadButton_Click);
            // 
            // MainFormLabel
            // 
            this.MainFormLabel.Location = new System.Drawing.Point(6, 34);
            this.MainFormLabel.Name = "MainFormLabel";
            this.MainFormLabel.Size = new System.Drawing.Size(236, 80);
            this.MainFormLabel.TabIndex = 1;
            this.MainFormLabel.Text = "This desktop application is used for DNS information query. More information prov" +
    "ided in Readme.txt.";
            // 
            // InfoGroupBox
            // 
            this.InfoGroupBox.Controls.Add(this.MainFormLabel);
            this.InfoGroupBox.Controls.Add(this.UploadButton);
            this.InfoGroupBox.Location = new System.Drawing.Point(227, 71);
            this.InfoGroupBox.Name = "InfoGroupBox";
            this.InfoGroupBox.Size = new System.Drawing.Size(253, 194);
            this.InfoGroupBox.TabIndex = 2;
            this.InfoGroupBox.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.InfoGroupBox);
            this.Name = "Form1";
            this.Text = "DNS Checker";
            this.InfoGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button UploadButton;
        private System.Windows.Forms.Label MainFormLabel;
        private System.Windows.Forms.GroupBox InfoGroupBox;
    }
}

