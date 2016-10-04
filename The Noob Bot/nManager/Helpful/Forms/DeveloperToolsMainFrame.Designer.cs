namespace nManager.Helpful.Forms
{
    partial class DeveloperToolsMainFrame
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeveloperToolsMainFrame));
            this.SearchObjectButton = new System.Windows.Forms.Label();
            this.TargetInfoButton = new System.Windows.Forms.Label();
            this.InformationArea = new System.Windows.Forms.RichTextBox();
            this.GpsButton = new System.Windows.Forms.Label();
            this.CsharpExecButton = new System.Windows.Forms.Label();
            this.TargetInfo2Button = new System.Windows.Forms.Label();
            this.NpcFactionButton = new System.Windows.Forms.Label();
            this.NpcTypeButton = new System.Windows.Forms.Label();
            this.LuaExecButton = new System.Windows.Forms.Label();
            this.TranslationManagerButton = new System.Windows.Forms.Label();
            this.AllObjectsButton = new System.Windows.Forms.Label();
            this.MainHeader = new nManager.Helpful.Forms.UserControls.TnbControlMenu();
            this.SuspendLayout();
            // 
            // SearchObjectButton
            // 
            this.SearchObjectButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.SearchObjectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SearchObjectButton.ForeColor = System.Drawing.Color.White;
            this.SearchObjectButton.Image = global::nManager.Properties.Resources.blackB;
            this.SearchObjectButton.Location = new System.Drawing.Point(413, 558);
            this.SearchObjectButton.Margin = new System.Windows.Forms.Padding(0);
            this.SearchObjectButton.MaximumSize = new System.Drawing.Size(106, 29);
            this.SearchObjectButton.MinimumSize = new System.Drawing.Size(106, 29);
            this.SearchObjectButton.Name = "SearchObjectButton";
            this.SearchObjectButton.Size = new System.Drawing.Size(106, 29);
            this.SearchObjectButton.TabIndex = 14;
            this.SearchObjectButton.Text = "Search Object";
            this.SearchObjectButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.SearchObjectButton.Click += new System.EventHandler(this.SearchObjectButton_Click);
            this.SearchObjectButton.MouseEnter += new System.EventHandler(this.SearchObjectButton_MouseEnter);
            this.SearchObjectButton.MouseLeave += new System.EventHandler(this.SearchObjectButton_MouseLeave);
            // 
            // TargetInfoButton
            // 
            this.TargetInfoButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.TargetInfoButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TargetInfoButton.ForeColor = System.Drawing.Color.White;
            this.TargetInfoButton.Image = global::nManager.Properties.Resources.blackB;
            this.TargetInfoButton.Location = new System.Drawing.Point(302, 517);
            this.TargetInfoButton.Margin = new System.Windows.Forms.Padding(0);
            this.TargetInfoButton.MaximumSize = new System.Drawing.Size(106, 29);
            this.TargetInfoButton.MinimumSize = new System.Drawing.Size(106, 29);
            this.TargetInfoButton.Name = "TargetInfoButton";
            this.TargetInfoButton.Size = new System.Drawing.Size(106, 29);
            this.TargetInfoButton.TabIndex = 15;
            this.TargetInfoButton.Text = "Target\'s information";
            this.TargetInfoButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.TargetInfoButton.Click += new System.EventHandler(this.TargetInfoButton_Click);
            this.TargetInfoButton.MouseEnter += new System.EventHandler(this.TargetInfoButton_MouseEnter);
            this.TargetInfoButton.MouseLeave += new System.EventHandler(this.TargetInfoButton_MouseLeave);
            // 
            // InformationArea
            // 
            this.InformationArea.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.InformationArea.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.InformationArea.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InformationArea.Location = new System.Drawing.Point(35, 69);
            this.InformationArea.Margin = new System.Windows.Forms.Padding(0);
            this.InformationArea.MaximumSize = new System.Drawing.Size(730, 435);
            this.InformationArea.MinimumSize = new System.Drawing.Size(730, 435);
            this.InformationArea.Multiline = true;
            this.InformationArea.Name = "InformationArea";
            this.InformationArea.Size = new System.Drawing.Size(730, 435);
            this.InformationArea.TabIndex = 21;
            // 
            // GpsButton
            // 
            this.GpsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.GpsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GpsButton.ForeColor = System.Drawing.Color.White;
            this.GpsButton.Image = global::nManager.Properties.Resources.blackB;
            this.GpsButton.Location = new System.Drawing.Point(190, 517);
            this.GpsButton.Margin = new System.Windows.Forms.Padding(0);
            this.GpsButton.MaximumSize = new System.Drawing.Size(106, 29);
            this.GpsButton.MinimumSize = new System.Drawing.Size(106, 29);
            this.GpsButton.Name = "GpsButton";
            this.GpsButton.Size = new System.Drawing.Size(106, 29);
            this.GpsButton.TabIndex = 23;
            this.GpsButton.Text = "GPS infos";
            this.GpsButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.GpsButton.Click += new System.EventHandler(this.GpsButton_Click);
            this.GpsButton.MouseEnter += new System.EventHandler(this.GpsButton_MouseEnter);
            this.GpsButton.MouseLeave += new System.EventHandler(this.GpsButton_MouseLeave);
            // 
            // CsharpExecButton
            // 
            this.CsharpExecButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.CsharpExecButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CsharpExecButton.ForeColor = System.Drawing.Color.White;
            this.CsharpExecButton.Image = global::nManager.Properties.Resources.blueB_150;
            this.CsharpExecButton.Location = new System.Drawing.Point(34, 558);
            this.CsharpExecButton.Margin = new System.Windows.Forms.Padding(0);
            this.CsharpExecButton.MaximumSize = new System.Drawing.Size(150, 29);
            this.CsharpExecButton.MinimumSize = new System.Drawing.Size(150, 29);
            this.CsharpExecButton.Name = "CsharpExecButton";
            this.CsharpExecButton.Size = new System.Drawing.Size(150, 29);
            this.CsharpExecButton.TabIndex = 24;
            this.CsharpExecButton.Text = "Execute C# code";
            this.CsharpExecButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CsharpExecButton.Click += new System.EventHandler(this.CsharpExecButton_Click);
            this.CsharpExecButton.MouseEnter += new System.EventHandler(this.CsharpExecButton_MouseEnter);
            this.CsharpExecButton.MouseLeave += new System.EventHandler(this.CsharpExecButton_MouseLeave);
            // 
            // TargetInfo2Button
            // 
            this.TargetInfo2Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.TargetInfo2Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TargetInfo2Button.ForeColor = System.Drawing.Color.White;
            this.TargetInfo2Button.Image = global::nManager.Properties.Resources.blackB;
            this.TargetInfo2Button.Location = new System.Drawing.Point(413, 517);
            this.TargetInfo2Button.Margin = new System.Windows.Forms.Padding(0);
            this.TargetInfo2Button.MaximumSize = new System.Drawing.Size(106, 29);
            this.TargetInfo2Button.MinimumSize = new System.Drawing.Size(106, 29);
            this.TargetInfo2Button.Name = "TargetInfo2Button";
            this.TargetInfo2Button.Size = new System.Drawing.Size(106, 29);
            this.TargetInfo2Button.TabIndex = 25;
            this.TargetInfo2Button.Text = "Target\'s information 2";
            this.TargetInfo2Button.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.TargetInfo2Button.Click += new System.EventHandler(this.TargetInfo2Button_Click);
            this.TargetInfo2Button.MouseEnter += new System.EventHandler(this.TargetInfo2Button_MouseEnter);
            this.TargetInfo2Button.MouseLeave += new System.EventHandler(this.TargetInfo2Button_MouseLeave);
            // 
            // NpcFactionButton
            // 
            this.NpcFactionButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.NpcFactionButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NpcFactionButton.ForeColor = System.Drawing.Color.White;
            this.NpcFactionButton.Image = global::nManager.Properties.Resources.blackB;
            this.NpcFactionButton.Location = new System.Drawing.Point(302, 558);
            this.NpcFactionButton.Margin = new System.Windows.Forms.Padding(0);
            this.NpcFactionButton.MaximumSize = new System.Drawing.Size(106, 29);
            this.NpcFactionButton.MinimumSize = new System.Drawing.Size(106, 29);
            this.NpcFactionButton.Name = "NpcFactionButton";
            this.NpcFactionButton.Size = new System.Drawing.Size(106, 29);
            this.NpcFactionButton.TabIndex = 26;
            this.NpcFactionButton.Text = "NPC Faction List";
            this.NpcFactionButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.NpcFactionButton.Click += new System.EventHandler(this.NpcFactionButton_Click);
            this.NpcFactionButton.MouseEnter += new System.EventHandler(this.NpcFactionButton_MouseEnter);
            this.NpcFactionButton.MouseLeave += new System.EventHandler(this.NpcFactionButton_MouseLeave);
            // 
            // NpcTypeButton
            // 
            this.NpcTypeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.NpcTypeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NpcTypeButton.ForeColor = System.Drawing.Color.White;
            this.NpcTypeButton.Image = global::nManager.Properties.Resources.blackB;
            this.NpcTypeButton.Location = new System.Drawing.Point(190, 558);
            this.NpcTypeButton.Margin = new System.Windows.Forms.Padding(0);
            this.NpcTypeButton.MaximumSize = new System.Drawing.Size(106, 29);
            this.NpcTypeButton.MinimumSize = new System.Drawing.Size(106, 29);
            this.NpcTypeButton.Name = "NpcTypeButton";
            this.NpcTypeButton.Size = new System.Drawing.Size(106, 29);
            this.NpcTypeButton.TabIndex = 27;
            this.NpcTypeButton.Text = "NPC Type List";
            this.NpcTypeButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.NpcTypeButton.Click += new System.EventHandler(this.NpcTypeButton_Click);
            this.NpcTypeButton.MouseEnter += new System.EventHandler(this.NpcTypeButton_MouseEnter);
            this.NpcTypeButton.MouseLeave += new System.EventHandler(this.NpcTypeButton_MouseLeave);
            // 
            // LuaExecButton
            // 
            this.LuaExecButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.LuaExecButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LuaExecButton.ForeColor = System.Drawing.Color.White;
            this.LuaExecButton.Image = global::nManager.Properties.Resources.blueB_150;
            this.LuaExecButton.Location = new System.Drawing.Point(34, 517);
            this.LuaExecButton.Margin = new System.Windows.Forms.Padding(0);
            this.LuaExecButton.MaximumSize = new System.Drawing.Size(150, 29);
            this.LuaExecButton.MinimumSize = new System.Drawing.Size(150, 29);
            this.LuaExecButton.Name = "LuaExecButton";
            this.LuaExecButton.Size = new System.Drawing.Size(150, 29);
            this.LuaExecButton.TabIndex = 29;
            this.LuaExecButton.Text = "Execute LUA code";
            this.LuaExecButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LuaExecButton.Click += new System.EventHandler(this.LuaExecButton_Click);
            this.LuaExecButton.MouseEnter += new System.EventHandler(this.LuaExecButton_MouseEnter);
            this.LuaExecButton.MouseLeave += new System.EventHandler(this.LuaExecButton_MouseLeave);
            // 
            // TranslationManagerButton
            // 
            this.TranslationManagerButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.TranslationManagerButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TranslationManagerButton.ForeColor = System.Drawing.Color.White;
            this.TranslationManagerButton.Image = global::nManager.Properties.Resources.blueB_242;
            this.TranslationManagerButton.Location = new System.Drawing.Point(525, 517);
            this.TranslationManagerButton.Margin = new System.Windows.Forms.Padding(0);
            this.TranslationManagerButton.MaximumSize = new System.Drawing.Size(242, 29);
            this.TranslationManagerButton.MinimumSize = new System.Drawing.Size(242, 29);
            this.TranslationManagerButton.Name = "TranslationManagerButton";
            this.TranslationManagerButton.Size = new System.Drawing.Size(242, 29);
            this.TranslationManagerButton.TabIndex = 32;
            this.TranslationManagerButton.Text = "Open Translation Manager";
            this.TranslationManagerButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.TranslationManagerButton.Click += new System.EventHandler(this.TranslationManagerButton_Click);
            this.TranslationManagerButton.MouseEnter += new System.EventHandler(this.TranslationManagerButton_MouseEnter);
            this.TranslationManagerButton.MouseLeave += new System.EventHandler(this.TranslationManagerButton_MouseLeave);
            // 
            // AllObjectsButton
            // 
            this.AllObjectsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.AllObjectsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AllObjectsButton.ForeColor = System.Drawing.Color.White;
            this.AllObjectsButton.Image = global::nManager.Properties.Resources.blackB_242;
            this.AllObjectsButton.Location = new System.Drawing.Point(525, 558);
            this.AllObjectsButton.Margin = new System.Windows.Forms.Padding(0);
            this.AllObjectsButton.MaximumSize = new System.Drawing.Size(242, 29);
            this.AllObjectsButton.MinimumSize = new System.Drawing.Size(242, 29);
            this.AllObjectsButton.Name = "AllObjectsButton";
            this.AllObjectsButton.Size = new System.Drawing.Size(242, 29);
            this.AllObjectsButton.TabIndex = 33;
            this.AllObjectsButton.Text = "Get all ingame objects information";
            this.AllObjectsButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.AllObjectsButton.Click += new System.EventHandler(this.AllObjectsButton_Click);
            this.AllObjectsButton.MouseEnter += new System.EventHandler(this.AllObjectsButton_MouseEnter);
            this.AllObjectsButton.MouseLeave += new System.EventHandler(this.AllObjectsButton_MouseLeave);
            // 
            // MainHeader
            // 
            this.MainHeader.BackgroundImage = global::nManager.Properties.Resources._800x43_controlbar;
            this.MainHeader.Location = new System.Drawing.Point(0, 0);
            this.MainHeader.LogoImage = ((System.Drawing.Image)(resources.GetObject("MainHeader.LogoImage")));
            this.MainHeader.Name = "MainHeader";
            this.MainHeader.Size = new System.Drawing.Size(800, 43);
            this.MainHeader.TabIndex = 34;
            this.MainHeader.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.MainHeader.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(222)))));
            this.MainHeader.TitleText = "Developer Tools - TheNoobBot - DevVersionRestrict";
            // 
            // DeveloperToolsMainFrame
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.MainHeader);
            this.Controls.Add(this.AllObjectsButton);
            this.Controls.Add(this.TranslationManagerButton);
            this.Controls.Add(this.LuaExecButton);
            this.Controls.Add(this.NpcTypeButton);
            this.Controls.Add(this.NpcFactionButton);
            this.Controls.Add(this.TargetInfo2Button);
            this.Controls.Add(this.CsharpExecButton);
            this.Controls.Add(this.GpsButton);
            this.Controls.Add(this.InformationArea);
            this.Controls.Add(this.TargetInfoButton);
            this.Controls.Add(this.SearchObjectButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(800, 600);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "DeveloperToolsMainFrame";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Translate Tools";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label SearchObjectButton;
        private System.Windows.Forms.Label TargetInfoButton;
        private System.Windows.Forms.RichTextBox InformationArea;
        private System.Windows.Forms.Label GpsButton;
        private System.Windows.Forms.Label CsharpExecButton;
        private System.Windows.Forms.Label TargetInfo2Button;
        private System.Windows.Forms.Label NpcFactionButton;
        private System.Windows.Forms.Label NpcTypeButton;
        private System.Windows.Forms.Label LuaExecButton;
        private System.Windows.Forms.Label TranslationManagerButton;
        private System.Windows.Forms.Label AllObjectsButton;
        private UserControls.TnbControlMenu MainHeader;
    }
}

