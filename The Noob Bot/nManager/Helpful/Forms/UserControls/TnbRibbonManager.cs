using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace nManager.Helpful.Forms.UserControls
{
    internal class TnbRibbonManager : Panel
    {
        private List<TnbExpendablePanel> _panelAttached  = new List<TnbExpendablePanel>();

        public TnbRibbonManager()
        {
            _panelAttached = new List<TnbExpendablePanel>();
            Size = new Size(573, 370);
            Location = new Point(573, 370);
            BorderStyle = BorderStyle.None;
            base.AutoScroll = false;
            ControlAdded += OnControlAdd;
            ControlRemoved += OnControlDel;
            Click += Relocator;
            Relocator();
        }

        private void OnControlAdd(object sender, ControlEventArgs controlEventArgs)
        {
            Relocator();
            foreach (Control x in Controls)
            {
                if (x is TnbExpendablePanel)
                {
                    if (!_panelAttached.Contains(x as TnbExpendablePanel))
                    {
                        TnbExpendablePanel y = x as TnbExpendablePanel;
                        if (_panelAttached.Count > 1)
                        {
                            if (y.OrderIndex == -1)
                                y.OrderIndex = _panelAttached[_panelAttached.Count - 1].OrderIndex + 1;
                            y.Location = Relocator();
                        }
                        y.OnStatusChanged += Relocator;
                        y.OnOrderChanged += Relocator;
                        
                        _panelAttached.Add(y);
                    }
                }
            }
            Relocator();
        }

        private void OnControlDel(object sender, ControlEventArgs controlEventArgs)
        {
            List<int> tmpList = new List<int>();

            for (int i = 0; i < _panelAttached.Count; i++)
            {
                TnbExpendablePanel x = _panelAttached[i];
                if (!Controls.Contains(x))
                    tmpList.Add(i);
            }
            foreach (int i in tmpList)
            {
                _panelAttached.RemoveAt(i);
            }
            Relocator();
        }

        [Category("Appearance")]
        public void Relocator(object sender, EventArgs eventArgs)
        {
            Relocator();
        }

        private Point Relocator()
        {
            _panelAttached = _panelAttached.OrderBy(o => o.OrderIndex).ToList();
            int oldHeight = 0;
            int oldY = 0;
            for (int i = 0; i < _panelAttached.Count; i++)
            {
                if (i != 0)
                {
                    var tmpPoint = new Point(0, oldY + oldHeight);
                    _panelAttached[i].Location = tmpPoint;
                    oldHeight += _panelAttached[i].Height - oldHeight;
                    oldY = _panelAttached[i].Location.Y;
                }
                else
                {
                    _panelAttached[i].Location = _panelAttached[i].Location.Y <= 0 ? new Point(0, _panelAttached[i].Location.Y) : new Point(0, 0);
                    oldHeight = _panelAttached[i].Height;
                    oldY = _panelAttached[i].Location.Y;
                }
            }
            Invalidate();
            return new Point(0, oldY + oldHeight);
        }
    }
}