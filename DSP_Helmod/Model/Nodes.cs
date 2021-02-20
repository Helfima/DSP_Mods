using DSP_Helmod.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DSP_Helmod.Model
{
    public class Nodes:Node
    {
        private List<Node> children = new List<Node>();
        MatrixValue[] objectives;
        public List<Node> Children
        {
            get { return children; }
        }

        public MatrixValue[] Objectives
        {
            get { return objectives; }
            set { objectives = value; }
        }

        public void Add(Node node)
        {
            if (node is Recipe && children.Count == 0) node.Count = 1;
            children.Add(node);
            UpdateItems();
        }

        public void Remove(Node node)
        {
            children.Remove(node);
            UpdateItems();
        }
        private void UpdateItems()
        {
            Icon = children.First().Icon;
            List<Item> products = new List<Item>();
            List<Item> ingredients = new List<Item>();
            foreach (Node node in children)
            {
                foreach (Item item in node.Products)
                {
                    products.Add(item);
                }
                foreach (Item item in node.Ingredients)
                {
                    ingredients.Add(item);
                }
            }
            Products = products.Distinct().ToList();
            Ingredients = ingredients.Distinct().ToList();
        }

    }
}
