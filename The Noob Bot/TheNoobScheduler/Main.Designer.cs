namespace TheNoobScheduler
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.LoggingUC = new nManager.Helpful.Forms.UserControls.LoggingSchedulerUC();
            this.MainHeader = new nManager.Helpful.Forms.UserControls.TnbControlMenu();
            this.AccountList = new nManager.Helpful.Forms.UserControls.TnbComboBox();
            this.tnbRibbonManager1 = new nManager.Helpful.Forms.UserControls.TnbRibbonManager();
            this.AccountListLabel = new System.Windows.Forms.Label();
            this.AccountAddButton = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.AccountEditButton = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.AccountDelButton = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.CharactersList = new System.Windows.Forms.ListBox();
            this.CharacterNote = new System.Windows.Forms.RichTextBox();
            this.tnbButton1 = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.tnbButton2 = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.tnbButton3 = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.CharacterNoteSaveButton = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.StartGarrisonFarming = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.InfoLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LoggingUC
            // 
            this.LoggingUC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.LoggingUC.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("LoggingUC.BackgroundImage")));
            this.LoggingUC.Location = new System.Drawing.Point(1, 395);
            this.LoggingUC.Margin = new System.Windows.Forms.Padding(0);
            this.LoggingUC.MaximumSize = new System.Drawing.Size(798, 202);
            this.LoggingUC.MinimumSize = new System.Drawing.Size(798, 202);
            this.LoggingUC.Name = "LoggingUC";
            this.LoggingUC.Size = new System.Drawing.Size(798, 202);
            this.LoggingUC.TabIndex = 1;
            // 
            // MainHeader
            // 
            this.MainHeader.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("MainHeader.BackgroundImage")));
            this.MainHeader.Location = new System.Drawing.Point(0, 0);
            this.MainHeader.LogoImage = ((System.Drawing.Image)(resources.GetObject("MainHeader.LogoImage")));
            this.MainHeader.MaximumSize = new System.Drawing.Size(800, 43);
            this.MainHeader.MinimumSize = new System.Drawing.Size(800, 43);
            this.MainHeader.Name = "MainHeader";
            this.MainHeader.Size = new System.Drawing.Size(800, 43);
            this.MainHeader.TabIndex = 0;
            this.MainHeader.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.MainHeader.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(222)))));
            this.MainHeader.TitleText = "TheNoobBot";
            // 
            // AccountList
            // 
            this.AccountList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.AccountList.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(121)))), ((int)(((byte)(121)))));
            this.AccountList.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.AccountList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AccountList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AccountList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(160)))), ((int)(((byte)(229)))));
            this.AccountList.FormattingEnabled = true;
            this.AccountList.HighlightColor = System.Drawing.Color.Gainsboro;
            this.AccountList.ItemHeight = 20;
            this.AccountList.Location = new System.Drawing.Point(14, 72);
            this.AccountList.Name = "AccountList";
            this.AccountList.SelectorBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(106)))), ((int)(((byte)(194)))));
            this.AccountList.SelectorImage = ((System.Drawing.Image)(resources.GetObject("AccountList.SelectorImage")));
            this.AccountList.Size = new System.Drawing.Size(372, 26);
            this.AccountList.TabIndex = 2;
            this.AccountList.SelectedIndexChanged += new System.EventHandler(this.AccountList_SelectedIndexChanged);
            // 
            // tnbRibbonManager1
            // 
            this.tnbRibbonManager1.Location = new System.Drawing.Point(399, 43);
            this.tnbRibbonManager1.Name = "tnbRibbonManager1";
            this.tnbRibbonManager1.Size = new System.Drawing.Size(2, 365);
            this.tnbRibbonManager1.TabIndex = 3;
            // 
            // AccountListLabel
            // 
            this.AccountListLabel.BackColor = System.Drawing.Color.Transparent;
            this.AccountListLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.AccountListLabel.Location = new System.Drawing.Point(14, 45);
            this.AccountListLabel.Name = "AccountListLabel";
            this.AccountListLabel.Size = new System.Drawing.Size(372, 26);
            this.AccountListLabel.TabIndex = 4;
            this.AccountListLabel.Text = "Account List";
            this.AccountListLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AccountAddButton
            // 
            this.AccountAddButton.AutoEllipsis = true;
            this.AccountAddButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.AccountAddButton.ForeColor = System.Drawing.Color.Snow;
            this.AccountAddButton.HooverImage = ((System.Drawing.Image)(resources.GetObject("AccountAddButton.HooverImage")));
            this.AccountAddButton.Image = ((System.Drawing.Image)(resources.GetObject("AccountAddButton.Image")));
            this.AccountAddButton.Location = new System.Drawing.Point(13, 103);
            this.AccountAddButton.Name = "AccountAddButton";
            this.AccountAddButton.Size = new System.Drawing.Size(106, 29);
            this.AccountAddButton.TabIndex = 7;
            this.AccountAddButton.Text = "Add";
            this.AccountAddButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AccountEditButton
            // 
            this.AccountEditButton.AutoEllipsis = true;
            this.AccountEditButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.AccountEditButton.ForeColor = System.Drawing.Color.Snow;
            this.AccountEditButton.HooverImage = ((System.Drawing.Image)(resources.GetObject("AccountEditButton.HooverImage")));
            this.AccountEditButton.Image = ((System.Drawing.Image)(resources.GetObject("AccountEditButton.Image")));
            this.AccountEditButton.Location = new System.Drawing.Point(168, 103);
            this.AccountEditButton.Name = "AccountEditButton";
            this.AccountEditButton.Size = new System.Drawing.Size(106, 29);
            this.AccountEditButton.TabIndex = 8;
            this.AccountEditButton.Text = "Edit";
            this.AccountEditButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AccountDelButton
            // 
            this.AccountDelButton.AutoEllipsis = true;
            this.AccountDelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.AccountDelButton.ForeColor = System.Drawing.Color.Snow;
            this.AccountDelButton.HooverImage = ((System.Drawing.Image)(resources.GetObject("AccountDelButton.HooverImage")));
            this.AccountDelButton.Image = ((System.Drawing.Image)(resources.GetObject("AccountDelButton.Image")));
            this.AccountDelButton.Location = new System.Drawing.Point(280, 103);
            this.AccountDelButton.Name = "AccountDelButton";
            this.AccountDelButton.Size = new System.Drawing.Size(106, 29);
            this.AccountDelButton.TabIndex = 9;
            this.AccountDelButton.Text = "Delete";
            this.AccountDelButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CharactersList
            // 
            this.CharactersList.FormattingEnabled = true;
            this.CharactersList.Location = new System.Drawing.Point(14, 150);
            this.CharactersList.Name = "CharactersList";
            this.CharactersList.ScrollAlwaysVisible = true;
            this.CharactersList.Size = new System.Drawing.Size(372, 95);
            this.CharactersList.TabIndex = 10;
            this.CharactersList.SelectedIndexChanged += new System.EventHandler(this.CharactersList_SelectedIndexChanged);
            this.CharactersList.DoubleClick += new System.EventHandler(this.CharactersList_DoubleClick);
            // 
            // CharacterNote
            // 
            this.CharacterNote.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.CharacterNote.DetectUrls = false;
            this.CharacterNote.Location = new System.Drawing.Point(15, 265);
            this.CharacterNote.Name = "CharacterNote";
            this.CharacterNote.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.CharacterNote.Size = new System.Drawing.Size(370, 93);
            this.CharacterNote.TabIndex = 11;
            this.CharacterNote.Text = "";
            // 
            // tnbButton1
            // 
            this.tnbButton1.AutoEllipsis = true;
            this.tnbButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.tnbButton1.ForeColor = System.Drawing.Color.Snow;
            this.tnbButton1.HooverImage = ((System.Drawing.Image)(resources.GetObject("tnbButton1.HooverImage")));
            this.tnbButton1.Image = ((System.Drawing.Image)(resources.GetObject("tnbButton1.Image")));
            this.tnbButton1.Location = new System.Drawing.Point(282, 366);
            this.tnbButton1.Name = "tnbButton1";
            this.tnbButton1.Size = new System.Drawing.Size(106, 29);
            this.tnbButton1.TabIndex = 14;
            this.tnbButton1.Text = "Delete";
            this.tnbButton1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.tnbButton1.Click += new System.EventHandler(this.tnbButton1_Click);
            // 
            // tnbButton2
            // 
            this.tnbButton2.AutoEllipsis = true;
            this.tnbButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.tnbButton2.ForeColor = System.Drawing.Color.Snow;
            this.tnbButton2.HooverImage = ((System.Drawing.Image)(resources.GetObject("tnbButton2.HooverImage")));
            this.tnbButton2.Image = ((System.Drawing.Image)(resources.GetObject("tnbButton2.Image")));
            this.tnbButton2.Location = new System.Drawing.Point(170, 366);
            this.tnbButton2.Name = "tnbButton2";
            this.tnbButton2.Size = new System.Drawing.Size(106, 29);
            this.tnbButton2.TabIndex = 13;
            this.tnbButton2.Text = "Edit";
            this.tnbButton2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tnbButton3
            // 
            this.tnbButton3.AutoEllipsis = true;
            this.tnbButton3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.tnbButton3.ForeColor = System.Drawing.Color.Snow;
            this.tnbButton3.HooverImage = ((System.Drawing.Image)(resources.GetObject("tnbButton3.HooverImage")));
            this.tnbButton3.Image = ((System.Drawing.Image)(resources.GetObject("tnbButton3.Image")));
            this.tnbButton3.Location = new System.Drawing.Point(15, 366);
            this.tnbButton3.Name = "tnbButton3";
            this.tnbButton3.Size = new System.Drawing.Size(106, 29);
            this.tnbButton3.TabIndex = 12;
            this.tnbButton3.Text = "Add";
            this.tnbButton3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CharacterNoteSaveButton
            // 
            this.CharacterNoteSaveButton.AutoEllipsis = true;
            this.CharacterNoteSaveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.CharacterNoteSaveButton.ForeColor = System.Drawing.Color.Snow;
            this.CharacterNoteSaveButton.HooverImage = ((System.Drawing.Image)(resources.GetObject("CharacterNoteSaveButton.HooverImage")));
            this.CharacterNoteSaveButton.Image = ((System.Drawing.Image)(resources.GetObject("CharacterNoteSaveButton.Image")));
            this.CharacterNoteSaveButton.Location = new System.Drawing.Point(263, 329);
            this.CharacterNoteSaveButton.Name = "CharacterNoteSaveButton";
            this.CharacterNoteSaveButton.Size = new System.Drawing.Size(106, 29);
            this.CharacterNoteSaveButton.TabIndex = 15;
            this.CharacterNoteSaveButton.Text = "Save Note";
            this.CharacterNoteSaveButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CharacterNoteSaveButton.Click += new System.EventHandler(this.CharacterNoteSaveButton_Click);
            // 
            // StartGarrisonFarming
            // 
            this.StartGarrisonFarming.AutoEllipsis = true;
            this.StartGarrisonFarming.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.StartGarrisonFarming.ForeColor = System.Drawing.Color.Snow;
            this.StartGarrisonFarming.HooverImage = ((System.Drawing.Image)(resources.GetObject("StartGarrisonFarming.HooverImage")));
            this.StartGarrisonFarming.Image = ((System.Drawing.Image)(resources.GetObject("StartGarrisonFarming.Image")));
            this.StartGarrisonFarming.Location = new System.Drawing.Point(501, 216);
            this.StartGarrisonFarming.Name = "StartGarrisonFarming";
            this.StartGarrisonFarming.Size = new System.Drawing.Size(200, 29);
            this.StartGarrisonFarming.TabIndex = 16;
            this.StartGarrisonFarming.Text = "Start Auto Garrison Farming";
            this.StartGarrisonFarming.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // InfoLabel
            // 
            this.InfoLabel.BackColor = System.Drawing.Color.Transparent;
            this.InfoLabel.Location = new System.Drawing.Point(407, 265);
            this.InfoLabel.Name = "InfoLabel";
            this.InfoLabel.Size = new System.Drawing.Size(381, 104);
            this.InfoLabel.TabIndex = 17;
            this.InfoLabel.Text = resources.GetString("InfoLabel.Text");
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.InfoLabel);
            this.Controls.Add(this.StartGarrisonFarming);
            this.Controls.Add(this.CharacterNoteSaveButton);
            this.Controls.Add(this.tnbButton1);
            this.Controls.Add(this.tnbButton2);
            this.Controls.Add(this.tnbButton3);
            this.Controls.Add(this.CharacterNote);
            this.Controls.Add(this.CharactersList);
            this.Controls.Add(this.AccountDelButton);
            this.Controls.Add(this.AccountEditButton);
            this.Controls.Add(this.AccountAddButton);
            this.Controls.Add(this.AccountListLabel);
            this.Controls.Add(this.tnbRibbonManager1);
            this.Controls.Add(this.AccountList);
            this.Controls.Add(this.LoggingUC);
            this.Controls.Add(this.MainHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(800, 600);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CloseButton_Click);
            this.ResumeLayout(false);

        }

        #endregion

        private nManager.Helpful.Forms.UserControls.TnbControlMenu MainHeader;
        private nManager.Helpful.Forms.UserControls.LoggingSchedulerUC LoggingUC;
        private nManager.Helpful.Forms.UserControls.TnbComboBox AccountList;
        private nManager.Helpful.Forms.UserControls.TnbRibbonManager tnbRibbonManager1;
        private System.Windows.Forms.Label AccountListLabel;
        private nManager.Helpful.Forms.UserControls.TnbButton AccountAddButton;
        private nManager.Helpful.Forms.UserControls.TnbButton AccountEditButton;
        private nManager.Helpful.Forms.UserControls.TnbButton AccountDelButton;
        private System.Windows.Forms.ListBox CharactersList;
        private System.Windows.Forms.RichTextBox CharacterNote;
        private nManager.Helpful.Forms.UserControls.TnbButton tnbButton1;
        private nManager.Helpful.Forms.UserControls.TnbButton tnbButton2;
        private nManager.Helpful.Forms.UserControls.TnbButton tnbButton3;
        private nManager.Helpful.Forms.UserControls.TnbButton CharacterNoteSaveButton;
        private nManager.Helpful.Forms.UserControls.TnbButton StartGarrisonFarming;
        private System.Windows.Forms.Label InfoLabel;
    }
}

