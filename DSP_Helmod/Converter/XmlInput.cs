using DSP_Helmod.Math;
using DSP_Helmod.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DSP_Helmod.Converter
{
    [Serializable]
    [XmlRoot("Input")]
    public class XmlInput
    {
        [XmlAttribute("Type")]
        public string Type;

        [XmlAttribute("Name")]
        public string Name;

        [XmlAttribute("Value")]
        public double Value;

        public static XmlInput Parse(MatrixValue matrixValue)
        {
            XmlInput xmlNode = new XmlInput();
            xmlNode.Type = matrixValue.Type;
            xmlNode.Name = matrixValue.Name;
            xmlNode.Value = matrixValue.Value;
            return xmlNode;
        }

        public MatrixValue GetObject()
        {
            return new MatrixValue(Type, Name, Value);
        }
    }
}
