using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful.Forms;
using nManager.Helpful.Forms.UserControls;
using nManager.Properties;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

namespace nManager.Helpful
{
    [Serializable]
    public abstract class Settings
    {
        #region Save

        public bool Save(string settingsPath)
        {
            try
            {
                XmlSerializer.Serialize(settingsPath, this);
                return true;
            }
            catch (Exception e)
            {
                Logging.WriteError("Save(string settingsPath): " + e);
            }
            return false;
        }

        #endregion Save

        #region Load

        public static T Load<T>(string settingsPath)
        {
            try
            {
                if (File.Exists(settingsPath))
                {
                    var t = XmlSerializer.Deserialize<T>(settingsPath);
                    return t;
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("Load<T>(string settingsPath): " + e);
            }

            return default(T);
        }

        #endregion Load

        public static string AdviserFilePathAndName(string productName)
        {
            string fileName;
            try
            {
                while (ObjectManager.Me.WowClass.ToString() == "None")
                {
                    Thread.Sleep(10);
                    Application.DoEvents();
                }
                fileName = productName + "-" + Others.DelSpecialChar(ObjectManager.Me.Name) + "." + ObjectManager.Me.WowClass + "." + Others.DelSpecialChar(Usefuls.RealmName) + ".xml";
            }
            catch (Exception e)
            {
                Logging.WriteError("AdviserFileName(string productName): " + e);
                fileName = productName + "-null.xml";
            }
            return Application.StartupPath + "\\Settings\\" + fileName;
        }

        #region Winform

        private const int LineSpacing = 12;
        private const int LineSpacingLabel = 5;
        private readonly List<FormSetting> _listFormSetting = new List<FormSetting>();
        private readonly ComponentResourceManager _resources = new ComponentResourceManager(typeof (DeveloperToolsMainFrame));
        private Form _mainForm;
        private TnbControlMenu _mainHeader;
        private TnbRibbonManager _mainPanel;
        private string _windowName = "Settings";

        public void ToForm()
        {
            try
            {
                // Create Form
                _mainForm = new Form
                {
                    BackColor = Color.FromArgb(232, 232, 232),
                    Size = new Size(575, 403),
                    FormBorderStyle = FormBorderStyle.None,
                    Name = "MainForm",
                    ShowIcon = false,
                    StartPosition = FormStartPosition.CenterParent,
                    Text = _windowName,
                    Icon = ((Icon) (_resources.GetObject("$this.Icon"))),
                    BackgroundImage = Resources.backgroundCustomSettings,
                    Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, (0))
                };
                _mainForm.Load += GiveFocusToMainPanel;
                _mainForm.Click += GiveFocusToMainPanel;
                if (nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature)
                    _mainForm.TopMost = true;
                _mainHeader = new TnbControlMenu
                {
                    BackgroundImageLayout = ImageLayout.None,
                    Location = new Point(0, 0),
                    Margin = new Padding(0),
                    Name = "MainHeader",
                    Size = new Size(575, 43),
                    TitleFont = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0),
                    TitleForeColor = Color.FromArgb(250, 250, 250),
                    TitleText = _windowName,
                };

                _mainForm.Controls.Add(_mainHeader);
                _mainPanel = new TnbRibbonManager
                {
                    Anchor = AnchorStyles.None,
                    AutoScroll = true,
                    AutoScrollMinSize = new Size(0, 361),
                    BackColor = Color.Transparent,
                    ForeColor = Color.FromArgb(52, 52, 52),
                    Location = new Point(-7, 24),
                    TabIndex = 0,
                    Name = "MainPanel",
                    Size = new Size(573, 359),
                };
                _mainForm.Controls.Add(_mainPanel);
                var listExpandablePanel = new List<TnbExpendablePanel>();
                var lastLineY = new Dictionary<string, int>();

                foreach (FormSetting f in _listFormSetting)
                {
                    Label label = null;

                    int indexTab = -1;
                    for (int i = listExpandablePanel.Count - 1; i >= 0; i--)
                    {
                        if (listExpandablePanel[i].TitleText != f.Category) continue;
                        indexTab = i;
                        break;
                    }
                    if (indexTab < 0)
                    {
                        var newPanel = new TnbExpendablePanel
                        {
                            BackColor = Color.FromArgb(232, 232, 232),
                            BorderColor = Color.FromArgb(52, 52, 52),
                            BorderStyle = ButtonBorderStyle.Solid,
                            ContentSize = new Size(556, 0),
                            Fold = false,
                            AutoSize = true,
                            HeaderBackColor = Color.FromArgb(250, 250, 250),
                            HeaderSize = new Size(556, 36),
                            Location = new Point(0, 0),
                            Margin = new Padding(0),
                            Padding = new Padding(0, 0, 0, LineSpacing),
                            Size = new Size(556, 36),
                            TitleFont = new Font("Segoe UI", 7.65F, FontStyle.Bold),
                            TitleForeColor = Color.FromArgb(232, 232, 232),
                            TitleText = f.Category,
                        };
                        newPanel.Click += GiveFocusToMainPanel;
                        listExpandablePanel.Add(newPanel);
                        lastLineY.Add(f.Category, newPanel.HeaderSize.Height + LineSpacing);
                        indexTab = listExpandablePanel.Count - 1;
                    }
                    int posY;
                    lastLineY.TryGetValue(f.Category, out posY);
                    if (f.FieldName == string.Empty)
                    {
                        // It's a description label for a field.
                        label = new Label
                        {
                            Text = f.Description,
                            Location = new Point(LineSpacing, posY),
                            Size = new Size(80, 17),
                            AutoSize = true,
                            BackColor = Color.FromArgb(232, 232, 232),
                            ForeColor = Color.FromArgb(52, 52, 52)
                        };
                        listExpandablePanel[indexTab].Controls.Add(label);
                        lastLineY.Remove(f.Category);
                        lastLineY.Add(f.Category, posY + LineSpacing);
                        continue;
                    }

                    FieldInfo fieldInfo = GetType().GetField(f.FieldName);
                    if (fieldInfo != null)
                    {
                        // It's a field.
                        NumericUpDown numericUpDown;
                        switch (Type.GetTypeCode(fieldInfo.FieldType))
                        {
                            case TypeCode.Boolean:
                                var switchButton = new TnbSwitchButton
                                {
                                    Value = (bool) fieldInfo.GetValue(this),
                                    ForeColor = Color.FromArgb(52, 52, 52),
                                    Location = new Point(LineSpacing, posY),
                                    Name = f.FieldName,
                                    OffText = Translate.Get(Translate.Id.NO),
                                    OnText = Translate.Get(Translate.Id.YES),
                                    Size = new Size(60, 20),
                                };
                                label = new Label
                                {
                                    Text = f.Description,
                                    Location = new Point(LineSpacing + switchButton.Size.Width + LineSpacing, posY + 4),
                                    Size = new Size(80, 17),
                                    AutoSize = true,
                                    BackColor = Color.Transparent,
                                };
                                listExpandablePanel[indexTab].Controls.Add(label);

                                lastLineY.Remove(f.Category);
                                lastLineY.Add(f.Category, posY + LineSpacing + switchButton.Size.Height);
                                listExpandablePanel[indexTab].Controls.Add(switchButton);
                                break;

                            case TypeCode.Int16:
                            case TypeCode.Int32:
                            case TypeCode.Int64:
                                numericUpDown = new NumericUpDown
                                {
                                    Location = new Point(LineSpacing, posY),
                                    Maximum = new decimal(new[]
                                    {
                                        -1592738368,
                                        7,
                                        0,
                                        0
                                    }),
                                    Minimum = new decimal(new[]
                                    {
                                        -1592738368,
                                        7,
                                        0,
                                        -2147483648
                                    }),
                                    Name = f.FieldName,
                                    Size = new Size(120, 20),
                                    Value = Convert.ToInt64(fieldInfo.GetValue(this))
                                };
                                label = new Label
                                {
                                    Text = f.Description,
                                    Location = new Point(LineSpacing + numericUpDown.Size.Width + LineSpacing, posY + 2),
                                    Size = new Size(80, 17),
                                    AutoSize = true,
                                    BackColor = Color.Transparent,
                                };
                                if (f.SettingsType == "Percentage")
                                {
                                    numericUpDown.Maximum = new decimal(100);
                                    numericUpDown.Minimum = new decimal(0);
                                    numericUpDown.Size = new Size(66, 22);
                                    label.Location = new Point(LineSpacing + numericUpDown.Size.Width + LineSpacing, posY + 2);
                                }
                                lastLineY.Remove(f.Category);
                                lastLineY.Add(f.Category, posY + LineSpacing + numericUpDown.Size.Height);
                                listExpandablePanel[indexTab].Controls.Add(numericUpDown);
                                listExpandablePanel[indexTab].Controls.Add(label);
                                break;

                            case TypeCode.UInt16:
                            case TypeCode.UInt32:
                            case TypeCode.UInt64:
                                numericUpDown = new NumericUpDown
                                {
                                    Location = new Point(LineSpacing, posY),
                                    Maximum = new decimal(new[]
                                    {
                                        -1592738368,
                                        7,
                                        0,
                                        0
                                    }),
                                    Name = f.FieldName,
                                    Size = new Size(120, 20),
                                    Value = Convert.ToUInt64(fieldInfo.GetValue(this))
                                };


                                label = new Label
                                {
                                    Text = f.Description,
                                    Location = new Point(LineSpacing + numericUpDown.Size.Width + LineSpacing, posY + 2),
                                    Size = new Size(80, 17),
                                    AutoSize = true,
                                    BackColor = Color.Transparent,
                                };
                                if (f.SettingsType == "Percentage")
                                {
                                    numericUpDown.Maximum = new decimal(100);
                                    numericUpDown.Minimum = new decimal(0);
                                    numericUpDown.Size = new Size(66, 22);
                                    label.Location = new Point(LineSpacing + numericUpDown.Size.Width + LineSpacing, posY + 2);
                                }
                                lastLineY.Remove(f.Category);
                                lastLineY.Add(f.Category, posY + LineSpacing + numericUpDown.Size.Height);
                                listExpandablePanel[indexTab].Controls.Add(numericUpDown);
                                listExpandablePanel[indexTab].Controls.Add(label);
                                break;

                            case TypeCode.Single:
                            case TypeCode.Double:
                                numericUpDown = new NumericUpDown
                                {
                                    Location = new Point(LineSpacing, posY),
                                    DecimalPlaces = 3,
                                    Increment = new decimal(new[]
                                    {
                                        1,
                                        0,
                                        0,
                                        65536
                                    }),
                                    Maximum = new decimal(new[]
                                    {
                                        -1592738368,
                                        7,
                                        0,
                                        0
                                    }),
                                    Minimum = new decimal(new[]
                                    {
                                        -1592738368,
                                        7,
                                        0,
                                        -2147483648
                                    }),
                                    Name = f.FieldName,
                                    Size = new Size(120, 20),
                                    Value = Convert.ToDecimal(fieldInfo.GetValue(this))
                                };
                                label = new Label
                                {
                                    Text = f.Description,
                                    Location = new Point(LineSpacing + numericUpDown.Size.Width + LineSpacing, posY + 2),
                                    Size = new Size(80, 17),
                                    AutoSize = true,
                                    BackColor = Color.Transparent,
                                };
                                lastLineY.Remove(f.Category);
                                lastLineY.Add(f.Category, posY + LineSpacing + numericUpDown.Size.Height);
                                listExpandablePanel[indexTab].Controls.Add(numericUpDown);
                                listExpandablePanel[indexTab].Controls.Add(label);
                                break;

                            case TypeCode.String:
                                var textBox = new TextBox
                                {
                                    Location = new Point(LineSpacing, posY),
                                    Name = f.FieldName,
                                    Size = new Size(100, 20),
                                    TabIndex = 3,
                                    Text = Convert.ToString(fieldInfo.GetValue(this))
                                };
                                label = new Label
                                {
                                    Text = f.Description,
                                    Location = new Point(LineSpacing + textBox.Size.Width + LineSpacing, posY + 2),
                                    TextAlign = ContentAlignment.MiddleLeft,
                                    Size = new Size(80, 17),
                                    AutoSize = true,
                                    BackColor = Color.Transparent,
                                };
                                if (f.SettingsType == "List")
                                {
                                    label.Size = new Size(532, 20);
                                    label.Location = new Point(LineSpacing, posY + 2);
                                    label.TextAlign = ContentAlignment.BottomLeft;
                                    textBox.Size = new Size(532, 20);
                                    textBox.Location = new Point(LineSpacing, posY + label.Height + LineSpacingLabel);
                                }

                                lastLineY.Remove(f.Category);
                                lastLineY.Add(f.Category, textBox.Location.Y + textBox.Size.Height + LineSpacing);
                                listExpandablePanel[indexTab].Controls.Add(label);
                                listExpandablePanel[indexTab].Controls.Add(textBox);
                                break;
                        }
                        if (f.SettingsType.Contains("Percentage") && f.SettingsType != "Percentage")
                        {
                            int Spacing = 0;
                            if (label != null)
                            {
                                Spacing = label.Size.Width + label.Location.X;
                            }
                            string labelName = "";
                            FieldInfo percentageField = GetType().GetField(f.FieldName + f.SettingsType);
                            if (percentageField != null)
                            {
                                switch (f.SettingsType)
                                {
                                    case "AtPercentage":
                                        labelName = "at this percentage:"; // if (UsePowerWordShieldAtPercentage == 10) 
                                        break;
                                    case "BelowPercentage":
                                        labelName = "below this percentage:"; // if (UsePowerWordShieldBelowPercentage < 10) 
                                        break;
                                    case "AbovePercentage":
                                        labelName = "above this percentage:"; // if (UsePowerWordShieldAbovePercentage > 10) 
                                        break;
                                }
                                var percentageLabel = new Label
                                {
                                    Text = labelName,
                                    Location = new Point(LineSpacing + Spacing, posY + 4),
                                    Size = new Size(90, 17),
                                    AutoSize = true,
                                    BackColor = Color.FromArgb(232, 232, 232),
                                    ForeColor = Color.FromArgb(52, 52, 52),
                                };
                                var percentage = new NumericUpDown
                                {
                                    Location = new Point(LineSpacing + percentageLabel.Location.X + percentageLabel.Width + LineSpacing, posY + 2),
                                    Maximum = new decimal(100),
                                    Minimum = new decimal(0),
                                    Name = f.FieldName + f.SettingsType,
                                    Size = new Size(38, 22),
                                    Value = Convert.ToUInt64(percentageField.GetValue(this))
                                };
                                listExpandablePanel[indexTab].Controls.Add(percentageLabel);
                                listExpandablePanel[indexTab].Controls.Add(percentage);
                            }
                        }
                    }
                }
                bool isFirst = true;
                foreach (TnbExpendablePanel tabPage in listExpandablePanel)
                {
                    if (!isFirst)
                    {
                        tabPage.Fold = true;
                    }
                    isFirst = false;
                    _mainPanel.Controls.Add(tabPage);
                }
                _mainPanel.Click += GiveFocusToMainPanel;
                _mainForm.Shown += GiveFocusToMainPanel;
                _mainForm.ShowDialog();
                ReadForm(_mainForm);
            }
            catch (Exception e)
            {
                Logging.WriteError("Settings > ToForm(): " + e);
            }
        }

        private void GiveFocusToMainPanel(object sender, EventArgs e)
        {
            _mainPanel.Focus();
        }

        private void ReadForm(Form form)
        {
            try
            {
                foreach (FormSetting f in _listFormSetting)
                {
                    if (f.FieldName == string.Empty)
                        continue;

                    Control[] controls = form.Controls.Find(f.FieldName, true);
                    FieldInfo fieldInfo = GetType().GetField(f.FieldName);
                    if (fieldInfo != null && controls.Length > 0)
                    {
                        switch (Type.GetTypeCode(fieldInfo.FieldType))
                        {
                            case TypeCode.Boolean:
                                fieldInfo.SetValue(this, ((TnbSwitchButton) controls[0]).Value);
                                break;

                            case TypeCode.Int16:
                            case TypeCode.Int32:
                            case TypeCode.Int64:
                                fieldInfo.SetValue(this,
                                    Convert.ChangeType(((NumericUpDown) controls[0]).Value,
                                        fieldInfo.FieldType));
                                break;

                            case TypeCode.UInt16:
                            case TypeCode.UInt32:
                            case TypeCode.UInt64:
                                fieldInfo.SetValue(this,
                                    Convert.ChangeType(((NumericUpDown) controls[0]).Value,
                                        fieldInfo.FieldType));
                                break;

                            case TypeCode.Single:
                            case TypeCode.Double:
                                fieldInfo.SetValue(this,
                                    Convert.ChangeType(((NumericUpDown) controls[0]).Value,
                                        fieldInfo.FieldType));
                                break;

                            case TypeCode.String:
                                fieldInfo.SetValue(this, controls[0].Text);
                                break;
                        }
                        if (f.SettingsType.Contains("Percentage") && f.SettingsType != "Percentage")
                        {
                            Control[] controlsP = form.Controls.Find(f.FieldName + f.SettingsType, true);
                            FieldInfo fieldInfoP = GetType().GetField(f.FieldName + f.SettingsType);
                            if (fieldInfoP != null && controlsP.Length > 0)
                                fieldInfoP.SetValue(this,
                                    Convert.ChangeType(((NumericUpDown) controlsP[0]).Value,
                                        fieldInfoP.FieldType));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("Settings > ReadForm(Form form): " + e);
            }
        }

        public void ConfigWinForm(string windowName = "Settings")
        {
            _windowName = windowName;
        }

        protected void AddControlInWinForm(string description, string fieldName, string category = "Main",
            string settingsType = "")
        {
            try
            {
                if (description != string.Empty && category != string.Empty)
                    _listFormSetting.Add(new FormSetting(description, fieldName, category, settingsType));
            }
            catch (Exception e)
            {
                Logging.WriteError(
                    "AddControlInWinForm(string description, string fieldName, string category = \"Main\"): " + e);
            }
        }

        private class FormSetting
        {
            public FormSetting(string description, string fieldName, string category = "Main", string settingsType = "")
            {
                Description = description;
                FieldName = fieldName;
                Category = category;
                SettingsType = settingsType;
            }

            public string SettingsType { get; private set; }
            public string Description { get; private set; }
            public string FieldName { get; private set; }
            public string Category { get; private set; }
        }

        #endregion Winform
    }
}