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
        private static byte[] ReadStream(Stream stream)
        {
            int num3;
            if (stream.CanSeek)
            {
                int num = (int)stream.Length;
                byte[] array = new byte[num];
                int num2 = 0;
                while ((num3 = stream.Read(array, num2, num - num2)) > 0)
                {
                    num2 += num3;
                }
                return array;
            }
            byte[] array2 = new byte[1024*8];
            MemoryStream memoryStream = new MemoryStream();
            while ((num3 = stream.Read(array2, 0, array2.Length)) > 0)
            {
                memoryStream.Write(array2, 0, num3);
            }
            return memoryStream.ToArray();
        }
        public static byte[] ReadEmbeddedRessourceBytes(string name)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            //Debug.Log($"Assembly:{assembly.Location}");
            //Debug.Log($"Try load");
            var resourceName = assembly.GetManifestResourceNames().FirstOrDefault(x => x.ToLower().Contains(name.ToLower()));
            byte[] data = null;
            using (StreamReader stream = new StreamReader(assembly.GetManifestResourceStream(resourceName)))
            {
                if (stream != null) {
                    data = ReadStream(stream.BaseStream);
                }
            }
            return data;
        }

        
        public static string ReadEmbeddedRessourceString(string name)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            
            var resourceName = assembly.GetManifestResourceNames().FirstOrDefault(x => x.Contains(name));
            using (StreamReader stream = new StreamReader(assembly.GetManifestResourceStream(resourceName)))
            {
                if (stream != null) return stream.ReadToEnd();
            }
            return null;
        }


        public static Texture2D LoadTexture2D(string name, int width = 64, int height = 64)
        {
            //Debug.Log($"Try load image");
            byte[] image = ReadEmbeddedRessourceBytes(name);
            //Debug.Log($"Loaded: {image.Length}");
            if (image != null)
            {
                Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
                texture.LoadImage(image);
                texture.Apply();
                return texture;
            }
            return null;
        }
    }
}
