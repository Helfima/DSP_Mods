using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPHelmod.Classes;
using DSPHelmod.UI.Core;
using UnityEngine;

namespace DSPHelmod.UI
{
    public class ItemSelector : HMForm
    {
        protected EItemType groupSelected = 0;
        protected string recipeSelected;
        protected int selection;

        public ItemSelector(UIController parent) : base(parent) {
            this.name = "Item Selector";
            this.Caption = "Add Item";
            this.IsTool = true;
        }
        public override void OnInit()
        {
            
        }

        public override void OnUpdate()
        {
            
        }

        public override void OnDoWindow()
        {
            DrawContent();
        }

        private Dictionary<EItemType, List<ItemProto>> GetItems()
        {
            Dictionary<EItemType, List<ItemProto>> items = new Dictionary<EItemType, List<ItemProto>>();
            foreach (ItemProto itemProto in LDB.items.dataArray)
            {
                EItemType key = itemProto.Type;
                if (!items.ContainsKey(key)) items.Add(key, new List<ItemProto>());
                items[key].Add(itemProto);
            }
            return items;
        }

        private void DrawContent()
        {
            Dictionary<EItemType, List<ItemProto>> itemList = GetItems();
            GUILayout.BeginHorizontal(boxStyle, GUILayout.MaxHeight(20), GUILayout.Width(80));
            foreach (EItemType entry in itemList.Keys)
            {
                if (GUILayout.Button(entry.ToString()))
                {
                    groupSelected = entry;
                }
                if (groupSelected == 0) groupSelected = entry;
            }
            GUILayout.EndHorizontal();

            List<ItemProto> items = itemList[groupSelected];
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

        private void DrawElements(List<ItemProto> items)
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUI.skin.box);
            GUIContent[] contents = new GUIContent[items.Count];
            Texture2D[] images = new Texture2D[items.Count];
            int index = 0;
            foreach (ItemProto item in items)
            {
                Texture2D texture = item.iconSprite.texture;
                string tooltip = item.name;
                images[index] = texture;
                GUIContent content = new GUIContent(texture, item.description);
                contents[index] = content;
                index++;
            }
            //GUILayout.BeginHorizontal(boxStyle, GUILayout.Width(80));
            GUILayoutOption[] GridLayoutOptions = new GUILayoutOption[] { GUILayout.MaxWidth(450) };
            selection = GUILayout.SelectionGrid(-1, contents, 10, GridLayoutOptions);
            if (selection != -1)
            {
                ItemProto item = items[selection];
                OnEventReached(new HMEvent(HMEventType.AddItem, item));
                selection = -1;
            }
            GUILayout.EndScrollView();
        }

        public override void OnEvent(object sender, HMEvent e)
        {
            
        }
    }
}
