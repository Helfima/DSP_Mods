﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSP_Helmod.UI.Gui;
using DSP_Helmod.Classes;
using DSP_Helmod.Helpers;
using DSP_Helmod.UI.Core;
using UnityEngine;

namespace DSP_Helmod.UI
{
    public class SelectorRecipe : HMForm
    {
        protected ERecipeType groupSelected = 0;
        protected string recipeSelected;
        protected int selection;

        public SelectorRecipe(UIController parent) : base(parent) {
            this.name = "Recipe Selector";
            this.Caption = "Add Recipe";
            this.IsTool = true;
            this.windowRect0 = new Rect(400, 200, 600, 400);
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

        private Dictionary<ERecipeType, List<RecipeProto>> GetRecipes()
        {
            Dictionary<ERecipeType, List<RecipeProto>> recipes = new Dictionary<ERecipeType, List<RecipeProto>>();
            foreach (RecipeProto recipeProto in LDB.recipes.dataArray)
            {
                ERecipeType key = recipeProto.Type;
                if (!recipes.ContainsKey(key)) recipes.Add(key, new List<RecipeProto>());
                recipes[key].Add(recipeProto);
            }
            return recipes;
        }

        private void DrawContent()
        {
            Dictionary<ERecipeType, List<RecipeProto>> recipeList = GetRecipes();
            GUILayout.BeginHorizontal(HMStyle.BoxStyle, GUILayout.MaxHeight(20), GUILayout.Width(80));
            foreach (ERecipeType entry in recipeList.Keys)
            {
                if (GUILayout.Button(entry.ToString()))
                {
                    groupSelected = entry;
                }
                if (groupSelected == 0) groupSelected = entry;
            }
            GUILayout.EndHorizontal();

            List<RecipeProto> recipes = recipeList[groupSelected];
            DrawElements(recipes);
            //GUILayout.EndHorizontal();
            

        }

        private void DrawElements(List<RecipeProto> recipes)
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUI.skin.box);
            GUIContent[] contents = new GUIContent[recipes.Count];
            Texture2D[] images = new Texture2D[recipes.Count];
            int index = 0;
            foreach (RecipeProto recipe in recipes)
            {
                Texture2D texture = recipe.iconSprite.texture;
                string tooltip = recipe.name;
                images[index] = texture;
                GUIContent content = new GUIContent(texture, RecipeProtoHelper.GetTootip(recipe));
                contents[index] = content;
                index++;
            }
            //GUILayout.BeginHorizontal(boxStyle, GUILayout.Width(80));
            GUILayoutOption[] GridLayoutOptions = new GUILayoutOption[] { GUILayout.MaxWidth(450) };
            selection = GUILayout.SelectionGrid(-1, contents, 10, GridLayoutOptions);
            if (selection != -1)
            {
                RecipeProto recipe = recipes[selection];
                //HMLogger.Debug($"Recipe:{recipe.name}");
                if (selectorMode == SelectorMode.Normal)
                {
                    HMEvent.SendEvent(this, new HMEvent(HMEventType.AddRecipe, recipe));
                }
                else if (selectorMode == SelectorMode.Properties)
                {
                    HMEvent.SendEvent(this, new HMEvent(HMEventType.AddProperties, recipe));
                }
                selection = -1;
            }
            GUILayout.EndScrollView();
        }

        public override void OnClose()
        {

        }

    }
}
