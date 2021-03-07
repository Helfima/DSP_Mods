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

namespace DSP_Helmod.UI
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
            //GUILayout.EndHorizontal();
            if (Event.current.type == EventType.Repaint)
            {
                if (lastTooltip != "")
                {
                    GUI.Label(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y + 20, 200, 200), GUI.tooltip);
                }

                lastTooltip = GUI.tooltip;
            }

        }

        private void DrawElements(List<VeinProto> items)
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUI.skin.box);
            GUIContent[] contents = new GUIContent[items.Count];
            Texture2D[] images = new Texture2D[items.Count];
            int index = 0;
            foreach (VeinProto item in items)
            {
                Texture2D texture = item.iconSprite.texture;
                string tooltip = item.name;
                images[index] = texture;
                GUIContent content = new GUIContent(texture, VeinProtoHelper.GetTootip(item));
                contents[index] = content;
                index++;
            }
            //GUILayout.BeginHorizontal(boxStyle, GUILayout.Width(80));
            GUILayoutOption[] GridLayoutOptions = new GUILayoutOption[] { GUILayout.MaxWidth(450), GUILayout.MaxHeight(100) };
            selection = GUILayout.SelectionGrid(-1, contents, 10, GridLayoutOptions);
            if (selection != -1)
            {
                VeinProto item = items[selection];
                if (selectorMode == SelectorMode.Normal)
                {
                    HMEvent.SendEvent(this, new HMEvent(HMEventType.AddItem, item));
                }
                else if(selectorMode == SelectorMode.Properties)
                {
                    HMEvent.SendEvent(this, new HMEvent(HMEventType.AddProperties, item));
                }
                selection = -1;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndScrollView();
        }

        public override void OnClose()
        {

        }

    }
}
