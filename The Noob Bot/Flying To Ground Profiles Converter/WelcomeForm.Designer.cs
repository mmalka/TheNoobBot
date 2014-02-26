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
            this.convertB = new DevComponents.DotNetBar.ButtonX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.SuspendLayout();
            // 
            // convertB
            // 
            this.convertB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.convertB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.convertB.Location = new System.Drawing.Point(12, 95);
            this.convertB.Name = "convertB";
            this.convertB.Size = new System.Drawing.Size(481, 35);
            this.convertB.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.convertB.TabIndex = 0;
            this.convertB.Text = "Select Flying profiles to convert into Ground profiles";
            this.convertB.Click += new System.EventHandler(this.convertB_Click);
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(12, -2);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(481, 17);
            this.labelX1.TabIndex = 1;
            this.labelX1.Text = "The <i>Flying To Ground Profiles Converter</i> works with <b>Gatherer</b>, <b>Gri" +
                "nder</b> and <b>Fisherbot</b> profiles.";
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(12, 21);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(481, 17);
            this.labelX2.TabIndex = 2;
            this.labelX2.Text = "The conversion process <u>can take minutes</u>. The generated profiles might not " +
                "be good Grounds profiles, ";
            // 
            // labelX3
            // 
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.Class = "";
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(12, 56);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(481, 17);
            this.labelX3.TabIndex = 3;
            this.labelX3.Text = "if you have issue with the generated profile, just create one by hand for this zo" +
                "ne.";
            // 
            // WelcomeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(503, 136);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.convertB);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(519, 174);
            this.MinimumSize = new System.Drawing.Size(519, 174);
            this.Name = "WelcomeForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Flying To Ground Profiles Converter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.form_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX convertB;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX3;
    }
}