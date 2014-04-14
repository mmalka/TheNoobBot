using System.Drawing;
using System.Windows.Forms;
using nManager.Helpful.Forms.UserControls;

namespace Mimesis
{
    partial class SettingsMimesisForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsMimesisForm));
            this.MimesisMasterAddr = new System.Windows.Forms.TextBox();
            this.MimesisMasterPort = new System.Windows.Forms.NumericUpDown();
            this.MasterBotIPAddressLabel = new System.Windows.Forms.Label();
            this.labelX6 = new System.Windows.Forms.Label();
            this.MasterBotIPPortLabel = new System.Windows.Forms.Label();
            this.ActivatePartyModeLabel = new System.Windows.Forms.Label();
            this.ActivatePartyMode = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.MainHeader = new nManager.Helpful.Forms.UserControls.TnbControlMenu();
            this.SaveAndCloseButton = new nManager.Helpful.Forms.UserControls.TnbButton();
            ((System.ComponentModel.ISupportInitialize)(this.MimesisMasterPort)).BeginInit();
            this.SuspendLayout();
            // 
            // MimesisMasterAddr
            // 
            this.MimesisMasterAddr.ForeColor = System.Drawing.Color.Black;
            this.MimesisMasterAddr.Location = new System.Drawing.Point(170, 51);
            this.MimesisMasterAddr.Name = "MimesisMasterAddr";
            this.MimesisMasterAddr.Size = new System.Drawing.Size(121, 22);
            this.MimesisMasterAddr.TabIndex = 15;
            // 
            // MimesisMasterPort
            // 
            this.MimesisMasterPort.ForeColor = System.Drawing.Color.Black;
            this.MimesisMasterPort.Location = new System.Drawing.Point(169, 79);
            this.MimesisMasterPort.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.MimesisMasterPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.MimesisMasterPort.Name = "MimesisMasterPort";
            this.MimesisMasterPort.Size = new System.Drawing.Size(121, 22);
            this.MimesisMasterPort.TabIndex = 17;
            this.MimesisMasterPort.Value = new decimal(new int[] {
            6543,
            0,
            0,
            0});
            // 
            // MasterBotIPAddressLabel
            // 
            this.MasterBotIPAddressLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MasterBotIPAddressLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.MasterBotIPAddressLabel.Location = new System.Drawing.Point(12, 52);
            this.MasterBotIPAddressLabel.Name = "MasterBotIPAddressLabel";
            this.MasterBotIPAddressLabel.Size = new System.Drawing.Size(140, 22);
            this.MasterBotIPAddressLabel.TabIndex = 16;
            this.MasterBotIPAddressLabel.Text = "IP Address of the master Bot";
            // 
            // labelX6
            // 
            this.labelX6.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.labelX6.Location = new System.Drawing.Point(12, 133);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(277, 24);
            this.labelX6.TabIndex = 19;
            this.labelX6.Text = "Nothing to says yet.";
            this.labelX6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MasterBotIPPortLabel
            // 
            this.MasterBotIPPortLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MasterBotIPPortLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.MasterBotIPPortLabel.Location = new System.Drawing.Point(12, 80);
            this.MasterBotIPPortLabel.Name = "MasterBotIPPortLabel";
            this.MasterBotIPPortLabel.Size = new System.Drawing.Size(140, 22);
            this.MasterBotIPPortLabel.TabIndex = 18;
            this.MasterBotIPPortLabel.Text = "Port used by the master bot";
            this.MasterBotIPPortLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ActivatePartyModeLabel
            // 
            this.ActivatePartyModeLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ActivatePartyModeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.ActivatePartyModeLabel.Location = new System.Drawing.Point(12, 110);
            this.ActivatePartyModeLabel.Name = "ActivatePartyModeLabel";
            this.ActivatePartyModeLabel.Size = new System.Drawing.Size(206, 22);
            this.ActivatePartyModeLabel.TabIndex = 22;
            this.ActivatePartyModeLabel.Text = "Party Mode (Asks master to invite us)";
            // 
            // ActivatePartyMode
            // 
            this.ActivatePartyMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.ActivatePartyMode.Location = new System.Drawing.Point(229, 110);
            this.ActivatePartyMode.MaximumSize = new System.Drawing.Size(60, 20);
            this.ActivatePartyMode.MinimumSize = new System.Drawing.Size(60, 20);
            this.ActivatePartyMode.Name = "ActivatePartyMode";
            this.ActivatePartyMode.OffText = "OFF";
            this.ActivatePartyMode.OnText = "ON";
            this.ActivatePartyMode.Size = new System.Drawing.Size(60, 20);
            this.ActivatePartyMode.TabIndex = 23;
            this.ActivatePartyMode.Value = false;
            // 
            // MainHeader
            // 
            this.MainHeader.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("MainHeader.BackgroundImage")));
            this.MainHeader.Location = new System.Drawing.Point(0, 0);
            this.MainHeader.LogoImage = ((System.Drawing.Image)(resources.GetObject("MainHeader.LogoImage")));
            this.MainHeader.Name = "MainHeader";
            this.MainHeader.Size = new System.Drawing.Size(320, 43);
            this.MainHeader.TabIndex = 24;
            this.MainHeader.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.MainHeader.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(222)))));
            this.MainHeader.TitleText = "Mimesis Settings";
            // 
            // SaveAndCloseButton
            // 
            this.SaveAndCloseButton.AutoEllipsis = true;
            this.SaveAndCloseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.SaveAndCloseButton.ForeColor = System.Drawing.Color.Snow;
            this.SaveAndCloseButton.HooverImage = global::Mimesis.Properties.Resources.greenB_200;
            this.SaveAndCloseButton.Image = global::Mimesis.Properties.Resources.blueB_200;
            this.SaveAndCloseButton.Location = new System.Drawing.Point(60, 168);
            this.SaveAndCloseButton.Name = "SaveAndCloseButton";
            this.SaveAndCloseButton.Size = new System.Drawing.Size(200, 27);
            this.SaveAndCloseButton.TabIndex = 20;
            this.SaveAndCloseButton.Text = "Save and Close";
            this.SaveAndCloseButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.SaveAndCloseButton.Click += new System.EventHandler(this.SaveAndCloseButton_Click);
            // 
            // SettingsMimesisForm
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(320, 208);
            this.Controls.Add(this.MainHeader);
            this.Controls.Add(this.ActivatePartyMode);
            this.Controls.Add(this.ActivatePartyModeLabel);
            this.Controls.Add(this.SaveAndCloseButton);
            this.Controls.Add(this.labelX6);
            this.Controls.Add(this.MasterBotIPPortLabel);
            this.Controls.Add(this.MimesisMasterPort);
            this.Controls.Add(this.MasterBotIPAddressLabel);
            this.Controls.Add(this.MimesisMasterAddr);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SettingsMimesisForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Mimesis Settings";
            ((System.ComponentModel.ISupportInitialize)(this.MimesisMasterPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox MimesisMasterAddr;
        private NumericUpDown MimesisMasterPort;
        private Label MasterBotIPAddressLabel;
        private Label labelX6;
        private Label MasterBotIPPortLabel;
        private TnbButton SaveAndCloseButton;
        private Label ActivatePartyModeLabel;
        private TnbSwitchButton ActivatePartyMode;
        private TnbControlMenu MainHeader;

    }
}