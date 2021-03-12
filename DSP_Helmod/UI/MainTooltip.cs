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
    public class MainTooltip : HMTooltip
    {
        public MainTooltip(UIController parent) : base(parent)
        {
        }
        public override void OnInit()
        {
            
        }
        
        public override void OnDoWindow(string tooltip)
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            if (tooltip.StartsWith("Action:"))
            {
                string label = tooltip.Substring(tooltip.IndexOf(':') + 1);
                GUILayout.Label(label, HMStyle.TextTooltip);
            }
            else
            {
                GUILayout.Label(tooltip, HMStyle.TextTooltip);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }

    }
}
