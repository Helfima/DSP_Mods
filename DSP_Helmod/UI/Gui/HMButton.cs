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

        public static void Texture(Texture2D texture, Callback.ForVoid callback)
        {
            if (texture != null)
            {
                if (GUILayout.Button(texture, HMStyle.ButtonIcon, HMLayoutOptions.Icon45))
                {
                    if (callback != null) callback();
                }
            }
        }
        public static void Sheet(Nodes sheet, Callback.ForSheet callback)
        {
            if (sheet.Icon == null)
            {
                if (GUILayout.Button(infoTexture, HMStyle.ButtonIcon, HMLayoutOptions.Icon45))
                {
                    if (callback != null) callback(sheet);
                }
            }
            else
            {
                if (GUILayout.Button(sheet.Icon, HMStyle.ButtonIcon, HMLayoutOptions.Icon45))
                {
                    if (callback != null) callback(sheet);
                }
            }
        }

        public static void Node(INode node, Callback.ForNode callback)
        {
            if (node.Icon == null)
            {
                if (GUILayout.Button("?", HMStyle.ButtonIcon, HMLayoutOptions.Icon45))
                {
                    if(callback != null) callback(node);
                }
            }
            else
            {
                if (GUILayout.Button(node.Icon, HMStyle.ButtonIcon, HMLayoutOptions.Icon45))
                {
                    if (callback != null) callback(node);
                }
            }
        }

        public static void Node(INode node, string tooltip, Callback.ForNode callback)
        {
            if (node.Icon == null)
            {
                GUIContent action = new GUIContent("?", tooltip);
                if (GUILayout.Button(action, HMStyle.ButtonIcon, HMLayoutOptions.Icon45))
                {
                    if (callback != null) callback(node);
                }
            }
            else
            {
                GUIContent action = new GUIContent(node.Icon, tooltip);
                if (GUILayout.Button(action, HMStyle.ButtonIcon, HMLayoutOptions.Icon45))
                {
                    if (callback != null) callback(node);
                }
            }
        }

        public static void ActionNode(INode node, GUIContent action, Callback.ForNode callback)
        {
            if (action != null)
            {
                if (GUILayout.Button(action, HMLayoutOptions.ActionButton))
                {
                    if (callback != null) callback(node);
                }
            }
        }

        public static void Action(string action, Callback.ForVoid callback)
        {
            if (action != null)
            {
                if (GUILayout.Button(action))
                {
                    if (callback != null) callback();
                }
            }
        }

        public static void Item(IItem item, double factor = 1.0, Callback.ForItem callback = null)
        {
            ItemColored(item, ItemColor.Normal, factor, callback);
        }
        public static void ItemColored(IItem item, ItemColor color, double factor = 1.0, Callback.ForItem callback = null)
        {
            GUIStyle style = HMStyle.ButtonIcon;
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
                if (GUILayout.Button("?", style, HMLayoutOptions.Icon45))
                {
                    if(callback != null) callback(item);
                }
            }
            else
            {
                GUIContent content = new GUIContent(item.Icon, $"{item.Name}\nFlow: {item.Flow * factor * 60:N2}/mn");
                if (GUILayout.Button(content, style, HMLayoutOptions.Icon45))
                {
                    if (callback != null) callback(item);
                }
            }
        }
        public static void Icon(IItem item, Callback.ForItem callback = null)
        {
            GUIStyle contentStyle = new GUIStyle(GUI.skin.button);
            contentStyle.normal.background = item.Icon;
            contentStyle.onHover.background = item.Icon;
            contentStyle.fontStyle = FontStyle.Bold;
            contentStyle.alignment = TextAnchor.LowerRight;
            if (GUILayout.Button(item.Count.ToString(), contentStyle, HMLayoutOptions.Icon45))
            {
                if (callback != null) callback(item);
            }
            
        }

        public static void IconLogistic(IItem item, Callback.ForItem callback = null)
        {
            if (GUILayout.Button(item.Icon, HMLayoutOptions.Icon30))
            {
                if (callback != null) callback(item);
            }

        }
    }
}
