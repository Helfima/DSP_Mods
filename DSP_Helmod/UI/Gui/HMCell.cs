using DSP_Helmod.Classes;
using DSP_Helmod.Math;
using DSP_Helmod.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DSP_Helmod.UI.Gui
{
    public class HMCell
    {
        private static double limit = 10;
        public static void Node(INode node, string tooltip = null, Callback.ForNode callback = null)
        {
            GUILayout.BeginVertical(HMStyle.BoxIconLayoutOptions);
            HMButton.Node(node, tooltip, callback);
            GUILayout.BeginHorizontal(HMStyle.TextBoxStyle, HMStyle.IconText45LayoutOptions);
            if (node.Count < limit) GUILayout.Label($"{node.Count:N2}", HMStyle.TextButtonIcon);
            else GUILayout.Label($"{node.Count:N1}", HMStyle.TextButtonIcon);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        public static void Node(INode node, double factor, string tooltip = null, Callback.ForNode callback = null)
        {
            double count = node.Count * factor;
            GUILayout.BeginVertical(HMStyle.BoxIconLayoutOptions);
            HMButton.Node(node, tooltip, callback);
            GUILayout.BeginHorizontal(HMStyle.TextBoxStyle, HMStyle.IconText45LayoutOptions);
            if (count < limit) GUILayout.Label($"{count:N2}", HMStyle.TextButtonIcon);
            else GUILayout.Label($"{count:N1}", HMStyle.TextButtonIcon);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        public static void NodePower(INode node, double factor, Callback.ForVoid callback = null)
        {
            double power = node.Power * factor;
            if (!(node is IRecipe)) power = power * node.Count;
            GUILayout.BeginVertical(HMStyle.BoxIconLayoutOptions);
            HMButton.Texture(HMTexture.eclaireTexture, callback);
            GUILayout.BeginHorizontal(HMStyle.TextBoxStyle, HMStyle.IconText45LayoutOptions);
            if (power > 1e9) GUILayout.Label($"{power / 1e9:N1}GW", HMStyle.TextButtonIcon, GUILayout.Width(60));
            else if (power > 1e6) GUILayout.Label($"{power / 1e6:N1}MW", HMStyle.TextButtonIcon, GUILayout.Width(60));
            else if (power > 1e3) GUILayout.Label($"{power / 1e3:N1}kW", HMStyle.TextButtonIcon, GUILayout.Width(60));
            else GUILayout.Label($"{power:N1}W", HMStyle.TextButtonIcon, GUILayout.Width(60));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        public static void NodeActions(Nodes parent, INode node)
        {
            GUILayout.BeginVertical(HMStyle.BoxIconLayoutOptions);
            {
                GUILayout.BeginHorizontal();
                GUIContent actionUp = new GUIContent("U", "Action:Up");
                HMButton.ActionNode(node, actionUp, delegate (INode element)
                {
                    HMEventQueue.EnQueue(parent, new HMEvent(HMEventType.UpNode, element));
                });
                GUILayout.FlexibleSpace();
                GUIContent actionDelete = new GUIContent("X", "Action:Delete");
                HMButton.ActionNode(node, actionDelete, delegate (INode element)
                {
                    HMEventQueue.EnQueue(parent, new HMEvent(HMEventType.RemoveNode, element));
                });
                GUILayout.EndHorizontal();
            }
            {
                GUILayout.BeginHorizontal();
                GUIContent actionDown = new GUIContent("D", "Action:Down");
                HMButton.ActionNode(node, actionDown, delegate (INode element)
                {
                    HMEventQueue.EnQueue(parent, new HMEvent(HMEventType.DownNode, element));
                });
                GUIContent actionDownLevel = new GUIContent("<", "Action:DownLevel");
                HMButton.ActionNode(node, actionDownLevel, delegate (INode element)
                {
                    HMEventQueue.EnQueue(parent, new HMEvent(HMEventType.DownLevelNode, element));
                });
                GUIContent actionUpLevel = new GUIContent(">", "Action:UpLevel");
                HMButton.ActionNode(node, actionUpLevel, delegate (INode element)
                {
                    HMEventQueue.EnQueue(parent, new HMEvent(HMEventType.UpLevelNode, element));
                });
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
        public static void RecipeTime(IRecipe recipe, Callback.ForVoid callback = null)
        {
            GUILayout.BeginVertical(HMStyle.BoxIconLayoutOptions);
            HMButton.Texture(HMTexture.time, callback);
            GUILayout.BeginHorizontal(HMStyle.TextBoxStyle, HMStyle.IconText45LayoutOptions);
            if (recipe.Energy < limit) GUILayout.Label($"{recipe.Energy:N2}", HMStyle.TextButtonIcon);
            else GUILayout.Label($"{recipe.Energy:N1}", HMStyle.TextButtonIcon);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
        public static void Product(IItem item, double factor = 1, Callback.ForItem callback = null)
        {
            ItemColored(item, ItemColor.Normal, factor, false, callback);
        }

        private static void ItemColored(IItem item, ItemColor color, double factor = 1, bool withLogistic = false, Callback.ForItem callback = null)
        {
            GUILayout.BeginVertical(HMStyle.BoxIconLayoutOptions);
            HMButton.ItemColored(item, color, callback);
            GUILayout.BeginHorizontal(HMStyle.TextBoxStyle, HMStyle.IconText45LayoutOptions);
            if (item.Count * factor < limit) GUILayout.Label($"{item.Count * factor:N2}", HMStyle.TextButtonIcon);
            else GUILayout.Label($"{item.Count * factor:N1}", HMStyle.TextButtonIcon);
            GUILayout.EndHorizontal();
            if (withLogistic && Settings.Instance.DisplayLogistic)
            {
                GUILayout.BeginHorizontal(HMStyle.TextBoxStyle, HMStyle.IconText45LayoutOptions);
                IItem logistic = Compute.GetLogisticItem(item);
                GUILayout.Box(logistic.Icon, HMStyle.BoxIcon, HMStyle.Icon15LayoutOptions);
                GUILayout.Label($"{logistic.Count * factor:N1}", HMStyle.TextButtonIcon);
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        public static void ItemProduct(IItem item, double factor = 1, Callback.ForItem callback = null)
        {
            ItemColor itemColor = ItemColor.Normal;
            switch (item.State)
            {
                case ItemState.Main:
                    itemColor = ItemColor.Green;
                    break;
                case ItemState.Overflow:
                    itemColor = ItemColor.Red;
                    break;
                case ItemState.Residual:
                    itemColor = ItemColor.Blue;
                    break;
            }
            ItemColored(item, itemColor, factor, true, callback);
        }

        public static void ItemIngredient(IItem item, double factor = 1, Callback.ForItem callback = null)
        {
            ItemColor itemColor = ItemColor.Yellow;
            ItemColored(item, itemColor, factor, true, callback);
        }

        public static void ItemList(List<IItem> items, Callback.ForItem callback = null)
        {
            GUILayout.BeginHorizontal();
            int index = 0;
            foreach (IItem item in items)
            {
                if (index % 5 == 0)
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                }
                if (item.State == ItemState.Main || item.Count > 0.01)
                {
                    Product(item, 1, callback);
                    index++;
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        public static void ItemProductList(INode node, double time, Callback.ForItem callback = null)
        {
            GUILayout.BeginHorizontal();
            int index = 0;
            foreach (IItem item in node.Products)
            {
                if (index % 5 == 0)
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                }
                if (item.State == ItemState.Main || item.Count > 0.01)
                {
                    item.Flow = item.Count / time;
                    ItemProduct(item, node.GetDeepCount(Settings.Instance.DisplayTotal), callback);
                    index++;
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        public static void ItemIngredientList(INode node, double time, Callback.ForItem callback = null)
        {
            GUILayout.BeginHorizontal();
            int index = 0;
            foreach (IItem item in node.Ingredients)
            {
                if (index % 5 == 0)
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                }
                if (item.State == ItemState.Main || item.Count > 0.01)
                {
                    item.Flow = item.Count / time;
                    ItemIngredient(item, node.GetDeepCount(Settings.Instance.DisplayTotal), callback);
                    index++;
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }



    }
}
