namespace nManager.Helpful.Forms
{
    partial class Error_Popup
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
            this.OkButton = new DevComponents.DotNetBar.ButtonX();
            this.ErrorDescription = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // OkButton
            // 
            this.OkButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.OkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.OkButton.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.OkButton.Location = new System.Drawing.Point(101, 97);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(120, 24);
            this.OkButton.TabIndex = 1;
            this.OkButton.Text = "Ok";
            this.OkButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // ErrorDescription
            // 
            this.ErrorDescription.AutoSize = true;
            this.ErrorDescription.BackColor = System.Drawing.Color.White;
            this.ErrorDescription.ForeColor = System.Drawing.Color.Black;
            this.ErrorDescription.Location = new System.Drawing.Point(12, 9);
            this.ErrorDescription.MaximumSize = new System.Drawing.Size(303, 0);
            this.ErrorDescription.Name = "ErrorDescription";
            this.ErrorDescription.Size = new System.Drawing.Size(93, 13);
            this.ErrorDescription.TabIndex = 3;
            this.ErrorDescription.Text = "Error description";
            // 
            // Error_Popup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(100F, 100F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(319, 133);
            this.ControlBox = false;
            this.Controls.Add(this.ErrorDescription);
            this.Controls.Add(this.OkButton);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Error_Popup";
            this.ShowIcon = false;
            this.ShowInTaskbar = true;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Error";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX OkButton;
        private System.Windows.Forms.Label ErrorDescription;
    }
}