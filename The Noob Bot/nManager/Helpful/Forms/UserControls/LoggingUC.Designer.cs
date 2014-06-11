using System.Windows.Forms;
using nManager.Helpful.Forms.UserControls;

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

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoggingUC));
            this.DebugLogSwitchLabel = new System.Windows.Forms.Label();
            this.NavigationLogSwitchLabel = new System.Windows.Forms.Label();
            this.FightLogSwitchLabel = new System.Windows.Forms.Label();
            this.NormalLogSwitchLabel = new System.Windows.Forms.Label();
            this.LoggingTextArea = new System.Windows.Forms.RichTextBox();
            this.DebugLogSwitchButton = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.NavigationLogSwitchButton = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.FightLogSwitchButton = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.NormalLogSwitchButton = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.LoggingAreaTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // DebugLogSwitchLabel
            // 
            this.DebugLogSwitchLabel.AutoEllipsis = true;
            this.DebugLogSwitchLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DebugLogSwitchLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.DebugLogSwitchLabel.Location = new System.Drawing.Point(493, 109);
            this.DebugLogSwitchLabel.Name = "DebugLogSwitchLabel";
            this.DebugLogSwitchLabel.Size = new System.Drawing.Size(70, 20);
            this.DebugLogSwitchLabel.TabIndex = 12;
            this.DebugLogSwitchLabel.Text = "Debug";
            this.DebugLogSwitchLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // NavigationLogSwitchLabel
            // 
            this.NavigationLogSwitchLabel.AutoEllipsis = true;
            this.NavigationLogSwitchLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NavigationLogSwitchLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.NavigationLogSwitchLabel.Location = new System.Drawing.Point(493, 77);
            this.NavigationLogSwitchLabel.Name = "NavigationLogSwitchLabel";
            this.NavigationLogSwitchLabel.Size = new System.Drawing.Size(70, 20);
            this.NavigationLogSwitchLabel.TabIndex = 11;
            this.NavigationLogSwitchLabel.Text = "Navigation";
            this.NavigationLogSwitchLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FightLogSwitchLabel
            // 
            this.FightLogSwitchLabel.AutoEllipsis = true;
            this.FightLogSwitchLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FightLogSwitchLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.FightLogSwitchLabel.Location = new System.Drawing.Point(493, 45);
            this.FightLogSwitchLabel.Name = "FightLogSwitchLabel";
            this.FightLogSwitchLabel.Size = new System.Drawing.Size(70, 20);
            this.FightLogSwitchLabel.TabIndex = 10;
            this.FightLogSwitchLabel.Text = "Fight";
            this.FightLogSwitchLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // NormalLogSwitchLabel
            // 
            this.NormalLogSwitchLabel.AutoEllipsis = true;
            this.NormalLogSwitchLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.NormalLogSwitchLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NormalLogSwitchLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.NormalLogSwitchLabel.Location = new System.Drawing.Point(493, 13);
            this.NormalLogSwitchLabel.Name = "NormalLogSwitchLabel";
            this.NormalLogSwitchLabel.Size = new System.Drawing.Size(70, 20);
            this.NormalLogSwitchLabel.TabIndex = 9;
            this.NormalLogSwitchLabel.Text = "Normal";
            this.NormalLogSwitchLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LoggingTextArea
            // 
            this.LoggingTextArea.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.LoggingTextArea.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.LoggingTextArea.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.42F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.LoggingTextArea.Location = new System.Drawing.Point(13, 13);
            this.LoggingTextArea.Margin = new System.Windows.Forms.Padding(0);
            this.LoggingTextArea.MaximumSize = new System.Drawing.Size(398, 176);
            this.LoggingTextArea.MinimumSize = new System.Drawing.Size(398, 176);
            this.LoggingTextArea.Name = "LoggingTextArea";
            this.LoggingTextArea.ReadOnly = true;
            this.LoggingTextArea.RightMargin = 355;
            this.LoggingTextArea.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.LoggingTextArea.Size = new System.Drawing.Size(398, 176);
            this.LoggingTextArea.TabIndex = 0;
            this.LoggingTextArea.Text = "";
            this.LoggingTextArea.VisibleChanged += new System.EventHandler(this.LoggingTextArea_VisibleChanged);
            // 
            // DebugLogSwitchButton
            // 
            this.DebugLogSwitchButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.DebugLogSwitchButton.Location = new System.Drawing.Point(424, 109);
            this.DebugLogSwitchButton.MaximumSize = new System.Drawing.Size(60, 20);
            this.DebugLogSwitchButton.MinimumSize = new System.Drawing.Size(60, 20);
            this.DebugLogSwitchButton.Name = "DebugLogSwitchButton";
            this.DebugLogSwitchButton.OffText = "OFF";
            this.DebugLogSwitchButton.OnText = "ON";
            this.DebugLogSwitchButton.Size = new System.Drawing.Size(60, 20);
            this.DebugLogSwitchButton.TabIndex = 16;
            this.DebugLogSwitchButton.Value = false;
            this.DebugLogSwitchButton.ValueChanged += new System.EventHandler(this.LoggingSwitchs_ValueChanged);
            // 
            // NavigationLogSwitchButton
            // 
            this.NavigationLogSwitchButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.NavigationLogSwitchButton.Location = new System.Drawing.Point(424, 77);
            this.NavigationLogSwitchButton.MaximumSize = new System.Drawing.Size(60, 20);
            this.NavigationLogSwitchButton.MinimumSize = new System.Drawing.Size(60, 20);
            this.NavigationLogSwitchButton.Name = "NavigationLogSwitchButton";
            this.NavigationLogSwitchButton.OffText = "OFF";
            this.NavigationLogSwitchButton.OnText = "ON";
            this.NavigationLogSwitchButton.Size = new System.Drawing.Size(60, 20);
            this.NavigationLogSwitchButton.TabIndex = 15;
            this.NavigationLogSwitchButton.Value = false;
            this.NavigationLogSwitchButton.ValueChanged += new System.EventHandler(this.LoggingSwitchs_ValueChanged);
            // 
            // FightLogSwitchButton
            // 
            this.FightLogSwitchButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.FightLogSwitchButton.Location = new System.Drawing.Point(424, 45);
            this.FightLogSwitchButton.MaximumSize = new System.Drawing.Size(60, 20);
            this.FightLogSwitchButton.MinimumSize = new System.Drawing.Size(60, 20);
            this.FightLogSwitchButton.Name = "FightLogSwitchButton";
            this.FightLogSwitchButton.OffText = "OFF";
            this.FightLogSwitchButton.OnText = "ON";
            this.FightLogSwitchButton.Size = new System.Drawing.Size(60, 20);
            this.FightLogSwitchButton.TabIndex = 14;
            this.FightLogSwitchButton.Value = true;
            this.FightLogSwitchButton.ValueChanged += new System.EventHandler(this.LoggingSwitchs_ValueChanged);
            // 
            // NormalLogSwitchButton
            // 
            this.NormalLogSwitchButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.NormalLogSwitchButton.Location = new System.Drawing.Point(424, 13);
            this.NormalLogSwitchButton.MaximumSize = new System.Drawing.Size(60, 20);
            this.NormalLogSwitchButton.MinimumSize = new System.Drawing.Size(60, 20);
            this.NormalLogSwitchButton.Name = "NormalLogSwitchButton";
            this.NormalLogSwitchButton.OffText = "OFF";
            this.NormalLogSwitchButton.OnText = "ON";
            this.NormalLogSwitchButton.Size = new System.Drawing.Size(60, 20);
            this.NormalLogSwitchButton.TabIndex = 13;
            this.NormalLogSwitchButton.Value = true;
            this.NormalLogSwitchButton.ValueChanged += new System.EventHandler(this.LoggingSwitchs_ValueChanged);
            // 
            // LoggingAreaTimer
            // 
            this.LoggingAreaTimer.Enabled = true;
            this.LoggingAreaTimer.Tick += new System.EventHandler(this.LoggingAreaTimer_Tick);
            // 
            // LoggingUC
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.Controls.Add(this.NormalLogSwitchButton);
            this.Controls.Add(this.NormalLogSwitchLabel);
            this.Controls.Add(this.DebugLogSwitchButton);
            this.Controls.Add(this.DebugLogSwitchLabel);
            this.Controls.Add(this.NavigationLogSwitchButton);
            this.Controls.Add(this.NavigationLogSwitchLabel);
            this.Controls.Add(this.FightLogSwitchButton);
            this.Controls.Add(this.FightLogSwitchLabel);
            this.Controls.Add(this.LoggingTextArea);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MaximumSize = new System.Drawing.Size(573, 203);
            this.MinimumSize = new System.Drawing.Size(573, 203);
            this.Name = "LoggingUC";
            this.Size = new System.Drawing.Size(573, 203);
            this.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.LoggingUC_ControlRemoved);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox LoggingTextArea;
        private System.Windows.Forms.Label DebugLogSwitchLabel;
        private System.Windows.Forms.Label NavigationLogSwitchLabel;
        private System.Windows.Forms.Label FightLogSwitchLabel;
        private System.Windows.Forms.Label NormalLogSwitchLabel;
        private TnbSwitchButton NormalLogSwitchButton;
        private TnbSwitchButton FightLogSwitchButton;
        private TnbSwitchButton DebugLogSwitchButton;
        private TnbSwitchButton NavigationLogSwitchButton;
        private System.Windows.Forms.Timer LoggingAreaTimer;
    }
}

