using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSP_Helmod.UI.Gui;
using DSP_Helmod.Classes;
using DSP_Helmod.Helpers;
using DSP_Helmod.UI.Core;
using UnityEngine;
using DSP_Helmod.Model;

namespace DSP_Helmod.UI
{
    public class SelectorRecipe : HMForm
    {
        protected string groupSelected = "";
        protected string recipeSelected;
        protected int selection;

        public SelectorRecipe(UIController parent) : base(parent) {
            this.name = "Recipe Selector";
            this.Caption = "Add Recipe";
            this.InMain = true;
            this.IsTool = true;
            this.windowRect0 = new Rect(400, 200, 700, 400);
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

        private void DrawContent()
        {
            Dictionary<string, List<IRecipe>> recipeList = Database.RecipesByGroup;
            GUILayout.BeginHorizontal(HMStyle.BoxStyle, GUILayout.MaxHeight(20), GUILayout.Width(80));
            foreach (string entry in recipeList.Keys)
            {
                if (GUILayout.Button(entry.ToString()))
                {
                    groupSelected = entry;
                }
                if (groupSelected == "") groupSelected = entry;
            }
            GUILayout.EndHorizontal();

            List<IRecipe> recipes = recipeList[groupSelected];
            DrawElements(recipes);
            //GUILayout.EndHorizontal();
            

        }

        private void DrawElements(List<IRecipe> recipes)
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUI.skin.box);

            GUILayout.BeginHorizontal(HMStyle.Icon50LayoutOptions);
            int index = 0;
            foreach (IRecipe recipe in recipes)
            {
                if (index != 0 && index % 10 == 0)
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                }
                HMButton.Node(recipe, RecipeProtoHelper.GetTootip(recipe), delegate(INode element) {
                    if (selectorMode == SelectorMode.Normal)
                    {
                        HMEvent.SendEvent(this, new HMEvent(HMEventType.AddRecipe, recipe));
                    }
                    else if (selectorMode == SelectorMode.Properties)
                    {
                        object proto = null;
                        if (recipe is Recipe) proto = ((Recipe)recipe).Proto;
                        if (recipe is RecipeVein) proto = ((RecipeVein)recipe).Proto;
                        if (recipe is RecipeOrbit) proto = ((RecipeOrbit)recipe).Proto;
                        if (recipe is RecipeOcean) proto = ((RecipeOcean)recipe).Proto;
                        if (recipe is RecipeCustom) proto = ((RecipeCustom)recipe).Proto;
                        if(proto != null)
                            HMEvent.SendEvent(this, new HMEvent(HMEventType.AddProperties, proto));
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
