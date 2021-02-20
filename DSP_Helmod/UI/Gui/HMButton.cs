using DSP_Helmod.Classes;
using DSP_Helmod.Model;
using DSP_Helmod;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DSP_Helmod.UI.Gui
{
    public class HMButton
    {
        private static Texture2D infoTexture = LoadAssembly.LoadTexture2D("info", 64, 64);
        
        public static void Sheet(Nodes sheet, Callback.ForSheet callback)
        {
            if (sheet.Icon == null)
            {
                if (GUILayout.Button(infoTexture, HMStyle.Icon45LayoutOptions))
                {
                    callback(sheet);
                }
            }
            else
            {
                if (GUILayout.Button(sheet.Icon, HMStyle.Icon45LayoutOptions))
                {
                    callback(sheet);
                }
            }
        }

        public static void Node(Node node, Callback.ForNode callback)
        {
            if (node.Icon == null)
            {
                if (GUILayout.Button("?", HMStyle.Icon45LayoutOptions))
                {
                    callback(node);
                }
            }
            else
            {
                if (GUILayout.Button(node.Icon, HMStyle.Icon45LayoutOptions))
                {
                    callback(node);
                }
            }
        }

        public static void Node(Node node, string action, Callback.ForNode callback)
        {
            if (action != null)
            {
                if (GUILayout.Button(action, HMStyle.ActionButtonLayoutOptions))
                {
                    callback(node);
                }
            }
        }

        public static void Item(Item item, Callback.ForItem callback = null)
        {
            ItemColored(item, ItemColor.Normal, callback);
        }
        public static void ItemColored(Item item, ItemColor color, Callback.ForItem callback = null)
        {
            GUIStyle style = new GUIStyle(GUI.skin.button);
            switch (color)
            {
                case ItemColor.Blue:
                    style = HMStyle.ButtonIconBlue;
                    break;
                case ItemColor.Green:
                    style = HMStyle.ButtonIconGreen;
                    break;
                case ItemColor.Orange:
                    style = HMStyle.ButtonIconOrange;
                    break;
                case ItemColor.Red:
                    style = HMStyle.ButtonIconRed;
                    break;
                case ItemColor.Yellow:
                    style = HMStyle.ButtonIconYellow;
                    break;
            }


            if (item.Icon == null)
            {
                if (GUILayout.Button("?", style, HMStyle.Icon45LayoutOptions))
                {
                    if(callback != null) callback(item);
                }
            }
            else
            {
                if (GUILayout.Button(item.Icon, style, HMStyle.Icon45LayoutOptions))
                {
                    if (callback != null) callback(item);
                }
            }
        }
        public static void Icon(Item item, Callback.ForItem callback = null)
        {
            GUIStyle contentStyle = new GUIStyle(GUI.skin.button);
            contentStyle.normal.background = item.Icon;
            contentStyle.onHover.background = item.Icon;
            contentStyle.fontStyle = FontStyle.Bold;
            contentStyle.alignment = TextAnchor.LowerRight;
            if (GUILayout.Button(item.Count.ToString(), contentStyle, HMStyle.Icon45LayoutOptions))
            {
                if (callback != null) callback(item);
            }
            
        }
    }
}
