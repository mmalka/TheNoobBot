using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
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
                fileName = productName + "-" +
                           Others.DelSpecialChar(ObjectManager.Me.Name) + "." +
                           ObjectManager.Me.WowClass + "." + Others.DelSpecialChar(Usefuls.RealmName) + ".xml";
            }
            catch (Exception e)
            {
                Logging.WriteError("AdviserFileName(string productName): " + e);
                fileName = productName + "-null.xml";
            }
            return Application.StartupPath + "\\Settings\\" + fileName;
        }

        #region Winform

        public void ToForm()
        {
            try
            {
                // Create Form

                var form = new DevComponents.DotNetBar.Metro.MetroForm()
                               {
                                   ClientSize = new Size(_sizeWinform),
                                   Text = _windowName,
                                   ShowIcon = false,
                                   ShowInTaskbar = false,
                                   AutoScaleDimensions = new SizeF(6F, 13F),
                                   AutoScaleMode = AutoScaleMode.Font,
                                   Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0),
                                   //ClientSize = new Size(625, 396),
                                   //DoubleBuffered = true,
                               };
                if (nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature)
                    form.TopMost = true;

                if (!_resizableWinform)
                {
                    form.FormBorderStyle = FormBorderStyle.FixedSingle;
                    form.MaximizeBox = false;
                }
                // Create Tab Control
                var panel = new Panel
                                {
                                    Anchor =
                                        ((AnchorStyles.Top |
                                          AnchorStyles.Bottom)
                                         | AnchorStyles.Left)
                                        | AnchorStyles.Right,
                                    AutoScroll = true,
                                    BackColor = Color.Transparent,
                                    Location = new Point(5, 5),
                                    Name = "panel",
                                    Size = new Size(_sizeWinform.X - 10, _sizeWinform.Y - 10)
                                };

                var listExpandablePanel = new List<ExpandablePanel>();
                var expandablePanelPosY = new Dictionary<string, int>();

                foreach (var f in _listFormSetting)
                {
                    Label label;

                    // Create or Seclect Tab:
                    int indexTab = -1;
                    for (int i = 0; i <= listExpandablePanel.Count - 1; i++)
                    {
                        if (listExpandablePanel[i].TitleText == f.Category)
                        {
                            indexTab = i;
                            break;
                        }
                    }
                    if (indexTab < 0)
                    {
                        //var tabPage = new TabPage { AutoScroll = true, Text = f.Category, UseVisualStyleBackColor = true };
                        var expandablePanel = new ExpandablePanel
                                                  {
                                                      CanvasColor = SystemColors.Control,
                                                      ColorSchemeStyle =
                                                          eDotNetBarStyle.StyleManagerControlled,
                                                      Dock = DockStyle.Top,
                                                      Expanded = true,
                                                      ExpandedBounds = new Rectangle(0, 0, 560, 161),
                                                      ExpandOnTitleClick = true,
                                                      Location = new Point(2, 2),
                                                      AutoSize = true
                                                  };
                        expandablePanel.Style.Alignment = StringAlignment.Center;
                        expandablePanel.Style.BackColor1.ColorSchemePart = eColorSchemePart.PanelBackground;
                        expandablePanel.Style.BackColor1.Color = Color.WhiteSmoke;
                        expandablePanel.Style.Border = eBorderType.SingleLine;
                        expandablePanel.Style.ForeColor.ColorSchemePart = eColorSchemePart.ItemText;
                        expandablePanel.Style.GradientAngle = 90;
                        expandablePanel.TitleStyle.Alignment = StringAlignment.Center;
                        expandablePanel.TitleStyle.BackColor1.ColorSchemePart = eColorSchemePart.PanelBackground;
                        expandablePanel.TitleStyle.BackColor2.Color = Color.FromArgb(70, 70, 70);
                        expandablePanel.TitleStyle.Border = eBorderType.RaisedInner;
                        expandablePanel.TitleStyle.ForeColor.ColorSchemePart = eColorSchemePart.PanelText;
                        expandablePanel.TitleStyle.GradientAngle = 90;
                        expandablePanel.TitleText = f.Category;
                        listExpandablePanel.Add(expandablePanel);
                        expandablePanelPosY.Add(f.Category, 34);
                        indexTab = listExpandablePanel.Count - 1;
                    }
                    int posY;
                    expandablePanelPosY.TryGetValue(f.Category, out posY);

                    // If is description only (not field):
                    if (f.FieldName == string.Empty)
                    {
                        label = new Label
                                    {
                                        Text = f.Description,
                                        Location = new Point(10, posY),
                                        Size = new Size(80, 17),
                                        AutoSize = true,
                                        BackColor = Color.Transparent,
                                    };
                        listExpandablePanel[indexTab].Controls.Add(label);
                        expandablePanelPosY.Remove(f.Category);
                        expandablePanelPosY.Add(f.Category, posY + 25);
                        continue;
                    }

                    // If is Field:
                    var fieldInfo = GetType().GetField(f.FieldName);
                    if (fieldInfo != null)
                    {
                        NumericUpDown numericUpDown;
                        switch (Type.GetTypeCode(fieldInfo.FieldType))
                        {
                            case TypeCode.Boolean: // CHECKBOX
                                var switchButton = new SwitchButton
                                                       {
                                                           BackColor = Color.WhiteSmoke,
                                                           Value = Convert.ToBoolean(fieldInfo.GetValue(this)),
                                                           ForeColor = Color.Black,
                                                           Location = new Point(10, posY),
                                                           Name = f.FieldName,
                                                           OffText = "NO",
                                                           OnText = "YES",
                                                           Size = new Size(66, 22),
                                                           Style = eDotNetBarStyle.StyleManagerControlled
                                                       };
                                switchButton.BackgroundStyle.Class = "";
                                switchButton.BackgroundStyle.CornerType = eCornerType.Square;
                                label = new Label
                                            {
                                                Text = f.Description,
                                                Location = new Point(66 + 10, posY),
                                                Size = new Size(80, 17),
                                                AutoSize = true,
                                                BackColor = Color.Transparent,
                                            };
                                listExpandablePanel[indexTab].Controls.Add(label);

                                expandablePanelPosY.Remove(f.Category);
                                expandablePanelPosY.Add(f.Category, posY + 28);
                                listExpandablePanel[indexTab].Controls.Add(switchButton);
                                break;

                            case TypeCode.Int16:
                            case TypeCode.Int32:
                            case TypeCode.Int64:
                                numericUpDown = new NumericUpDown
                                                    {
                                                        Location = new Point(10, posY),
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
                                                Location = new Point(120 + 10, posY),
                                                Size = new Size(80, 17),
                                                AutoSize = true,
                                                BackColor = Color.Transparent,
                                            };
                                if (f.SettingsType == "Percentage")
                                {
                                    numericUpDown.Maximum = new decimal(100);
                                    numericUpDown.Minimum = new decimal(0);
                                    numericUpDown.Size = new Size(66, 22);
                                    label.Location = new Point(66 + 10, posY);
                                }
                                expandablePanelPosY.Remove(f.Category);
                                expandablePanelPosY.Add(f.Category, posY + 25);
                                listExpandablePanel[indexTab].Controls.Add(numericUpDown);
                                listExpandablePanel[indexTab].Controls.Add(label);

                                break;

                            case TypeCode.UInt16:
                            case TypeCode.UInt32:
                            case TypeCode.UInt64:
                                numericUpDown = new NumericUpDown
                                                    {
                                                        Location = new Point(10, posY),
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
                                                Location = new Point(120 + 10, posY),
                                                Size = new Size(80, 17),
                                                AutoSize = true,
                                                BackColor = Color.Transparent,
                                            };
                                if (f.SettingsType == "Percentage")
                                {
                                    numericUpDown.Maximum = new decimal(100);
                                    numericUpDown.Minimum = new decimal(0);
                                    numericUpDown.Size = new Size(66, 22);
                                    label.Location = new Point(66 + 10, posY);
                                }
                                expandablePanelPosY.Remove(f.Category);
                                expandablePanelPosY.Add(f.Category, posY + 25);
                                listExpandablePanel[indexTab].Controls.Add(numericUpDown);
                                listExpandablePanel[indexTab].Controls.Add(label);
                                break;

                            case TypeCode.Single:
                            case TypeCode.Double:
                                numericUpDown = new NumericUpDown
                                                    {
                                                        Location = new Point(10, posY),
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
                                                Location = new Point(120 + 10, posY),
                                                Size = new Size(80, 17),
                                                AutoSize = true,
                                                BackColor = Color.Transparent,
                                            };
                                expandablePanelPosY.Remove(f.Category);
                                expandablePanelPosY.Add(f.Category, posY + 25);
                                listExpandablePanel[indexTab].Controls.Add(numericUpDown);
                                listExpandablePanel[indexTab].Controls.Add(label);
                                break;

                            case TypeCode.String:
                                var textBox = new TextBox
                                                  {
                                                      Location = new Point(10, posY),
                                                      Name = f.FieldName,
                                                      Size = new Size(100, 20),
                                                      TabIndex = 3,
                                                      Text = Convert.ToString(fieldInfo.GetValue(this))
                                                  };
                                label = new Label
                                            {
                                                Text = f.Description,
                                                Location = new Point(100 + 10, posY),
                                                Size = new Size(80, 17),
                                                AutoSize = true,
                                                BackColor = Color.Transparent,
                                            };

                                expandablePanelPosY.Remove(f.Category);
                                expandablePanelPosY.Add(f.Category, posY + 25);
                                listExpandablePanel[indexTab].Controls.Add(label);
                                listExpandablePanel[indexTab].Controls.Add(textBox);
                                break;
                        }
                        if (f.SettingsType.Contains("Percentage") && f.SettingsType != "Percentage")
                        {
                            var labelName = "";
                            var percentageField = GetType().GetField(f.FieldName + f.SettingsType);
                            if (percentageField != null)
                            {
                                var percentage = new NumericUpDown
                                                     {
                                                         Location = new Point(10 + 180 + 66 + 100 + 60, posY),
                                                         Maximum = new decimal(100),
                                                         Minimum = new decimal(0),
                                                         Name = f.FieldName + f.SettingsType,
                                                         Size = new Size(38, 22),
                                                         Value = Convert.ToUInt64(percentageField.GetValue(this))
                                                     };
                                switch (f.SettingsType)
                                {
                                    case "AtPercentage":
                                        labelName = "At Percentage"; // if (UsePowerWordShieldAtPercentage == 10) 
                                        break;
                                    case "BelowPercentage":
                                        labelName = "Below Percentage"; // if (UsePowerWordShieldBelowPercentage < 10) 
                                        break;
                                    case "AbovePercentage":
                                        labelName = "Above Percentage"; // if (UsePowerWordShieldAbovePercentage > 10) 
                                        break;
                                }
                                var percentageLabel = new Label
                                                          {
                                                              Text = labelName,
                                                              Location = new Point(10 + 180 + 66 + 60, posY),
                                                              Size = new Size(90, 17),
                                                              AutoSize = true,
                                                              BackColor = Color.Transparent,
                                                              ForeColor = Color.Aqua,
                                                          };
                                //label.Location = new Point(66 + 10, posY);
                                listExpandablePanel[indexTab].Controls.Add(percentageLabel);
                                listExpandablePanel[indexTab].Controls.Add(percentage);
                            }
                        }
                    }
                }
                // Add tab in tab control:
                foreach (var tabPage in listExpandablePanel)
                {
                    panel.Controls.Add(tabPage);
                }
                // Add tab control in form:
                form.Controls.Add(panel);
                // Show form:
                form.ShowDialog();
                // Get changed settings:
                ReadForm(form);
            }
            catch (Exception e)
            {
                Logging.WriteError("Settings > ToForm(): " + e);
            }
        }

        private void ReadForm(Form form)
        {
            try
            {
                foreach (var f in _listFormSetting)
                {
                    if (f.FieldName == string.Empty)
                        continue;

                    var controls = form.Controls.Find(f.FieldName, true);
                    var fieldInfo = GetType().GetField(f.FieldName);
                    if (fieldInfo != null && controls.Length > 0)
                    {
                        switch (Type.GetTypeCode(fieldInfo.FieldType))
                        {
                            case TypeCode.Boolean:
                                fieldInfo.SetValue(this, ((SwitchButton) controls[0]).Value);
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
                            var controlsP = form.Controls.Find(f.FieldName + f.SettingsType, true);
                            var fieldInfoP = GetType().GetField(f.FieldName + f.SettingsType);
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

        private readonly List<FormSetting> _listFormSetting = new List<FormSetting>();
        private Point _sizeWinform = new Point(300, 500);
        private string _windowName = "Settings";
        private bool _resizableWinform;

        public void ConfigWinForm(Point size, string windowName = "Settings", bool resizable = false)
        {
            _sizeWinform = size;
            _windowName = windowName;
            _resizableWinform = resizable;
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