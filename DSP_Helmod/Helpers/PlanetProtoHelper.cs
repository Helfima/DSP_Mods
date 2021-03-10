using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSP_Helmod.Helpers
{
    public static class PlanetProtoHelper
    {
        public static string GetTootip(PlanetData item)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(item.name);
            stringBuilder.AppendLine(item.typeString);
            if(item.gasItems != null && item.gasItems.Length > 0)
            {
                stringBuilder.Append("Gas:");
                bool first = false;
                foreach (int id in item.gasItems)
                {
                    if (first) stringBuilder.Append(",");
                    ItemProto gas = LDB.items.Select(id);
                    stringBuilder.Append(gas.name);
                    first = true;
                }
                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }

        
    }
}
