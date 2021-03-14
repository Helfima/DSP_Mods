using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSP_Helmod.Helpers
{
    public static class StarHelper
    {
        public static string GetTootip(StarData item)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(item.name);
            stringBuilder.AppendLine(item.typeString);
            return stringBuilder.ToString();
        }

        
    }
}
