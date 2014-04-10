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
            this.ControlMenu = new System.Windows.Forms.PictureBox();
            this.ReduceButton = new System.Windows.Forms.PictureBox();
            this.CloseButton = new System.Windows.Forms.PictureBox();
            this.SessionList = new System.Windows.Forms.ListBox();
            this.TopLeftLogo = new System.Windows.Forms.PictureBox();
            this.Identifier = new System.Windows.Forms.TextBox();
            this.Password = new System.Windows.Forms.TextBox();
            this.keyLogoPassword = new System.Windows.Forms.PictureBox();
            this.manLogoIdentifier = new System.Windows.Forms.PictureBox();
            this.Remember = new System.Windows.Forms.CheckBox();
            this.Register = new System.Windows.Forms.Label();
            this.LoginFormTitle = new System.Windows.Forms.Label();
            this.LoginButton = new System.Windows.Forms.Label();
            this.RefreshButton = new System.Windows.Forms.Label();
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
            ((System.ComponentModel.ISupportInitialize)(this.ControlMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReduceButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CloseButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TopLeftLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.keyLogoPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.manLogoIdentifier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FormFocusLogin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FormFocusPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EasterEgg)).BeginInit();
            this.SuspendLayout();
            // 
            // ControlMenu
            // 
            this.ControlMenu.BackColor = System.Drawing.Color.Black;
            this.ControlMenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.ControlMenu.ErrorImage = null;
            this.ControlMenu.Image = ((System.Drawing.Image)(resources.GetObject("ControlMenu.Image")));
            this.ControlMenu.InitialImage = null;
            this.ControlMenu.Location = new System.Drawing.Point(0, 0);
            this.ControlMenu.Margin = new System.Windows.Forms.Padding(0);
            this.ControlMenu.MaximumSize = new System.Drawing.Size(575, 43);
            this.ControlMenu.MinimumSize = new System.Drawing.Size(575, 43);
            this.ControlMenu.Name = "ControlMenu";
            this.ControlMenu.Size = new System.Drawing.Size(575, 43);
            this.ControlMenu.TabIndex = 0;
            this.ControlMenu.TabStop = false;
            this.ControlMenu.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseDown);
            this.ControlMenu.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseMove);
            this.ControlMenu.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseUp);
            // 
            // ReduceButton
            // 
            this.ReduceButton.BackColor = System.Drawing.Color.Transparent;
            this.ReduceButton.ErrorImage = null;
            this.ReduceButton.Image = ((System.Drawing.Image)(resources.GetObject("ReduceButton.Image")));
            this.ReduceButton.InitialImage = null;
            this.ReduceButton.Location = new System.Drawing.Point(523, 13);
            this.ReduceButton.Margin = new System.Windows.Forms.Padding(0);
            this.ReduceButton.MaximumSize = new System.Drawing.Size(13, 14);
            this.ReduceButton.MinimumSize = new System.Drawing.Size(13, 14);
            this.ReduceButton.Name = "ReduceButton";
            this.ReduceButton.Size = new System.Drawing.Size(13, 14);
            this.ReduceButton.TabIndex = 1;
            this.ReduceButton.TabStop = false;
            this.ReduceButton.Click += new System.EventHandler(this.ReduceButton_Click);
            this.ReduceButton.MouseEnter += new System.EventHandler(this.ReduceButton_MouseEnter);
            this.ReduceButton.MouseLeave += new System.EventHandler(this.ReduceButton_MouseLeave);
            // 
            // CloseButton
            // 
            this.CloseButton.BackColor = System.Drawing.Color.Transparent;
            this.CloseButton.ErrorImage = null;
            this.CloseButton.Image = ((System.Drawing.Image)(resources.GetObject("CloseButton.Image")));
            this.CloseButton.InitialImage = null;
            this.CloseButton.Location = new System.Drawing.Point(550, 13);
            this.CloseButton.Margin = new System.Windows.Forms.Padding(0);
            this.CloseButton.MaximumSize = new System.Drawing.Size(13, 14);
            this.CloseButton.MinimumSize = new System.Drawing.Size(13, 14);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(13, 14);
            this.CloseButton.TabIndex = 2;
            this.CloseButton.TabStop = false;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            this.CloseButton.MouseEnter += new System.EventHandler(this.CloseButton_MouseEnter);
            this.CloseButton.MouseLeave += new System.EventHandler(this.CloseButton_MouseLeave);
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
            // TopLeftLogo
            // 
            this.TopLeftLogo.ErrorImage = null;
            this.TopLeftLogo.Image = ((System.Drawing.Image)(resources.GetObject("TopLeftLogo.Image")));
            this.TopLeftLogo.InitialImage = null;
            this.TopLeftLogo.Location = new System.Drawing.Point(13, 3);
            this.TopLeftLogo.Margin = new System.Windows.Forms.Padding(0);
            this.TopLeftLogo.Name = "TopLeftLogo";
            this.TopLeftLogo.Size = new System.Drawing.Size(30, 35);
            this.TopLeftLogo.TabIndex = 4;
            this.TopLeftLogo.TabStop = false;
            this.TopLeftLogo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseDown);
            this.TopLeftLogo.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseMove);
            this.TopLeftLogo.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseUp);
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
            // LoginFormTitle
            // 
            this.LoginFormTitle.AutoSize = true;
            this.LoginFormTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.LoginFormTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoginFormTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(222)))));
            this.LoginFormTitle.Location = new System.Drawing.Point(57, 4);
            this.LoginFormTitle.Margin = new System.Windows.Forms.Padding(0);
            this.LoginFormTitle.MaximumSize = new System.Drawing.Size(450, 35);
            this.LoginFormTitle.MinimumSize = new System.Drawing.Size(450, 35);
            this.LoginFormTitle.Name = "LoginFormTitle";
            this.LoginFormTitle.Size = new System.Drawing.Size(450, 35);
            this.LoginFormTitle.TabIndex = 12;
            this.LoginFormTitle.Text = "TheNoobBot";
            this.LoginFormTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LoginFormTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseDown);
            this.LoginFormTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseMove);
            this.LoginFormTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseUp);
            // 
            // LoginButton
            // 
            this.LoginButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoginButton.ForeColor = System.Drawing.Color.White;
            this.LoginButton.Image = global::The_Noob_Bot.Properties.Resources.blueB;
            this.LoginButton.Location = new System.Drawing.Point(274, 184);
            this.LoginButton.Margin = new System.Windows.Forms.Padding(0);
            this.LoginButton.MaximumSize = new System.Drawing.Size(106, 30);
            this.LoginButton.MinimumSize = new System.Drawing.Size(106, 30);
            this.LoginButton.Name = "LoginButton";
            this.LoginButton.Size = new System.Drawing.Size(106, 30);
            this.LoginButton.TabIndex = 13;
            this.LoginButton.Text = "CONNECT";
            this.LoginButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LoginButton.Click += new System.EventHandler(this.LoginButton_Click);
            this.LoginButton.MouseEnter += new System.EventHandler(this.LoginButton_MouseEnter);
            this.LoginButton.MouseLeave += new System.EventHandler(this.LoginButton_MouseLeave);
            // 
            // RefreshButton
            // 
            this.RefreshButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RefreshButton.ForeColor = System.Drawing.Color.White;
            this.RefreshButton.Image = global::The_Noob_Bot.Properties.Resources.blackB;
            this.RefreshButton.Location = new System.Drawing.Point(434, 184);
            this.RefreshButton.Margin = new System.Windows.Forms.Padding(0);
            this.RefreshButton.MaximumSize = new System.Drawing.Size(106, 30);
            this.RefreshButton.MinimumSize = new System.Drawing.Size(106, 30);
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(106, 30);
            this.RefreshButton.TabIndex = 14;
            this.RefreshButton.Text = "REFRESH";
            this.RefreshButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            this.RefreshButton.MouseEnter += new System.EventHandler(this.RefreshButton_MouseEnter);
            this.RefreshButton.MouseLeave += new System.EventHandler(this.RefreshButton_MouseLeave);
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
            // Login
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(575, 270);
            this.Controls.Add(this.EsterEggTrigger3);
            this.Controls.Add(this.EsterEggTrigger2);
            this.Controls.Add(this.EsterEggTrigger1);
            this.Controls.Add(this.LangSelection);
            this.Controls.Add(this.ForumLink);
            this.Controls.Add(this.WebsiteLink);
            this.Controls.Add(this.RefreshButton);
            this.Controls.Add(this.LoginButton);
            this.Controls.Add(this.LoginFormTitle);
            this.Controls.Add(this.Register);
            this.Controls.Add(this.Remember);
            this.Controls.Add(this.manLogoIdentifier);
            this.Controls.Add(this.keyLogoPassword);
            this.Controls.Add(this.Password);
            this.Controls.Add(this.Identifier);
            this.Controls.Add(this.TopLeftLogo);
            this.Controls.Add(this.SessionList);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.ReduceButton);
            this.Controls.Add(this.ControlMenu);
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
            ((System.ComponentModel.ISupportInitialize)(this.ControlMenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReduceButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CloseButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TopLeftLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.keyLogoPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.manLogoIdentifier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FormFocusLogin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FormFocusPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EasterEgg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox ControlMenu;
        private System.Windows.Forms.PictureBox ReduceButton;
        private System.Windows.Forms.PictureBox CloseButton;
        private System.Windows.Forms.ListBox SessionList;
        private System.Windows.Forms.PictureBox TopLeftLogo;
        private System.Windows.Forms.TextBox Identifier;
        private System.Windows.Forms.TextBox Password;
        private System.Windows.Forms.PictureBox keyLogoPassword;
        private System.Windows.Forms.PictureBox manLogoIdentifier;
        private System.Windows.Forms.CheckBox Remember;
        private System.Windows.Forms.Label Register;
        private System.Windows.Forms.Label LoginFormTitle;
        private System.Windows.Forms.Label LoginButton;
        private System.Windows.Forms.Label RefreshButton;
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
    }
}

