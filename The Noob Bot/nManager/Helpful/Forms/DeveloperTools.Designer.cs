namespace nManager.Helpful.Forms
{
    partial class DeveloperTools
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeveloperTools));
            this.getAllInfo = new DevComponents.DotNetBar.ButtonX();
            this.infoTb = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.targetInfoB = new DevComponents.DotNetBar.ButtonX();
            this.myPositionB = new DevComponents.DotNetBar.ButtonX();
            this.npcType = new DevComponents.DotNetBar.ButtonX();
            this.npcFactionListB = new DevComponents.DotNetBar.ButtonX();
            this.addByNameNpcB = new DevComponents.DotNetBar.ButtonX();
            this.nameNpcTb = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.launchLuaB = new DevComponents.DotNetBar.ButtonX();
            this.launchCSharpScriptB = new DevComponents.DotNetBar.ButtonX();
            this.translateToolsB = new DevComponents.DotNetBar.ButtonX();
            this.getTargetInfoAdv = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // getAllInfo
            // 
            this.getAllInfo.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.getAllInfo.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.getAllInfo.Location = new System.Drawing.Point(234, 357);
            this.getAllInfo.Name = "getAllInfo";
            this.getAllInfo.Size = new System.Drawing.Size(278, 52);
            this.getAllInfo.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.getAllInfo.TabIndex = 21;
            this.getAllInfo.Text = "Get informations of all ingame objects";
            this.getAllInfo.Click += new System.EventHandler(this.getAllInfo_Click);
            // 
            // infoTb
            // 
            this.infoTb.BackColor = System.Drawing.Color.WhiteSmoke;
            // 
            // 
            // 
            this.infoTb.Border.Class = "TextBoxBorder";
            this.infoTb.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.infoTb.ForeColor = System.Drawing.Color.Black;
            this.infoTb.Location = new System.Drawing.Point(12, 12);
            this.infoTb.Multiline = true;
            this.infoTb.Name = "infoTb";
            this.infoTb.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.infoTb.Size = new System.Drawing.Size(501, 279);
            this.infoTb.TabIndex = 22;
            // 
            // targetInfoB
            // 
            this.targetInfoB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.targetInfoB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.targetInfoB.Location = new System.Drawing.Point(272, 295);
            this.targetInfoB.Name = "targetInfoB";
            this.targetInfoB.Size = new System.Drawing.Size(75, 56);
            this.targetInfoB.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.targetInfoB.TabIndex = 23;
            this.targetInfoB.Text = "Npc Infos for the target";
            this.targetInfoB.Click += new System.EventHandler(this.targetInfoB_Click);
            // 
            // myPositionB
            // 
            this.myPositionB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.myPositionB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.myPositionB.Location = new System.Drawing.Point(97, 295);
            this.myPositionB.Name = "myPositionB";
            this.myPositionB.Size = new System.Drawing.Size(78, 56);
            this.myPositionB.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.myPositionB.TabIndex = 24;
            this.myPositionB.Text = "Player\'s position";
            this.myPositionB.Click += new System.EventHandler(this.myPositionB_Click);
            // 
            // npcType
            // 
            this.npcType.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.npcType.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.npcType.Location = new System.Drawing.Point(12, 295);
            this.npcType.Name = "npcType";
            this.npcType.Size = new System.Drawing.Size(79, 22);
            this.npcType.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.npcType.TabIndex = 25;
            this.npcType.Text = "Npc type list";
            this.npcType.Click += new System.EventHandler(this.npcType_Click);
            // 
            // npcFactionListB
            // 
            this.npcFactionListB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.npcFactionListB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.npcFactionListB.Location = new System.Drawing.Point(12, 321);
            this.npcFactionListB.Name = "npcFactionListB";
            this.npcFactionListB.Size = new System.Drawing.Size(79, 28);
            this.npcFactionListB.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.npcFactionListB.TabIndex = 26;
            this.npcFactionListB.Text = "Npc faction list";
            this.npcFactionListB.Click += new System.EventHandler(this.npcFactionListB_Click);
            // 
            // addByNameNpcB
            // 
            this.addByNameNpcB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.addByNameNpcB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.addByNameNpcB.Location = new System.Drawing.Point(180, 321);
            this.addByNameNpcB.Name = "addByNameNpcB";
            this.addByNameNpcB.Size = new System.Drawing.Size(86, 30);
            this.addByNameNpcB.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.addByNameNpcB.TabIndex = 28;
            this.addByNameNpcB.Text = "Get infomations by name";
            this.addByNameNpcB.Click += new System.EventHandler(this.addByNameNpcB_Click);
            // 
            // nameNpcTb
            // 
            this.nameNpcTb.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.nameNpcTb.Border.Class = "TextBoxBorder";
            this.nameNpcTb.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.nameNpcTb.ForeColor = System.Drawing.Color.Black;
            this.nameNpcTb.Location = new System.Drawing.Point(180, 295);
            this.nameNpcTb.Name = "nameNpcTb";
            this.nameNpcTb.Size = new System.Drawing.Size(86, 22);
            this.nameNpcTb.TabIndex = 27;
            this.nameNpcTb.Text = "Name";
            // 
            // launchLuaB
            // 
            this.launchLuaB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.launchLuaB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.launchLuaB.Location = new System.Drawing.Point(120, 357);
            this.launchLuaB.Name = "launchLuaB";
            this.launchLuaB.Size = new System.Drawing.Size(108, 52);
            this.launchLuaB.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.launchLuaB.TabIndex = 29;
            this.launchLuaB.Text = "Launch Lua script";
            this.launchLuaB.Click += new System.EventHandler(this.launchLuaB_Click);
            // 
            // launchCSharpScriptB
            // 
            this.launchCSharpScriptB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.launchCSharpScriptB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.launchCSharpScriptB.Location = new System.Drawing.Point(12, 357);
            this.launchCSharpScriptB.Name = "launchCSharpScriptB";
            this.launchCSharpScriptB.Size = new System.Drawing.Size(102, 52);
            this.launchCSharpScriptB.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.launchCSharpScriptB.TabIndex = 30;
            this.launchCSharpScriptB.Text = "Launch C# script";
            this.launchCSharpScriptB.Click += new System.EventHandler(this.launchCSharpScriptB_Click);
            // 
            // translateToolsB
            // 
            this.translateToolsB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.translateToolsB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.translateToolsB.Location = new System.Drawing.Point(434, 295);
            this.translateToolsB.Name = "translateToolsB";
            this.translateToolsB.Size = new System.Drawing.Size(79, 56);
            this.translateToolsB.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.translateToolsB.TabIndex = 31;
            this.translateToolsB.Text = "Translation Manager ";
            this.translateToolsB.Click += new System.EventHandler(this.translateToolsB_Click);
            // 
            // getTargetInfoAdv
            // 
            this.getTargetInfoAdv.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.getTargetInfoAdv.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.getTargetInfoAdv.Location = new System.Drawing.Point(353, 295);
            this.getTargetInfoAdv.Name = "getTargetInfoAdv";
            this.getTargetInfoAdv.Size = new System.Drawing.Size(75, 56);
            this.getTargetInfoAdv.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.getTargetInfoAdv.TabIndex = 32;
            this.getTargetInfoAdv.Text = "Advanced Infos on the Target";
            this.getTargetInfoAdv.Click += new System.EventHandler(this.getTargetInfoAdv_Click);
            // 
            // Developer_Tools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(524, 409);
            this.Controls.Add(this.getTargetInfoAdv);
            this.Controls.Add(this.translateToolsB);
            this.Controls.Add(this.launchCSharpScriptB);
            this.Controls.Add(this.launchLuaB);
            this.Controls.Add(this.addByNameNpcB);
            this.Controls.Add(this.nameNpcTb);
            this.Controls.Add(this.npcFactionListB);
            this.Controls.Add(this.npcType);
            this.Controls.Add(this.myPositionB);
            this.Controls.Add(this.targetInfoB);
            this.Controls.Add(this.infoTb);
            this.Controls.Add(this.getAllInfo);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(540, 447);
            this.MinimumSize = new System.Drawing.Size(540, 447);
            this.Name = "DeveloperTools";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Developer Tools";
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX getAllInfo;
        private DevComponents.DotNetBar.Controls.TextBoxX infoTb;
        private DevComponents.DotNetBar.ButtonX targetInfoB;
        private DevComponents.DotNetBar.ButtonX myPositionB;
        private DevComponents.DotNetBar.ButtonX npcType;
        private DevComponents.DotNetBar.ButtonX npcFactionListB;
        private DevComponents.DotNetBar.ButtonX addByNameNpcB;
        private DevComponents.DotNetBar.Controls.TextBoxX nameNpcTb;
        private DevComponents.DotNetBar.ButtonX launchLuaB;
        private DevComponents.DotNetBar.ButtonX launchCSharpScriptB;
        private DevComponents.DotNetBar.ButtonX translateToolsB;
        private DevComponents.DotNetBar.ButtonX getTargetInfoAdv;
    }
}