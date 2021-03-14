using DSP_Helmod.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DSP_Helmod.Model
{
    abstract public class Node: INode
    {
        protected Effects effects = new Effects();
        protected double product = 1;
        public int Id { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public double Count { get; set; }
        public double Power { get; set; }
        public double Production {
            get { return product; }
            set { product = value; }
        }
        public string ProductionString
        {
            get { 
                return $"{product * 100.0}";
            }
            set { 
                double percent = 100;
                double.TryParse(value, out percent);
                product = percent / 100.0;
            }
        }
        public Texture2D Icon { get; set; }
        public Nodes Parent { get; set; }
        protected List<IItem> products = new List<IItem>();
        protected List<IItem> ingredients = new List<IItem>();
        public Effects Effects
        {
            get { return effects; }
        }
        public List<IItem> Products
        {
            get { return products; }
            set { products = value; }
        }
        public List<IItem> Ingredients
        {
            get { return ingredients; }
            set { ingredients = value; }
        }
        public bool Match(MatrixValue other)
        {
            if (other == null || Type == null || Name == null) return false;
            return Type.Equals(other.Type) && Name.Equals(other.Name);
        }
        
        public double GetItemDeepCount(bool deep)
        {
            double count = Count;
            if(deep && Parent != null)
            {
                double total = Parent.GetDeepCount(deep);
                if (total == 0) total = 1;
                count = count * total;
            }
            return count;
        }

        public double GetDeepCount(bool deep)
        {
            if (!deep) return 1;
            double count = Count;
            if (Parent != null)
            {
                double total = Parent.GetDeepCount(deep);
                if (total == 0) total = 1;
                count = count * total;
            }
            return count;
        }
    }
}
