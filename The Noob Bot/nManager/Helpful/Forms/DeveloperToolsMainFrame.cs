using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.CSharp;
using nManager.Helpful.Interface;
using nManager.Properties;
using nManager.Wow;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using nManager.Wow.Patchables;
using Point = nManager.Wow.Class.Point;

namespace nManager.Helpful.Forms
{
    public partial class DeveloperToolsMainFrame : Form
    {
        private string searchInputBox = "Type in the name of the WoWObject you are looking for:";

        public DeveloperToolsMainFrame()
        {
            try
            {
                InitializeComponent();
                Translate();
                if (nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature)
                    TopMost = true;
            }
            catch (Exception e)
            {
                Logging.WriteError("DeveloperToolsMainFrame > DeveloperToolsMainFrame(): " + e);
            }
        }

        private void Translate()
        {
            MainHeader.TitleText = nManager.Translate.Get(nManager.Translate.Id.Developer_Tools) + @" - " + Information.MainTitle;
            this.Text = MainHeader.TitleText;
            LuaExecButton.Text = nManager.Translate.Get(nManager.Translate.Id.LuaExecButton);
            GpsButton.Text = nManager.Translate.Get(nManager.Translate.Id.GpsButton);
            TargetInfoButton.Text = nManager.Translate.Get(nManager.Translate.Id.TargetInfoButton);
            TargetInfo2Button.Text = nManager.Translate.Get(nManager.Translate.Id.TargetInfo2Button);
            TranslationManagerButton.Text = nManager.Translate.Get(nManager.Translate.Id.TranslationManagerButton);
            CsharpExecButton.Text = nManager.Translate.Get(nManager.Translate.Id.CsharpExecButton);
            NpcTypeButton.Text = nManager.Translate.Get(nManager.Translate.Id.NpcTypeButton);
            NpcFactionButton.Text = nManager.Translate.Get(nManager.Translate.Id.NpcFactionButton);
            SearchObjectButton.Text = nManager.Translate.Get(nManager.Translate.Id.SearchObjectButton);
            AllObjectsButton.Text = nManager.Translate.Get(nManager.Translate.Id.AllObjectsButton);
            searchInputBox = nManager.Translate.Get(nManager.Translate.Id.SearchObjectBox);
        }

        private void SearchObjectButton_MouseEnter(object sender, EventArgs e)
        {
            SearchObjectButton.Image = Resources.greenB;
        }

        private void SearchObjectButton_MouseLeave(object sender, EventArgs e)
        {
            SearchObjectButton.Image = Resources.blackB;
        }

        private void TargetInfoButton_MouseEnter(object sender, EventArgs e)
        {
            TargetInfoButton.Image = Resources.greenB;
        }

        private void TargetInfoButton_MouseLeave(object sender, EventArgs e)
        {
            TargetInfoButton.Image = Resources.blackB;
        }

        private void SearchObjectButton_Click(object sender, EventArgs e)
        {
            try
            {
                SearchObjectButton.Enabled = false;
                string searchString = "";
                if (InputBox(SearchObjectButton.Text, searchInputBox, ref searchString) != DialogResult.OK)
                {
                    SearchObjectButton.Enabled = true;
                    return;
                }

                if (!ObjectManager.Me.IsValid)
                {
                    SearchObjectButton.Enabled = true;
                    return;
                }

                if (string.IsNullOrEmpty(searchString))
                {
                    MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.Name_Empty));
                    SearchObjectButton.Enabled = true;
                    return;
                }

                var npc = new Npc();

                List<WoWGameObject> gameObjects = ObjectManager.GetWoWGameObjectByName(searchString);

                if (gameObjects.Count > 0)
                {
                    WoWGameObject gameObject = ObjectManager.GetNearestWoWGameObject(gameObjects);
                    if (gameObject.IsValid)
                    {
                        npc.Entry = gameObject.Entry;
                        npc.Position = gameObject.Position;
                        npc.Name = gameObject.Name;
                        npc.Faction = UnitRelation.GetObjectRacialFaction(gameObject.Faction);
                    }
                }

                if (npc.Entry <= 0)
                {
                    List<WoWUnit> units = ObjectManager.GetWoWUnitByName(searchString);
                    if (units.Count > 0)
                    {
                        WoWUnit unit = ObjectManager.GetNearestWoWUnit(units);
                        if (unit.IsValid)
                        {
                            npc.Entry = unit.Entry;
                            npc.Position = unit.Position;
                            npc.Name = unit.Name;
                            npc.Faction = UnitRelation.GetObjectRacialFaction(unit.Faction);
                        }
                    }
                }

                if (npc.Entry <= 0)
                {
                    MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.NPCNotFound));
                    SearchObjectButton.Enabled = true;
                    return;
                }
                npc.ContinentIdInt = Usefuls.ContinentId;
                npc.Faction = (Npc.FactionType) Enum.Parse(typeof (Npc.FactionType), ObjectManager.Me.PlayerFaction, true);
                if (Usefuls.IsOutdoors)
                    npc.Position.Type = "Flying";
                InformationArea.Text =
                    "  <Npc>" + Environment.NewLine +
                    "    <Entry>" + npc.Entry + "</Entry>" + Environment.NewLine +
                    "    <Name>" + npc.Name + "</Name>" + Environment.NewLine +
                    "    <Position>" + Environment.NewLine +
                    "      <X>" + npc.Position.X + "</X>" + Environment.NewLine +
                    "      <Y>" + npc.Position.Y + "</Y>" + Environment.NewLine +
                    "      <Z>" + npc.Position.Z + "</Z>" + Environment.NewLine +
                    "      <Type>" + npc.Position.Type + "</Type>" + Environment.NewLine +
                    "    </Position>" + Environment.NewLine +
                    "    <Faction>" +
                    (Npc.FactionType) Enum.Parse(typeof (Npc.FactionType), ObjectManager.Me.PlayerFaction, true) +
                    "</Faction>" + Environment.NewLine +
                    "    <Type>None</Type>" + Environment.NewLine +
                    "    <ContinentId>" + npc.ContinentId + "</ContinentId>" +
                    Environment.NewLine +
                    "  </Npc>";
            }
            catch (Exception ex)
            {
                Logging.WriteError("DeveloperToolsMainFrame > SearchObjectButton_Click(object sender, EventArgs e): " + ex);
            }
            SearchObjectButton.Enabled = true;
        }

        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            var form = new Form();
            var label = new Label();
            var textBox = new TextBox();
            var buttonOk = new Button();
            var buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] {label, textBox, buttonOk, buttonCancel});
            form.ClientSize = new Size(System.Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

        private void TranslationManagerButton_Click(object sender, EventArgs e)
        {
            try
            {
                TranslationManagerButton.Enabled = false;
                var t = new TranslationManagementMainFrame();
                t.ShowDialog();
            }
            catch (Exception ex)
            {
                Logging.WriteError("DeveloperToolsMainFrame > SearchObjectButton_Click(object sender, EventArgs e): " + ex);
            }
            TranslationManagerButton.Enabled = true;
        }

        private void AllObjectsButton_Click(object sender, EventArgs e)
        {
            try
            {
                AllObjectsButton.Enabled = false;
                string codeHtml =
                    "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"> <html xmlns=\"http://www.w3.org/1999/xhtml\"> <head> <meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" /> <title>Get all objects information - " +
                    DateTime.Now.ToString("dd/mm/yy HHh mmMin") +
                    "</title>  <table width=\"100%\" bgcolor=\"#E8E8E8\" border=\"1\">   <tr>   <b>     <td bgcolor=\"#CCCCCC\">Name</td>     <td>Type</td>     <td bgcolor=\"#CCCCCC\">Entry ID</td>     <td>Position X</td>     <td bgcolor=\"#CCCCCC\">Position Y</td>     <td>Position Z</td>     <td bgcolor=\"#CCCCCC\">Distance</td>     <td>Faction</td>     <td bgcolor=\"#CCCCCC\">GUID</td>     <td>Summoned/Created By</td> <td>Unit Created By</td>    </b>   </tr>  ";
                // Me
                codeHtml +=
                    "<tr>     <td bgcolor=\"#CCCCCC\">" + ObjectManager.Me.Name +
                    "</td>     <td>WoWPlayer (" + ObjectManager.Me.Guid.GetWoWType + ")</td>     <td bgcolor=\"#CCCCCC\">" + ObjectManager.Me.Entry +
                    "</td>     <td>" + ObjectManager.Me.Position.X + "</td>     <td bgcolor=\"#CCCCCC\">" +
                    ObjectManager.Me.Position.Y + "</td>     <td>" + ObjectManager.Me.Position.Z +
                    "</td>     <td bgcolor=\"#CCCCCC\">" + ObjectManager.Me.GetDistance + "</td>     <td>" +
                    ObjectManager.Me.Faction + "</td>     <td bgcolor=\"#CCCCCC\">" + ObjectManager.Me.Guid +
                    "</td>     <td>" + ObjectManager.Me.SummonedBy + "</td>   </tr>";

                // WoWPlayer
                foreach (WoWPlayer woWPlayer in ObjectManager.GetObjectWoWPlayer())
                {
                    Application.DoEvents();
                    codeHtml +=
                        "<tr>     <td bgcolor=\"#CCCCCC\">" + woWPlayer.Name +
                        "</td>     <td>WoWPlayer (" + woWPlayer.Guid.GetWoWType + ")</td>     <td bgcolor=\"#CCCCCC\">" + woWPlayer.Entry + "</td>     <td>" +
                        woWPlayer.Position.X + "</td>     <td bgcolor=\"#CCCCCC\">" + woWPlayer.Position.Y +
                        "</td>     <td>" + woWPlayer.Position.Z + "</td>     <td bgcolor=\"#CCCCCC\">" +
                        woWPlayer.GetDistance + "</td>     <td>" + woWPlayer.Faction +
                        "</td>     <td bgcolor=\"#CCCCCC\">" + woWPlayer.Guid + "</td>     <td>" + woWPlayer.SummonedBy +
                        "</td>   </tr>";
                }
                // WoWUnit
                foreach (WoWUnit wowO in ObjectManager.GetObjectWoWUnit())
                {
                    Application.DoEvents();
                    codeHtml +=
                        "<tr>     <td bgcolor=\"#CCCCCC\">" + wowO.Name + " - (<i><a href=\"http://wowhead.com/npc=" +
                        wowO.Entry +
                        "\"  target=\"_blank\">on WowHead</a></i>)</td>     <td>WoWUnit (" + wowO.Guid.GetWoWType + ")</td>     <td bgcolor=\"#CCCCCC\">" +
                        wowO.Entry + "</td>     <td>" + wowO.Position.X + "</td>     <td bgcolor=\"#CCCCCC\">" +
                        wowO.Position.Y + "</td>     <td>" + wowO.Position.Z + "</td>     <td bgcolor=\"#CCCCCC\">" +
                        wowO.GetDistance + "</td>     <td>" + wowO.Faction + "</td>     <td bgcolor=\"#CCCCCC\">" +
                        wowO.Guid + "</td>     <td>" + wowO.SummonedBy + "</td>        <td>" + wowO.CreatedBy +
                        "</td>   </tr>";
                }
                // WoWGameObject
                foreach (WoWGameObject wowO in ObjectManager.GetObjectWoWGameObject())
                {
                    Application.DoEvents();
                    codeHtml +=
                        "<tr>     <td bgcolor=\"#CCCCCC\">" + wowO.Name + " - (<i><a href=\"http://wowhead.com/object=" +
                        wowO.Entry +
                        "\"  target=\"_blank\">on WowHead</a></i>)</td>     <td>WoWGameObject (" + wowO.Guid.GetWoWType + ")</td>     <td bgcolor=\"#CCCCCC\">" +
                        wowO.Entry + "</td>     <td>" + wowO.Position.X + "</td>     <td bgcolor=\"#CCCCCC\">" +
                        wowO.Position.Y + "</td>     <td>" + wowO.Position.Z + "</td>     <td bgcolor=\"#CCCCCC\">" +
                        wowO.GetDistance + "</td>     <td>-</td>     <td bgcolor=\"#CCCCCC\">" + wowO.Guid +
                        "</td>     <td>" + wowO.CreatedBy + "</td>   </tr>";
                }
                // WoWItem
                foreach (WoWItem wowO in ObjectManager.GetObjectWoWItem())
                {
                    Application.DoEvents();
                    codeHtml +=
                        "<tr>     <td bgcolor=\"#CCCCCC\">" + wowO.Name + " - (<i><a href=\"http://wowhead.com/item=" +
                        wowO.Entry +
                        "\"  target=\"_blank\">on WowHead</a></i>)</td>     <td>WoWItem (" + wowO.Guid.GetWoWType + ";" + wowO.Guid.GetWoWSubType + ")</td>     <td bgcolor=\"#CCCCCC\">" +
                        wowO.Entry +
                        "</td>     <td>-</td>     <td bgcolor=\"#CCCCCC\">-</td>     <td>-</td>     <td bgcolor=\"#CCCCCC\">-</td>     <td>-</td>     <td bgcolor=\"#CCCCCC\">" +
                        wowO.Guid + "</td>     <td>" + wowO.Owner +
                        "</td><td>IsEquipped by me ? " + EquippedItems.IsEquippedItemByGuid(wowO.Guid) + "</td>   </tr>";
                }
                // WoWCorpse
                foreach (WoWCorpse wowO in ObjectManager.GetObjectWoWCorpse())
                {
                    Application.DoEvents();
                    codeHtml +=
                        "<tr>     <td bgcolor=\"#CCCCCC\">" + wowO.Name +
                        "</td>     <td>WoWCorpse (" + wowO.Guid.GetWoWType + "</td>     <td bgcolor=\"#CCCCCC\">" + wowO.Entry + "</td>     <td>" +
                        wowO.Position.X + "</td>     <td bgcolor=\"#CCCCCC\">" + wowO.Position.Y + "</td>     <td>" +
                        wowO.Position.Z + "</td>     <td bgcolor=\"#CCCCCC\">" + wowO.GetDistance +
                        "</td>     <td>-</td>     <td bgcolor=\"#CCCCCC\">" + wowO.Guid + "</td>     <td>-</td>   </tr>";
                }
                // WoWContainer
                foreach (WoWContainer wowO in ObjectManager.GetObjectWoWContainer())
                {
                    Application.DoEvents();
                    codeHtml +=
                        "<tr>     <td bgcolor=\"#CCCCCC\">" + wowO.Name + " - (<i><a href=\"http://wowhead.com/item=" +
                        wowO.Entry +
                        "\"  target=\"_blank\">on WowHead</a></i>)</td>     <td>WoWContainer (" + wowO.Guid.GetWoWType + ")</td>     <td bgcolor=\"#CCCCCC\">" +
                        wowO.Entry +
                        "</td>     <td>-</td>     <td bgcolor=\"#CCCCCC\">-</td>     <td>-</td>     <td bgcolor=\"#CCCCCC\">-</td>     <td>-</td>     <td bgcolor=\"#CCCCCC\">" +
                        wowO.Guid + "</td>     <td>-</td>   </tr>";
                }


                codeHtml += " </table> </body> </html>";
                Others.WriteFile("AllObjectsButton.html", codeHtml);

                var myInfo = new Process
                {
                    StartInfo =
                    {
                        FileName = "AllObjectsButton.html",
                        WorkingDirectory = Application.StartupPath
                    }
                };
                myInfo.Start();
            }
            catch (Exception ex)
            {
                Logging.WriteError("DeveloperToolsMainFrame > AllObjectsButton_Click(object sender, EventArgs e): " + ex);
            }
            AllObjectsButton.Enabled = true;
        }

        private void TargetInfo2Button_Click(object sender, EventArgs e)
        {
            try
            {
                TargetInfo2Button.Enabled = false;
                InformationArea.Text = "";
                if (ObjectManager.Target.IsValid)
                {
                    string questStatusText = "";
                    if (ObjectManager.Target.GetDescriptor<UnitNPCFlags>(Descriptors.UnitFields.NpcFlags).HasFlag(UnitNPCFlags.QuestGiver))
                    {
                        var questStatusFlag =
                            (UnitQuestGiverStatus) Memory.WowMemory.Memory.ReadInt(ObjectManager.Target.GetBaseAddress + (uint) Addresses.Quests.QuestGiverStatus);
                        if (questStatusFlag > 0x0)
                        {
                            questStatusText = "QuestGiverStatus: " + questStatusFlag + Environment.NewLine;
                        }
                    }
                    InformationArea.Text =
                        "Name: " + ObjectManager.Target.Name + Environment.NewLine +
                        "BaseAddress: " + ObjectManager.Target.GetBaseAddress + Environment.NewLine +
                        "Entry: " + ObjectManager.Target.Entry + Environment.NewLine +
                        "Position: " + ObjectManager.Target.Position + Environment.NewLine +
                        "Faction: " + (Npc.FactionType) Enum.Parse(typeof (Npc.FactionType), ObjectManager.Me.PlayerFaction, true) + Environment.NewLine +
                        "ContinentId: " + Usefuls.ContinentNameByContinentId(Usefuls.ContinentId) + " (" + Usefuls.ContinentId + ")" + Environment.NewLine +
                        "IsDead : " + ObjectManager.Target.IsDead + Environment.NewLine +
                        "IsTrivial : " + ObjectManager.Target.IsTrivial + Environment.NewLine +
                        "UnitFlag: " + ObjectManager.Target.GetDescriptor<UnitFlags>(Descriptors.UnitFields.Flags) + Environment.NewLine +
                        "UnitFlag2: " + ObjectManager.Target.GetDescriptor<UnitFlags2>(Descriptors.UnitFields.Flags2) + Environment.NewLine +
                        "NPCFlag: " + ObjectManager.Target.GetDescriptor<UnitNPCFlags>(Descriptors.UnitFields.NpcFlags) + Environment.NewLine +
                        questStatusText +
                        "DynamicFlag: " + ObjectManager.Target.GetDescriptor<UnitDynamicFlags>(Descriptors.ObjectFields.DynamicFlags) + Environment.NewLine;
                    if (ObjectManager.Target.GetDescriptor<UnitNPCFlags>(Descriptors.UnitFields.NpcFlags).HasFlag(UnitNPCFlags.Taxi))
                    {
                        InformationArea.Text += "If you have the TaxiWindow opened while requesting those informations, TaxiNodes will be dumped to DebugLog" + Environment.NewLine;
                        Gossip.ExportTaxiInfo();
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("DeveloperToolsMainFrame > TargetInfo2Button_Click(object sender, EventArgs e): " + ex);
            }
            TargetInfo2Button.Enabled = true;
        }

        private void NpcFactionButton_Click(object sender, EventArgs e)
        {
            try
            {
                NpcFactionButton.Enabled = false;
                InformationArea.Text = "";

                foreach (Npc.FactionType en in Enum.GetValues(typeof (Npc.FactionType)).Cast<Npc.FactionType>().ToList())
                {
                    InformationArea.Text += en + Environment.NewLine;
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("DeveloperToolsMainFrame > NpcFactionButton_Click(object sender, EventArgs e): " + ex);
            }
            NpcFactionButton.Enabled = true;
        }

        private void GpsButton_Click(object sender, EventArgs e)
        {
            try
            {
                GpsButton.Enabled = false;
                InformationArea.Text = "";
                if (ObjectManager.Me.IsValid)
                {
                    InformationArea.Text =
                        "Internal Map name: " + Usefuls.ContinentNameMpqByContinentId(Usefuls.RealContinentId) + " (" + Usefuls.RealContinentId + ")" + Environment.NewLine +
                        "" + ObjectManager.Me.Position + Environment.NewLine +
                        "" + Environment.NewLine +
                        "" + Environment.NewLine +
                        "<Position>" + Environment.NewLine +
                        " <X>" + ObjectManager.Me.Position.X + "</X>" + Environment.NewLine +
                        " <Y>" + ObjectManager.Me.Position.Y + "</Y>" + Environment.NewLine +
                        " <Z>" + ObjectManager.Me.Position.Z + "</Z>" + Environment.NewLine +
                        " <Type>" + ObjectManager.Me.Position.Type + "</Type>" + Environment.NewLine +
                        "</Position>";
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("DeveloperToolsMainFrame > GpsButton_Click(object sender, EventArgs e): " + ex);
            }
            GpsButton.Enabled = true;
        }

        private void NpcTypeButton_Click(object sender, EventArgs e)
        {
            try
            {
                NpcTypeButton.Enabled = false;
                InformationArea.Text = "";

                foreach (Npc.NpcType en in Enum.GetValues(typeof (Npc.NpcType)).Cast<Npc.NpcType>().ToList())
                {
                    InformationArea.Text += en + Environment.NewLine;
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("DeveloperToolsMainFrame > NpcTypeButton_Click(object sender, EventArgs e): " + ex);
            }
            NpcTypeButton.Enabled = true;
        }

        private void LuaExecButton_Click(object sender, EventArgs e)
        {
            try
            {
                LuaExecButton.Enabled = false;
                Lua.LuaDoString("SetCVar(\"ScriptErrors\", \"1\")");
                Lua.LuaDoString(InformationArea.Text);
                Lua.LuaDoString("SetCVar(\"ScriptErrors\", \"0\")");
            }
            catch (Exception ex)
            {
                Logging.WriteError("DeveloperToolsMainFrame > LuaExecButton_Click(object sender, EventArgs e): " + ex);
            }
            LuaExecButton.Enabled = true;
        }

        private void CsharpExecButton_Click(object sender, EventArgs e)
        {
            try
            {
                CsharpExecButton.Enabled = false;

                ExecuteScript();
            }
            catch (Exception ex)
            {
                Logging.WriteError("DeveloperToolsMainFrame > CsharpExecButton_Click(object sender, EventArgs e): " + ex);
            }
            CsharpExecButton.Enabled = true;
        }

        private void TargetInfoButton_Click(object sender, EventArgs e)
        {
            try
            {
                TargetInfoButton.Enabled = false;
                InformationArea.Text = "";
                if (!ObjectManager.Target.IsValid)
                {
                    TargetInfoButton.Enabled = true;
                    return;
                }
                Point pos = ObjectManager.Target.Position;
                if (Usefuls.IsOutdoors)
                    pos.Type = "Flying";
                InformationArea.Text =
                    "  <Npc>" + Environment.NewLine +
                    "    <Entry>" + ObjectManager.Target.Entry + "</Entry>" + Environment.NewLine +
                    "    <Name>" + ObjectManager.Target.Name + "</Name>" + Environment.NewLine +
                    "    <Position>" + Environment.NewLine +
                    "      <X>" + pos.X + "</X>" + Environment.NewLine +
                    "      <Y>" + pos.Y + "</Y>" + Environment.NewLine +
                    "      <Z>" + pos.Z + "</Z>" + Environment.NewLine +
                    "      <Type>" + pos.Type + "</Type>" + Environment.NewLine +
                    "    </Position>" + Environment.NewLine +
                    "    <Faction>" +
                    (Npc.FactionType) Enum.Parse(typeof (Npc.FactionType), ObjectManager.Me.PlayerFaction, true) +
                    "</Faction>" + Environment.NewLine +
                    "    <Type>None</Type>" + Environment.NewLine +
                    "    <ContinentId>" + Usefuls.ContinentNameByContinentId(Usefuls.ContinentId) + "</ContinentId>" +
                    Environment.NewLine +
                    "  </Npc>" +
                    Environment.NewLine + Environment.NewLine +
                    "Distance: " + pos.DistanceTo(ObjectManager.Me.Position);
            }
            catch (Exception ex)
            {
                Logging.WriteError("DeveloperToolsMainFrame > TargetInfoButton_Click(object sender, EventArgs e): " + ex);
            }
            TargetInfoButton.Enabled = true;
        }

        internal void ExecuteScript()
        {
            try
            {
                CodeDomProvider cc = new CSharpCodeProvider();
                var cp = new CompilerParameters();

                IEnumerable<string> assemblies = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .Where(
                        a =>
                            !a.IsDynamic &&
                            !a.CodeBase.Contains((Process.GetCurrentProcess().ProcessName + ".exe")))
                    .Select(a => a.Location);
                cp.ReferencedAssemblies.AddRange(assemblies.ToArray());

                CompilerResults cr = cc.CompileAssemblyFromSource(cp,
                    " public class Main : nManager.Helpful.Interface.IScriptOnlineManager { public void Initialize() { " +
                    InformationArea.Text + " } } ");
                if (cr.Errors.HasErrors)
                {
                    String text = cr.Errors.Cast<CompilerError>()
                        .Aggregate("Compilator Error :\n", (current, err) => current + (err + "\n"));
                    MessageBox.Show(text);
                    return;
                }

                Assembly assembly = cr.CompiledAssembly;

                object obj = assembly.CreateInstance("Main", true);
                var instanceFromOtherAssembly = obj as IScriptOnlineManager;


                if (instanceFromOtherAssembly != null)
                    instanceFromOtherAssembly.Initialize();
                else
                    Logging.WriteError("DeveloperToolsMainFrame > ExecuteScript()#2");
            }
            catch (Exception e)
            {
                Logging.WriteError("DeveloperToolsMainFrame > ExecuteScript()#1: " + e);
            }
        }

        private void LuaExecButton_MouseEnter(object sender, EventArgs e)
        {
            LuaExecButton.Image = Resources.greenB_150;
        }

        private void LuaExecButton_MouseLeave(object sender, EventArgs e)
        {
            LuaExecButton.Image = Resources.blueB_150;
        }

        private void CsharpExecButton_MouseEnter(object sender, EventArgs e)
        {
            CsharpExecButton.Image = Resources.greenB_150;
        }

        private void CsharpExecButton_MouseLeave(object sender, EventArgs e)
        {
            CsharpExecButton.Image = Resources.blueB_150;
        }

        private void GpsButton_MouseEnter(object sender, EventArgs e)
        {
            GpsButton.Image = Resources.greenB;
        }

        private void GpsButton_MouseLeave(object sender, EventArgs e)
        {
            GpsButton.Image = Resources.blackB;
        }

        private void NpcTypeButton_MouseEnter(object sender, EventArgs e)
        {
            NpcTypeButton.Image = Resources.greenB;
        }

        private void NpcTypeButton_MouseLeave(object sender, EventArgs e)
        {
            NpcTypeButton.Image = Resources.blackB;
        }

        private void NpcFactionButton_MouseEnter(object sender, EventArgs e)
        {
            NpcFactionButton.Image = Resources.greenB;
        }

        private void NpcFactionButton_MouseLeave(object sender, EventArgs e)
        {
            NpcFactionButton.Image = Resources.blackB;
        }

        private void TargetInfo2Button_MouseEnter(object sender, EventArgs e)
        {
            TargetInfo2Button.Image = Resources.greenB;
        }

        private void TargetInfo2Button_MouseLeave(object sender, EventArgs e)
        {
            TargetInfo2Button.Image = Resources.blackB;
        }

        private void TranslationManagerButton_MouseEnter(object sender, EventArgs e)
        {
            TranslationManagerButton.Image = Resources.greenB_242;
        }

        private void TranslationManagerButton_MouseLeave(object sender, EventArgs e)
        {
            TranslationManagerButton.Image = Resources.blueB_242;
        }

        private void AllObjectsButton_MouseEnter(object sender, EventArgs e)
        {
            AllObjectsButton.Image = Resources.greenB_242;
        }

        private void AllObjectsButton_MouseLeave(object sender, EventArgs e)
        {
            AllObjectsButton.Image = Resources.blackB_242;
        }
    }
}