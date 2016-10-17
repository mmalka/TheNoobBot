<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class TnbProfileCreatorForm
    Inherits System.Windows.Forms.Form

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requise par le Concepteur Windows Form
    Private components As System.ComponentModel.IContainer

    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée à l'aide du Concepteur Windows Form.  
    'Ne la modifiez pas à l'aide de l'éditeur de code.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TnbProfileCreatorForm))
        Me.TreeView = New System.Windows.Forms.TreeView()
        Me.ContextMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.DeleteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.InsertUpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.InsertDownToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ButtonLoadXML = New System.Windows.Forms.Button()
        Me.PanelNPC = New System.Windows.Forms.Panel()
        Me.ButtonNpcImport = New System.Windows.Forms.Button()
        Me.CBNpcType = New System.Windows.Forms.ComboBox()
        Me.CBNpcFaction = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.TBNpcContinentId = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.ButtonSaveNPC = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TBNpcPosition = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TBNpcId = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TBNpcName = New System.Windows.Forms.TextBox()
        Me.ButtonNewNPC = New System.Windows.Forms.Button()
        Me.PanelSimpleQuest = New System.Windows.Forms.Panel()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPageQuest = New System.Windows.Forms.TabPage()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.TBQuestWQ = New System.Windows.Forms.TextBox()
        Me.CBQuestWQ = New System.Windows.Forms.CheckBox()
        Me.CBQuestAutoAccept = New System.Windows.Forms.CheckBox()
        Me.ButtonQuestAlliance = New System.Windows.Forms.Button()
        Me.ButtonQuestHorde = New System.Windows.Forms.Button()
        Me.CLBQuestRaceMask = New System.Windows.Forms.CheckedListBox()
        Me.CLBQuestClassMask = New System.Windows.Forms.CheckedListBox()
        Me.ButtonQuestImportFromGame = New System.Windows.Forms.Button()
        Me.ButtonOpenWowHead = New System.Windows.Forms.Button()
        Me.TBQuestNeedQuestCompId = New System.Windows.Forms.TextBox()
        Me.ContextMenuStripNeedQuest = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItemAddNeedQuestComp = New System.Windows.Forms.ToolStripMenuItem()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.ButtonQuestNew = New System.Windows.Forms.Button()
        Me.ButtonQuestSave = New System.Windows.Forms.Button()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.TBQuestQuestName = New System.Windows.Forms.TextBox()
        Me.TBQuestID = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.CBClassMask = New System.Windows.Forms.ComboBox()
        Me.TBQuestTurnInID = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.CheckBoxItemPickUp = New System.Windows.Forms.CheckBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.TBQuestLevel = New System.Windows.Forms.TextBox()
        Me.TBQuestPickUpID = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.CBRaceMask = New System.Windows.Forms.ComboBox()
        Me.TBQuestMaxLvl = New System.Windows.Forms.TextBox()
        Me.TBQuestMinLvl = New System.Windows.Forms.TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.TabPageObjectives = New System.Windows.Forms.TabPage()
        Me.ButtonObjDumpIndex = New System.Windows.Forms.Button()
        Me.PanelObjAll = New System.Windows.Forms.Panel()
        Me.TBObjGossipOption = New System.Windows.Forms.TextBox()
        Me.Label35 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.CBObjCanPullUnitsInFight = New System.Windows.Forms.CheckBox()
        Me.ButtonObjImportGPS = New System.Windows.Forms.Button()
        Me.TBObjCount = New System.Windows.Forms.TextBox()
        Me.ButtonObjImportEntry = New System.Windows.Forms.Button()
        Me.TBObjEntry = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.TBObjCollectItemID = New System.Windows.Forms.TextBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.TBObjCollectCount = New System.Windows.Forms.TextBox()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.TBObjUseItemID = New System.Windows.Forms.TextBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.TBObjPosition = New System.Windows.Forms.TextBox()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.TBObjWaitMs = New System.Windows.Forms.TextBox()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.TBObjRange = New System.Windows.Forms.TextBox()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.TBObjUseSpellId = New System.Windows.Forms.TextBox()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.TBObjHotSpots = New System.Windows.Forms.TextBox()
        Me.ButtonObjHotSpots = New System.Windows.Forms.Button()
        Me.ButtonObjImportFromGame = New System.Windows.Forms.Button()
        Me.CBObjKillMobPickUpItem = New System.Windows.Forms.CheckBox()
        Me.CBObjInternalQuestID = New System.Windows.Forms.ComboBox()
        Me.CBInternalObj = New System.Windows.Forms.CheckBox()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.CBObjIgnoreQuestCompleted = New System.Windows.Forms.CheckBox()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.TBObjQuestName = New System.Windows.Forms.TextBox()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.TBObjQuestID = New System.Windows.Forms.TextBox()
        Me.LabelObjNPCIDorName = New System.Windows.Forms.Label()
        Me.TBObjNPCId = New System.Windows.Forms.TextBox()
        Me.ButtonObjectiveNew = New System.Windows.Forms.Button()
        Me.ButtonObjectiveSave = New System.Windows.Forms.Button()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.CBObjType = New System.Windows.Forms.ComboBox()
        Me.PanelObjTaxi = New System.Windows.Forms.Panel()
        Me.TBObjFlightWaitMs = New System.Windows.Forms.TextBox()
        Me.Label36 = New System.Windows.Forms.Label()
        Me.ButtonObjGetXY = New System.Windows.Forms.Button()
        Me.TBObjDestinationY = New System.Windows.Forms.TextBox()
        Me.TBObjTaxiEntryId = New System.Windows.Forms.TextBox()
        Me.Label33 = New System.Windows.Forms.Label()
        Me.TBObjDestinationX = New System.Windows.Forms.TextBox()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.Label34 = New System.Windows.Forms.Label()
        Me.ButtonSaveAsXML = New System.Windows.Forms.Button()
        Me.ButtonNewXML = New System.Windows.Forms.Button()
        Me.CBMainDisplayXML = New System.Windows.Forms.CheckBox()
        Me.TnbControlMenu1 = New nManager.Helpful.Forms.UserControls.TnbControlMenu()
        Me.ButtonSaveXML = New System.Windows.Forms.Button()
        Me.UcXmlRichTextBox1 = New nManager.QPE.CustomXmlViewer.ucXmlRichTextBox()
        Me.TBObjInternalIndex = New System.Windows.Forms.TextBox()
        Me.Label37 = New System.Windows.Forms.Label()
        Me.ContextMenuStrip.SuspendLayout()
        Me.PanelNPC.SuspendLayout()
        Me.PanelSimpleQuest.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPageQuest.SuspendLayout()
        Me.ContextMenuStripNeedQuest.SuspendLayout()
        Me.TabPageObjectives.SuspendLayout()
        Me.PanelObjAll.SuspendLayout()
        Me.PanelObjTaxi.SuspendLayout()
        Me.SuspendLayout()
        '
        'TreeView
        '
        Me.TreeView.ContextMenuStrip = Me.ContextMenuStrip
        Me.TreeView.Location = New System.Drawing.Point(9, 50)
        Me.TreeView.Margin = New System.Windows.Forms.Padding(2)
        Me.TreeView.Name = "TreeView"
        Me.TreeView.Size = New System.Drawing.Size(258, 432)
        Me.TreeView.TabIndex = 0
        '
        'ContextMenuStrip
        '
        Me.ContextMenuStrip.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ContextMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DeleteToolStripMenuItem, Me.InsertUpToolStripMenuItem, Me.InsertDownToolStripMenuItem1})
        Me.ContextMenuStrip.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip.Size = New System.Drawing.Size(141, 70)
        '
        'DeleteToolStripMenuItem
        '
        Me.DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem"
        Me.DeleteToolStripMenuItem.Size = New System.Drawing.Size(140, 22)
        Me.DeleteToolStripMenuItem.Text = "Delete"
        '
        'InsertUpToolStripMenuItem
        '
        Me.InsertUpToolStripMenuItem.Name = "InsertUpToolStripMenuItem"
        Me.InsertUpToolStripMenuItem.Size = New System.Drawing.Size(140, 22)
        Me.InsertUpToolStripMenuItem.Text = "Insert Above"
        '
        'InsertDownToolStripMenuItem1
        '
        Me.InsertDownToolStripMenuItem1.Name = "InsertDownToolStripMenuItem1"
        Me.InsertDownToolStripMenuItem1.Size = New System.Drawing.Size(140, 22)
        Me.InsertDownToolStripMenuItem1.Text = "Insert Below"
        '
        'ButtonLoadXML
        '
        Me.ButtonLoadXML.Location = New System.Drawing.Point(275, 50)
        Me.ButtonLoadXML.Margin = New System.Windows.Forms.Padding(2)
        Me.ButtonLoadXML.Name = "ButtonLoadXML"
        Me.ButtonLoadXML.Size = New System.Drawing.Size(56, 20)
        Me.ButtonLoadXML.TabIndex = 1
        Me.ButtonLoadXML.Text = "Load Xml"
        Me.ButtonLoadXML.UseVisualStyleBackColor = True
        '
        'PanelNPC
        '
        Me.PanelNPC.Controls.Add(Me.ButtonNpcImport)
        Me.PanelNPC.Controls.Add(Me.CBNpcType)
        Me.PanelNPC.Controls.Add(Me.CBNpcFaction)
        Me.PanelNPC.Controls.Add(Me.Label6)
        Me.PanelNPC.Controls.Add(Me.TBNpcContinentId)
        Me.PanelNPC.Controls.Add(Me.Label5)
        Me.PanelNPC.Controls.Add(Me.Label4)
        Me.PanelNPC.Controls.Add(Me.ButtonSaveNPC)
        Me.PanelNPC.Controls.Add(Me.Label3)
        Me.PanelNPC.Controls.Add(Me.TBNpcPosition)
        Me.PanelNPC.Controls.Add(Me.Label2)
        Me.PanelNPC.Controls.Add(Me.TBNpcId)
        Me.PanelNPC.Controls.Add(Me.Label1)
        Me.PanelNPC.Controls.Add(Me.TBNpcName)
        Me.PanelNPC.Controls.Add(Me.ButtonNewNPC)
        Me.PanelNPC.Location = New System.Drawing.Point(270, 74)
        Me.PanelNPC.Margin = New System.Windows.Forms.Padding(2)
        Me.PanelNPC.Name = "PanelNPC"
        Me.PanelNPC.Size = New System.Drawing.Size(390, 415)
        Me.PanelNPC.TabIndex = 0
        '
        'ButtonNpcImport
        '
        Me.ButtonNpcImport.Location = New System.Drawing.Point(148, 165)
        Me.ButtonNpcImport.Margin = New System.Windows.Forms.Padding(2)
        Me.ButtonNpcImport.Name = "ButtonNpcImport"
        Me.ButtonNpcImport.Size = New System.Drawing.Size(68, 19)
        Me.ButtonNpcImport.TabIndex = 15
        Me.ButtonNpcImport.Text = "Import"
        Me.ButtonNpcImport.UseVisualStyleBackColor = True
        '
        'CBNpcType
        '
        Me.CBNpcType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CBNpcType.FormattingEnabled = True
        Me.CBNpcType.Location = New System.Drawing.Point(70, 106)
        Me.CBNpcType.Margin = New System.Windows.Forms.Padding(2)
        Me.CBNpcType.Name = "CBNpcType"
        Me.CBNpcType.Size = New System.Drawing.Size(92, 21)
        Me.CBNpcType.TabIndex = 14
        '
        'CBNpcFaction
        '
        Me.CBNpcFaction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CBNpcFaction.FormattingEnabled = True
        Me.CBNpcFaction.Location = New System.Drawing.Point(70, 81)
        Me.CBNpcFaction.Margin = New System.Windows.Forms.Padding(2)
        Me.CBNpcFaction.Name = "CBNpcFaction"
        Me.CBNpcFaction.Size = New System.Drawing.Size(92, 21)
        Me.CBNpcFaction.TabIndex = 13
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(2, 130)
        Me.Label6.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(66, 13)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "Continent ID"
        '
        'TBNpcContinentId
        '
        Me.TBNpcContinentId.Location = New System.Drawing.Point(70, 130)
        Me.TBNpcContinentId.Margin = New System.Windows.Forms.Padding(2)
        Me.TBNpcContinentId.Name = "TBNpcContinentId"
        Me.TBNpcContinentId.Size = New System.Drawing.Size(76, 20)
        Me.TBNpcContinentId.TabIndex = 11
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(2, 106)
        Me.Label5.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(31, 13)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "Type"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(2, 81)
        Me.Label4.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(42, 13)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Faction"
        '
        'ButtonSaveNPC
        '
        Me.ButtonSaveNPC.Location = New System.Drawing.Point(76, 165)
        Me.ButtonSaveNPC.Margin = New System.Windows.Forms.Padding(2)
        Me.ButtonSaveNPC.Name = "ButtonSaveNPC"
        Me.ButtonSaveNPC.Size = New System.Drawing.Size(68, 19)
        Me.ButtonSaveNPC.TabIndex = 6
        Me.ButtonSaveNPC.Text = "Save NPC"
        Me.ButtonSaveNPC.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(2, 58)
        Me.Label3.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(44, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Position"
        '
        'TBNpcPosition
        '
        Me.TBNpcPosition.Location = New System.Drawing.Point(70, 58)
        Me.TBNpcPosition.Margin = New System.Windows.Forms.Padding(2)
        Me.TBNpcPosition.Name = "TBNpcPosition"
        Me.TBNpcPosition.Size = New System.Drawing.Size(76, 20)
        Me.TBNpcPosition.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(2, 36)
        Me.Label2.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(18, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "ID"
        '
        'TBNpcId
        '
        Me.TBNpcId.Location = New System.Drawing.Point(70, 36)
        Me.TBNpcId.Margin = New System.Windows.Forms.Padding(2)
        Me.TBNpcId.Name = "TBNpcId"
        Me.TBNpcId.Size = New System.Drawing.Size(76, 20)
        Me.TBNpcId.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(2, 12)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "NPC Name"
        '
        'TBNpcName
        '
        Me.TBNpcName.Location = New System.Drawing.Point(70, 13)
        Me.TBNpcName.Margin = New System.Windows.Forms.Padding(2)
        Me.TBNpcName.Name = "TBNpcName"
        Me.TBNpcName.Size = New System.Drawing.Size(76, 20)
        Me.TBNpcName.TabIndex = 0
        '
        'ButtonNewNPC
        '
        Me.ButtonNewNPC.Location = New System.Drawing.Point(4, 165)
        Me.ButtonNewNPC.Margin = New System.Windows.Forms.Padding(2)
        Me.ButtonNewNPC.Name = "ButtonNewNPC"
        Me.ButtonNewNPC.Size = New System.Drawing.Size(68, 19)
        Me.ButtonNewNPC.TabIndex = 5
        Me.ButtonNewNPC.Text = "New NPC"
        Me.ButtonNewNPC.UseVisualStyleBackColor = True
        '
        'PanelSimpleQuest
        '
        Me.PanelSimpleQuest.Controls.Add(Me.TabControl1)
        Me.PanelSimpleQuest.Location = New System.Drawing.Point(270, 74)
        Me.PanelSimpleQuest.Margin = New System.Windows.Forms.Padding(2)
        Me.PanelSimpleQuest.Name = "PanelSimpleQuest"
        Me.PanelSimpleQuest.Size = New System.Drawing.Size(392, 415)
        Me.PanelSimpleQuest.TabIndex = 12
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPageQuest)
        Me.TabControl1.Controls.Add(Me.TabPageObjectives)
        Me.TabControl1.Location = New System.Drawing.Point(5, 1)
        Me.TabControl1.Margin = New System.Windows.Forms.Padding(2)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(383, 409)
        Me.TabControl1.TabIndex = 31
        '
        'TabPageQuest
        '
        Me.TabPageQuest.Controls.Add(Me.Label28)
        Me.TabPageQuest.Controls.Add(Me.TBQuestWQ)
        Me.TabPageQuest.Controls.Add(Me.CBQuestWQ)
        Me.TabPageQuest.Controls.Add(Me.CBQuestAutoAccept)
        Me.TabPageQuest.Controls.Add(Me.ButtonQuestAlliance)
        Me.TabPageQuest.Controls.Add(Me.ButtonQuestHorde)
        Me.TabPageQuest.Controls.Add(Me.CLBQuestRaceMask)
        Me.TabPageQuest.Controls.Add(Me.CLBQuestClassMask)
        Me.TabPageQuest.Controls.Add(Me.ButtonQuestImportFromGame)
        Me.TabPageQuest.Controls.Add(Me.ButtonOpenWowHead)
        Me.TabPageQuest.Controls.Add(Me.TBQuestNeedQuestCompId)
        Me.TabPageQuest.Controls.Add(Me.Label27)
        Me.TabPageQuest.Controls.Add(Me.ButtonQuestNew)
        Me.TabPageQuest.Controls.Add(Me.ButtonQuestSave)
        Me.TabPageQuest.Controls.Add(Me.Label12)
        Me.TabPageQuest.Controls.Add(Me.TBQuestQuestName)
        Me.TabPageQuest.Controls.Add(Me.TBQuestID)
        Me.TabPageQuest.Controls.Add(Me.Label11)
        Me.TabPageQuest.Controls.Add(Me.Label19)
        Me.TabPageQuest.Controls.Add(Me.CBClassMask)
        Me.TabPageQuest.Controls.Add(Me.TBQuestTurnInID)
        Me.TabPageQuest.Controls.Add(Me.Label13)
        Me.TabPageQuest.Controls.Add(Me.CheckBoxItemPickUp)
        Me.TabPageQuest.Controls.Add(Me.Label14)
        Me.TabPageQuest.Controls.Add(Me.Label18)
        Me.TabPageQuest.Controls.Add(Me.TBQuestLevel)
        Me.TabPageQuest.Controls.Add(Me.TBQuestPickUpID)
        Me.TabPageQuest.Controls.Add(Me.Label15)
        Me.TabPageQuest.Controls.Add(Me.Label17)
        Me.TabPageQuest.Controls.Add(Me.CBRaceMask)
        Me.TabPageQuest.Controls.Add(Me.TBQuestMaxLvl)
        Me.TabPageQuest.Controls.Add(Me.TBQuestMinLvl)
        Me.TabPageQuest.Controls.Add(Me.Label16)
        Me.TabPageQuest.Location = New System.Drawing.Point(4, 22)
        Me.TabPageQuest.Margin = New System.Windows.Forms.Padding(2)
        Me.TabPageQuest.Name = "TabPageQuest"
        Me.TabPageQuest.Padding = New System.Windows.Forms.Padding(2)
        Me.TabPageQuest.Size = New System.Drawing.Size(375, 383)
        Me.TabPageQuest.TabIndex = 0
        Me.TabPageQuest.Text = "Quest"
        Me.TabPageQuest.UseVisualStyleBackColor = True
        '
        'Label28
        '
        Me.Label28.AutoSize = True
        Me.Label28.Location = New System.Drawing.Point(148, 332)
        Me.Label28.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(47, 13)
        Me.Label28.TabIndex = 81
        Me.Label28.Text = "WQ Pos"
        '
        'TBQuestWQ
        '
        Me.TBQuestWQ.Location = New System.Drawing.Point(202, 330)
        Me.TBQuestWQ.Margin = New System.Windows.Forms.Padding(2)
        Me.TBQuestWQ.Name = "TBQuestWQ"
        Me.TBQuestWQ.Size = New System.Drawing.Size(76, 20)
        Me.TBQuestWQ.TabIndex = 80
        '
        'CBQuestWQ
        '
        Me.CBQuestWQ.AutoSize = True
        Me.CBQuestWQ.Location = New System.Drawing.Point(282, 333)
        Me.CBQuestWQ.Margin = New System.Windows.Forms.Padding(2)
        Me.CBQuestWQ.Name = "CBQuestWQ"
        Me.CBQuestWQ.Size = New System.Drawing.Size(91, 17)
        Me.CBQuestWQ.TabIndex = 79
        Me.CBQuestWQ.Text = "World Quest?"
        Me.CBQuestWQ.UseVisualStyleBackColor = True
        '
        'CBQuestAutoAccept
        '
        Me.CBQuestAutoAccept.AutoSize = True
        Me.CBQuestAutoAccept.Location = New System.Drawing.Point(205, 75)
        Me.CBQuestAutoAccept.Margin = New System.Windows.Forms.Padding(2)
        Me.CBQuestAutoAccept.Name = "CBQuestAutoAccept"
        Me.CBQuestAutoAccept.Size = New System.Drawing.Size(82, 17)
        Me.CBQuestAutoAccept.TabIndex = 78
        Me.CBQuestAutoAccept.Text = "AutoAccept"
        Me.CBQuestAutoAccept.UseVisualStyleBackColor = True
        '
        'ButtonQuestAlliance
        '
        Me.ButtonQuestAlliance.Location = New System.Drawing.Point(9, 228)
        Me.ButtonQuestAlliance.Margin = New System.Windows.Forms.Padding(2)
        Me.ButtonQuestAlliance.Name = "ButtonQuestAlliance"
        Me.ButtonQuestAlliance.Size = New System.Drawing.Size(64, 19)
        Me.ButtonQuestAlliance.TabIndex = 77
        Me.ButtonQuestAlliance.Text = "Alliance"
        Me.ButtonQuestAlliance.UseVisualStyleBackColor = True
        '
        'ButtonQuestHorde
        '
        Me.ButtonQuestHorde.Location = New System.Drawing.Point(9, 206)
        Me.ButtonQuestHorde.Margin = New System.Windows.Forms.Padding(2)
        Me.ButtonQuestHorde.Name = "ButtonQuestHorde"
        Me.ButtonQuestHorde.Size = New System.Drawing.Size(64, 19)
        Me.ButtonQuestHorde.TabIndex = 76
        Me.ButtonQuestHorde.Text = "Horde"
        Me.ButtonQuestHorde.UseVisualStyleBackColor = True
        '
        'CLBQuestRaceMask
        '
        Me.CLBQuestRaceMask.CheckOnClick = True
        Me.CLBQuestRaceMask.ColumnWidth = 90
        Me.CLBQuestRaceMask.FormattingEnabled = True
        Me.CLBQuestRaceMask.HorizontalScrollbar = True
        Me.CLBQuestRaceMask.Location = New System.Drawing.Point(86, 191)
        Me.CLBQuestRaceMask.Margin = New System.Windows.Forms.Padding(2)
        Me.CLBQuestRaceMask.MultiColumn = True
        Me.CLBQuestRaceMask.Name = "CLBQuestRaceMask"
        Me.CLBQuestRaceMask.ScrollAlwaysVisible = True
        Me.CLBQuestRaceMask.Size = New System.Drawing.Size(268, 79)
        Me.CLBQuestRaceMask.TabIndex = 74
        '
        'CLBQuestClassMask
        '
        Me.CLBQuestClassMask.CheckOnClick = True
        Me.CLBQuestClassMask.ColumnWidth = 70
        Me.CLBQuestClassMask.FormattingEnabled = True
        Me.CLBQuestClassMask.HorizontalScrollbar = True
        Me.CLBQuestClassMask.Location = New System.Drawing.Point(86, 109)
        Me.CLBQuestClassMask.Margin = New System.Windows.Forms.Padding(2)
        Me.CLBQuestClassMask.MultiColumn = True
        Me.CLBQuestClassMask.Name = "CLBQuestClassMask"
        Me.CLBQuestClassMask.ScrollAlwaysVisible = True
        Me.CLBQuestClassMask.Size = New System.Drawing.Size(268, 79)
        Me.CLBQuestClassMask.TabIndex = 72
        '
        'ButtonQuestImportFromGame
        '
        Me.ButtonQuestImportFromGame.Location = New System.Drawing.Point(272, 11)
        Me.ButtonQuestImportFromGame.Margin = New System.Windows.Forms.Padding(2)
        Me.ButtonQuestImportFromGame.Name = "ButtonQuestImportFromGame"
        Me.ButtonQuestImportFromGame.Size = New System.Drawing.Size(94, 37)
        Me.ButtonQuestImportFromGame.TabIndex = 71
        Me.ButtonQuestImportFromGame.Text = "Import From Game (NPC)"
        Me.ButtonQuestImportFromGame.UseVisualStyleBackColor = True
        '
        'ButtonOpenWowHead
        '
        Me.ButtonOpenWowHead.Location = New System.Drawing.Point(272, 52)
        Me.ButtonOpenWowHead.Margin = New System.Windows.Forms.Padding(2)
        Me.ButtonOpenWowHead.Name = "ButtonOpenWowHead"
        Me.ButtonOpenWowHead.Size = New System.Drawing.Size(94, 19)
        Me.ButtonOpenWowHead.TabIndex = 28
        Me.ButtonOpenWowHead.Text = "Open Wowhead"
        Me.ButtonOpenWowHead.UseVisualStyleBackColor = True
        '
        'TBQuestNeedQuestCompId
        '
        Me.TBQuestNeedQuestCompId.ContextMenuStrip = Me.ContextMenuStripNeedQuest
        Me.TBQuestNeedQuestCompId.Location = New System.Drawing.Point(123, 56)
        Me.TBQuestNeedQuestCompId.Margin = New System.Windows.Forms.Padding(2)
        Me.TBQuestNeedQuestCompId.Multiline = True
        Me.TBQuestNeedQuestCompId.Name = "TBQuestNeedQuestCompId"
        Me.TBQuestNeedQuestCompId.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TBQuestNeedQuestCompId.Size = New System.Drawing.Size(78, 46)
        Me.TBQuestNeedQuestCompId.TabIndex = 2
        '
        'ContextMenuStripNeedQuest
        '
        Me.ContextMenuStripNeedQuest.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ContextMenuStripNeedQuest.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemAddNeedQuestComp})
        Me.ContextMenuStripNeedQuest.Name = "ContextMenuStripNeedQuest"
        Me.ContextMenuStripNeedQuest.Size = New System.Drawing.Size(193, 26)
        '
        'ToolStripMenuItemAddNeedQuestComp
        '
        Me.ToolStripMenuItemAddNeedQuestComp.Name = "ToolStripMenuItemAddNeedQuestComp"
        Me.ToolStripMenuItemAddNeedQuestComp.Size = New System.Drawing.Size(192, 22)
        Me.ToolStripMenuItemAddNeedQuestComp.Text = "Add Previous Quest ID"
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.Location = New System.Drawing.Point(2, 58)
        Me.Label27.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(106, 13)
        Me.Label27.TabIndex = 27
        Me.Label27.Text = "Need Quest Comp Id"
        '
        'ButtonQuestNew
        '
        Me.ButtonQuestNew.Location = New System.Drawing.Point(69, 363)
        Me.ButtonQuestNew.Margin = New System.Windows.Forms.Padding(2)
        Me.ButtonQuestNew.Name = "ButtonQuestNew"
        Me.ButtonQuestNew.Size = New System.Drawing.Size(56, 19)
        Me.ButtonQuestNew.TabIndex = 15
        Me.ButtonQuestNew.Text = "New Quest"
        Me.ButtonQuestNew.UseVisualStyleBackColor = True
        '
        'ButtonQuestSave
        '
        Me.ButtonQuestSave.Location = New System.Drawing.Point(9, 363)
        Me.ButtonQuestSave.Margin = New System.Windows.Forms.Padding(2)
        Me.ButtonQuestSave.Name = "ButtonQuestSave"
        Me.ButtonQuestSave.Size = New System.Drawing.Size(56, 19)
        Me.ButtonQuestSave.TabIndex = 14
        Me.ButtonQuestSave.Text = "Save"
        Me.ButtonQuestSave.UseVisualStyleBackColor = True
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(4, 14)
        Me.Label12.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(66, 13)
        Me.Label12.TabIndex = 2
        Me.Label12.Text = "Quest Name"
        '
        'TBQuestQuestName
        '
        Me.TBQuestQuestName.Location = New System.Drawing.Point(123, 11)
        Me.TBQuestQuestName.Margin = New System.Windows.Forms.Padding(2)
        Me.TBQuestQuestName.Name = "TBQuestQuestName"
        Me.TBQuestQuestName.Size = New System.Drawing.Size(137, 20)
        Me.TBQuestQuestName.TabIndex = 0
        '
        'TBQuestID
        '
        Me.TBQuestID.Location = New System.Drawing.Point(123, 34)
        Me.TBQuestID.Margin = New System.Windows.Forms.Padding(2)
        Me.TBQuestID.Name = "TBQuestID"
        Me.TBQuestID.Size = New System.Drawing.Size(78, 20)
        Me.TBQuestID.TabIndex = 1
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(4, 37)
        Me.Label11.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(18, 13)
        Me.Label11.TabIndex = 4
        Me.Label11.Text = "ID"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(146, 310)
        Me.Label19.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(52, 13)
        Me.Label19.TabIndex = 26
        Me.Label19.Text = "TurnIn ID"
        '
        'CBClassMask
        '
        Me.CBClassMask.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CBClassMask.FormattingEnabled = True
        Me.CBClassMask.Location = New System.Drawing.Point(264, 109)
        Me.CBClassMask.Margin = New System.Windows.Forms.Padding(2)
        Me.CBClassMask.Name = "CBClassMask"
        Me.CBClassMask.Size = New System.Drawing.Size(90, 21)
        Me.CBClassMask.TabIndex = 3
        Me.CBClassMask.Visible = False
        '
        'TBQuestTurnInID
        '
        Me.TBQuestTurnInID.Location = New System.Drawing.Point(202, 308)
        Me.TBQuestTurnInID.Margin = New System.Windows.Forms.Padding(2)
        Me.TBQuestTurnInID.Name = "TBQuestTurnInID"
        Me.TBQuestTurnInID.Size = New System.Drawing.Size(76, 20)
        Me.TBQuestTurnInID.TabIndex = 9
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(4, 111)
        Me.Label13.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(61, 13)
        Me.Label13.TabIndex = 13
        Me.Label13.Text = "Class Mask"
        '
        'CheckBoxItemPickUp
        '
        Me.CheckBoxItemPickUp.AutoSize = True
        Me.CheckBoxItemPickUp.Location = New System.Drawing.Point(282, 288)
        Me.CheckBoxItemPickUp.Margin = New System.Windows.Forms.Padding(2)
        Me.CheckBoxItemPickUp.Name = "CheckBoxItemPickUp"
        Me.CheckBoxItemPickUp.Size = New System.Drawing.Size(84, 17)
        Me.CheckBoxItemPickUp.TabIndex = 10
        Me.CheckBoxItemPickUp.Text = "Item PickUp"
        Me.CheckBoxItemPickUp.UseVisualStyleBackColor = True
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(4, 191)
        Me.Label14.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(62, 13)
        Me.Label14.TabIndex = 14
        Me.Label14.Text = "Race Mask"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(146, 288)
        Me.Label18.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(56, 13)
        Me.Label18.TabIndex = 23
        Me.Label18.Text = "PickUp ID"
        '
        'TBQuestLevel
        '
        Me.TBQuestLevel.Location = New System.Drawing.Point(68, 285)
        Me.TBQuestLevel.Margin = New System.Windows.Forms.Padding(2)
        Me.TBQuestLevel.Name = "TBQuestLevel"
        Me.TBQuestLevel.Size = New System.Drawing.Size(76, 20)
        Me.TBQuestLevel.TabIndex = 5
        '
        'TBQuestPickUpID
        '
        Me.TBQuestPickUpID.Location = New System.Drawing.Point(202, 285)
        Me.TBQuestPickUpID.Margin = New System.Windows.Forms.Padding(2)
        Me.TBQuestPickUpID.Name = "TBQuestPickUpID"
        Me.TBQuestPickUpID.Size = New System.Drawing.Size(76, 20)
        Me.TBQuestPickUpID.TabIndex = 8
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(6, 286)
        Me.Label15.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(64, 13)
        Me.Label15.TabIndex = 16
        Me.Label15.Text = "Quest Level"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(6, 332)
        Me.Label17.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(56, 13)
        Me.Label17.TabIndex = 21
        Me.Label17.Text = "Max Level"
        '
        'CBRaceMask
        '
        Me.CBRaceMask.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CBRaceMask.FormattingEnabled = True
        Me.CBRaceMask.Location = New System.Drawing.Point(264, 188)
        Me.CBRaceMask.Margin = New System.Windows.Forms.Padding(2)
        Me.CBRaceMask.Name = "CBRaceMask"
        Me.CBRaceMask.Size = New System.Drawing.Size(90, 21)
        Me.CBRaceMask.TabIndex = 4
        Me.CBRaceMask.Visible = False
        '
        'TBQuestMaxLvl
        '
        Me.TBQuestMaxLvl.Location = New System.Drawing.Point(68, 330)
        Me.TBQuestMaxLvl.Margin = New System.Windows.Forms.Padding(2)
        Me.TBQuestMaxLvl.Name = "TBQuestMaxLvl"
        Me.TBQuestMaxLvl.Size = New System.Drawing.Size(76, 20)
        Me.TBQuestMaxLvl.TabIndex = 7
        '
        'TBQuestMinLvl
        '
        Me.TBQuestMinLvl.Location = New System.Drawing.Point(68, 308)
        Me.TBQuestMinLvl.Margin = New System.Windows.Forms.Padding(2)
        Me.TBQuestMinLvl.Name = "TBQuestMinLvl"
        Me.TBQuestMinLvl.Size = New System.Drawing.Size(76, 20)
        Me.TBQuestMinLvl.TabIndex = 6
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(6, 309)
        Me.Label16.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(53, 13)
        Me.Label16.TabIndex = 19
        Me.Label16.Text = "Min Level"
        '
        'TabPageObjectives
        '
        Me.TabPageObjectives.Controls.Add(Me.Label37)
        Me.TabPageObjectives.Controls.Add(Me.TBObjInternalIndex)
        Me.TabPageObjectives.Controls.Add(Me.ButtonObjDumpIndex)
        Me.TabPageObjectives.Controls.Add(Me.PanelObjAll)
        Me.TabPageObjectives.Controls.Add(Me.TBObjHotSpots)
        Me.TabPageObjectives.Controls.Add(Me.ButtonObjHotSpots)
        Me.TabPageObjectives.Controls.Add(Me.ButtonObjImportFromGame)
        Me.TabPageObjectives.Controls.Add(Me.CBObjKillMobPickUpItem)
        Me.TabPageObjectives.Controls.Add(Me.CBObjInternalQuestID)
        Me.TabPageObjectives.Controls.Add(Me.CBInternalObj)
        Me.TabPageObjectives.Controls.Add(Me.Label32)
        Me.TabPageObjectives.Controls.Add(Me.CBObjIgnoreQuestCompleted)
        Me.TabPageObjectives.Controls.Add(Me.Label30)
        Me.TabPageObjectives.Controls.Add(Me.TBObjQuestName)
        Me.TabPageObjectives.Controls.Add(Me.Label29)
        Me.TabPageObjectives.Controls.Add(Me.TBObjQuestID)
        Me.TabPageObjectives.Controls.Add(Me.LabelObjNPCIDorName)
        Me.TabPageObjectives.Controls.Add(Me.TBObjNPCId)
        Me.TabPageObjectives.Controls.Add(Me.ButtonObjectiveNew)
        Me.TabPageObjectives.Controls.Add(Me.ButtonObjectiveSave)
        Me.TabPageObjectives.Controls.Add(Me.Label8)
        Me.TabPageObjectives.Controls.Add(Me.Label7)
        Me.TabPageObjectives.Controls.Add(Me.CBObjType)
        Me.TabPageObjectives.Controls.Add(Me.PanelObjTaxi)
        Me.TabPageObjectives.Location = New System.Drawing.Point(4, 22)
        Me.TabPageObjectives.Margin = New System.Windows.Forms.Padding(2)
        Me.TabPageObjectives.Name = "TabPageObjectives"
        Me.TabPageObjectives.Padding = New System.Windows.Forms.Padding(2)
        Me.TabPageObjectives.Size = New System.Drawing.Size(375, 383)
        Me.TabPageObjectives.TabIndex = 1
        Me.TabPageObjectives.Text = "Objectives"
        Me.TabPageObjectives.UseVisualStyleBackColor = True
        '
        'ButtonObjDumpIndex
        '
        Me.ButtonObjDumpIndex.Location = New System.Drawing.Point(208, 11)
        Me.ButtonObjDumpIndex.Margin = New System.Windows.Forms.Padding(2)
        Me.ButtonObjDumpIndex.Name = "ButtonObjDumpIndex"
        Me.ButtonObjDumpIndex.Size = New System.Drawing.Size(119, 19)
        Me.ButtonObjDumpIndex.TabIndex = 73
        Me.ButtonObjDumpIndex.Text = "Dump Internal Index"
        Me.ButtonObjDumpIndex.UseVisualStyleBackColor = True
        '
        'PanelObjAll
        '
        Me.PanelObjAll.Controls.Add(Me.TBObjGossipOption)
        Me.PanelObjAll.Controls.Add(Me.Label35)
        Me.PanelObjAll.Controls.Add(Me.Label9)
        Me.PanelObjAll.Controls.Add(Me.CBObjCanPullUnitsInFight)
        Me.PanelObjAll.Controls.Add(Me.ButtonObjImportGPS)
        Me.PanelObjAll.Controls.Add(Me.TBObjCount)
        Me.PanelObjAll.Controls.Add(Me.ButtonObjImportEntry)
        Me.PanelObjAll.Controls.Add(Me.TBObjEntry)
        Me.PanelObjAll.Controls.Add(Me.Label10)
        Me.PanelObjAll.Controls.Add(Me.TBObjCollectItemID)
        Me.PanelObjAll.Controls.Add(Me.Label20)
        Me.PanelObjAll.Controls.Add(Me.TBObjCollectCount)
        Me.PanelObjAll.Controls.Add(Me.Label21)
        Me.PanelObjAll.Controls.Add(Me.TBObjUseItemID)
        Me.PanelObjAll.Controls.Add(Me.Label22)
        Me.PanelObjAll.Controls.Add(Me.TBObjPosition)
        Me.PanelObjAll.Controls.Add(Me.Label23)
        Me.PanelObjAll.Controls.Add(Me.TBObjWaitMs)
        Me.PanelObjAll.Controls.Add(Me.Label24)
        Me.PanelObjAll.Controls.Add(Me.TBObjRange)
        Me.PanelObjAll.Controls.Add(Me.Label25)
        Me.PanelObjAll.Controls.Add(Me.TBObjUseSpellId)
        Me.PanelObjAll.Controls.Add(Me.Label26)
        Me.PanelObjAll.Location = New System.Drawing.Point(12, 58)
        Me.PanelObjAll.Margin = New System.Windows.Forms.Padding(2)
        Me.PanelObjAll.Name = "PanelObjAll"
        Me.PanelObjAll.Size = New System.Drawing.Size(168, 288)
        Me.PanelObjAll.TabIndex = 14
        Me.PanelObjAll.Visible = False
        '
        'TBObjGossipOption
        '
        Me.TBObjGossipOption.Location = New System.Drawing.Point(88, 240)
        Me.TBObjGossipOption.Margin = New System.Windows.Forms.Padding(2)
        Me.TBObjGossipOption.Name = "TBObjGossipOption"
        Me.TBObjGossipOption.Size = New System.Drawing.Size(76, 20)
        Me.TBObjGossipOption.TabIndex = 76
        '
        'Label35
        '
        Me.Label35.AutoSize = True
        Me.Label35.Location = New System.Drawing.Point(2, 243)
        Me.Label35.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(78, 13)
        Me.Label35.TabIndex = 77
        Me.Label35.Text = "Gossip Options"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(2, 9)
        Me.Label9.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(35, 13)
        Me.Label9.TabIndex = 32
        Me.Label9.Text = "Count"
        '
        'CBObjCanPullUnitsInFight
        '
        Me.CBObjCanPullUnitsInFight.AutoSize = True
        Me.CBObjCanPullUnitsInFight.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CBObjCanPullUnitsInFight.Location = New System.Drawing.Point(-2, 265)
        Me.CBObjCanPullUnitsInFight.Name = "CBObjCanPullUnitsInFight"
        Me.CBObjCanPullUnitsInFight.Size = New System.Drawing.Size(153, 17)
        Me.CBObjCanPullUnitsInFight.TabIndex = 73
        Me.CBObjCanPullUnitsInFight.Text = "CanPullUnitsAlreadyInFight"
        Me.CBObjCanPullUnitsInFight.UseVisualStyleBackColor = True
        '
        'ButtonObjImportGPS
        '
        Me.ButtonObjImportGPS.Location = New System.Drawing.Point(60, 148)
        Me.ButtonObjImportGPS.Margin = New System.Windows.Forms.Padding(2)
        Me.ButtonObjImportGPS.Name = "ButtonObjImportGPS"
        Me.ButtonObjImportGPS.Size = New System.Drawing.Size(23, 19)
        Me.ButtonObjImportGPS.TabIndex = 75
        Me.ButtonObjImportGPS.Text = "Import From Game"
        Me.ButtonObjImportGPS.UseVisualStyleBackColor = True
        '
        'TBObjCount
        '
        Me.TBObjCount.Location = New System.Drawing.Point(88, 5)
        Me.TBObjCount.Margin = New System.Windows.Forms.Padding(2)
        Me.TBObjCount.Name = "TBObjCount"
        Me.TBObjCount.Size = New System.Drawing.Size(76, 20)
        Me.TBObjCount.TabIndex = 1
        '
        'ButtonObjImportEntry
        '
        Me.ButtonObjImportEntry.Location = New System.Drawing.Point(60, 29)
        Me.ButtonObjImportEntry.Margin = New System.Windows.Forms.Padding(2)
        Me.ButtonObjImportEntry.Name = "ButtonObjImportEntry"
        Me.ButtonObjImportEntry.Size = New System.Drawing.Size(23, 19)
        Me.ButtonObjImportEntry.TabIndex = 74
        Me.ButtonObjImportEntry.Text = "Import From Game"
        Me.ButtonObjImportEntry.UseVisualStyleBackColor = True
        '
        'TBObjEntry
        '
        Me.TBObjEntry.Location = New System.Drawing.Point(88, 29)
        Me.TBObjEntry.Margin = New System.Windows.Forms.Padding(2)
        Me.TBObjEntry.Multiline = True
        Me.TBObjEntry.Name = "TBObjEntry"
        Me.TBObjEntry.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TBObjEntry.Size = New System.Drawing.Size(76, 48)
        Me.TBObjEntry.TabIndex = 2
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(2, 33)
        Me.Label10.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(31, 13)
        Me.Label10.TabIndex = 34
        Me.Label10.Text = "Entry"
        '
        'TBObjCollectItemID
        '
        Me.TBObjCollectItemID.Location = New System.Drawing.Point(88, 80)
        Me.TBObjCollectItemID.Margin = New System.Windows.Forms.Padding(2)
        Me.TBObjCollectItemID.Name = "TBObjCollectItemID"
        Me.TBObjCollectItemID.Size = New System.Drawing.Size(76, 20)
        Me.TBObjCollectItemID.TabIndex = 3
        Me.TBObjCollectItemID.Text = "1"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(2, 84)
        Me.Label20.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(68, 13)
        Me.Label20.TabIndex = 36
        Me.Label20.Text = "CollectItemId"
        '
        'TBObjCollectCount
        '
        Me.TBObjCollectCount.Location = New System.Drawing.Point(88, 102)
        Me.TBObjCollectCount.Margin = New System.Windows.Forms.Padding(2)
        Me.TBObjCollectCount.Name = "TBObjCollectCount"
        Me.TBObjCollectCount.Size = New System.Drawing.Size(76, 20)
        Me.TBObjCollectCount.TabIndex = 4
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(2, 106)
        Me.Label21.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(67, 13)
        Me.Label21.TabIndex = 38
        Me.Label21.Text = "CollectCount"
        '
        'TBObjUseItemID
        '
        Me.TBObjUseItemID.Location = New System.Drawing.Point(88, 126)
        Me.TBObjUseItemID.Margin = New System.Windows.Forms.Padding(2)
        Me.TBObjUseItemID.Name = "TBObjUseItemID"
        Me.TBObjUseItemID.Size = New System.Drawing.Size(76, 20)
        Me.TBObjUseItemID.TabIndex = 5
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(2, 128)
        Me.Label22.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(55, 13)
        Me.Label22.TabIndex = 40
        Me.Label22.Text = "UseItemId"
        '
        'TBObjPosition
        '
        Me.TBObjPosition.Location = New System.Drawing.Point(88, 148)
        Me.TBObjPosition.Margin = New System.Windows.Forms.Padding(2)
        Me.TBObjPosition.Name = "TBObjPosition"
        Me.TBObjPosition.Size = New System.Drawing.Size(76, 20)
        Me.TBObjPosition.TabIndex = 6
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Location = New System.Drawing.Point(2, 151)
        Me.Label23.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(44, 13)
        Me.Label23.TabIndex = 42
        Me.Label23.Text = "Position"
        '
        'TBObjWaitMs
        '
        Me.TBObjWaitMs.Location = New System.Drawing.Point(88, 170)
        Me.TBObjWaitMs.Margin = New System.Windows.Forms.Padding(2)
        Me.TBObjWaitMs.Name = "TBObjWaitMs"
        Me.TBObjWaitMs.Size = New System.Drawing.Size(76, 20)
        Me.TBObjWaitMs.TabIndex = 7
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Location = New System.Drawing.Point(2, 174)
        Me.Label24.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(43, 13)
        Me.Label24.TabIndex = 44
        Me.Label24.Text = "WaitMs"
        '
        'TBObjRange
        '
        Me.TBObjRange.Location = New System.Drawing.Point(88, 196)
        Me.TBObjRange.Margin = New System.Windows.Forms.Padding(2)
        Me.TBObjRange.Name = "TBObjRange"
        Me.TBObjRange.Size = New System.Drawing.Size(76, 20)
        Me.TBObjRange.TabIndex = 8
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.Location = New System.Drawing.Point(2, 200)
        Me.Label25.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(39, 13)
        Me.Label25.TabIndex = 46
        Me.Label25.Text = "Range"
        '
        'TBObjUseSpellId
        '
        Me.TBObjUseSpellId.Location = New System.Drawing.Point(88, 219)
        Me.TBObjUseSpellId.Margin = New System.Windows.Forms.Padding(2)
        Me.TBObjUseSpellId.Name = "TBObjUseSpellId"
        Me.TBObjUseSpellId.Size = New System.Drawing.Size(76, 20)
        Me.TBObjUseSpellId.TabIndex = 9
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Location = New System.Drawing.Point(2, 222)
        Me.Label26.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(58, 13)
        Me.Label26.TabIndex = 48
        Me.Label26.Text = "UseSpellId"
        '
        'TBObjHotSpots
        '
        Me.TBObjHotSpots.Location = New System.Drawing.Point(181, 280)
        Me.TBObjHotSpots.Margin = New System.Windows.Forms.Padding(2)
        Me.TBObjHotSpots.Multiline = True
        Me.TBObjHotSpots.Name = "TBObjHotSpots"
        Me.TBObjHotSpots.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TBObjHotSpots.Size = New System.Drawing.Size(181, 58)
        Me.TBObjHotSpots.TabIndex = 72
        '
        'ButtonObjHotSpots
        '
        Me.ButtonObjHotSpots.Location = New System.Drawing.Point(180, 250)
        Me.ButtonObjHotSpots.Margin = New System.Windows.Forms.Padding(2)
        Me.ButtonObjHotSpots.Name = "ButtonObjHotSpots"
        Me.ButtonObjHotSpots.Size = New System.Drawing.Size(180, 28)
        Me.ButtonObjHotSpots.TabIndex = 71
        Me.ButtonObjHotSpots.Text = "Add HotSpot (Player Position IG)"
        Me.ButtonObjHotSpots.UseVisualStyleBackColor = True
        '
        'ButtonObjImportFromGame
        '
        Me.ButtonObjImportFromGame.Location = New System.Drawing.Point(100, 11)
        Me.ButtonObjImportFromGame.Margin = New System.Windows.Forms.Padding(2)
        Me.ButtonObjImportFromGame.Name = "ButtonObjImportFromGame"
        Me.ButtonObjImportFromGame.Size = New System.Drawing.Size(104, 19)
        Me.ButtonObjImportFromGame.TabIndex = 70
        Me.ButtonObjImportFromGame.Text = "Import From Game"
        Me.ButtonObjImportFromGame.UseVisualStyleBackColor = True
        '
        'CBObjKillMobPickUpItem
        '
        Me.CBObjKillMobPickUpItem.AutoSize = True
        Me.CBObjKillMobPickUpItem.Location = New System.Drawing.Point(277, 39)
        Me.CBObjKillMobPickUpItem.Name = "CBObjKillMobPickUpItem"
        Me.CBObjKillMobPickUpItem.Size = New System.Drawing.Size(90, 17)
        Me.CBObjKillMobPickUpItem.TabIndex = 65
        Me.CBObjKillMobPickUpItem.Text = "PickUp Item?"
        Me.CBObjKillMobPickUpItem.UseVisualStyleBackColor = True
        Me.CBObjKillMobPickUpItem.Visible = False
        '
        'CBObjInternalQuestID
        '
        Me.CBObjInternalQuestID.FormattingEnabled = True
        Me.CBObjInternalQuestID.Location = New System.Drawing.Point(188, 190)
        Me.CBObjInternalQuestID.Name = "CBObjInternalQuestID"
        Me.CBObjInternalQuestID.Size = New System.Drawing.Size(158, 21)
        Me.CBObjInternalQuestID.TabIndex = 64
        '
        'CBInternalObj
        '
        Me.CBInternalObj.AutoSize = True
        Me.CBInternalObj.Checked = True
        Me.CBInternalObj.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CBInternalObj.Location = New System.Drawing.Point(187, 153)
        Me.CBInternalObj.Name = "CBInternalObj"
        Me.CBInternalObj.Size = New System.Drawing.Size(98, 17)
        Me.CBInternalObj.TabIndex = 14
        Me.CBInternalObj.Text = "Internal Quest?"
        Me.CBInternalObj.UseVisualStyleBackColor = True
        '
        'Label32
        '
        Me.Label32.AutoSize = True
        Me.Label32.Location = New System.Drawing.Point(200, 175)
        Me.Label32.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(85, 13)
        Me.Label32.TabIndex = 57
        Me.Label32.Text = "Internal Quest Id"
        '
        'CBObjIgnoreQuestCompleted
        '
        Me.CBObjIgnoreQuestCompleted.AutoSize = True
        Me.CBObjIgnoreQuestCompleted.Checked = True
        Me.CBObjIgnoreQuestCompleted.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CBObjIgnoreQuestCompleted.Location = New System.Drawing.Point(187, 133)
        Me.CBObjIgnoreQuestCompleted.Name = "CBObjIgnoreQuestCompleted"
        Me.CBObjIgnoreQuestCompleted.Size = New System.Drawing.Size(140, 17)
        Me.CBObjIgnoreQuestCompleted.TabIndex = 13
        Me.CBObjIgnoreQuestCompleted.Text = "Ignore Quest Completed"
        Me.CBObjIgnoreQuestCompleted.UseVisualStyleBackColor = True
        '
        'Label30
        '
        Me.Label30.AutoSize = True
        Me.Label30.Location = New System.Drawing.Point(184, 113)
        Me.Label30.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(66, 13)
        Me.Label30.TabIndex = 54
        Me.Label30.Text = "Quest Name"
        '
        'TBObjQuestName
        '
        Me.TBObjQuestName.Location = New System.Drawing.Point(269, 108)
        Me.TBObjQuestName.Margin = New System.Windows.Forms.Padding(2)
        Me.TBObjQuestName.Name = "TBObjQuestName"
        Me.TBObjQuestName.Size = New System.Drawing.Size(76, 20)
        Me.TBObjQuestName.TabIndex = 12
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.Location = New System.Drawing.Point(184, 90)
        Me.Label29.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(46, 13)
        Me.Label29.TabIndex = 52
        Me.Label29.Text = "QuestID"
        '
        'TBObjQuestID
        '
        Me.TBObjQuestID.Location = New System.Drawing.Point(269, 85)
        Me.TBObjQuestID.Margin = New System.Windows.Forms.Padding(2)
        Me.TBObjQuestID.Name = "TBObjQuestID"
        Me.TBObjQuestID.Size = New System.Drawing.Size(76, 20)
        Me.TBObjQuestID.TabIndex = 11
        '
        'LabelObjNPCIDorName
        '
        Me.LabelObjNPCIDorName.AutoSize = True
        Me.LabelObjNPCIDorName.Location = New System.Drawing.Point(184, 66)
        Me.LabelObjNPCIDorName.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.LabelObjNPCIDorName.Name = "LabelObjNPCIDorName"
        Me.LabelObjNPCIDorName.Size = New System.Drawing.Size(41, 13)
        Me.LabelObjNPCIDorName.TabIndex = 50
        Me.LabelObjNPCIDorName.Text = "NPC Id"
        '
        'TBObjNPCId
        '
        Me.TBObjNPCId.Location = New System.Drawing.Point(269, 61)
        Me.TBObjNPCId.Margin = New System.Windows.Forms.Padding(2)
        Me.TBObjNPCId.Name = "TBObjNPCId"
        Me.TBObjNPCId.Size = New System.Drawing.Size(76, 20)
        Me.TBObjNPCId.TabIndex = 10
        '
        'ButtonObjectiveNew
        '
        Me.ButtonObjectiveNew.Location = New System.Drawing.Point(69, 363)
        Me.ButtonObjectiveNew.Margin = New System.Windows.Forms.Padding(2)
        Me.ButtonObjectiveNew.Name = "ButtonObjectiveNew"
        Me.ButtonObjectiveNew.Size = New System.Drawing.Size(56, 19)
        Me.ButtonObjectiveNew.TabIndex = 17
        Me.ButtonObjectiveNew.Text = "New Quest"
        Me.ButtonObjectiveNew.UseVisualStyleBackColor = True
        '
        'ButtonObjectiveSave
        '
        Me.ButtonObjectiveSave.Location = New System.Drawing.Point(9, 363)
        Me.ButtonObjectiveSave.Margin = New System.Windows.Forms.Padding(2)
        Me.ButtonObjectiveSave.Name = "ButtonObjectiveSave"
        Me.ButtonObjectiveSave.Size = New System.Drawing.Size(56, 19)
        Me.ButtonObjectiveSave.TabIndex = 16
        Me.ButtonObjectiveSave.Text = "Save"
        Me.ButtonObjectiveSave.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(15, 42)
        Me.Label8.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(31, 13)
        Me.Label8.TabIndex = 30
        Me.Label8.Text = "Type"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(15, 14)
        Me.Label7.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(57, 13)
        Me.Label7.TabIndex = 28
        Me.Label7.Text = "Objectives"
        '
        'CBObjType
        '
        Me.CBObjType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CBObjType.FormattingEnabled = True
        Me.CBObjType.Location = New System.Drawing.Point(100, 37)
        Me.CBObjType.Margin = New System.Windows.Forms.Padding(2)
        Me.CBObjType.Name = "CBObjType"
        Me.CBObjType.Size = New System.Drawing.Size(174, 21)
        Me.CBObjType.TabIndex = 0
        '
        'PanelObjTaxi
        '
        Me.PanelObjTaxi.Controls.Add(Me.TBObjFlightWaitMs)
        Me.PanelObjTaxi.Controls.Add(Me.Label36)
        Me.PanelObjTaxi.Controls.Add(Me.ButtonObjGetXY)
        Me.PanelObjTaxi.Controls.Add(Me.TBObjDestinationY)
        Me.PanelObjTaxi.Controls.Add(Me.TBObjTaxiEntryId)
        Me.PanelObjTaxi.Controls.Add(Me.Label33)
        Me.PanelObjTaxi.Controls.Add(Me.TBObjDestinationX)
        Me.PanelObjTaxi.Controls.Add(Me.Label31)
        Me.PanelObjTaxi.Controls.Add(Me.Label34)
        Me.PanelObjTaxi.Location = New System.Drawing.Point(12, 211)
        Me.PanelObjTaxi.Margin = New System.Windows.Forms.Padding(2)
        Me.PanelObjTaxi.Name = "PanelObjTaxi"
        Me.PanelObjTaxi.Size = New System.Drawing.Size(170, 145)
        Me.PanelObjTaxi.TabIndex = 14
        Me.PanelObjTaxi.Visible = False
        '
        'TBObjFlightWaitMs
        '
        Me.TBObjFlightWaitMs.Location = New System.Drawing.Point(90, 80)
        Me.TBObjFlightWaitMs.Margin = New System.Windows.Forms.Padding(2)
        Me.TBObjFlightWaitMs.Name = "TBObjFlightWaitMs"
        Me.TBObjFlightWaitMs.Size = New System.Drawing.Size(76, 20)
        Me.TBObjFlightWaitMs.TabIndex = 64
        '
        'Label36
        '
        Me.Label36.AutoSize = True
        Me.Label36.Location = New System.Drawing.Point(4, 81)
        Me.Label36.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(43, 13)
        Me.Label36.TabIndex = 65
        Me.Label36.Text = "WaitMs"
        '
        'ButtonObjGetXY
        '
        Me.ButtonObjGetXY.Location = New System.Drawing.Point(2, 106)
        Me.ButtonObjGetXY.Margin = New System.Windows.Forms.Padding(2)
        Me.ButtonObjGetXY.Name = "ButtonObjGetXY"
        Me.ButtonObjGetXY.Size = New System.Drawing.Size(163, 31)
        Me.ButtonObjGetXY.TabIndex = 14
        Me.ButtonObjGetXY.Text = "Import X Y After Flight"
        Me.ButtonObjGetXY.UseVisualStyleBackColor = True
        '
        'TBObjDestinationY
        '
        Me.TBObjDestinationY.Location = New System.Drawing.Point(90, 56)
        Me.TBObjDestinationY.Margin = New System.Windows.Forms.Padding(2)
        Me.TBObjDestinationY.Name = "TBObjDestinationY"
        Me.TBObjDestinationY.Size = New System.Drawing.Size(76, 20)
        Me.TBObjDestinationY.TabIndex = 62
        '
        'TBObjTaxiEntryId
        '
        Me.TBObjTaxiEntryId.Location = New System.Drawing.Point(90, 8)
        Me.TBObjTaxiEntryId.Margin = New System.Windows.Forms.Padding(2)
        Me.TBObjTaxiEntryId.Name = "TBObjTaxiEntryId"
        Me.TBObjTaxiEntryId.Size = New System.Drawing.Size(76, 20)
        Me.TBObjTaxiEntryId.TabIndex = 58
        '
        'Label33
        '
        Me.Label33.AutoSize = True
        Me.Label33.Location = New System.Drawing.Point(4, 11)
        Me.Label33.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(60, 13)
        Me.Label33.TabIndex = 60
        Me.Label33.Text = "TaxiEntryId"
        '
        'TBObjDestinationX
        '
        Me.TBObjDestinationX.Location = New System.Drawing.Point(90, 32)
        Me.TBObjDestinationX.Margin = New System.Windows.Forms.Padding(2)
        Me.TBObjDestinationX.Name = "TBObjDestinationX"
        Me.TBObjDestinationX.Size = New System.Drawing.Size(76, 20)
        Me.TBObjDestinationX.TabIndex = 59
        '
        'Label31
        '
        Me.Label31.AutoSize = True
        Me.Label31.Location = New System.Drawing.Point(4, 32)
        Me.Label31.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(70, 13)
        Me.Label31.TabIndex = 61
        Me.Label31.Text = "Destination X"
        '
        'Label34
        '
        Me.Label34.AutoSize = True
        Me.Label34.Location = New System.Drawing.Point(4, 57)
        Me.Label34.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(70, 13)
        Me.Label34.TabIndex = 63
        Me.Label34.Text = "Destination Y"
        '
        'ButtonSaveAsXML
        '
        Me.ButtonSaveAsXML.Location = New System.Drawing.Point(335, 50)
        Me.ButtonSaveAsXML.Margin = New System.Windows.Forms.Padding(2)
        Me.ButtonSaveAsXML.Name = "ButtonSaveAsXML"
        Me.ButtonSaveAsXML.Size = New System.Drawing.Size(56, 20)
        Me.ButtonSaveAsXML.TabIndex = 3
        Me.ButtonSaveAsXML.Text = "Save As"
        Me.ButtonSaveAsXML.UseVisualStyleBackColor = True
        '
        'ButtonNewXML
        '
        Me.ButtonNewXML.Location = New System.Drawing.Point(455, 50)
        Me.ButtonNewXML.Margin = New System.Windows.Forms.Padding(2)
        Me.ButtonNewXML.Name = "ButtonNewXML"
        Me.ButtonNewXML.Size = New System.Drawing.Size(56, 20)
        Me.ButtonNewXML.TabIndex = 13
        Me.ButtonNewXML.Text = "New Quest"
        Me.ButtonNewXML.UseVisualStyleBackColor = True
        '
        'CBMainDisplayXML
        '
        Me.CBMainDisplayXML.AutoSize = True
        Me.CBMainDisplayXML.Location = New System.Drawing.Point(521, 53)
        Me.CBMainDisplayXML.Margin = New System.Windows.Forms.Padding(2)
        Me.CBMainDisplayXML.Name = "CBMainDisplayXML"
        Me.CBMainDisplayXML.Size = New System.Drawing.Size(85, 17)
        Me.CBMainDisplayXML.TabIndex = 82
        Me.CBMainDisplayXML.Text = "Display XML"
        Me.CBMainDisplayXML.UseVisualStyleBackColor = True
        '
        'TnbControlMenu1
        '
        Me.TnbControlMenu1.BackgroundImage = CType(resources.GetObject("TnbControlMenu1.BackgroundImage"), System.Drawing.Image)
        Me.TnbControlMenu1.Dock = System.Windows.Forms.DockStyle.Top
        Me.TnbControlMenu1.Location = New System.Drawing.Point(0, 0)
        Me.TnbControlMenu1.LogoImage = CType(resources.GetObject("TnbControlMenu1.LogoImage"), System.Drawing.Image)
        Me.TnbControlMenu1.Name = "TnbControlMenu1"
        Me.TnbControlMenu1.Size = New System.Drawing.Size(663, 43)
        Me.TnbControlMenu1.TabIndex = 83
        Me.TnbControlMenu1.TitleFont = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.TnbControlMenu1.TitleForeColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(222, Byte), Integer), CType(CType(222, Byte), Integer))
        Me.TnbControlMenu1.TitleText = "TheNoobBot"
        '
        'ButtonSaveXML
        '
        Me.ButtonSaveXML.Location = New System.Drawing.Point(395, 50)
        Me.ButtonSaveXML.Margin = New System.Windows.Forms.Padding(2)
        Me.ButtonSaveXML.Name = "ButtonSaveXML"
        Me.ButtonSaveXML.Size = New System.Drawing.Size(56, 20)
        Me.ButtonSaveXML.TabIndex = 84
        Me.ButtonSaveXML.Text = "Save"
        Me.ButtonSaveXML.UseVisualStyleBackColor = True
        '
        'UcXmlRichTextBox1
        '
        Me.UcXmlRichTextBox1.Font = New System.Drawing.Font("Consolas", 9.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UcXmlRichTextBox1.Location = New System.Drawing.Point(667, 50)
        Me.UcXmlRichTextBox1.Margin = New System.Windows.Forms.Padding(2)
        Me.UcXmlRichTextBox1.Name = "UcXmlRichTextBox1"
        Me.UcXmlRichTextBox1.Size = New System.Drawing.Size(425, 432)
        Me.UcXmlRichTextBox1.TabIndex = 15
        Me.UcXmlRichTextBox1.Text = ""
        Me.UcXmlRichTextBox1.Xml = ""
        '
        'TBObjInternalIndex
        '
        Me.TBObjInternalIndex.Location = New System.Drawing.Point(269, 219)
        Me.TBObjInternalIndex.Margin = New System.Windows.Forms.Padding(2)
        Me.TBObjInternalIndex.Name = "TBObjInternalIndex"
        Me.TBObjInternalIndex.Size = New System.Drawing.Size(76, 20)
        Me.TBObjInternalIndex.TabIndex = 78
        '
        'Label37
        '
        Me.Label37.AutoSize = True
        Me.Label37.Location = New System.Drawing.Point(186, 222)
        Me.Label37.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(68, 13)
        Me.Label37.TabIndex = 78
        Me.Label37.Text = "InternalIndex"
        '
        'TnbProfileCreatorForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(663, 487)
        Me.ControlBox = False
        Me.Controls.Add(Me.ButtonSaveXML)
        Me.Controls.Add(Me.TnbControlMenu1)
        Me.Controls.Add(Me.CBMainDisplayXML)
        Me.Controls.Add(Me.UcXmlRichTextBox1)
        Me.Controls.Add(Me.ButtonNewXML)
        Me.Controls.Add(Me.PanelSimpleQuest)
        Me.Controls.Add(Me.ButtonSaveAsXML)
        Me.Controls.Add(Me.PanelNPC)
        Me.Controls.Add(Me.ButtonLoadXML)
        Me.Controls.Add(Me.TreeView)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "TnbProfileCreatorForm"
        Me.ContextMenuStrip.ResumeLayout(False)
        Me.PanelNPC.ResumeLayout(False)
        Me.PanelNPC.PerformLayout()
        Me.PanelSimpleQuest.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPageQuest.ResumeLayout(False)
        Me.TabPageQuest.PerformLayout()
        Me.ContextMenuStripNeedQuest.ResumeLayout(False)
        Me.TabPageObjectives.ResumeLayout(False)
        Me.TabPageObjectives.PerformLayout()
        Me.PanelObjAll.ResumeLayout(False)
        Me.PanelObjAll.PerformLayout()
        Me.PanelObjTaxi.ResumeLayout(False)
        Me.PanelObjTaxi.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TreeView As TreeView
    Friend WithEvents ButtonLoadXML As Button
    Friend WithEvents PanelNPC As Panel
    Friend WithEvents Label3 As Label
    Friend WithEvents TBNpcPosition As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents TBNpcId As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents TBNpcName As TextBox
    Friend WithEvents ButtonNewNPC As Button
    Friend WithEvents PanelSimpleQuest As Panel
    Friend WithEvents Label19 As Label
    Friend WithEvents TBQuestTurnInID As TextBox
    Friend WithEvents CheckBoxItemPickUp As CheckBox
    Friend WithEvents Label18 As Label
    Friend WithEvents TBQuestPickUpID As TextBox
    Friend WithEvents Label17 As Label
    Friend WithEvents TBQuestMaxLvl As TextBox
    Friend WithEvents Label16 As Label
    Friend WithEvents TBQuestMinLvl As TextBox
    Friend WithEvents CBRaceMask As ComboBox
    Friend WithEvents Label15 As Label
    Friend WithEvents TBQuestLevel As TextBox
    Friend WithEvents Label14 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents CBClassMask As ComboBox
    Friend WithEvents Label11 As Label
    Friend WithEvents TBQuestID As TextBox
    Friend WithEvents Label12 As Label
    Friend WithEvents TBQuestQuestName As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents CBObjType As ComboBox
    Friend WithEvents Label7 As Label
    Friend WithEvents ButtonSaveAsXML As Button
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPageQuest As TabPage
    Friend WithEvents TabPageObjectives As TabPage
    Friend WithEvents Label24 As Label
    Friend WithEvents TBObjWaitMs As TextBox
    Friend WithEvents Label23 As Label
    Friend WithEvents TBObjPosition As TextBox
    Friend WithEvents Label22 As Label
    Friend WithEvents TBObjUseItemID As TextBox
    Friend WithEvents Label21 As Label
    Friend WithEvents TBObjCollectCount As TextBox
    Friend WithEvents Label20 As Label
    Friend WithEvents TBObjCollectItemID As TextBox
    Friend WithEvents Label10 As Label
    Friend WithEvents TBObjEntry As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents TBObjCount As TextBox
    Friend WithEvents Label25 As Label
    Friend WithEvents TBObjRange As TextBox
    Friend WithEvents Label26 As Label
    Friend WithEvents TBObjUseSpellId As TextBox
    Friend WithEvents ButtonNewXML As Button
    Friend WithEvents ButtonSaveNPC As Button
    Friend WithEvents ButtonQuestNew As Button
    Friend WithEvents ButtonQuestSave As Button
    Friend WithEvents TBQuestNeedQuestCompId As TextBox
    Friend WithEvents Label27 As Label
    Friend WithEvents ButtonObjectiveNew As Button
    Friend WithEvents ButtonObjectiveSave As Button
    Friend WithEvents LabelObjNPCIDorName As Label
    Friend WithEvents TBObjNPCId As TextBox
    Friend WithEvents Label30 As Label
    Friend WithEvents TBObjQuestName As TextBox
    Friend WithEvents Label29 As Label
    Friend WithEvents TBObjQuestID As TextBox
    Friend WithEvents CBObjIgnoreQuestCompleted As CheckBox
    Friend WithEvents CBInternalObj As CheckBox
    Friend WithEvents Label32 As Label
    Friend WithEvents Label34 As Label
    Friend WithEvents TBObjDestinationY As TextBox
    Friend WithEvents Label31 As Label
    Friend WithEvents TBObjDestinationX As TextBox
    Friend WithEvents Label33 As Label
    Friend WithEvents TBObjTaxiEntryId As TextBox
    Friend WithEvents CBObjInternalQuestID As ComboBox
    Friend WithEvents CBObjKillMobPickUpItem As CheckBox
    Friend WithEvents ButtonObjImportFromGame As Button
    Friend WithEvents ButtonOpenWowHead As Button
    Friend WithEvents ButtonQuestImportFromGame As Button
    Friend WithEvents CLBQuestClassMask As CheckedListBox
    Friend WithEvents CLBQuestRaceMask As CheckedListBox
    Friend WithEvents TBObjHotSpots As TextBox
    Friend WithEvents ButtonObjHotSpots As Button
    Friend WithEvents ButtonQuestAlliance As Button
    Friend WithEvents ButtonQuestHorde As Button
    Friend WithEvents CBObjCanPullUnitsInFight As CheckBox
    Friend WithEvents ButtonObjImportEntry As Button
    Friend WithEvents ButtonObjImportGPS As Button
    Friend WithEvents PanelObjTaxi As Panel
    Friend WithEvents PanelObjAll As Panel
    Friend WithEvents ButtonObjGetXY As Button
    Friend WithEvents Label6 As Label
    Friend WithEvents TBNpcContinentId As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents CBNpcType As ComboBox
    Friend WithEvents CBNpcFaction As ComboBox
    Friend WithEvents ButtonNpcImport As Button
    Friend WithEvents CBQuestAutoAccept As CheckBox
    Friend WithEvents Label28 As Label
    Friend WithEvents TBQuestWQ As TextBox
    Friend WithEvents CBQuestWQ As CheckBox
    Friend WithEvents UcXmlRichTextBox1 As CustomXmlViewer.ucXmlRichTextBox
    Friend WithEvents CBMainDisplayXML As CheckBox
    Friend WithEvents TBObjGossipOption As TextBox
    Friend WithEvents Label35 As Label
    Friend WithEvents ContextMenuStrip As ContextMenuStrip
    Friend WithEvents InsertUpToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents InsertDownToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents DeleteToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ContextMenuStripNeedQuest As ContextMenuStrip
    Friend WithEvents ToolStripMenuItemAddNeedQuestComp As ToolStripMenuItem
    Friend WithEvents TnbControlMenu1 As nManager.Helpful.Forms.UserControls.TnbControlMenu
    Friend WithEvents TBObjFlightWaitMs As TextBox
    Friend WithEvents Label36 As Label
    Friend WithEvents ButtonSaveXML As Button
    Friend WithEvents ButtonObjDumpIndex As Button
    Friend WithEvents Label37 As Label
    Friend WithEvents TBObjInternalIndex As TextBox
End Class
