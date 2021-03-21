using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSP_Helmod.UI.Gui;
using DSP_Helmod.Classes;
using DSP_Helmod.UI.Core;
using UnityEngine;
using DSP_Helmod.Helpers;
using DSP_Helmod.Model;

namespace DSP_Helmod.UI.Selectors
{
    public class SelectorVein : HMForm
    {
        protected EMinerType groupSelected = 0;
        protected string recipeSelected;
        protected int selection;

        public SelectorVein(UIController parent) : base(parent) {
            this.name = "Vein Selector";
            this.Caption = "Add Vein";
            this.IsTool = true;
        }
        public override void OnInit()
        {
            this.windowRect0 = new Rect(200, 20, 600, 600);
        }

        public override void OnUpdate()
        {
            
        }

        public override void OnDoWindow()
        {
            DrawContent();
        }

        private Dictionary<EMinerType, List<VeinProto>> GetItems()
        {
            Dictionary<EMinerType, List<VeinProto>> items = new Dictionary<EMinerType, List<VeinProto>>();
            foreach (VeinProto veinProto in LDB.veins.dataArray)
            {
                EMinerType key = veinProto.prefabDesc.minerType;
                if (!items.ContainsKey(key)) items.Add(key, new List<VeinProto>());
                items[key].Add(veinProto);
            }
            return items;
        }

        private void DrawContent()
        {
            Dictionary<EMinerType, List<VeinProto>> itemList = GetItems();
            GUILayout.BeginHorizontal(HMStyle.BoxStyle, GUILayout.MaxHeight(20), GUILayout.Width(80));
            foreach (EMinerType entry in itemList.Keys)
            {
                if (GUILayout.Button(entry.ToString()))
                {
                    groupSelected = entry;
                }
                if (groupSelected == 0) groupSelected = entry;
            }
            GUILayout.EndHorizontal();

            List<VeinProto> items = itemList[groupSelected];
            DrawElements(items);
            
        }

        private void DrawElements(List<VeinProto> items)
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUI.skin.box);
            GUILayout.BeginHorizontal();
            int index = 0;
            foreach (VeinProto item in items)
            {
                if (index != 0 && index % 10 == 0)
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                }
                HMButton.Texture(item.iconSprite.texture, delegate () {
                    if (selectorMode == SelectorMode.Properties)
                    {
                        HMEvent.SendEvent(this, new HMEvent(HMEventType.AddProperties, item));
                    }
                });
                index++;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();
        }

        public override void OnClose()
        {

        }

    }
}
