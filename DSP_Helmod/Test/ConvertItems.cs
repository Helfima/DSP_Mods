using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityEngine;

namespace DSP_Helmod.Test
{
    public class ConvertItems
    {
        public static void Export()
        {
            Test1();
        }

        private static void Test1()
        {
            string fileName = "C:/Temp/export.json";
            Items items = new Items()
            {
                Name = "toto"
            };
            Item item1 = new Item()
            {
                Name = "titi",
                Value = 0
            };
            Item item2 = new Item()
            {
                Name = "tata",
                Value = 1
            };
            items.items = new Item[2] { item1, item2 };

            FileStream writer = new FileStream(fileName, FileMode.Create);
            XmlSerializer serializer = new XmlSerializer(typeof(Items));
            serializer.Serialize(writer, items);
            
        }
    }
}
