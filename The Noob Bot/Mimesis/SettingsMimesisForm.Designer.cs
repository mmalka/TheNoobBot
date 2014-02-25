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
            this.MimesisMasterAddr = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.MimesisMasterPort = new DevComponents.Editors.IntegerInput();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.saveB = new DevComponents.DotNetBar.ButtonX();
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
            this.MimesisMasterPort.Location = new System.Drawing.Point(169, 75);
            this.MimesisMasterPort.Name = "Port";
            this.MimesisMasterPort.Size = new System.Drawing.Size(121, 22);
            this.MimesisMasterPort.TabIndex = 17;
            // 
            // labelX4
            // 
            this.labelX4.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.Class = "";
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.ForeColor = System.Drawing.Color.Black;
            this.labelX4.Location = new System.Drawing.Point(12, 40);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(140, 22);
            this.labelX4.TabIndex = 16;
            this.labelX4.Text = "IP Address of the master Bot";
            // 
            // labelX6
            // 
            this.labelX6.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.labelX6.BackgroundStyle.Class = "";
            this.labelX6.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX6.ForeColor = System.Drawing.Color.Black;
            this.labelX6.Location = new System.Drawing.Point(12, 107);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(269, 22);
            this.labelX6.TabIndex = 19;
            this.labelX6.Text = "Nothing to says yet.";
            this.labelX6.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // labelX5
            // 
            this.labelX5.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.labelX5.BackgroundStyle.Class = "";
            this.labelX5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX5.ForeColor = System.Drawing.Color.Black;
            this.labelX5.Location = new System.Drawing.Point(12, 73);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(140, 22);
            this.labelX5.TabIndex = 18;
            this.labelX5.Text = "Port used by the master bot";
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
            // SettingsMimesisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(301, 133);
            this.Controls.Add(this.saveB);
            this.Controls.Add(this.labelX6);
            this.Controls.Add(this.labelX5);
            this.Controls.Add(this.MimesisMasterPort);
            this.Controls.Add(this.labelX4);
            this.Controls.Add(this.MimesisMasterAddr);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
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
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.LabelX labelX6;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.ButtonX saveB;

    }
}