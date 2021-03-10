using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSP_Helmod.Model;
using DSP_Helmod.UI.Gui;
using DSP_Helmod.Classes;
using DSP_Helmod.Helpers;
using DSP_Helmod.UI.Core;
using UnityEngine;
using DSP_Helmod.Math;
using UnityEngine.UI;
using DSP_Helmod.Converter;

namespace DSP_Helmod.UI
{
    public class TopMenu : HMForm
    {
        private int length = 0;
        public TopMenu(UIController parent) : base(parent) {
#if DEBUG
            this.length = 1;
#endif
            this.Show = true;
            this.windowRect0 = new Rect(20, 20, 120, 50 + 25 * length);
            this.WindowButtons = false;
            this.BackgroundColor = Color.black;
        }
        public override void OnInit()
        {
            this.IsPersistant = true;
        }


        public override void OnUpdate()
        {
            
        }

        public override void OnDoWindow()
        {
            HMButton.Action("Open/Close", delegate () {
                HMEvent.SendEvent(this, new HMEvent(HMEventType.OpenClose, typeof(MainPanel)));
            });
            if (length > 0)
            {
                HMButton.Action("Properties", delegate ()
                {
                    HMEvent.SendEvent(this, new HMEvent(HMEventType.OpenClose, typeof(PropertiesPanel)));
                });
            }
        }


        public void OnEvent(object sender, HMEvent e)
        {
            
        }

        
        public override void OnClose()
        {
            
        }
    }
}
