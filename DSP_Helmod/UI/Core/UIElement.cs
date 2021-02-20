using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DSP_Helmod.UI.Core
{
    public class UIElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private bool mouse_over = false;
        void Update()
        {
            if (mouse_over)
            {
                Debug.Log("Mouse Over");
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            mouse_over = true;
            Debug.Log("Mouse enter");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            mouse_over = false;
            Debug.Log("Mouse exit");
        }
    }
}
