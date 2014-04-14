namespace Quester.Profile
{
    partial class SimpleProfileManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimpleProfileManager));
            this.ProfileQuestList = new System.Windows.Forms.ListBox();
            this.ProfileQuestersListLabel = new System.Windows.Forms.Label();
            this.SaveSimpleProfileAs = new System.Windows.Forms.Label();
            this.ProfileQuestListLabel = new System.Windows.Forms.Label();
            this.ProfileQuesterList = new System.Windows.Forms.ListBox();
            this.SaveSimpleProfile = new System.Windows.Forms.Label();
            this.CancelSimpleProfileEdition = new System.Windows.Forms.Label();
            this.AddNewQuestButton = new System.Windows.Forms.Label();
            this.EditSelectedQuestButton = new System.Windows.Forms.Label();
            this.DeleteSelectedQuestButton = new System.Windows.Forms.Label();
            this.DeleteSelectedQuesterButton = new System.Windows.Forms.Label();
            this.EditSelectedQuesterButton = new System.Windows.Forms.Label();
            this.AddNewQuesterButton = new System.Windows.Forms.Label();
            this.MainHeader = new nManager.Helpful.Forms.UserControls.TnbControlMenu();
            this.SuspendLayout();
            // 
            // ProfileQuestList
            // 
            this.ProfileQuestList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.ProfileQuestList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ProfileQuestList.FormattingEnabled = true;
            this.ProfileQuestList.IntegralHeight = false;
            this.ProfileQuestList.Location = new System.Drawing.Point(30, 104);
            this.ProfileQuestList.Margin = new System.Windows.Forms.Padding(0);
            this.ProfileQuestList.MaximumSize = new System.Drawing.Size(302, 128);
            this.ProfileQuestList.MinimumSize = new System.Drawing.Size(302, 128);
            this.ProfileQuestList.Name = "ProfileQuestList";
            this.ProfileQuestList.Size = new System.Drawing.Size(302, 128);
            this.ProfileQuestList.TabIndex = 3;
            this.ProfileQuestList.DoubleClick += new System.EventHandler(this.EditSelectedQuest);
            // 
            // ProfileQuestersListLabel
            // 
            this.ProfileQuestersListLabel.AutoSize = true;
            this.ProfileQuestersListLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.ProfileQuestersListLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProfileQuestersListLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(160)))), ((int)(((byte)(229)))));
            this.ProfileQuestersListLabel.Location = new System.Drawing.Point(32, 258);
            this.ProfileQuestersListLabel.Margin = new System.Windows.Forms.Padding(0);
            this.ProfileQuestersListLabel.MaximumSize = new System.Drawing.Size(234, 29);
            this.ProfileQuestersListLabel.MinimumSize = new System.Drawing.Size(234, 29);
            this.ProfileQuestersListLabel.Name = "ProfileQuestersListLabel";
            this.ProfileQuestersListLabel.Size = new System.Drawing.Size(234, 29);
            this.ProfileQuestersListLabel.TabIndex = 11;
            this.ProfileQuestersListLabel.Text = "Profile\'s Questers List";
            this.ProfileQuestersListLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SaveSimpleProfileAs
            // 
            this.SaveSimpleProfileAs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.SaveSimpleProfileAs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveSimpleProfileAs.ForeColor = System.Drawing.Color.White;
            this.SaveSimpleProfileAs.Image = global::Quester.Properties.Resources.blueB_150;
            this.SaveSimpleProfileAs.Location = new System.Drawing.Point(276, 434);
            this.SaveSimpleProfileAs.Margin = new System.Windows.Forms.Padding(0);
            this.SaveSimpleProfileAs.MaximumSize = new System.Drawing.Size(150, 29);
            this.SaveSimpleProfileAs.MinimumSize = new System.Drawing.Size(150, 29);
            this.SaveSimpleProfileAs.Name = "SaveSimpleProfileAs";
            this.SaveSimpleProfileAs.Size = new System.Drawing.Size(150, 29);
            this.SaveSimpleProfileAs.TabIndex = 13;
            this.SaveSimpleProfileAs.Text = "Save as new && Close";
            this.SaveSimpleProfileAs.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.SaveSimpleProfileAs.Click += new System.EventHandler(this.SaveSimpleProfileAs_Click);
            this.SaveSimpleProfileAs.MouseEnter += new System.EventHandler(this.SaveSimpleProfileAs_MouseEnter);
            this.SaveSimpleProfileAs.MouseLeave += new System.EventHandler(this.SaveSimpleProfileAs_MouseLeave);
            // 
            // ProfileQuestListLabel
            // 
            this.ProfileQuestListLabel.AutoSize = true;
            this.ProfileQuestListLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.ProfileQuestListLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProfileQuestListLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(160)))), ((int)(((byte)(229)))));
            this.ProfileQuestListLabel.Location = new System.Drawing.Point(32, 71);
            this.ProfileQuestListLabel.Margin = new System.Windows.Forms.Padding(0);
            this.ProfileQuestListLabel.MaximumSize = new System.Drawing.Size(234, 29);
            this.ProfileQuestListLabel.MinimumSize = new System.Drawing.Size(234, 29);
            this.ProfileQuestListLabel.Name = "ProfileQuestListLabel";
            this.ProfileQuestListLabel.Size = new System.Drawing.Size(234, 29);
            this.ProfileQuestListLabel.TabIndex = 15;
            this.ProfileQuestListLabel.Text = "Profile\'s Quest List";
            this.ProfileQuestListLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ProfileQuesterList
            // 
            this.ProfileQuesterList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.ProfileQuesterList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ProfileQuesterList.FormattingEnabled = true;
            this.ProfileQuesterList.IntegralHeight = false;
            this.ProfileQuesterList.Location = new System.Drawing.Point(30, 290);
            this.ProfileQuesterList.Margin = new System.Windows.Forms.Padding(0);
            this.ProfileQuesterList.MaximumSize = new System.Drawing.Size(302, 128);
            this.ProfileQuesterList.MinimumSize = new System.Drawing.Size(302, 128);
            this.ProfileQuesterList.Name = "ProfileQuesterList";
            this.ProfileQuesterList.Size = new System.Drawing.Size(302, 128);
            this.ProfileQuesterList.TabIndex = 16;
            this.ProfileQuesterList.DoubleClick += new System.EventHandler(this.EditSelectedQuester);
            // 
            // SaveSimpleProfile
            // 
            this.SaveSimpleProfile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.SaveSimpleProfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveSimpleProfile.ForeColor = System.Drawing.Color.White;
            this.SaveSimpleProfile.Image = global::Quester.Properties.Resources.blueB;
            this.SaveSimpleProfile.Location = new System.Drawing.Point(155, 434);
            this.SaveSimpleProfile.Margin = new System.Windows.Forms.Padding(0);
            this.SaveSimpleProfile.MaximumSize = new System.Drawing.Size(106, 29);
            this.SaveSimpleProfile.MinimumSize = new System.Drawing.Size(106, 29);
            this.SaveSimpleProfile.Name = "SaveSimpleProfile";
            this.SaveSimpleProfile.Size = new System.Drawing.Size(106, 29);
            this.SaveSimpleProfile.TabIndex = 17;
            this.SaveSimpleProfile.Text = "Save && Close";
            this.SaveSimpleProfile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.SaveSimpleProfile.Click += new System.EventHandler(this.SaveSimpleProfile_Click);
            this.SaveSimpleProfile.MouseEnter += new System.EventHandler(this.SaveSimpleProfile_MouseEnter);
            this.SaveSimpleProfile.MouseLeave += new System.EventHandler(this.SaveSimpleProfile_MouseLeave);
            // 
            // CancelSimpleProfileEdition
            // 
            this.CancelSimpleProfileEdition.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.CancelSimpleProfileEdition.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CancelSimpleProfileEdition.ForeColor = System.Drawing.Color.White;
            this.CancelSimpleProfileEdition.Image = global::Quester.Properties.Resources.blackB;
            this.CancelSimpleProfileEdition.Location = new System.Drawing.Point(441, 434);
            this.CancelSimpleProfileEdition.Margin = new System.Windows.Forms.Padding(0);
            this.CancelSimpleProfileEdition.MaximumSize = new System.Drawing.Size(106, 29);
            this.CancelSimpleProfileEdition.MinimumSize = new System.Drawing.Size(106, 29);
            this.CancelSimpleProfileEdition.Name = "CancelSimpleProfileEdition";
            this.CancelSimpleProfileEdition.Size = new System.Drawing.Size(106, 29);
            this.CancelSimpleProfileEdition.TabIndex = 18;
            this.CancelSimpleProfileEdition.Text = "Cancel && Close";
            this.CancelSimpleProfileEdition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CancelSimpleProfileEdition.Click += new System.EventHandler(this.CancelSimpleProfileEdition_Click);
            this.CancelSimpleProfileEdition.MouseEnter += new System.EventHandler(this.CancelSimpleProfileEdition_MouseEnter);
            this.CancelSimpleProfileEdition.MouseLeave += new System.EventHandler(this.CancelSimpleProfileEdition_MouseLeave);
            // 
            // AddNewQuestButton
            // 
            this.AddNewQuestButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.AddNewQuestButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddNewQuestButton.ForeColor = System.Drawing.Color.White;
            this.AddNewQuestButton.Image = global::Quester.Properties.Resources.blueB_200;
            this.AddNewQuestButton.Location = new System.Drawing.Point(347, 103);
            this.AddNewQuestButton.Margin = new System.Windows.Forms.Padding(0);
            this.AddNewQuestButton.MaximumSize = new System.Drawing.Size(200, 29);
            this.AddNewQuestButton.MinimumSize = new System.Drawing.Size(200, 29);
            this.AddNewQuestButton.Name = "AddNewQuestButton";
            this.AddNewQuestButton.Size = new System.Drawing.Size(200, 29);
            this.AddNewQuestButton.TabIndex = 19;
            this.AddNewQuestButton.Text = "Add a new Quest";
            this.AddNewQuestButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.AddNewQuestButton.Click += new System.EventHandler(this.AddNewQuest);
            this.AddNewQuestButton.MouseEnter += new System.EventHandler(this.AddNewQuestButton_MouseEnter);
            this.AddNewQuestButton.MouseLeave += new System.EventHandler(this.AddNewQuestButton_MouseLeave);
            // 
            // EditSelectedQuestButton
            // 
            this.EditSelectedQuestButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.EditSelectedQuestButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EditSelectedQuestButton.ForeColor = System.Drawing.Color.White;
            this.EditSelectedQuestButton.Image = global::Quester.Properties.Resources.blueB_200;
            this.EditSelectedQuestButton.Location = new System.Drawing.Point(347, 144);
            this.EditSelectedQuestButton.Margin = new System.Windows.Forms.Padding(0);
            this.EditSelectedQuestButton.MaximumSize = new System.Drawing.Size(200, 29);
            this.EditSelectedQuestButton.MinimumSize = new System.Drawing.Size(200, 29);
            this.EditSelectedQuestButton.Name = "EditSelectedQuestButton";
            this.EditSelectedQuestButton.Size = new System.Drawing.Size(200, 29);
            this.EditSelectedQuestButton.TabIndex = 20;
            this.EditSelectedQuestButton.Text = "Edit the selected Quest";
            this.EditSelectedQuestButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.EditSelectedQuestButton.Click += new System.EventHandler(this.EditSelectedQuest);
            this.EditSelectedQuestButton.MouseEnter += new System.EventHandler(this.EditSelectedQuestButton_MouseEnter);
            this.EditSelectedQuestButton.MouseLeave += new System.EventHandler(this.EditSelectedQuestButton_MouseLeave);
            // 
            // DeleteSelectedQuestButton
            // 
            this.DeleteSelectedQuestButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.DeleteSelectedQuestButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeleteSelectedQuestButton.ForeColor = System.Drawing.Color.White;
            this.DeleteSelectedQuestButton.Image = global::Quester.Properties.Resources.blackB_200;
            this.DeleteSelectedQuestButton.Location = new System.Drawing.Point(347, 204);
            this.DeleteSelectedQuestButton.Margin = new System.Windows.Forms.Padding(0);
            this.DeleteSelectedQuestButton.MaximumSize = new System.Drawing.Size(200, 29);
            this.DeleteSelectedQuestButton.MinimumSize = new System.Drawing.Size(200, 29);
            this.DeleteSelectedQuestButton.Name = "DeleteSelectedQuestButton";
            this.DeleteSelectedQuestButton.Size = new System.Drawing.Size(200, 29);
            this.DeleteSelectedQuestButton.TabIndex = 21;
            this.DeleteSelectedQuestButton.Text = "Delete the selected Quest";
            this.DeleteSelectedQuestButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.DeleteSelectedQuestButton.Click += new System.EventHandler(this.DeleteSelectedQuest);
            this.DeleteSelectedQuestButton.MouseEnter += new System.EventHandler(this.DeleteSelectedQuestButton_MouseEnter);
            this.DeleteSelectedQuestButton.MouseLeave += new System.EventHandler(this.DeleteSelectedQuestButton_MouseLeave);
            // 
            // DeleteSelectedQuesterButton
            // 
            this.DeleteSelectedQuesterButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.DeleteSelectedQuesterButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeleteSelectedQuesterButton.ForeColor = System.Drawing.Color.White;
            this.DeleteSelectedQuesterButton.Image = global::Quester.Properties.Resources.blackB_200;
            this.DeleteSelectedQuesterButton.Location = new System.Drawing.Point(347, 390);
            this.DeleteSelectedQuesterButton.Margin = new System.Windows.Forms.Padding(0);
            this.DeleteSelectedQuesterButton.MaximumSize = new System.Drawing.Size(200, 29);
            this.DeleteSelectedQuesterButton.MinimumSize = new System.Drawing.Size(200, 29);
            this.DeleteSelectedQuesterButton.Name = "DeleteSelectedQuesterButton";
            this.DeleteSelectedQuesterButton.Size = new System.Drawing.Size(200, 29);
            this.DeleteSelectedQuesterButton.TabIndex = 22;
            this.DeleteSelectedQuesterButton.Text = "Deleted the selected NPC";
            this.DeleteSelectedQuesterButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.DeleteSelectedQuesterButton.Click += new System.EventHandler(this.DeleteSelectedQuester);
            this.DeleteSelectedQuesterButton.MouseEnter += new System.EventHandler(this.DeleteSelectedQuesterButton_MouseEnter);
            this.DeleteSelectedQuesterButton.MouseLeave += new System.EventHandler(this.DeleteSelectedQuesterButton_MouseLeave);
            // 
            // EditSelectedQuesterButton
            // 
            this.EditSelectedQuesterButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.EditSelectedQuesterButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EditSelectedQuesterButton.ForeColor = System.Drawing.Color.White;
            this.EditSelectedQuesterButton.Image = global::Quester.Properties.Resources.blueB_200;
            this.EditSelectedQuesterButton.Location = new System.Drawing.Point(347, 330);
            this.EditSelectedQuesterButton.Margin = new System.Windows.Forms.Padding(0);
            this.EditSelectedQuesterButton.MaximumSize = new System.Drawing.Size(200, 29);
            this.EditSelectedQuesterButton.MinimumSize = new System.Drawing.Size(200, 29);
            this.EditSelectedQuesterButton.Name = "EditSelectedQuesterButton";
            this.EditSelectedQuesterButton.Size = new System.Drawing.Size(200, 29);
            this.EditSelectedQuesterButton.TabIndex = 23;
            this.EditSelectedQuesterButton.Text = "Edit the selected NPC";
            this.EditSelectedQuesterButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.EditSelectedQuesterButton.Click += new System.EventHandler(this.EditSelectedQuester);
            this.EditSelectedQuesterButton.MouseEnter += new System.EventHandler(this.EditSelectedQuesterButton_MouseEnter);
            this.EditSelectedQuesterButton.MouseLeave += new System.EventHandler(this.EditSelectedQuesterButton_MouseLeave);
            // 
            // AddNewQuesterButton
            // 
            this.AddNewQuesterButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.AddNewQuesterButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddNewQuesterButton.ForeColor = System.Drawing.Color.White;
            this.AddNewQuesterButton.Image = global::Quester.Properties.Resources.blueB_200;
            this.AddNewQuesterButton.Location = new System.Drawing.Point(347, 289);
            this.AddNewQuesterButton.Margin = new System.Windows.Forms.Padding(0);
            this.AddNewQuesterButton.MaximumSize = new System.Drawing.Size(200, 29);
            this.AddNewQuesterButton.MinimumSize = new System.Drawing.Size(200, 29);
            this.AddNewQuesterButton.Name = "AddNewQuesterButton";
            this.AddNewQuesterButton.Size = new System.Drawing.Size(200, 29);
            this.AddNewQuesterButton.TabIndex = 24;
            this.AddNewQuesterButton.Text = "Add the target NPC";
            this.AddNewQuesterButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.AddNewQuesterButton.Click += new System.EventHandler(this.AddNewQuester);
            this.AddNewQuesterButton.MouseEnter += new System.EventHandler(this.AddNewQuesterButton_MouseEnter);
            this.AddNewQuesterButton.MouseLeave += new System.EventHandler(this.AddNewQuesterButton_MouseLeave);
            // 
            // MainHeader
            // 
            this.MainHeader.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("MainHeader.BackgroundImage")));
            this.MainHeader.Location = new System.Drawing.Point(0, 0);
            this.MainHeader.LogoImage = ((System.Drawing.Image)(resources.GetObject("MainHeader.LogoImage")));
            this.MainHeader.Name = "MainHeader";
            this.MainHeader.Size = new System.Drawing.Size(575, 43);
            this.MainHeader.TabIndex = 25;
            this.MainHeader.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.MainHeader.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(222)))));
            this.MainHeader.TitleText = "Simple Profile Manager - TheNoobBot DevVersionRestrict";
            // 
            // SimpleProfileManager
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(575, 475);
            this.Controls.Add(this.MainHeader);
            this.Controls.Add(this.AddNewQuesterButton);
            this.Controls.Add(this.EditSelectedQuesterButton);
            this.Controls.Add(this.DeleteSelectedQuesterButton);
            this.Controls.Add(this.DeleteSelectedQuestButton);
            this.Controls.Add(this.EditSelectedQuestButton);
            this.Controls.Add(this.AddNewQuestButton);
            this.Controls.Add(this.CancelSimpleProfileEdition);
            this.Controls.Add(this.SaveSimpleProfile);
            this.Controls.Add(this.ProfileQuesterList);
            this.Controls.Add(this.ProfileQuestListLabel);
            this.Controls.Add(this.SaveSimpleProfileAs);
            this.Controls.Add(this.ProfileQuestersListLabel);
            this.Controls.Add(this.ProfileQuestList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(575, 475);
            this.MinimumSize = new System.Drawing.Size(575, 475);
            this.Name = "SimpleProfileManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SimpleProfileManager";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox ProfileQuestList;
        private System.Windows.Forms.Label ProfileQuestersListLabel;
        private System.Windows.Forms.Label SaveSimpleProfileAs;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label ProfileQuestListLabel;
        private System.Windows.Forms.ListBox ProfileQuesterList;
        private System.Windows.Forms.Label SaveSimpleProfile;
        private System.Windows.Forms.Label CancelSimpleProfileEdition;
        private System.Windows.Forms.Label AddNewQuestButton;
        private System.Windows.Forms.Label EditSelectedQuestButton;
        private System.Windows.Forms.Label DeleteSelectedQuestButton;
        private System.Windows.Forms.Label DeleteSelectedQuesterButton;
        private System.Windows.Forms.Label EditSelectedQuesterButton;
        private System.Windows.Forms.Label AddNewQuesterButton;
        private nManager.Helpful.Forms.UserControls.TnbControlMenu MainHeader;
    }
}

