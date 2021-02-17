using DSP_Helmod.Classes;
using DSP_Helmod.Model;
using DSPHelmod;
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
        
        public static void Sheet(Sheet sheet, Callback.ForSheet callback)
        {
            if (sheet.Icon == null)
            {
                if (GUILayout.Button("?", HMStyle.Icon45LayoutOptions))
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

        public static void Item(Item item, Callback.ForItem callback = null)
        {
            if (item.Icon == null)
            {
                if (GUILayout.Button("?", HMStyle.Icon45LayoutOptions))
                {
                    if(callback != null) callback(item);
                }
            }
            else
            {
                if (GUILayout.Button(item.Icon, HMStyle.Icon45LayoutOptions))
                {
                    if (callback != null) callback(item);
                }
            }
        }
    }
}
