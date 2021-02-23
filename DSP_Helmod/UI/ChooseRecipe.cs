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
    class ChooseRecipe : HMForm
    {
        private string value;
        private Nodes nodes;
        private Item item;
        public ChooseRecipe(UIController parent) : base(parent)
        {
            this.name = "Choose Recipe";
            this.Caption = "Choose Recipe";
        }
        public override void OnDoWindow()
        {
            DrawContent();
        }

        public override void OnInit()
        {
            this.windowRect0 = new Rect(300, 200, 900, 400);
        }

        public override void OnUpdate()
        {

        }

        private void DrawContent()
        {
            GUILayout.BeginHorizontal(GUILayout.MaxHeight(25));

            GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMStyle.ColumnRecipeLayoutOptions);
            GUILayout.Label("Recipe", HMStyle.TextAlignMiddleCenter);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMStyle.ColumnMachineLayoutOptions);
            GUILayout.Label("Machine", HMStyle.TextAlignMiddleCenter);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMStyle.ColumnProductsLayoutOptions);
            GUILayout.Label("Product", HMStyle.TextAlignMiddleCenter);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMStyle.ColumnIngredientsLayoutOptions);
            GUILayout.Label("Ingredient", HMStyle.TextAlignMiddleCenter);
            GUILayout.EndHorizontal();

            GUILayout.EndHorizontal();

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, HMStyle.ScrollDataLayoutOptions);
            foreach (Recipe recipe in item.Recipes)
            {
                DrawRecipe(recipe);
            }
            GUILayout.EndScrollView();

        }

        public void DrawRecipe(Recipe recipe)
        {
            GUILayout.BeginHorizontal(GUILayout.MaxHeight(70));

            // recipe
            GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMStyle.ColumnRecipeLayoutOptions);
            HMCell.Node(recipe, delegate(Node element) {
                HMEvent.SendEvent(this, new HMEvent(HMEventType.AddRecipe, recipe.Proto));
            });
            GUILayout.EndHorizontal();
            // Machine
            GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMStyle.ColumnMachineLayoutOptions);
            HMCell.Item(recipe.Factory);
            GUILayout.EndHorizontal();
            // Products
            GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMStyle.ColumnProductsLayoutOptions);
            foreach (Item item in recipe.Products)
            {
                HMCell.ItemProduct(item, item.Count);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMStyle.ColumnIngredientsLayoutOptions);
            foreach (Item item in recipe.Ingredients)
            {
                HMCell.ItemIngredient(item, item.Count);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.EndHorizontal();
        }

        public void OnEvent(object sender, HMEvent e)
        {
            switch (e.Type)
            {
                case HMEventType.ChooseRecipe:
                    SwitchShow();
                    nodes = (Nodes)sender;
                    item = e.GetItem<Item>();
                    break;
            }
        }

        public override void OnClose()
        {

        }
    }
}
