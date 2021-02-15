using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPHelmod.Classes;
using DSPHelmod.Helpers;
using DSPHelmod.UI.Core;
using UnityEngine;

namespace DSPHelmod.UI
{
    public class MainPanel : HMForm
    {
        protected ERecipeType groupSelected = 0;
        protected string recipeSelected;
        protected int selection;
        protected int toolbarInt = -1;
        protected List<HMForm> toolbarForms;
        protected List<RecipeProto> recipes = new List<RecipeProto>();
        public MainPanel(UIController parent) : base(parent) {
            this.name = "Helmod v0.1";
            this.Show = true;
            this.windowRect0 = new Rect(20, 20, 900, 500);
        }
        public override void OnInit()
        {
            
        }
        

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                Show = !Show;
            }
            
        }

        public override void OnDoWindow()
        {
            DrawContent();
        }

        private void DrawContent()
        {
            DrawMenu();
            DrawTable();
        }

        private void DrawMenu()
        {
            if (parent.Forms != null)
            {
                toolbarForms = new List<HMForm>();
                List<string> toolbarString = new List<string>();
                foreach (HMForm form in parent.Forms)
                {
                    if (form.IsTool)
                    {
                        toolbarForms.Add(form);
                        toolbarString.Add(form.Name);
                    }
                }

                GUILayout.BeginHorizontal(boxStyle, GUILayout.MaxHeight(20), GUILayout.Width(80));
                toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarString.ToArray());
                GUILayout.EndHorizontal();

                if (toolbarForms != null && toolbarInt > -1)
                {
                    toolbarForms[toolbarInt].Show = !toolbarForms[toolbarInt].Show;
                    toolbarInt = -1;
                }
            }
        }

        private void DrawTableHeader()
        {
            GUIStyle textAlignStyle = new GUIStyle(GUI.skin.label);
            textAlignStyle.alignment = TextAnchor.MiddleCenter;

            GUILayout.BeginHorizontal(GUILayout.MaxHeight(50));

            GUILayout.BeginHorizontal(boxStyle, GUILayout.Width(80));
            GUILayout.Label("Recipe", textAlignStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(boxStyle, GUILayout.Width(80));
            GUILayout.Label("Power", textAlignStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(boxStyle, GUILayout.Width(80));
            GUILayout.Label("Machine", textAlignStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(boxStyle, GUILayout.Width(200));
            GUILayout.Label("Product", textAlignStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(boxStyle, GUILayout.Width(200));
            GUILayout.Label("Ingredient", textAlignStyle);
            GUILayout.EndHorizontal();

            GUILayout.EndHorizontal();
        }
        private void DrawTable()
        {
            if (recipes != null && recipes.Count > 0)
            {
                DrawTableHeader();
                GUIStyle textAlignStyle = new GUIStyle(GUI.skin.label);
                textAlignStyle.alignment = TextAnchor.LowerRight;

                scrollPosition = GUILayout.BeginScrollView(scrollPosition);
                foreach (RecipeProto recipe in recipes)
                {
                    GUILayout.BeginHorizontal(GUILayout.MaxHeight(50));

                    GUILayout.BeginHorizontal(boxStyle, GUILayout.Width(80));
                    GUILayout.TextField("100");
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(boxStyle, GUILayout.Width(80));
                    GUILayout.Box(recipe.iconSprite.texture, IconLayoutOptions);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(boxStyle, GUILayout.Width(80));
                    GUILayout.Label("");
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(boxStyle, GUILayout.Width(80));
                    GUILayout.Label("");
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(boxStyle, GUILayout.Width(200));
                    for (int index = 0; index < recipe.ResultCounts.Length; index++)
                    {
                        int count = recipe.ResultCounts[index];
                        int id = recipe.Results[index];
                        ItemProto item = LDB.items.Select(id);
                        GUILayout.BeginHorizontal();
                        GUILayout.Label($"{count}x", textAlignStyle);
                        GUILayout.Box(item.iconSprite.texture, IconLayoutOptions);
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(boxStyle, GUILayout.Width(200));
                    for (int index = 0; index < recipe.ItemCounts.Length; index++)
                    {
                        int count = recipe.ItemCounts[index];
                        int id = recipe.Items[index];
                        ItemProto item = LDB.items.Select(id);
                        GUILayout.BeginHorizontal();
                        GUILayout.Label($"{count}x", textAlignStyle);
                        GUILayout.Box(item.iconSprite.texture, IconLayoutOptions);
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.EndHorizontal();
                }
                GUILayout.EndScrollView();
            }
        }

        public override void OnEvent(object sender, HMEvent e)
        {
            switch (e.Type)
            {
                case HMEventType.AddRecipe:
                    recipes.Add(e.GetItem<RecipeProto>());
                    break;
            }
        }
    }
}
