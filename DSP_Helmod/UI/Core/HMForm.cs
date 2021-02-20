﻿using UnityEngine;
using System.Collections;
using DSP_Helmod.Classes;
using System;

namespace DSP_Helmod.UI.Core
{
    abstract public class HMForm : MonoBehaviour
    {
        //public event EventHandler<HMEvent> HMEventHandler;

        protected UIController parent;
        protected Rect windowRect0 = new Rect(200, 20, 600, 300);
        public int id = 66600001;
        protected string name = "Test";

        protected bool IsInit = false;
        protected Vector2 scrollPosition;

        public bool Show = false;
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
            //GUI.color = Color.gray;
            windowRect0 = GUI.Window(id, windowRect0, DoWindow, name);
        }
        abstract public void OnUpdate();
        // Make the contents of the window.
        // The value of GUI.color is set to what it was when the window
        // was created in the code above.
        void DoWindow(int windowID)
        {
            if (GUI.Button(new Rect(windowRect0.width - 30, 0, 30, 20), "X"))
            {
                Show = !Show;
            }
            OnDoWindow();
            // Make the windows be draggable.
            GUI.DragWindow(new Rect(0, 0, 10000, 10000));
        }

        abstract public void OnDoWindow();

    }
}