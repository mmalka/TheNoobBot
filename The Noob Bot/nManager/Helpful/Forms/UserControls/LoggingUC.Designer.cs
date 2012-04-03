namespace nManager.Helpful.Forms.UserControls
{
    partial class LoggingUC
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.richTextBox = new System.Windows.Forms.RichTextBox();
            this.updateLog = new System.Windows.Forms.Timer(this.components);
            this.normalCb = new DevComponents.DotNetBar.Controls.SwitchButton();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.debugCb = new DevComponents.DotNetBar.Controls.SwitchButton();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.navigatorCb = new DevComponents.DotNetBar.Controls.SwitchButton();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.fightCb = new DevComponents.DotNetBar.Controls.SwitchButton();
            this.SuspendLayout();
            // 
            // richTextBox
            // 
            this.richTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.richTextBox.Location = new System.Drawing.Point(3, 3);
            this.richTextBox.Name = "richTextBox";
            this.richTextBox.ReadOnly = true;
            this.richTextBox.Size = new System.Drawing.Size(201, 144);
            this.richTextBox.TabIndex = 3;
            this.richTextBox.Text = "";
            // 
            // updateLog
            // 
            this.updateLog.Enabled = true;
            this.updateLog.Interval = 150;
            this.updateLog.Tick += new System.EventHandler(this.UpdateLogTick);
            // 
            // normalCb
            // 
            this.normalCb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.normalCb.BackgroundStyle.Class = "";
            this.normalCb.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.normalCb.Location = new System.Drawing.Point(210, 12);
            this.normalCb.Name = "normalCb";
            this.normalCb.Size = new System.Drawing.Size(58, 20);
            this.normalCb.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.normalCb.TabIndex = 8;
            this.normalCb.Value = true;
            this.normalCb.ValueChanged += new System.EventHandler(this.CbCheckedChanged);
            // 
            // labelX1
            // 
            this.labelX1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(274, 9);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 23);
            this.labelX1.TabIndex = 9;
            this.labelX1.Text = "Normal";
            // 
            // labelX2
            // 
            this.labelX2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(274, 124);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(75, 23);
            this.labelX2.TabIndex = 11;
            this.labelX2.Text = "Debug";
            // 
            // debugCb
            // 
            this.debugCb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.debugCb.BackgroundStyle.Class = "";
            this.debugCb.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.debugCb.Location = new System.Drawing.Point(210, 127);
            this.debugCb.Name = "debugCb";
            this.debugCb.Size = new System.Drawing.Size(58, 20);
            this.debugCb.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.debugCb.TabIndex = 10;
            this.debugCb.ValueChanged += new System.EventHandler(this.CbCheckedChanged);
            // 
            // labelX3
            // 
            this.labelX3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.Class = "";
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(274, 85);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(75, 23);
            this.labelX3.TabIndex = 13;
            this.labelX3.Text = "Navigator";
            // 
            // navigatorCb
            // 
            this.navigatorCb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.navigatorCb.BackgroundStyle.Class = "";
            this.navigatorCb.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.navigatorCb.Location = new System.Drawing.Point(210, 88);
            this.navigatorCb.Name = "navigatorCb";
            this.navigatorCb.Size = new System.Drawing.Size(58, 20);
            this.navigatorCb.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.navigatorCb.TabIndex = 12;
            this.navigatorCb.ValueChanged += new System.EventHandler(this.CbCheckedChanged);
            // 
            // labelX4
            // 
            this.labelX4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.Class = "";
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Location = new System.Drawing.Point(274, 47);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(75, 23);
            this.labelX4.TabIndex = 15;
            this.labelX4.Text = "Fight";
            // 
            // fightCb
            // 
            this.fightCb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.fightCb.BackgroundStyle.Class = "";
            this.fightCb.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.fightCb.Location = new System.Drawing.Point(210, 50);
            this.fightCb.Name = "fightCb";
            this.fightCb.Size = new System.Drawing.Size(58, 20);
            this.fightCb.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.fightCb.TabIndex = 14;
            this.fightCb.Value = true;
            this.fightCb.ValueChanged += new System.EventHandler(this.CbCheckedChanged);
            // 
            // LoggingUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelX4);
            this.Controls.Add(this.fightCb);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.navigatorCb);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.debugCb);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.normalCb);
            this.Controls.Add(this.richTextBox);
            this.Name = "LoggingUC";
            this.Size = new System.Drawing.Size(327, 152);
            this.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.LoggingUcControlRemoved);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox;
        private System.Windows.Forms.Timer updateLog;
        private DevComponents.DotNetBar.Controls.SwitchButton normalCb;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.SwitchButton debugCb;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.SwitchButton navigatorCb;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.Controls.SwitchButton fightCb;
    }
}
