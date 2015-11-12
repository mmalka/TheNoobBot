using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace nManager.Helpful.Forms.UserControls
{
    public partial class LoggingUC : UserControl
    {
        public LoggingUC()
        {
            try
            {
                InitializeComponent();
                Translate();
                Logging.OnChanged += SynchroniseLogging;

                CbCheckedChanged(null, null);
            }
            catch (Exception e)
            {
                Logging.WriteError("LoggingUC(): " + e);
            }
        }

        private void Translate()
        {
            NormalLogSwitchLabel.Text = nManager.Translate.Get(nManager.Translate.Id.Normal);
            FightLogSwitchLabel.Text = nManager.Translate.Get(nManager.Translate.Id.Fight);
            NavigationLogSwitchLabel.Text = nManager.Translate.Get(nManager.Translate.Id.Navigator);
            DebugLogSwitchLabel.Text = nManager.Translate.Get(nManager.Translate.Id.Debug);
        }

        private readonly List<Logging.Log> _listLog = new List<Logging.Log>();

        private void SynchroniseLogging(object sender, Logging.LoggingChangeEventArgs e)
        {
            try
            {
                lock (this)
                {
                    if ((e.Log.LogType & GetFlag()) == e.Log.LogType)
                        _listLog.Add(e.Log);
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("SynchroniseLoggin(object sender, Logging.LoggingChangeEventArgs e): " + ex);
            }
        }

        private void AddLog()
        {
            try
            {
                lock (this)
                {
                    if (_listLog.Count > 0)
                    {
                        LoggingTextArea.AppendText(_listLog[0].ToString());
                        LoggingTextArea.Select(LoggingTextArea.Text.Length - _listLog[0].ToString().Length,
                            _listLog[0].ToString().Length);
                        LoggingTextArea.SelectionColor = _listLog[0].Color;
                        LoggingTextArea.AppendText(Environment.NewLine);
                        _listLog.RemoveAt(0);
                        LoggingTextArea.ScrollToCaret();
                    }
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("AddLog(): " + e);
            }
        }

        private Logging.LogType GetFlag()
        {
            try
            {
                Logging.LogType flag = Logging.LogType.None;

                if (NormalLogSwitchButton.Value)
                    flag |= Logging.LogType.S;
                    flag |= Logging.LogType.P;
                if (DebugLogSwitchButton.Value)
                {
                    flag |= Logging.LogType.D;
                    flag |= Logging.LogType.E;
                    flag |= Logging.LogType.EP;
                    flag |= Logging.LogType.DP;
                }
                if (FightLogSwitchButton.Value)
                    flag |= Logging.LogType.F;
                if (NavigationLogSwitchButton.Value)
                    flag |= Logging.LogType.N;

                return flag;
            }
            catch (Exception ex)
            {
                Logging.WriteError("GetFlag(): " + ex);
            }
            return Logging.LogType.None;
        }

        private void CbCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                lock (this)
                {
                    _listLog.Clear();
                    _listLog.AddRange(Logging.ReadList(GetFlag()));
                    LoggingTextArea.Clear();
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("CbCheckedChanged(object sender, EventArgs e): " + ex);
            }
        }

        private void LoggingAreaTimer_Tick(object sender, EventArgs e)
        {
            AddLog();
        }

        private void LoggingTextArea_VisibleChanged(object sender, EventArgs e)
        {
            if (LoggingTextArea.Visible)
            {
                LoggingTextArea.SelectionStart = LoggingTextArea.TextLength;
                LoggingTextArea.ScrollToCaret();
            }
        }

        private void LoggingSwitchs_ValueChanged(object sender, EventArgs e)
        {
            lock (this)
            {
                _listLog.Clear();
                _listLog.AddRange(Logging.ReadList(GetFlag(), true));
                LoggingTextArea.Clear();
            }
        }

        private void LoggingUC_ControlRemoved(object sender, ControlEventArgs e)
        {
            LoggingAreaTimer.Enabled = false;
            Logging.OnChanged -= SynchroniseLogging;
        }
    }
}