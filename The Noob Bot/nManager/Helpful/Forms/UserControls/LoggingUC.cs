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
                Logging.OnChanged += SynchroniseLoggin;

                CbCheckedChanged(null, null);
            }
            catch (Exception e)
            {
                Logging.WriteError("LoggingUC(): " + e);
            }
        }

        private void Translate()
        {
            labelX1.Text = nManager.Translate.Get(nManager.Translate.Id.Normal);
            labelX2.Text = nManager.Translate.Get(nManager.Translate.Id.Debug);
            labelX3.Text = nManager.Translate.Get(nManager.Translate.Id.Navigator);
            labelX4.Text = nManager.Translate.Get(nManager.Translate.Id.Fight);
        }

        private List<Logging.Log> _listLog = new List<Logging.Log>();

        private void SynchroniseLoggin(object sender, Logging.LoggingChangeEventArgs e)
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
                        richTextBox.AppendText(_listLog[0].ToString());
                        richTextBox.Select(richTextBox.Text.Length - _listLog[0].ToString().Length,
                                           _listLog[0].ToString().Length);
                        richTextBox.SelectionColor = _listLog[0].Color;
                        richTextBox.AppendText(Environment.NewLine);
                        _listLog.RemoveAt(0);
                        richTextBox.ScrollToCaret();
                    }
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("AddLog(): " + e);
            }
        }

        private void LoggingUcControlRemoved(object sender, ControlEventArgs e)
        {
            try
            {
                updateLog.Enabled = false;
                Logging.OnChanged -= SynchroniseLoggin;
            }
            catch (Exception ex)
            {
                Logging.WriteError("LoggingUcControlRemoved(object sender, ControlEventArgs e): " + ex);
            }
        }

        private void UpdateLogTick(object sender, EventArgs e)
        {
            try
            {
                AddLog();
            }
            catch (Exception ex)
            {
                Logging.WriteError("UpdateLogTick(object sender, EventArgs e): " + ex);
            }
        }

        private Logging.LogType GetFlag()
        {
            try
            {
                Logging.LogType flag = Logging.LogType.None;

                if (normalCb.Value)
                    flag |= Logging.LogType.S;
                if (debugCb.Value)
                {
                    flag |= Logging.LogType.D;
                    flag |= Logging.LogType.E;
                }
                if (fightCb.Value)
                    flag |= Logging.LogType.F;
                if (navigatorCb.Value)
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
                    richTextBox.Clear();
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("CbCheckedChanged(object sender, EventArgs e): " + ex);
            }
        }
    }
}