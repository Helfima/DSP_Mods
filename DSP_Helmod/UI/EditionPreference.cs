using DSP_Helmod.Classes;
using DSP_Helmod.Model;
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
    public class EditionPreference : HMForm
    {
        protected SectionPreference selection = SectionPreference.Controls;
        protected Dictionary<string, string> controls = new Dictionary<string, string>();
        public EditionPreference(UIController parent) : base(parent)
        {
            this.name = "Edition Preferences";
            this.Caption = "Edition Preferences";
        }
        public override void OnDoWindow()
        {
            DrawMenu();
            DrawContent();
        }

        public override void OnInit()
        {
            this.windowRect0 = new Rect(200, 20, 600, 700);
            controls.Add("Open/Close", Settings.Instance.OpenCloseKeyCode.ToString());
        }

        public override void OnUpdate()
        {
            
        }

        private void DrawMenu()
        {
            GUILayout.BeginHorizontal(HMStyle.BoxStyle, GUILayout.MaxHeight(20), GUILayout.Width(80));
            foreach (SectionPreference entry in Enum.GetValues(typeof(SectionPreference)))
            {
                if (GUILayout.Button(entry.ToString()))
                {
                    selection = entry;
                }
            }
            GUILayout.EndHorizontal();
        }
        private void DrawContent()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUI.skin.box);
            GUILayout.BeginVertical();
            switch (selection)
            {
                case SectionPreference.Controls:
                    DrawControls();
                    break;
            }
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }

        private void DrawControls()
        {
            foreach (KeyValuePair<string, string> entry in controls)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label($"{entry.Key:30}:{entry.Value}");
                GUILayout.EndHorizontal();
            }
        }

        public void OnEvent(object sender, HMEvent e)
        {
            
        }

        public override void OnClose()
        {
            
        }
    }

    public enum SectionPreference
    {
        Controls
    }
}
