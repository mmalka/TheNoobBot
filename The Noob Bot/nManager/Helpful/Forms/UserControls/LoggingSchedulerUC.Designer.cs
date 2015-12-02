using System.Windows.Forms;
using nManager.Helpful.Forms.UserControls;

namespace nManager.Helpful.Forms.UserControls
{
    partial class LoggingSchedulerUC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoggingSchedulerUC));
            this.DebugLogSwitchLabel = new System.Windows.Forms.Label();
            this.NormalLogSwitchLabel = new System.Windows.Forms.Label();
            this.LoggingTextArea = new System.Windows.Forms.RichTextBox();
            this.LoggingAreaTimer = new System.Windows.Forms.Timer(this.components);
            this.NormalLogSwitchButton = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.DebugLogSwitchButton = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.SuspendLayout();
            // 
            // DebugLogSwitchLabel
            // 
            this.DebugLogSwitchLabel.AutoEllipsis = true;
            this.DebugLogSwitchLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DebugLogSwitchLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.DebugLogSwitchLabel.Location = new System.Drawing.Point(720, 45);
            this.DebugLogSwitchLabel.Name = "DebugLogSwitchLabel";
            this.DebugLogSwitchLabel.Size = new System.Drawing.Size(70, 20);
            this.DebugLogSwitchLabel.TabIndex = 12;
            this.DebugLogSwitchLabel.Text = "Debug";
            this.DebugLogSwitchLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // NormalLogSwitchLabel
            // 
            this.NormalLogSwitchLabel.AutoEllipsis = true;
            this.NormalLogSwitchLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.NormalLogSwitchLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NormalLogSwitchLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.NormalLogSwitchLabel.Location = new System.Drawing.Point(720, 13);
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
            this.LoggingTextArea.Location = new System.Drawing.Point(14, 14);
            this.LoggingTextArea.Margin = new System.Windows.Forms.Padding(0);
            this.LoggingTextArea.MaximumSize = new System.Drawing.Size(625, 176);
            this.LoggingTextArea.MinimumSize = new System.Drawing.Size(625, 176);
            this.LoggingTextArea.Name = "LoggingTextArea";
            this.LoggingTextArea.ReadOnly = true;
            this.LoggingTextArea.RightMargin = 582;
            this.LoggingTextArea.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.LoggingTextArea.Size = new System.Drawing.Size(625, 176);
            this.LoggingTextArea.TabIndex = 0;
            this.LoggingTextArea.Text = "";
            this.LoggingTextArea.VisibleChanged += new System.EventHandler(this.LoggingTextArea_VisibleChanged);
            // 
            // LoggingAreaTimer
            // 
            this.LoggingAreaTimer.Enabled = true;
            this.LoggingAreaTimer.Tick += new System.EventHandler(this.LoggingAreaTimer_Tick);
            // 
            // NormalLogSwitchButton
            // 
            this.NormalLogSwitchButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.NormalLogSwitchButton.Location = new System.Drawing.Point(651, 13);
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
            // DebugLogSwitchButton
            // 
            this.DebugLogSwitchButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.DebugLogSwitchButton.Location = new System.Drawing.Point(651, 45);
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
            // LoggingSchedulerUC
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.Controls.Add(this.NormalLogSwitchButton);
            this.Controls.Add(this.NormalLogSwitchLabel);
            this.Controls.Add(this.DebugLogSwitchButton);
            this.Controls.Add(this.DebugLogSwitchLabel);
            this.Controls.Add(this.LoggingTextArea);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MaximumSize = new System.Drawing.Size(800, 202);
            this.MinimumSize = new System.Drawing.Size(800, 202);
            this.Name = "LoggingSchedulerUC";
            this.Size = new System.Drawing.Size(800, 202);
            this.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.LoggingSchedulerUC_ControlRemoved);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox LoggingTextArea;
        private System.Windows.Forms.Label DebugLogSwitchLabel;
        private System.Windows.Forms.Label NormalLogSwitchLabel;
        private TnbSwitchButton NormalLogSwitchButton;
        private TnbSwitchButton DebugLogSwitchButton;
        private System.Windows.Forms.Timer LoggingAreaTimer;
    }
}

