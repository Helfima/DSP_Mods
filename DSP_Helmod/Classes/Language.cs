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
    public class Language
    {
        private static Dictionary<string, string> values;

        public static void Load()
        {
            Language.values = new Dictionary<string, string>();
            Language.values.Add("open.sheet", "Open Sheet");
            Language.values.Add("open.node", "Open Node");
            Language.values.Add("edition.recipe", "Edit Recipe");
        }

        public static string Get(string value)
        {
            if (!Language.values.ContainsKey(value)) return value;
            return Language.values[value];
        }
        
    }
}
