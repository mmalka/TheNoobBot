using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.MemoryClass;
using Process = nManager.Wow.MemoryClass.Process;

namespace The_Noob_Bot
{
    internal partial class Login : DevComponents.DotNetBar.Metro.MetroForm
    {
        public Login()
        {
            InitializeComponent();
            InitializeProgram();
            Translate();
        }

        void SetToolTypeIfNeeded(Control label)
        {
            using (System.Drawing.Graphics g = CreateGraphics())
            {
                System.Drawing.SizeF size = g.MeasureString(label.Text, label.Font);
                if (size.Width > label.Width)
                {
                    this.labelsToolTip.SetToolTip(label, label.Text);
                }
            }
        }

        void Translate()
        {
            // Choose lang:
            langChooseCb.Items.Clear();
            foreach (var l in Others.GetFilesDirectory(Application.StartupPath + "\\Data\\Lang\\", "*.xml"))
            {
                langChooseCb.Items.Add(l.Remove(l.Length - 1 - 3));
            }

            var langChoosed = "English.xml";
            if (Others.ExistFile(Application.StartupPath + "\\Settings\\lang.txt"))
            {
                var langTemp = Others.ReadFile(Application.StartupPath + "\\Settings\\lang.txt");
                if (!string.IsNullOrEmpty(langTemp))
                {
                    if (Others.ExistFile(Application.StartupPath + "\\Data\\Lang\\" + langTemp))
                        langChoosed = langTemp;
                }
            }
            if (!nManager.Translate.Load(langChoosed))
                return;

            langChooseCb.Text = langChoosed.Remove(langChoosed.Length - 1 - 3);
            langChooseCb.SelectedIndexChanged += langChooseCb_SelectedIndexChanged;

            // Translate text:
            labelX1.Text = nManager.Translate.Get(nManager.Translate.Id.User_Name) + "";
            SetToolTypeIfNeeded(labelX1);
            labelX2.Text = nManager.Translate.Get(nManager.Translate.Id.Password);
            SetToolTypeIfNeeded(labelX2);
            saveCb.Text = nManager.Translate.Get(nManager.Translate.Id.Save);
            SetToolTypeIfNeeded(saveCb);
            createB.Text = nManager.Translate.Get(nManager.Translate.Id.Create);
            SetToolTypeIfNeeded(createB);
            launchBotB.Text = nManager.Translate.Get(nManager.Translate.Id.Launch_Tnb);
            SetToolTypeIfNeeded(launchBotB);
            refreshB.Text = nManager.Translate.Get(nManager.Translate.Id.Refresh);
            SetToolTypeIfNeeded(refreshB);
            Text = nManager.Translate.Get(nManager.Translate.Id.Login);
                
            labelItem1.Text = nManager.Translate.Get(nManager.Translate.Id.Launch_Game);
                
            buttonLaunchWoWDX9.Text = nManager.Translate.Get(nManager.Translate.Id.With) + " DirectX 9";
                
            buttonLaunchWoWDX11.Text = nManager.Translate.Get(nManager.Translate.Id.With) + " DirectX 11";               
        }

        void InitializeProgram()
        {
            try
            {
                // File .exe.config
                var tempsProcess = System.Diagnostics.Process.GetCurrentProcess();
                if (!Others.ExistFile(Application.StartupPath + "\\" + tempsProcess.ProcessName + ".exe.config"))
                {
                    var sw = new StreamWriter(Application.StartupPath + "\\" + tempsProcess.ProcessName + ".exe.config");
                    sw.WriteLine("<?xml version=\"1.0\"?>");
                    sw.WriteLine("<configuration>");
                    sw.WriteLine("<startup useLegacyV2RuntimeActivationPolicy=\"true\">");
                    sw.WriteLine("<supportedRuntime version=\"v4.0\"/>");
                    sw.WriteLine("</startup>");
                    sw.WriteLine("<runtime>");
                    sw.WriteLine("<loadFromRemoteSources enabled=\"true\"/>");
                    sw.WriteLine("</runtime>");
                    sw.WriteLine("</configuration>");
                    sw.Close();

                    System.Diagnostics.Process.Start(Application.StartupPath + "\\" + tempsProcess.ProcessName + ".exe");
                    tempsProcess.Kill();
                }

                langChooseCb.DropDownStyle = ComboBoxStyle.DropDownList;
                Others.GetVisualCpp2010();
            }
            catch (Exception ex)
            {
                Logging.WriteError("Login > InitializeProgram(): " + ex);
            }
        }
        private const string keyNManager = "dfs,kl,se8JDè__fs_vcss454fzdse&é";

        private void launchBotB_Click(object sender, EventArgs e)
        {
            try
            {
                launchBotB.Enabled = false;
                refreshB.Enabled = false;
                launchBotB.Text = nManager.Translate.Get(nManager.Translate.Id.In_progress);
                if (LoginOnServer() && AttachProcess())
                {
                    var formMain = new Main();
                    formMain.Show();
                    Hide();
                }
                launchBotB.Enabled = true;
                refreshB.Enabled = true;
                launchBotB.Text = nManager.Translate.Get(nManager.Translate.Id.Launch_Tnb);
            }
            catch (Exception ex)
            {
                Logging.WriteError("launchBotB_Click(object sender, EventArgs e): " + ex);
            }
        }

        bool AttachProcess()
        {
            try
            {
                if (listProcessLb.SelectedIndex < 0)
                    MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.Please_select_game_Process_and_connect_to_the_game) + ".", nManager.Translate.Get(nManager.Translate.Id.Stop),
                                    MessageBoxButtons.OK, MessageBoxIcon.Stop);

                nManager.Pulsator.Dispose();

                if (listProcessLb.SelectedIndex >= 0)
                {
                    string[] idStringArray = listProcessLb.SelectedItem.ToString().Replace(" ", "").Split(Convert.ToChar("-"));

                    var idProcess = Convert.ToInt32(idStringArray[0]);

                    if (!Hook.IsInGame(idProcess))
                    {
                        MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.Please_connect_to_the_game) + ".", nManager.Translate.Get(nManager.Translate.Id.Stop), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return false;
                    }
                    if (Hook.WowIsUsed(idProcess))
                    {
                        DialogResult resulMb =
                            MessageBox.Show(
                                nManager.Translate.Get(nManager.Translate.Id.The_Game_is_currently_used_by_TheNoobBot_or_contains_traces) + "\n\n" +
                                nManager.Translate.Get(nManager.Translate.Id.If_no_others_session_of_TheNoobBot_is_currently_active),
                                nManager.Translate.Get(nManager.Translate.Id.Use_this_Game) + "?" + @" - " + Hook.PlayerName(idProcess), MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                        if (resulMb == DialogResult.No)
                        {
                            return false;
                        }
                    }

                    nManager.Pulsator.Pulse(idProcess, keyNManager);
                    Logging.Write("Select game process: " + listProcessLb.SelectedItem);
                    if (nManager.Pulsator.IsActive)
                    {
                        if (nManager.Wow.Helpers.Usefuls.InGame || !nManager.Wow.Helpers.Usefuls.IsLoadingOrConnecting)
                            return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("AttachProcess(): " + ex);
            }
            return false;

        }

        bool LoginOnServer()
        {
            try
            {
                if (LoginServer.IsConnected)
                    return true;

                if ((userNameTb.Text.Replace(" ", "") != "" && passwordTb.Text.Replace(" ", "") != ""))
                {
                    userNameTb.Enabled = false;
                    passwordTb.Enabled = false;
                    createB.Enabled = false;
                    saveCb.Enabled = false;

                    if (saveCb.Checked) // Save login and pass
                    {
                        Directory.CreateDirectory(Application.StartupPath + "\\Settings\\");
                        var sw = new StreamWriter(Application.StartupPath + "\\Settings\\.login");
                        sw.WriteLine(Others.StringToEncryptString(userNameTb.Text + "#" + passwordTb.Text));
                        sw.Close();
                    }
                    else // Delete .wr file
                    {
                        string fileToDelete = Application.StartupPath + "\\Settings\\.login";
                        if (File.Exists(fileToDelete))
                            File.Delete(fileToDelete);
                    }

                    LoginServer.Connect(userNameTb.Text, passwordTb.Text);

                    while (!LoginServer.IsConnected)
                    {
                        Application.DoEvents();
                        Thread.Sleep(100);
                    }

                    return LoginServer.IsConnected;
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("LoginOnServer(): " + ex);
            }
            MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.Please_enter_your_user_name_and_password) + ".", nManager.Translate.Get(nManager.Translate.Id.Error), MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        private void Login_Shown(object sender, EventArgs e)
        {
            try
            {
                Text = nManager.Translate.Get(nManager.Translate.Id.Login___The_Noob_Bot_version) + " " + nManager.Information.Version;

                // Load email and password
                if (Others.ExistFile(Application.StartupPath + "\\Settings\\.login"))
                {
                    var strReader = new StreamReader(Application.StartupPath + "\\Settings\\.login", Encoding.Default);
                    try
                    {
                        string texte = Others.EncryptStringToString(strReader.ReadLine());
                        string[] texte2 = texte.Split(Convert.ToChar("#"));
                        userNameTb.Text = texte2[0];
                        passwordTb.Text = texte2[1];
                        if (userNameTb.Text != "")
                        {
                            saveCb.Checked = true;
                        }
                    }
                    catch
                    {
                        userNameTb.Text = "";
                        passwordTb.Text = "";
                        saveCb.Checked = false;
                    }
                    strReader.Close();
                }

                launchBotB.Enabled = false;
                refreshB.Enabled = false;

                launchBotB.Text = nManager.Translate.Get(nManager.Translate.Id.Server_connection) + "...";
                LoginServer.CheckServerIsOnline();
                while (!LoginServer.IsOnlineserver)
                {
                    Thread.Sleep(10);
                    Application.DoEvents(); 
                    Thread.Sleep(50);
                }
                LoginServer.CheckAccountSecurity();
                launchBotB.Text = nManager.Translate.Get(nManager.Translate.Id.Launch_Tnb);
                LoginServer.CheckUpdate();
                launchBotB.Enabled = true;
                refreshB.Enabled = true;

                RefreshProcessList();
            }
            catch (Exception ex)
            {
                Logging.WriteError("Login_Shown(object sender, EventArgs e): " + ex);
            }
        }

        private void refreshB_Click(object sender, EventArgs e)
        {
            try
            {
                RefreshProcessList();
            }
            catch (Exception ex)
            {
                Logging.WriteError("refreshB_Click(object sender, EventArgs e): " + ex);
            }
        }

        private void RefreshProcessList()
        {
            try
            {
                if (System.Diagnostics.Process.GetProcessesByName("WoW-64").Length > 0)
                    MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.WoW_Client_64bit), nManager.Translate.Get(nManager.Translate.Id.Title_WoW_Client_64bit), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                listProcessLb.Items.Clear();
                //ProcessList.SelectedIndex = -1;
                var usedProcess = new List<string>();

                for (var i = Process.ListeProcessIdByName().Length - 1; i >= 0; i--)
                {

                    if (listProcessLb.SelectedIndex == -1 && !Hook.WowIsUsed(Process.ListeProcessIdByName()[i].Id))
                    {
                        listProcessLb.Items.Add(Process.ListeProcessIdByName()[i].Id + " - " + Hook.PlayerName(Process.ListeProcessIdByName()[i].Id));
                        listProcessLb.SelectedIndex = 0;
                    }
                    else
                    {
                        var used = "";
                        if (Hook.WowIsUsed(Process.ListeProcessIdByName()[i].Id))
                            used = " - " + nManager.Translate.Get(nManager.Translate.Id.In_use) + ".";
                        usedProcess.Add(Process.ListeProcessIdByName()[i].Id + " - " + Hook.PlayerName(Process.ListeProcessIdByName()[i].Id) + used);
                    }
                }

                foreach (var v in usedProcess)
                {
                    listProcessLb.Items.Add(v);
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("RefreshProcessList(): " + ex);
            }
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            try
            {
                Others.OpenWebBrowserOrApplication("http://thenoobbot.com/");
            }
            catch (Exception ex)
            {
                Logging.WriteError("Login > buttonX1_Click(object sender, EventArgs e): " + ex);
            }
        }

        private void createB_Click(object sender, EventArgs e)
        {
            try
            {
                Others.OpenWebBrowserOrApplication("http://thenoobbot.com/login/?action=register");
            }
            catch (Exception ex)
            {
                Logging.WriteError("createB_Click(object sender, EventArgs e): " + ex);
            }
        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                nManager.Pulsator.Dispose(true);
            }
            catch (Exception ex)
            {
                Logging.WriteError("Login_FormClosed(object sender, FormClosedEventArgs e): " + ex);
            }
        }

        private void langChooseCb_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Directory.CreateDirectory(Application.StartupPath + "\\Settings\\");
                Others.WriteFile(Application.StartupPath + "\\Settings\\lang.txt", langChooseCb.Text + ".xml");
                Others.OpenWebBrowserOrApplication(System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            catch (Exception ex)
            {
                Logging.WriteError("langChooseCb_SelectedIndexChanged(object sender, EventArgs e): " + ex);
            }
        }

        private void buttonLaunchWoWDX9_Click(object sender, EventArgs e)
        {
            nManager.Wow.Helpers.Usefuls.LaunchWow("-d3d9");
            Thread.Sleep(1000);
            RefreshProcessList();
        }

        private void buttonLaunchWoWDX11_Click(object sender, EventArgs e)
        {
            nManager.Wow.Helpers.Usefuls.LaunchWow("-d3d11");
            Thread.Sleep(1000);
            RefreshProcessList();
        }
    }
}