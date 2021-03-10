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
        private Nodes nodes;
        private IItem item;
        public ChooseRecipe(UIController parent) : base(parent)
        {
            this.name = "Choose Recipe";
            this.Caption = "Choose Recipe";
        }
        public override void OnDoWindow()
        {
            DrawHeader();
            DrawContent();
        }

        public override void OnInit()
        {
            this.windowRect0 = new Rect(300, 200, 900, 400);
        }

        public override void OnUpdate()
        {

        }

        private void DrawHeader()
        {
            //HMLogger.Debug("DrawHeader");

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

        }
        private void DrawContent()
        {
            //HMLogger.Debug("DrawContent");

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, HMStyle.ScrollChooseLayoutOptions);
            if (item != null)
            {
                List<IRecipe> recipes = Database.SelectRecipeByProduct(item);
                //HMLogger.Debug($"Recipes:{recipes.Count}");
                foreach (IRecipe recipe in recipes)
                {
                    //HMLogger.Debug($"Recipe:{recipe.Name}");
                    DrawRecipe(recipe);
                }
            }
            GUILayout.EndScrollView();

        }

        public void DrawRecipe(IRecipe recipe)
        {
            //HMLogger.Debug("DrawRecipe");
            GUILayout.BeginHorizontal(GUILayout.MaxHeight(70));

            // recipe
            GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMStyle.ColumnRecipeLayoutOptions);
            HMCell.Node(recipe, null, delegate(INode element) {
                HMEvent.SendEvent(this, new HMEvent(HMEventType.ChooseRecipe, recipe));
            });
            GUILayout.EndHorizontal();
            // Machine
            GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMStyle.ColumnMachineLayoutOptions);
            HMCell.Product(recipe.Factory);
            GUILayout.EndHorizontal();
            // Products
            GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMStyle.ColumnProductsLayoutOptions);
            foreach (IItem item in recipe.Products)
            {
                HMCell.ItemProduct(item, item.Count);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMStyle.ColumnIngredientsLayoutOptions);
            foreach (IItem item in recipe.Ingredients)
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
                case HMEventType.SwitchChooseRecipe:
                    nodes = (Nodes)sender;
                    item = e.GetItem<IItem>();
                    SwitchShow();
                    break;
            }
        }

        public override void OnClose()
        {

        }
    }
}
