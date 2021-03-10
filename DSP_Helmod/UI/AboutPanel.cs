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
    public class AboutPanel : HMForm
    {
        protected SectionAbout selection = SectionAbout.About;
        protected Dictionary<string, string> abouts = new Dictionary<string, string>();
        public AboutPanel(UIController parent) : base(parent)
        {
            this.name = "About";
            this.Caption = "About";
        }
        public override void OnDoWindow()
        {
            DrawMenu();
            DrawContent();
        }

        public override void OnInit()
        {
            this.windowRect0 = new Rect(200, 20, 600, 700);
            abouts.Add("Mode", PluginInfo.Instance.name);
            abouts.Add("Version", PluginInfo.Instance.version_number);
            abouts.Add("Description", PluginInfo.Instance.description);
            abouts.Add("Url", PluginInfo.Instance.website_url);
        }

        public override void OnUpdate()
        {
            
        }

        private void DrawMenu()
        {
            GUILayout.BeginHorizontal(HMStyle.BoxStyle, GUILayout.MaxHeight(20), GUILayout.Width(80));
            foreach (SectionAbout entry in Enum.GetValues(typeof(SectionAbout)))
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
                case SectionAbout.About:
                    DrawAbout();
                    break;
                case SectionAbout.ChangeLog:
                    DrawChangeLog();
                    break;
            }
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }
        private void DrawAbout()
        {
            foreach (KeyValuePair<string, string> entry in abouts)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label($"{entry.Key:30}:{entry.Value}");
                GUILayout.EndHorizontal();
            }

        }
        private void DrawChangeLog()
        {
            string text = LoadAssembly.ReadEmbeddedRessourceString("changelog");
            GUILayout.Label(text);
        }

        public void OnEvent(object sender, HMEvent e)
        {
            
        }

        public override void OnClose()
        {
            
        }
    }
    public enum SectionAbout
    {
        About,
        ChangeLog
    }
}
