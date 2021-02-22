using DSP_Helmod.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DSP_Helmod.Model
{
    abstract public class Node
    {
        public int Id;
        public int Index;
        public string Name;
        public string Type;
        public double Count;
        public double Power;
        public List<Item> Products = new List<Item>();
        public List<Item> Ingredients = new List<Item>();
        public Texture2D Icon;
        public Nodes Parent;

        public bool Match(MatrixValue other)
        {
            Classes.HMLogger.Debug($"Test Node match {this.GetType()}: {Type}=={other.Type} && {Name}=={other.Name}");
            if (other == null || Type == null || Name == null) return false;
            return Type.Equals(other.Type) && Name.Equals(other.Name);
        }
        
    }
}
