using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSP_Helmod.Model
{
    public class Database
    {
        private static List<ItemProto> factories;

        private static void Load()
        {
            foreach (ItemProto itemProto in LDB.items.dataArray)
            {
                switch (itemProto.Type)
                {
                    case EItemType.Production:
                        factories.Add(itemProto);
                        break;
                }
            }
        }
    }
}
