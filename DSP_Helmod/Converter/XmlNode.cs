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
    [XmlRoot("Node")]
    public class XmlNode
    {
        [XmlAttribute("Id", typeof(int))]
        public int Id;

        [XmlAttribute("Type", typeof(string))]
        public string Type;

        [XmlAttribute("Name", typeof(string))]
        public string Name;

        [XmlAttribute("IsNodes", typeof(bool))]
        public bool IsNodes;

        [XmlElement("Inputs", typeof(XmlInput))]
        public List<XmlInput> Inputs = new List<XmlInput>();

        [XmlElement("Children", typeof(XmlNode))]
        public List<XmlNode> Children = new List<XmlNode>();
        public static XmlNode Parse(Node node)
        {
            XmlNode xmlNode = new XmlNode();
            xmlNode.Id = node.Id;
            xmlNode.Name = node.Name;
            xmlNode.Type = node.Type;
            xmlNode.IsNodes = node is Nodes;
            if (node is Nodes)
            {
                Nodes nodes = (Nodes)node;
                if (nodes.Children != null)
                {
                    foreach (Node childNode in nodes.Children)
                    {
                        xmlNode.Children.Add(XmlNode.Parse(childNode));
                    }
                }
                if (nodes.Inputs != null)
                {
                    foreach (MatrixValue input in nodes.Inputs)
                    {
                        xmlNode.Inputs.Add(XmlInput.Parse(input));
                    }
                }
            }
            return xmlNode;
        }

        public Node GetObject()
        {
            
            
            if (IsNodes)
            {
                Nodes nodes = new Nodes();
                if (Inputs != null)
                {
                    foreach (XmlInput xmlInput in Inputs)
                    {
                        nodes.SetInput(xmlInput.GetObject());
                    }
                }
                foreach (XmlNode xmlNode in Children)
                {
                    nodes.Add(xmlNode.GetObject());
                }
                return nodes;
            }
            else
            {
                switch (Type)
                {
                    case "Recipe":
                        Recipe recipe = new Recipe(Id);
                        return recipe;
                }
            }
            return null;
        }
    }
}
