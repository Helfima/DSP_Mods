using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DSP_Helmod.Test
{
    [Serializable]
    [XmlRoot("Item")]
    public class Item
    {
        [XmlElement("Name", typeof(string))]
        public string Name;
        [XmlElement("Value", typeof(int))]
        public int Value;
    }
}
