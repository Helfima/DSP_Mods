using BepInEx;
using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DSP_Helmod.Classes
{
    public class Settings
    {
        private ConfigFile configFile;
        public static Settings Instance = new Settings();

        private ConfigEntry<KeyCode> openCloseKeyCode;
        private ConfigEntry<bool> displayLogistic;
        private ConfigEntry<int> itemIdLogistic;
        private ConfigEntry<bool> displayTotal;
        private ConfigEntry<float> windowAlpha;

        public KeyCode OpenCloseKeyCode
        {
            get { return openCloseKeyCode.Value; }
        }

        public bool DisplayLogistic
        {
            get { return displayLogistic.Value; }
            set { displayLogistic.Value = value; }
        }
        public int ItemIdLogistic
        {
            get { return itemIdLogistic.Value; }
            set { itemIdLogistic.Value = value; }
        }
        public bool DisplayTotal
        {
            get { return displayTotal.Value; }
            set { displayTotal.Value = value; }
        }
        public float WindowAlpha
        {
            get { return windowAlpha.Value; }
            set { windowAlpha.Value = value; }
        }

        private Settings()
        {
            string filename = System.IO.Path.Combine(Paths.ConfigPath, "DSPHelmod/DSPHelmod.Settings.cfg");
            configFile = new ConfigFile(filename, true);
        }

        public void Init()
        {
            openCloseKeyCode = configFile.Bind<KeyCode>("Settings", "OpenClose", KeyCode.I, "Key to Open or Close the main panel.");
            displayLogistic = configFile.Bind<bool>("Settings", "DisplayLogistic", false, "Display with logistic in the main panel.");
            itemIdLogistic = configFile.Bind<int>("Settings", "ItemIdLogistic", -1, "Item id for logistic in the main panel.");
            displayTotal = configFile.Bind<bool>("Settings", "DisplayTotal", false, "Display total in the main panel.");
            windowAlpha = configFile.Bind<float>("Settings", "WindowAlpha", 1, "Alpha background of windows.");
            configFile.Save();
        }
        
    }
}
