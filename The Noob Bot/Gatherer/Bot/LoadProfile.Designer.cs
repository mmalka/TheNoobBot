namespace Gatherer.Bot
{
    partial class LoadProfile
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
            this.loadProfileB = new DevComponents.DotNetBar.ButtonX();
            this.listProfileCb = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.createProfileB = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // loadProfileB
            // 
            this.loadProfileB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.loadProfileB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.loadProfileB.Location = new System.Drawing.Point(4, 67);
            this.loadProfileB.Name = "loadProfileB";
            this.loadProfileB.Size = new System.Drawing.Size(277, 23);
            this.loadProfileB.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.loadProfileB.TabIndex = 1;
            this.loadProfileB.Text = "Load Profile";
            this.loadProfileB.Click += new System.EventHandler(this.loadProfileB_Click);
            // 
            // listProfileCb
            // 
            this.listProfileCb.DisplayMember = "Text";
            this.listProfileCb.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listProfileCb.ForeColor = System.Drawing.Color.Black;
            this.listProfileCb.FormattingEnabled = true;
            this.listProfileCb.ItemHeight = 16;
            this.listProfileCb.Location = new System.Drawing.Point(4, 39);
            this.listProfileCb.Name = "listProfileCb";
            this.listProfileCb.Size = new System.Drawing.Size(277, 22);
            this.listProfileCb.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.listProfileCb.TabIndex = 0;
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.WhiteSmoke;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX1.ForeColor = System.Drawing.Color.Black;
            this.labelX1.Location = new System.Drawing.Point(4, 12);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 23);
            this.labelX1.TabIndex = 2;
            this.labelX1.Text = "Profile:";
            // 
            // createProfileB
            // 
            this.createProfileB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.createProfileB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.createProfileB.Location = new System.Drawing.Point(159, 12);
            this.createProfileB.Name = "createProfileB";
            this.createProfileB.Size = new System.Drawing.Size(122, 23);
            this.createProfileB.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.createProfileB.TabIndex = 3;
            this.createProfileB.Text = "Profile creator";
            this.createProfileB.Click += new System.EventHandler(this.createProfileB_Click);
            // 
            // LoadProfile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(100F, 100F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ClientSize = new System.Drawing.Size(284, 91);
            this.Controls.Add(this.createProfileB);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.loadProfileB);
            this.Controls.Add(this.listProfileCb);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(300, 129);
            this.MinimumSize = new System.Drawing.Size(300, 129);
            this.Name = "LoadProfile";
            this.ShowIcon = false;
            this.ShowInTaskbar = true;
            this.Text = "Load Gatherer Profile";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoadProfile_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX loadProfileB;
        private DevComponents.DotNetBar.Controls.ComboBoxEx listProfileCb;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX createProfileB;
    }
}