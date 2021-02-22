using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DSP_Helmod.Test
{
    [XmlRoot("Items", IsNullable = false)]
    public class Items
    {
        [XmlElement("Name", typeof(string))]
        public string Name;

        [XmlElement("item", typeof(Item))]
        public Item[] items;
    }
}
