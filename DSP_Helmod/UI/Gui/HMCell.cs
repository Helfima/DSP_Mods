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
            GUILayout.BeginVertical(HMStyle.BoxIconLayoutOptions);
            HMButton.Node(node, callback);
            GUILayout.BeginHorizontal(HMStyle.TextBoxStyle, HMStyle.IconText45LayoutOptions);
            if (node.Count < limit) GUILayout.Label($"{node.Count:N2}", HMStyle.TextButtonIcon);
            else GUILayout.Label($"{node.Count:N1}", HMStyle.TextButtonIcon);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        public static void NodePower(Node node, Callback.ForVoid callback = null)
        {
            GUILayout.BeginVertical(HMStyle.BoxIconLayoutOptions);
            HMButton.Texture(HMTexture.eclaireTexture, callback);
            GUILayout.BeginHorizontal(HMStyle.TextBoxStyle, HMStyle.IconText45LayoutOptions);
            if (node.Power > 1e9) GUILayout.Label($"{node.Power / 1e9:N1}GW", HMStyle.TextButtonIcon, GUILayout.Width(60));
            else if (node.Power > 1e6) GUILayout.Label($"{node.Power / 1e6:N1}MW", HMStyle.TextButtonIcon, GUILayout.Width(60));
            else if (node.Power > 1e3) GUILayout.Label($"{node.Power / 1e3:N1}kW", HMStyle.TextButtonIcon, GUILayout.Width(60));
            else GUILayout.Label($"{node.Power:N1}W", HMStyle.TextButtonIcon, GUILayout.Width(60));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
        public static void NodeActions(Nodes parent, Node node)
        {
            GUILayout.BeginVertical(HMStyle.BoxIconLayoutOptions);
            {
                GUILayout.BeginHorizontal();
                GUIContent actionUp = new GUIContent("U", "Action:Up");
                HMButton.Node(node, actionUp, delegate (Node element)
                {
                    HMEventQueue.EnQueue(parent, new HMEvent(HMEventType.UpNode, element));
                });
                GUILayout.FlexibleSpace();
                GUIContent actionDelete = new GUIContent("X", "Action:Delete");
                HMButton.Node(node, actionDelete, delegate (Node element)
                {
                    HMEventQueue.EnQueue(parent, new HMEvent(HMEventType.RemoveNode, element));
                });
                GUILayout.EndHorizontal();
            }
            {
                GUILayout.BeginHorizontal();
                GUIContent actionDown = new GUIContent("D", "Action:Down");
                HMButton.Node(node, actionDown, delegate (Node element)
                {
                    HMEventQueue.EnQueue(parent, new HMEvent(HMEventType.DownNode, element));
                });
                GUIContent actionDownLevel = new GUIContent("<", "Action:DownLevel");
                HMButton.Node(node, actionDownLevel, delegate (Node element)
                {
                    HMEventQueue.EnQueue(parent, new HMEvent(HMEventType.DownLevelNode, element));
                });
                GUIContent actionUpLevel = new GUIContent(">", "Action:UpLevel");
                HMButton.Node(node, actionUpLevel, delegate (Node element)
                {
                    HMEventQueue.EnQueue(parent, new HMEvent(HMEventType.UpLevelNode, element));
                });
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
        public static void Item(Item item, double factor = 1, Callback.ForItem callback = null)
        {
            ItemColored(item, ItemColor.Normal, factor, callback);
        }

        public static void ItemColored(Item item, ItemColor color, double factor = 1, Callback.ForItem callback = null)
        {
            GUILayout.BeginVertical(HMStyle.BoxIconLayoutOptions);
            HMButton.ItemColored(item, color, callback);
            GUILayout.BeginHorizontal(HMStyle.TextBoxStyle, HMStyle.IconText45LayoutOptions);
            if (item.Count * factor < limit) GUILayout.Label($"{item.Count * factor:N2}", HMStyle.TextButtonIcon);
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
                if (item.State == ItemState.Main || item.Count > 0.01)
                {
                    Item(item, 1, callback);
                    index++;
                }
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
                if (item.State == ItemState.Main || item.Count > 0.01)
                {
                    ItemProduct(item, 1, callback);
                    index++;
                }
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
                if (item.State == ItemState.Main || item.Count > 0.01)
                {
                    ItemIngredient(item, 1, callback);
                    index++;
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }



    }
}
