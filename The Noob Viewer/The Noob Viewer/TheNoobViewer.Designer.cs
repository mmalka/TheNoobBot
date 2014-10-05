namespace TheNoobViewer
{
    partial class TheNoobViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TheNoobViewer));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Azeroth = new System.Windows.Forms.ToolStripMenuItem();
            this.Vashjir = new System.Windows.Forms.ToolStripMenuItem();
            this.Kalimdor = new System.Windows.Forms.ToolStripMenuItem();
            this.Outland = new System.Windows.Forms.ToolStripMenuItem();
            this.Northrend = new System.Windows.Forms.ToolStripMenuItem();
            this.Pandaria = new System.Windows.Forms.ToolStripMenuItem();
            this.TolBarad = new System.Windows.Forms.ToolStripMenuItem();
            this.Deepholm = new System.Windows.Forms.ToolStripMenuItem();
            this.Darkmoon = new System.Windows.Forms.ToolStripMenuItem();
            this.IsleThunder = new System.Windows.Forms.ToolStripMenuItem();
            this.Draenor = new System.Windows.Forms.ToolStripMenuItem();
            this.ZoomCombo = new System.Windows.Forms.ToolStripComboBox();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuWebLink = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusContinent = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.mapToolStripMenuItem,
            this.ZoomCombo,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(519, 27);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 23);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(109, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // mapToolStripMenuItem
            // 
            this.mapToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Azeroth,
            this.Kalimdor,
            this.Outland,
            this.Northrend,
            this.Pandaria,
            this.Draenor,
            this.TolBarad,
            this.Deepholm,
            this.Darkmoon,
            this.IsleThunder});
            this.mapToolStripMenuItem.Name = "mapToolStripMenuItem";
            this.mapToolStripMenuItem.Size = new System.Drawing.Size(43, 23);
            this.mapToolStripMenuItem.Text = "Map";
            // 
            // Azeroth
            // 
            this.Azeroth.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Vashjir});
            this.Azeroth.Name = "Azeroth";
            this.Azeroth.Size = new System.Drawing.Size(164, 22);
            this.Azeroth.Text = "Azeroth";
            this.Azeroth.Click += new System.EventHandler(this.Azeroth_Click);
            // 
            // Vashjir
            // 
            this.Vashjir.Name = "Vashjir";
            this.Vashjir.Size = new System.Drawing.Size(112, 22);
            this.Vashjir.Text = "Vashj\'ir";
            this.Vashjir.Click += new System.EventHandler(this.Vashjir_Click);
            // 
            // Kalimdor
            // 
            this.Kalimdor.Name = "Kalimdor";
            this.Kalimdor.Size = new System.Drawing.Size(164, 22);
            this.Kalimdor.Text = "Kalimdor";
            this.Kalimdor.Click += new System.EventHandler(this.Kalimdor_Click);
            // 
            // Outland
            // 
            this.Outland.Name = "Outland";
            this.Outland.Size = new System.Drawing.Size(164, 22);
            this.Outland.Text = "Outland";
            this.Outland.Click += new System.EventHandler(this.Outland_Click);
            // 
            // Northrend
            // 
            this.Northrend.Name = "Northrend";
            this.Northrend.Size = new System.Drawing.Size(164, 22);
            this.Northrend.Text = "Northrend";
            this.Northrend.Click += new System.EventHandler(this.Northrend_Click);
            // 
            // Pandaria
            // 
            this.Pandaria.Name = "Pandaria";
            this.Pandaria.Size = new System.Drawing.Size(164, 22);
            this.Pandaria.Text = "Pandaria";
            this.Pandaria.Click += new System.EventHandler(this.Pandaria_Click);
            // 
            // TolBarad
            // 
            this.TolBarad.Name = "TolBarad";
            this.TolBarad.Size = new System.Drawing.Size(164, 22);
            this.TolBarad.Text = "Tol Barad";
            this.TolBarad.Click += new System.EventHandler(this.TolBarad_Click);
            // 
            // Deepholm
            // 
            this.Deepholm.Name = "Deepholm";
            this.Deepholm.Size = new System.Drawing.Size(164, 22);
            this.Deepholm.Text = "Deepholm";
            this.Deepholm.Click += new System.EventHandler(this.Deepholm_Click);
            // 
            // Darkmoon
            // 
            this.Darkmoon.Name = "Darkmoon";
            this.Darkmoon.Size = new System.Drawing.Size(164, 22);
            this.Darkmoon.Text = "Darkmoon Island";
            this.Darkmoon.Click += new System.EventHandler(this.Darkmoon_Click);
            // 
            // IsleThunder
            // 
            this.IsleThunder.Name = "IsleThunder";
            this.IsleThunder.Size = new System.Drawing.Size(164, 22);
            this.IsleThunder.Text = "Isle of Thunder";
            this.IsleThunder.Click += new System.EventHandler(this.IsleThunder_Click);
            // 
            // Draenor
            // 
            this.Draenor.Name = "Draenor";
            this.Draenor.Size = new System.Drawing.Size(164, 22);
            this.Draenor.Text = "Dreanor";
            this.Draenor.Click += new System.EventHandler(this.Draenor_Click);
            // 
            // ZoomCombo
            // 
            this.ZoomCombo.CausesValidation = false;
            this.ZoomCombo.Name = "ZoomCombo";
            this.ZoomCombo.Size = new System.Drawing.Size(90, 23);
            this.ZoomCombo.SelectedIndexChanged += new System.EventHandler(this.ZoomCombo_SelectedIndexChanged);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuWebLink});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 23);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // menuWebLink
            // 
            this.menuWebLink.Name = "menuWebLink";
            this.menuWebLink.Size = new System.Drawing.Size(155, 22);
            this.menuWebLink.Text = "The Noob Boot";
            this.menuWebLink.Click += new System.EventHandler(this.menuWebLink_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 27);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 3, 1, 20);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(519, 411);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(89, 215);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(327, 15);
            this.textBox1.TabIndex = 0;
            this.textBox1.TabStop = false;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusContinent});
            this.statusStrip1.Location = new System.Drawing.Point(0, 441);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(519, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(98, 17);
            this.toolStripStatusLabel1.Text = "Please open a file";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(10, 17);
            this.toolStripStatusLabel2.Text = "|";
            // 
            // toolStripStatusContinent
            // 
            this.toolStripStatusContinent.Name = "toolStripStatusContinent";
            this.toolStripStatusContinent.Size = new System.Drawing.Size(29, 17);
            this.toolStripStatusContinent.Text = "toto";
            // 
            // TheNoobViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(519, 463);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(250, 150);
            this.Name = "TheNoobViewer";
            this.Text = "The Noob Viewer";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripMenuItem mapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Azeroth;
        private System.Windows.Forms.ToolStripMenuItem Kalimdor;
        private System.Windows.Forms.ToolStripMenuItem Outland;
        private System.Windows.Forms.ToolStripMenuItem Northrend;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusContinent;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripMenuItem menuWebLink;
        private System.Windows.Forms.ToolStripComboBox ZoomCombo;
        private System.Windows.Forms.ToolStripMenuItem TolBarad;
        private System.Windows.Forms.ToolStripMenuItem Vashjir;
        private System.Windows.Forms.ToolStripMenuItem Deepholm;
        private System.Windows.Forms.ToolStripMenuItem Darkmoon;
        private System.Windows.Forms.ToolStripMenuItem Pandaria;
        private System.Windows.Forms.ToolStripMenuItem IsleThunder;
        private System.Windows.Forms.ToolStripMenuItem Draenor;
    }
}

