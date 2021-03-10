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
        protected SelectorMode selectorMode;

        protected Rect windowRect0 = new Rect(200, 20, 600, 300);
        public int id = 66600001;
        protected new string name = "DSP Helmod";
        protected Color BackgroundColor = Color.white;

        protected bool IsInit = false;
        protected Vector2 scrollPosition;

        public bool Show = false;
        protected bool WindowButtons = true;
        protected bool AlphaButtons = false;
        public bool InMain = false;
        public bool IsTool = false;
        public bool IsPersistant = false;
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

        public Rect Position
        {
            set { windowRect0 = value; }
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
            AutoResize(1920, 1200);
            // change alpha
            GUI.backgroundColor = new Color(1, 1, 1, Settings.Instance.WindowAlpha);
            // build window
            windowRect0 = GUI.Window(id, windowRect0, DoWindow, name, HMStyle.Form);
            CheckInputInRect(windowRect0);
        }

        private void AutoResize(int screenWidth, int screenHeight, float factor = 1f)
        {
            Vector2 resizeRatio = new Vector2((float)Screen.width / screenWidth, (float)Screen.height / screenHeight);
            GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(resizeRatio.x * factor, resizeRatio.y * factor, 1.0f));
        }

        abstract public void OnUpdate();
        // Make the contents of the window.
        // The value of GUI.color is set to what it was when the window
        // was created in the code above.
        void DoWindow(int windowID)
        {
            if (AlphaButtons)
            {
                float newAlpha = GUI.HorizontalSlider(new Rect(windowRect0.width - 200, 3, 60, 20), Settings.Instance.WindowAlpha, 0, 1);
                if(newAlpha != Settings.Instance.WindowAlpha)
                {
                    Debug.Log($"New alpha:{newAlpha}");
                    Settings.Instance.WindowAlpha = newAlpha;
                }
            }
            if (WindowButtons)
            {
                if (GUI.Button(new Rect(windowRect0.width - 90, 0, 30, 20), "*"))
                {
                    HMEvent.SendEvent(this, new HMEvent(HMEventType.OpenClose, typeof(EditionPreference)));
                }
                if (GUI.Button(new Rect(windowRect0.width - 60, 0, 30, 20), "?"))
                {
                    HMEvent.SendEvent(this, new HMEvent(HMEventType.OpenClose, typeof(AboutPanel)));
                }
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
                    //parent.Tooltip = GUI.tooltip;
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
                GUI.Label(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y + 20, 100, 25), label, HMStyle.TextTooltip);
            }
            else
            {
                string[] split = tooltip.Split('\n');
                GUI.Box(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y + 20, 200, 15 * (1 + split.Length)), tooltip, HMStyle.TextTooltip);
            }
        }
        private void DrawTooltip2(string tooltip)
        {
            if (tooltip == null || tooltip == "") return;
            //Debug.Log("Draw tooltip" + GUI.tooltip);
            GUI.backgroundColor = Color.red;
            GUILayout.BeginArea(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y + 50, 200, 200));
            if (tooltip.StartsWith("Action:"))
            {
                string label = tooltip.Substring(tooltip.IndexOf(':') + 1);
                GUILayout.Label(label, HMStyle.TextTooltip);
            }
            else
            {
                GUILayout.Label(tooltip, HMStyle.TextTooltip);
            }
            GUILayout.EndArea();
        }

        public void MoveMousePosition()
        {
            this.windowRect0 = new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, windowRect0.width, windowRect0.height);
        }

        public void SwitchShow(SelectorMode selectorMode = SelectorMode.Normal)
        {
             // si show va fermer ensuite d'ou OnClose
            this.selectorMode = selectorMode;
            if (Show) OnClose();
            Show = !Show;
        }

        public void Close()
        {
            if (Show)
            {
                Show = false;
                OnClose();
            }
        }

        abstract public void OnDoWindow();
        abstract public void OnClose();

        private void CheckInputInRect(Rect area)
        {
            if (area.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)))
            {
                var isMouseInput = Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.mouseScrollDelta.y != 0;

                if (!isMouseInput)
                {
                    return;
                }
                else
                {
                    Input.ResetInputAxes();
                }

            }
        }
    }
}
