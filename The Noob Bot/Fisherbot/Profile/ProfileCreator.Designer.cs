namespace Fisherbot.Profile
{
    partial class ProfileCreator
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
            this.recordWayB = new DevComponents.DotNetBar.ButtonX();
            this.saveB = new DevComponents.DotNetBar.ButtonX();
            this.listPoint = new System.Windows.Forms.ListBox();
            this.nSeparatorDistance = new DevComponents.Editors.IntegerInput();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.delB = new DevComponents.DotNetBar.ButtonX();
            this.delBlackRadius = new DevComponents.DotNetBar.ButtonX();
            this.listBlackRadius = new System.Windows.Forms.ListBox();
            this.radiusN = new DevComponents.Editors.IntegerInput();
            this.addBlackB = new DevComponents.DotNetBar.ButtonX();
            this.loadB = new DevComponents.DotNetBar.ButtonX();
            ((System.ComponentModel.ISupportInitialize)(this.nSeparatorDistance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radiusN)).BeginInit();
            this.SuspendLayout();
            // 
            // recordWayB
            // 
            this.recordWayB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.recordWayB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.recordWayB.Location = new System.Drawing.Point(12, 241);
            this.recordWayB.Name = "recordWayB";
            this.recordWayB.Size = new System.Drawing.Size(352, 23);
            this.recordWayB.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.recordWayB.TabIndex = 1;
            this.recordWayB.Text = "Record Way";
            this.recordWayB.Click += new System.EventHandler(this.recordWayB_Click);
            // 
            // saveB
            // 
            this.saveB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.saveB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.saveB.Location = new System.Drawing.Point(318, 8);
            this.saveB.Name = "saveB";
            this.saveB.Size = new System.Drawing.Size(58, 19);
            this.saveB.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.saveB.TabIndex = 2;
            this.saveB.Text = "Save";
            this.saveB.Click += new System.EventHandler(this.saveB_Click);
            // 
            // listPoint
            // 
            this.listPoint.BackColor = System.Drawing.Color.White;
            this.listPoint.ForeColor = System.Drawing.Color.Black;
            this.listPoint.FormattingEnabled = true;
            this.listPoint.Location = new System.Drawing.Point(1, 36);
            this.listPoint.Name = "listPoint";
            this.listPoint.Size = new System.Drawing.Size(375, 199);
            this.listPoint.TabIndex = 4;
            // 
            // nSeparatorDistance
            // 
            this.nSeparatorDistance.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.nSeparatorDistance.BackgroundStyle.Class = "DateTimeInputBackground";
            this.nSeparatorDistance.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.nSeparatorDistance.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.nSeparatorDistance.ForeColor = System.Drawing.Color.Black;
            this.nSeparatorDistance.Location = new System.Drawing.Point(145, 5);
            this.nSeparatorDistance.MaxValue = 200;
            this.nSeparatorDistance.MinValue = 1;
            this.nSeparatorDistance.Name = "nSeparatorDistance";
            this.nSeparatorDistance.ShowUpDown = true;
            this.nSeparatorDistance.Size = new System.Drawing.Size(94, 22);
            this.nSeparatorDistance.TabIndex = 5;
            this.nSeparatorDistance.Value = 2;
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.ForeColor = System.Drawing.Color.Black;
            this.labelX1.Location = new System.Drawing.Point(1, 4);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(138, 23);
            this.labelX1.TabIndex = 6;
            this.labelX1.Text = "Separation distance record:";
            // 
            // delB
            // 
            this.delB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.delB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.delB.Location = new System.Drawing.Point(339, 36);
            this.delB.Name = "delB";
            this.delB.Size = new System.Drawing.Size(37, 23);
            this.delB.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.delB.TabIndex = 8;
            this.delB.Text = "Del";
            this.delB.Click += new System.EventHandler(this.delB_Click);
            // 
            // delBlackRadius
            // 
            this.delBlackRadius.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.delBlackRadius.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.delBlackRadius.Location = new System.Drawing.Point(339, 278);
            this.delBlackRadius.Name = "delBlackRadius";
            this.delBlackRadius.Size = new System.Drawing.Size(37, 18);
            this.delBlackRadius.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.delBlackRadius.TabIndex = 10;
            this.delBlackRadius.Text = "Del";
            this.delBlackRadius.Click += new System.EventHandler(this.delBlackRadius_Click);
            // 
            // listBlackRadius
            // 
            this.listBlackRadius.BackColor = System.Drawing.Color.White;
            this.listBlackRadius.ForeColor = System.Drawing.Color.Black;
            this.listBlackRadius.FormattingEnabled = true;
            this.listBlackRadius.Location = new System.Drawing.Point(1, 278);
            this.listBlackRadius.Name = "listBlackRadius";
            this.listBlackRadius.Size = new System.Drawing.Size(375, 69);
            this.listBlackRadius.TabIndex = 9;
            // 
            // radiusN
            // 
            this.radiusN.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.radiusN.BackgroundStyle.Class = "DateTimeInputBackground";
            this.radiusN.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.radiusN.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.radiusN.ForeColor = System.Drawing.Color.Black;
            this.radiusN.Location = new System.Drawing.Point(12, 354);
            this.radiusN.MinValue = 1;
            this.radiusN.Name = "radiusN";
            this.radiusN.ShowUpDown = true;
            this.radiusN.Size = new System.Drawing.Size(113, 22);
            this.radiusN.TabIndex = 11;
            this.radiusN.Value = 35;
            // 
            // addBlackB
            // 
            this.addBlackB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.addBlackB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.addBlackB.Location = new System.Drawing.Point(131, 353);
            this.addBlackB.Name = "addBlackB";
            this.addBlackB.Size = new System.Drawing.Size(233, 23);
            this.addBlackB.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.addBlackB.TabIndex = 12;
            this.addBlackB.Text = "Add this position to Black list Radius";
            this.addBlackB.Click += new System.EventHandler(this.addBlackB_Click);
            // 
            // loadB
            // 
            this.loadB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.loadB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.loadB.Location = new System.Drawing.Point(254, 8);
            this.loadB.Name = "loadB";
            this.loadB.Size = new System.Drawing.Size(58, 19);
            this.loadB.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.loadB.TabIndex = 18;
            this.loadB.Text = "Load";
            this.loadB.Click += new System.EventHandler(this.loadB_Click);
            // 
            // ProfileCreator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(100F, 100F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ClientSize = new System.Drawing.Size(379, 379);
            this.Controls.Add(this.loadB);
            this.Controls.Add(this.addBlackB);
            this.Controls.Add(this.radiusN);
            this.Controls.Add(this.delBlackRadius);
            this.Controls.Add(this.listBlackRadius);
            this.Controls.Add(this.delB);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.nSeparatorDistance);
            this.Controls.Add(this.listPoint);
            this.Controls.Add(this.saveB);
            this.Controls.Add(this.recordWayB);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(395, 417);
            this.MinimumSize = new System.Drawing.Size(395, 417);
            this.Name = "ProfileCreator";
            this.ShowIcon = false;
            this.ShowInTaskbar = true;
            this.Text = "Profile Creator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProfileCreator_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.nSeparatorDistance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radiusN)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX recordWayB;
        private DevComponents.DotNetBar.ButtonX saveB;
        private System.Windows.Forms.ListBox listPoint;
        private DevComponents.Editors.IntegerInput nSeparatorDistance;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX delB;
        private DevComponents.DotNetBar.ButtonX delBlackRadius;
        private System.Windows.Forms.ListBox listBlackRadius;
        private DevComponents.Editors.IntegerInput radiusN;
        private DevComponents.DotNetBar.ButtonX addBlackB;
        private DevComponents.DotNetBar.ButtonX loadB;
    }
}