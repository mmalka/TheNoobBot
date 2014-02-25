namespace Quester.Profile
{
    partial class SimpleProfileManager
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
            this.SaveSimpleProfileAs = new DevComponents.DotNetBar.ButtonX();
            this.ProfileQuestList = new System.Windows.Forms.ListBox();
            this.SaveSimpleProfile = new DevComponents.DotNetBar.ButtonX();
            this.ProfileQuestListLabel = new DevComponents.DotNetBar.LabelX();
            this.CancelSimpleProfileEdition = new DevComponents.DotNetBar.ButtonX();
            this.AddNewQuestButton = new DevComponents.DotNetBar.ButtonX();
            this.EditSelectedQuestButton = new DevComponents.DotNetBar.ButtonX();
            this.DeleteSelectedQuestButton = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // SaveSimpleProfileAs
            // 
            this.SaveSimpleProfileAs.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.SaveSimpleProfileAs.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.SaveSimpleProfileAs.Location = new System.Drawing.Point(12, 148);
            this.SaveSimpleProfileAs.Name = "SaveSimpleProfileAs";
            this.SaveSimpleProfileAs.Size = new System.Drawing.Size(166, 22);
            this.SaveSimpleProfileAs.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.SaveSimpleProfileAs.TabIndex = 0;
            this.SaveSimpleProfileAs.Text = "Save as new && Close";
            this.SaveSimpleProfileAs.Click += new System.EventHandler(this.SaveSimpleProfileAs_Click);
            // 
            // ProfileQuestList
            // 
            this.ProfileQuestList.FormattingEnabled = true;
            this.ProfileQuestList.Location = new System.Drawing.Point(12, 34);
            this.ProfileQuestList.Name = "ProfileQuestList";
            this.ProfileQuestList.Size = new System.Drawing.Size(340, 95);
            this.ProfileQuestList.TabIndex = 1;
            this.ProfileQuestList.DoubleClick += new System.EventHandler(this.EditSelectedQuest);
            // 
            // SaveSimpleProfile
            // 
            this.SaveSimpleProfile.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.SaveSimpleProfile.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.SaveSimpleProfile.Location = new System.Drawing.Point(186, 148);
            this.SaveSimpleProfile.Name = "SaveSimpleProfile";
            this.SaveSimpleProfile.Size = new System.Drawing.Size(166, 22);
            this.SaveSimpleProfile.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.SaveSimpleProfile.TabIndex = 3;
            this.SaveSimpleProfile.Text = "Save && Close";
            this.SaveSimpleProfile.Click += new System.EventHandler(this.SaveSimpleProfile_Click);
            // 
            // ProfileQuestListLabel
            // 
            // 
            // 
            // 
            this.ProfileQuestListLabel.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ProfileQuestListLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.ProfileQuestListLabel.Location = new System.Drawing.Point(12, 12);
            this.ProfileQuestListLabel.Name = "ProfileQuestListLabel";
            this.ProfileQuestListLabel.Size = new System.Drawing.Size(516, 16);
            this.ProfileQuestListLabel.TabIndex = 7;
            this.ProfileQuestListLabel.Text = "Profile\'s Quest List :";
            // 
            // CancelSimpleProfileEdition
            // 
            this.CancelSimpleProfileEdition.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.CancelSimpleProfileEdition.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.CancelSimpleProfileEdition.Location = new System.Drawing.Point(361, 148);
            this.CancelSimpleProfileEdition.Name = "CancelSimpleProfileEdition";
            this.CancelSimpleProfileEdition.Size = new System.Drawing.Size(166, 22);
            this.CancelSimpleProfileEdition.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.CancelSimpleProfileEdition.TabIndex = 10;
            this.CancelSimpleProfileEdition.Text = "Cancel && Close";
            this.CancelSimpleProfileEdition.Click += new System.EventHandler(this.CancelSimpleProfileEdition_Click);
            // 
            // AddNewQuest
            // 
            this.AddNewQuestButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.AddNewQuestButton.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.AddNewQuestButton.Location = new System.Drawing.Point(363, 34);
            this.AddNewQuestButton.Name = "AddNewQuestButton";
            this.AddNewQuestButton.Size = new System.Drawing.Size(166, 22);
            this.AddNewQuestButton.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.AddNewQuestButton.TabIndex = 16;
            this.AddNewQuestButton.Text = "Add a new quest";
            this.AddNewQuestButton.Click += new System.EventHandler(this.AddNewQuest);
            // 
            // EditSelectedQuest
            // 
            this.EditSelectedQuestButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.EditSelectedQuestButton.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.EditSelectedQuestButton.Location = new System.Drawing.Point(363, 62);
            this.EditSelectedQuestButton.Name = "EditSelectedQuest";
            this.EditSelectedQuestButton.Size = new System.Drawing.Size(166, 22);
            this.EditSelectedQuestButton.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.EditSelectedQuestButton.TabIndex = 17;
            this.EditSelectedQuestButton.Text = "Edit the selected quest";
            this.EditSelectedQuestButton.Click += new System.EventHandler(this.EditSelectedQuest);
            // 
            // DeleteSelectedQuest
            // 
            this.DeleteSelectedQuestButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.DeleteSelectedQuestButton.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.DeleteSelectedQuestButton.Location = new System.Drawing.Point(363, 107);
            this.DeleteSelectedQuestButton.Name = "DeleteSelectedQuestButton";
            this.DeleteSelectedQuestButton.Size = new System.Drawing.Size(166, 22);
            this.DeleteSelectedQuestButton.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.DeleteSelectedQuestButton.TabIndex = 18;
            this.DeleteSelectedQuestButton.Text = "Delete the selected quest";
            this.DeleteSelectedQuestButton.Click += new System.EventHandler(this.DeleteSelectedQuest);
            // 
            // SimpleProfileManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(541, 182);
            this.Controls.Add(this.DeleteSelectedQuestButton);
            this.Controls.Add(this.EditSelectedQuestButton);
            this.Controls.Add(this.AddNewQuestButton);
            this.Controls.Add(this.CancelSimpleProfileEdition);
            this.Controls.Add(this.ProfileQuestListLabel);
            this.Controls.Add(this.SaveSimpleProfile);
            this.Controls.Add(this.ProfileQuestList);
            this.Controls.Add(this.SaveSimpleProfileAs);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(557, 220);
            this.MinimumSize = new System.Drawing.Size(557, 220);
            this.Name = "SimpleProfileManager";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Simple Profile Manager";
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX SaveSimpleProfileAs;
        private System.Windows.Forms.ListBox ProfileQuestList;
        private DevComponents.DotNetBar.ButtonX SaveSimpleProfile;
        private DevComponents.DotNetBar.LabelX ProfileQuestListLabel;
        private DevComponents.DotNetBar.ButtonX CancelSimpleProfileEdition;
        private DevComponents.DotNetBar.ButtonX AddNewQuestButton;
        private DevComponents.DotNetBar.ButtonX EditSelectedQuestButton;
        private DevComponents.DotNetBar.ButtonX DeleteSelectedQuestButton;
    }
}