using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSP_Helmod.Model;
using DSP_Helmod.UI.Gui;
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

        protected Model model = new Model();
        protected Sheet currentSheet;
        protected Node currentNode;
        public MainPanel(UIController parent) : base(parent) {
            this.name = "Helmod v0.1";
            this.Show = true;
            this.windowRect0 = new Rect(20, 20, 1200, 500);
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
                foreach (Sheet sheet in model.Sheets)
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
            Sheet sheet = new Sheet();
            model.Sheets.Add(sheet);
            SetCurrentSheet(sheet);
        }
        private void SetCurrentSheet(Sheet sheet)
        {
            this.currentSheet = sheet;
            this.currentNode = sheet;
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

            GUILayout.BeginHorizontal(HMStyle.BoxStyle, GUILayout.MaxHeight(25));
            GUILayout.Label("Input", HMStyle.TextAlignMiddleCenter);
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        private void DrawTableHeader()
        {
            GUIStyle textAlignStyle = new GUIStyle(GUI.skin.label);
            textAlignStyle.alignment = TextAnchor.MiddleCenter;

            GUILayout.BeginHorizontal(GUILayout.MaxHeight(50));

            GUILayout.BeginHorizontal(HMStyle.BoxStyle, GUILayout.Width(50));
            GUILayout.Label("Action", textAlignStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(HMStyle.BoxStyle, GUILayout.Width(40));
            GUILayout.Label("%", textAlignStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(HMStyle.BoxStyle, GUILayout.Width(80));
            GUILayout.Label("Recipe", textAlignStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(HMStyle.BoxStyle, GUILayout.Width(80));
            GUILayout.Label("Power", textAlignStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(HMStyle.BoxStyle, GUILayout.Width(80));
            GUILayout.Label("Machine", textAlignStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(HMStyle.BoxStyle, GUILayout.Width(200));
            GUILayout.Label("Product", textAlignStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(HMStyle.BoxStyle, GUILayout.Width(200));
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

                scrollPosition = GUILayout.BeginScrollView(scrollPosition);
                int end = currentNode.Children.Count;
                for (int index = 0; index < end; index++ )
                {
                    Node node = currentNode.Children[index];
                    GUILayout.BeginHorizontal(GUILayout.MaxHeight(50));

                    GUILayout.BeginHorizontal(HMStyle.BoxStyle, GUILayout.Width(50));
                    GUILayout.Label("");
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(HMStyle.BoxStyle, GUILayout.Width(40));
                    GUILayout.TextField("100");
                    GUILayout.EndHorizontal();

                    Debug.Log("Draw node.Icon");
                    GUILayout.BeginHorizontal(HMStyle.BoxStyle, GUILayout.Width(80));
                    HMCell.Node(node);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(HMStyle.BoxStyle, GUILayout.Width(80));
                    GUILayout.Label("");
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(HMStyle.BoxStyle, GUILayout.Width(80));
                    GUILayout.Label("");
                    GUILayout.EndHorizontal();
                    Debug.Log("Draw node.Products");
                    Debug.Log($"node type: {node.GetType()}");
                    Debug.Log($"node.Products: {node.Products == null}");
                    GUILayout.BeginHorizontal(HMStyle.BoxStyle, GUILayout.Width(200));
                    foreach (Item item in node.Products)
                    {
                        HMCell.Item(item, delegate(Item element) { 
                            HMEvent.SendEvent(this, new HMEvent(HMEventType.AddRecipeByProduct, element)); 
                        });
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(HMStyle.BoxStyle, GUILayout.Width(200));
                    Debug.Log("Draw node.Ingredients");
                    foreach (Item item in node.Ingredients)
                    {
                        HMCell.Item(item, delegate (Item element) {
                            HMEvent.SendEvent(this, new HMEvent(HMEventType.AddRecipeByIngredient, element));
                        });
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.EndHorizontal();
                }
                
                GUILayout.EndScrollView();
                Debug.Log("end DrawTable()");
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
            }
        }

        private void AddRecipe(RecipeProto recipe)
        {
            currentNode.Children.Add(new Recipe(recipe));
        }

        private void AddRecipe(Item item)
        {
            if (item != null && item.Proto != null && item.Proto.recipes != null && item.Proto.recipes.Count > 0)
            {
                RecipeProto recipe = item.Proto.recipes.First();
                currentNode.Children.Add(new Recipe(recipe));
            }

        }
    }
}
