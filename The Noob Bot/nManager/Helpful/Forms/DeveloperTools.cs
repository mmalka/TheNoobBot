using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Microsoft.CSharp;
using nManager.Helpful.Interface;
using nManager.Wow.Class;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

namespace nManager.Helpful.Forms
{
    public partial class Developer_Tools : DevComponents.DotNetBar.Metro.MetroForm
    {
        public Developer_Tools()
        {
            try
            {
                InitializeComponent();
                TranslateForm();
                if (nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature)
                    this.TopMost = true;
            }
            catch (Exception ex)
            {
                Logging.WriteError("Developer_Tools > Developer_Tools(): " + ex);
            }
        }

        private void TranslateForm()
        {
            getAllInfo.Text = Translate.Get(Translate.Id.Get_informations_of_all_ingame_objects);
            targetInfoB.Text = Translate.Get(Translate.Id.Get_Taget_informations);
            myPositionB.Text = Translate.Get(Translate.Id.Get_My_Position);
            npcFactionListB.Text = Translate.Get(Translate.Id.Npc_faction_list);
            addByNameNpcB.Text = Translate.Get(Translate.Id.Get_infomations_by_name);
            nameNpcTb.Text = Translate.Get(Translate.Id.Name);
            launchLuaB.Text = Translate.Get(Translate.Id.Launch_Lua_script);
            launchCSharpScriptB.Text = Translate.Get(Translate.Id.Launch_C__script);
            translateToolsB.Text = Translate.Get(Translate.Id.Translate_Tools);
            Text = Translate.Get(Translate.Id.Developer_Tools);
        }

        private void getAllInfo_Click(object sender, EventArgs e)
        {
            try
            {
                var codeHtml =
                    "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"> <html xmlns=\"http://www.w3.org/1999/xhtml\"> <head> <meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" /> <title>Get all objects information - " +
                    DateTime.Now.ToString("dd/mm/yy HHh mmMin") +
                    "</title>  <script type\"text/javascript\"> function trouverMots(chaine) { /* document.getElementById('rechDsPg').value = ''; */  var ouvrirBalise = '<span style=\"background-color: '; var frmOvrBalise = ';\">'; var fermerBalise = '</span>'; var doc = document.body.innerHTML; var j = 0;  var arrayClrs = new Array(\"#FFFF00\", \"#66FFFF\", \"#33FF33\", \"#3333FF\", \"#FF9900\", \"#FF33FF\", \"#CCFF00\", \"#FF0000\");  tablMots = chaine.split(' ');  rchSupp = new RegExp( '(' + ouvrirBalise + '[^><]*>)' , 'gi'); doc = doc.replace(rchSupp, ''); rchSupp = new RegExp( '(' + fermerBalise + ')' , 'gi'); doc = doc.replace(rchSupp, '');  for (i = 0; i < tablMots.length; i++) { if (j >= arrayClrs.length) {j = 0;} if (tablMots[i] != '' && tablMots[i].length > 2) { rch = new RegExp( '(' + tablMots[i] + ')' , 'gi'); ouvrBalise = ouvrirBalise + arrayClrs[j] + frmOvrBalise; doc = doc.replace(rch, ouvrBalise + '$1' + fermerBalise); j += 1; } }  document.body.innerHTML = doc; } </script>   </head>  <body>  <p> Search : <input id=\"rechDsPg\" type=\"text\" value=\"\" name=\"rechDsPg\" /> <input type=\"button\" onclick=\"trouverMots(document.getElementById('rechDsPg').value);\" value=\"OK\"> </p>   <table width=\"100%\" border=\"1\">   <tr>   <b>     <td bgcolor=\"#CCCCCC\">Name</td>     <td>Type</td>     <td bgcolor=\"#CCCCCC\">Entry ID</td>     <td>Position X</td>     <td bgcolor=\"#CCCCCC\">Position Y</td>     <td>Position Z</td>     <td bgcolor=\"#CCCCCC\">Distance</td>     <td>Faction</td>     <td bgcolor=\"#CCCCCC\">GUID</td>     <td>Summoned/Created By</td> <td>Unit Created By</td>    </b>   </tr>  ";
                // Me
                codeHtml +=
                    "<tr>     <td bgcolor=\"#CCCCCC\">" + ObjectManager.Me.Name +
                    "</td>     <td>WoWPlayer</td>     <td bgcolor=\"#CCCCCC\">" + ObjectManager.Me.Entry +
                    "</td>     <td>" + ObjectManager.Me.Position.X + "</td>     <td bgcolor=\"#CCCCCC\">" +
                    ObjectManager.Me.Position.Y + "</td>     <td>" + ObjectManager.Me.Position.Z +
                    "</td>     <td bgcolor=\"#CCCCCC\">" + ObjectManager.Me.GetDistance + "</td>     <td>" +
                    ObjectManager.Me.Faction + "</td>     <td bgcolor=\"#CCCCCC\">" + ObjectManager.Me.Guid +
                    "</td>     <td>" + ObjectManager.Me.SummonedBy + "</td>   </tr>";

                // WoWPlayer
                foreach (var woWPlayer in ObjectManager.GetObjectWoWPlayer())
                {
                    codeHtml +=
                        "<tr>     <td bgcolor=\"#CCCCCC\">" + woWPlayer.Name +
                        "</td>     <td>WoWPlayer</td>     <td bgcolor=\"#CCCCCC\">" + woWPlayer.Entry + "</td>     <td>" +
                        woWPlayer.Position.X + "</td>     <td bgcolor=\"#CCCCCC\">" + woWPlayer.Position.Y +
                        "</td>     <td>" + woWPlayer.Position.Z + "</td>     <td bgcolor=\"#CCCCCC\">" +
                        woWPlayer.GetDistance + "</td>     <td>" + woWPlayer.Faction +
                        "</td>     <td bgcolor=\"#CCCCCC\">" + woWPlayer.Guid + "</td>     <td>" + woWPlayer.SummonedBy +
                        "</td>   </tr>";
                }
                // WoWUnit
                foreach (var wowO in ObjectManager.GetObjectWoWUnit())
                {
                    codeHtml +=
                        "<tr>     <td bgcolor=\"#CCCCCC\">" + wowO.Name + " - (<i><a href=\"http://wowhead.com/npc=" +
                        wowO.Entry +
                        "\"  target=\"_blank\">on WowHead</a></i>)</td>     <td>WoWUnit</td>     <td bgcolor=\"#CCCCCC\">" +
                        wowO.Entry + "</td>     <td>" + wowO.Position.X + "</td>     <td bgcolor=\"#CCCCCC\">" +
                        wowO.Position.Y + "</td>     <td>" + wowO.Position.Z + "</td>     <td bgcolor=\"#CCCCCC\">" +
                        wowO.GetDistance + "</td>     <td>" + wowO.Faction + "</td>     <td bgcolor=\"#CCCCCC\">" +
                        wowO.Guid + "</td>     <td>" + wowO.SummonedBy + "</td>        <td>" + wowO.CreatedBy +
                        "</td>   </tr>";
                }
                // WoWGameObject
                foreach (var wowO in ObjectManager.GetObjectWoWGameObject())
                {
                    codeHtml +=
                        "<tr>     <td bgcolor=\"#CCCCCC\">" + wowO.Name + " - (<i><a href=\"http://wowhead.com/object=" +
                        wowO.Entry +
                        "\"  target=\"_blank\">on WowHead</a></i>)</td>     <td>WoWGameObject</td>     <td bgcolor=\"#CCCCCC\">" +
                        wowO.Entry + "</td>     <td>" + wowO.Position.X + "</td>     <td bgcolor=\"#CCCCCC\">" +
                        wowO.Position.Y + "</td>     <td>" + wowO.Position.Z + "</td>     <td bgcolor=\"#CCCCCC\">" +
                        wowO.GetDistance + "</td>     <td>-</td>     <td bgcolor=\"#CCCCCC\">" + wowO.Guid +
                        "</td>     <td>" + wowO.CreatedBy + "</td>   </tr>";
                }
                // WoWItem
                foreach (var wowO in ObjectManager.GetObjectWoWItem())
                {
                    codeHtml +=
                        "<tr>     <td bgcolor=\"#CCCCCC\">" + wowO.Name + " - (<i><a href=\"http://wowhead.com/item=" +
                        wowO.Entry +
                        "\"  target=\"_blank\">on WowHead</a></i>)</td>     <td>WoWItem</td>     <td bgcolor=\"#CCCCCC\">" +
                        wowO.Entry +
                        "</td>     <td>-</td>     <td bgcolor=\"#CCCCCC\">-</td>     <td>-</td>     <td bgcolor=\"#CCCCCC\">-</td>     <td>-</td>     <td bgcolor=\"#CCCCCC\">-</td>     <td>-</td>   </tr>";
                }
                // WoWCorpse
                foreach (var wowO in ObjectManager.GetObjectWoWCorpse())
                {
                    codeHtml +=
                        "<tr>     <td bgcolor=\"#CCCCCC\">" + wowO.Name +
                        "</td>     <td>WoWCorpse</td>     <td bgcolor=\"#CCCCCC\">" + wowO.Entry + "</td>     <td>" +
                        wowO.Position.X + "</td>     <td bgcolor=\"#CCCCCC\">" + wowO.Position.Y + "</td>     <td>" +
                        wowO.Position.Z + "</td>     <td bgcolor=\"#CCCCCC\">" + wowO.GetDistance +
                        "</td>     <td>-</td>     <td bgcolor=\"#CCCCCC\">" + wowO.Guid + "</td>     <td>-</td>   </tr>";
                }
                // WoWContainer
                foreach (var wowO in ObjectManager.GetObjectWoWContainer())
                {
                    codeHtml +=
                        "<tr>     <td bgcolor=\"#CCCCCC\">" + wowO.Name + " - (<i><a href=\"http://wowhead.com/item=" +
                        wowO.Entry +
                        "\"  target=\"_blank\">on WowHead</a></i>)</td>     <td>WoWContainer</td>     <td bgcolor=\"#CCCCCC\">" +
                        wowO.Entry +
                        "</td>     <td>-</td>     <td bgcolor=\"#CCCCCC\">-</td>     <td>-</td>     <td bgcolor=\"#CCCCCC\">-</td>     <td>-</td>     <td bgcolor=\"#CCCCCC\">" +
                        wowO.Guid + "</td>     <td>-</td>   </tr>";
                }


                codeHtml += " </table> </body> </html>";
                Others.WriteFile("Get all objects information.html", codeHtml);

                var myInfo = new Process
                                 {
                                     StartInfo =
                                         {
                                             FileName = "Get all objects information.html",
                                             WorkingDirectory = Application.StartupPath
                                         }
                                 };
                myInfo.Start();
            }
            catch (Exception ex)
            {
                Logging.WriteError("Developer_Tools > getAllInfo_Click(object sender, EventArgs e): " + ex);
            }
        }

        private void targetInfoB_Click(object sender, EventArgs e)
        {
            try
            {
                infoTb.Text = "";
                if (ObjectManager.Target.IsValid)
                {
                    var pos = ObjectManager.Target.Position;
                    if (Usefuls.IsOutdoors)
                        pos.Type = "Flying";
                    infoTb.Text =
                        "  <Npc>" + Environment.NewLine +
                        "    <Entry>" + ObjectManager.Target.Entry + "</Entry>" + Environment.NewLine +
                        "    <Name>" + ObjectManager.Target.Name + "</Name>" + Environment.NewLine +
                        "    <Position>" + Environment.NewLine +
                        "      <X>" + pos.X.ToString().Replace(",", ".") + "</X>" + Environment.NewLine +
                        "      <Y>" + pos.Y.ToString().Replace(",", ".") + "</Y>" + Environment.NewLine +
                        "      <Z>" + pos.Z.ToString().Replace(",", ".") + "</Z>" + Environment.NewLine +
                        "      <Type>" + pos.Type + "</Type>" + Environment.NewLine +
                        "    </Position>" + Environment.NewLine +
                        "    <Faction>" +
                        (Npc.FactionType) Enum.Parse(typeof (Npc.FactionType), ObjectManager.Me.PlayerFaction, true) +
                        "</Faction>" + Environment.NewLine +
                        "    <Type>None</Type>" + Environment.NewLine +
                        "    <ContinentId>" + (Wow.Enums.ContinentId) (Usefuls.ContinentId) + "</ContinentId>" +
                        Environment.NewLine +
                        "  </Npc>";
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("Developer_Tools > targetInfoB_Click(object sender, EventArgs e): " + ex);
            }
        }

        private void myPositionB_Click(object sender, EventArgs e)
        {
            try
            {
                infoTb.Text = "";
                if (ObjectManager.Me.IsValid)
                {
                    infoTb.Text =
                        "Internal Map name: " + Usefuls.ContinentNameMpq + Environment.NewLine +
                        "" + ObjectManager.Me.Position + Environment.NewLine +
                        "" + Environment.NewLine +
                        "" + Environment.NewLine +
                        "<Position>" + Environment.NewLine +
                        " <X>" + ObjectManager.Me.Position.X.ToString().Replace(",", ".") + "</X>" + Environment.NewLine +
                        " <Y>" + ObjectManager.Me.Position.Y.ToString().Replace(",", ".") + "</Y>" + Environment.NewLine +
                        " <Z>" + ObjectManager.Me.Position.Z.ToString().Replace(",", ".") + "</Z>" + Environment.NewLine +
                        " <Type>" + ObjectManager.Me.Position.Type + "</Type>" + Environment.NewLine +
                        "</Position>";
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("Developer_Tools > myPositionB_Click(object sender, EventArgs e): " + ex);
            }
        }

        private void npcType_Click(object sender, EventArgs e)
        {
            try
            {
                infoTb.Text = "";

                foreach (var en in Enum.GetValues(typeof (Npc.NpcType)).Cast<Npc.NpcType>().ToList())
                {
                    infoTb.Text += en + Environment.NewLine;
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("Developer_Tools > npcType_Click(object sender, EventArgs e): " + ex);
            }
        }

        private void npcFactionListB_Click(object sender, EventArgs e)
        {
            try
            {
                infoTb.Text = "";

                foreach (var en in Enum.GetValues(typeof (Npc.FactionType)).Cast<Npc.FactionType>().ToList())
                {
                    infoTb.Text += en + Environment.NewLine;
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("Developer_Tools > npcFactionListB_Click(object sender, EventArgs e): " + ex);
            }
        }

        private void addByNameNpcB_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ObjectManager.Me.IsValid)
                    return;

                if (string.IsNullOrEmpty(nameNpcTb.Text))
                {
                    MessageBox.Show(Translate.Get(Translate.Id.Name_Empty));
                    return;
                }

                var npc = new Npc();

                var gameObjects = ObjectManager.GetWoWGameObjectByName(nameNpcTb.Text);

                if (gameObjects.Count > 0)
                {
                    var gameObject = ObjectManager.GetNearestWoWGameObject(gameObjects);
                    if (gameObject.IsValid)
                    {
                        npc.Entry = gameObject.Entry;
                        npc.Position = gameObject.Position;
                        npc.Name = gameObject.Name;
                    }
                }

                if (npc.Entry <= 0)
                {
                    var units = ObjectManager.GetWoWUnitByName(nameNpcTb.Text);
                    if (units.Count > 0)
                    {
                        var unit = ObjectManager.GetNearestWoWUnit(units);
                        if (unit.IsValid)
                        {
                            npc.Entry = unit.Entry;
                            npc.Position = unit.Position;
                            npc.Name = unit.Name;
                        }
                    }
                }

                if (npc.Entry <= 0)
                {
                    MessageBox.Show(Translate.Get(Translate.Id.No_found));
                    return;
                }

                npc.ContinentId =
                    (Wow.Enums.ContinentId) (Usefuls.ContinentId);
                npc.Faction =
                    (Npc.FactionType)
                    Enum.Parse(typeof (Npc.FactionType), ObjectManager.Me.PlayerFaction, true);

                if (Usefuls.IsOutdoors)
                    npc.Position.Type = "Flying";

                infoTb.Text = "";
                infoTb.Text =
                    "  <Npc>" + Environment.NewLine +
                    "    <Entry>" + npc.Entry + "</Entry>" + Environment.NewLine +
                    "    <Name>" + npc.Name + "</Name>" + Environment.NewLine +
                    "    <Position>" + Environment.NewLine +
                    "      <X>" + npc.Position.X.ToString().Replace(",", ".") + "</X>" + Environment.NewLine +
                    "      <Y>" + npc.Position.Y.ToString().Replace(",", ".") + "</Y>" + Environment.NewLine +
                    "      <Z>" + npc.Position.Z.ToString().Replace(",", ".") + "</Z>" + Environment.NewLine +
                    "      <Type>" + npc.Position.Type + "</Type>" + Environment.NewLine +
                    "    </Position>" + Environment.NewLine +
                    "    <Faction>" +
                    (Npc.FactionType) Enum.Parse(typeof (Npc.FactionType), ObjectManager.Me.PlayerFaction, true) +
                    "</Faction>" + Environment.NewLine +
                    "    <Type>None</Type>" + Environment.NewLine +
                    "    <ContinentId>" + (Wow.Enums.ContinentId) (Usefuls.ContinentId) + "</ContinentId>" +
                    Environment.NewLine +
                    "  </Npc>";
                nameNpcTb.Text = "";
            }
            catch (Exception ex)
            {
                Logging.WriteError("Developer_Tools > addByNameNpcB_Click(object sender, EventArgs e): " + ex);
            }
        }

        private void launchLuaB_Click(object sender, EventArgs e)
        {
            try
            {
                Lua.LuaDoString("SetCVar(\"ScriptErrors\", \"1\")");
                Lua.LuaDoString(infoTb.Text);
                Lua.LuaDoString("SetCVar(\"ScriptErrors\", \"0\")");
            }
            catch (Exception ex)
            {
                Logging.WriteError("Developer_Tools > launchLuaB_Click(object sender, EventArgs e): " + ex);
            }
        }

        private void translateToolsB_Click(object sender, EventArgs e)
        {
            try
            {
                var t = new Translate_Tools();
                t.Show();
            }
            catch (Exception ex)
            {
                Logging.WriteError("Developer_Tools > translateToolsB_Click(object sender, EventArgs e): " + ex);
            }
        }

        private void launchCSharpScriptB_Click(object sender, EventArgs e)
        {
            LoadScript();
        }

        internal void LoadScript()
        {
            try
            {
                CodeDomProvider cc = new CSharpCodeProvider();
                var cp = new CompilerParameters();

                var assemblies = AppDomain.CurrentDomain
                                          .GetAssemblies()
                                          .Where(
                                              a =>
                                              !a.IsDynamic &&
                                              !a.CodeBase.Contains((Process.GetCurrentProcess().ProcessName + ".exe")))
                                          .Select(a => a.Location);
                cp.ReferencedAssemblies.AddRange(assemblies.ToArray());

                CompilerResults cr = cc.CompileAssemblyFromSource(cp,
                                                                  " public class Main : nManager.Helpful.Interface.IScriptOnlineManager { public void Initialize() { " +
                                                                  infoTb.Text + " } } ");
                if (cr.Errors.HasErrors)
                {
                    String text = cr.Errors.Cast<CompilerError>()
                                    .Aggregate("Compilator Error :\n", (current, err) => current + (err + "\n"));
                    MessageBox.Show(text);
                    return;
                }

                var _assembly = cr.CompiledAssembly;

                var _obj = _assembly.CreateInstance("Main", true);
                var _instanceFromOtherAssembly = _obj as IScriptOnlineManager;


                if (_instanceFromOtherAssembly != null)
                    _instanceFromOtherAssembly.Initialize();
                else
                    Logging.WriteError("grzGRDSFfezfsgfvsdg error");
            }
            catch (Exception e)
            {
                Logging.WriteError("grzGRDSFfezfsgfvsdg#2: " + e);
            }
        }
    }
}