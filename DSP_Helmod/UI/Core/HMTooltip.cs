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
        protected UIController parent;
        protected Rect windowRect0 = new Rect(200, 20, 600, 300);
        protected bool IsInit = false;
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

        public void OnGUI()
        {
            if (!IsInit) Init();
            if (Event.current.type == EventType.Repaint)
            {
                GUI.backgroundColor = Color.red;
                GUILayout.BeginArea(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y + 50, 200, 200));
                OnDoWindow(parent.Tooltip);
                GUILayout.EndArea();
            }
            
        }

        abstract public void OnDoWindow(string tooltip);
    }
}
