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
    public class EditionProduct : HMForm
    {
        private string value;
        private Nodes nodes;
        private Item item;
        public EditionProduct(UIController parent) : base(parent)
        {
            this.name = "Edition Product";
            this.Caption = "Edition Product";
        }
        public override void OnDoWindow()
        {
            DrawContent();
        }

        public override void OnInit()
        {
            this.windowRect0 = new Rect(200, 20, 200, 100);
        }

        public override void OnUpdate()
        {
            
        }

        private void DrawContent()
        {
            GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMStyle.ColumnProductionLayoutOptions);
            value = GUILayout.TextField(value);
            GUILayout.EndHorizontal();
            if (GUILayout.Button("OK"))
            {
                double result;
                double.TryParse(value, out result);
                nodes.SetInput(item, result);
                HMEvent.SendEvent(this, new HMEvent(HMEventType.UpdateSheet, nodes));
            }
        }

        public void OnEvent(object sender, HMEvent e)
        {
            switch (e.Type)
            {
                case HMEventType.EditionProduct:
                    Show = !Show;
                    nodes = (Nodes)sender;
                    item = e.GetItem<Item>();
                    value = item.Count.ToString();
                    break;
            }
        }

        public override void OnClose()
        {
            
        }
    }
}
