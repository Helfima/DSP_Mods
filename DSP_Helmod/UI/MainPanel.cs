﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSP_Helmod.Model;
using DSP_Helmod.UI.Gui;
using DSP_Helmod.Classes;
using DSP_Helmod.Helpers;
using DSP_Helmod.UI.Core;
using UnityEngine;
using DSP_Helmod.Math;
using UnityEngine.UI;
using DSP_Helmod.Converter;

namespace DSP_Helmod.UI
{
    public class MainPanel : HMForm
    {
        protected ERecipeType groupSelected = 0;
        protected string recipeSelected;
        protected int selection;
        protected int toolbarInt = -1;
        protected List<HMForm> toolbarForms;
        protected List<RecipeProto> recipes = new List<RecipeProto>();

        protected Nodes currentSheet;
        protected Nodes currentNode;

        protected Vector2 ScrollOutputPosition;
        protected Vector2 ScrollInputPosition;
        protected Vector2 ScrollNavPosition;

        protected int timeSelected = 0;
        protected string[] timesString = new string[] {"1s", "1mn", "1h" };
        public MainPanel(UIController parent) : base(parent) {
            this.name = $"DSP Helmod V{PluginInfo.Instance.version_number}";
#if DEBUG
            this.Show = true;
#endif
            this.windowRect0 = new Rect(500, 200, 1400, 650);
            this.AlphaButtons = true;
        }
        public override void OnInit()
        {
        }


        public override void OnUpdate()
        {
            if (currentSheet != null)
            {
                if (currentSheet.TimeSelected != timeSelected)
                {
                    currentSheet.TimeSelected = timeSelected;
                    Compute();
                }
            }
        }

        public override void OnDoWindow()
        {
            DrawContent();
        }

        private void DrawContent()
        {
            DrawMenuSheet();
            DrawMenu();
            DrawData();
        }

        private void DrawMenuSheet()
        {
            GUIStyle textAlignStyle = new GUIStyle(GUI.skin.label);
            textAlignStyle.alignment = TextAnchor.LowerRight;

            GUILayout.BeginHorizontal(GUILayout.MaxHeight(50), GUILayout.MinHeight(50));
            if (Database.DataModel != null)
            {
                int index = 0;
                foreach (Nodes sheet in Database.DataModel.Sheets)
                {
                    if(index % 15 == 0)
                    {
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal(GUILayout.MaxHeight(50), GUILayout.MinHeight(50));
                    }
                    GUILayout.BeginHorizontal(GUILayout.MaxWidth(50), GUILayout.MaxWidth(50));
                    if (this.currentSheet == sheet)
                    {
                        GUI.color = Color.yellow;
                    }
                    HMButton.Sheet(sheet, SetCurrentSheet);
                    
                    GUI.color = Color.white;
                    GUILayout.EndHorizontal();
                    index++;
                }
            }
            
            if (GUILayout.Button("New", HMStyle.ButtonIcon, HMLayoutOptions.Icon45))
            {
                CreateNewSheet();
            }
            GUILayout.EndHorizontal();
        }

        private void DrawMenu()
        {
            if (parent.Forms != null)
            {
                toolbarForms = new List<HMForm>();
                List<string> toolbarString = new List<string>();
                foreach (HMForm form in parent.Forms)
                {
                    if (form.InMain)
                    {
                        toolbarForms.Add(form);
                        toolbarString.Add(form.Name);
                    }
                }
                GUILayout.BeginHorizontal(HMStyle.BoxStyle, GUILayout.MaxHeight(20));
                {
                    GUILayout.BeginHorizontal();
                    toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarString.ToArray());
                    GUILayout.EndHorizontal();
                }
                {
                    HMButton.Action("Refresh", delegate ()
                    {
                        Compute();
                    });
                    HMButton.Action("Delete", delegate ()
                    {
                        DeleteSheet();
                    });
                }
                GUILayout.FlexibleSpace();
                { 
                    if(Settings.Instance.DisplayTotal) GUI.color = Color.yellow;
                    HMButton.Action("Total", delegate ()
                    {
                        Settings.Instance.DisplayTotal = !Settings.Instance.DisplayTotal;
                    });
                    GUI.color = Color.white;
                    HMButton.Action("Logistic", delegate ()
                    {
                        Settings.Instance.DisplayLogistic = !Settings.Instance.DisplayLogistic;
                    });
                    if (Settings.Instance.DisplayLogistic)
                    {
                        foreach (Item logistic in Database.LogisticItems)
                        {
                            if(logistic.Id == Settings.Instance.ItemIdLogistic)
                            {
                                GUI.color = Color.yellow;
                            }
                            HMButton.IconLogistic(logistic, delegate(IItem element) {
                                HMEventQueue.EnQueue(this, new HMEvent(HMEventType.ChangeLogisticItem, element));
                            });
                            GUI.color = Color.white;
                        }
                    }
                }
                GUILayout.FlexibleSpace();

                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Base time:");
                    timeSelected = GUILayout.Toolbar(timeSelected, timesString.ToArray());
                    GUILayout.EndHorizontal();
                }

                GUILayout.EndHorizontal();

                if (toolbarForms != null && toolbarInt > -1)
                {
                    toolbarForms[toolbarInt].SwitchShow();
                    toolbarInt = -1;
                }
            }
        }

        private void DrawData()
        {
            GUILayout.BeginHorizontal();
            DrawNavigate();
            DrawTable();
            DrawDetail();
            GUILayout.EndHorizontal();

        }

        private void DrawNavigate()
        {
            ScrollNavPosition = GUILayout.BeginScrollView(ScrollNavPosition, HMLayoutOptions.ScrollNav);
            GUILayout.BeginVertical();
            if (currentSheet != null)
            {
                if (this.currentNode == currentSheet)
                {
                    GUI.color = Color.yellow;
                }
                HMCell.Node(currentSheet, Classes.Language.Get("open.sheet"), delegate (INode element) {
                    SetCurrentNodes((Nodes)element);
                });
                GUI.color = Color.white;
                DrawNavigate(currentSheet);
            }
            else
            {
                GUILayout.Label("");
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }
        private void DrawNavigate(Nodes nodes)
        {
            if (nodes != null && nodes.Children != null && nodes.Children.Count > 0)
            {
                int index = 1;
                List<INode> navNodes = nodes.Children.Where(item => item is Nodes).ToList();
                foreach (Node node in navNodes)
                {
                    if (node is Nodes)
                    {
                        if(index == navNodes.Count) GUILayout.BeginHorizontal(HMStyle.BoxNavigateTreeLeaf);
                        else GUILayout.BeginHorizontal(HMStyle.BoxNavigateTree);
                        {
                            GUILayout.BeginVertical();
                            Nodes childNodes = (Nodes)node;
                            if (this.currentNode == childNodes)
                            {
                                GUI.color = Color.yellow;
                            }
                            HMCell.Node(node, Classes.Language.Get("open.node"), delegate (INode element)
                            {
                                SetCurrentNodes((Nodes)element);
                            });
                            GUI.color = Color.white;
                            DrawNavigate(childNodes);
                            GUILayout.EndVertical();
                        }
                        GUILayout.EndHorizontal();
                        index++;
                    }
                }
            }
        }

        private void DrawDetail()
        {
            GUILayout.BeginVertical(HMLayoutOptions.ListDetail);
            GUILayout.BeginHorizontal(HMStyle.BoxStyle, GUILayout.MaxHeight(25));
            GUILayout.Label("Information", HMStyle.TextAlignMiddleCenter);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (currentNode != null)
            {
                double count = currentNode.Count;
                if (count == 0) count = 1.0;
                HMCell.NodePower(currentNode, currentNode.GetDeepCount(Settings.Instance.DisplayTotal) / currentNode.Count);
            }
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal(HMStyle.BoxStyle, GUILayout.MaxHeight(25));
            GUILayout.Label("Output", HMStyle.TextAlignMiddleCenter);
            GUILayout.EndHorizontal();
            ScrollOutputPosition = GUILayout.BeginScrollView(ScrollOutputPosition, HMStyle.ScrollListDetail, HMLayoutOptions.ScrollListDetail);
            if (currentNode != null && currentNode.Products != null)
            {
                HMCell.ItemProductList(currentNode, currentSheet.Time, true);
            }

            GUILayout.EndScrollView();

            GUILayout.BeginHorizontal(HMStyle.BoxStyle, GUILayout.MaxHeight(25));
            GUILayout.Label("Input", HMStyle.TextAlignMiddleCenter);
            GUILayout.EndHorizontal();

            ScrollInputPosition = GUILayout.BeginScrollView(ScrollInputPosition, HMStyle.ScrollListDetail, HMLayoutOptions.ScrollListDetail);
            if (currentNode != null && currentNode.Ingredients != null)
            {
                HMCell.ItemIngredientList(currentNode, currentSheet.Time, true, delegate(IItem element){
                        HMEventQueue.EnQueue(this, new HMEvent(HMEventType.AddRecipeByIngredient, element));
                });
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        private void DrawTableHeader()
        {
            GUIStyle textAlignStyle = new GUIStyle(GUI.skin.label);
            textAlignStyle.alignment = TextAnchor.MiddleCenter;

            GUILayout.BeginHorizontal(HMStyle.None, GUILayout.MaxHeight(25));

            GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMLayoutOptions.ColumnAction);
            GUILayout.Label("Action", textAlignStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMLayoutOptions.ColumnProduction);
            GUILayout.Label("%", textAlignStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMLayoutOptions.ColumnRecipe);
            GUILayout.Label("Recipe", textAlignStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMLayoutOptions.ColumnPower);
            GUILayout.Label("Power", textAlignStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMLayoutOptions.ColumnMachine);
            GUILayout.Label("Machine", textAlignStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMLayoutOptions.ColumnProducts);
            GUILayout.Label("Product", textAlignStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMLayoutOptions.ColumnIngredients);
            GUILayout.Label("Ingredient", textAlignStyle);
            GUILayout.EndHorizontal();

            GUILayout.EndHorizontal();
        }

        private void DrawTable()
        {
            GUILayout.BeginVertical(HMStyle.None);
            if (currentNode != null && currentNode.Children.Count > 0)
            {
                DrawTableHeader();
                bool displayLogistic = Settings.Instance.DisplayLogistic;
                GUIStyle textAlignStyle = new GUIStyle(GUI.skin.label);
                textAlignStyle.alignment = TextAnchor.LowerRight;

                scrollPosition = GUILayout.BeginScrollView(scrollPosition, HMStyle.ScrollData, HMLayoutOptions.ScrollData);
                int end = currentNode.Children.Count;
                for (int index = 0; index < end; index++ )
                {
                    INode node = currentNode.Children[index];
                    GUILayout.BeginHorizontal(HMStyle.None);

                    // actions
                    //HMLogger.Debug("DrawTable:actions");
                    GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMLayoutOptions.GetDataCell(DataColumn.Actions, displayLogistic));
                    HMCell.NodeActions(currentNode, node);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMLayoutOptions.GetDataCell(DataColumn.Production, displayLogistic));
                    node.ProductionString = GUILayout.TextField(node.ProductionString);
                    GUILayout.EndHorizontal();
                    // recipe
                    //HMLogger.Debug("DrawTable:recipe");
                    GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMLayoutOptions.GetDataCell(DataColumn.Recipe, displayLogistic));
                    HMCell.Node(node, currentNode.GetDeepCount(Settings.Instance.DisplayTotal), Classes.Language.Get("edition.recipe"), delegate (INode element)
                    {
                        if (element is IRecipe)
                        {
                            HMEventQueue.EnQueue(currentNode, new HMEvent(HMEventType.EditionRecipe, element));
                        }
                    });
                    GUILayout.EndHorizontal();
                    // power
                    //HMLogger.Debug("DrawTable:power");
                    GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMLayoutOptions.GetDataCell(DataColumn.Power, displayLogistic));
                    HMCell.NodePower(node, currentNode.GetDeepCount(Settings.Instance.DisplayTotal), delegate ()
                    {
                        if (node is IRecipe)
                        {
                            HMEventQueue.EnQueue(currentNode, new HMEvent(HMEventType.EditionRecipe, node));
                        }
                    });
                    GUILayout.EndHorizontal();
                    //machine
                    //HMLogger.Debug("DrawTable:machine");
                    GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMLayoutOptions.GetDataCell(DataColumn.Machine, displayLogistic));
                    if (node is IRecipe)
                    {
                        IRecipe recipe = (IRecipe)node;
                        HMCell.Product(recipe.Factory, currentNode.GetDeepCount(Settings.Instance.DisplayTotal), delegate(IItem item)
                        {
                            HMEventQueue.EnQueue(currentNode, new HMEvent(HMEventType.EditionRecipe, node));
                        });
                    }
                    else
                    {
                        GUILayout.Label("");
                    }
                    
                    GUILayout.EndHorizontal();
                    // Products
                    //HMLogger.Debug("DrawTable:Products");
                    GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMLayoutOptions.GetDataCell(DataColumn.Products, displayLogistic));
                    GUILayout.BeginVertical(HMStyle.None);
                    GUILayout.BeginHorizontal(HMStyle.None);
                    int pindex = 0;
                    foreach (IItem item in node.Products)
                    {
                        if (pindex != 0 && pindex % HMLayoutOptions.numProducts == 0)
                        {
                            GUILayout.FlexibleSpace();
                            GUILayout.EndHorizontal();
                            GUILayout.BeginHorizontal(HMStyle.None);
                        }
                        if (item.State == ItemState.Main || item.Count > 0.01)
                        {
                            item.Flow = item.Count * node.Effects.Productivity / currentSheet.Time;
                            HMCell.ItemProduct(item, node.GetItemDeepCount(Settings.Instance.DisplayTotal) * node.Effects.Productivity, true, delegate (IItem element)
                            {
                                if (element.State == ItemState.Main)
                                {
                                    HMEventQueue.EnQueue(currentNode, new HMEvent(HMEventType.EditionProduct, element));
                                }
                                else
                                {
                                    HMEventQueue.EnQueue(this, new HMEvent(HMEventType.AddRecipeByProduct, element));
                                }
                            });
                            pindex++;
                        }
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    //Ingredients
                    //HMLogger.Debug("DrawTable:Ingredients");
                    GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMLayoutOptions.GetDataCell(DataColumn.Ingredients, displayLogistic));
                    GUILayout.BeginVertical(HMStyle.None);
                    GUILayout.BeginHorizontal(HMStyle.None);
                    int iindex = 0;
                    foreach (IItem item in node.Ingredients)
                    {
                        if (iindex != 0 && iindex % HMLayoutOptions.numIngredients == 0)
                        {
                            GUILayout.FlexibleSpace();
                            GUILayout.EndHorizontal();
                            GUILayout.BeginHorizontal(HMStyle.None);
                        }
                        if (item.State == ItemState.Main || item.Count > 0.01)
                        {
                            item.Flow = item.Count / currentSheet.Time;
                            HMCell.ItemIngredient(item, node.GetItemDeepCount(Settings.Instance.DisplayTotal), true, delegate (IItem element)
                            {
                                HMEventQueue.EnQueue(this, new HMEvent(HMEventType.AddRecipeByIngredient, element));
                            });
                            iindex++;
                        }
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();

                    GUILayout.EndHorizontal();
                }
                
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
        }

        private void CreateNewSheet()
        {
            Nodes sheet = new Nodes();
            Database.DataModel.Sheets.Add(sheet);
            SetCurrentSheet(sheet);
        }
        private void SetCurrentNodes(Nodes nodes)
        {
            this.currentNode = nodes;
        }
        private void SetCurrentSheet(Nodes sheet)
        {
            this.currentSheet = sheet;
            this.currentNode = sheet;
            switch (sheet.Time)
            {
                case 60:
                    this.timeSelected = 1;
                    break;
                case 3600:
                    this.timeSelected = 2;
                    break;
                default:
                    this.timeSelected = 0;
                    break;
            }
        }
        public void OnEvent(object sender, HMEvent e)
        {
            Nodes parentNode;
            Node node;
            IRecipe recipe;
            Item item;
            switch (e.Type)
            {
                case HMEventType.LoadedModel:
                    if (Database.DataModel.Sheets.Count > 0) SetCurrentSheet(Database.DataModel.Sheets.First());
                    else CreateNewSheet();
                    break;
                case HMEventType.AddRecipe:
                    if (currentNode == null) CreateNewSheet();
                    recipe = e.GetItem<IRecipe>();
                    AddRecipe(recipe);
                    break;
                case HMEventType.ChooseRecipe:
                    if (currentNode == null) CreateNewSheet();
                    recipe = e.GetItem<IRecipe>();
                    AddRecipe(recipe);
                    break;
                case HMEventType.AddRecipeByIngredient:
                    item = e.GetItem<Item>();
                    AddRecipe(item);
                    break;
                case HMEventType.RemoveNode:
                    parentNode = (Nodes)sender;
                    node = e.GetItem<Node>();
                    parentNode.Remove(node);
                    Compute();
                    break;
                case HMEventType.UpdateSheet:
                    Compute();
                    break;
                case HMEventType.UpNode:
                    parentNode = (Nodes)sender;
                    node = e.GetItem<Node>();
                    ListExtensions.MoveStep(parentNode.Children, node, -1);
                    Compute();
                    break;
                case HMEventType.DownNode:
                    parentNode = (Nodes)sender;
                    node = e.GetItem<Node>();
                    ListExtensions.MoveStep(parentNode.Children, node, 1);
                    Compute();
                    break;
                case HMEventType.ChangeLogisticItem:
                    item = e.GetItem<Item>();
                    Settings.Instance.ItemIdLogistic = item.Id;
                    break;
                case HMEventType.UpLevelNode:
                    node = e.GetItem<Node>();
                    if (node.Parent != null)
                    {
                        node.Parent.UpLevelNode(node);
                        Compute();
                    }
                    break;
                case HMEventType.DownLevelNode:
                    node = e.GetItem<Node>();
                    if (node.Parent != null)
                    {
                        node.Parent.DownLevelNode(node);
                        Compute();
                    }
                    break;
            }
        }

        private void AddRecipe(IRecipe recipe)
        {
            IRecipe newRecipe = recipe.Clone();
            UpdateRecipeFactory(newRecipe);
            currentNode.Add(newRecipe);
            Compute();
        }

        private void UpdateRecipeFactory(IRecipe recipe)
        {
            if (recipe.Factories.Count > 1)
            {
                if (Settings.Instance.FactorySelection == FactorySelection.Last)
                {
                    recipe.Factory = recipe.Factories.Last();
                }
                else
                {
                    recipe.Factory = recipe.Factories.First();
                }
            }
        }

        private void AddRecipe(Item item)
        {
            if (item != null && item.Proto != null)
            {
                List<IRecipe> recipes = Database.SelectRecipeByProduct(item);
                if (recipes != null && recipes.Count > 0)
                {
                    if (recipes.Count == 1)
                    {
                        IRecipe newRecipe = recipes.First().Clone();
                        UpdateRecipeFactory(newRecipe);
                        currentNode.Add(newRecipe);
                        Compute();
                    }
                    else
                    {
                        HMEvent.SendEvent(currentNode, new HMEvent(HMEventType.SwitchChooseRecipe, item));
                    }
                }
            }

        }

        private void Compute()
        {
            Compute compute = new Compute();
            compute.Update(currentSheet);
        }

        private void ComputeAll()
        {
            Compute compute = new Compute();
            foreach(Nodes sheet in Database.DataModel.Sheets)
            {
                compute.Update(sheet);
            }
        }

        private void DeleteSheet()
        {
            Database.DataModel.Sheets.Remove(currentSheet);
            if(Database.DataModel.Sheets.Count == 0)
            {
                CreateNewSheet();
            }
            else
            {
                SetCurrentSheet(Database.DataModel.Sheets.First());
            }
        }

        public override void OnClose()
        {
            Database.SaveModel();
        }
    }
}
