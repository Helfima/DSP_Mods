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

        protected DataModel model = new DataModel();
        protected Nodes currentSheet;
        protected Nodes currentNode;

        protected Vector2 ScrollOutputPosition;
        protected Vector2 ScrollInputPosition;

        protected int timeSelected = 0;
        protected string[] timesString = new string[] {"1s", "1mn", "1h" };
        public MainPanel(UIController parent) : base(parent) {
            this.name = "Helmod v0.1";
            this.Show = true;
            this.windowRect0 = new Rect(200, 20, 1200, 650);
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

        private void CreateNewSheet()
        {
            Nodes sheet = new Nodes();
            model.Sheets.Add(sheet);
            SetCurrentSheet(sheet);
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
                toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarString.ToArray());
                
                GUILayout.FlexibleSpace();
                
                GUILayout.BeginHorizontal();
                GUILayout.Label("Base time:");
                timeSelected = GUILayout.Toolbar(timeSelected, timesString.ToArray());
                GUILayout.EndHorizontal();

                GUILayout.EndHorizontal();

                if (toolbarForms != null && toolbarInt > -1)
                {
                    toolbarForms[toolbarInt].Show = !toolbarForms[toolbarInt].Show;
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
            GUILayout.BeginVertical();
            GUILayout.EndVertical();
        }

        private void DrawDetail()
        {
            GUILayout.BeginVertical(GUILayout.Width(300));

            GUILayout.BeginHorizontal(HMStyle.BoxStyle, GUILayout.MaxHeight(25));
            GUILayout.Label("Output", HMStyle.TextAlignMiddleCenter);
            GUILayout.EndHorizontal();
            ScrollOutputPosition = GUILayout.BeginScrollView(ScrollOutputPosition, HMStyle.ScrollListDetail, HMStyle.ScrollListDetailLayoutOptions);
            if (currentNode != null && currentNode.Products != null)
            {
                HMCell.ItemProductList(currentNode.Products);
            }

            GUILayout.EndScrollView();

            GUILayout.BeginHorizontal(HMStyle.BoxStyle, GUILayout.MaxHeight(25));
            GUILayout.Label("Input", HMStyle.TextAlignMiddleCenter);
            GUILayout.EndHorizontal();

            ScrollInputPosition = GUILayout.BeginScrollView(ScrollInputPosition, HMStyle.ScrollListDetail, HMStyle.ScrollListDetailLayoutOptions);
            if (currentNode != null && currentNode.Ingredients != null)
            {
                HMCell.ItemIngredientList(currentNode.Ingredients, delegate(Item element){
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
                    HMCell.Node(node);
                    GUILayout.EndHorizontal();
                    // power
                    GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMStyle.ColumnPowerLayoutOptions);
                    if (node.Power > 1e9) GUILayout.Label($"{node.Power / 1e9:N1}GW");
                    else if (node.Power > 1e6) GUILayout.Label($"{node.Power / 1e6:N1}MW");
                    else if (node.Power > 1e3) GUILayout.Label($"{node.Power / 1e3:N1}kW");
                    else GUILayout.Label($"{node.Power:N1}W");
                    GUILayout.EndHorizontal();
                    //machine
                    GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMStyle.ColumnMachineLayoutOptions);
                    if(node is Recipe)
                    {
                        Recipe recipe = (Recipe)node;
                        HMCell.Item(recipe.Factory);
                    }
                    else
                    {
                        GUILayout.Label("");
                    }
                    
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMStyle.ColumnProductsLayoutOptions);
                    foreach (Item item in node.Products)
                    {
                        HMCell.ItemProduct(item, node.Count, delegate(Item element) {
                            if(element.State == ItemState.Main)
                            {
                                HMEventQueue.EnQueue(currentNode, new HMEvent(HMEventType.EditionProduct, element));
                            }
                            else
                            {
                                HMEventQueue.EnQueue(this, new HMEvent(HMEventType.AddRecipeByProduct, element));
                            }
                        });
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(HMStyle.BoxStyle, HMStyle.ColumnIngredientsLayoutOptions);
                    foreach (Item item in node.Ingredients)
                    {
                        HMCell.ItemIngredient(item, node.Count, delegate (Item element) {
                            HMEventQueue.EnQueue(this, new HMEvent(HMEventType.AddRecipeByIngredient, element));
                        });
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.EndHorizontal();
                }
                
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
        }
        public void OnEvent(object sender, HMEvent e)
        {
            Debug.Log("OnEvent()");
            switch (e.Type)
            {
                case HMEventType.AddRecipe:
                    if (currentNode == null) CreateNewSheet();
                    RecipeProto recipeProto = e.GetItem<RecipeProto>();
                    AddRecipe(recipeProto);
                    break;
                case HMEventType.AddRecipeByIngredient:
                    Item item = e.GetItem<Item>();
                    AddRecipe(item);
                    break;
                case HMEventType.RemoveNode:
                    Nodes parentNode = (Nodes)sender;
                    Node node = e.GetItem<Node>();
                    parentNode.Remove(node);
                    Compute();
                    break;
                case HMEventType.UpdateSheet:
                    Compute();
                    break;
            }
        }

        private void AddRecipe(RecipeProto recipe)
        {
            Debug.Log(recipe.madeFromString);
            currentNode.Add(new Recipe(recipe));
            Compute();
        }

        private void AddRecipe(Item item)
        {
            if (item != null && item.Proto != null && item.Proto.recipes != null && item.Proto.recipes.Count > 0)
            {
                RecipeProto recipe = item.Proto.recipes.First();
                currentNode.Add(new Recipe(recipe));
                Compute();
            }

        }

        private void Compute()
        {
            Compute compute = new Compute();
            compute.Update(currentSheet);
        }

        private void Save()
        {
            string jsonModel = UnityEngine.JsonUtility.ToJson(model);
            System.IO.File.WriteAllText("C:/Temp/DataModel.json", jsonModel);
        }

        public override void OnClose()
        {

        }
    }
}
