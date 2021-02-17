using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DSP_Helmod.UI.Gui
{
    public class HMStyle
    {
        public static GUILayoutOption[] Icon45LayoutOptions = new GUILayoutOption[] { GUILayout.Height(45), GUILayout.Width(45) };
        public static GUILayoutOption[] Icon30LayoutOptions = new GUILayoutOption[] { GUILayout.Height(30), GUILayout.Width(30) };

        public static GUIStyle TextAlignLowerRight = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.LowerRight
        };

        public static GUIStyle TextAlignMiddleCenter = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter
        };

        public static GUIStyle BoxStyle = new GUIStyle(GUI.skin.box)
        {
            margin = new RectOffset(5, 0, 0, 0)
        };
    }
}
