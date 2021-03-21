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

namespace DSP_Helmod.UI.Selectors
{
    public class SelectorPlanet : HMForm
    {
        protected EPlanetType groupSelected = 0;
        protected string recipeSelected;
        protected int selection;

        public SelectorPlanet(UIController parent) : base(parent) {
            this.name = "Planet Selector";
            this.Caption = "Add Planet";
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

        private Dictionary<EPlanetType, List<PlanetData>> GetItems()
        {
            Dictionary<EPlanetType, List<PlanetData>> items = new Dictionary<EPlanetType, List<PlanetData>>();
            foreach (PlanetData planetData in Model.GameData.Planets)
            {
                EPlanetType key = planetData.type;
                if (!items.ContainsKey(key)) items.Add(key, new List<PlanetData>());
                items[key].Add(planetData);
            }
            return items;
        }

        private void DrawContent()
        {
            Dictionary<EPlanetType, List<PlanetData>> itemList = GetItems();
            GUILayout.BeginHorizontal(HMStyle.BoxStyle, GUILayout.MaxHeight(20), GUILayout.Width(80));
            foreach (EPlanetType entry in itemList.Keys)
            {
                if (GUILayout.Button(entry.ToString()))
                {
                    groupSelected = entry;
                }
                if (groupSelected == 0) groupSelected = entry;
            }
            GUILayout.EndHorizontal();

            List<PlanetData> items = itemList[groupSelected];
            DrawElements(items);
            
        }

        private void DrawElements(List<PlanetData> items)
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUI.skin.box);

            GUILayout.BeginHorizontal();
            int index = 0;
            foreach (PlanetData item in items)
            {
                if (index != 0 && index % 5 == 0)
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                }
                HMButton.Text(item.name, PlanetProtoHelper.GetTootip(item), 100, 25, delegate () {
                    if (selectorMode == SelectorMode.Normal)
                    {
                        HMEvent.SendEvent(this, new HMEvent(HMEventType.AddRecipe, item));
                    }
                    else if (selectorMode == SelectorMode.Properties)
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
