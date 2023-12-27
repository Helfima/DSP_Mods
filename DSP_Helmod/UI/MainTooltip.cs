using DSP_Helmod.Model;
using DSP_Helmod.UI.Core;
using DSP_Helmod.UI.Gui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

namespace DSP_Helmod.UI
{
    public class MainTooltip : HMTooltip
    {
        public MainTooltip(UIController parent) : base(parent)
        {
        }
        public override void OnInit()
        {
            
        }
        
        public override void OnDoWindow(string tooltip)
        {
            try
            {
                GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                if (tooltip.StartsWith("Action:"))
                {
                    GUILayout.BeginVertical(HMStyle.BoxTooltip);
                    string label = tooltip.Substring(tooltip.IndexOf(':') + 1);
                    GUILayout.Label(label);
                    GUILayout.EndVertical();
                }
                else if (tooltip.StartsWith("Recipe:"))
                {
                    GUILayout.BeginVertical(HMStyle.BoxTooltip, new GUILayoutOption[] { GUILayout.Height(50), GUILayout.Width(250) });
                    DrawRecipeRegex(tooltip);
                    GUILayout.EndVertical();
                    //GUILayout.Label(tooltip);
                }
                else
                {
                    GUILayout.BeginVertical(HMStyle.BoxTooltip);
                    GUILayout.Label(tooltip);
                    GUILayout.EndVertical();
                }
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        private Regex RecipeRegex = new Regex("Recipe:([a-zA-Z]*)=([0-9]*)", RegexOptions.IgnoreCase);
        private void DrawRecipeRegex(string tooltip)
        {
            try
            {
                Match match = RecipeRegex.Match(tooltip);
                if (match.Success)
                {
                    string recipeType = match.Groups[1].Captures[0].Value;
                    string recipeId = match.Groups[2].Captures[0].Value;
                    int id;
                    int.TryParse(recipeId, out id);
                    IRecipe irecipe = Database.SelectRecipe(recipeType, id);
                    if (irecipe != null)
                    {
                        DrawRecipe(irecipe);
                    }
                }
            }
            catch
            {
            }
        }

        private void DrawRecipeSplit(string tooltip)
        {
            try { 
                string[] split1 = tooltip.Split(':');
                string[] split2 = split1[1].Split('=');
                string recipeType = split2[0];
                string recipeId = split2[1];
                int id;
                int.TryParse(recipeId, out id);
                IRecipe irecipe = Database.SelectRecipe(recipeType, id);
                if (irecipe != null)
                {
                    DrawRecipe(irecipe);
                }
            }
            catch
            {
            }
        }
        private void DrawRecipe(IRecipe irecipe)
        {
            if (irecipe != null)
            {
                DrawCell(irecipe.Icon, irecipe.Name);
                DrawCell(HMTexture.time, $"{irecipe.Energy}s");
                if (irecipe.Products.Count > 0)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Products:");
                    GUILayout.EndHorizontal();
                    foreach (IItem item in irecipe.Products)
                    {
                        DrawCell(item.Icon, $"x{item.Count}: {item.Name}");
                    }
                }
                if (irecipe.Ingredients.Count > 0)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Ingredients:");
                    GUILayout.EndHorizontal();
                    foreach (IItem item in irecipe.Ingredients)
                    {
                        DrawCell(item.Icon, $"x{item.Count}: {item.Name}");
                    }
                }
                if (irecipe.Factories.Count > 0)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Made in:");
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    foreach (IItem item in irecipe.Factories)
                    {
                        GUILayout.Box(item.Icon, HMLayoutOptions.Icon30);
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                }
            }
        }

        private void DrawCell(Texture2D icon, string label)
        {
            GUILayout.BeginHorizontal();
            try
            {
                GUILayout.Box(icon, HMLayoutOptions.Icon30);
                GUILayout.Label(label);
            }
            catch { }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }
}
