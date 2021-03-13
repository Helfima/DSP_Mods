using DSP_Helmod.Classes;
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
        public static GUIStyle FormTooltip = ChangeTexture(new GUIStyle(GUI.skin.window), Texture2D.blackTexture, Texture2D.blackTexture);

        public static GUIStyle ButtonIcon = new GUIStyle(GUI.skin.button) {
            // interieur
            padding = new RectOffset(5, 5, 5, 5),
            // exterieur
            margin = new RectOffset(1, 1, 1, 1)
        };
        public static GUIStyle ButtonIconBlue = ChangeTexture(ButtonIcon, HMTexture.icon_blue);
        public static GUIStyle ButtonIconGreen = ChangeTexture(ButtonIcon, HMTexture.icon_green);
        public static GUIStyle ButtonIconOrange = ChangeTexture(ButtonIcon, HMTexture.icon_orange);
        public static GUIStyle ButtonIconRed = ChangeTexture(ButtonIcon, HMTexture.icon_red);
        public static GUIStyle ButtonIconYellow = ChangeTexture(ButtonIcon, HMTexture.icon_yellow);

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
            padding = new RectOffset(1, 1, 1, 1),
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
        public static GUIStyle BoxTooltip = ChangeTexture(new GUIStyle()
        {
            margin = new RectOffset(0, 0, 0, 0),
            padding = new RectOffset(3, 3, 3, 3),
            border = new RectOffset(5,5,5,5)
            
        }, HMTexture.tooltip_gray);

        public static GUIStyle TextBoxStyle = new GUIStyle(GUI.skin.box)
        {
            padding = new RectOffset(0, 0, 0, 0),
            margin = new RectOffset(1, 1, 1, 1),
        };

        public static GUIStyle ScrollListDetail = new GUIStyle()
        {

        };

        public static GUIStyle None = new GUIStyle()
        {
            margin = new RectOffset(0, 0, 0, 0),
            padding = new RectOffset(0, 0, 0, 0),
        };

        public static GUIStyle ScrollData = new GUIStyle()
        {
            margin = new RectOffset(0, 3, 0, 0),
            padding = new RectOffset(0, 0, 0, 0),
        };
    }
}
