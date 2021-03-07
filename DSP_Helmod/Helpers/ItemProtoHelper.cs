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
            if (item.prefabDesc.isAssembler) stringBuilder.AppendLine($"assemblerSpeed: {item.prefabDesc.assemblerSpeed}");
            if (item.prefabDesc.isLab)
            {
                stringBuilder.AppendLine($"labAssembleSpeed: {item.prefabDesc.labAssembleSpeed}");
                stringBuilder.AppendLine($"labResearchSpeed: {item.prefabDesc.labResearchSpeed}");
            }
            if (item.prefabDesc.isBelt)
            {
                stringBuilder.AppendLine($"beltSpeed: {item.prefabDesc.beltSpeed}");
            }
#endif

            return stringBuilder.ToString();
        }

        
    }
}
