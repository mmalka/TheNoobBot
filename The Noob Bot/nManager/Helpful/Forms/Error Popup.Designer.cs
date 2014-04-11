namespace nManager.Helpful.Forms
{
    partial class ErrorPopup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ErrorPopup));
            this.ErrorDescription = new System.Windows.Forms.Label();
            this.Controlbar = new System.Windows.Forms.PictureBox();
            this.Logo = new System.Windows.Forms.PictureBox();
            this.CloseButton = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Controlbar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CloseButton)).BeginInit();
            this.SuspendLayout();
            // 
            // ErrorDescription
            // 
            this.ErrorDescription.BackColor = System.Drawing.Color.White;
            this.ErrorDescription.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrorDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.ErrorDescription.Location = new System.Drawing.Point(7, 28);
            this.ErrorDescription.MaximumSize = new System.Drawing.Size(306, 165);
            this.ErrorDescription.MinimumSize = new System.Drawing.Size(306, 165);
            this.ErrorDescription.Name = "ErrorDescription";
            this.ErrorDescription.Size = new System.Drawing.Size(306, 165);
            this.ErrorDescription.TabIndex = 3;
            this.ErrorDescription.Text = "Error description";
            this.ErrorDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Controlbar
            // 
            this.Controlbar.ErrorImage = null;
            this.Controlbar.Image = ((System.Drawing.Image)(resources.GetObject("Controlbar.Image")));
            this.Controlbar.InitialImage = null;
            this.Controlbar.Location = new System.Drawing.Point(0, 0);
            this.Controlbar.Name = "Controlbar";
            this.Controlbar.Size = new System.Drawing.Size(320, 22);
            this.Controlbar.TabIndex = 4;
            this.Controlbar.TabStop = false;
            this.Controlbar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseDown);
            this.Controlbar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseMove);
            this.Controlbar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseUp);
            // 
            // Logo
            // 
            this.Logo.BackColor = System.Drawing.Color.Transparent;
            this.Logo.Image = ((System.Drawing.Image)(resources.GetObject("Logo.Image")));
            this.Logo.Location = new System.Drawing.Point(3, 3);
            this.Logo.Name = "Logo";
            this.Logo.Size = new System.Drawing.Size(15, 16);
            this.Logo.TabIndex = 5;
            this.Logo.TabStop = false;
            this.Logo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseDown);
            this.Logo.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseMove);
            this.Logo.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseUp);
            // 
            // CloseButton
            // 
            this.CloseButton.ErrorImage = null;
            this.CloseButton.Image = global::nManager.Properties.Resources.close_button;
            this.CloseButton.InitialImage = null;
            this.CloseButton.Location = new System.Drawing.Point(302, 3);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(13, 14);
            this.CloseButton.TabIndex = 6;
            this.CloseButton.TabStop = false;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            this.CloseButton.MouseEnter += new System.EventHandler(this.CloseButton_MouseEnter);
            this.CloseButton.MouseLeave += new System.EventHandler(this.CloseButton_MouseLeave);
            // 
            // ErrorPopup
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(320, 200);
            this.ControlBox = false;
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.Logo);
            this.Controls.Add(this.Controlbar);
            this.Controls.Add(this.ErrorDescription);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ErrorPopup";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Error";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.Controlbar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CloseButton)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label ErrorDescription;
        private System.Windows.Forms.PictureBox Controlbar;
        private System.Windows.Forms.PictureBox Logo;
        private System.Windows.Forms.PictureBox CloseButton;
    }
}