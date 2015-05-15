using System.Drawing;
using System.Windows.Forms;
using nManager.Helpful.Forms.UserControls;

namespace Damage_Dealer
{
    partial class DamageDealerSettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DamageDealerSettingsForm));
            this.ActivateAutoFacingLabel = new System.Windows.Forms.Label();
            this.ActivateMovementsLabel = new System.Windows.Forms.Label();
            this.MainHeader = new nManager.Helpful.Forms.UserControls.TnbControlMenu();
            this.ActivateMovements = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.SaveAndCloseButton = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.ActivateAutoFacing = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.SpellSettingsShortcutButton = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.SuspendLayout();
            // 
            // ActivateAutoFacingLabel
            // 
            this.ActivateAutoFacingLabel.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.ActivateAutoFacingLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.ActivateAutoFacingLabel.Location = new System.Drawing.Point(12, 52);
            this.ActivateAutoFacingLabel.Name = "ActivateAutoFacingLabel";
            this.ActivateAutoFacingLabel.Size = new System.Drawing.Size(230, 22);
            this.ActivateAutoFacingLabel.TabIndex = 16;
            this.ActivateAutoFacingLabel.Text = "Automatically face (Good for AFK only)";
            this.ActivateAutoFacingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ActivateMovementsLabel
            // 
            this.ActivateMovementsLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ActivateMovementsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.ActivateMovementsLabel.Location = new System.Drawing.Point(12, 90);
            this.ActivateMovementsLabel.Name = "ActivateMovementsLabel";
            this.ActivateMovementsLabel.Size = new System.Drawing.Size(230, 22);
            this.ActivateMovementsLabel.TabIndex = 22;
            this.ActivateMovementsLabel.Text = "Allow Movements (Good for AFK only)";
            this.ActivateMovementsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.MainHeader.TitleText = "Damage Dealer Settings";
            // 
            // ActivateMovements
            // 
            this.ActivateMovements.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.ActivateMovements.Location = new System.Drawing.Point(248, 90);
            this.ActivateMovements.MaximumSize = new System.Drawing.Size(60, 20);
            this.ActivateMovements.MinimumSize = new System.Drawing.Size(60, 20);
            this.ActivateMovements.Name = "ActivateMovements";
            this.ActivateMovements.OffText = "OFF";
            this.ActivateMovements.OnText = "ON";
            this.ActivateMovements.Size = new System.Drawing.Size(60, 20);
            this.ActivateMovements.TabIndex = 23;
            this.ActivateMovements.Value = false;
            // 
            // SaveAndCloseButton
            // 
            this.SaveAndCloseButton.AutoEllipsis = true;
            this.SaveAndCloseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.SaveAndCloseButton.ForeColor = System.Drawing.Color.Snow;
            this.SaveAndCloseButton.HooverImage = ((System.Drawing.Image)(resources.GetObject("SaveAndCloseButton.HooverImage")));
            this.SaveAndCloseButton.Image = ((System.Drawing.Image)(resources.GetObject("SaveAndCloseButton.Image")));
            this.SaveAndCloseButton.Location = new System.Drawing.Point(151, 137);
            this.SaveAndCloseButton.Name = "SaveAndCloseButton";
            this.SaveAndCloseButton.Size = new System.Drawing.Size(150, 27);
            this.SaveAndCloseButton.TabIndex = 20;
            this.SaveAndCloseButton.Text = "Save and Close";
            this.SaveAndCloseButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.SaveAndCloseButton.Click += new System.EventHandler(this.SaveAndCloseButton_Click);
            // 
            // ActivateAutoFacing
            // 
            this.ActivateAutoFacing.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.ActivateAutoFacing.Location = new System.Drawing.Point(248, 54);
            this.ActivateAutoFacing.MaximumSize = new System.Drawing.Size(60, 20);
            this.ActivateAutoFacing.MinimumSize = new System.Drawing.Size(60, 20);
            this.ActivateAutoFacing.Name = "ActivateAutoFacing";
            this.ActivateAutoFacing.OffText = "OFF";
            this.ActivateAutoFacing.OnText = "ON";
            this.ActivateAutoFacing.Size = new System.Drawing.Size(60, 20);
            this.ActivateAutoFacing.TabIndex = 24;
            this.ActivateAutoFacing.Value = false;
            // 
            // SpellSettingsShortcutButton
            // 
            this.SpellSettingsShortcutButton.AutoEllipsis = true;
            this.SpellSettingsShortcutButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.SpellSettingsShortcutButton.ForeColor = System.Drawing.Color.Snow;
            this.SpellSettingsShortcutButton.HooverImage = ((System.Drawing.Image)(resources.GetObject("SpellSettingsShortcutButton.HooverImage")));
            this.SpellSettingsShortcutButton.Image = ((System.Drawing.Image)(resources.GetObject("SpellSettingsShortcutButton.Image")));
            this.SpellSettingsShortcutButton.Location = new System.Drawing.Point(27, 136);
            this.SpellSettingsShortcutButton.Name = "SpellSettingsShortcutButton";
            this.SpellSettingsShortcutButton.Size = new System.Drawing.Size(106, 29);
            this.SpellSettingsShortcutButton.TabIndex = 25;
            this.SpellSettingsShortcutButton.Text = "Spell Settings";
            this.SpellSettingsShortcutButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.SpellSettingsShortcutButton.Click += new System.EventHandler(this.SpellSettingsShortcutButton_Click);
            // 
            // SettingsMimesisForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(320, 178);
            this.Controls.Add(this.SpellSettingsShortcutButton);
            this.Controls.Add(this.ActivateAutoFacing);
            this.Controls.Add(this.MainHeader);
            this.Controls.Add(this.ActivateMovements);
            this.Controls.Add(this.ActivateMovementsLabel);
            this.Controls.Add(this.SaveAndCloseButton);
            this.Controls.Add(this.ActivateAutoFacingLabel);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SettingsMimesisForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Mimesis Settings";
            this.ResumeLayout(false);

        }

        #endregion

        private Label ActivateAutoFacingLabel;
        private TnbButton SaveAndCloseButton;
        private Label ActivateMovementsLabel;
        private TnbSwitchButton ActivateMovements;
        private TnbControlMenu MainHeader;
        private TnbSwitchButton ActivateAutoFacing;
        private TnbButton SpellSettingsShortcutButton;

    }
}