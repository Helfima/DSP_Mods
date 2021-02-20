﻿using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using DSP_Helmod.UI;
using DSP_Helmod.UI.Core;
using System.IO;
using System.Reflection;
using UnityEngine.EventSystems;

namespace DSP_Helmod
{
    [BepInPlugin("helfima.helmod.plugin", "DSP Helmod Plug-In", "1.0.0.0")]
    public class HelmodPlugin : BaseUnityPlugin
    {
        public static string PluginPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private UI.UIController uiController = new UI.UIController();

        // Awake is called once when both the game and the plug-in are loaded
        internal void Awake()
        {
            UnityEngine.Debug.Log(HelmodPlugin.PluginPath);
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
        void Save()
        {

        }
        
    }
}