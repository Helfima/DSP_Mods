using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DSP_Helmod.Classes
{
    public class LoadAssembly
    {
        public static void LoadXml(string resourceName)
        {
            Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            using (StreamReader reader = new StreamReader(assembly.GetManifestResourceStream(resourceName)))
            {
                var xml = reader.ReadToEnd();
                LoadXml(xml);
            }
        }

        public static string ReadEmbeddedRessourceString(string name)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Debug.Log($"Assembly:{assembly.Location}");
            Debug.Log($"Try load");
            foreach (string resource in assembly.GetManifestResourceNames())
            {
                Debug.Log($"Resource:{resource}");
            }
            var resourceName = assembly.GetManifestResourceNames().FirstOrDefault(x => x.Contains(name));
            using (StreamReader stream = new StreamReader(assembly.GetManifestResourceStream(resourceName)))
            {
                if (stream != null) return stream.ReadToEnd();
            }
            return null;
        }

 
        public static Texture2D ReadTexture2D(string name, int width = 64, int height = 64)
        {
            Debug.Log($"Try load");
            string image = ReadEmbeddedRessourceString(name);
            Debug.Log($"Loaded");
            if (image != null)
            {
                IntPtr sptr = Marshal.StringToHGlobalAnsi(image);
                Texture2D texture = new Texture2D(width, height);
                texture.LoadRawTextureData(sptr, image.Length);
                Marshal.FreeHGlobal(sptr);
                return texture;
            }
            return null;
        }
    }
}
