namespace PD2Trimmer
{
    partial class FormMain
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
            this._btnUntrimmed = new System.Windows.Forms.Button();
            this._rtbLog = new System.Windows.Forms.RichTextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this._btnEnum = new System.Windows.Forms.Button();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _btnUntrimmed
            // 
            this._btnUntrimmed.Location = new System.Drawing.Point(12, 12);
            this._btnUntrimmed.Name = "_btnUntrimmed";
            this._btnUntrimmed.Size = new System.Drawing.Size(104, 23);
            this._btnUntrimmed.TabIndex = 0;
            this._btnUntrimmed.Text = "Clean me up !";
            this._btnUntrimmed.UseVisualStyleBackColor = true;
            this._btnUntrimmed.Click += new System.EventHandler(this._btnUntrimmed_Click);
            // 
            // _rtbLog
            // 
            this._rtbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this._rtbLog.Location = new System.Drawing.Point(0, 0);
            this._rtbLog.Name = "_rtbLog";
            this._rtbLog.Size = new System.Drawing.Size(938, 287);
            this._rtbLog.TabIndex = 1;
            this._rtbLog.Text = "";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this._btnEnum);
            this.splitContainer1.Panel1.Controls.Add(this._btnUntrimmed);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this._rtbLog);
            this.splitContainer1.Size = new System.Drawing.Size(938, 362);
            this.splitContainer1.SplitterDistance = 71;
            this.splitContainer1.TabIndex = 2;
            // 
            // _btnEnum
            // 
            this._btnEnum.Location = new System.Drawing.Point(12, 41);
            this._btnEnum.Name = "_btnEnum";
            this._btnEnum.Size = new System.Drawing.Size(104, 23);
            this._btnEnum.TabIndex = 2;
            this._btnEnum.Text = "Make me a enum!";
            this._btnEnum.UseVisualStyleBackColor = true;
            this._btnEnum.Click += new System.EventHandler(this._btnEnum_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(938, 362);
            this.Controls.Add(this.splitContainer1);
            this.Name = "FormMain";
            this.Text = "PD2 Trimmer";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _btnUntrimmed;
        private System.Windows.Forms.RichTextBox _rtbLog;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button _btnEnum;
    }
}

