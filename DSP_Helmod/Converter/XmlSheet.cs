using DSP_Helmod.Classes;
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
    [XmlRoot("Sheet")]
    public class XmlSheet
    {
        [XmlAttribute("Time", typeof(int))]
        public int Time;

        [XmlElement("Inputs", typeof(XmlInput))]
        public List<XmlInput> Inputs = new List<XmlInput>();

        [XmlElement("Children", typeof(XmlNode))]
        public List<XmlNode> Children = new List<XmlNode>();

        public static XmlSheet Parse(Nodes nodes)
        {
            XmlSheet xmlNode = new XmlSheet();
            xmlNode.Time = nodes.Time;
            if (nodes.Inputs != null)
            {
                foreach (MatrixValue input in nodes.Inputs)
                {
                    xmlNode.Inputs.Add(XmlInput.Parse(input));
                }
            }
            if (nodes.Children != null)
            {
                foreach (Node childNode in nodes.Children)
                {
                    xmlNode.Children.Add(XmlNode.Parse(childNode));
                }
            }
            return xmlNode;
        }

        public Nodes GetObject()
        {
            //HMLogger.Debug($"XmlSheet.Time:{Time}");
            //HMLogger.Debug($"XmlSheet.Inputs:{Inputs.Count}");
            //HMLogger.Debug($"XmlSheet.Children:{Children.Count}");
            Nodes nodes = new Nodes(Time);
            if (Inputs != null)
            {
                foreach (XmlInput xmlInput in Inputs)
                {
                    nodes.SetInput(xmlInput.GetObject());
                }
            }
            if (Children != null)
            {
                foreach (XmlNode xmlNode in Children)
                {
                    nodes.Add(xmlNode.GetObject());
                }
            }
            return nodes;
        }
    }
}
