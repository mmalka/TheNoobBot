namespace TheNoobScheduler
{
    partial class AccountEditionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AccountEditionForm));
            this.tnbControlMenu1 = new nManager.Helpful.Forms.UserControls.TnbControlMenu();
            this.SuspendLayout();
            // 
            // tnbControlMenu1
            // 
            this.tnbControlMenu1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tnbControlMenu1.BackgroundImage")));
            this.tnbControlMenu1.Location = new System.Drawing.Point(0, 0);
            this.tnbControlMenu1.LogoImage = ((System.Drawing.Image)(resources.GetObject("tnbControlMenu1.LogoImage")));
            this.tnbControlMenu1.Name = "tnbControlMenu1";
            this.tnbControlMenu1.Size = new System.Drawing.Size(284, 43);
            this.tnbControlMenu1.TabIndex = 0;
            this.tnbControlMenu1.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.tnbControlMenu1.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(222)))));
            this.tnbControlMenu1.TitleText = "TheNoobBot";
            // 
            // AccountEditionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 328);
            this.Controls.Add(this.tnbControlMenu1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AccountEditionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AccountEditionForm";
            this.ResumeLayout(false);

        }

        #endregion

        private nManager.Helpful.Forms.UserControls.TnbControlMenu tnbControlMenu1;
    }
}