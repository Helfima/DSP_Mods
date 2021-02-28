using System;
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

        protected DataModel model;
        protected Nodes currentSheet;
        protected Nodes currentNode;

        protected Vector2 ScrollOutputPosition;
        protected Vector2 ScrollInputPosition;
        protected Vector2 ScrollNavPosition;

        protected int timeSelected = 0;
        protected string[] timesString = new string[] {"1s", "1mn", "1h" };
        protected string SavePath;
        public MainPanel(UIController parent) : base(parent) {
            this.name = "DSP Helmod V1.1.0";
#if DEBUG
            this.Show = true;
#endif
            this.windowRect0 = new Rect(500, 200, 1400, 650);
            this.AlphaButtons = true;
        }
        public override void OnInit()
        {
            HMLogger.Debug("MainPanel.OnInit");
            if (SavePath == null)
            {
                // placement dans AppData
                SavePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DSP_Helmod/datamodel.xml");
                try
                {
                    // ancien chemin
                    string oldSave = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DSP_Helmod/datamodel.xml");
                    if (System.IO.File.Exists(oldSave))
                    {
                            string oldFolder = System.IO.Path.GetDirectoryName(oldSave);
                            string newFolder = System.IO.Path.GetDirectoryName(SavePath);
                            System.IO.Directory.Move(oldFolder, newFolder);
                    }
                }
                catch { }
            }
            if (model == null)
            {
                model = DataModelConverter.ReadXml(SavePath);
                if (model != null && model.Sheets != null && model.Sheets.Count > 0)
                {
                    SetCurrentSheet(model.Sheets.First());
                    ComputeAll();
                }
            }
            if (model == null) model = new DataModel();
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
            if (model != null)
            {
                int index = 0;
                foreach (Nodes sheet in model.Sheets)
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
            
            if (GUILayout.Button("New", HMStyle.Icon45LayoutOptions))
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
                    if (form.IsTool)
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
                            HMButton.IconLogistic(logistic, delegate(Item element) {
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
            ScrollNavPosition = GUILayout.BeginScrollView(ScrollNavPosition, HMStyle.ScrollNavLayoutOptions);
            GUILayout.BeginVertical();
            if (currentSheet != null)
            {
                if (this.currentNode == currentSheet)
                {
                    GUI.color = Color.yellow;
                }
                HMCell.Node(currentSheet, Classes.Language.Get("open.sheet"), delegate (Node element) {
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
                foreach (Node node in nodes.Children)
                {
                    if (node is Nodes)
                    {
                        GUILayout.BeginHorizontal(HMStyle.BoxNavigate);
                        {
                            //GUILayoutOption width = GUILayout.Width(5);
                            //if (index == nodes.Children.Count)
                            //{
                            //    GUILayout.BeginVertical(GUILayout.Height(50));
                            //    GUILayout.Box(HMTexture.icon_blue, HMStyle.TreeBarNavigate, width);
                            //    GUILayout.EndVertical();
                            //}
                            //else
                            //{
                            //    GUILayout.BeginVertical(HMStyle.TreeBarNavigateStretch);
                            //    GUILayout.BeginHorizontal(HMStyle.TreeBarNavigate, width);
                            //    GUILayout.EndHorizontal();
                            //    GUILayout.EndVertical();
                            //}
                        }
                        {
                            GUILayout.BeginVertical();
                            Nodes childNodes = (Nodes)node;
                            if (this.currentNode == childNodes)
                            {
                                GUI.color = Color.yellow;
                            }
                            HMCell.Node(node, Classes.Language.Get("open.node"), delegate (Node element)
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
            GUILayout.BeginVertical(GUILayout.Width(300));
            GUILayout.BeginHorizontal(HMStyle.BoxStyle, GUILayout.MaxHeight(25));
            GUILayout.Label("Information", HMStyle.TextAlignMiddleCenter);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (currentNode != null)
            {
                HMCell.NodePower(currentNode, currentNode.GetDeepCount(Settings.Instance.DisplayTotal) / currentNode.Count);
            }
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal(HMStyle.BoxStyle, GUILayout.MaxHeight(25));
            GUILayout.Label("Output", HMStyle.TextAlignMiddleCenter);
            GUILayout.EndHorizontal();
            ScrollOutputPosition = GUILayout.BeginScrollView(ScrollOutputPosition, HMStyle.ScrollListDetail, HMStyle.ScrollListDetailLayoutOptions);
            if (currentNode != null && currentNode.Products != null)
            {
                HMCell.ItemProductList(currentNode, currentSheet.Time);
            }

            GUILayout.EndScrollView();

            GUILayout.BeginHorizontal(HMStyle.BoxStyle, GUILayout.MaxHeight(25));
            GUILayout.Label("Input", HMStyle.TextAlignMiddleCenter);
            GUILayout.EndHorizontal();

            ScrollInputPosition = GUILayout.BeginScrollView(ScrollInputPosition, HMStyle.ScrollListDetail, HMStyle.ScrollListDetailLayoutOptions);
            if (currentNode != null && currentNode.Ingredients != null)
            {
                HMCell.ItemIngredientList(currentNode, currentSheet.Time, delegate(Item element){
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

            GUILayout.BeginHorizontal(GUILayout.MaxHeight(25));

            GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMStyle.ColumnActionLayoutOptions);
            GUILayout.Label("Action", textAlignStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMStyle.ColumnProductionLayoutOptions);
            GUILayout.Label("%", textAlignStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMStyle.ColumnRecipeLayoutOptions);
            GUILayout.Label("Recipe", textAlignStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMStyle.ColumnPowerLayoutOptions);
            GUILayout.Label("Power", textAlignStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMStyle.ColumnMachineLayoutOptions);
            GUILayout.Label("Machine", textAlignStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMStyle.ColumnProductsLayoutOptions);
            GUILayout.Label("Product", textAlignStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMStyle.ColumnIngredientsLayoutOptions);
            GUILayout.Label("Ingredient", textAlignStyle);
            GUILayout.EndHorizontal();

            GUILayout.EndHorizontal();
        }

        private void DrawTable()
        {
            GUILayout.BeginVertical();
            if (currentNode != null && currentNode.Children.Count > 0)
            {
                DrawTableHeader();
                GUIStyle textAlignStyle = new GUIStyle(GUI.skin.label);
                textAlignStyle.alignment = TextAnchor.LowerRight;

                scrollPosition = GUILayout.BeginScrollView(scrollPosition, HMStyle.ScrollDataLayoutOptions);
                int end = currentNode.Children.Count;
                for (int index = 0; index < end; index++ )
                {
                    Node node = currentNode.Children[index];
                    GUILayout.BeginHorizontal(GUILayout.MaxHeight(70));

                    // actions
                    GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMStyle.ColumnActionLayoutOptions);
                    HMCell.NodeActions(currentNode, node);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMStyle.ColumnProductionLayoutOptions);
                    GUILayout.TextField("100");
                    GUILayout.EndHorizontal();
                    // recipe
                    GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMStyle.ColumnRecipeLayoutOptions);
                    HMCell.Node(node, currentNode.GetDeepCount(Settings.Instance.DisplayTotal), Classes.Language.Get("edition.recipe"), delegate (Node element)
                    {
                        if (element is Recipe)
                        {
                            HMEventQueue.EnQueue(currentNode, new HMEvent(HMEventType.EditionRecipe, element));
                        }
                    });
                    GUILayout.EndHorizontal();
                    // power
                    GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMStyle.ColumnPowerLayoutOptions);
                    HMCell.NodePower(node, currentNode.GetDeepCount(Settings.Instance.DisplayTotal), delegate ()
                    {
                        if (node is Recipe)
                        {
                            HMEventQueue.EnQueue(currentNode, new HMEvent(HMEventType.EditionRecipe, node));
                        }
                    });
                    GUILayout.EndHorizontal();
                    //machine
                    GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMStyle.ColumnMachineLayoutOptions);
                    if(node is Recipe)
                    {
                        Recipe recipe = (Recipe)node;
                        HMCell.Item(recipe.Factory, currentNode.GetDeepCount(Settings.Instance.DisplayTotal), delegate(Item item)
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
                    GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMStyle.ColumnProductsLayoutOptions);
                    foreach (Item item in node.Products)
                    {
                        if (item.State == ItemState.Main || item.Count > 0.01)
                        {
                            item.Flow = item.Count / currentSheet.Time;
                            HMCell.ItemProduct(item, node.GetItemDeepCount(Settings.Instance.DisplayTotal), delegate (Item element)
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
                        }
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMStyle.ColumnIngredientsLayoutOptions);
                    foreach (Item item in node.Ingredients)
                    {
                        if (item.State == ItemState.Main || item.Count > 0.01)
                        {
                            item.Flow = item.Count / currentSheet.Time;
                            HMCell.ItemIngredient(item, node.GetItemDeepCount(Settings.Instance.DisplayTotal), delegate (Item element)
                            {
                                HMEventQueue.EnQueue(this, new HMEvent(HMEventType.AddRecipeByIngredient, element));
                            });
                        }
                    }
                    GUILayout.FlexibleSpace();
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
            model.Sheets.Add(sheet);
            SetCurrentSheet(sheet);
        }
        private void SetCurrentNodes(Nodes nodes)
        {
            HMLogger.Debug($"Old Current Nodes: {currentNode.Type}.{currentNode.Name}");
            HMLogger.Debug($"SetCurrentNodes: {nodes.Type}.{nodes.Name}");
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
            HMLogger.Debug("OnEvent()");
            Nodes parentNode;
            Node node;
            RecipeProto recipeProto;
            Item item;
            switch (e.Type)
            {
                case HMEventType.OpenClose:
                    SwitchShow();
                    break;
                case HMEventType.AddRecipe:
                    if (currentNode == null) CreateNewSheet();
                    recipeProto = e.GetItem<RecipeProto>();
                    AddRecipe(recipeProto);
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

        private void AddRecipe(RecipeProto recipe)
        {
            HMLogger.Debug(recipe.madeFromString);
            currentNode.Add(new Recipe(recipe));
            Compute();
        }

        private void AddRecipe(Item item)
        {
            if (item != null && item.Proto != null && item.Proto.recipes != null && item.Proto.recipes.Count > 0)
            {
                if (item.Proto.recipes.Count == 1)
                {
                    RecipeProto recipe = item.Proto.recipes.First();
                    currentNode.Add(new Recipe(recipe));
                    Compute();
                }
                else
                {
                    HMEvent.SendEvent(currentNode, new HMEvent(HMEventType.ChooseRecipe, item));
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
            foreach(Nodes sheet in model.Sheets)
            {
                compute.Update(sheet);
            }
        }

        private void DeleteSheet()
        {
            model.Sheets.Remove(currentSheet);
            if(model.Sheets.Count == 0)
            {
                CreateNewSheet();
            }
            else
            {
                SetCurrentSheet(model.Sheets.First());
            }
        }

        public override void OnClose()
        {
            

            DataModelConverter.WriteXml(SavePath, model);
        }
    }
}
