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

namespace DSP_Helmod.UI.Editions
{
    public class EditionRecipe : HMForm
    {
        private Node node;
        public EditionRecipe(UIController parent) : base(parent)
        {
            this.name = "Edition Recipe";
            this.Caption = "Edition Recipe";
        }
        public override void OnDoWindow()
        {
            DrawDetail();
            DrawMachines();
        }

        public override void OnInit()
        {
            this.windowRect0 = new Rect(300, 200, 900, 400);
        }

        public override void OnUpdate()
        {
            

        }

        private void DrawMachines()
        {

            GUILayout.BeginHorizontal(HMStyle.BoxStyle, GUILayout.MaxHeight(25));
            GUILayout.Label("Machines", HMStyle.TextAlignMiddleCenter);
            HMButton.Text("Apply on all", "Apply selection of machine on all recipes", 100, 25, delegate () { ModelBuilder.ApplyFactoryOnAll(node as IRecipe); });
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (node is IRecipe)
            {
                IRecipe recipe = (IRecipe)node;
                foreach(Factory factory in recipe.Factories)
                {
                    if (recipe.Factory.Name.Equals(factory.Name))
                    {
                        GUI.color = Color.yellow;
                    }
                    HMCell.Product(factory, factory.Speed, delegate(IItem element){
                        UpdateFactory(recipe, factory);
                    });
                    GUI.color = Color.white;
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private void UpdateFactory(IRecipe recipe, Factory factory)
        {
            recipe.Factory = (Factory)factory.Clone();
            HMEventQueue.EnQueue(this, new HMEvent(HMEventType.UpdateSheet, recipe));
        }
        private void DrawDetail()
        {
            GUILayout.BeginHorizontal(GUILayout.MaxHeight(25));
            {
                GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMLayoutOptions.ColumnProduction);
                GUILayout.Label("%", HMStyle.TextAlignMiddleCenter);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMLayoutOptions.ColumnRecipe);
                GUILayout.Label("Recipe", HMStyle.TextAlignMiddleCenter);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMLayoutOptions.ColumnPower);
                GUILayout.Label("Energy", HMStyle.TextAlignMiddleCenter);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMLayoutOptions.ColumnMachine);
                GUILayout.Label("Machine", HMStyle.TextAlignMiddleCenter);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMLayoutOptions.ColumnProducts);
                GUILayout.Label("Product", HMStyle.TextAlignMiddleCenter);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMLayoutOptions.ColumnIngredients);
                GUILayout.Label("Ingredient", HMStyle.TextAlignMiddleCenter);
                GUILayout.EndHorizontal();

                GUILayout.EndHorizontal();
            }
            {
                if (node is IRecipe)
                {
                    IRecipe recipe = ((IRecipe)node).Clone(1);
                    recipe.Factory.Count = recipe.Energy * recipe.Count / (recipe.Factory.Speed);
                    //Debug.Log($"Recipe count:{recipe.Count}");
                    GUILayout.BeginHorizontal(GUILayout.MaxHeight(70));

                    GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMLayoutOptions.GetDataCell(DataColumn.Production, false));
                    GUILayout.TextField("100");
                    GUILayout.EndHorizontal();
                    // recipe
                    GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMLayoutOptions.GetDataCell(DataColumn.Recipe, false));
                    HMCell.Node(recipe);
                    GUILayout.EndHorizontal();
                    // energy
                    GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMLayoutOptions.GetDataCell(DataColumn.Recipe, false));
                    HMCell.RecipeTime(recipe);
                    GUILayout.EndHorizontal();
                    //machine
                    GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMLayoutOptions.GetDataCell(DataColumn.Machine, false));
                    HMCell.Product(recipe.Factory, 1);
                    GUILayout.EndHorizontal();
                    // Products
                    GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMLayoutOptions.GetDataCell(DataColumn.Products, false));
                    foreach (IItem item in recipe.Products)
                    {
                        HMCell.ItemProduct(item, recipe.Count * node.Effects.Productivity);
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMLayoutOptions.GetDataCell(DataColumn.Ingredients, false));
                    foreach (IItem item in recipe.Ingredients)
                    {
                        HMCell.ItemIngredient(item, recipe.Count);
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndHorizontal();
            
        }

        public void OnEvent(object sender, HMEvent e)
        {
            switch (e.Type)
            {
                case HMEventType.EditionRecipe:
                    SwitchShow();
                    node = e.GetItem<Node>();
                    break;
            }
        }

        public override void OnClose()
        {

        }
    }
}
