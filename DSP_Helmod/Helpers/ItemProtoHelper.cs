using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSP_Helmod.Helpers
{
    public static class ItemProtoHelper
    {
        public static string GetTootip(ItemProto item)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(item.name);
            stringBuilder.AppendLine(item.typeString);
            //stringBuilder.AppendLine(item.description);
#if DEBUG
            stringBuilder.AppendLine("----------DEBUG------------");
            stringBuilder.AppendLine($"IsEntity: {item.IsEntity}");
            stringBuilder.AppendLine($"IsFluid: {item.IsFluid}");
            stringBuilder.AppendLine($"isRaw: {item.isRaw}");
            stringBuilder.AppendLine($"CanBuild: {item.CanBuild}");
            stringBuilder.AppendLine($"miningFrom: {item.miningFrom}");
            stringBuilder.AppendLine($"ProduceFrom: {item.ProduceFrom}");
#endif

            return stringBuilder.ToString();
        }
    }
}
