using UnityEngine;
using System.Collections;
using DSP_Helmod.Classes;
using System;
using DSP_Helmod.UI.Gui;

namespace DSP_Helmod.UI.Core
{
    abstract public class HMForm : MonoBehaviour
    {
        //public event EventHandler<HMEvent> HMEventHandler;

        protected UIController parent;
        protected Rect windowRect0 = new Rect(200, 20, 600, 300);
        public int id = 66600001;
        protected string name = "DSP Helmod";
        protected Color BackgroundColor = Color.white;

        protected bool IsInit = false;
        protected Vector2 scrollPosition;

        public bool Show = false;
        protected bool WindowButtons = true;
        public bool IsTool = false;
        public string Caption = "";
        public string lastTooltip = "";
        public HMForm(UIController parent)
        {
            this.parent = parent;
        }

        public string Name
        {
            get { return name; }
        }

        protected void Init()
        {
            OnInit();
            IsInit = true;
        }
        abstract public void OnInit();

        public void OnGUI()
        {
            if (!IsInit) Init();
            //Color test = new Color(1, 1, 1);
            //test.a = 0.6f + 0.4f * Mathf.Sin(Time.time);
            //GUI.backgroundColor = test;

            windowRect0 = GUI.Window(id, windowRect0, DoWindow, name);
            
        }
        abstract public void OnUpdate();
        // Make the contents of the window.
        // The value of GUI.color is set to what it was when the window
        // was created in the code above.
        void DoWindow(int windowID)
        {
            if (WindowButtons)
            {
                if (GUI.Button(new Rect(windowRect0.width - 30, 0, 30, 20), "X"))
                {
                    SwitchShow();
                }
            }
            OnDoWindow();

            GUI.color = Color.white;
            if (Event.current.type == EventType.Repaint)
            {
                if (lastTooltip != "")
                {
                    DrawTooltip(GUI.tooltip);
                }

                lastTooltip = GUI.tooltip;
            }
            // Make the windows be draggable.
            GUI.DragWindow(new Rect(0, 0, 10000, 10000));

        }

        private void DrawTooltip(string tooltip)
        {
            //Debug.Log("Draw tooltip" + GUI.tooltip);
            GUI.backgroundColor = Color.red;
            if (tooltip.StartsWith("Action:"))
            {
                string label = tooltip.Substring(tooltip.IndexOf(':')+1);
                GUI.Label(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y + 20, 200, 200), label, HMStyle.TextTooltip);
            }
            else
            {
                GUI.Label(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y + 20, 200, 200), tooltip, HMStyle.TextTooltip);
            }
        }

        public void SwitchShow()
        {
            Show = !Show;
            if (!Show) OnClose();
        }

        abstract public void OnDoWindow();
        abstract public void OnClose();

    }
}
