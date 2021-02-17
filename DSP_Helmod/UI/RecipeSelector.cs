using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSP_Helmod.UI.Gui;
using DSPHelmod.Classes;
using DSPHelmod.Helpers;
using DSPHelmod.UI.Core;
using UnityEngine;

namespace DSPHelmod.UI
{
    public class RecipeSelector : HMForm
    {
        protected ERecipeType groupSelected = 0;
        protected string recipeSelected;
        protected int selection;

        public RecipeSelector(UIController parent) : base(parent) {
            this.name = "Recipe Selector";
            this.Caption = "Add Recipe";
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
            if (Event.current.type == EventType.Repaint)
            {
                if (lastTooltip != "")
                {
                    GUI.Label(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y + 20, 200, 200), GUI.tooltip);
                }

                lastTooltip = GUI.tooltip;
            }

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
                Debug.Log($"Recipe:{recipe.name}");
                HMEvent.SendEvent(this, new HMEvent(HMEventType.AddRecipe, recipe));
                selection = -1;
            }
            GUILayout.EndScrollView();
        }

    }
}
