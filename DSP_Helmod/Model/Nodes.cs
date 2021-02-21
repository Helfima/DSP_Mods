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
        private int time = 1;
        public int Time
        {
            get { return time; }
        }
        public int TimeSelected
        {
            get
            {
                switch (Time)
                {
                    case 1:
                        return 0;
                    case 60:
                        return 1;
                    case 3600:
                        return 2;
                    default:
                        return 0;
                }
            }
            set
            {
                switch (value)
                {
                    case 0:
                        time = 1;
                        break;
                    case 1:
                        time = 60;
                        break;
                    case 2:
                        time = 3600;
                        break;
                    default:
                        time = 1;
                        break;
                }
            }
        }
        
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

        public void SetObjective(Item item, double value)
        {
            if (objectives == null)
            {
                objectives = new MatrixValue[1];
                objectives[0] = new MatrixValue(item.GetType().Name, item.Name, value);
            }
            else
            {
                bool exist = false;
                foreach (MatrixValue matrixValue in objectives)
                {
                    if (matrixValue.Name.Equals(item.Name))
                    {
                        matrixValue.Value = value;
                        exist = true;
                    }
                }
                if (!exist)
                {
                    Array.Resize(ref objectives, objectives.Length + 1);
                    objectives[objectives.Length] = new MatrixValue(item.GetType().Name, item.Name, value);
                }
            }
        }
        public void Remove(Node node)
        {
            children.Remove(node);
            UpdateItems();
        }
        private void UpdateItems()
        {
            if(children.Count > 0)
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
            else
            {
                Icon = null;
                Products.Clear();
                Ingredients.Clear();
            }
            
        }

    }
}
