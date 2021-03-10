using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSP_Helmod.Helpers
{
    public class VegeProtoHelper
    {
        public static string GetTootip(VegeProto item)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(item.name);
            //stringBuilder.AppendLine($"MiningTime: {item.MiningTime}");
            //stringBuilder.AppendLine($"CircleRadius: {item.CircleRadius}");
            return stringBuilder.ToString();
        }
    }
}
