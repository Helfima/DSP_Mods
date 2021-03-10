using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DSP_Helmod.Classes
{
    [Serializable]
    public class PluginInfo
    {
        private static PluginInfo instance;
        public string name;
        public string version_number;
        public string website_url;
        public string description;
        public string[] dependencies;

        public static PluginInfo Instance
        {
            get
            {
                if (instance == null) instance = Load();
                return instance;
            }
        }

        public static PluginInfo Load()
        {
            string json = LoadAssembly.ReadEmbeddedRessourceString("manifest");
            PluginInfo pluginInfo = JsonUtility.FromJson<PluginInfo>(json);
            return pluginInfo;
        }

    }
}
