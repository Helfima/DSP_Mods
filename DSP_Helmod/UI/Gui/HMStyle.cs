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
        internal static GUIStyle ChangeTexture(GUIStyle style, Texture2D image)
        {
            GUIStyle newStyle = new GUIStyle(style);
            newStyle.normal.background = image;
            return newStyle;
        }

        internal static GUIStyle ChangeTexture(GUIStyle style, Texture2D background, Texture2D active)
        {
            GUIStyle newStyle = new GUIStyle(style);
            newStyle.normal.background = background;
            newStyle.onNormal.background = active;
            newStyle.active.background = background;
            newStyle.onActive.background = active;
            return newStyle;
        }

        public static GUIStyle Form = ChangeTexture(new GUIStyle(GUI.skin.window), HMTexture.form_gray, HMTexture.form_gray_active);

        public static GUILayoutOption[] BoxIconLayoutOptions = new GUILayoutOption[] { GUILayout.Height(70) };

        public static GUILayoutOption[] Icon50LayoutOptions = new GUILayoutOption[] { GUILayout.Height(50), GUILayout.Width(50) };
        public static GUILayoutOption[] Icon45LayoutOptions = new GUILayoutOption[] { GUILayout.Height(45), GUILayout.Width(45) };
        public static GUILayoutOption[] IconText45LayoutOptions = new GUILayoutOption[] { GUILayout.Height(15), GUILayout.Width(45) };
        public static GUILayoutOption[] Icon30LayoutOptions = new GUILayoutOption[] { GUILayout.Height(30), GUILayout.Width(30) };
        public static GUILayoutOption[] IconText30LayoutOptions = new GUILayoutOption[] { GUILayout.Height(15), GUILayout.Width(30) };

        public static GUILayoutOption[] Icon20LayoutOptions = new GUILayoutOption[] { GUILayout.Height(20), GUILayout.Width(20) };
        public static GUILayoutOption[] Icon15LayoutOptions = new GUILayoutOption[] { GUILayout.Height(15), GUILayout.Width(15) };

        public static GUILayoutOption[] ActionButtonLayoutOptions = new GUILayoutOption[] { GUILayout.Height(25), GUILayout.Width(25) };

        public static GUILayoutOption[] ScrollListDetailLayoutOptions = new GUILayoutOption[] { GUILayout.Height(170) };

        public static GUILayoutOption[] ScrollNavLayoutOptions = new GUILayoutOption[] { GUILayout.Height(500), GUILayout.Width(150) };
        public static GUILayoutOption[] ScrollDataLayoutOptions = new GUILayoutOption[] { GUILayout.Height(500) };
        public static GUILayoutOption[] ScrollChooseLayoutOptions = new GUILayoutOption[] { GUILayout.Height(300) };
        public static GUILayoutOption[] ColumnActionLayoutOptions = new GUILayoutOption[] { GUILayout.Width(100) };
        public static GUILayoutOption[] ColumnProductionLayoutOptions = new GUILayoutOption[] { GUILayout.Width(40) };
        public static GUILayoutOption[] ColumnRecipeLayoutOptions = new GUILayoutOption[] { GUILayout.Width(80) };
        public static GUILayoutOption[] ColumnPowerLayoutOptions = new GUILayoutOption[] { GUILayout.Width(80) };
        public static GUILayoutOption[] ColumnMachineLayoutOptions = new GUILayoutOption[] { GUILayout.Width(80) };
        public static GUILayoutOption[] ColumnProductsLayoutOptions = new GUILayoutOption[] { GUILayout.Width(200) };
        public static GUILayoutOption[] ColumnIngredientsLayoutOptions = new GUILayoutOption[] { GUILayout.Width(320) };

        public static GUIStyle ButtonIconBlue = ChangeTexture(GUI.skin.button, HMTexture.icon_blue);
        public static GUIStyle ButtonIconGreen = ChangeTexture(GUI.skin.button, HMTexture.icon_green);
        public static GUIStyle ButtonIconOrange = ChangeTexture(GUI.skin.button, HMTexture.icon_orange);
        public static GUIStyle ButtonIconRed = ChangeTexture(GUI.skin.button, HMTexture.icon_red);
        public static GUIStyle ButtonIconYellow = ChangeTexture(GUI.skin.button, HMTexture.icon_yellow);

        public static GUIStyle BoxNavigate = new GUIStyle()
        {
            padding = new RectOffset(10, 0, 0, 0),
            margin = new RectOffset(0,0,0,0)
        };
        public static GUIStyle TreeBarNavigateStretch = ChangeTexture(new GUIStyle()
        {
            padding = new RectOffset(0, 0, 0, 0),
            margin = new RectOffset(0, 0, 0, 0),
            stretchHeight = true
        }, HMTexture.icon_orange);

        public static GUIStyle TreeBarNavigate = new GUIStyle(GUI.skin.box)
        {
            padding = new RectOffset(0, 0, 0, 0),
            margin = new RectOffset(0, 0, 0, 0)
        };

        public static GUIStyle TextAlignLowerRight = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.LowerRight
        };

        public static GUIStyle TextAlignMiddleCenter = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter
        };

        internal static GUIStyle ChangeTooltip(GUIStyle style)
        {
            GUIStyle newStyle = new GUIStyle(style);
            newStyle.normal.background = HMTexture.black;
            newStyle.stretchWidth = true;
            newStyle.stretchHeight = true;
            newStyle.alignment = TextAnchor.UpperLeft;
            return newStyle;
        }
        public static GUIStyle TextTooltip = ChangeTooltip(GUI.skin.label);

        public static GUIStyle TextButtonIcon = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            margin = new RectOffset(0, 0, 0, 0),
            padding = new RectOffset(0, 0, 0,0),
        };

        public static GUIStyle BoxStyle = new GUIStyle(GUI.skin.box)
        {
            margin = new RectOffset(2, 2, 2, 2)
        };

        public static GUIStyle BoxIcon = new GUIStyle()
        {
            margin = new RectOffset(0, 0, 0, 0),
            padding = new RectOffset(0, 0, 0, 0),
        };

        public static GUIStyle TextBoxStyle = new GUIStyle(GUI.skin.box)
        {
            margin = new RectOffset(2, 0, 0, 0),
            padding = new RectOffset(0, 0, 0, 2),
        };

        public static GUIStyle ScrollListDetail = new GUIStyle()
        {

        };

    }
}
