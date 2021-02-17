using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DSP_Helmod.Model
{
    public class Node
    {
        public int Id;
        public string Name;
        public double Count;
        public List<Item> Products;
        public List<Item> Ingredients;
        public Texture2D Icon;

        private List<Node> children = new List<Node>();

        public List<Node> Children
        {
            get { return children; }
        }

    }
}
