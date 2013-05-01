namespace The_Noob_Bot
{
    internal partial class Login
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.userNameTb = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.passwordTb = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.listProcessLb = new System.Windows.Forms.ListBox();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.saveCb = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.createB = new DevComponents.DotNetBar.ButtonX();
            this.styleManager1 = new DevComponents.DotNetBar.StyleManager(this.components);
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.langChooseCb = new System.Windows.Forms.ComboBox();
            this.launchBotB = new DevComponents.DotNetBar.ButtonX();
            this.refreshB = new DevComponents.DotNetBar.ButtonX();
            this.requiredFieldValidator1 = new DevComponents.DotNetBar.Validator.RequiredFieldValidator("Your error message here.");
            this.requiredFieldValidator2 = new DevComponents.DotNetBar.Validator.RequiredFieldValidator("Your error message here.");
            this.bar1 = new DevComponents.DotNetBar.Bar();
            this.labelItem1 = new DevComponents.DotNetBar.LabelItem();
            this.buttonLaunchWoWDX9 = new DevComponents.DotNetBar.ButtonItem();
            this.buttonLaunchWoWDX11 = new DevComponents.DotNetBar.ButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).BeginInit();
            this.SuspendLayout();
            // 
            // userNameTb
            // 
            this.userNameTb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(229)))), ((int)(((byte)(230)))));
            // 
            // 
            // 
            this.userNameTb.Border.Class = "TextBoxBorder";
            this.userNameTb.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.userNameTb.ForeColor = System.Drawing.Color.Black;
            this.userNameTb.Location = new System.Drawing.Point(72, 12);
            this.userNameTb.Name = "userNameTb";
            this.userNameTb.Size = new System.Drawing.Size(197, 22);
            this.userNameTb.TabIndex = 0;
            // 
            // passwordTb
            // 
            this.passwordTb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(229)))), ((int)(((byte)(230)))));
            // 
            // 
            // 
            this.passwordTb.Border.Class = "TextBoxBorder";
            this.passwordTb.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.passwordTb.ForeColor = System.Drawing.Color.Black;
            this.passwordTb.Location = new System.Drawing.Point(72, 45);
            this.passwordTb.Name = "passwordTb";
            this.passwordTb.PasswordChar = '*';
            this.passwordTb.Size = new System.Drawing.Size(168, 22);
            this.passwordTb.TabIndex = 1;
            // 
            // listProcessLb
            // 
            this.listProcessLb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(229)))), ((int)(((byte)(230)))));
            this.listProcessLb.ForeColor = System.Drawing.Color.Black;
            this.listProcessLb.FormattingEnabled = true;
            this.listProcessLb.Location = new System.Drawing.Point(1, 82);
            this.listProcessLb.Name = "listProcessLb";
            this.listProcessLb.Size = new System.Drawing.Size(239, 82);
            this.listProcessLb.TabIndex = 3;
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(229)))), ((int)(((byte)(230)))));
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.ForeColor = System.Drawing.Color.Black;
            this.labelX1.Location = new System.Drawing.Point(1, 15);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(67, 19);
            this.labelX1.TabIndex = 4;
            this.labelX1.Text = "User Name:";
            // 
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(229)))), ((int)(((byte)(230)))));
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.ForeColor = System.Drawing.Color.Black;
            this.labelX2.Location = new System.Drawing.Point(1, 48);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(67, 19);
            this.labelX2.TabIndex = 7;
            this.labelX2.Text = "Password:";
            // 
            // saveCb
            // 
            this.saveCb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(229)))), ((int)(((byte)(230)))));
            // 
            // 
            // 
            this.saveCb.BackgroundStyle.Class = "";
            this.saveCb.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.saveCb.ForeColor = System.Drawing.Color.Black;
            this.saveCb.Location = new System.Drawing.Point(246, 37);
            this.saveCb.Name = "saveCb";
            this.saveCb.Size = new System.Drawing.Size(94, 23);
            this.saveCb.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.saveCb.TabIndex = 8;
            this.saveCb.Text = "Save";
            // 
            // createB
            // 
            this.createB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.createB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.createB.Location = new System.Drawing.Point(285, 12);
            this.createB.Name = "createB";
            this.createB.Size = new System.Drawing.Size(55, 22);
            this.createB.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.createB.TabIndex = 9;
            this.createB.Text = "Create";
            this.createB.Click += new System.EventHandler(this.createB_Click);
            // 
            // styleManager1
            // 
            this.styleManager1.ManagerColorTint = System.Drawing.Color.WhiteSmoke;
            this.styleManager1.ManagerStyle = DevComponents.DotNetBar.eStyle.Metro;
            this.styleManager1.MetroColorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(229)))), ((int)(((byte)(230))))), System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(181)))), ((int)(((byte)(22))))));
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX1.Location = new System.Drawing.Point(1, 167);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(183, 17);
            this.buttonX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX1.TabIndex = 10;
            this.buttonX1.Text = "http://www.thenoobbot.com/";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // langChooseCb
            // 
            this.langChooseCb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(229)))), ((int)(((byte)(230)))));
            this.langChooseCb.ForeColor = System.Drawing.Color.Black;
            this.langChooseCb.FormattingEnabled = true;
            this.langChooseCb.Location = new System.Drawing.Point(190, 166);
            this.langChooseCb.Name = "langChooseCb";
            this.langChooseCb.Size = new System.Drawing.Size(150, 21);
            this.langChooseCb.TabIndex = 11;
            // 
            // launchBotB
            // 
            this.launchBotB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.launchBotB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.launchBotB.Image = global::The_Noob_Bot.Properties.Resources.Play;
            this.launchBotB.ImageFixedSize = new System.Drawing.Size(30, 30);
            this.launchBotB.Location = new System.Drawing.Point(246, 82);
            this.launchBotB.Name = "launchBotB";
            this.launchBotB.Size = new System.Drawing.Size(94, 38);
            this.launchBotB.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.launchBotB.TabIndex = 6;
            this.launchBotB.Text = "Launch Bot";
            this.launchBotB.Click += new System.EventHandler(this.launchBotB_Click);
            // 
            // refreshB
            // 
            this.refreshB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.refreshB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.refreshB.Image = global::The_Noob_Bot.Properties.Resources.Refresh;
            this.refreshB.ImageFixedSize = new System.Drawing.Size(30, 30);
            this.refreshB.Location = new System.Drawing.Point(246, 126);
            this.refreshB.Margin = new System.Windows.Forms.Padding(2);
            this.refreshB.Name = "refreshB";
            this.refreshB.Size = new System.Drawing.Size(94, 38);
            this.refreshB.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.refreshB.TabIndex = 5;
            this.refreshB.Text = "Refresh";
            this.refreshB.Click += new System.EventHandler(this.refreshB_Click);
            // 
            // requiredFieldValidator1
            // 
            this.requiredFieldValidator1.ErrorMessage = "Your error message here.";
            this.requiredFieldValidator1.HighlightColor = DevComponents.DotNetBar.Validator.eHighlightColor.Red;
            // 
            // requiredFieldValidator2
            // 
            this.requiredFieldValidator2.ErrorMessage = "Your error message here.";
            this.requiredFieldValidator2.HighlightColor = DevComponents.DotNetBar.Validator.eHighlightColor.Red;
            // 
            // bar1
            // 
            this.bar1.AntiAlias = true;
            this.bar1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.bar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.labelItem1,
            this.buttonLaunchWoWDX9,
            this.buttonLaunchWoWDX11});
            this.bar1.Location = new System.Drawing.Point(246, 60);
            this.bar1.Name = "bar1";
            this.bar1.Size = new System.Drawing.Size(94, 19);
            this.bar1.Stretch = true;
            this.bar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.bar1.TabIndex = 12;
            this.bar1.TabStop = false;
            this.bar1.Text = "Launch Wow";
            // 
            // labelItem1
            // 
            this.labelItem1.Name = "labelItem1";
            this.labelItem1.Text = "Launch Game";
            // 
            // buttonLaunchWoWDX9
            // 
            this.buttonLaunchWoWDX9.Name = "buttonLaunchWoWDX9";
            this.buttonLaunchWoWDX9.Text = "With DirectX9";
            this.buttonLaunchWoWDX9.Click += new System.EventHandler(this.buttonLaunchWoWDX9_Click);
            // 
            // buttonLaunchWoWDX11
            // 
            this.buttonLaunchWoWDX11.Name = "buttonLaunchWoWDX11";
            this.buttonLaunchWoWDX11.Text = "With DirectX11";
            this.buttonLaunchWoWDX11.Click += new System.EventHandler(this.buttonLaunchWoWDX11_Click);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 188);
            this.Controls.Add(this.bar1);
            this.Controls.Add(this.langChooseCb);
            this.Controls.Add(this.buttonX1);
            this.Controls.Add(this.createB);
            this.Controls.Add(this.saveCb);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.launchBotB);
            this.Controls.Add(this.refreshB);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.listProcessLb);
            this.Controls.Add(this.passwordTb);
            this.Controls.Add(this.userNameTb);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(358, 226);
            this.MinimumSize = new System.Drawing.Size(358, 226);
            this.Name = "Login";
            this.Text = "Login";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Login_FormClosed);
            this.Shown += new System.EventHandler(this.Login_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).EndInit();
            this.labelsToolTip = new System.Windows.Forms.ToolTip(new System.ComponentModel.Container());
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.TextBoxX userNameTb;
        private DevComponents.DotNetBar.Controls.TextBoxX passwordTb;
        private System.Windows.Forms.ListBox listProcessLb;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX refreshB;
        private DevComponents.DotNetBar.ButtonX launchBotB;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.CheckBoxX saveCb;
        private DevComponents.DotNetBar.ButtonX createB;
        private DevComponents.DotNetBar.StyleManager styleManager1;
        private DevComponents.DotNetBar.ButtonX buttonX1;
        private System.Windows.Forms.ComboBox langChooseCb;
        private DevComponents.DotNetBar.Validator.RequiredFieldValidator requiredFieldValidator2;
        private DevComponents.DotNetBar.Validator.RequiredFieldValidator requiredFieldValidator1;
        private DevComponents.DotNetBar.Bar bar1;
        private DevComponents.DotNetBar.ButtonItem buttonLaunchWoWDX9;
        private DevComponents.DotNetBar.ButtonItem buttonLaunchWoWDX11;
        private DevComponents.DotNetBar.LabelItem labelItem1;

        private System.Windows.Forms.ToolTip labelsToolTip;
    }
}