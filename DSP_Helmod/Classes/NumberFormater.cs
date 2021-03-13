using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSP_Helmod.Classes
{
    public class NumberFormater
    {
        public static string Format(double num, string suffix = "")
        {
            if (num >= 1e10)
            {
                return (num / 1e9D).ToString("0.#G") + suffix;
            }
            if (num >= 1e9)
            {
                return (num / 1e9D).ToString("0.##G") + suffix;
            }
            if (num >= 1e7)
            {
                return (num / 1e6D).ToString("0.#M") + suffix;
            }
            if (num >= 1e6)
            {
                return (num / 1e6D).ToString("0.##M") + suffix;
            }
            if (num >= 1e4)
            {
                return (num / 1e3D).ToString("0.#k") + suffix;
            }
            if (num >= 1e3)
            {
                return (num / 1e3D).ToString("0.##k") + suffix;
            }

            return num.ToString("#,0") + suffix;
        }
    }
}
