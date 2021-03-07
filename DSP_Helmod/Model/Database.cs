using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSP_Helmod.Model
{
    public class Database
    {
        private static List<Factory> factories = new List<Factory>();

        private static List<Item> logistics = new List<Item>();

        public static void Load()
        {
            foreach (ItemProto itemProto in LDB.items.dataArray)
            {
                switch (itemProto.Type)
                {
                    case EItemType.Production:
                        Database.factories.Add(new Factory(itemProto, 1));
                        break;
                    case EItemType.Logistics:
                        Database.logistics.Add(new Item(itemProto, 1));
                        break;
                }
            }
        }
        public static List<Item> LogisticItems
        {
            get { return Database.logistics.Where(item => item.Proto.prefabDesc.isBelt).ToList(); }
        }

        public static List<Factory> Factories
        {
            get { return Database.factories; }
        }
    }
}
