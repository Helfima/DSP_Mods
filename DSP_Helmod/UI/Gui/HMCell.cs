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
        public static void Node(Node node, Callback.ForNode callback = null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label($"{node.Count}x", HMStyle.TextAlignLowerRight);
            HMButton.Node(node, callback);
            GUILayout.EndHorizontal();
        }
        public static void Item(Item item, Callback.ForItem callback = null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label($"{item.Count}x", HMStyle.TextAlignLowerRight);
            HMButton.Item(item, callback);
            GUILayout.EndHorizontal();
        }

    }
}
