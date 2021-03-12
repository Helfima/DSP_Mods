using DSP_Helmod.Classes;
using DSP_Helmod.UI.Gui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DSP_Helmod.UI.Core
{
    abstract public class HMTooltip : MonoBehaviour
    {
        protected new string name = "";
        protected UIController parent;
        protected Rect windowRect0 = new Rect(20, 20, 1000, 1000);
        public int id = 66700001;
        protected bool IsInit = false;
        public bool Show = false;
        public HMTooltip(UIController parent)
        {
            this.parent = parent;
        }
        protected void Init()
        {
            OnInit();
            IsInit = true;
        }
        abstract public void OnInit();
        public void OnUpdate()
        {
            string tooltip = parent.Tooltip;
            if (tooltip != null && tooltip != "" && tooltip != " ")
            {
                Show = true;
            }
            else Show = false;
        }

        public void OnGUI()
        {
            if (!IsInit) Init();
            AutoResize(1920, 1200);
            // change alpha
            GUI.backgroundColor = new Color(1, 1, 1, 0);
            // build window
            windowRect0 = GUI.Window(id, windowRect0, DoWindow, name, HMStyle.Form);
        }
        private void AutoResize(int screenWidth, int screenHeight, float factor = 1f)
        {
            Vector2 resizeRatio = new Vector2((float)Screen.width / screenWidth, (float)Screen.height / screenHeight);
            GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(resizeRatio.x * factor, resizeRatio.y * factor, 1.0f));
        }

        void DoWindow(int windowID)
        {
            windowRect0 = new Rect(parent.TooltipPosition.x + 10, parent.TooltipPosition.y, 1000, 1000);
            OnDoWindow(parent.Tooltip);
        }
        abstract public void OnDoWindow(string tooltip);
    }
}
