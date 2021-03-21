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
using UnityEngine.UI;

namespace DSP_Helmod.UI.Editions
{
    public class EditionProduct : HMForm
    {
        private string value;
        private Nodes nodes;
        private Item item;
        private bool focus = false;
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
            this.windowRect0 = new Rect(200, 20, 300, 100);
        }

        public override void OnUpdate()
        {
            if (Event.current.functionKey && Event.current.keyCode == KeyCode.KeypadEnter)
            {
                Submit();
            }
            //GameObject inputFieldGo = GameObject.Find("MyTextField");
            //InputField inputFieldCo = inputFieldGo.GetComponent<InputField>();
            //Debug.Log(inputFieldCo.text);
        }

        private void DrawContent()
        {
            GUILayout.BeginHorizontal(HMStyle.BoxStyle);
            GUI.SetNextControlName("MyTextField");
            value = GUILayout.TextField(value);
            GUILayout.EndHorizontal();
            if (GUILayout.Button("OK"))
            {
                Submit();
            }
            if (!focus)
            {
                GUI.FocusControl("MyTextField");
                focus = true;
            }
        }

        private void Submit()
        {
            double result;
            double.TryParse(value, out result);
            nodes.SetInput(item, result);
            HMEvent.SendEvent(this, new HMEvent(HMEventType.UpdateSheet, nodes));
            Close();
        }

        public void OnEvent(object sender, HMEvent e)
        {
            switch (e.Type)
            {
                case HMEventType.EditionProduct:
                    SwitchShow();
                    nodes = (Nodes)sender;
                    item = e.GetItem<Item>();
                    value = nodes.GetInputValue(item).ToString();
                    focus = false;
                    break;
            }
        }

        public override void OnClose()
        {
            
        }
    }
}
