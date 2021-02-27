using DSP_Helmod.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DSP_Helmod.UI.Gui
{
    public class HMTexture
    {
        public static Texture2D infoTexture = LoadAssembly.LoadTexture2D("info", 64, 64);
        public static Texture2D eclaireTexture = LoadAssembly.LoadTexture2D("eclair_x64", 64, 64);

        public static Texture2D black = LoadAssembly.LoadTexture2D("black_x64", 64, 64);

        public static Texture2D icon_blue = LoadAssembly.LoadTexture2D("icon_blue_x64", 64, 64);
        public static Texture2D icon_green = LoadAssembly.LoadTexture2D("icon_green_x64", 64, 64);
        public static Texture2D icon_orange = LoadAssembly.LoadTexture2D("icon_orange_x64", 64, 64);
        public static Texture2D icon_red = LoadAssembly.LoadTexture2D("icon_red_x64", 64, 64);
        public static Texture2D icon_yellow = LoadAssembly.LoadTexture2D("icon_yellow_x64", 64, 64);

        public static Texture2D time = LoadAssembly.LoadTexture2D("time_x64", 64, 64);
    }
}
