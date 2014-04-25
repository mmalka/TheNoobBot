using System.Windows.Forms;
using nManager.Helpful.Forms.UserControls;

namespace Flying_To_Ground_Profiles_Converter
{
    partial class WelcomeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WelcomeForm));
            this.FtGConverterLine1 = new System.Windows.Forms.Label();
            this.FtGConverterLine2 = new System.Windows.Forms.Label();
            this.FtGConverterLine3 = new System.Windows.Forms.Label();
            this.FtGConverterButton = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.MainHeader = new nManager.Helpful.Forms.UserControls.TnbControlMenu();
            this.SuspendLayout();
            // 
            // FtGConverterLine1
            // 
            this.FtGConverterLine1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.FtGConverterLine1.Location = new System.Drawing.Point(12, 52);
            this.FtGConverterLine1.Name = "FtGConverterLine1";
            this.FtGConverterLine1.Size = new System.Drawing.Size(560, 17);
            this.FtGConverterLine1.TabIndex = 1;
            this.FtGConverterLine1.Text = "The Flying To Ground Profiles Converter works with Gatherer, Grinder and Fisherbo" +
                "t profiles.";
            // 
            // FtGConverterLine2
            // 
            this.FtGConverterLine2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.FtGConverterLine2.Location = new System.Drawing.Point(12, 72);
            this.FtGConverterLine2.Name = "FtGConverterLine2";
            this.FtGConverterLine2.Size = new System.Drawing.Size(560, 17);
            this.FtGConverterLine2.TabIndex = 2;
            this.FtGConverterLine2.Text = "The conversion process can take minutes. The generated profiles might not be good" +
                " Grounds profiles, ";
            // 
            // FtGConverterLine3
            // 
            this.FtGConverterLine3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.FtGConverterLine3.Location = new System.Drawing.Point(12, 92);
            this.FtGConverterLine3.Name = "FtGConverterLine3";
            this.FtGConverterLine3.Size = new System.Drawing.Size(560, 17);
            this.FtGConverterLine3.TabIndex = 3;
            this.FtGConverterLine3.Text = "if you have issue with the generated profile, just create one by hand for this zo" +
                "ne.";
            // 
            // FtGConverterButton
            // 
            this.FtGConverterButton.AutoEllipsis = true;
            this.FtGConverterButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.FtGConverterButton.ForeColor = System.Drawing.Color.Snow;
            this.FtGConverterButton.HooverImage = global::Flying_To_Ground_Profiles_Converter.Properties.Resources.greenB_260;
            this.FtGConverterButton.Image = global::Flying_To_Ground_Profiles_Converter.Properties.Resources.blueB_260;
            this.FtGConverterButton.Location = new System.Drawing.Point(157, 114);
            this.FtGConverterButton.Name = "FtGConverterButton";
            this.FtGConverterButton.Size = new System.Drawing.Size(260, 29);
            this.FtGConverterButton.TabIndex = 0;
            this.FtGConverterButton.Text = "Select Flying profiles to convert into Ground profiles";
            this.FtGConverterButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.FtGConverterButton.Click += new System.EventHandler(this.convertB_Click);
            // 
            // MainHeader
            // 
            this.MainHeader.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("MainHeader.BackgroundImage")));
            this.MainHeader.Location = new System.Drawing.Point(0, 0);
            this.MainHeader.LogoImage = ((System.Drawing.Image)(resources.GetObject("MainHeader.LogoImage")));
            this.MainHeader.Name = "MainHeader";
            this.MainHeader.Size = new System.Drawing.Size(575, 43);
            this.MainHeader.TabIndex = 4;
            this.MainHeader.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.MainHeader.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(222)))));
            this.MainHeader.TitleText = "ThePrivateBot";
            // 
            // WelcomeForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(575, 153);
            this.Controls.Add(this.MainHeader);
            this.Controls.Add(this.FtGConverterLine3);
            this.Controls.Add(this.FtGConverterLine2);
            this.Controls.Add(this.FtGConverterLine1);
            this.Controls.Add(this.FtGConverterButton);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "WelcomeForm";
            
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Flying To Ground Profiles Converter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.form_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private TnbButton FtGConverterButton;
        private Label FtGConverterLine1;
        private Label FtGConverterLine2;
        private Label FtGConverterLine3;
        private TnbControlMenu MainHeader;
    }
}