using DSP_Helmod.UI.Core;
using DSP_Helmod.UI.Gui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DSP_Helmod.UI
{
    public class MainTooltip : HMForm
    {
        public MainTooltip(UIController parent) : base(parent)
        {
            this.name = "";
            this.WindowButtons = false;
        }
        public override void OnInit()
        {
            
        }
        public void OnDoWindow(string tooltip)
        {
            if (tooltip.StartsWith("Action:"))
            {
                string label = tooltip.Substring(tooltip.IndexOf(':') + 1);
                GUILayout.Label(label, HMStyle.TextTooltip);
            }
            else
            {
                GUILayout.Label(tooltip, HMStyle.TextTooltip);
            }
        }

        public override void OnUpdate()
        {
            string tooltip = parent.Tooltip;
            if (tooltip != null && tooltip != "" && tooltip != " ")
            {
                Show = true;
            }
            else Show = false;
        }

        public override void OnDoWindow()
        {
            Position = new Rect(Event.current.mousePosition.x + 240, Event.current.mousePosition.y + 20, 200, 200);

            //GUI.backgroundColor = Color.red;
            //GUILayout.BeginArea(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y + 50, 200, 200));
            OnDoWindow(parent.Tooltip);
            //GUILayout.EndArea();
        }

        public override void OnClose()
        {
            
        }
    }
}
