using nManager.Helpful.Forms.UserControls;

namespace The_Noob_Bot
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.SessionList = new System.Windows.Forms.ListBox();
            this.Identifier = new System.Windows.Forms.TextBox();
            this.Password = new System.Windows.Forms.TextBox();
            this.keyLogoPassword = new System.Windows.Forms.PictureBox();
            this.manLogoIdentifier = new System.Windows.Forms.PictureBox();
            this.Remember = new System.Windows.Forms.CheckBox();
            this.Register = new System.Windows.Forms.Label();
            this.FormFocusLogin = new System.Windows.Forms.PictureBox();
            this.FormFocusPassword = new System.Windows.Forms.PictureBox();
            this.WebsiteLink = new System.Windows.Forms.Label();
            this.ForumLink = new System.Windows.Forms.Label();
            this.LangSelection = new System.Windows.Forms.ComboBox();
            this.EasterEgg = new System.Windows.Forms.PictureBox();
            this.EsterEggTrigger1 = new System.Windows.Forms.Panel();
            this.EsterEggTrigger2 = new System.Windows.Forms.Panel();
            this.EsterEggTrigger3 = new System.Windows.Forms.Panel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.RefreshButton = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.LoginButton = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.MainHeader = new nManager.Helpful.Forms.UserControls.TnbControlMenu();
            this.LoginMainFormTimer = new System.Windows.Forms.Timer(this.components);
            this.UseKey = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.keyLogoPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.manLogoIdentifier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FormFocusLogin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FormFocusPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EasterEgg)).BeginInit();
            this.SuspendLayout();
            // 
            // SessionList
            // 
            this.SessionList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.SessionList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.SessionList.FormattingEnabled = true;
            this.SessionList.IntegralHeight = false;
            this.SessionList.Location = new System.Drawing.Point(274, 69);
            this.SessionList.Margin = new System.Windows.Forms.Padding(0);
            this.SessionList.MaximumSize = new System.Drawing.Size(266, 99);
            this.SessionList.MinimumSize = new System.Drawing.Size(266, 99);
            this.SessionList.Name = "SessionList";
            this.SessionList.Size = new System.Drawing.Size(266, 99);
            this.SessionList.TabIndex = 3;
            // 
            // Identifier
            // 
            this.Identifier.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.Identifier.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Identifier.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Identifier.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.Identifier.Location = new System.Drawing.Point(60, 74);
            this.Identifier.Margin = new System.Windows.Forms.Padding(0);
            this.Identifier.MaximumSize = new System.Drawing.Size(192, 25);
            this.Identifier.MinimumSize = new System.Drawing.Size(192, 25);
            this.Identifier.Name = "Identifier";
            this.Identifier.Size = new System.Drawing.Size(192, 25);
            this.Identifier.TabIndex = 5;
            this.Identifier.Text = "LOGIN";
            this.Identifier.Enter += new System.EventHandler(this.Identifier_Enter);
            this.Identifier.Leave += new System.EventHandler(this.Identifier_Leave);
            // 
            // Password
            // 
            this.Password.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.Password.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Password.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Password.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.Password.Location = new System.Drawing.Point(60, 138);
            this.Password.Margin = new System.Windows.Forms.Padding(0);
            this.Password.MaximumSize = new System.Drawing.Size(192, 25);
            this.Password.MinimumSize = new System.Drawing.Size(192, 25);
            this.Password.Name = "Password";
            this.Password.PasswordChar = '*';
            this.Password.Size = new System.Drawing.Size(192, 25);
            this.Password.TabIndex = 6;
            this.Password.Text = "Password";
            this.Password.UseSystemPasswordChar = true;
            this.Password.Enter += new System.EventHandler(this.Password_Enter);
            this.Password.Leave += new System.EventHandler(this.Password_Leave);
            // 
            // keyLogoPassword
            // 
            this.keyLogoPassword.Image = ((System.Drawing.Image)(resources.GetObject("keyLogoPassword.Image")));
            this.keyLogoPassword.Location = new System.Drawing.Point(30, 133);
            this.keyLogoPassword.Margin = new System.Windows.Forms.Padding(0);
            this.keyLogoPassword.MaximumSize = new System.Drawing.Size(29, 35);
            this.keyLogoPassword.MinimumSize = new System.Drawing.Size(29, 35);
            this.keyLogoPassword.Name = "keyLogoPassword";
            this.keyLogoPassword.Size = new System.Drawing.Size(29, 35);
            this.keyLogoPassword.TabIndex = 8;
            this.keyLogoPassword.TabStop = false;
            // 
            // manLogoIdentifier
            // 
            this.manLogoIdentifier.Image = ((System.Drawing.Image)(resources.GetObject("manLogoIdentifier.Image")));
            this.manLogoIdentifier.Location = new System.Drawing.Point(30, 69);
            this.manLogoIdentifier.Margin = new System.Windows.Forms.Padding(0);
            this.manLogoIdentifier.MaximumSize = new System.Drawing.Size(29, 35);
            this.manLogoIdentifier.MinimumSize = new System.Drawing.Size(29, 35);
            this.manLogoIdentifier.Name = "manLogoIdentifier";
            this.manLogoIdentifier.Size = new System.Drawing.Size(29, 35);
            this.manLogoIdentifier.TabIndex = 9;
            this.manLogoIdentifier.TabStop = false;
            // 
            // Remember
            // 
            this.Remember.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.Remember.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.Remember.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.Remember.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.Remember.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Remember.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Remember.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.Remember.Location = new System.Drawing.Point(30, 184);
            this.Remember.MaximumSize = new System.Drawing.Size(120, 24);
            this.Remember.MinimumSize = new System.Drawing.Size(120, 24);
            this.Remember.Name = "Remember";
            this.Remember.Size = new System.Drawing.Size(120, 24);
            this.Remember.TabIndex = 10;
            this.Remember.Text = "SAVE";
            this.Remember.UseVisualStyleBackColor = false;
            // 
            // Register
            // 
            this.Register.AutoSize = true;
            this.Register.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.Register.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Register.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(160)))), ((int)(((byte)(229)))));
            this.Register.Location = new System.Drawing.Point(157, 184);
            this.Register.Margin = new System.Windows.Forms.Padding(0);
            this.Register.MaximumSize = new System.Drawing.Size(95, 30);
            this.Register.MinimumSize = new System.Drawing.Size(95, 30);
            this.Register.Name = "Register";
            this.Register.Size = new System.Drawing.Size(95, 30);
            this.Register.TabIndex = 11;
            this.Register.Text = "REGISTER";
            this.Register.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Register.Click += new System.EventHandler(this.Register_Click);
            this.Register.MouseEnter += new System.EventHandler(this.Register_MouseEnter);
            this.Register.MouseLeave += new System.EventHandler(this.Register_MouseLeave);
            // 
            // FormFocusLogin
            // 
            this.FormFocusLogin.ErrorImage = null;
            this.FormFocusLogin.Image = ((System.Drawing.Image)(resources.GetObject("FormFocusLogin.Image")));
            this.FormFocusLogin.InitialImage = null;
            this.FormFocusLogin.Location = new System.Drawing.Point(29, 68);
            this.FormFocusLogin.Margin = new System.Windows.Forms.Padding(0);
            this.FormFocusLogin.MaximumSize = new System.Drawing.Size(225, 37);
            this.FormFocusLogin.MinimumSize = new System.Drawing.Size(225, 37);
            this.FormFocusLogin.Name = "FormFocusLogin";
            this.FormFocusLogin.Size = new System.Drawing.Size(225, 37);
            this.FormFocusLogin.TabIndex = 15;
            this.FormFocusLogin.TabStop = false;
            this.FormFocusLogin.Visible = false;
            // 
            // FormFocusPassword
            // 
            this.FormFocusPassword.ErrorImage = null;
            this.FormFocusPassword.Image = ((System.Drawing.Image)(resources.GetObject("FormFocusPassword.Image")));
            this.FormFocusPassword.InitialImage = null;
            this.FormFocusPassword.Location = new System.Drawing.Point(29, 132);
            this.FormFocusPassword.Margin = new System.Windows.Forms.Padding(0);
            this.FormFocusPassword.MaximumSize = new System.Drawing.Size(225, 37);
            this.FormFocusPassword.MinimumSize = new System.Drawing.Size(225, 37);
            this.FormFocusPassword.Name = "FormFocusPassword";
            this.FormFocusPassword.Size = new System.Drawing.Size(225, 37);
            this.FormFocusPassword.TabIndex = 16;
            this.FormFocusPassword.TabStop = false;
            this.FormFocusPassword.Visible = false;
            // 
            // WebsiteLink
            // 
            this.WebsiteLink.AutoSize = true;
            this.WebsiteLink.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.WebsiteLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WebsiteLink.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(160)))), ((int)(((byte)(229)))));
            this.WebsiteLink.Location = new System.Drawing.Point(445, 251);
            this.WebsiteLink.Margin = new System.Windows.Forms.Padding(0);
            this.WebsiteLink.MaximumSize = new System.Drawing.Size(125, 14);
            this.WebsiteLink.MinimumSize = new System.Drawing.Size(125, 14);
            this.WebsiteLink.Name = "WebsiteLink";
            this.WebsiteLink.Size = new System.Drawing.Size(125, 14);
            this.WebsiteLink.TabIndex = 17;
            this.WebsiteLink.Text = "WEBSITE";
            this.WebsiteLink.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.WebsiteLink.Click += new System.EventHandler(this.WebsiteLink_Click);
            this.WebsiteLink.MouseEnter += new System.EventHandler(this.WebsiteLink_MouseEnter);
            this.WebsiteLink.MouseLeave += new System.EventHandler(this.WebsiteLink_MouseLeave);
            // 
            // ForumLink
            // 
            this.ForumLink.AutoSize = true;
            this.ForumLink.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.ForumLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForumLink.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(160)))), ((int)(((byte)(229)))));
            this.ForumLink.Location = new System.Drawing.Point(291, 251);
            this.ForumLink.Margin = new System.Windows.Forms.Padding(0);
            this.ForumLink.MaximumSize = new System.Drawing.Size(150, 14);
            this.ForumLink.MinimumSize = new System.Drawing.Size(150, 14);
            this.ForumLink.Name = "ForumLink";
            this.ForumLink.Size = new System.Drawing.Size(150, 14);
            this.ForumLink.TabIndex = 18;
            this.ForumLink.Text = "COMMUNITY FORUM";
            this.ForumLink.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ForumLink.Click += new System.EventHandler(this.ForumLink_Click);
            this.ForumLink.MouseEnter += new System.EventHandler(this.ForumLink_MouseEnter);
            this.ForumLink.MouseLeave += new System.EventHandler(this.ForumLink_MouseLeave);
            // 
            // LangSelection
            // 
            this.LangSelection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.LangSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LangSelection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LangSelection.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LangSelection.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(160)))), ((int)(((byte)(229)))));
            this.LangSelection.FormattingEnabled = true;
            this.LangSelection.ItemHeight = 15;
            this.LangSelection.Location = new System.Drawing.Point(30, 244);
            this.LangSelection.Margin = new System.Windows.Forms.Padding(0);
            this.LangSelection.MaximumSize = new System.Drawing.Size(120, 0);
            this.LangSelection.MinimumSize = new System.Drawing.Size(120, 0);
            this.LangSelection.Name = "LangSelection";
            this.LangSelection.Size = new System.Drawing.Size(120, 23);
            this.LangSelection.TabIndex = 20;
            // 
            // EasterEgg
            // 
            this.EasterEgg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.EasterEgg.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("EasterEgg.BackgroundImage")));
            this.EasterEgg.ErrorImage = null;
            this.EasterEgg.InitialImage = null;
            this.EasterEgg.Location = new System.Drawing.Point(1, 43);
            this.EasterEgg.Name = "EasterEgg";
            this.EasterEgg.Size = new System.Drawing.Size(573, 224);
            this.EasterEgg.TabIndex = 21;
            this.EasterEgg.TabStop = false;
            this.EasterEgg.Visible = false;
            // 
            // EsterEggTrigger1
            // 
            this.EsterEggTrigger1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.EsterEggTrigger1.Location = new System.Drawing.Point(550, 225);
            this.EsterEggTrigger1.Name = "EsterEggTrigger1";
            this.EsterEggTrigger1.Size = new System.Drawing.Size(10, 10);
            this.EsterEggTrigger1.TabIndex = 22;
            this.EsterEggTrigger1.MouseEnter += new System.EventHandler(this.EsterEggTrigger_MouseEnter);
            this.EsterEggTrigger1.MouseLeave += new System.EventHandler(this.EsterEggTrigger_MouseLeave);
            // 
            // EsterEggTrigger2
            // 
            this.EsterEggTrigger2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.EsterEggTrigger2.Location = new System.Drawing.Point(222, 119);
            this.EsterEggTrigger2.Name = "EsterEggTrigger2";
            this.EsterEggTrigger2.Size = new System.Drawing.Size(10, 10);
            this.EsterEggTrigger2.TabIndex = 23;
            this.EsterEggTrigger2.MouseEnter += new System.EventHandler(this.EsterEggTrigger_MouseEnter);
            this.EsterEggTrigger2.MouseLeave += new System.EventHandler(this.EsterEggTrigger_MouseLeave);
            // 
            // EsterEggTrigger3
            // 
            this.EsterEggTrigger3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.EsterEggTrigger3.Location = new System.Drawing.Point(189, 244);
            this.EsterEggTrigger3.Name = "EsterEggTrigger3";
            this.EsterEggTrigger3.Size = new System.Drawing.Size(10, 10);
            this.EsterEggTrigger3.TabIndex = 23;
            this.EsterEggTrigger3.MouseEnter += new System.EventHandler(this.EsterEggTrigger_MouseEnter);
            this.EsterEggTrigger3.MouseLeave += new System.EventHandler(this.EsterEggTrigger_MouseLeave);
            // 
            // RefreshButton
            // 
            this.RefreshButton.AutoEllipsis = true;
            this.RefreshButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.RefreshButton.ForeColor = System.Drawing.Color.Snow;
            this.RefreshButton.HooverImage = ((System.Drawing.Image)(resources.GetObject("RefreshButton.HooverImage")));
            this.RefreshButton.Image = ((System.Drawing.Image)(resources.GetObject("RefreshButton.Image")));
            this.RefreshButton.Location = new System.Drawing.Point(434, 184);
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(106, 29);
            this.RefreshButton.TabIndex = 25;
            this.RefreshButton.Text = "REFRESH";
            this.RefreshButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // LoginButton
            // 
            this.LoginButton.AutoEllipsis = true;
            this.LoginButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.LoginButton.ForeColor = System.Drawing.Color.Snow;
            this.LoginButton.HooverImage = global::The_Noob_Bot.Properties.Resources.greenB;
            this.LoginButton.Image = ((System.Drawing.Image)(resources.GetObject("LoginButton.Image")));
            this.LoginButton.Location = new System.Drawing.Point(274, 184);
            this.LoginButton.Name = "LoginButton";
            this.LoginButton.Size = new System.Drawing.Size(106, 29);
            this.LoginButton.TabIndex = 24;
            this.LoginButton.Text = "CONNECT";
            this.LoginButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LoginButton.Click += new System.EventHandler(this.LoginButton_Click);
            // 
            // MainHeader
            // 
            this.MainHeader.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("MainHeader.BackgroundImage")));
            this.MainHeader.Location = new System.Drawing.Point(0, 0);
            this.MainHeader.LogoImage = ((System.Drawing.Image)(resources.GetObject("MainHeader.LogoImage")));
            this.MainHeader.Name = "MainHeader";
            this.MainHeader.Size = new System.Drawing.Size(575, 43);
            this.MainHeader.TabIndex = 26;
            this.MainHeader.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.MainHeader.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(222)))));
            this.MainHeader.TitleText = "Login - TheNoobBot DevVersionRestrict";
            // 
            // LoginMainFormTimer
            // 
            this.LoginMainFormTimer.Interval = 2500;
            this.LoginMainFormTimer.Tick += new System.EventHandler(this.RefreshButton_Click);
            // 
            // UseKey
            // 
            this.UseKey.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.UseKey.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.UseKey.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.UseKey.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.UseKey.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UseKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UseKey.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.UseKey.Location = new System.Drawing.Point(29, 105);
            this.UseKey.MaximumSize = new System.Drawing.Size(120, 24);
            this.UseKey.MinimumSize = new System.Drawing.Size(120, 24);
            this.UseKey.Name = "UseKey";
            this.UseKey.Size = new System.Drawing.Size(120, 24);
            this.UseKey.TabIndex = 27;
            this.UseKey.Text = "Use Key";
            this.UseKey.UseVisualStyleBackColor = false;
            this.UseKey.CheckedChanged += new System.EventHandler(this.UseKey_CheckedChanged);
            // 
            // Login
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(575, 270);
            this.Controls.Add(this.UseKey);
            this.Controls.Add(this.MainHeader);
            this.Controls.Add(this.RefreshButton);
            this.Controls.Add(this.LoginButton);
            this.Controls.Add(this.EsterEggTrigger3);
            this.Controls.Add(this.EsterEggTrigger2);
            this.Controls.Add(this.EsterEggTrigger1);
            this.Controls.Add(this.LangSelection);
            this.Controls.Add(this.ForumLink);
            this.Controls.Add(this.WebsiteLink);
            this.Controls.Add(this.Register);
            this.Controls.Add(this.Remember);
            this.Controls.Add(this.manLogoIdentifier);
            this.Controls.Add(this.keyLogoPassword);
            this.Controls.Add(this.Password);
            this.Controls.Add(this.Identifier);
            this.Controls.Add(this.SessionList);
            this.Controls.Add(this.FormFocusPassword);
            this.Controls.Add(this.FormFocusLogin);
            this.Controls.Add(this.EasterEgg);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(575, 270);
            this.MinimumSize = new System.Drawing.Size(575, 270);
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CloseButton_Click);
            this.Shown += new System.EventHandler(this.MainFormOnLoad);
            ((System.ComponentModel.ISupportInitialize)(this.keyLogoPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.manLogoIdentifier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FormFocusLogin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FormFocusPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EasterEgg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox SessionList;
        private System.Windows.Forms.TextBox Identifier;
        private System.Windows.Forms.TextBox Password;
        private System.Windows.Forms.PictureBox keyLogoPassword;
        private System.Windows.Forms.PictureBox manLogoIdentifier;
        private System.Windows.Forms.CheckBox Remember;
        private System.Windows.Forms.Label Register;
        private System.Windows.Forms.PictureBox FormFocusLogin;
        private System.Windows.Forms.PictureBox FormFocusPassword;
        private System.Windows.Forms.Label WebsiteLink;
        private System.Windows.Forms.Label ForumLink;
        private System.Windows.Forms.ComboBox LangSelection;
        private System.Windows.Forms.PictureBox EasterEgg;
        private System.Windows.Forms.Panel EsterEggTrigger1;
        private System.Windows.Forms.Panel EsterEggTrigger2;
        private System.Windows.Forms.Panel EsterEggTrigger3;
        private System.Windows.Forms.ToolTip toolTip;
        private TnbButton LoginButton;
        private TnbButton RefreshButton;
        private TnbControlMenu MainHeader;
        private System.Windows.Forms.Timer LoginMainFormTimer;
        private System.Windows.Forms.CheckBox UseKey;
    }
}

