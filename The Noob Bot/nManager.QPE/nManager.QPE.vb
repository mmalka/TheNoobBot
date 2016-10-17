Imports System.Xml
Imports System.Reflection
Imports nManager
Imports nManager.Helpful
Imports nManager.Wow.Helpers
Imports Quester
Imports nManager.Wow.ObjectManager.ObjectManager
Imports nManager.Wow.ObjectManager
Imports System.Windows.Forms.TreeNode
Imports System.ComponentModel
Imports System.IO
Public Class TnbProfileCreatorForm

    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        '  EventsListener.UnHookEvent(nManager.Wow.Enums.WoWEventsType.QUEST_COMPLETE, Sub() QuestFinished("d"), False)
        '  EventsListener.UnHookEvent(nManager.Wow.Enums.WoWEventsType.QUEST_DETAIL, Sub() QuestDetail("d"), False)
    End Sub

    Dim NPCParentNode As TreeNode
    Dim QuestParentNode As TreeNode

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' EventsListener.HookEvent(nManager.Wow.Enums.WoWEventsType.QUEST_DETAIL, Sub() QuestDetail("d"), False, True)
        ' EventsListener.HookEvent(nManager.Wow.Enums.WoWEventsType.QUEST_COMPLETE, Sub() QuestFinished("d"), False, True)

        'TODO Auto Create Taxi
        'Wow.Enums.WoWEventsType.ENABLE_TAXI_BENCHMARK
        'Wow.Enums.WoWEventsType.DISABLE_TAXI_BENCHMARK

        'TODO QUEST_LOOT_RECEIVED = 804 Auto QuestLoot
        'QUEST_PROGRESS = 181 Check si 
        'Dim machin As UInt32 = 0

        'machin = machin And Wow.Enums.WoWClassMask.Paladin And Wow.Enums.WoWClassMask.DemonHunter
        'If (machin And Wow.Enums.WoWClassMask.Paladin) > 0 Then

        'End If
        '        Int exp = (Int()) ObjectManager.Me.WowRace - 1;
        '            //exp = 1 - 1; // Human
        '            uint mBitRace = exp >= 0?(uint) Math.Pow(2, exp) : 0;
        '            exp = (Int()) ObjectManager.Me.WowClass - 1;
        '            //exp = 2 - 1; // Paladin
        '            uint mBitClass = exp >= 0?(uint) Math.Pow(2, exp) : 0;
        'VesperTV -Today at 1: 23 AM
        'For (Int() i = Quests.Count - 1; i >= 0; i--)
        '            {
        '                Quest Quest = Quests[i];
        '                If (Quest.ClassMask > 0 && (Quest.ClassMask & mBitClass) == 0 ||
        '                    (quest.RaceMask > 0 && (quest.RaceMask & mBitRace) == 0) ||
        '                    Quest.Gender!= (uint) WoWGender.Both && quest.Gender != (uint) ObjectManager.Me.WowGender)
        '                    Quests.Remove(Quest);
        '            }

        ' Dim questProfile As Quester.Profile.QuesterProfile = nManager.Helpful.XmlSerializer.Deserialize(Of Quester.Profile.QuesterProfile)("C:\Users\Arasal\Documents\TheNoobBot-6.3.2\TheNoobBot-6.3.2\Profiles\Quester\[A] 15-20 Redridge Mountains.xml")

        ''Dim myQuest As Quester.Profile.Quest = New Quester.Profile.Quest With {.Id = ""}

        ''questProfile.Quests.Add(myQuest)

        'For Each quest In questProfile.Quests

        '    Others.OpenWebBrowserOrApplication("http://www.wowhead.com/quest=" & quest.Id.ToString)
        '    For Each QuestO In quest.Objectives
        '        QuestO.IgnoreQuestCompleted = True


        '    Next

        '    '     Logging.Write("Id: " & quest.Id & ", Name:" & quest.Name)
        'Next
        'Dim questname As String = String.Empty


        'questname = Lua.LuaDoString("luaS = GetTitleText()", "luaS")
        'MessageBox.Show(questname)

        '  nManager.Helpful.XmlSerializer.Serialize("C:\Users\Arasal\Documents\TheNoobBot-6.3.2\TheNoobBot-6.3.2\Profiles\Quester\[A] 15-20 Redridge Mountains2.xml", questProfile)
        'nManager.Wow.Helpers.Usefuls.CloseAllBags()

        nManager.nManagerSetting.CurrentSetting.CanPullUnitsAlreadyInFight = False
        If nManager.nManagerSetting.CurrentSetting.CanPullUnitsAlreadyInFight Then
            MessageBox.Show("Please deactivate ""CanPullInitsAlreadyInFight"" in the Settings when Creating Profiles")

        End If

        fsize = Me.Size

        PopulateComboBox()

        PanelObjTaxi.Location = PanelObjAll.Location
        NPCParentNode = New TreeNode("NPCs")
        QuestParentNode = New TreeNode("Quests")
        If loadPath <> String.Empty Then
            LoadNodes()
        End If
    End Sub
    Dim loadPath As String

    Sub New(Optional path As String = "")

        ' This call is required by the designer.
        InitializeComponent()
        loadPath = path

        If nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature Then
            TopMost = True
        End If

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub



    Dim profile As Quester.Profile.QuesterProfile
    Private Sub ButtonLoadXML_Click(sender As Object, e As EventArgs) Handles ButtonLoadXML.Click
        ClearQuestForm()
        DisableObjForm()
        LoadNodes()

    End Sub

    Private Sub ButtonSaveXML_Click_1(sender As Object, e As EventArgs) Handles ButtonSaveXML.Click
        If profile IsNot Nothing Then
            If path <> String.Empty Then
                nManager.Helpful.XmlSerializer.Serialize(path, profile)
            Else
                MessageBox.Show("Please SaveAs your new profile first.")
            End If
        End If
    End Sub


    Private Sub ButtonSaveXML_Click(sender As Object, e As EventArgs) Handles ButtonSaveAsXML.Click
        '  profileXML.Save("C:\Users\Julien\Documents\Visual Studio 2013\Templates\TestTemplates\Test.xml")
        '  nManager.Helpful.XmlSerializer.Serialize("C:\Users\Arasal\Documents\TESTGen.xml", profile)
        If profile IsNot Nothing Then

            Try


                Dim ofd As New OpenFileDialog
                ofd.Filter = "Profile files (*.xml)|*.xml|All files (*.*)|*.*"
                ofd.ShowHelp = True
                ofd.InitialDirectory = Application.StartupPath & "\\Profiles\\Quester\\"
                ofd.Title = "Save Profile"
                '  ofd.OverwritePrompt = True
                '  ofd.CreatePrompt = True
                ofd.CheckFileExists = False
                ofd.SupportMultiDottedExtensions = True
                'Dim savePath As String = nManager.Helpful.Others.DialogBoxSaveFile(Application.StartupPath & "\\Profiles\\Quester\\", "Profile files (*.xml)|*.xml|All files (*.*)|*.*")

                'If savePath <> String.Empty Then
                '    nManager.Helpful.XmlSerializer.Serialize(savePath, profile)
                'End If


                If ofd.ShowDialog = DialogResult.OK Then
                    If File.Exists(ofd.FileName) Then
                        If MessageBox.Show("File Exists, Replace?", "", MessageBoxButtons.YesNo) = DialogResult.No Then
                            Exit Sub
                        End If
                    End If
                    path = ofd.FileName
                    nManager.Helpful.XmlSerializer.Serialize(path, profile)
                End If

            Catch ex As Exception
                MessageBox.Show("There was a profile saving the XML, check the syntax.")
            End Try
        End If
    End Sub

    Private Sub ButtonNewNPC_Click(sender As Object, e As EventArgs) Handles ButtonNewNPC.Click

        ClearNPCForm()
        TreeView.SelectedNode = Nothing
    End Sub

    Private Sub ButtonSaveNPC_Click(sender As Object, e As EventArgs) Handles ButtonSaveNPC.Click
        'DONE
        If TreeView.SelectedNode IsNot Nothing AndAlso TreeView.SelectedNode.Tag <> "NPCs" Then
            'NPC Modification

            Dim quester As Wow.Class.Npc = profile.Questers(lastSelectedNpc.Index)

            With quester
                .Name = TBNpcName.Text
                .Entry = TBNpcId.Text
                .Position = New Wow.Class.Point(TBNpcPosition.Text.Split(";"c).ElementAt(0).Trim, TBNpcPosition.Text.Split(";"c).ElementAt(1), TBNpcPosition.Text.Split(";"c).ElementAt(2))
                .Faction = CBNpcFaction.SelectedValue
                .Type = CBNpcType.SelectedValue
                .ContinentId = TBNpcContinentId.Text

            End With

            TreeView.SelectedNode.Text = TBNpcName.Text

        Else
            'New NPC

            Dim newQuester As New Wow.Class.Npc



            With newQuester
                .Name = TBNpcName.Text
                .Entry = TBNpcId.Text
                .Position = New Wow.Class.Point(TBNpcPosition.Text.Split(";"c).ElementAt(0).Trim, TBNpcPosition.Text.Split(";"c).ElementAt(1), TBNpcPosition.Text.Split(";"c).ElementAt(2))
                .Faction = CBNpcFaction.SelectedValue
                .Type = CBNpcType.SelectedValue
                .ContinentId = TBNpcContinentId.Text

            End With

            profile.Questers.Add(newQuester)

            Dim NpcNode As TreeNode = New TreeNode(TBNpcName.Text)

            NpcNode.Tag = "NPC"
            NPCParentNode.Nodes.Add(NpcNode)
            TreeView.SelectedNode = NpcNode

        End If
    End Sub

    Private Sub ButtonNpcImport_Click(sender As Object, e As EventArgs) Handles ButtonNpcImport.Click





        If Target.IsValid AndAlso QuestersDB.GetNpcByEntry(Target.Entry).Entry = 0 Then

            If Target.IsValid Then

                TBNpcName.Text = Target.Name
                TBNpcId.Text = Target.Entry
                TBNpcPosition.Text = Target.Position.ToString
                CBNpcFaction.SelectedValue = Target.Faction

                If (ObjectManager.Target.GetDescriptor(Of nManager.Wow.Enums.UnitNPCFlags)(Wow.Patchables.Descriptors.UnitFields.NpcFlags).HasFlag(nManager.Wow.Enums.UnitNPCFlags.QuestGiver)) Then
                    CBNpcType.SelectedValue = Wow.Class.Npc.NpcType.QuestGiver
                Else
                    CBNpcType.SelectedValue = Wow.Class.Npc.NpcType.FlightMaster
                End If

                TBNpcContinentId.Text = Usefuls.ContinentNameByContinentId(Usefuls.ContinentId)

            End If
        Else

            Dim wowGO As WoWGameObject = GetNearestWoWGameObject(GetWoWGameObjectOfType(nManager.Wow.Enums.WoWGameObjectType.Questgiver))

            If wowGO.Entry > 0 AndAlso [Me].Position.DistanceTo(wowGO.Position) < 5.0F AndAlso QuestersDB.GetNpcByEntry(wowGO.Entry).Entry = 0 Then

                TBNpcName.Text = wowGO.Name
                TBNpcId.Text = wowGO.Entry
                TBNpcPosition.Text = wowGO.Position.ToString
                CBNpcFaction.SelectedValue = wowGO.Faction
                CBNpcType.SelectedValue = Wow.Class.Npc.NpcType.QuestGiver
                TBNpcContinentId.Text = Usefuls.ContinentNameByContinentId(Usefuls.ContinentId)
            End If

        End If

    End Sub


    Private Sub ButtonObjectiveNew_Click(sender As Object, e As EventArgs) Handles ButtonObjectiveNew.Click
        DisableObjForm()
        TreeView.SelectedNode = Nothing
    End Sub

    Private Sub ButtonObjectiveSave_Click(sender As Object, e As EventArgs) Handles ButtonObjectiveSave.Click
        Try



            If TreeView.SelectedNode IsNot Nothing AndAlso (TreeView.SelectedNode.Tag = "Objective" OrElse TreeView.SelectedNode.Tag = "NewObjective") Then
                'Modification


                Dim objective = profile.Quests.ElementAt(lastSelectedObjective.Parent.Index).Objectives.ElementAt(lastSelectedObjective.Index)

                With objective

                    If TreeView.SelectedNode.Tag = "NewObjective" Then

                        Dim cbObjSelValue As Integer
                        If IsNumeric(CBObjType.SelectedValue) Then
                            cbObjSelValue = CBObjType.SelectedValue
                        Else
                            cbObjSelValue = CType(CBObjType.SelectedValue, ComboBoxValueString).Value
                        End If
                        .Objective = cbObjSelValue
                        TreeView.SelectedNode.Tag = "Objective"
                        TreeView.SelectedNode.Text = .Objective.ToString & " " & .QuestName
                    End If

                    Select Case .Objective.ToString
                        Case "KillMob"

                            If .CollectItemId > 0 OrElse CBObjKillMobPickUpItem.Checked Then
                                TBObjCollectCount.Enabled = True
                                TBObjCollectItemID.Enabled = True
                                .CollectCount = TBObjCollectCount.Text
                                .CollectItemId = TBObjCollectItemID.Text
                                .Count = 0

                            Else
                                .Count = TBObjCount.Text
                                .CollectCount = 0
                                .CollectItemId = 0
                            End If


                            .CanPullUnitsAlreadyInFight = CBObjCanPullUnitsInFight.Checked

                        Case "BuyItem"

                            .CollectCount = TBObjCollectCount.Text
                            .CollectItemId = TBObjCollectItemID.Text
                            .Name = TBObjNPCId.Text
                            .Position = New Wow.Class.Point(TBObjPosition.Text.Split(";"c).ElementAt(0).Trim, TBObjPosition.Text.Split(";"c).ElementAt(1), TBObjPosition.Text.Split(";"c).ElementAt(2))

                        Case "PickUpObject"

                            .CollectCount = TBObjCollectCount.Text
                            .CollectItemId = TBObjCollectItemID.Text

                        Case "UseItem"

                            .UseItemId = TBObjUseItemID.Text
                            .Count = TBObjCount.Text
                            .Position = New Wow.Class.Point(TBObjPosition.Text.Split(";"c).ElementAt(0).Trim, TBObjPosition.Text.Split(";"c).ElementAt(1), TBObjPosition.Text.Split(";"c).ElementAt(2))


                        Case "UseSpell"
                            'TODO ENTRY + NAME + POSITION
                            .Position = New Wow.Class.Point(TBObjPosition.Text.Split(";"c).ElementAt(0).Trim, TBObjPosition.Text.Split(";"c).ElementAt(1), TBObjPosition.Text.Split(";"c).ElementAt(2))


                            .UseSpellId = TBObjUseSpellId.Text
                            .WaitMs = TBObjWaitMs.Text

                        Case "UseSpellAOE"
                            .Position = New Wow.Class.Point(TBObjPosition.Text.Split(";"c).ElementAt(0).Trim, TBObjPosition.Text.Split(";"c).ElementAt(1), TBObjPosition.Text.Split(";"c).ElementAt(2))


                            .UseSpellId = TBObjUseSpellId.Text
                            .Range = TBObjRange.Text
                            .WaitMs = TBObjWaitMs.Text

                        Case "InteractWith"
                            .Position = New Wow.Class.Point(TBObjPosition.Text.Split(";"c).ElementAt(0).Trim, TBObjPosition.Text.Split(";"c).ElementAt(1), TBObjPosition.Text.Split(";"c).ElementAt(2))
                            .GossipOptionsInteractWith = IIf(IsNumeric(TBObjGossipOption.Text), TBObjGossipOption.Text, 0)
                            .WaitMs = IIf(IsNumeric(TBObjWaitMs.Text), TBObjWaitMs.Text, 0)

                        Case "MoveTo"
                            .Position = New Wow.Class.Point(TBObjPosition.Text.Split(";"c).ElementAt(0).Trim, TBObjPosition.Text.Split(";"c).ElementAt(1), TBObjPosition.Text.Split(";"c).ElementAt(2))


                        Case "Wait"
                            .WaitMs = TBObjWaitMs.Text
                        Case "PickUpQuest"

                            .NpcEntry = TBObjNPCId.Text
                            .QuestId = TBObjQuestID.Text
                            .QuestName = TBObjQuestName.Text

                        Case "TurnInQuest"
                            .NpcEntry = TBObjNPCId.Text
                            .QuestId = TBObjQuestID.Text
                            .QuestName = TBObjQuestName.Text

                        Case "UseFlightPath"
                            .TaxiEntry = TBObjTaxiEntryId.Text
                            .FlightDestinationX = TBObjDestinationX.Text
                            .FlightDestinationY = TBObjDestinationY.Text
                            .WaitMs = TBObjFlightWaitMs.Text

                        Case "PickUpNPC"
                            .Count = TBObjCount.Text
                            .CanPullUnitsAlreadyInFight = CBObjCanPullUnitsInFight.Checked
                            .GossipOptionsInteractWith = IIf(IsNumeric(TBObjGossipOption.Text), TBObjGossipOption.Text, 0)

                        Case "UseVehicle"
                            .EntryVehicle = TBObjEntry.Text
                            .Position = New Wow.Class.Point(TBObjPosition.Text.Split(";"c).ElementAt(0).Trim, TBObjPosition.Text.Split(";"c).ElementAt(1), TBObjPosition.Text.Split(";"c).ElementAt(2))

                        Case "ClickOnTerrain"
                            .Position = New Wow.Class.Point(TBObjPosition.Text.Split(";"c).ElementAt(0).Trim, TBObjPosition.Text.Split(";"c).ElementAt(1), TBObjPosition.Text.Split(";"c).ElementAt(2))
                            .WaitMs = TBObjWaitMs.Text

                    End Select

                    If CBInternalObj.Checked = True Then

                        If .InternalQuestId = 0 Then 'If modify obj as internal quest
                            .InternalQuestId = CBObjInternalQuestID.SelectedValue

                        End If

                    Else
                        .InternalQuestId = 0
                    End If

                    If IsNumeric(TBObjInternalIndex.Text) Then
                        .InternalIndex = TBObjInternalIndex.Text
                    End If

                    .IgnoreQuestCompleted = CBObjIgnoreQuestCompleted.Checked

                    .Entry.Clear()
                    For Each st In TBObjEntry.Lines
                        .Entry.Add(CType(st, Integer))
                    Next

                    .Hotspots.Clear()
                    For Each st In TBObjHotSpots.Lines
                        .Hotspots.Add(New Wow.Class.Point(st.Split(";"c).ElementAt(0).Trim, st.Split(";"c).ElementAt(1), st.Split(";"c).ElementAt(2)))
                    Next

                    If displayXML Then
                        DisplayXMLs(objective)
                    End If

                End With
            ElseIf lastSelectedQuest IsNot Nothing Then
                'New Objective

                Dim cbObjSelValue As Integer
                If IsNumeric(CBObjType.SelectedValue) Then
                    cbObjSelValue = CBObjType.SelectedValue
                Else
                    cbObjSelValue = CType(CBObjType.SelectedValue, ComboBoxValueString).Value
                End If



                Dim newObjective As Quester.Profile.QuestObjective = New Quester.Profile.QuestObjective
                newObjective.Objective = cbObjSelValue

                With newObjective
                    Select Case .Objective.ToString
                        Case "KillMob"

                            If CBObjKillMobPickUpItem.Checked Then

                                .CollectCount = TBObjCollectCount.Text
                                .CollectItemId = TBObjCollectItemID.Text

                            Else
                                .Count = TBObjCount.Text
                            End If

                            .CanPullUnitsAlreadyInFight = CBObjCanPullUnitsInFight.Checked

                        Case "BuyItem"

                            .CollectCount = TBObjCollectCount.Text
                            .CollectItemId = TBObjCollectItemID.Text
                            .Name = TBObjNPCId.Text
                            .Position = New Wow.Class.Point(TBObjPosition.Text.Split(";"c).ElementAt(0).Trim, TBObjPosition.Text.Split(";"c).ElementAt(1), TBObjPosition.Text.Split(";"c).ElementAt(2))

                        Case "PickUpObject"
                            .CollectCount = TBObjCollectCount.Text
                            .CollectItemId = TBObjCollectItemID.Text

                        Case "UseItem"
                            .UseItemId = TBObjUseItemID.Text
                            .Count = TBObjCount.Text
                            .Position = New Wow.Class.Point(TBObjPosition.Text.Split(";"c).ElementAt(0).Trim, TBObjPosition.Text.Split(";"c).ElementAt(1), TBObjPosition.Text.Split(";"c).ElementAt(2))

                        Case "UseSpell"
                            'TODO ENTRY + NAME + POSITION

                            .Position = New Wow.Class.Point(TBObjPosition.Text.Split(";"c).ElementAt(0).Trim, TBObjPosition.Text.Split(";"c).ElementAt(1), TBObjPosition.Text.Split(";"c).ElementAt(2))
                            .UseSpellId = TBObjUseSpellId.Text
                            .WaitMs = TBObjWaitMs.Text

                        Case "UseSpellAOE"

                            .Position = New Wow.Class.Point(TBObjPosition.Text.Split(";"c).ElementAt(0).Trim, TBObjPosition.Text.Split(";"c).ElementAt(1), TBObjPosition.Text.Split(";"c).ElementAt(2))
                            .UseSpellId = TBObjUseSpellId.Text
                            .Range = TBObjRange.Text
                            .WaitMs = TBObjWaitMs.Text

                        Case "InteractWith"
                            .Position = New Wow.Class.Point(TBObjPosition.Text.Split(";"c).ElementAt(0).Trim, TBObjPosition.Text.Split(";"c).ElementAt(1), TBObjPosition.Text.Split(";"c).ElementAt(2))
                            .GossipOptionsInteractWith = IIf(IsNumeric(TBObjGossipOption.Text), TBObjGossipOption.Text, 0)
                            .WaitMs = IIf(IsNumeric(TBObjWaitMs.Text), TBObjWaitMs.Text, 0)
                            .Entry.Add(TBObjEntry.Text)

                        Case "MoveTo"
                            .Position = New Wow.Class.Point(TBObjPosition.Text.Split(";"c).ElementAt(0).Trim, TBObjPosition.Text.Split(";"c).ElementAt(1), TBObjPosition.Text.Split(";"c).ElementAt(2))


                        Case "Wait"
                            .WaitMs = TBObjWaitMs.Text
                        Case "PickUpQuest"
                            .NpcEntry = TBObjNPCId.Text
                            .QuestId = TBObjQuestID.Text
                            .QuestName = TBObjQuestName.Text




                        Case "TurnInQuest"
                            .NpcEntry = TBObjNPCId.Text
                            .QuestId = TBObjQuestID.Text
                            .QuestName = TBObjQuestName.Text

                        Case "UseFlightPath"
                            .TaxiEntry = TBObjTaxiEntryId.Text
                            .FlightDestinationX = TBObjDestinationX.Text
                            .FlightDestinationY = TBObjDestinationY.Text
                            .WaitMs = TBObjFlightWaitMs.Text
                        Case "PickUpNPC"

                            .Count = TBObjCount.Text
                            .CanPullUnitsAlreadyInFight = CBObjCanPullUnitsInFight.Checked
                            .GossipOptionsInteractWith = IIf(IsNumeric(TBObjGossipOption.Text), TBObjGossipOption.Text, 0)

                        Case "UseVehicle"
                            .EntryVehicle = TBObjEntry.Text
                            .Position = New Wow.Class.Point(TBObjPosition.Text.Split(";"c).ElementAt(0).Trim, TBObjPosition.Text.Split(";"c).ElementAt(1), TBObjPosition.Text.Split(";"c).ElementAt(2))
                        Case "ClickOnTerrain"
                            .Position = New Wow.Class.Point(TBObjPosition.Text.Split(";"c).ElementAt(0).Trim, TBObjPosition.Text.Split(";"c).ElementAt(1), TBObjPosition.Text.Split(";"c).ElementAt(2))
                            .WaitMs = TBObjWaitMs.Text

                    End Select
                    If .Objective.ToString <> "UseVehicle" Then
                        .Entry.Clear()
                        For Each st In TBObjEntry.Lines
                            .Entry.Add(CType(st, Integer))
                        Next
                    End If

                    .Hotspots.Clear()
                    For Each st In TBObjHotSpots.Lines
                        .Hotspots.Add(New Wow.Class.Point(st.Split(";"c).ElementAt(0).Trim, st.Split(";"c).ElementAt(1), st.Split(";"c).ElementAt(2)))

                    Next

                    If CBInternalObj.Checked = True Then
                        .InternalQuestId = CBObjInternalQuestID.SelectedValue

                    End If

                    If IsNumeric(TBObjInternalIndex.Text) Then
                        .InternalIndex = TBObjInternalIndex.Text
                    End If

                    .IgnoreQuestCompleted = CBObjIgnoreQuestCompleted.Checked



                End With

                Dim lastSelQuestx As Quester.Profile.Quest = profile.Quests.ElementAt(lastSelectedQuest.Index)
                'If lastSelectedQuest.Nodes.Count = 0 Then
                '    lastQuestxElem.Element("Objectives").Add(newObjective)
                '    lastQuestxElem.Add(newObjective)
                'Else
                '    lastQuestxElem.Elements("Objectives").Elements("QuestObjective").ElementAt(lastQuestxElem.Elements("Objectives").Elements("QuestObjective").Count - 1).AddAfterSelf(newObjective)

                'End If
                lastSelQuestx.Objectives.Add(newObjective)

                Dim objectiveNode As TreeNode = New TreeNode(newObjective.Objective.ToString)

                objectiveNode.Tag = "Objective"
                lastSelectedQuest.Nodes.Add(objectiveNode)
                TreeView.SelectedNode = objectiveNode

                If displayXML Then
                    DisplayXMLs(newObjective)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Make sure the form is filled with numbers." & vbCrLf & "Position/HostSpots ex: X;Y;Z")
        End Try

    End Sub


    Function GetNextInternalId(questID As Integer) As Integer
        Dim idx As Integer = 0

        For Each obj In profile.Quests.ElementAt(lastSelectedQuest.Index).Objectives
            If obj.InternalQuestId = questID Then
                idx = 1
            End If
        Next
        Return idx + 1
    End Function

    Private Sub ButtonQuestNew_Click(sender As Object, e As EventArgs) Handles ButtonQuestNew.Click
        ClearQuestForm()
        TreeView.SelectedNode = Nothing

    End Sub

    Private Sub ButtonQuestSave_Click(sender As Object, e As EventArgs) Handles ButtonQuestSave.Click


        If profile IsNot Nothing Then


                If TreeView.SelectedNode IsNot Nothing AndAlso TreeView.SelectedNode.Tag = "Quest" Then
                    'Modification


                    Dim Quest = profile.Quests.ElementAt(TreeView.SelectedNode.Index)


                With Quest
                        .Name = TBQuestQuestName.Text
                        .Id = IIf(IsNumeric(TBQuestID.Text), TBQuestID.Text, 0)

                    Dim raceMask As Integer = 0
                        For Each item As ComboBoxValue In CLBQuestRaceMask.CheckedItems
                            raceMask += item.Value
                        Next

                        .RaceMask = raceMask

                        Dim classMask As Integer = 0
                        For Each item As ComboBoxValue In CLBQuestClassMask.CheckedItems
                            classMask += item.Value
                        Next

                        .ClassMask = classMask

                        .QuestLevel = IIf(IsNumeric(TBQuestLevel.Text), TBQuestLevel.Text, 0)
                        .MinLevel = IIf(IsNumeric(TBQuestMinLvl.Text), TBQuestMinLvl.Text, 0)
                        .MaxLevel = IIf(IsNumeric(TBQuestMaxLvl.Text), TBQuestMaxLvl.Text, 0)

                        .AutoAccepted = CBQuestAutoAccept.Checked

                        .NeedQuestCompletedId.Clear()
                        If TBQuestNeedQuestCompId.Lines.Count > 0 Then
                            For Each qInt In TBQuestNeedQuestCompId.Lines
                                If qInt <> String.Empty AndAlso IsNumeric(qInt) Then

                                    .NeedQuestCompletedId.Add(qInt)

                                End If
                            Next

                        End If

                        If CBQuestWQ.Checked Then
                            .WorldQuestLocation = New Wow.Class.Point(TBQuestWQ.Text.Split(";"c).ElementAt(0).Trim, TBQuestWQ.Text.Split(";"c).ElementAt(1), TBQuestWQ.Text.Split(";"c).ElementAt(2))

                        Else
                            If CheckBoxItemPickUp.Checked Then
                                .ItemPickUp = IIf(IsNumeric(TBQuestPickUpID.Text), TBQuestPickUpID.Text, 0) 'IF quest started by an item
                            Else
                                .PickUp = IIf(IsNumeric(TBQuestPickUpID.Text), TBQuestPickUpID.Text, 0)
                            End If

                            .TurnIn = IIf(IsNumeric(TBQuestTurnInID.Text), TBQuestTurnInID.Text, 0)
                        End If


                        If displayXML Then
                            DisplayXMLs(Quest)
                        End If

                        '  profile.Quests.Add(Quest)

                        TreeView.SelectedNode.Text = TBQuestQuestName.Text


                    End With
                Else
                    'New Quest
                    Try


                        Dim newQuest As New Quester.Profile.Quest

                        With newQuest
                            .Name = TBQuestQuestName.Text
                            .Id = IIf(IsNumeric(TBQuestID.Text), TBQuestID.Text, 0)

                            Dim raceMask As Integer = 0
                            For Each item As ComboBoxValue In CLBQuestRaceMask.CheckedItems
                                raceMask += item.Value
                            Next

                            .RaceMask = raceMask

                            Dim classMask As Integer = 0
                            For Each item As ComboBoxValue In CLBQuestClassMask.CheckedItems
                                classMask += item.Value
                            Next

                            .ClassMask = classMask

                            .QuestLevel = IIf(IsNumeric(TBQuestLevel.Text), TBQuestLevel.Text, 0)
                            .MinLevel = IIf(IsNumeric(TBQuestMinLvl.Text), TBQuestMinLvl.Text, 0)
                            .MaxLevel = IIf(IsNumeric(TBQuestMaxLvl.Text), TBQuestMaxLvl.Text, 0)


                            .AutoAccepted = CBQuestAutoAccept.Checked

                            If TBQuestNeedQuestCompId.Lines.Count > 0 Then

                                For Each qInt In TBQuestNeedQuestCompId.Lines
                                    If qInt <> String.Empty AndAlso IsNumeric(qInt) Then

                                        .NeedQuestCompletedId.Add(qInt)

                                    End If
                                Next
                            Else
                                .NeedQuestCompletedId.Clear()
                            End If


                            If CBQuestWQ.Checked Then
                                .WorldQuestLocation = New Wow.Class.Point(TBQuestWQ.Text.Split(";"c).ElementAt(0).Trim, TBQuestWQ.Text.Split(";"c).ElementAt(1), TBQuestWQ.Text.Split(";"c).ElementAt(2))
                            Else
                                If CheckBoxItemPickUp.Checked Then
                                    .ItemPickUp = IIf(IsNumeric(TBQuestPickUpID.Text), TBQuestPickUpID.Text, 0) 'IF quest started by an item
                                Else
                                    .PickUp = IIf(IsNumeric(TBQuestPickUpID.Text), TBQuestPickUpID.Text, 0)
                                End If

                                .TurnIn = IIf(IsNumeric(TBQuestTurnInID.Text), TBQuestTurnInID.Text, 0)
                            End If

                            'If QuestParentNode.Nodes.Count = 0 Then
                            '    profileXML.Element("Quests").Add(newQuest)
                            'Else
                            '    profileXML.Elements("Quests").Elements("Quest").ElementAt(profileXML.Elements("Quests").Elements.Count - 1).AddAfterSelf(newQuest)
                            'End If

                            profile.Quests.Add(newQuest)

                            Dim questNode As New TreeNode(TBQuestQuestName.Text)
                            questNode.Tag = "Quest"

                            QuestParentNode.Nodes.Add(questNode)
                            TreeView.SelectedNode = questNode

                        End With
                        If displayXML Then
                            DisplayXMLs(newQuest)
                        End If
                    Catch ex As Exception
                        MessageBox.Show("Please make sure that the fields are filled with numbers!")
                    End Try
                End If
            End If

    End Sub






    Private Sub ButtonNewXML_Click(sender As Object, e As EventArgs) Handles ButtonNewXML.Click
        TreeView.Nodes.Clear()
        NPCParentNode.Nodes.Clear()
        QuestParentNode.Nodes.Clear()
        NPCParentNode.Tag = "NPCs"
        QuestParentNode.Tag = "Quests"
        ClearNPCForm()
        ClearQuestForm()

        lastSelectedNpc = Nothing
        lastSelectedObjective = Nothing
        lastSelectedQuest = Nothing

        TreeView.Nodes.Add(NPCParentNode)
        TreeView.Nodes.Add(QuestParentNode)
        ' NPCParentNode.Expand()
        QuestParentNode.Expand()
        path = False
        profile = New Quester.Profile.QuesterProfile

    End Sub
    Dim path As String
    Public Sub LoadNodes()
        TreeView.Nodes.Clear()
        NPCParentNode.Nodes.Clear()
        QuestParentNode.Nodes.Clear()

        Dim ofd As New OpenFileDialog
        ofd.Filter = "Profile files (*.xml)|*.xml|All files (*.*)|*.*"
        ofd.ShowHelp = True
        ofd.InitialDirectory = Application.StartupPath & "\\Profiles\\Quester\\"

        Try


            If loadPath <> String.Empty OrElse ofd.ShowDialog = DialogResult.OK Then
                If loadPath <> String.Empty Then
                    path = loadPath
                    loadPath = String.Empty
                Else
                    path = ofd.FileName
                End If

                'profileXML = XElement.Load("C:\Users\Arasal\Documents\Quester Profile Creator\Quester Profile Creator\h1-5.xml")
                '  profileXML = XElement.Load("C:\Users\Arasal\Documents\Quester Profile Creator\Quester Profile Creator\q.xml")

                ' profileXML = XElement.Load("C:\Users\Julien\Documents\Visual Studio 2013\Templates\TestTemplates\Quester Profile Creator\h1-5.xml")

                ' profileXML = XElement.Load("C:\Users\Julien\Documents\Visual Studio 2013\Templates\TestTemplates\Quester Profile Creator\q.xml")

                'profile = nManager.Helpful.XmlSerializer.Deserialize(Of Quester.Profile.QuesterProfile)("C:\Users\Arasal\Documents\TheNoobBot-6.3.2\TheNoobBot-6.3.2\Profiles\Quester\TESTGen.xml")
                profile = nManager.Helpful.XmlSerializer.Deserialize(Of Quester.Profile.QuesterProfile)(path)



                NPCParentNode.Tag = "NPCs"
                TreeView.Nodes.Add(NPCParentNode)

                For Each vQuester In profile.Questers
                    Dim npcNode As TreeNode = New TreeNode(vQuester.Name)
                    npcNode.Tag = "NPC"
                    NPCParentNode.Nodes.Add(npcNode)
                Next

                QuestParentNode.Tag = "Quests"
                TreeView.Nodes.Add(QuestParentNode)
                For Each quest In profile.Quests
                    'QUEST
                    Dim QuestNode As TreeNode = New TreeNode(quest.Name & " (" & quest.Id & ")")
                    QuestNode.Tag = "Quest"
                    QuestParentNode.Nodes.Add(QuestNode)

                    'QUEST OBJECTIVES
                    For Each questObjective In quest.Objectives
                        'If questObjective.Element("Objective")

                        Dim questObjectiveNode As TreeNode
                        'If questObjective.Objective = Quester.Profile.Objective.PickUpQuest Or questObjective.Objective = Quester.Profile.Objective.TurnInQuest Then
                        questObjectiveNode = New TreeNode(questObjective.Objective.ToString & " " & questObjective.QuestName)
                        ' Else
                        '    questObjectiveNode = New TreeNode(questObjective.Objective.ToString)
                        'End If

                        questObjectiveNode.Tag = "Objective"
                        QuestNode.Nodes.Add(questObjectiveNode)

                    Next questObjective

                Next quest

                ' NPCParentNode.Expand()
                QuestParentNode.Expand()






            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Dim selectedQuest As XElement

    Private Sub TreeView_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TreeView.AfterSelect
        If e.Node.Tag = "NPCs" Then
            PanelNPC.Visible = True
            PanelSimpleQuest.Visible = False
        ElseIf e.Node.Tag = "Quests" Then
            PanelNPC.Visible = False
            PanelSimpleQuest.Visible = True


        ElseIf e.Node.Tag = "NPC" Then
            PanelNPC.Visible = True
            PanelSimpleQuest.Visible = False

            Dim vquester As Wow.Class.Npc = profile.Questers(e.Node.Index)

            With vquester
                TBNpcName.Text = .Name
                TBNpcId.Text = .Entry
                TBNpcPosition.Text = .Position.X & ";" & .Position.Y & ";" & .Position.Z
                CBNpcFaction.SelectedValue = CType(String.Format("{0:D}", .Faction), Integer)
                CBNpcType.SelectedValue = CType(String.Format("{0:D}", .Type), Integer)
                TBNpcContinentId.Text = .ContinentId
            End With
            lastSelectedNpc = e.Node
        ElseIf e.Node.Tag = "Quest" Then

            DisableObjForm()
            ClearQuestForm()

            PanelNPC.Visible = False
            PanelSimpleQuest.Visible = True
            TabControl1.SelectedTab = TabPageQuest


            'selectedQuest = quest

            Dim quest = profile.Quests.ElementAt(e.Node.Index)

            With quest
                TBQuestQuestName.Text = .Name
                TBQuestID.Text = .Id
                FillQuestMaskAfterObjSelection(quest.ClassMask, quest.RaceMask)


                If .NeedQuestCompletedId.Count > 0 Then
                    Dim intList As New List(Of String)
                    For Each qInt In .NeedQuestCompletedId
                        intList.Add(qInt)


                    Next
                    TBQuestNeedQuestCompId.Lines = intList.ToArray
                End If

                CBQuestAutoAccept.Checked = .AutoAccepted

                TBQuestLevel.Text = .QuestLevel
                TBQuestMinLvl.Text = .MinLevel
                TBQuestMaxLvl.Text = .MaxLevel

                If .WorldQuestLocation IsNot Nothing Then
                    TBQuestWQ.Text = .WorldQuestLocation.ToString
                    CBQuestWQ.Checked = True
                Else
                    If .ItemPickUp <> 0 Then
                        CheckBoxItemPickUp.Checked = True
                        TBQuestPickUpID.Text = .ItemPickUp
                    Else
                        CheckBoxItemPickUp.Checked = False
                        TBQuestPickUpID.Text = .PickUp
                    End If

                    TBQuestTurnInID.Text = .TurnIn
                End If

            End With
            lastSelectedQuest = e.Node

            'If displayXML Then
            '    Dim xmldoc As New XmlDocument
            '    xmldoc.Load(path)

            '    Dim newNPC As XElement = XElement.Load(New XmlNodeReader(xmldoc)).Element("Quests").Elements("Quest").ElementAt(e.Node.Index)
            '    UcXmlRichTextBox1.Xml = newNPC.ToString
            'End If

            'Dim ser As New Xml.Serialization.XmlSerializer(quest.GetType)
            'Dim sww As New StringWriter
            'Using writer As XmlWriter = XmlWriter.Create(sww)
            '    ser.Serialize(writer, quest)
            '    UcXmlRichTextBox1.Xml = sww.ToString
            'End Using
            If displayXML Then
                DisplayXMLs(quest)
                'Dim xmldoc As New XmlDocument
                'Dim ser As New Xml.Serialization.XmlSerializer(quest.GetType)
                'Dim sww As New StringWriter
                'Using writer As XmlWriter = XmlWriter.Create(sww)
                '    ser.Serialize(writer, quest)
                '    xmldoc.LoadXml(sww.ToString)
                'End Using

                'Dim questXML As XElement = XElement.Load(New XmlNodeReader(xmldoc))
                'questXML.Attributes.Remove
                'UcXmlRichTextBox1.Xml = questXML.ToString

            End If


        ElseIf e.Node.Tag = "Objective" Or e.Node.Tag = "NewObjective" Then 'Objective selection

            Dim QuestObjective As Profile.QuestObjective = profile.Quests.ElementAt(e.Node.Parent.Index).Objectives.ElementAt(e.Node.Index) 'Idx node objective, idx node quest (quest parent of objective)
            TabControl1.SelectedTab = TabPageObjectives
            lastSelectedObjective = e.Node
            lastSelectedQuest = e.Node.Parent

            If displayXML Then
                DisplayXMLs(QuestObjective)
            End If
            FillObjectiveFormByType(QuestObjective, profile.Quests.ElementAt(e.Node.Parent.Index))



            'MessageBox.Show(FindQuestObjectiveByIndex(e.Node.Index, questFound.Element("Name")).Element("Objective"))

        End If
    End Sub
    Dim lastSelectedQuest As TreeNode
    Dim lastSelectedObjective As TreeNode
    Dim lastSelectedNpc As TreeNode

    Sub DisplayXMLs(objet As Object)
        Dim xmldoc As New XmlDocument
        Dim ser As New Xml.Serialization.XmlSerializer(objet.GetType)
        Dim sww As New StringWriter
        Using writer As XmlWriter = XmlWriter.Create(sww)
            ser.Serialize(writer, objet)
            xmldoc.LoadXml(sww.ToString)
        End Using

        Dim questXML As XElement = XElement.Load(New XmlNodeReader(xmldoc))
        questXML.Attributes.Remove
        UcXmlRichTextBox1.Xml = questXML.ToString
    End Sub
    Sub DisableObjForm()
        TBObjCount.Enabled = False
        TBObjEntry.Enabled = False
        TBObjCollectItemID.Enabled = False
        TBObjCollectCount.Enabled = False
        TBObjUseItemID.Enabled = False
        TBObjPosition.Enabled = False
        TBObjWaitMs.Enabled = False
        TBObjRange.Enabled = False
        TBObjUseSpellId.Enabled = False
        TBObjNPCId.Enabled = False
        TBObjQuestID.Enabled = False
        TBObjQuestName.Enabled = False
        CBObjIgnoreQuestCompleted.Checked = False
        TBObjDestinationX.Enabled = False
        TBObjDestinationY.Enabled = False
        TBObjTaxiEntryId.Enabled = False
        CBObjInternalQuestID.Enabled = False
        CBObjKillMobPickUpItem.Visible = False
        CBObjCanPullUnitsInFight.Enabled = False
        CBInternalObj.Enabled = False
        TBObjGossipOption.Enabled = False

        RemoveHandler CBInternalObj.CheckedChanged, AddressOf CBInternalObj_CheckedChanged
        CBInternalObj.Checked = False
        AddHandler CBInternalObj.CheckedChanged, AddressOf CBInternalObj_CheckedChanged


        TBObjCount.Text = String.Empty
        TBObjEntry.Text = String.Empty
        TBObjCollectItemID.Text = String.Empty
        TBObjCollectCount.Text = String.Empty
        TBObjUseItemID.Text = String.Empty
        TBObjPosition.Text = String.Empty
        TBObjWaitMs.Text = String.Empty
        TBObjRange.Text = String.Empty
        TBObjUseSpellId.Text = String.Empty
        TBObjNPCId.Text = String.Empty
        TBObjQuestID.Text = String.Empty
        TBObjQuestName.Text = String.Empty
        TBObjDestinationX.Text = String.Empty
        TBObjDestinationY.Text = String.Empty
        TBObjTaxiEntryId.Text = String.Empty
        CBObjInternalQuestID.DataSource = Nothing
        CBObjCanPullUnitsInFight.Checked = False
        LabelObjNPCIDorName.Text = "NPC Id"
        TBObjHotSpots.Clear()
        CBInternalObj.Checked = False
        TBObjGossipOption.Text = String.Empty
        TBObjInternalIndex.Text = String.Empty
        RemoveHandler CBObjKillMobPickUpItem.CheckedChanged, AddressOf CBObjKillMobPickUpItem_CheckedChanged
        CBObjKillMobPickUpItem.Checked = False
        AddHandler CBObjKillMobPickUpItem.CheckedChanged, AddressOf CBObjKillMobPickUpItem_CheckedChanged
    End Sub

    Sub ClearObjectiveForm()




    End Sub
    Sub ClearQuestForm()

        ClearMaskListBox()

        TBQuestQuestName.Text = String.Empty
        TBQuestID.Text = String.Empty
        CBClassMask.SelectedValue = 0
        CBRaceMask.SelectedValue = 0
        TBQuestLevel.Text = String.Empty
        TBQuestMinLvl.Text = String.Empty
        TBQuestMaxLvl.Text = String.Empty
        CheckBoxItemPickUp.Checked = False
        TBQuestPickUpID.Text = String.Empty
        TBQuestTurnInID.Text = String.Empty
        TBQuestNeedQuestCompId.Text = String.Empty
        TBQuestWQ.Text = String.Empty
        CBQuestWQ.Checked = False
        CBQuestAutoAccept.Checked = False
        TBQuestWQ.Enabled = False
    End Sub
    Sub ClearNPCForm()

        TBNpcName.Text = String.Empty
        TBNpcId.Text = String.Empty
        TBNpcPosition.Text = String.Empty
        TBNpcContinentId.Text = String.Empty



    End Sub

    Sub FillObjectiveFormByType(QObjective As Quester.Profile.QuestObjective, Quest As Quester.Profile.Quest)

        DisableObjForm()

        Dim cbSelectValue As Integer
        With QObjective
            cbSelectValue = .Objective
            Select Case .Objective.ToString
                Case "KillMob"
                    CBObjKillMobPickUpItem.Visible = True
                    TBObjEntry.Enabled = True
                    If .CollectItemId > 0 Then
                        TBObjCollectCount.Enabled = True
                        TBObjCollectItemID.Enabled = True
                        TBObjCollectCount.Text = .CollectCount
                        TBObjCollectItemID.Text = .CollectItemId
                        cbSelectValue = Quester.Profile.Objective.KillMob
                        CBObjKillMobPickUpItem.Checked = True
                    Else
                        TBObjCount.Enabled = True
                        TBObjCount.Text = .Count
                    End If

                    CBObjCanPullUnitsInFight.Enabled = True
                    CBObjCanPullUnitsInFight.Checked = .CanPullUnitsAlreadyInFight

                Case "BuyItem"

                    TBObjEntry.Enabled = True
                    TBObjCollectCount.Enabled = True
                    TBObjCollectItemID.Enabled = True
                    TBObjNPCId.Enabled = True
                    TBObjPosition.Enabled = True
                    LabelObjNPCIDorName.Text = "NPC Name"
                 
                    TBObjCollectCount.Text = .CollectCount
                    TBObjCollectItemID.Text = .CollectItemId
                    TBObjNPCId.Text = .Name
                    TBObjPosition.Text = .Position.ToString()

                Case "PickUpObject"
                    TBObjCollectCount.Enabled = True
                    TBObjCollectItemID.Enabled = True
                    TBObjEntry.Enabled = True


                    TBObjCollectCount.Text = .CollectCount
                    TBObjCollectItemID.Text = .CollectItemId

                Case "UseItem"
                    TBObjUseItemID.Enabled = True
                    TBObjCount.Enabled = True
                    TBObjPosition.Enabled = True
                    TBObjEntry.Enabled = True
                    TBObjUseItemID.Text = .UseItemId
                    TBObjCount.Text = .Count
                    TBObjPosition.Text = .Position.ToString()

                Case "UseSpell"
                    'TODO ENTRY + NAME + POSITION
                    TBObjUseSpellId.Enabled = True
                    TBObjPosition.Enabled = True
                    TBObjWaitMs.Enabled = True

                    TBObjPosition.Text = .Position.ToString
                    TBObjUseSpellId.Text = .UseSpellId
                    TBObjWaitMs.Text = .WaitMs

                Case "UseSpellAOE"
                    TBObjUseSpellId.Enabled = True
                    TBObjPosition.Enabled = True
                    TBObjRange.Enabled = True
                    TBObjWaitMs.Enabled = True

                    TBObjPosition.Text = .Position.ToString
                    TBObjUseSpellId.Text = .UseSpellId
                    TBObjWaitMs.Text = .WaitMs
                    TBObjRange.Text = .Range

                Case "InteractWith"
                    TBObjEntry.Enabled = True
                    TBObjPosition.Enabled = True
                    TBObjWaitMs.Enabled = True
                    TBObjGossipOption.Enabled = True

                    TBObjPosition.Text = .Position.ToString
                    TBObjWaitMs.Text = .WaitMs
                    TBObjGossipOption.Text = .GossipOptionsInteractWith
                Case "MoveTo"
                    TBObjPosition.Enabled = True

                    TBObjPosition.Text = .Position.ToString
                Case "Wait"
                    TBObjWaitMs.Enabled = True

                    TBObjWaitMs.Text = .WaitMs

                Case "PickUpQuest"
                    TBObjNPCId.Enabled = True
                    TBObjQuestID.Enabled = True
                    TBObjQuestName.Enabled = True
                    CBObjIgnoreQuestCompleted.Enabled = True
                    CBInternalObj.Enabled = False

                    TBObjNPCId.Text = .NpcEntry
                    TBObjQuestID.Text = .QuestId
                    TBObjQuestName.Text = .QuestName

                Case "TurnInQuest"
                    TBObjNPCId.Enabled = True
                    TBObjQuestID.Enabled = True
                    TBObjQuestName.Enabled = True
                    CBObjIgnoreQuestCompleted.Enabled = True
                    CBInternalObj.Enabled = False

                    TBObjNPCId.Text = .NpcEntry
                    TBObjQuestID.Text = .QuestId
                    TBObjQuestName.Text = .QuestName

                Case "UseFlightPath"
                    TBObjTaxiEntryId.Enabled = True
                    TBObjDestinationY.Enabled = True
                    TBObjDestinationX.Enabled = True

                    TBObjTaxiEntryId.Text = .TaxiEntry
                    TBObjDestinationY.Text = .FlightDestinationY
                    TBObjDestinationX.Text = .FlightDestinationX
                    TBObjFlightWaitMs.Text = .WaitMs
                Case "PickUpNPC"
                    TBObjCount.Enabled = True
                    TBObjEntry.Enabled = True
                    CBObjCanPullUnitsInFight.Enabled = True
                    TBObjGossipOption.Enabled = True

                    CBObjCanPullUnitsInFight.Checked = .CanPullUnitsAlreadyInFight
                    TBObjCount.Text = .Count
                    TBObjGossipOption.Text = .GossipOptionsInteractWith

                Case "UseVehicle"
                    TBObjPosition.Enabled = True
                    TBObjEntry.Enabled = True

                    TBObjEntry.Text = .EntryVehicle
                    TBObjPosition.Text = .Position.ToString()
                Case "ClickOnTerrain"
                    TBObjPosition.Enabled = True
                    TBObjWaitMs.Enabled = True

                    TBObjPosition.Text = .Position.ToString
                    TBObjWaitMs.Text = .WaitMs
            End Select

            Select Case .Objective.ToString
                Case "UseFlightPath"
                    PanelObjAll.Visible = False
                    PanelObjTaxi.Visible = True
                Case Else
                    PanelObjAll.Visible = True
                    PanelObjTaxi.Visible = False
            End Select

            CBObjIgnoreQuestCompleted.Checked = .IgnoreQuestCompleted


            RemoveHandler CBObjType.SelectedValueChanged, AddressOf CBObjType_SelectedValueChanged
            CBObjType.SelectedValue = cbSelectValue
            AddHandler CBObjType.SelectedValueChanged, AddressOf CBObjType_SelectedValueChanged

            CBObjInternalQuestID.Enabled = IIf(.InternalQuestId > 0, True, False)

            TBObjInternalIndex.Text = .InternalIndex

            If .Objective.ToString <> "TurnInQuest" AndAlso .Objective.ToString <> "PickUpQuest" Then
                CBInternalObj.Enabled = True
                CBInternalObj.Checked = IIf(.InternalQuestId > 0, True, False)
            End If

            'Fill HotSpots
            If .Hotspots.Count > 0 Then
                Dim intList As New List(Of String)
                For Each hPoint In .Hotspots
                    intList.Add(hPoint.X.ToString & ";" & hPoint.Y.ToString & ";" & hPoint.Z.ToString)

                Next

                TBObjHotSpots.Lines = intList.ToArray
            End If

            'Fill Entry
            If .Entry.Count > 0 AndAlso .Objective.ToString <> "UseVehicle" Then
                Dim intList As New List(Of String)
                For Each ent In .Entry
                    intList.Add(ent)

                Next

                TBObjEntry.Lines = intList.ToArray
            End If

            If CBInternalObj.Checked = True Then

                Dim listQuest As New List(Of ComboBoxValue)
                With listQuest

                    For Each obj In Quest.Objectives

                        If obj.Objective = Quester.Profile.Objective.PickUpQuest Then
                            listQuest.Add(New ComboBoxValue With {.Name = obj.QuestName & " " & obj.QuestId, .Value = obj.QuestId})
                        End If

                    Next

                    CBObjInternalQuestID.DataSource = listQuest

                    CBObjInternalQuestID.ValueMember = "Value"
                    CBObjInternalQuestID.DisplayMember = "Name"
                    CBObjInternalQuestID.SelectedValue = QObjective.InternalQuestId
                End With
            End If


        End With
    End Sub

    Sub PopulateComboBox()

        Dim cbObjTypeList As New List(Of ComboBoxValueString)


        'None = 0
        'ApplyBuff = 1
        'BuyItem = 2
        'EjectVehicle = 3
        'EquipItem = 4
        'InteractWith = 5
        'KillMob = 6
        'MoveTo = 7
        'PickUpObject = 8
        'PickUpQuest = 9
        'PressKey = 10
        'UseItem = 11
        'UseLuaMacro = 12
        'TurnInQuest = 13
        'UseFlightPath = 14
        'UseItemAOE = 15
        'UseActionButtonOnUnit = 16
        'UseRuneForge = 17
        'UseSpell = 18
        'UseSpellAOE = 19
        'UseVehicle = 20
        'Wait = 21
        'TravelTo = 22
        'ClickOnTerrain = 23
        'MessageBox = 24
        'PickUpNPC = 25
        'GarrisonHearthstone = 26




        With cbObjTypeList
            .Add(New ComboBoxValueString With {.Name = "Killing mobs", .Value = 6})
            .Add(New ComboBoxValueString With {.Name = "Buying Item", .Value = 2})
            .Add(New ComboBoxValueString With {.Name = "Gathering Items", .Value = 8})
            .Add(New ComboBoxValueString With {.Name = "Use an Item", .Value = 11})
            .Add(New ComboBoxValueString With {.Name = "Casting a spell", .Value = 18})
            .Add(New ComboBoxValueString With {.Name = "Casting a spell AOE", .Value = 19})
            .Add(New ComboBoxValueString With {.Name = "Interacting with a gameobject", .Value = 5})
            .Add(New ComboBoxValueString With {.Name = "Going Somewhere", .Value = 7})
            .Add(New ComboBoxValueString With {.Name = "Just Wait", .Value = 21})
            .Add(New ComboBoxValueString With {.Name = "Pickup Quest", .Value = 9})
            .Add(New ComboBoxValueString With {.Name = "Turnin Quest", .Value = 13})
            .Add(New ComboBoxValueString With {.Name = "Use Flight", .Value = 14})
            .Add(New ComboBoxValueString With {.Name = "Use Vehicle", .Value = 20})
            .Add(New ComboBoxValueString With {.Name = "Eject Vehicle", .Value = 3})
            .Add(New ComboBoxValueString With {.Name = "PickUp Npc", .Value = 25})
            .Add(New ComboBoxValueString With {.Name = "Click On Terrain", .Value = 23})

            CBObjType.DataSource = cbObjTypeList

            CBObjType.ValueMember = "Value"
            CBObjType.DisplayMember = "Name"
        End With


        'Dim classMaskList As New List(Of ComboBoxValue)
        'With classMaskList
        '    .Add(New ComboBoxValue With {.Name = "N/A", .Value = 0})
        '    .Add(New ComboBoxValue With {.Name = "Warrior", .Value = 1})
        '    .Add(New ComboBoxValue With {.Name = "Paladin", .Value = 2})
        '    .Add(New ComboBoxValue With {.Name = "Hunter", .Value = 4})
        '    .Add(New ComboBoxValue With {.Name = "Rogue", .Value = 8})
        '    .Add(New ComboBoxValue With {.Name = "Priest", .Value = 16})
        '    .Add(New ComboBoxValue With {.Name = "Death Knight", .Value = 32})
        '    .Add(New ComboBoxValue With {.Name = "Shaman", .Value = 64})
        '    .Add(New ComboBoxValue With {.Name = "Mage", .Value = 128})
        '    .Add(New ComboBoxValue With {.Name = "Warlock", .Value = 256})
        '    .Add(New ComboBoxValue With {.Name = "Monk", .Value = 512})
        '    .Add(New ComboBoxValue With {.Name = "Druid", .Value = 1024})

        '    CBClassMask.DataSource = classMaskList

        '    CBClassMask.ValueMember = "Value"
        '    CBClassMask.DisplayMember = "Name"
        'End With

        'Dim raceMaskList As New List(Of ComboBoxValue)

        'With raceMaskList
        '    .Add(New ComboBoxValue With {.Name = "N/A", .Value = 0})
        '    .Add(New ComboBoxValue With {.Name = "Human", .Value = 1})
        '    .Add(New ComboBoxValue With {.Name = "Orc", .Value = 2})
        '    .Add(New ComboBoxValue With {.Name = "Dwarf", .Value = 4})
        '    .Add(New ComboBoxValue With {.Name = "Night Elf", .Value = 8})
        '    .Add(New ComboBoxValue With {.Name = "Undead", .Value = 16})
        '    .Add(New ComboBoxValue With {.Name = "Tauren", .Value = 32})
        '    .Add(New ComboBoxValue With {.Name = "Gnome", .Value = 64})
        '    .Add(New ComboBoxValue With {.Name = "Troll", .Value = 128})
        '    .Add(New ComboBoxValue With {.Name = "Goblin", .Value = 256})
        '    .Add(New ComboBoxValue With {.Name = "Blood Elf", .Value = 512})
        '    .Add(New ComboBoxValue With {.Name = "Draenei", .Value = 1024})

        '    CBRaceMask.DataSource = raceMaskList
        '    CBRaceMask.ValueMember = "Value"
        '    CBRaceMask.DisplayMember = "Name"


        'End With

        'NPC TYPE
        Dim npcType As New List(Of ComboBoxValue)

        npcType.Add(New ComboBoxValue With {.Name = "QuestGiver", .Value = 44})
        npcType.Add(New ComboBoxValue With {.Name = "FlightMaster", .Value = 35})



        CBNpcType.DataSource = npcType
        CBNpcType.ValueMember = "Value"
        CBNpcType.DisplayMember = "Name"

        Dim factL As New List(Of ComboBoxValue)

        For Each st In [Enum].GetValues(GetType(nManager.Wow.Class.Npc.FactionType))
            factL.Add(New ComboBoxValue With {.Name = st.ToString, .Value = CType(st, Integer)})

        Next
        CBNpcFaction.DataSource = factL
        CBNpcFaction.ValueMember = "Value"
        CBNpcFaction.DisplayMember = "Name"

        Dim classValue As Integer

        For Each st In [Enum].GetNames(GetType(nManager.Wow.Enums.WoWClassMask))
            classValue = CType(String.Format("{0:D}", [Enum].Parse(GetType(Wow.Enums.WoWClassMask), st)), Integer)

            CLBQuestClassMask.Items.Add(New ComboBoxValue With {.Name = st, .Value = classValue})

        Next
        CLBQuestClassMask.DisplayMember = "Name"
        CLBQuestClassMask.ValueMember = "Value"
        Dim raceValue As Integer
        Dim exp As Integer = 0
        Dim raceMask As Integer = 0
        For Each st In [Enum].GetNames(GetType(nManager.Wow.Enums.WoWRace))
            Select Case st
                Case "Human", "Orc", "Dwarf", "NightElf", "Undead", "Tauren", "Gnome", "Troll", "Goblin", "BloodElf", "Draenei", "Worgen", "PandarenAliance", "PandarenHorde"
                    raceValue = CType(String.Format("{0:D}", [Enum].Parse(GetType(Wow.Enums.WoWRace), st)), Integer)
                    exp = raceValue - 1
                    raceMask = IIf(exp >= 0, System.Math.Pow(2, exp), 0)
                    CLBQuestRaceMask.Items.Add(New ComboBoxValue With {.Name = st, .Value = raceMask})
            End Select


        Next
        CLBQuestRaceMask.DisplayMember = "Name"
        CLBQuestRaceMask.ValueMember = "Value"

    End Sub
    Function GetSelectedObjectiveTypeName() As String

        Dim cbObjSelValue As Integer
        If IsNumeric(CBObjType.SelectedValue) Then
            cbObjSelValue = CBObjType.SelectedValue
        Else
            cbObjSelValue = CType(CBObjType.SelectedValue, ComboBoxValueString).Value
        End If

        Dim SelectedObjectiveName As String = [Enum].GetName(GetType(Quester.Profile.Objective), cbObjSelValue)
        Return SelectedObjectiveName
    End Function
    Private Sub CBObjType_SelectedValueChanged(sender As Object, e As EventArgs) Handles CBObjType.SelectedValueChanged

        DisableObjForm()
        If TreeView.SelectedNode IsNot Nothing AndAlso TreeView.SelectedNode.Tag <> "NewObjective" Then
            TreeView.SelectedNode = Nothing
        End If



        '  cbSelectValue = [Enum].Parse(GetType(ObjectivesEnum), .Element("Objective"))


        Dim SelectedObjectiveName As String = GetSelectedObjectiveTypeName()

        Select Case SelectedObjectiveName
            Case "KillMob"
                TBObjEntry.Enabled = True
                CBObjKillMobPickUpItem.Visible = True
                TBObjCount.Enabled = True
                CBObjCanPullUnitsInFight.Enabled = True
            Case "BuyItem"

                TBObjEntry.Enabled = True
                TBObjCollectCount.Enabled = True
                TBObjCollectItemID.Enabled = True
                TBObjNPCId.Enabled = True
                TBObjPosition.Enabled = True
                LabelObjNPCIDorName.Text = "NPC Name"
            Case "PickUpObject"
                TBObjCollectCount.Enabled = True
                TBObjCollectItemID.Enabled = True
                TBObjEntry.Enabled = True



            Case "UseItem"
                TBObjUseItemID.Enabled = True
                TBObjCount.Enabled = True
                TBObjPosition.Enabled = True
                TBObjEntry.Enabled = True


            Case "UseSpell"
                'TODO ENTRY + NAME + POSITION
                TBObjUseSpellId.Enabled = True
                TBObjPosition.Enabled = True
                TBObjWaitMs.Enabled = True


            Case "UseSpellAOE"
                TBObjUseSpellId.Enabled = True
                TBObjPosition.Enabled = True
                TBObjRange.Enabled = True
                TBObjWaitMs.Enabled = True



            Case "InteractWith"
                TBObjEntry.Enabled = True
                TBObjPosition.Enabled = True
                TBObjWaitMs.Enabled = True
                TBObjGossipOption.Enabled = True
            Case "MoveTo"
                TBObjPosition.Enabled = True

            Case "Wait"
                TBObjWaitMs.Enabled = True
            Case "PickUpQuest"

                TBObjNPCId.Enabled = True
                TBObjQuestID.Enabled = True
                TBObjQuestName.Enabled = True

            Case "TurnInQuest"
                TBObjNPCId.Enabled = True
                TBObjQuestID.Enabled = True
                TBObjQuestName.Enabled = True

            Case "UseFlightPath"
                TBObjTaxiEntryId.Enabled = True
                TBObjDestinationY.Enabled = True
                TBObjDestinationX.Enabled = True

            Case "PickUpNPC"
                TBObjCount.Enabled = True
                TBObjEntry.Enabled = True
                CBObjCanPullUnitsInFight.Enabled = True
                TBObjGossipOption.Enabled = True

            Case "UseVehicle"
                TBObjPosition.Enabled = True
                TBObjEntry.Enabled = True

            Case "ClickOnTerrain"
                TBObjPosition.Enabled = True
                TBObjWaitMs.Enabled = True

        End Select

        Select Case SelectedObjectiveName
            Case "UseFlightPath"
                PanelObjAll.Visible = False
                PanelObjTaxi.Visible = True
            Case Else
                PanelObjAll.Visible = True
                PanelObjTaxi.Visible = False
        End Select

        CBInternalObj.Enabled = IIf(SelectedObjectiveName <> "TurnInQuest" AndAlso SelectedObjectiveName <> "PickUpQuest", True, False)





    End Sub

    Private Sub CBObjKillMobPickUpItem_CheckedChanged(sender As Object, e As EventArgs) Handles CBObjKillMobPickUpItem.CheckedChanged
        If CBObjKillMobPickUpItem.Checked Then
            TBObjEntry.Enabled = True
            TBObjCollectCount.Enabled = True
            TBObjCollectItemID.Enabled = True
            TBObjCollectItemID.Text = 1
            TBObjCount.Enabled = False

        Else

            TBObjCount.Enabled = True

            TBObjCollectItemID.Text = String.Empty
            TBObjCollectCount.Enabled = False
            TBObjCollectItemID.Enabled = False
        End If
        TBObjEntry.Enabled = True
    End Sub

    Private Sub CBInternalObj_CheckedChanged(sender As Object, e As EventArgs) Handles CBInternalObj.CheckedChanged
        Try


            If CBInternalObj.Checked Then
                CBObjInternalQuestID.Enabled = True

                'Load the list of obj with PickUp Quest
                Dim listQuest As New List(Of ComboBoxValue)
                With listQuest

                    For Each obj In profile.Quests.ElementAt(lastSelectedQuest.Index).Objectives

                        If obj.Objective = Quester.Profile.Objective.PickUpQuest Then
                            listQuest.Add(New ComboBoxValue With {.Name = obj.QuestName & " " & obj.QuestId, .Value = obj.QuestId})
                        End If

                    Next

                    CBObjInternalQuestID.DataSource = listQuest

                    CBObjInternalQuestID.ValueMember = "Value"
                    CBObjInternalQuestID.DisplayMember = "Name"
                End With
                CBObjIgnoreQuestCompleted.Checked = True
            Else
                CBObjInternalQuestID.Enabled = False
                CBObjInternalQuestID.DataSource = Nothing

            End If
        Catch ex As Exception
            'Catch Form Load triggering this Sub....and crashing
        End Try
    End Sub

    Private Sub ButtonObjImportFromGame_Click(sender As Object, e As EventArgs) Handles ButtonObjImportFromGame.Click


        '   If lastSelectedObjective IsNot Nothing Then

        If Target.IsValid AndAlso QuestersDB.GetNpcByEntry(Target.Entry).Entry = 0 Then
            If MessageBox.Show("This Quest Giver isnt in the DB. Do you want to Add it?", "Warning", MessageBoxButtons.YesNo) = DialogResult.Yes Then

                ButtonNewNPC_Click(Nothing, Nothing)
                ButtonNpcImport_Click(Nothing, Nothing)
                ButtonSaveNPC_Click(Nothing, Nothing)


            End If
        End If

        Dim wowGOv As WoWGameObject = GetNearestWoWGameObject(GetWoWGameObjectOfType(nManager.Wow.Enums.WoWGameObjectType.Questgiver))
        If wowGOv.Entry > 0 AndAlso [Me].Position.DistanceTo(wowGOv.Position) < 5.0F AndAlso QuestersDB.GetNpcByEntry(wowGOv.Entry).Entry = 0 Then
            If MessageBox.Show("This Quest Giver (Object) isnt in the DB. Do you want to Add it?", "Warning", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                ButtonNewNPC_Click(Nothing, Nothing)
                ButtonNpcImport_Click(Nothing, Nothing)
                ButtonSaveNPC_Click(Nothing, Nothing)
            End If
        End If


        If Target.IsNpcQuestGiver Then
            Select Case GetSelectedObjectiveTypeName()
                Case "PickUpQuest", "TurnInQuest"

                    TBObjNPCId.Text = Target.Entry
                    TBObjQuestID.Text = Lua.LuaDoString("luaS = GetQuestID()", "luaS")
                    TBObjQuestName.Text = Lua.LuaDoString("luaS = GetTitleText()", "luaS")

            End Select
        Else

            Dim wowGO As WoWGameObject = GetNearestWoWGameObject(GetWoWGameObjectOfType(nManager.Wow.Enums.WoWGameObjectType.Questgiver))
            If wowGO.Entry > 0 AndAlso [Me].Position.DistanceTo(wowGO.Position) < 5.0F Then
                TBObjNPCId.Text = wowGO.Entry
                TBObjQuestID.Text = Lua.LuaDoString("luaS = GetQuestID()", "luaS")
                TBObjQuestName.Text = Lua.LuaDoString("luaS = GetTitleText()", "luaS")
            End If
        End If



        'Fill Count 
        If GetSelectedObjectiveTypeName() = "KillMob" And CBObjKillMobPickUpItem.Checked = False Then
                Dim qId As Integer
                Dim count As Integer = 0

                If CBInternalObj.Checked And CBObjInternalQuestID.SelectedValue IsNot Nothing Then
                    qId = CBObjInternalQuestID.SelectedValue
                    count = Lua.LuaDoString("text, objectiveType, finishedd,currentStatut,finishStatut= GetQuestObjectiveInfo(" & qId & ",1,false)", "finishStatut")
                Else
                    qId = profile.Quests(lastSelectedQuest.Index).Id
                    count = Lua.LuaDoString("text, objectiveType, finishedd,currentStatut,finishStatut= GetQuestObjectiveInfo(" & qId & ",1,false)", "finishStatut")


            End If

                If CBObjKillMobPickUpItem.Checked = False Then
                    TBObjCount.Text = count
                Else
                    TBObjCollectCount.Text = count

                End If
            End If

            If Target.IsNpcFlightMaster AndAlso GetSelectedObjectiveTypeName() = "UseFlightPath" Then

                TBObjTaxiEntryId.Text = Target.Entry

            End If
       ' End If
    End Sub
    Private Sub ButtonObjGetXY_Click(sender As Object, e As EventArgs) Handles ButtonObjGetXY.Click
        'TODO TAXI LIST

        Dim unit As WoWUnit
        If Target.IsNpcFlightMaster Then
            unit = Target
        Else
            If [Me].Position.DistanceTo(GetNearestWoWUnit(GetWoWUnitFlightMaster()).Position) < 5.0F Then
                unit = GetNearestWoWUnit(GetWoWUnitFlightMaster())
            Else
                Exit Sub
            End If
        End If


        '   Dim taxilist As XElement = XElement.Load("C:\Users\Julien\Documents\Visual Studio 2013\Templates\TestTemplates\Quester Profile Creator\TaxiList.xml")
        Dim taxilist As XElement = XElement.Load(Application.StartupPath & "\\Data\\TaxiList.xml")

        For Each taxi In taxilist.Elements

            If taxi.Attribute("Id").Value = unit.Entry Then
                TBObjDestinationX.Text = taxi.Element("Xcoord").Value
                TBObjDestinationY.Text = taxi.Element("Ycoord").Value
                Exit For

            End If
        Next

    End Sub
    Private Sub ButtonOpenWowHead_Click(sender As Object, e As EventArgs) Handles ButtonOpenWowHead.Click
        If TBQuestID.Text <> String.Empty AndAlso IsNumeric(TBQuestID.Text) Then
            Others.OpenWebBrowserOrApplication("http://www.wowhead.com/quest=" & TBQuestID.Text)
        End If


    End Sub

    Sub TODOs()
        'InteractWith Count ou jusqua quest completed. Quete gros mob dnas dustwallow
        'TODO Add quest required ID from prev quest ID
        'TODO Position WQ
        'TODO Add Objective Messagebox 
        '  <QuestObjective>
        '  <Objective>MessageBox</Objective>
        '  <Message>You must Do this quest manually, don't worry, it's fast, just loot the blueprint, learn it, place your barrack, then activate it, then the bot can finish the quest for you.</Message>
        '</QuestObjective>
    End Sub
    Private Sub ButtonQuestImportFromGame_Click(sender As Object, e As EventArgs) Handles ButtonQuestImportFromGame.Click
        ' If Target.IsNpcQuestGiver And Others.IsFrameVisible("QuestFrameDetailPanel") Then
        If Others.IsFrameVisible("QuestFrameDetailPanel") Then

            If Target.IsValid AndAlso QuestersDB.GetNpcByEntry(Target.Entry) Is Nothing Then
                If MessageBox.Show("This Quest Giver isnt in the DB. Do you want to Add it?", "Warning", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                    ButtonNewNPC.PerformClick()
                    ButtonNpcImport.PerformClick()
                    ButtonSaveNPC.PerformClick()

                    PanelNPC.Visible = True
                    PanelSimpleQuest.Visible = False
                End If
            End If

            Dim wowGO As WoWGameObject = GetNearestWoWGameObject(GetWoWGameObjectOfType(nManager.Wow.Enums.WoWGameObjectType.Questgiver))
            If wowGO.Entry > 0 AndAlso [Me].Position.DistanceTo(wowGO.Position) < 5.0F AndAlso QuestersDB.GetNpcByEntry(wowGO.Entry) Is Nothing Then
                If MessageBox.Show("This Quest Giver (Object) isnt in the DB. Do you want to Add it?", "Warning", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                    ButtonNewNPC.PerformClick()
                    ButtonNpcImport.PerformClick()
                    ButtonSaveNPC.PerformClick()
                    PanelNPC.Visible = True
                    PanelSimpleQuest.Visible = False
                End If
            End If


            TBQuestPickUpID.Text = Target.Entry
            TBQuestTurnInID.Text = Target.Entry

            TBQuestID.Text = Lua.LuaDoString("luaS = GetQuestID()", "luaS")
            TBQuestQuestName.Text = Lua.LuaDoString("luaS = GetTitleText()", "luaS")

            Wow.Helpers.Quest.AcceptQuest()
            Threading.Thread.Sleep(1000)
            Dim questLogIdx As Integer = Lua.LuaDoString("idx = GetQuestLogIndexByID(" & TBQuestID.Text & ")", "idx")

            Dim questl As String = Lua.LuaDoString("title, level, suggestedGroup, isHeader, isCollapsed,isComplete, frequency, questID, startEvent, displayQuestID, isOnMap, hasLocalPOI, isTask, isStory,t,tt,ttt,tttt = GetQuestLogTitle(" & questLogIdx & ")", "level")

            TBQuestLevel.Text = questl
            TBQuestMaxLvl.Text = GetMaxQuestLvl(questl)
            TBQuestMinLvl.Text = GetMinQuestLvl(questl)
        Else
            MessageBox.Show("No QuestGiver Quest Frame Opened, Cancel.")
        End If
    End Sub

    Function GetMaxQuestLvl(questLvl As Integer) As Integer
        Dim questGreenRange As Integer = Lua.LuaDoString("range = GetQuestGreenRange()", "range")

        Dim temp As Integer = 0
        Dim levelDiff As Integer
        Dim pLevel As Integer = questLvl
        Dim r As Integer

        While temp = 0
            levelDiff = questLvl - pLevel
            If levelDiff >= 5 Then
                pLevel = pLevel + 1
            ElseIf levelDiff >= 3 Then
                pLevel = pLevel + 1
            ElseIf levelDiff >= -2 Then
                pLevel = pLevel + 1
            ElseIf System.Math.Abs(levelDiff) <= questGreenRange Then
                pLevel = pLevel + 1
            Else
                temp = 1
                r = pLevel - 1
            End If

        End While
        Return r

    End Function
    Function GetMinQuestLvl(questLvl As Integer) As Integer
        Dim questGreenRange As Integer = Lua.LuaDoString("range = GetQuestGreenRange()", "range")

        Dim temp As Integer = 0
        Dim levelDiff As Integer
        Dim pLevel As Integer = questLvl
        Dim r As Integer

        While temp = 0
            levelDiff = questLvl - pLevel
            If levelDiff >= 5 Then

            ElseIf levelDiff >= 3 Then
                temp = 1
                r = pLevel
            ElseIf levelDiff >= -2 Then
                pLevel = pLevel - 1
            ElseIf System.Math.Abs(levelDiff) <= questGreenRange Then
                pLevel = pLevel - 1
            Else

            End If

        End While
        Return r

    End Function

    Sub ClearMaskListBox()
        For Each item In CLBQuestClassMask.CheckedIndices
            CLBQuestClassMask.SetItemCheckState(item, False)
        Next
        For Each item In CLBQuestRaceMask.CheckedIndices
            CLBQuestRaceMask.SetItemCheckState(item, False)
        Next
        CLBQuestRaceMask.ClearSelected()
        CLBQuestClassMask.ClearSelected()
    End Sub

    Sub SaveQuestMask()


    End Sub

    Sub FillQuestMaskAfterObjSelection(qClassMask As Integer, qRaceMask As Integer)
        Dim idxList As New List(Of Integer)
        Dim idx As Integer = 0
        Dim qmask As Integer = qClassMask
        For Each item As ComboBoxValue In CLBQuestClassMask.Items

            If item.Value And qmask Then
                idxList.Add(idx)
            End If

            idx += 1

        Next


        For Each i In idxList
            CLBQuestClassMask.SetItemChecked(i, True)
        Next

        idx = 0
        qmask = qRaceMask
        idxList.Clear()
        ' Dim raceMask As Integer
        'Dim exp As Integer

        For Each item As ComboBoxValue In CLBQuestRaceMask.Items
            '      exp = item.Value - 1
            '    raceMask = IIf(exp >= 0, System.Math.Pow(2, exp), 0)
            If item.Value And qmask Then
                idxList.Add(idx)
            End If

            idx += 1

        Next

        For Each i In idxList
            CLBQuestRaceMask.SetItemChecked(i, True)
        Next

    End Sub

#Region "EVENTS - Optional"
    Sub QuestDetail(te As String)
        MessageBox.Show(te)
    End Sub
    Sub QuestFinished(te As String)
        Dim questID As Integer = Lua.LuaDoString("qId = GetQuestID()", "qId")

        Dim questName As String = Lua.LuaDoString("qTxt = GetTitleText()", "qTxt")

        For Each quest In profile.Quests
            If quest.Id = questID Then
                For Each node As TreeNode In QuestParentNode.Nodes
                    If node.Text = questName Then 'Chercher si la quete existe
                        If ReturnNumQuestPickAndQuestTurnIn(quest.Objectives) > 0 Then 'If quest doesnt have objectives other than its own 
                            TreeView.SelectedNode = node
                            TBQuestTurnInID.Text = Target.Entry
                        Else 'If the quest is a "multiple" quest
                            quest.Objectives.Add(CreateTurnInObj(Target.Entry, questID, questName))
                            Wow.Helpers.Quest.AcceptQuest()
                        End If

                        Exit For
                    End If
                Next
            End If
            For Each obj In quest.Objectives
                Select Case obj.Objective.ToString
                    Case "PickUpQuest"
                        If obj.QuestId = questID Then
                            'Find Parent Node
                            For Each node As TreeNode In QuestParentNode.Nodes
                                If node.Text = quest.Name Then
                                    Dim newObjNode As New TreeNode("TurnInQuest " & questName)
                                    newObjNode.Tag = "Objective"
                                    node.Nodes.Add(newObjNode)

                                End If
                            Next
                            quest.Objectives.Add(CreateTurnInObj(Target.Entry, questID, questName))
                            Wow.Helpers.Quest.AcceptQuest()
                            Exit For
                        End If

                End Select
            Next

        Next
    End Sub

    Function ReturnNumQuestPickAndQuestTurnIn(objs As List(Of Quester.Profile.QuestObjective)) As Integer

        Dim count As Integer = 0

        For Each obj In objs
            If obj.Objective = Quester.Profile.Objective.PickUpQuest Or obj.Objective = Quester.Profile.Objective.TurnInQuest Then
                count += 1
            End If
        Next
        Return count
    End Function

    Function CreateTurnInObj(npcId As Integer, questId As Integer, questName As String) As Quester.Profile.QuestObjective
        Dim qObj As New Quester.Profile.QuestObjective

        With qObj
            .Objective = Quester.Profile.Objective.TurnInQuest
            .IgnoreQuestCompleted = True
            .NpcEntry = npcId
            .QuestId = questId
            .QuestName = questName
        End With

        Return qObj

    End Function

    Private Sub ButtonObjHotSpots_Click(sender As Object, e As EventArgs) Handles ButtonObjHotSpots.Click

        Dim pos As String = ObjectManager.Me.Position.X.ToString & ";" & ObjectManager.Me.Position.Y.ToString & ";" & ObjectManager.Me.Position.Z.ToString
        If TBObjHotSpots.Lines.Count > 0 Then
            TBObjHotSpots.AppendText(Environment.NewLine & pos)
        Else
            TBObjHotSpots.AppendText(pos)
        End If


    End Sub

    Private Sub ButtonQuestHorde_Click(sender As Object, e As EventArgs) Handles ButtonQuestHorde.Click
        ClearMaskListBox()
        FillQuestMaskAfterObjSelection(0, 33555378)

    End Sub

    Private Sub ButtonQuestAlliance_Click(sender As Object, e As EventArgs) Handles ButtonQuestAlliance.Click
        ClearMaskListBox()
        FillQuestMaskAfterObjSelection(0, 18875469)
    End Sub

    Private Sub ButtonObjImportEntry_Click(sender As Object, e As EventArgs) Handles ButtonObjImportEntry.Click
        If ObjectManager.Target.IsValid Then
            Dim entry As Integer = ObjectManager.Target.Entry
            If TBObjEntry.Lines.Count > 0 Then
                TBObjEntry.AppendText(Environment.NewLine & entry)
            Else
                TBObjEntry.AppendText(entry)
            End If
        Else

            Dim oName As String = InputBox("Input Object Name:", "Import Object ID")
            If oName <> String.Empty Then
                Dim wowGo As WoWGameObject = GetNearestWoWGameObject(GetWoWGameObjectByName(oName))
                If wowGo.Entry > 0 Then
                    If TBObjEntry.Lines.Count > 0 Then
                        TBObjEntry.AppendText(Environment.NewLine & wowGo.Entry)
                    Else
                        TBObjEntry.AppendText(wowGo.Entry)
                    End If


                End If
            End If
        End If
    End Sub

    Private Sub ButtonObjImportGPS_Click(sender As Object, e As EventArgs) Handles ButtonObjImportGPS.Click
        TBObjPosition.Text = ObjectManager.Me.Position.ToString()
    End Sub

    Private Sub CBQuestWQ_CheckedChanged(sender As Object, e As EventArgs) Handles CBQuestWQ.CheckedChanged
        If CBQuestWQ.Checked Then
            TBQuestTurnInID.Enabled = False
            TBQuestTurnInID.Text = String.Empty
            TBQuestPickUpID.Enabled = False
            TBQuestPickUpID.Text = String.Empty
            TBQuestWQ.Enabled = True

        Else
            TBQuestPickUpID.Enabled = True
            TBQuestTurnInID.Enabled = True
            TBQuestWQ.Enabled = False
            TBQuestWQ.Text = String.Empty
        End If
    End Sub

    Dim displayXML As Boolean = False
    Private Sub CBMainDisplayXML_CheckedChanged(sender As Object, e As EventArgs) Handles CBMainDisplayXML.CheckedChanged
        If CBMainDisplayXML.Checked Then
            displayXML = True
            Me.Size = New Drawing.Size(fsize.Width + 579, Me.Size.Height)
        Else
            displayXML = False
            Me.Size = fsize
        End If
        Dim bla As Color = Color.FromArgb(131, 157, 20)
    End Sub

    Private Sub TreeView_NodeMouseClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles TreeView.NodeMouseClick
        If e.Button = MouseButtons.Right Then
            TreeView.SelectedNode = e.Node

        End If
    End Sub

    Dim fsize As Size

    Private Sub DeleteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteToolStripMenuItem.Click
        If TreeView.SelectedNode.Tag = "Quest" Then
            profile.Quests.RemoveAt(TreeView.SelectedNode.Index)
            TreeView.SelectedNode.Remove()
        ElseIf TreeView.SelectedNode.Tag = "Objective" Then
            profile.Quests(TreeView.SelectedNode.Parent.Index).Objectives.RemoveAt(TreeView.SelectedNode.Index)
            TreeView.SelectedNode.Remove()
        End If
    End Sub

    Private Sub InsertUpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles InsertUpToolStripMenuItem.Click, InsertDownToolStripMenuItem1.Click
        Dim insertIdxProfile = 0
        Dim insertIdxTreeView = 0
        If sender Is InsertUpToolStripMenuItem Then
            insertIdxProfile = 1
        Else
            insertIdxProfile = -1
            insertIdxTreeView = 1
        End If
        If TreeView.SelectedNode.Tag = "Quest" Then
            Dim newQuestNode As New TreeNode("New Quest")
            newQuestNode.Tag = "Quest"
            QuestParentNode.Nodes.Insert(TreeView.SelectedNode.Index + insertIdxTreeView, newQuestNode)

            Dim newQuest As New Quester.Profile.Quest
            newQuest.Name = "New Quest"
            profile.Quests.Insert(TreeView.SelectedNode.Index - insertIdxProfile, newQuest)

        ElseIf TreeView.SelectedNode.Tag = "Objective" Then
            Dim newObjNode As New TreeNode("New Objective")
            newObjNode.Tag = "NewObjective"
            TreeView.SelectedNode.Parent.Nodes.Insert(TreeView.SelectedNode.Index + insertIdxTreeView, newObjNode)

            Dim newObjective As New Quester.Profile.QuestObjective
            profile.Quests(TreeView.SelectedNode.Parent.Index).Objectives.Insert(TreeView.SelectedNode.Index - insertIdxProfile, newObjective)
        End If
    End Sub

    Private Sub ToolStripMenuItemAddNeedQuestComp_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemAddNeedQuestComp.Click

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles ButtonObjDumpIndex.Click

        Dim questId As Integer
        If TreeView.SelectedNode IsNot Nothing AndAlso TreeView.SelectedNode.Tag <> "NPCs" AndAlso TreeView.SelectedNode.Tag <> "Quests" AndAlso TreeView.SelectedNode.Tag <> "NPC" Then

            If TreeView.SelectedNode.Tag = "Quest" Then
                questId = profile.Quests.ElementAt(TreeView.SelectedNode.Index).Id
            ElseIf TreeView.SelectedNode.Tag = "Objective" Then
                Dim obj As Quester.Profile.QuestObjective = profile.Quests(TreeView.SelectedNode.Parent.Index).Objectives(TreeView.SelectedNode.Index)
                If obj.Objective.ToString = "PickUpQuest" Then
                    questId = profile.Quests(TreeView.SelectedNode.Parent.Index).Objectives(TreeView.SelectedNode.Index).QuestId
                Else
                    If obj.InternalQuestId > 0 Then
                        questId = obj.InternalQuestId
                    ElseIf CBInternalObj.Checked Then
                        questId = CBObjInternalQuestID.SelectedValue
                    Else
                        questId = profile.Quests(TreeView.SelectedNode.Parent.Index).Id
                    End If
                End If

            End If

            Quest.DumpInternalIndexForQuestId(questId)
        End If
    End Sub




















#End Region


End Class

Public Class ComboBoxValueString
    Property Name As String
    Property Value As Integer

End Class

Public Class ComboBoxValue
    Property Name As String
    Property Value As Integer

End Class
