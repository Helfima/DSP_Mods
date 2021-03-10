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
    [XmlRoot("Node")]
    public class XmlNode
    {
        [XmlAttribute("Id", typeof(int))]
        public int Id;

        [XmlAttribute("Type", typeof(string))]
        public string Type;

        [XmlAttribute("Name", typeof(string))]
        public string Name;

        [XmlAttribute("Factory", typeof(int))]
        public int Factory;

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
            if(node is Recipe)
            {
                Recipe recipe = (Recipe)node;
                xmlNode.Factory = recipe.Factory.Id;
            }
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
            try
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
                    IRecipe recipe = null;
                    switch (Type)
                    {
                        case "Recipe":
                            recipe = Database.SelectRecipe<Recipe>(Id);
                            break;
                        case "RecipeVein":
                            recipe = Database.SelectRecipe<RecipeVein>(Id);
                            break;
                        case "RecipeOrbit":
                            recipe = Database.SelectRecipe<RecipeOrbit>(Id);
                            break;
                        case "RecipeOcean":
                            recipe = Database.SelectRecipe<RecipeOcean>(Id);
                            break;
                        case "RecipeCustom":
                            recipe = Database.SelectRecipe<RecipeCustom>(Id);
                            break;
                    }
                    if (recipe != null && Factory > 0)
                    {
                        recipe.Factory = new Factory(Factory);
                    }
                    return (Node)recipe;
                }
            }
            catch(Exception e)
            {
                HMLogger.Error(e.Message);
            }
            return null;
        }
        public Node GetObject2()
        {
            try
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
                            if (Factory > 0)
                            {
                                recipe.Factory = new Factory(Factory);
                            }
                            return recipe;
                        case "RecipeVein":
                            RecipeVein recipeVein = new RecipeVein(Id);
                            if (Factory > 0)
                            {
                                recipeVein.Factory = new Factory(Factory);
                            }
                            return recipeVein;
                        case "RecipeOrbit":
                            RecipeOrbit recipeOrbit = new RecipeOrbit(Id);
                            if (Factory > 0)
                            {
                                recipeOrbit.Factory = new Factory(Factory);
                            }
                            return recipeOrbit;
                        case "RecipeOcean":
                            RecipeOcean recipeOcean = new RecipeOcean(Id);
                            if (Factory > 0)
                            {
                                recipeOcean.Factory = new Factory(Factory);
                            }
                            return recipeOcean;
                        case "RecipeCustom":
                            RecipeCustom recipeCustom = new RecipeCustom(Id);
                            if (Factory > 0)
                            {
                                recipeCustom.Factory = new Factory(Factory);
                            }
                            return recipeCustom;
                    }
                }
            }
            catch (Exception e)
            {
                HMLogger.Error(e.Message);
            }
            return null;
        }
    }
}
