using System.Windows.Forms;
using nManager.Helpful.Forms.UserControls;

namespace Profiles_Converters
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.MainContent = new System.Windows.Forms.Label();
            this.MainHeader = new nManager.Helpful.Forms.UserControls.TnbControlMenu();
            this.convertB = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.SuspendLayout();
            // 
            // MainContent
            // 
            this.MainContent.Location = new System.Drawing.Point(8, 52);
            this.MainContent.Name = "MainContent";
            this.MainContent.Size = new System.Drawing.Size(296, 40);
            this.MainContent.TabIndex = 1;
            this.MainContent.Text = "Convert profiles (Pirox Fly gatherer, MMOLazy MyFlyer, Gather Buddy,  WowRobot Ga" +
                "ther Fly, HonorBuddy  grind) to The Noob Bot profiles.";
            // 
            // MainHeader
            // 
            this.MainHeader.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("MainHeader.BackgroundImage")));
            this.MainHeader.Location = new System.Drawing.Point(0, 0);
            this.MainHeader.LogoImage = ((System.Drawing.Image)(resources.GetObject("MainHeader.LogoImage")));
            this.MainHeader.Name = "MainHeader";
            this.MainHeader.Size = new System.Drawing.Size(320, 43);
            this.MainHeader.TabIndex = 2;
            this.MainHeader.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.MainHeader.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(222)))));
            this.MainHeader.TitleText = "Profile Converter";
            // 
            // convertB
            // 
            this.convertB.AutoEllipsis = true;
            this.convertB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.convertB.ForeColor = System.Drawing.Color.Snow;
            this.convertB.HooverImage = global::Profiles_Converters.Properties.Resources.greenB_200;
            this.convertB.Image = global::Profiles_Converters.Properties.Resources.blueB_200;
            this.convertB.Location = new System.Drawing.Point(60, 97);
            this.convertB.Name = "convertB";
            this.convertB.Size = new System.Drawing.Size(200, 36);
            this.convertB.TabIndex = 0;
            this.convertB.Text = "Convert Profiles";
            this.convertB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.convertB.Click += new System.EventHandler(this.convertB_Click);
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(320, 141);
            this.Controls.Add(this.MainHeader);
            this.Controls.Add(this.MainContent);
            this.Controls.Add(this.convertB);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Profiles Converters";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.form_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private TnbButton convertB;
        private Label MainContent;
        private TnbControlMenu MainHeader;
    }
}