using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using DSPHelmod.UI;
using DSPHelmod.UI.Core;

namespace DSPHelmod
{
    [BepInPlugin("helfima.helmod.plugin", "DSP Helmod Plug-In", "1.0.0.0")]
    public class ExamplePlugin : BaseUnityPlugin
    {
        private UI.UIController uiController = new UI.UIController();

        // Awake is called once when both the game and the plug-in are loaded
        internal void Awake()
        {
            //UnityEngine.Debug.Log("Helmod!");
            var harmony = new Harmony("helfima.helmod.plugin");
            harmony.PatchAll();
        }

        void Update()
        {
            uiController.Update();
        }

        void OnGUI()
        {
            uiController.OnGUI();
        }

    }
}
