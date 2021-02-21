using DSP_Helmod.Classes;
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
        public static void Node(Node node, Callback.ForNode callback = null)
        {
            GUILayout.BeginVertical();
            HMButton.Node(node, callback);
            GUILayout.BeginHorizontal(HMStyle.TextBoxStyle, HMStyle.IconText45LayoutOptions);
            if (node.Count < limit) GUILayout.Label($"{node.Count:N2}", HMStyle.TextButtonIcon);
            else GUILayout.Label($"{node.Count:N1}", HMStyle.TextButtonIcon);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
        public static void NodeActions(Nodes parent, Node node)
        {
            GUILayout.BeginVertical();
            HMButton.Node(node, "X", delegate (Node element) {
                HMEventQueue.EnQueue(parent, new HMEvent(HMEventType.RemoveNode, element));
            });
            GUILayout.EndVertical();
        }
        public static void Item(Item item, double factor = 1, Callback.ForItem callback = null)
        {
            ItemColored(item, ItemColor.Normal, factor, callback);
        }

        public static void ItemColored(Item item, ItemColor color, double factor = 1, Callback.ForItem callback = null)
        {
            GUILayout.BeginVertical();
            HMButton.ItemColored(item, color, callback);
            GUILayout.BeginHorizontal(HMStyle.TextBoxStyle, HMStyle.IconText45LayoutOptions);
            if (item.Count < limit) GUILayout.Label($"{item.Count * factor:N2}", HMStyle.TextButtonIcon);
            else GUILayout.Label($"{item.Count * factor:N1}", HMStyle.TextButtonIcon);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
        public static void ItemProduct(Item item, double factor = 1, Callback.ForItem callback = null)
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
            ItemColored(item, itemColor, factor, callback);
        }

        public static void ItemIngredient(Item item, double factor = 1, Callback.ForItem callback = null)
        {
            ItemColor itemColor = ItemColor.Yellow;
            ItemColored(item, itemColor, factor, callback);
        }

        public static void ItemList(List<Item> items, Callback.ForItem callback = null)
        {
            GUILayout.BeginHorizontal();
            int index = 0;
            foreach (Item item in items)
            {
                if (index % 5 == 0)
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                }
                Item(item, 1, callback);
                index++;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        public static void ItemProductList(List<Item> items, Callback.ForItem callback = null)
        {
            GUILayout.BeginHorizontal();
            int index = 0;
            foreach (Item item in items)
            {
                if (index % 5 == 0)
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                }
                ItemProduct(item, 1, callback);
                index++;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        public static void ItemIngredientList(List<Item> items, Callback.ForItem callback = null)
        {
            GUILayout.BeginHorizontal();
            int index = 0;
            foreach (Item item in items)
            {
                if (index % 5 == 0)
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                }
                ItemIngredient(item, 1, callback);
                index++;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }



    }
}
