using DSP_Helmod.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSP_Helmod.Model
{
    public class ModelBuilder
    {
        public static void ApplyFactoryOnAll(IRecipe recipe)
        {
            INode root = GetRootNode(recipe);
            ApplyFactoryOnAll(root as Nodes, recipe);
            Compute compute = new Compute();
            compute.Update(root as Nodes);
        }

        public static void ApplyFactoryOnAll(Nodes nodes, IRecipe recipe)
        {
            if (nodes == null) return;
            foreach (Node node in nodes.Children)
            {
                if(node is Nodes)
                {
                    ApplyFactoryOnAll(node as Nodes, recipe);
                }
                if(node is IRecipe)
                {
                    IRecipe currentRecipe = node as IRecipe;
                    if (currentRecipe.MadeIn == recipe.MadeIn)
                    {
                        currentRecipe.Factory = recipe.Factory.Clone() as Factory;
                    }
                }
            }
        }
        
        public static INode GetRootNode(INode node)
        {
            if (node.Parent == null) return node;
            return GetRootNode(node.Parent);
        }
    }
}
