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
            this.MimesisMasterAddr = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.MimesisMasterPort = new DevComponents.Editors.IntegerInput();
            this.MasterBotIPAddressLabel = new DevComponents.DotNetBar.LabelX();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.MasterBotIPPortLabel = new DevComponents.DotNetBar.LabelX();
            this.saveB = new DevComponents.DotNetBar.ButtonX();
            this.ActivatePartyModeLabel = new DevComponents.DotNetBar.LabelX();
            this.ActivatePartyMode = new DevComponents.DotNetBar.Controls.SwitchButton();
            ((System.ComponentModel.ISupportInitialize)(this.MimesisMasterPort)).BeginInit();
            this.SuspendLayout();
            // 
            // MimesisMasterAddr
            // 
            this.MimesisMasterAddr.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.MimesisMasterAddr.Border.Class = "TextBoxBorder";
            this.MimesisMasterAddr.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.MimesisMasterAddr.ForeColor = System.Drawing.Color.Black;
            this.MimesisMasterAddr.Location = new System.Drawing.Point(170, 42);
            this.MimesisMasterAddr.Name = "MimesisMasterAddr";
            this.MimesisMasterAddr.Size = new System.Drawing.Size(121, 22);
            this.MimesisMasterAddr.TabIndex = 15;
            // 
            // MimesisMasterPort
            // 
            this.MimesisMasterPort.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.MimesisMasterPort.BackgroundStyle.Class = "DateTimeInputBackground";
            this.MimesisMasterPort.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.MimesisMasterPort.ForeColor = System.Drawing.Color.Black;
            this.MimesisMasterPort.Location = new System.Drawing.Point(169, 70);
            this.MimesisMasterPort.Name = "MimesisMasterPort";
            this.MimesisMasterPort.Size = new System.Drawing.Size(121, 22);
            this.MimesisMasterPort.TabIndex = 17;
            // 
            // MasterBotIPAddressLabel
            // 
            this.MasterBotIPAddressLabel.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.MasterBotIPAddressLabel.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.MasterBotIPAddressLabel.ForeColor = System.Drawing.Color.Black;
            this.MasterBotIPAddressLabel.Location = new System.Drawing.Point(12, 40);
            this.MasterBotIPAddressLabel.Name = "MasterBotIPAddressLabel";
            this.MasterBotIPAddressLabel.Size = new System.Drawing.Size(140, 22);
            this.MasterBotIPAddressLabel.TabIndex = 16;
            this.MasterBotIPAddressLabel.Text = "IP Address of the master Bot";
            // 
            // labelX6
            // 
            this.labelX6.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.labelX6.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX6.ForeColor = System.Drawing.Color.Black;
            this.labelX6.Location = new System.Drawing.Point(12, 121);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(277, 24);
            this.labelX6.TabIndex = 19;
            this.labelX6.Text = "Nothing to says yet.";
            this.labelX6.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // MasterBotIPPortLabel
            // 
            this.MasterBotIPPortLabel.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.MasterBotIPPortLabel.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.MasterBotIPPortLabel.ForeColor = System.Drawing.Color.Black;
            this.MasterBotIPPortLabel.Location = new System.Drawing.Point(12, 68);
            this.MasterBotIPPortLabel.Name = "MasterBotIPPortLabel";
            this.MasterBotIPPortLabel.Size = new System.Drawing.Size(140, 22);
            this.MasterBotIPPortLabel.TabIndex = 18;
            this.MasterBotIPPortLabel.Text = "Port used by the master bot";
            // 
            // saveB
            // 
            this.saveB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.saveB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.saveB.Location = new System.Drawing.Point(199, 2);
            this.saveB.Name = "saveB";
            this.saveB.Size = new System.Drawing.Size(90, 27);
            this.saveB.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.saveB.TabIndex = 20;
            this.saveB.Text = "Save and Close";
            this.saveB.Click += new System.EventHandler(this.saveB_Click);
            // 
            // ActivatePartyModeLabel
            // 
            this.ActivatePartyModeLabel.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.ActivatePartyModeLabel.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ActivatePartyModeLabel.ForeColor = System.Drawing.Color.Black;
            this.ActivatePartyModeLabel.Location = new System.Drawing.Point(12, 98);
            this.ActivatePartyModeLabel.Name = "ActivatePartyModeLabel";
            this.ActivatePartyModeLabel.Size = new System.Drawing.Size(206, 22);
            this.ActivatePartyModeLabel.TabIndex = 22;
            this.ActivatePartyModeLabel.Text = "Party Mode (Asks master to invite us)";
            // 
            // ActivatePartyMode
            // 
            // 
            // 
            // 
            this.ActivatePartyMode.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ActivatePartyMode.Location = new System.Drawing.Point(224, 98);
            this.ActivatePartyMode.Name = "ActivatePartyMode";
            this.ActivatePartyMode.Size = new System.Drawing.Size(66, 22);
            this.ActivatePartyMode.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ActivatePartyMode.TabIndex = 23;
            // 
            // SettingsMimesisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(301, 142);
            this.Controls.Add(this.ActivatePartyMode);
            this.Controls.Add(this.ActivatePartyModeLabel);
            this.Controls.Add(this.saveB);
            this.Controls.Add(this.labelX6);
            this.Controls.Add(this.MasterBotIPPortLabel);
            this.Controls.Add(this.MimesisMasterPort);
            this.Controls.Add(this.MasterBotIPAddressLabel);
            this.Controls.Add(this.MimesisMasterAddr);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(317, 180);
            this.MinimumSize = new System.Drawing.Size(317, 180);
            this.Name = "SettingsMimesisForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Mimesis Settings";
            ((System.ComponentModel.ISupportInitialize)(this.MimesisMasterPort)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.TextBoxX MimesisMasterAddr;
        private DevComponents.Editors.IntegerInput MimesisMasterPort;
        private DevComponents.DotNetBar.LabelX MasterBotIPAddressLabel;
        private DevComponents.DotNetBar.LabelX labelX6;
        private DevComponents.DotNetBar.LabelX MasterBotIPPortLabel;
        private DevComponents.DotNetBar.ButtonX saveB;
        private DevComponents.DotNetBar.LabelX ActivatePartyModeLabel;
        private DevComponents.DotNetBar.Controls.SwitchButton ActivatePartyMode;

    }
}