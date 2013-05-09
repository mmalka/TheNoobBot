namespace Tracker
{
    partial class FormTracker
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTracker));
            this.ctNoTrack = new System.Windows.Forms.ListBox();
            this.otNoTrack = new System.Windows.Forms.ListBox();
            this.unNoTrack = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.ctTrack = new System.Windows.Forms.ListBox();
            this.otTrack = new System.Windows.Forms.ListBox();
            this.unTrack = new System.Windows.Forms.ListBox();
            this.unAddTrack = new DevComponents.DotNetBar.ButtonX();
            this.unDelTrack = new DevComponents.DotNetBar.ButtonX();
            this.otDelTrack = new DevComponents.DotNetBar.ButtonX();
            this.otAddTrack = new DevComponents.DotNetBar.ButtonX();
            this.ctDelTrack = new DevComponents.DotNetBar.ButtonX();
            this.ctAddTrack = new DevComponents.DotNetBar.ButtonX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // ctNoTrack
            // 
            this.ctNoTrack.FormattingEnabled = true;
            this.ctNoTrack.Location = new System.Drawing.Point(12, 35);
            this.ctNoTrack.Name = "ctNoTrack";
            this.ctNoTrack.Size = new System.Drawing.Size(133, 95);
            this.ctNoTrack.TabIndex = 0;
            // 
            // otNoTrack
            // 
            this.otNoTrack.FormattingEnabled = true;
            this.otNoTrack.Location = new System.Drawing.Point(12, 161);
            this.otNoTrack.Name = "otNoTrack";
            this.otNoTrack.Size = new System.Drawing.Size(133, 95);
            this.otNoTrack.TabIndex = 1;
            // 
            // unNoTrack
            // 
            this.unNoTrack.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.unNoTrack.Border.Class = "TextBoxBorder";
            this.unNoTrack.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.unNoTrack.ForeColor = System.Drawing.Color.Black;
            this.unNoTrack.Location = new System.Drawing.Point(12, 295);
            this.unNoTrack.Name = "unNoTrack";
            this.unNoTrack.Size = new System.Drawing.Size(133, 22);
            this.unNoTrack.TabIndex = 2;
            // 
            // ctTrack
            // 
            this.ctTrack.FormattingEnabled = true;
            this.ctTrack.Location = new System.Drawing.Point(194, 35);
            this.ctTrack.Name = "ctTrack";
            this.ctTrack.Size = new System.Drawing.Size(133, 95);
            this.ctTrack.TabIndex = 3;
            // 
            // otTrack
            // 
            this.otTrack.FormattingEnabled = true;
            this.otTrack.Location = new System.Drawing.Point(194, 161);
            this.otTrack.Name = "otTrack";
            this.otTrack.Size = new System.Drawing.Size(133, 95);
            this.otTrack.TabIndex = 4;
            // 
            // unTrack
            // 
            this.unTrack.FormattingEnabled = true;
            this.unTrack.Location = new System.Drawing.Point(194, 275);
            this.unTrack.Name = "unTrack";
            this.unTrack.Size = new System.Drawing.Size(133, 69);
            this.unTrack.TabIndex = 5;
            // 
            // unAddTrack
            // 
            this.unAddTrack.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.unAddTrack.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.unAddTrack.Font = new System.Drawing.Font("Segoe UI", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.unAddTrack.Location = new System.Drawing.Point(151, 284);
            this.unAddTrack.Name = "unAddTrack";
            this.unAddTrack.Size = new System.Drawing.Size(37, 22);
            this.unAddTrack.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.unAddTrack.TabIndex = 6;
            this.unAddTrack.Text = ">";
            this.unAddTrack.Click += new System.EventHandler(this.unAddTrack_Click);
            // 
            // unDelTrack
            // 
            this.unDelTrack.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.unDelTrack.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.unDelTrack.Font = new System.Drawing.Font("Segoe UI", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.unDelTrack.Location = new System.Drawing.Point(151, 312);
            this.unDelTrack.Name = "unDelTrack";
            this.unDelTrack.Size = new System.Drawing.Size(37, 22);
            this.unDelTrack.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.unDelTrack.TabIndex = 7;
            this.unDelTrack.Text = "<";
            this.unDelTrack.Click += new System.EventHandler(this.unDelTrack_Click);
            // 
            // otDelTrack
            // 
            this.otDelTrack.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.otDelTrack.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.otDelTrack.Font = new System.Drawing.Font("Segoe UI", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.otDelTrack.Location = new System.Drawing.Point(151, 207);
            this.otDelTrack.Name = "otDelTrack";
            this.otDelTrack.Size = new System.Drawing.Size(37, 22);
            this.otDelTrack.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.otDelTrack.TabIndex = 9;
            this.otDelTrack.Text = "<";
            this.otDelTrack.Click += new System.EventHandler(this.otDelTrack_Click);
            // 
            // otAddTrack
            // 
            this.otAddTrack.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.otAddTrack.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.otAddTrack.Font = new System.Drawing.Font("Segoe UI", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.otAddTrack.Location = new System.Drawing.Point(151, 179);
            this.otAddTrack.Name = "otAddTrack";
            this.otAddTrack.Size = new System.Drawing.Size(37, 22);
            this.otAddTrack.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.otAddTrack.TabIndex = 8;
            this.otAddTrack.Text = ">";
            this.otAddTrack.Click += new System.EventHandler(this.otAddTrack_Click);
            // 
            // ctDelTrack
            // 
            this.ctDelTrack.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ctDelTrack.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.ctDelTrack.Font = new System.Drawing.Font("Segoe UI", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctDelTrack.Location = new System.Drawing.Point(151, 85);
            this.ctDelTrack.Name = "ctDelTrack";
            this.ctDelTrack.Size = new System.Drawing.Size(37, 22);
            this.ctDelTrack.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ctDelTrack.TabIndex = 11;
            this.ctDelTrack.Text = "<";
            this.ctDelTrack.Click += new System.EventHandler(this.ctDelTrack_Click);
            // 
            // ctAddTrack
            // 
            this.ctAddTrack.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ctAddTrack.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.ctAddTrack.Font = new System.Drawing.Font("Segoe UI", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctAddTrack.Location = new System.Drawing.Point(151, 57);
            this.ctAddTrack.Name = "ctAddTrack";
            this.ctAddTrack.Size = new System.Drawing.Size(37, 22);
            this.ctAddTrack.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ctAddTrack.TabIndex = 10;
            this.ctAddTrack.Text = ">";
            this.ctAddTrack.Click += new System.EventHandler(this.ctAddTrack_Click);
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(12, 266);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 23);
            this.labelX1.TabIndex = 12;
            this.labelX1.Text = "By npc name:";
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(12, 136);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(75, 23);
            this.labelX2.TabIndex = 13;
            this.labelX2.Text = "Object type:";
            // 
            // labelX3
            // 
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.Class = "";
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(12, 12);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(75, 23);
            this.labelX3.TabIndex = 14;
            this.labelX3.Text = "Creature type:";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(100F, 100F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ClientSize = new System.Drawing.Size(339, 344);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.ctDelTrack);
            this.Controls.Add(this.ctAddTrack);
            this.Controls.Add(this.otDelTrack);
            this.Controls.Add(this.otAddTrack);
            this.Controls.Add(this.unDelTrack);
            this.Controls.Add(this.unAddTrack);
            this.Controls.Add(this.unTrack);
            this.Controls.Add(this.otTrack);
            this.Controls.Add(this.ctTrack);
            this.Controls.Add(this.unNoTrack);
            this.Controls.Add(this.otNoTrack);
            this.Controls.Add(this.ctNoTrack);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(355, 382);
            this.MinimumSize = new System.Drawing.Size(355, 382);
            this.Name = "Form";
            this.Text = "Tracker";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox ctNoTrack;
        private System.Windows.Forms.ListBox otNoTrack;
        private DevComponents.DotNetBar.Controls.TextBoxX unNoTrack;
        private System.Windows.Forms.ListBox ctTrack;
        private System.Windows.Forms.ListBox otTrack;
        private System.Windows.Forms.ListBox unTrack;
        private DevComponents.DotNetBar.ButtonX unAddTrack;
        private DevComponents.DotNetBar.ButtonX unDelTrack;
        private DevComponents.DotNetBar.ButtonX otDelTrack;
        private DevComponents.DotNetBar.ButtonX otAddTrack;
        private DevComponents.DotNetBar.ButtonX ctDelTrack;
        private DevComponents.DotNetBar.ButtonX ctAddTrack;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX3;
        private System.Windows.Forms.Timer timer1;
    }
}