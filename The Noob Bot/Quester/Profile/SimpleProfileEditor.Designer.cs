using System.Windows.Forms;
using System;
using nManager.Helpful;

namespace Quester.Profile
{
    partial class SimpleProfileEditor : System.Windows.Forms.Form
    {

        //Form remplace la méthode Dispose pour nettoyer la liste des composants.
        [System.Diagnostics.DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components != null)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        //Requise par le Concepteur Windows Form

        private System.ComponentModel.IContainer components;
        //REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
        //Elle peut être modifiée à l'aide du Concepteur Windows Form.  
        //Ne la modifiez pas à l'aide de l'éditeur de code.
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimpleProfileEditor));
            this.TreeView = new System.Windows.Forms.TreeView();
            this.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.DeleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.InsertUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.InsertDownToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ButtonLoadXML = new System.Windows.Forms.Button();
            this.PanelNPC = new System.Windows.Forms.Panel();
            this.ButtonNpcImport = new System.Windows.Forms.Button();
            this.CBNpcType = new System.Windows.Forms.ComboBox();
            this.CBNpcFaction = new System.Windows.Forms.ComboBox();
            this.Label6 = new System.Windows.Forms.Label();
            this.TBNpcContinentId = new System.Windows.Forms.TextBox();
            this.Label5 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.ButtonSaveNPC = new System.Windows.Forms.Button();
            this.Label3 = new System.Windows.Forms.Label();
            this.TBNpcPosition = new System.Windows.Forms.TextBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.TBNpcId = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.TBNpcName = new System.Windows.Forms.TextBox();
            this.ButtonNewNPC = new System.Windows.Forms.Button();
            this.PanelSimpleQuest = new System.Windows.Forms.Panel();
            this.TabControl1 = new System.Windows.Forms.TabControl();
            this.TabPageQuest = new System.Windows.Forms.TabPage();
            this.label40 = new System.Windows.Forms.Label();
            this.TBQuestAutoAcceptIDs = new System.Windows.Forms.TextBox();
            this.label39 = new System.Windows.Forms.Label();
            this.TBQuestAutoCompleteIDs = new System.Windows.Forms.TextBox();
            this.label38 = new System.Windows.Forms.Label();
            this.TBQuestNeedQuestNotCompId = new System.Windows.Forms.TextBox();
            this.ContextMenuStripNeedQuest = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItemAddNeedQuestComp = new System.Windows.Forms.ToolStripMenuItem();
            this.Label28 = new System.Windows.Forms.Label();
            this.TBQuestWQ = new System.Windows.Forms.TextBox();
            this.CBQuestWQ = new System.Windows.Forms.CheckBox();
            this.CBQuestAutoAccepted = new System.Windows.Forms.CheckBox();
            this.ButtonQuestAlliance = new System.Windows.Forms.Button();
            this.ButtonQuestHorde = new System.Windows.Forms.Button();
            this.CLBQuestRaceMask = new System.Windows.Forms.CheckedListBox();
            this.CLBQuestClassMask = new System.Windows.Forms.CheckedListBox();
            this.ButtonQuestImportFromGame = new System.Windows.Forms.Button();
            this.ButtonOpenWowHead = new System.Windows.Forms.Button();
            this.TBQuestNeedQuestCompId = new System.Windows.Forms.TextBox();
            this.Label27 = new System.Windows.Forms.Label();
            this.ButtonQuestNew = new System.Windows.Forms.Button();
            this.ButtonQuestSave = new System.Windows.Forms.Button();
            this.Label12 = new System.Windows.Forms.Label();
            this.TBQuestQuestName = new System.Windows.Forms.TextBox();
            this.TBQuestID = new System.Windows.Forms.TextBox();
            this.Label11 = new System.Windows.Forms.Label();
            this.Label19 = new System.Windows.Forms.Label();
            this.CBClassMask = new System.Windows.Forms.ComboBox();
            this.TBQuestTurnInID = new System.Windows.Forms.TextBox();
            this.Label13 = new System.Windows.Forms.Label();
            this.CheckBoxItemPickUp = new System.Windows.Forms.CheckBox();
            this.Label14 = new System.Windows.Forms.Label();
            this.Label18 = new System.Windows.Forms.Label();
            this.TBQuestLevel = new System.Windows.Forms.TextBox();
            this.TBQuestPickUpID = new System.Windows.Forms.TextBox();
            this.Label15 = new System.Windows.Forms.Label();
            this.Label17 = new System.Windows.Forms.Label();
            this.CBRaceMask = new System.Windows.Forms.ComboBox();
            this.TBQuestMaxLvl = new System.Windows.Forms.TextBox();
            this.TBQuestMinLvl = new System.Windows.Forms.TextBox();
            this.Label16 = new System.Windows.Forms.Label();
            this.TabPageObjectives = new System.Windows.Forms.TabPage();
            this.LBObjHotspots = new System.Windows.Forms.ListBox();
            this.CBObjInternalQuestIdManual = new System.Windows.Forms.CheckBox();
            this.Label37 = new System.Windows.Forms.Label();
            this.TBObjInternalIndex = new System.Windows.Forms.TextBox();
            this.ButtonObjDumpIndex = new System.Windows.Forms.Button();
            this.PanelObjAll = new System.Windows.Forms.Panel();
            this.CBObjIsDead = new System.Windows.Forms.CheckBox();
            this.TBObjGossipOption = new System.Windows.Forms.TextBox();
            this.Label35 = new System.Windows.Forms.Label();
            this.Label9 = new System.Windows.Forms.Label();
            this.CBObjCanPullUnitsInFight = new System.Windows.Forms.CheckBox();
            this.ButtonObjImportGPS = new System.Windows.Forms.Button();
            this.TBObjCount = new System.Windows.Forms.TextBox();
            this.ButtonObjImportEntry = new System.Windows.Forms.Button();
            this.TBObjEntry = new System.Windows.Forms.TextBox();
            this.Label10 = new System.Windows.Forms.Label();
            this.TBObjCollectItemID = new System.Windows.Forms.TextBox();
            this.Label20 = new System.Windows.Forms.Label();
            this.TBObjCollectCount = new System.Windows.Forms.TextBox();
            this.Label21 = new System.Windows.Forms.Label();
            this.TBObjUseItemID = new System.Windows.Forms.TextBox();
            this.Label22 = new System.Windows.Forms.Label();
            this.TBObjPosition = new System.Windows.Forms.TextBox();
            this.Label23 = new System.Windows.Forms.Label();
            this.TBObjWaitMs = new System.Windows.Forms.TextBox();
            this.Label24 = new System.Windows.Forms.Label();
            this.TBObjRange = new System.Windows.Forms.TextBox();
            this.Label25 = new System.Windows.Forms.Label();
            this.TBObjUseSpellId = new System.Windows.Forms.TextBox();
            this.Label26 = new System.Windows.Forms.Label();
            this.ButtonObjHotSpots = new System.Windows.Forms.Button();
            this.ButtonObjImportFromGame = new System.Windows.Forms.Button();
            this.CBObjKillMobPickUpItem = new System.Windows.Forms.CheckBox();
            this.CBObjInternalQuestID = new System.Windows.Forms.ComboBox();
            this.CBInternalObj = new System.Windows.Forms.CheckBox();
            this.Label32 = new System.Windows.Forms.Label();
            this.CBObjIgnoreQuestCompleted = new System.Windows.Forms.CheckBox();
            this.Label30 = new System.Windows.Forms.Label();
            this.TBObjQuestName = new System.Windows.Forms.TextBox();
            this.Label29 = new System.Windows.Forms.Label();
            this.TBObjQuestID = new System.Windows.Forms.TextBox();
            this.LabelObjNPCIDorName = new System.Windows.Forms.Label();
            this.TBObjNPCId = new System.Windows.Forms.TextBox();
            this.ButtonObjectiveNew = new System.Windows.Forms.Button();
            this.ButtonObjectiveSave = new System.Windows.Forms.Button();
            this.Label8 = new System.Windows.Forms.Label();
            this.Label7 = new System.Windows.Forms.Label();
            this.CBObjType = new System.Windows.Forms.ComboBox();
            this.PanelObjTaxi = new System.Windows.Forms.Panel();
            this.TBObjFlightWaitMs = new System.Windows.Forms.TextBox();
            this.Label36 = new System.Windows.Forms.Label();
            this.ButtonObjGetXY = new System.Windows.Forms.Button();
            this.TBObjDestinationY = new System.Windows.Forms.TextBox();
            this.TBObjTaxiEntryId = new System.Windows.Forms.TextBox();
            this.Label33 = new System.Windows.Forms.Label();
            this.TBObjDestinationX = new System.Windows.Forms.TextBox();
            this.Label31 = new System.Windows.Forms.Label();
            this.Label34 = new System.Windows.Forms.Label();
            this.ButtonSaveAsXML = new System.Windows.Forms.Button();
            this.ButtonNewXML = new System.Windows.Forms.Button();
            this.CBMainDisplayXML = new System.Windows.Forms.CheckBox();
            this.ButtonSaveXML = new System.Windows.Forms.Button();
            this.tnbControlMenu1 = new nManager.Helpful.Forms.UserControls.TnbControlMenu();
            this.UcXmlRichTextBox1 = new nManager.Helpful.Forms.UserControls.UCXmlRichTextBox();
            this.label42 = new System.Windows.Forms.Label();
            this.TBObjPressKey = new System.Windows.Forms.TextBox();
            this.label43 = new System.Windows.Forms.Label();
            this.TBObjMessage = new System.Windows.Forms.TextBox();
            this.ContextMenuStrip.SuspendLayout();
            this.PanelNPC.SuspendLayout();
            this.PanelSimpleQuest.SuspendLayout();
            this.TabControl1.SuspendLayout();
            this.TabPageQuest.SuspendLayout();
            this.ContextMenuStripNeedQuest.SuspendLayout();
            this.TabPageObjectives.SuspendLayout();
            this.PanelObjAll.SuspendLayout();
            this.PanelObjTaxi.SuspendLayout();
            this.SuspendLayout();
            // 
            // TreeView
            // 
            this.TreeView.ContextMenuStrip = this.ContextMenuStrip;
            this.TreeView.Location = new System.Drawing.Point(9, 50);
            this.TreeView.Margin = new System.Windows.Forms.Padding(2);
            this.TreeView.Name = "TreeView";
            this.TreeView.Size = new System.Drawing.Size(258, 432);
            this.TreeView.TabIndex = 0;
            this.TreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView_AfterSelect);
            this.TreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeView_NodeMouseClick);
            // 
            // ContextMenuStrip
            // 
            this.ContextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DeleteToolStripMenuItem,
            this.InsertUpToolStripMenuItem,
            this.InsertDownToolStripMenuItem1});
            this.ContextMenuStrip.Name = "ContextMenuStrip1";
            this.ContextMenuStrip.Size = new System.Drawing.Size(141, 70);
            // 
            // DeleteToolStripMenuItem
            // 
            this.DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem";
            this.DeleteToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.DeleteToolStripMenuItem.Text = "Delete";
            this.DeleteToolStripMenuItem.Click += new System.EventHandler(this.DeleteToolStripMenuItem_Click);
            // 
            // InsertUpToolStripMenuItem
            // 
            this.InsertUpToolStripMenuItem.Name = "InsertUpToolStripMenuItem";
            this.InsertUpToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.InsertUpToolStripMenuItem.Text = "Insert Above";
            this.InsertUpToolStripMenuItem.Click += new System.EventHandler(this.InsertUpToolStripMenuItem_Click);
            // 
            // InsertDownToolStripMenuItem1
            // 
            this.InsertDownToolStripMenuItem1.Name = "InsertDownToolStripMenuItem1";
            this.InsertDownToolStripMenuItem1.Size = new System.Drawing.Size(140, 22);
            this.InsertDownToolStripMenuItem1.Text = "Insert Below";
            this.InsertDownToolStripMenuItem1.Click += new System.EventHandler(this.InsertUpToolStripMenuItem_Click);
            // 
            // ButtonLoadXML
            // 
            this.ButtonLoadXML.Location = new System.Drawing.Point(275, 50);
            this.ButtonLoadXML.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonLoadXML.Name = "ButtonLoadXML";
            this.ButtonLoadXML.Size = new System.Drawing.Size(56, 20);
            this.ButtonLoadXML.TabIndex = 1;
            this.ButtonLoadXML.Text = "Load Xml";
            this.ButtonLoadXML.UseVisualStyleBackColor = true;
            this.ButtonLoadXML.Click += new System.EventHandler(this.ButtonLoadXML_Click);
            // 
            // PanelNPC
            // 
            this.PanelNPC.Controls.Add(this.ButtonNpcImport);
            this.PanelNPC.Controls.Add(this.CBNpcType);
            this.PanelNPC.Controls.Add(this.CBNpcFaction);
            this.PanelNPC.Controls.Add(this.Label6);
            this.PanelNPC.Controls.Add(this.TBNpcContinentId);
            this.PanelNPC.Controls.Add(this.Label5);
            this.PanelNPC.Controls.Add(this.Label4);
            this.PanelNPC.Controls.Add(this.ButtonSaveNPC);
            this.PanelNPC.Controls.Add(this.Label3);
            this.PanelNPC.Controls.Add(this.TBNpcPosition);
            this.PanelNPC.Controls.Add(this.Label2);
            this.PanelNPC.Controls.Add(this.TBNpcId);
            this.PanelNPC.Controls.Add(this.Label1);
            this.PanelNPC.Controls.Add(this.TBNpcName);
            this.PanelNPC.Controls.Add(this.ButtonNewNPC);
            this.PanelNPC.Location = new System.Drawing.Point(270, 74);
            this.PanelNPC.Margin = new System.Windows.Forms.Padding(2);
            this.PanelNPC.Name = "PanelNPC";
            this.PanelNPC.Size = new System.Drawing.Size(390, 415);
            this.PanelNPC.TabIndex = 0;
            // 
            // ButtonNpcImport
            // 
            this.ButtonNpcImport.Location = new System.Drawing.Point(148, 165);
            this.ButtonNpcImport.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonNpcImport.Name = "ButtonNpcImport";
            this.ButtonNpcImport.Size = new System.Drawing.Size(68, 19);
            this.ButtonNpcImport.TabIndex = 15;
            this.ButtonNpcImport.Text = "Import";
            this.ButtonNpcImport.UseVisualStyleBackColor = true;
            this.ButtonNpcImport.Click += new System.EventHandler(this.ButtonNpcImport_Click);
            // 
            // CBNpcType
            // 
            this.CBNpcType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBNpcType.FormattingEnabled = true;
            this.CBNpcType.Location = new System.Drawing.Point(70, 106);
            this.CBNpcType.Margin = new System.Windows.Forms.Padding(2);
            this.CBNpcType.Name = "CBNpcType";
            this.CBNpcType.Size = new System.Drawing.Size(92, 21);
            this.CBNpcType.TabIndex = 14;
            // 
            // CBNpcFaction
            // 
            this.CBNpcFaction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBNpcFaction.FormattingEnabled = true;
            this.CBNpcFaction.Location = new System.Drawing.Point(70, 81);
            this.CBNpcFaction.Margin = new System.Windows.Forms.Padding(2);
            this.CBNpcFaction.Name = "CBNpcFaction";
            this.CBNpcFaction.Size = new System.Drawing.Size(92, 21);
            this.CBNpcFaction.TabIndex = 13;
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Location = new System.Drawing.Point(2, 130);
            this.Label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(66, 13);
            this.Label6.TabIndex = 12;
            this.Label6.Text = "Continent ID";
            // 
            // TBNpcContinentId
            // 
            this.TBNpcContinentId.Location = new System.Drawing.Point(70, 130);
            this.TBNpcContinentId.Margin = new System.Windows.Forms.Padding(2);
            this.TBNpcContinentId.Name = "TBNpcContinentId";
            this.TBNpcContinentId.Size = new System.Drawing.Size(76, 20);
            this.TBNpcContinentId.TabIndex = 11;
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(2, 106);
            this.Label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(31, 13);
            this.Label5.TabIndex = 10;
            this.Label5.Text = "Type";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(2, 81);
            this.Label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(42, 13);
            this.Label4.TabIndex = 8;
            this.Label4.Text = "Faction";
            // 
            // ButtonSaveNPC
            // 
            this.ButtonSaveNPC.Location = new System.Drawing.Point(76, 165);
            this.ButtonSaveNPC.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonSaveNPC.Name = "ButtonSaveNPC";
            this.ButtonSaveNPC.Size = new System.Drawing.Size(68, 19);
            this.ButtonSaveNPC.TabIndex = 6;
            this.ButtonSaveNPC.Text = "Save NPC";
            this.ButtonSaveNPC.UseVisualStyleBackColor = true;
            this.ButtonSaveNPC.Click += new System.EventHandler(this.ButtonSaveNPC_Click);
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(2, 58);
            this.Label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(44, 13);
            this.Label3.TabIndex = 6;
            this.Label3.Text = "Position";
            // 
            // TBNpcPosition
            // 
            this.TBNpcPosition.Location = new System.Drawing.Point(70, 58);
            this.TBNpcPosition.Margin = new System.Windows.Forms.Padding(2);
            this.TBNpcPosition.Name = "TBNpcPosition";
            this.TBNpcPosition.Size = new System.Drawing.Size(76, 20);
            this.TBNpcPosition.TabIndex = 2;
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(2, 36);
            this.Label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(18, 13);
            this.Label2.TabIndex = 4;
            this.Label2.Text = "ID";
            // 
            // TBNpcId
            // 
            this.TBNpcId.Location = new System.Drawing.Point(70, 36);
            this.TBNpcId.Margin = new System.Windows.Forms.Padding(2);
            this.TBNpcId.Name = "TBNpcId";
            this.TBNpcId.Size = new System.Drawing.Size(76, 20);
            this.TBNpcId.TabIndex = 1;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(2, 12);
            this.Label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(60, 13);
            this.Label1.TabIndex = 2;
            this.Label1.Text = "NPC Name";
            // 
            // TBNpcName
            // 
            this.TBNpcName.Location = new System.Drawing.Point(70, 13);
            this.TBNpcName.Margin = new System.Windows.Forms.Padding(2);
            this.TBNpcName.Name = "TBNpcName";
            this.TBNpcName.Size = new System.Drawing.Size(76, 20);
            this.TBNpcName.TabIndex = 0;
            // 
            // ButtonNewNPC
            // 
            this.ButtonNewNPC.Location = new System.Drawing.Point(4, 165);
            this.ButtonNewNPC.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonNewNPC.Name = "ButtonNewNPC";
            this.ButtonNewNPC.Size = new System.Drawing.Size(68, 19);
            this.ButtonNewNPC.TabIndex = 5;
            this.ButtonNewNPC.Text = "New NPC";
            this.ButtonNewNPC.UseVisualStyleBackColor = true;
            this.ButtonNewNPC.Click += new System.EventHandler(this.ButtonNewNPC_Click);
            // 
            // PanelSimpleQuest
            // 
            this.PanelSimpleQuest.Controls.Add(this.TabControl1);
            this.PanelSimpleQuest.Location = new System.Drawing.Point(270, 74);
            this.PanelSimpleQuest.Margin = new System.Windows.Forms.Padding(2);
            this.PanelSimpleQuest.Name = "PanelSimpleQuest";
            this.PanelSimpleQuest.Size = new System.Drawing.Size(392, 415);
            this.PanelSimpleQuest.TabIndex = 12;
            // 
            // TabControl1
            // 
            this.TabControl1.Controls.Add(this.TabPageQuest);
            this.TabControl1.Controls.Add(this.TabPageObjectives);
            this.TabControl1.Location = new System.Drawing.Point(5, 1);
            this.TabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.TabControl1.Name = "TabControl1";
            this.TabControl1.SelectedIndex = 0;
            this.TabControl1.Size = new System.Drawing.Size(383, 409);
            this.TabControl1.TabIndex = 31;
            // 
            // TabPageQuest
            // 
            this.TabPageQuest.Controls.Add(this.label40);
            this.TabPageQuest.Controls.Add(this.TBQuestAutoAcceptIDs);
            this.TabPageQuest.Controls.Add(this.label39);
            this.TabPageQuest.Controls.Add(this.TBQuestAutoCompleteIDs);
            this.TabPageQuest.Controls.Add(this.label38);
            this.TabPageQuest.Controls.Add(this.TBQuestNeedQuestNotCompId);
            this.TabPageQuest.Controls.Add(this.Label28);
            this.TabPageQuest.Controls.Add(this.TBQuestWQ);
            this.TabPageQuest.Controls.Add(this.CBQuestWQ);
            this.TabPageQuest.Controls.Add(this.CBQuestAutoAccepted);
            this.TabPageQuest.Controls.Add(this.ButtonQuestAlliance);
            this.TabPageQuest.Controls.Add(this.ButtonQuestHorde);
            this.TabPageQuest.Controls.Add(this.CLBQuestRaceMask);
            this.TabPageQuest.Controls.Add(this.CLBQuestClassMask);
            this.TabPageQuest.Controls.Add(this.ButtonQuestImportFromGame);
            this.TabPageQuest.Controls.Add(this.ButtonOpenWowHead);
            this.TabPageQuest.Controls.Add(this.TBQuestNeedQuestCompId);
            this.TabPageQuest.Controls.Add(this.Label27);
            this.TabPageQuest.Controls.Add(this.ButtonQuestNew);
            this.TabPageQuest.Controls.Add(this.ButtonQuestSave);
            this.TabPageQuest.Controls.Add(this.Label12);
            this.TabPageQuest.Controls.Add(this.TBQuestQuestName);
            this.TabPageQuest.Controls.Add(this.TBQuestID);
            this.TabPageQuest.Controls.Add(this.Label11);
            this.TabPageQuest.Controls.Add(this.Label19);
            this.TabPageQuest.Controls.Add(this.CBClassMask);
            this.TabPageQuest.Controls.Add(this.TBQuestTurnInID);
            this.TabPageQuest.Controls.Add(this.Label13);
            this.TabPageQuest.Controls.Add(this.CheckBoxItemPickUp);
            this.TabPageQuest.Controls.Add(this.Label14);
            this.TabPageQuest.Controls.Add(this.Label18);
            this.TabPageQuest.Controls.Add(this.TBQuestLevel);
            this.TabPageQuest.Controls.Add(this.TBQuestPickUpID);
            this.TabPageQuest.Controls.Add(this.Label15);
            this.TabPageQuest.Controls.Add(this.Label17);
            this.TabPageQuest.Controls.Add(this.CBRaceMask);
            this.TabPageQuest.Controls.Add(this.TBQuestMaxLvl);
            this.TabPageQuest.Controls.Add(this.TBQuestMinLvl);
            this.TabPageQuest.Controls.Add(this.Label16);
            this.TabPageQuest.Location = new System.Drawing.Point(4, 22);
            this.TabPageQuest.Margin = new System.Windows.Forms.Padding(2);
            this.TabPageQuest.Name = "TabPageQuest";
            this.TabPageQuest.Padding = new System.Windows.Forms.Padding(2);
            this.TabPageQuest.Size = new System.Drawing.Size(375, 383);
            this.TabPageQuest.TabIndex = 0;
            this.TabPageQuest.Text = "Quest";
            this.TabPageQuest.UseVisualStyleBackColor = true;
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(211, 82);
            this.label40.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(66, 13);
            this.label40.TabIndex = 87;
            this.label40.Text = "Auto Accept";
            // 
            // TBQuestAutoAcceptIDs
            // 
            this.TBQuestAutoAcceptIDs.Location = new System.Drawing.Point(290, 82);
            this.TBQuestAutoAcceptIDs.Margin = new System.Windows.Forms.Padding(2);
            this.TBQuestAutoAcceptIDs.Multiline = true;
            this.TBQuestAutoAcceptIDs.Name = "TBQuestAutoAcceptIDs";
            this.TBQuestAutoAcceptIDs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TBQuestAutoAcceptIDs.Size = new System.Drawing.Size(76, 24);
            this.TBQuestAutoAcceptIDs.TabIndex = 86;
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(211, 56);
            this.label39.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(76, 13);
            this.label39.TabIndex = 85;
            this.label39.Text = "Auto Complete";
            // 
            // TBQuestAutoCompleteIDs
            // 
            this.TBQuestAutoCompleteIDs.Location = new System.Drawing.Point(290, 56);
            this.TBQuestAutoCompleteIDs.Margin = new System.Windows.Forms.Padding(2);
            this.TBQuestAutoCompleteIDs.Multiline = true;
            this.TBQuestAutoCompleteIDs.Name = "TBQuestAutoCompleteIDs";
            this.TBQuestAutoCompleteIDs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TBQuestAutoCompleteIDs.Size = new System.Drawing.Size(76, 24);
            this.TBQuestAutoCompleteIDs.TabIndex = 84;
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(2, 81);
            this.label38.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(126, 13);
            this.label38.TabIndex = 83;
            this.label38.Text = "Need Quest Not Comp Id";
            // 
            // TBQuestNeedQuestNotCompId
            // 
            this.TBQuestNeedQuestNotCompId.ContextMenuStrip = this.ContextMenuStripNeedQuest;
            this.TBQuestNeedQuestNotCompId.Location = new System.Drawing.Point(131, 82);
            this.TBQuestNeedQuestNotCompId.Margin = new System.Windows.Forms.Padding(2);
            this.TBQuestNeedQuestNotCompId.Multiline = true;
            this.TBQuestNeedQuestNotCompId.Name = "TBQuestNeedQuestNotCompId";
            this.TBQuestNeedQuestNotCompId.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TBQuestNeedQuestNotCompId.Size = new System.Drawing.Size(78, 23);
            this.TBQuestNeedQuestNotCompId.TabIndex = 82;
            // 
            // ContextMenuStripNeedQuest
            // 
            this.ContextMenuStripNeedQuest.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ContextMenuStripNeedQuest.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemAddNeedQuestComp});
            this.ContextMenuStripNeedQuest.Name = "ContextMenuStripNeedQuest";
            this.ContextMenuStripNeedQuest.Size = new System.Drawing.Size(193, 26);
            // 
            // ToolStripMenuItemAddNeedQuestComp
            // 
            this.ToolStripMenuItemAddNeedQuestComp.Name = "ToolStripMenuItemAddNeedQuestComp";
            this.ToolStripMenuItemAddNeedQuestComp.Size = new System.Drawing.Size(192, 22);
            this.ToolStripMenuItemAddNeedQuestComp.Text = "Add Previous Quest ID";
            // 
            // Label28
            // 
            this.Label28.AutoSize = true;
            this.Label28.Location = new System.Drawing.Point(148, 332);
            this.Label28.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label28.Name = "Label28";
            this.Label28.Size = new System.Drawing.Size(47, 13);
            this.Label28.TabIndex = 81;
            this.Label28.Text = "WQ Pos";
            // 
            // TBQuestWQ
            // 
            this.TBQuestWQ.Location = new System.Drawing.Point(202, 330);
            this.TBQuestWQ.Margin = new System.Windows.Forms.Padding(2);
            this.TBQuestWQ.Name = "TBQuestWQ";
            this.TBQuestWQ.Size = new System.Drawing.Size(76, 20);
            this.TBQuestWQ.TabIndex = 80;
            // 
            // CBQuestWQ
            // 
            this.CBQuestWQ.AutoSize = true;
            this.CBQuestWQ.Location = new System.Drawing.Point(282, 333);
            this.CBQuestWQ.Margin = new System.Windows.Forms.Padding(2);
            this.CBQuestWQ.Name = "CBQuestWQ";
            this.CBQuestWQ.Size = new System.Drawing.Size(91, 17);
            this.CBQuestWQ.TabIndex = 79;
            this.CBQuestWQ.Text = "World Quest?";
            this.CBQuestWQ.UseVisualStyleBackColor = true;
            this.CBQuestWQ.CheckedChanged += new System.EventHandler(this.CBQuestWQ_CheckedChanged);
            // 
            // CBQuestAutoAccepted
            // 
            this.CBQuestAutoAccepted.AutoSize = true;
            this.CBQuestAutoAccepted.Location = new System.Drawing.Point(213, 35);
            this.CBQuestAutoAccepted.Margin = new System.Windows.Forms.Padding(2);
            this.CBQuestAutoAccepted.Name = "CBQuestAutoAccepted";
            this.CBQuestAutoAccepted.Size = new System.Drawing.Size(94, 17);
            this.CBQuestAutoAccepted.TabIndex = 78;
            this.CBQuestAutoAccepted.Text = "AutoAccepted";
            this.CBQuestAutoAccepted.UseVisualStyleBackColor = true;
            // 
            // ButtonQuestAlliance
            // 
            this.ButtonQuestAlliance.Location = new System.Drawing.Point(9, 228);
            this.ButtonQuestAlliance.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonQuestAlliance.Name = "ButtonQuestAlliance";
            this.ButtonQuestAlliance.Size = new System.Drawing.Size(64, 19);
            this.ButtonQuestAlliance.TabIndex = 77;
            this.ButtonQuestAlliance.Text = "Alliance";
            this.ButtonQuestAlliance.UseVisualStyleBackColor = true;
            this.ButtonQuestAlliance.Click += new System.EventHandler(this.ButtonQuestAlliance_Click);
            // 
            // ButtonQuestHorde
            // 
            this.ButtonQuestHorde.Location = new System.Drawing.Point(9, 206);
            this.ButtonQuestHorde.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonQuestHorde.Name = "ButtonQuestHorde";
            this.ButtonQuestHorde.Size = new System.Drawing.Size(64, 19);
            this.ButtonQuestHorde.TabIndex = 76;
            this.ButtonQuestHorde.Text = "Horde";
            this.ButtonQuestHorde.UseVisualStyleBackColor = true;
            this.ButtonQuestHorde.Click += new System.EventHandler(this.ButtonQuestHorde_Click);
            // 
            // CLBQuestRaceMask
            // 
            this.CLBQuestRaceMask.CheckOnClick = true;
            this.CLBQuestRaceMask.ColumnWidth = 90;
            this.CLBQuestRaceMask.FormattingEnabled = true;
            this.CLBQuestRaceMask.HorizontalScrollbar = true;
            this.CLBQuestRaceMask.Location = new System.Drawing.Point(86, 191);
            this.CLBQuestRaceMask.Margin = new System.Windows.Forms.Padding(2);
            this.CLBQuestRaceMask.MultiColumn = true;
            this.CLBQuestRaceMask.Name = "CLBQuestRaceMask";
            this.CLBQuestRaceMask.ScrollAlwaysVisible = true;
            this.CLBQuestRaceMask.Size = new System.Drawing.Size(280, 79);
            this.CLBQuestRaceMask.TabIndex = 74;
            // 
            // CLBQuestClassMask
            // 
            this.CLBQuestClassMask.CheckOnClick = true;
            this.CLBQuestClassMask.ColumnWidth = 70;
            this.CLBQuestClassMask.FormattingEnabled = true;
            this.CLBQuestClassMask.HorizontalScrollbar = true;
            this.CLBQuestClassMask.Location = new System.Drawing.Point(86, 109);
            this.CLBQuestClassMask.Margin = new System.Windows.Forms.Padding(2);
            this.CLBQuestClassMask.MultiColumn = true;
            this.CLBQuestClassMask.Name = "CLBQuestClassMask";
            this.CLBQuestClassMask.ScrollAlwaysVisible = true;
            this.CLBQuestClassMask.Size = new System.Drawing.Size(280, 64);
            this.CLBQuestClassMask.TabIndex = 72;
            // 
            // ButtonQuestImportFromGame
            // 
            this.ButtonQuestImportFromGame.Location = new System.Drawing.Point(272, 11);
            this.ButtonQuestImportFromGame.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonQuestImportFromGame.Name = "ButtonQuestImportFromGame";
            this.ButtonQuestImportFromGame.Size = new System.Drawing.Size(94, 21);
            this.ButtonQuestImportFromGame.TabIndex = 71;
            this.ButtonQuestImportFromGame.Text = "Import";
            this.ButtonQuestImportFromGame.UseVisualStyleBackColor = true;
            this.ButtonQuestImportFromGame.Click += new System.EventHandler(this.ButtonQuestImportFromGame_Click);
            // 
            // ButtonOpenWowHead
            // 
            this.ButtonOpenWowHead.Location = new System.Drawing.Point(276, 363);
            this.ButtonOpenWowHead.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonOpenWowHead.Name = "ButtonOpenWowHead";
            this.ButtonOpenWowHead.Size = new System.Drawing.Size(94, 19);
            this.ButtonOpenWowHead.TabIndex = 28;
            this.ButtonOpenWowHead.Text = "Open Wowhead";
            this.ButtonOpenWowHead.UseVisualStyleBackColor = true;
            this.ButtonOpenWowHead.Click += new System.EventHandler(this.ButtonOpenWowHead_Click);
            // 
            // TBQuestNeedQuestCompId
            // 
            this.TBQuestNeedQuestCompId.ContextMenuStrip = this.ContextMenuStripNeedQuest;
            this.TBQuestNeedQuestCompId.Location = new System.Drawing.Point(131, 56);
            this.TBQuestNeedQuestCompId.Margin = new System.Windows.Forms.Padding(2);
            this.TBQuestNeedQuestCompId.Multiline = true;
            this.TBQuestNeedQuestCompId.Name = "TBQuestNeedQuestCompId";
            this.TBQuestNeedQuestCompId.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TBQuestNeedQuestCompId.Size = new System.Drawing.Size(78, 23);
            this.TBQuestNeedQuestCompId.TabIndex = 2;
            // 
            // Label27
            // 
            this.Label27.AutoSize = true;
            this.Label27.Location = new System.Drawing.Point(2, 58);
            this.Label27.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label27.Name = "Label27";
            this.Label27.Size = new System.Drawing.Size(106, 13);
            this.Label27.TabIndex = 27;
            this.Label27.Text = "Need Quest Comp Id";
            // 
            // ButtonQuestNew
            // 
            this.ButtonQuestNew.Location = new System.Drawing.Point(69, 363);
            this.ButtonQuestNew.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonQuestNew.Name = "ButtonQuestNew";
            this.ButtonQuestNew.Size = new System.Drawing.Size(56, 19);
            this.ButtonQuestNew.TabIndex = 15;
            this.ButtonQuestNew.Text = "New Quest";
            this.ButtonQuestNew.UseVisualStyleBackColor = true;
            this.ButtonQuestNew.Click += new System.EventHandler(this.ButtonQuestNew_Click);
            // 
            // ButtonQuestSave
            // 
            this.ButtonQuestSave.Location = new System.Drawing.Point(9, 363);
            this.ButtonQuestSave.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonQuestSave.Name = "ButtonQuestSave";
            this.ButtonQuestSave.Size = new System.Drawing.Size(56, 19);
            this.ButtonQuestSave.TabIndex = 14;
            this.ButtonQuestSave.Text = "Save";
            this.ButtonQuestSave.UseVisualStyleBackColor = true;
            this.ButtonQuestSave.Click += new System.EventHandler(this.ButtonQuestSave_Click);
            // 
            // Label12
            // 
            this.Label12.AutoSize = true;
            this.Label12.Location = new System.Drawing.Point(4, 14);
            this.Label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label12.Name = "Label12";
            this.Label12.Size = new System.Drawing.Size(66, 13);
            this.Label12.TabIndex = 2;
            this.Label12.Text = "Quest Name";
            // 
            // TBQuestQuestName
            // 
            this.TBQuestQuestName.Location = new System.Drawing.Point(131, 11);
            this.TBQuestQuestName.Margin = new System.Windows.Forms.Padding(2);
            this.TBQuestQuestName.Name = "TBQuestQuestName";
            this.TBQuestQuestName.Size = new System.Drawing.Size(137, 20);
            this.TBQuestQuestName.TabIndex = 0;
            // 
            // TBQuestID
            // 
            this.TBQuestID.Location = new System.Drawing.Point(131, 34);
            this.TBQuestID.Margin = new System.Windows.Forms.Padding(2);
            this.TBQuestID.Name = "TBQuestID";
            this.TBQuestID.Size = new System.Drawing.Size(78, 20);
            this.TBQuestID.TabIndex = 1;
            // 
            // Label11
            // 
            this.Label11.AutoSize = true;
            this.Label11.Location = new System.Drawing.Point(4, 37);
            this.Label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label11.Name = "Label11";
            this.Label11.Size = new System.Drawing.Size(18, 13);
            this.Label11.TabIndex = 4;
            this.Label11.Text = "ID";
            // 
            // Label19
            // 
            this.Label19.AutoSize = true;
            this.Label19.Location = new System.Drawing.Point(146, 310);
            this.Label19.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label19.Name = "Label19";
            this.Label19.Size = new System.Drawing.Size(52, 13);
            this.Label19.TabIndex = 26;
            this.Label19.Text = "TurnIn ID";
            // 
            // CBClassMask
            // 
            this.CBClassMask.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBClassMask.FormattingEnabled = true;
            this.CBClassMask.Location = new System.Drawing.Point(264, 109);
            this.CBClassMask.Margin = new System.Windows.Forms.Padding(2);
            this.CBClassMask.Name = "CBClassMask";
            this.CBClassMask.Size = new System.Drawing.Size(90, 21);
            this.CBClassMask.TabIndex = 3;
            this.CBClassMask.Visible = false;
            // 
            // TBQuestTurnInID
            // 
            this.TBQuestTurnInID.Location = new System.Drawing.Point(202, 308);
            this.TBQuestTurnInID.Margin = new System.Windows.Forms.Padding(2);
            this.TBQuestTurnInID.Name = "TBQuestTurnInID";
            this.TBQuestTurnInID.Size = new System.Drawing.Size(76, 20);
            this.TBQuestTurnInID.TabIndex = 9;
            // 
            // Label13
            // 
            this.Label13.AutoSize = true;
            this.Label13.Location = new System.Drawing.Point(4, 111);
            this.Label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label13.Name = "Label13";
            this.Label13.Size = new System.Drawing.Size(61, 13);
            this.Label13.TabIndex = 13;
            this.Label13.Text = "Class Mask";
            // 
            // CheckBoxItemPickUp
            // 
            this.CheckBoxItemPickUp.AutoSize = true;
            this.CheckBoxItemPickUp.Location = new System.Drawing.Point(282, 288);
            this.CheckBoxItemPickUp.Margin = new System.Windows.Forms.Padding(2);
            this.CheckBoxItemPickUp.Name = "CheckBoxItemPickUp";
            this.CheckBoxItemPickUp.Size = new System.Drawing.Size(84, 17);
            this.CheckBoxItemPickUp.TabIndex = 10;
            this.CheckBoxItemPickUp.Text = "Item PickUp";
            this.CheckBoxItemPickUp.UseVisualStyleBackColor = true;
            // 
            // Label14
            // 
            this.Label14.AutoSize = true;
            this.Label14.Location = new System.Drawing.Point(4, 191);
            this.Label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label14.Name = "Label14";
            this.Label14.Size = new System.Drawing.Size(62, 13);
            this.Label14.TabIndex = 14;
            this.Label14.Text = "Race Mask";
            // 
            // Label18
            // 
            this.Label18.AutoSize = true;
            this.Label18.Location = new System.Drawing.Point(146, 288);
            this.Label18.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label18.Name = "Label18";
            this.Label18.Size = new System.Drawing.Size(56, 13);
            this.Label18.TabIndex = 23;
            this.Label18.Text = "PickUp ID";
            // 
            // TBQuestLevel
            // 
            this.TBQuestLevel.Location = new System.Drawing.Point(68, 285);
            this.TBQuestLevel.Margin = new System.Windows.Forms.Padding(2);
            this.TBQuestLevel.Name = "TBQuestLevel";
            this.TBQuestLevel.Size = new System.Drawing.Size(76, 20);
            this.TBQuestLevel.TabIndex = 5;
            // 
            // TBQuestPickUpID
            // 
            this.TBQuestPickUpID.Location = new System.Drawing.Point(202, 285);
            this.TBQuestPickUpID.Margin = new System.Windows.Forms.Padding(2);
            this.TBQuestPickUpID.Name = "TBQuestPickUpID";
            this.TBQuestPickUpID.Size = new System.Drawing.Size(76, 20);
            this.TBQuestPickUpID.TabIndex = 8;
            // 
            // Label15
            // 
            this.Label15.AutoSize = true;
            this.Label15.Location = new System.Drawing.Point(6, 286);
            this.Label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label15.Name = "Label15";
            this.Label15.Size = new System.Drawing.Size(64, 13);
            this.Label15.TabIndex = 16;
            this.Label15.Text = "Quest Level";
            // 
            // Label17
            // 
            this.Label17.AutoSize = true;
            this.Label17.Location = new System.Drawing.Point(6, 332);
            this.Label17.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label17.Name = "Label17";
            this.Label17.Size = new System.Drawing.Size(56, 13);
            this.Label17.TabIndex = 21;
            this.Label17.Text = "Max Level";
            // 
            // CBRaceMask
            // 
            this.CBRaceMask.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBRaceMask.FormattingEnabled = true;
            this.CBRaceMask.Location = new System.Drawing.Point(264, 188);
            this.CBRaceMask.Margin = new System.Windows.Forms.Padding(2);
            this.CBRaceMask.Name = "CBRaceMask";
            this.CBRaceMask.Size = new System.Drawing.Size(90, 21);
            this.CBRaceMask.TabIndex = 4;
            this.CBRaceMask.Visible = false;
            // 
            // TBQuestMaxLvl
            // 
            this.TBQuestMaxLvl.Location = new System.Drawing.Point(68, 330);
            this.TBQuestMaxLvl.Margin = new System.Windows.Forms.Padding(2);
            this.TBQuestMaxLvl.Name = "TBQuestMaxLvl";
            this.TBQuestMaxLvl.Size = new System.Drawing.Size(76, 20);
            this.TBQuestMaxLvl.TabIndex = 7;
            // 
            // TBQuestMinLvl
            // 
            this.TBQuestMinLvl.Location = new System.Drawing.Point(68, 308);
            this.TBQuestMinLvl.Margin = new System.Windows.Forms.Padding(2);
            this.TBQuestMinLvl.Name = "TBQuestMinLvl";
            this.TBQuestMinLvl.Size = new System.Drawing.Size(76, 20);
            this.TBQuestMinLvl.TabIndex = 6;
            // 
            // Label16
            // 
            this.Label16.AutoSize = true;
            this.Label16.Location = new System.Drawing.Point(6, 309);
            this.Label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label16.Name = "Label16";
            this.Label16.Size = new System.Drawing.Size(53, 13);
            this.Label16.TabIndex = 19;
            this.Label16.Text = "Min Level";
            // 
            // TabPageObjectives
            // 
            this.TabPageObjectives.Controls.Add(this.label43);
            this.TabPageObjectives.Controls.Add(this.TBObjMessage);
            this.TabPageObjectives.Controls.Add(this.label42);
            this.TabPageObjectives.Controls.Add(this.TBObjPressKey);
            this.TabPageObjectives.Controls.Add(this.LBObjHotspots);
            this.TabPageObjectives.Controls.Add(this.CBObjInternalQuestIdManual);
            this.TabPageObjectives.Controls.Add(this.Label37);
            this.TabPageObjectives.Controls.Add(this.TBObjInternalIndex);
            this.TabPageObjectives.Controls.Add(this.ButtonObjDumpIndex);
            this.TabPageObjectives.Controls.Add(this.PanelObjAll);
            this.TabPageObjectives.Controls.Add(this.ButtonObjHotSpots);
            this.TabPageObjectives.Controls.Add(this.ButtonObjImportFromGame);
            this.TabPageObjectives.Controls.Add(this.CBObjKillMobPickUpItem);
            this.TabPageObjectives.Controls.Add(this.CBObjInternalQuestID);
            this.TabPageObjectives.Controls.Add(this.CBInternalObj);
            this.TabPageObjectives.Controls.Add(this.Label32);
            this.TabPageObjectives.Controls.Add(this.CBObjIgnoreQuestCompleted);
            this.TabPageObjectives.Controls.Add(this.Label30);
            this.TabPageObjectives.Controls.Add(this.TBObjQuestName);
            this.TabPageObjectives.Controls.Add(this.Label29);
            this.TabPageObjectives.Controls.Add(this.TBObjQuestID);
            this.TabPageObjectives.Controls.Add(this.LabelObjNPCIDorName);
            this.TabPageObjectives.Controls.Add(this.TBObjNPCId);
            this.TabPageObjectives.Controls.Add(this.ButtonObjectiveNew);
            this.TabPageObjectives.Controls.Add(this.ButtonObjectiveSave);
            this.TabPageObjectives.Controls.Add(this.Label8);
            this.TabPageObjectives.Controls.Add(this.Label7);
            this.TabPageObjectives.Controls.Add(this.CBObjType);
            this.TabPageObjectives.Controls.Add(this.PanelObjTaxi);
            this.TabPageObjectives.Location = new System.Drawing.Point(4, 22);
            this.TabPageObjectives.Margin = new System.Windows.Forms.Padding(2);
            this.TabPageObjectives.Name = "TabPageObjectives";
            this.TabPageObjectives.Padding = new System.Windows.Forms.Padding(2);
            this.TabPageObjectives.Size = new System.Drawing.Size(375, 383);
            this.TabPageObjectives.TabIndex = 1;
            this.TabPageObjectives.Text = "Objectives";
            this.TabPageObjectives.UseVisualStyleBackColor = true;
            // 
            // LBObjHotspots
            // 
            this.LBObjHotspots.FormattingEnabled = true;
            this.LBObjHotspots.Location = new System.Drawing.Point(183, 290);
            this.LBObjHotspots.Margin = new System.Windows.Forms.Padding(2);
            this.LBObjHotspots.Name = "LBObjHotspots";
            this.LBObjHotspots.Size = new System.Drawing.Size(189, 69);
            this.LBObjHotspots.TabIndex = 80;
            // 
            // CBObjInternalQuestIdManual
            // 
            this.CBObjInternalQuestIdManual.AutoSize = true;
            this.CBObjInternalQuestIdManual.Checked = true;
            this.CBObjInternalQuestIdManual.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CBObjInternalQuestIdManual.Location = new System.Drawing.Point(282, 186);
            this.CBObjInternalQuestIdManual.Name = "CBObjInternalQuestIdManual";
            this.CBObjInternalQuestIdManual.Size = new System.Drawing.Size(67, 17);
            this.CBObjInternalQuestIdManual.TabIndex = 79;
            this.CBObjInternalQuestIdManual.Text = "Manual?";
            this.CBObjInternalQuestIdManual.UseVisualStyleBackColor = true;
            // 
            // Label37
            // 
            this.Label37.AutoSize = true;
            this.Label37.Location = new System.Drawing.Point(186, 243);
            this.Label37.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label37.Name = "Label37";
            this.Label37.Size = new System.Drawing.Size(68, 13);
            this.Label37.TabIndex = 78;
            this.Label37.Text = "InternalIndex";
            // 
            // TBObjInternalIndex
            // 
            this.TBObjInternalIndex.Location = new System.Drawing.Point(269, 240);
            this.TBObjInternalIndex.Margin = new System.Windows.Forms.Padding(2);
            this.TBObjInternalIndex.Name = "TBObjInternalIndex";
            this.TBObjInternalIndex.Size = new System.Drawing.Size(98, 20);
            this.TBObjInternalIndex.TabIndex = 78;
            // 
            // ButtonObjDumpIndex
            // 
            this.ButtonObjDumpIndex.Location = new System.Drawing.Point(208, 11);
            this.ButtonObjDumpIndex.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonObjDumpIndex.Name = "ButtonObjDumpIndex";
            this.ButtonObjDumpIndex.Size = new System.Drawing.Size(119, 19);
            this.ButtonObjDumpIndex.TabIndex = 73;
            this.ButtonObjDumpIndex.Text = "Dump Internal Index";
            this.ButtonObjDumpIndex.UseVisualStyleBackColor = true;
            this.ButtonObjDumpIndex.Click += new System.EventHandler(this.ButtonObjDumpIndex_Click);
            // 
            // PanelObjAll
            // 
            this.PanelObjAll.Controls.Add(this.CBObjIsDead);
            this.PanelObjAll.Controls.Add(this.TBObjGossipOption);
            this.PanelObjAll.Controls.Add(this.Label35);
            this.PanelObjAll.Controls.Add(this.Label9);
            this.PanelObjAll.Controls.Add(this.CBObjCanPullUnitsInFight);
            this.PanelObjAll.Controls.Add(this.ButtonObjImportGPS);
            this.PanelObjAll.Controls.Add(this.TBObjCount);
            this.PanelObjAll.Controls.Add(this.ButtonObjImportEntry);
            this.PanelObjAll.Controls.Add(this.TBObjEntry);
            this.PanelObjAll.Controls.Add(this.Label10);
            this.PanelObjAll.Controls.Add(this.TBObjCollectItemID);
            this.PanelObjAll.Controls.Add(this.Label20);
            this.PanelObjAll.Controls.Add(this.TBObjCollectCount);
            this.PanelObjAll.Controls.Add(this.Label21);
            this.PanelObjAll.Controls.Add(this.TBObjUseItemID);
            this.PanelObjAll.Controls.Add(this.Label22);
            this.PanelObjAll.Controls.Add(this.TBObjPosition);
            this.PanelObjAll.Controls.Add(this.Label23);
            this.PanelObjAll.Controls.Add(this.TBObjWaitMs);
            this.PanelObjAll.Controls.Add(this.Label24);
            this.PanelObjAll.Controls.Add(this.TBObjRange);
            this.PanelObjAll.Controls.Add(this.Label25);
            this.PanelObjAll.Controls.Add(this.TBObjUseSpellId);
            this.PanelObjAll.Controls.Add(this.Label26);
            this.PanelObjAll.Location = new System.Drawing.Point(12, 58);
            this.PanelObjAll.Margin = new System.Windows.Forms.Padding(2);
            this.PanelObjAll.Name = "PanelObjAll";
            this.PanelObjAll.Size = new System.Drawing.Size(168, 301);
            this.PanelObjAll.TabIndex = 14;
            this.PanelObjAll.Visible = false;
            // 
            // CBObjIsDead
            // 
            this.CBObjIsDead.AutoSize = true;
            this.CBObjIsDead.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CBObjIsDead.Location = new System.Drawing.Point(48, 281);
            this.CBObjIsDead.Name = "CBObjIsDead";
            this.CBObjIsDead.Size = new System.Drawing.Size(103, 17);
            this.CBObjIsDead.TabIndex = 78;
            this.CBObjIsDead.Text = "Target Is Dead?";
            this.CBObjIsDead.UseVisualStyleBackColor = true;
            // 
            // TBObjGossipOption
            // 
            this.TBObjGossipOption.Location = new System.Drawing.Point(88, 240);
            this.TBObjGossipOption.Margin = new System.Windows.Forms.Padding(2);
            this.TBObjGossipOption.Name = "TBObjGossipOption";
            this.TBObjGossipOption.Size = new System.Drawing.Size(76, 20);
            this.TBObjGossipOption.TabIndex = 76;
            // 
            // Label35
            // 
            this.Label35.AutoSize = true;
            this.Label35.Location = new System.Drawing.Point(2, 243);
            this.Label35.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label35.Name = "Label35";
            this.Label35.Size = new System.Drawing.Size(78, 13);
            this.Label35.TabIndex = 77;
            this.Label35.Text = "Gossip Options";
            // 
            // Label9
            // 
            this.Label9.AutoSize = true;
            this.Label9.Location = new System.Drawing.Point(2, 9);
            this.Label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label9.Name = "Label9";
            this.Label9.Size = new System.Drawing.Size(35, 13);
            this.Label9.TabIndex = 32;
            this.Label9.Text = "Count";
            // 
            // CBObjCanPullUnitsInFight
            // 
            this.CBObjCanPullUnitsInFight.AutoSize = true;
            this.CBObjCanPullUnitsInFight.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CBObjCanPullUnitsInFight.Location = new System.Drawing.Point(-2, 265);
            this.CBObjCanPullUnitsInFight.Name = "CBObjCanPullUnitsInFight";
            this.CBObjCanPullUnitsInFight.Size = new System.Drawing.Size(153, 17);
            this.CBObjCanPullUnitsInFight.TabIndex = 73;
            this.CBObjCanPullUnitsInFight.Text = "CanPullUnitsAlreadyInFight";
            this.CBObjCanPullUnitsInFight.UseVisualStyleBackColor = true;
            // 
            // ButtonObjImportGPS
            // 
            this.ButtonObjImportGPS.Location = new System.Drawing.Point(60, 148);
            this.ButtonObjImportGPS.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonObjImportGPS.Name = "ButtonObjImportGPS";
            this.ButtonObjImportGPS.Size = new System.Drawing.Size(23, 19);
            this.ButtonObjImportGPS.TabIndex = 75;
            this.ButtonObjImportGPS.Text = "Import From Game";
            this.ButtonObjImportGPS.UseVisualStyleBackColor = true;
            this.ButtonObjImportGPS.Click += new System.EventHandler(this.ButtonObjImportGPS_Click);
            // 
            // TBObjCount
            // 
            this.TBObjCount.Location = new System.Drawing.Point(88, 5);
            this.TBObjCount.Margin = new System.Windows.Forms.Padding(2);
            this.TBObjCount.Name = "TBObjCount";
            this.TBObjCount.Size = new System.Drawing.Size(76, 20);
            this.TBObjCount.TabIndex = 1;
            // 
            // ButtonObjImportEntry
            // 
            this.ButtonObjImportEntry.Location = new System.Drawing.Point(60, 29);
            this.ButtonObjImportEntry.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonObjImportEntry.Name = "ButtonObjImportEntry";
            this.ButtonObjImportEntry.Size = new System.Drawing.Size(23, 19);
            this.ButtonObjImportEntry.TabIndex = 74;
            this.ButtonObjImportEntry.Text = "Import From Game";
            this.ButtonObjImportEntry.UseVisualStyleBackColor = true;
            this.ButtonObjImportEntry.Click += new System.EventHandler(this.ButtonObjImportEntry_Click);
            // 
            // TBObjEntry
            // 
            this.TBObjEntry.Location = new System.Drawing.Point(88, 29);
            this.TBObjEntry.Margin = new System.Windows.Forms.Padding(2);
            this.TBObjEntry.Multiline = true;
            this.TBObjEntry.Name = "TBObjEntry";
            this.TBObjEntry.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TBObjEntry.Size = new System.Drawing.Size(76, 48);
            this.TBObjEntry.TabIndex = 2;
            // 
            // Label10
            // 
            this.Label10.AutoSize = true;
            this.Label10.Location = new System.Drawing.Point(2, 33);
            this.Label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label10.Name = "Label10";
            this.Label10.Size = new System.Drawing.Size(31, 13);
            this.Label10.TabIndex = 34;
            this.Label10.Text = "Entry";
            // 
            // TBObjCollectItemID
            // 
            this.TBObjCollectItemID.Location = new System.Drawing.Point(88, 80);
            this.TBObjCollectItemID.Margin = new System.Windows.Forms.Padding(2);
            this.TBObjCollectItemID.Name = "TBObjCollectItemID";
            this.TBObjCollectItemID.Size = new System.Drawing.Size(76, 20);
            this.TBObjCollectItemID.TabIndex = 3;
            this.TBObjCollectItemID.Text = "1";
            // 
            // Label20
            // 
            this.Label20.AutoSize = true;
            this.Label20.Location = new System.Drawing.Point(2, 84);
            this.Label20.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label20.Name = "Label20";
            this.Label20.Size = new System.Drawing.Size(68, 13);
            this.Label20.TabIndex = 36;
            this.Label20.Text = "CollectItemId";
            // 
            // TBObjCollectCount
            // 
            this.TBObjCollectCount.Location = new System.Drawing.Point(88, 102);
            this.TBObjCollectCount.Margin = new System.Windows.Forms.Padding(2);
            this.TBObjCollectCount.Name = "TBObjCollectCount";
            this.TBObjCollectCount.Size = new System.Drawing.Size(76, 20);
            this.TBObjCollectCount.TabIndex = 4;
            // 
            // Label21
            // 
            this.Label21.AutoSize = true;
            this.Label21.Location = new System.Drawing.Point(2, 106);
            this.Label21.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label21.Name = "Label21";
            this.Label21.Size = new System.Drawing.Size(67, 13);
            this.Label21.TabIndex = 38;
            this.Label21.Text = "CollectCount";
            // 
            // TBObjUseItemID
            // 
            this.TBObjUseItemID.Location = new System.Drawing.Point(88, 126);
            this.TBObjUseItemID.Margin = new System.Windows.Forms.Padding(2);
            this.TBObjUseItemID.Name = "TBObjUseItemID";
            this.TBObjUseItemID.Size = new System.Drawing.Size(76, 20);
            this.TBObjUseItemID.TabIndex = 5;
            // 
            // Label22
            // 
            this.Label22.AutoSize = true;
            this.Label22.Location = new System.Drawing.Point(2, 128);
            this.Label22.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label22.Name = "Label22";
            this.Label22.Size = new System.Drawing.Size(55, 13);
            this.Label22.TabIndex = 40;
            this.Label22.Text = "UseItemId";
            // 
            // TBObjPosition
            // 
            this.TBObjPosition.Location = new System.Drawing.Point(88, 148);
            this.TBObjPosition.Margin = new System.Windows.Forms.Padding(2);
            this.TBObjPosition.Name = "TBObjPosition";
            this.TBObjPosition.Size = new System.Drawing.Size(76, 20);
            this.TBObjPosition.TabIndex = 6;
            // 
            // Label23
            // 
            this.Label23.AutoSize = true;
            this.Label23.Location = new System.Drawing.Point(2, 151);
            this.Label23.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label23.Name = "Label23";
            this.Label23.Size = new System.Drawing.Size(44, 13);
            this.Label23.TabIndex = 42;
            this.Label23.Text = "Position";
            // 
            // TBObjWaitMs
            // 
            this.TBObjWaitMs.Location = new System.Drawing.Point(88, 170);
            this.TBObjWaitMs.Margin = new System.Windows.Forms.Padding(2);
            this.TBObjWaitMs.Name = "TBObjWaitMs";
            this.TBObjWaitMs.Size = new System.Drawing.Size(76, 20);
            this.TBObjWaitMs.TabIndex = 7;
            // 
            // Label24
            // 
            this.Label24.AutoSize = true;
            this.Label24.Location = new System.Drawing.Point(2, 174);
            this.Label24.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label24.Name = "Label24";
            this.Label24.Size = new System.Drawing.Size(43, 13);
            this.Label24.TabIndex = 44;
            this.Label24.Text = "WaitMs";
            // 
            // TBObjRange
            // 
            this.TBObjRange.Location = new System.Drawing.Point(88, 196);
            this.TBObjRange.Margin = new System.Windows.Forms.Padding(2);
            this.TBObjRange.Name = "TBObjRange";
            this.TBObjRange.Size = new System.Drawing.Size(76, 20);
            this.TBObjRange.TabIndex = 8;
            // 
            // Label25
            // 
            this.Label25.AutoSize = true;
            this.Label25.Location = new System.Drawing.Point(2, 200);
            this.Label25.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label25.Name = "Label25";
            this.Label25.Size = new System.Drawing.Size(39, 13);
            this.Label25.TabIndex = 46;
            this.Label25.Text = "Range";
            // 
            // TBObjUseSpellId
            // 
            this.TBObjUseSpellId.Location = new System.Drawing.Point(88, 219);
            this.TBObjUseSpellId.Margin = new System.Windows.Forms.Padding(2);
            this.TBObjUseSpellId.Name = "TBObjUseSpellId";
            this.TBObjUseSpellId.Size = new System.Drawing.Size(76, 20);
            this.TBObjUseSpellId.TabIndex = 9;
            // 
            // Label26
            // 
            this.Label26.AutoSize = true;
            this.Label26.Location = new System.Drawing.Point(2, 222);
            this.Label26.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label26.Name = "Label26";
            this.Label26.Size = new System.Drawing.Size(58, 13);
            this.Label26.TabIndex = 48;
            this.Label26.Text = "UseSpellId";
            // 
            // ButtonObjHotSpots
            // 
            this.ButtonObjHotSpots.Location = new System.Drawing.Point(180, 262);
            this.ButtonObjHotSpots.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonObjHotSpots.Name = "ButtonObjHotSpots";
            this.ButtonObjHotSpots.Size = new System.Drawing.Size(191, 28);
            this.ButtonObjHotSpots.TabIndex = 71;
            this.ButtonObjHotSpots.Text = "Add HotSpot (Player Position IG)";
            this.ButtonObjHotSpots.UseVisualStyleBackColor = true;
            this.ButtonObjHotSpots.Click += new System.EventHandler(this.ButtonObjHotSpots_Click);
            // 
            // ButtonObjImportFromGame
            // 
            this.ButtonObjImportFromGame.Location = new System.Drawing.Point(100, 11);
            this.ButtonObjImportFromGame.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonObjImportFromGame.Name = "ButtonObjImportFromGame";
            this.ButtonObjImportFromGame.Size = new System.Drawing.Size(104, 19);
            this.ButtonObjImportFromGame.TabIndex = 70;
            this.ButtonObjImportFromGame.Text = "Import From Game";
            this.ButtonObjImportFromGame.UseVisualStyleBackColor = true;
            this.ButtonObjImportFromGame.Click += new System.EventHandler(this.ButtonObjImportFromGame_Click);
            // 
            // CBObjKillMobPickUpItem
            // 
            this.CBObjKillMobPickUpItem.AutoSize = true;
            this.CBObjKillMobPickUpItem.Location = new System.Drawing.Point(277, 39);
            this.CBObjKillMobPickUpItem.Name = "CBObjKillMobPickUpItem";
            this.CBObjKillMobPickUpItem.Size = new System.Drawing.Size(90, 17);
            this.CBObjKillMobPickUpItem.TabIndex = 65;
            this.CBObjKillMobPickUpItem.Text = "PickUp Item?";
            this.CBObjKillMobPickUpItem.UseVisualStyleBackColor = true;
            this.CBObjKillMobPickUpItem.Visible = false;
            this.CBObjKillMobPickUpItem.CheckedChanged += new System.EventHandler(this.CBObjKillMobPickUpItem_CheckedChanged);
            // 
            // CBObjInternalQuestID
            // 
            this.CBObjInternalQuestID.FormattingEnabled = true;
            this.CBObjInternalQuestID.Location = new System.Drawing.Point(188, 217);
            this.CBObjInternalQuestID.Name = "CBObjInternalQuestID";
            this.CBObjInternalQuestID.Size = new System.Drawing.Size(179, 21);
            this.CBObjInternalQuestID.TabIndex = 64;
            // 
            // CBInternalObj
            // 
            this.CBInternalObj.AutoSize = true;
            this.CBInternalObj.Checked = true;
            this.CBInternalObj.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CBInternalObj.Location = new System.Drawing.Point(187, 187);
            this.CBInternalObj.Name = "CBInternalObj";
            this.CBInternalObj.Size = new System.Drawing.Size(98, 17);
            this.CBInternalObj.TabIndex = 14;
            this.CBInternalObj.Text = "Internal Quest?";
            this.CBInternalObj.UseVisualStyleBackColor = true;
            this.CBInternalObj.CheckedChanged += new System.EventHandler(this.CBInternalObj_CheckedChanged);
            // 
            // Label32
            // 
            this.Label32.AutoSize = true;
            this.Label32.Location = new System.Drawing.Point(200, 203);
            this.Label32.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label32.Name = "Label32";
            this.Label32.Size = new System.Drawing.Size(85, 13);
            this.Label32.TabIndex = 57;
            this.Label32.Text = "Internal Quest Id";
            // 
            // CBObjIgnoreQuestCompleted
            // 
            this.CBObjIgnoreQuestCompleted.AutoSize = true;
            this.CBObjIgnoreQuestCompleted.Checked = true;
            this.CBObjIgnoreQuestCompleted.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CBObjIgnoreQuestCompleted.Location = new System.Drawing.Point(187, 170);
            this.CBObjIgnoreQuestCompleted.Name = "CBObjIgnoreQuestCompleted";
            this.CBObjIgnoreQuestCompleted.Size = new System.Drawing.Size(140, 17);
            this.CBObjIgnoreQuestCompleted.TabIndex = 13;
            this.CBObjIgnoreQuestCompleted.Text = "Ignore Quest Completed";
            this.CBObjIgnoreQuestCompleted.UseVisualStyleBackColor = true;
            // 
            // Label30
            // 
            this.Label30.AutoSize = true;
            this.Label30.Location = new System.Drawing.Point(184, 113);
            this.Label30.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label30.Name = "Label30";
            this.Label30.Size = new System.Drawing.Size(66, 13);
            this.Label30.TabIndex = 54;
            this.Label30.Text = "Quest Name";
            // 
            // TBObjQuestName
            // 
            this.TBObjQuestName.Location = new System.Drawing.Point(269, 107);
            this.TBObjQuestName.Margin = new System.Windows.Forms.Padding(2);
            this.TBObjQuestName.Name = "TBObjQuestName";
            this.TBObjQuestName.Size = new System.Drawing.Size(98, 20);
            this.TBObjQuestName.TabIndex = 12;
            // 
            // Label29
            // 
            this.Label29.AutoSize = true;
            this.Label29.Location = new System.Drawing.Point(184, 90);
            this.Label29.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label29.Name = "Label29";
            this.Label29.Size = new System.Drawing.Size(46, 13);
            this.Label29.TabIndex = 52;
            this.Label29.Text = "QuestID";
            // 
            // TBObjQuestID
            // 
            this.TBObjQuestID.Location = new System.Drawing.Point(269, 84);
            this.TBObjQuestID.Margin = new System.Windows.Forms.Padding(2);
            this.TBObjQuestID.Name = "TBObjQuestID";
            this.TBObjQuestID.Size = new System.Drawing.Size(98, 20);
            this.TBObjQuestID.TabIndex = 11;
            // 
            // LabelObjNPCIDorName
            // 
            this.LabelObjNPCIDorName.AutoSize = true;
            this.LabelObjNPCIDorName.Location = new System.Drawing.Point(184, 66);
            this.LabelObjNPCIDorName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LabelObjNPCIDorName.Name = "LabelObjNPCIDorName";
            this.LabelObjNPCIDorName.Size = new System.Drawing.Size(41, 13);
            this.LabelObjNPCIDorName.TabIndex = 50;
            this.LabelObjNPCIDorName.Text = "NPC Id";
            // 
            // TBObjNPCId
            // 
            this.TBObjNPCId.Location = new System.Drawing.Point(269, 61);
            this.TBObjNPCId.Margin = new System.Windows.Forms.Padding(2);
            this.TBObjNPCId.Name = "TBObjNPCId";
            this.TBObjNPCId.Size = new System.Drawing.Size(98, 20);
            this.TBObjNPCId.TabIndex = 10;
            // 
            // ButtonObjectiveNew
            // 
            this.ButtonObjectiveNew.Location = new System.Drawing.Point(69, 363);
            this.ButtonObjectiveNew.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonObjectiveNew.Name = "ButtonObjectiveNew";
            this.ButtonObjectiveNew.Size = new System.Drawing.Size(56, 19);
            this.ButtonObjectiveNew.TabIndex = 17;
            this.ButtonObjectiveNew.Text = "New Quest";
            this.ButtonObjectiveNew.UseVisualStyleBackColor = true;
            this.ButtonObjectiveNew.Click += new System.EventHandler(this.ButtonObjectiveNew_Click);
            // 
            // ButtonObjectiveSave
            // 
            this.ButtonObjectiveSave.Location = new System.Drawing.Point(9, 363);
            this.ButtonObjectiveSave.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonObjectiveSave.Name = "ButtonObjectiveSave";
            this.ButtonObjectiveSave.Size = new System.Drawing.Size(56, 19);
            this.ButtonObjectiveSave.TabIndex = 16;
            this.ButtonObjectiveSave.Text = "Save";
            this.ButtonObjectiveSave.UseVisualStyleBackColor = true;
            this.ButtonObjectiveSave.Click += new System.EventHandler(this.ButtonObjectiveSave_Click);
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.Location = new System.Drawing.Point(15, 42);
            this.Label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(31, 13);
            this.Label8.TabIndex = 30;
            this.Label8.Text = "Type";
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Location = new System.Drawing.Point(15, 14);
            this.Label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(57, 13);
            this.Label7.TabIndex = 28;
            this.Label7.Text = "Objectives";
            // 
            // CBObjType
            // 
            this.CBObjType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBObjType.FormattingEnabled = true;
            this.CBObjType.Location = new System.Drawing.Point(100, 37);
            this.CBObjType.Margin = new System.Windows.Forms.Padding(2);
            this.CBObjType.Name = "CBObjType";
            this.CBObjType.Size = new System.Drawing.Size(174, 21);
            this.CBObjType.TabIndex = 0;
            this.CBObjType.SelectedValueChanged += new System.EventHandler(this.CBObjType_SelectedValueChanged);
            // 
            // PanelObjTaxi
            // 
            this.PanelObjTaxi.Controls.Add(this.TBObjFlightWaitMs);
            this.PanelObjTaxi.Controls.Add(this.Label36);
            this.PanelObjTaxi.Controls.Add(this.ButtonObjGetXY);
            this.PanelObjTaxi.Controls.Add(this.TBObjDestinationY);
            this.PanelObjTaxi.Controls.Add(this.TBObjTaxiEntryId);
            this.PanelObjTaxi.Controls.Add(this.Label33);
            this.PanelObjTaxi.Controls.Add(this.TBObjDestinationX);
            this.PanelObjTaxi.Controls.Add(this.Label31);
            this.PanelObjTaxi.Controls.Add(this.Label34);
            this.PanelObjTaxi.Location = new System.Drawing.Point(12, 211);
            this.PanelObjTaxi.Margin = new System.Windows.Forms.Padding(2);
            this.PanelObjTaxi.Name = "PanelObjTaxi";
            this.PanelObjTaxi.Size = new System.Drawing.Size(170, 145);
            this.PanelObjTaxi.TabIndex = 14;
            this.PanelObjTaxi.Visible = false;
            // 
            // TBObjFlightWaitMs
            // 
            this.TBObjFlightWaitMs.Location = new System.Drawing.Point(90, 80);
            this.TBObjFlightWaitMs.Margin = new System.Windows.Forms.Padding(2);
            this.TBObjFlightWaitMs.Name = "TBObjFlightWaitMs";
            this.TBObjFlightWaitMs.Size = new System.Drawing.Size(76, 20);
            this.TBObjFlightWaitMs.TabIndex = 64;
            // 
            // Label36
            // 
            this.Label36.AutoSize = true;
            this.Label36.Location = new System.Drawing.Point(4, 81);
            this.Label36.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label36.Name = "Label36";
            this.Label36.Size = new System.Drawing.Size(43, 13);
            this.Label36.TabIndex = 65;
            this.Label36.Text = "WaitMs";
            // 
            // ButtonObjGetXY
            // 
            this.ButtonObjGetXY.Location = new System.Drawing.Point(2, 106);
            this.ButtonObjGetXY.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonObjGetXY.Name = "ButtonObjGetXY";
            this.ButtonObjGetXY.Size = new System.Drawing.Size(163, 31);
            this.ButtonObjGetXY.TabIndex = 14;
            this.ButtonObjGetXY.Text = "Import X Y After Flight";
            this.ButtonObjGetXY.UseVisualStyleBackColor = true;
            // 
            // TBObjDestinationY
            // 
            this.TBObjDestinationY.Location = new System.Drawing.Point(90, 56);
            this.TBObjDestinationY.Margin = new System.Windows.Forms.Padding(2);
            this.TBObjDestinationY.Name = "TBObjDestinationY";
            this.TBObjDestinationY.Size = new System.Drawing.Size(76, 20);
            this.TBObjDestinationY.TabIndex = 62;
            // 
            // TBObjTaxiEntryId
            // 
            this.TBObjTaxiEntryId.Location = new System.Drawing.Point(90, 8);
            this.TBObjTaxiEntryId.Margin = new System.Windows.Forms.Padding(2);
            this.TBObjTaxiEntryId.Name = "TBObjTaxiEntryId";
            this.TBObjTaxiEntryId.Size = new System.Drawing.Size(76, 20);
            this.TBObjTaxiEntryId.TabIndex = 58;
            // 
            // Label33
            // 
            this.Label33.AutoSize = true;
            this.Label33.Location = new System.Drawing.Point(4, 11);
            this.Label33.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label33.Name = "Label33";
            this.Label33.Size = new System.Drawing.Size(60, 13);
            this.Label33.TabIndex = 60;
            this.Label33.Text = "TaxiEntryId";
            // 
            // TBObjDestinationX
            // 
            this.TBObjDestinationX.Location = new System.Drawing.Point(90, 32);
            this.TBObjDestinationX.Margin = new System.Windows.Forms.Padding(2);
            this.TBObjDestinationX.Name = "TBObjDestinationX";
            this.TBObjDestinationX.Size = new System.Drawing.Size(76, 20);
            this.TBObjDestinationX.TabIndex = 59;
            // 
            // Label31
            // 
            this.Label31.AutoSize = true;
            this.Label31.Location = new System.Drawing.Point(4, 32);
            this.Label31.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label31.Name = "Label31";
            this.Label31.Size = new System.Drawing.Size(70, 13);
            this.Label31.TabIndex = 61;
            this.Label31.Text = "Destination X";
            // 
            // Label34
            // 
            this.Label34.AutoSize = true;
            this.Label34.Location = new System.Drawing.Point(4, 57);
            this.Label34.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label34.Name = "Label34";
            this.Label34.Size = new System.Drawing.Size(70, 13);
            this.Label34.TabIndex = 63;
            this.Label34.Text = "Destination Y";
            // 
            // ButtonSaveAsXML
            // 
            this.ButtonSaveAsXML.Location = new System.Drawing.Point(335, 50);
            this.ButtonSaveAsXML.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonSaveAsXML.Name = "ButtonSaveAsXML";
            this.ButtonSaveAsXML.Size = new System.Drawing.Size(56, 20);
            this.ButtonSaveAsXML.TabIndex = 3;
            this.ButtonSaveAsXML.Text = "Save As";
            this.ButtonSaveAsXML.UseVisualStyleBackColor = true;
            this.ButtonSaveAsXML.Click += new System.EventHandler(this.ButtonSaveAsXML_Click);
            // 
            // ButtonNewXML
            // 
            this.ButtonNewXML.Location = new System.Drawing.Point(455, 50);
            this.ButtonNewXML.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonNewXML.Name = "ButtonNewXML";
            this.ButtonNewXML.Size = new System.Drawing.Size(56, 20);
            this.ButtonNewXML.TabIndex = 13;
            this.ButtonNewXML.Text = "New Quest";
            this.ButtonNewXML.UseVisualStyleBackColor = true;
            this.ButtonNewXML.Click += new System.EventHandler(this.ButtonNewXML_Click);
            // 
            // CBMainDisplayXML
            // 
            this.CBMainDisplayXML.AutoSize = true;
            this.CBMainDisplayXML.Location = new System.Drawing.Point(521, 53);
            this.CBMainDisplayXML.Margin = new System.Windows.Forms.Padding(2);
            this.CBMainDisplayXML.Name = "CBMainDisplayXML";
            this.CBMainDisplayXML.Size = new System.Drawing.Size(85, 17);
            this.CBMainDisplayXML.TabIndex = 82;
            this.CBMainDisplayXML.Text = "Display XML";
            this.CBMainDisplayXML.UseVisualStyleBackColor = true;
            this.CBMainDisplayXML.CheckedChanged += new System.EventHandler(this.CBMainDisplayXML_CheckedChanged);
            // 
            // ButtonSaveXML
            // 
            this.ButtonSaveXML.Location = new System.Drawing.Point(395, 50);
            this.ButtonSaveXML.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonSaveXML.Name = "ButtonSaveXML";
            this.ButtonSaveXML.Size = new System.Drawing.Size(56, 20);
            this.ButtonSaveXML.TabIndex = 84;
            this.ButtonSaveXML.Text = "Save";
            this.ButtonSaveXML.UseVisualStyleBackColor = true;
            this.ButtonSaveXML.Click += new System.EventHandler(this.ButtonSaveXML_Click);
            // 
            // tnbControlMenu1
            // 
            this.tnbControlMenu1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tnbControlMenu1.BackgroundImage")));
            this.tnbControlMenu1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tnbControlMenu1.Location = new System.Drawing.Point(0, 0);
            this.tnbControlMenu1.LogoImage = ((System.Drawing.Image)(resources.GetObject("tnbControlMenu1.LogoImage")));
            this.tnbControlMenu1.Margin = new System.Windows.Forms.Padding(2);
            this.tnbControlMenu1.Name = "tnbControlMenu1";
            this.tnbControlMenu1.Size = new System.Drawing.Size(663, 35);
            this.tnbControlMenu1.TabIndex = 85;
            this.tnbControlMenu1.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.tnbControlMenu1.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(222)))));
            this.tnbControlMenu1.TitleText = "TheNoobBot";
            // 
            // UcXmlRichTextBox1
            // 
            this.UcXmlRichTextBox1.Font = new System.Drawing.Font("Consolas", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UcXmlRichTextBox1.Location = new System.Drawing.Point(667, 50);
            this.UcXmlRichTextBox1.Margin = new System.Windows.Forms.Padding(2);
            this.UcXmlRichTextBox1.Name = "UcXmlRichTextBox1";
            this.UcXmlRichTextBox1.Size = new System.Drawing.Size(425, 432);
            this.UcXmlRichTextBox1.TabIndex = 15;
            this.UcXmlRichTextBox1.Text = "";
            this.UcXmlRichTextBox1.Xml = "";
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(184, 153);
            this.label42.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(54, 13);
            this.label42.TabIndex = 84;
            this.label42.Text = "Press Key";
            // 
            // TBObjPressKey
            // 
            this.TBObjPressKey.Location = new System.Drawing.Point(269, 150);
            this.TBObjPressKey.Margin = new System.Windows.Forms.Padding(2);
            this.TBObjPressKey.Name = "TBObjPressKey";
            this.TBObjPressKey.Size = new System.Drawing.Size(98, 20);
            this.TBObjPressKey.TabIndex = 83;
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(184, 132);
            this.label43.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(50, 13);
            this.label43.TabIndex = 86;
            this.label43.Text = "Message";
            // 
            // TBObjMessage
            // 
            this.TBObjMessage.Location = new System.Drawing.Point(269, 129);
            this.TBObjMessage.Margin = new System.Windows.Forms.Padding(2);
            this.TBObjMessage.Name = "TBObjMessage";
            this.TBObjMessage.Size = new System.Drawing.Size(98, 20);
            this.TBObjMessage.TabIndex = 85;
            // 
            // SimpleProfileEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 487);
            this.ControlBox = false;
            this.Controls.Add(this.tnbControlMenu1);
            this.Controls.Add(this.ButtonSaveXML);
            this.Controls.Add(this.CBMainDisplayXML);
            this.Controls.Add(this.UcXmlRichTextBox1);
            this.Controls.Add(this.ButtonNewXML);
            this.Controls.Add(this.PanelSimpleQuest);
            this.Controls.Add(this.ButtonSaveAsXML);
            this.Controls.Add(this.PanelNPC);
            this.Controls.Add(this.ButtonLoadXML);
            this.Controls.Add(this.TreeView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SimpleProfileEditor";
            this.ContextMenuStrip.ResumeLayout(false);
            this.PanelNPC.ResumeLayout(false);
            this.PanelNPC.PerformLayout();
            this.PanelSimpleQuest.ResumeLayout(false);
            this.TabControl1.ResumeLayout(false);
            this.TabPageQuest.ResumeLayout(false);
            this.TabPageQuest.PerformLayout();
            this.ContextMenuStripNeedQuest.ResumeLayout(false);
            this.TabPageObjectives.ResumeLayout(false);
            this.TabPageObjectives.PerformLayout();
            this.PanelObjAll.ResumeLayout(false);
            this.PanelObjAll.PerformLayout();
            this.PanelObjTaxi.ResumeLayout(false);
            this.PanelObjTaxi.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        internal TreeView TreeView;
        internal Button ButtonLoadXML;
        internal Panel PanelNPC;
        internal Label Label3;
        internal TextBox TBNpcPosition;
        internal Label Label2;
        internal TextBox TBNpcId;
        internal Label Label1;
        internal TextBox TBNpcName;
        internal Button ButtonNewNPC;
        internal Panel PanelSimpleQuest;
        internal Label Label19;
        internal TextBox TBQuestTurnInID;
        internal CheckBox CheckBoxItemPickUp;
        internal Label Label18;
        internal TextBox TBQuestPickUpID;
        internal Label Label17;
        internal TextBox TBQuestMaxLvl;
        internal Label Label16;
        internal TextBox TBQuestMinLvl;
        internal ComboBox CBRaceMask;
        internal Label Label15;
        internal TextBox TBQuestLevel;
        internal Label Label14;
        internal Label Label13;
        internal ComboBox CBClassMask;
        internal Label Label11;
        internal TextBox TBQuestID;
        internal Label Label12;
        internal TextBox TBQuestQuestName;
        internal Label Label8;
        internal ComboBox CBObjType;
        internal Label Label7;
        internal Button ButtonSaveAsXML;
        internal TabControl TabControl1;
        internal TabPage TabPageQuest;
        internal TabPage TabPageObjectives;
        internal Label Label24;
        internal TextBox TBObjWaitMs;
        internal Label Label23;
        internal TextBox TBObjPosition;
        internal Label Label22;
        internal TextBox TBObjUseItemID;
        internal Label Label21;
        internal TextBox TBObjCollectCount;
        internal Label Label20;
        internal TextBox TBObjCollectItemID;
        internal Label Label10;
        internal TextBox TBObjEntry;
        internal Label Label9;
        internal TextBox TBObjCount;
        internal Label Label25;
        internal TextBox TBObjRange;
        internal Label Label26;
        internal TextBox TBObjUseSpellId;
        internal Button ButtonNewXML;
        internal Button ButtonSaveNPC;
        internal Button ButtonQuestNew;
        internal Button ButtonQuestSave;
        internal TextBox TBQuestNeedQuestCompId;
        internal Label Label27;
        internal Button ButtonObjectiveNew;
        internal Button ButtonObjectiveSave;
        internal Label LabelObjNPCIDorName;
        internal TextBox TBObjNPCId;
        internal Label Label30;
        internal TextBox TBObjQuestName;
        internal Label Label29;
        internal TextBox TBObjQuestID;
        internal CheckBox CBObjIgnoreQuestCompleted;
        internal CheckBox CBInternalObj;
        internal Label Label32;
        internal Label Label34;
        internal TextBox TBObjDestinationY;
        internal Label Label31;
        internal TextBox TBObjDestinationX;
        internal Label Label33;
        internal TextBox TBObjTaxiEntryId;
        internal ComboBox CBObjInternalQuestID;
        internal CheckBox CBObjKillMobPickUpItem;
        internal Button ButtonObjImportFromGame;
        internal Button ButtonOpenWowHead;
        internal Button ButtonQuestImportFromGame;
        internal CheckedListBox CLBQuestClassMask;
        internal CheckedListBox CLBQuestRaceMask;
        internal Button ButtonObjHotSpots;
        internal Button ButtonQuestAlliance;
        internal Button ButtonQuestHorde;
        internal CheckBox CBObjCanPullUnitsInFight;
        internal Button ButtonObjImportEntry;
        internal Button ButtonObjImportGPS;
        internal Panel PanelObjTaxi;
        internal Panel PanelObjAll;
        internal Button ButtonObjGetXY;
        internal Label Label6;
        internal TextBox TBNpcContinentId;
        internal Label Label5;
        internal Label Label4;
        internal ComboBox CBNpcType;
        internal ComboBox CBNpcFaction;
        internal Button ButtonNpcImport;
        internal CheckBox CBQuestAutoAccepted;
        internal Label Label28;
        internal TextBox TBQuestWQ;
        internal CheckBox CBQuestWQ;
        internal nManager.Helpful.Forms.UserControls.UCXmlRichTextBox UcXmlRichTextBox1;
        internal CheckBox CBMainDisplayXML;
        internal TextBox TBObjGossipOption;
        internal Label Label35;
        internal new ContextMenuStrip ContextMenuStrip;
        internal ToolStripMenuItem InsertUpToolStripMenuItem;
        internal ToolStripMenuItem InsertDownToolStripMenuItem1;
        internal ToolStripMenuItem DeleteToolStripMenuItem;
        internal ContextMenuStrip ContextMenuStripNeedQuest;
        internal ToolStripMenuItem ToolStripMenuItemAddNeedQuestComp;
        internal TextBox TBObjFlightWaitMs;
        internal Label Label36;
        internal Button ButtonSaveXML;
        internal Button ButtonObjDumpIndex;
        internal Label Label37;
        internal TextBox TBObjInternalIndex;
        private nManager.Helpful.Forms.UserControls.TnbControlMenu tnbControlMenu1;
        internal Label label38;
        internal TextBox TBQuestNeedQuestNotCompId;
        internal Label label39;
        internal TextBox TBQuestAutoCompleteIDs;
        internal Label label40;
        internal TextBox TBQuestAutoAcceptIDs;
        internal CheckBox CBObjInternalQuestIdManual;
        private ListBox LBObjHotspots;
        internal CheckBox CBObjIsDead;
        internal Label label43;
        internal TextBox TBObjMessage;
        internal Label label42;
        internal TextBox TBObjPressKey;
    }
}