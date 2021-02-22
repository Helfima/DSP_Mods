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
        MatrixValue[] inputs;
        private int time;

        public Nodes()
        {
            this.time = 1;
        }

        public Nodes(int time)
        {
            this.time = time;
        }
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

        public MatrixValue[] Inputs
        {
            get { return inputs; }
            set { inputs = value; }
        }

        public void Add(Node node)
        {
            if (node == null) return; // TODO exception
            if (node is Recipe && children.Count == 0) node.Count = 1;
            children.Add(node);
            node.Parent = this;
            UpdateItems();
        }
        public void Remove(Node node)
        {
            children.Remove(node);
            UpdateItems();
        }

        public void UpLevelNode(Node node)
        {
            int index = children.IndexOf(node);
            children.RemoveAt(index);
            Nodes newNodes = new Nodes();
            newNodes.Add(node);
            children.Insert(index, newNodes);
        }
        public void AddObjective(Item item, double value)
        {
            objectives = AddMatrixValue(objectives, item, value, true);
        }
        public void SetInput(Item item, double value)
        {
            inputs = AddMatrixValue(inputs, item, value, false);
        }

        public void SetInput(MatrixValue matrixValue)
        {
            inputs = AddMatrixValue(inputs, matrixValue, false);
        }
        /// <summary>
        /// Copy Input after Reset objectives
        /// </summary>
        public void CopyInputsToObjectives()
        {
            Classes.HMLogger.Debug($"CopyInputsToObjectives:{inputs != null}");
            objectives = null;
            if (inputs != null)
            {
                foreach (MatrixValue input in inputs)
                {
                    Classes.HMLogger.Debug($"Copy input:{input}");
                    objectives = AddMatrixValue(objectives, input, true);
                }
            }
        }

        internal MatrixValue[] AddMatrixValue(MatrixValue[] matrixValues, Item item, double value, bool append = false)
        {
            MatrixValue matrixValue = new MatrixValue(item.GetType().Name, item.Name, value);
            return AddMatrixValue(matrixValues, matrixValue, append);
        }
        internal MatrixValue[] AddMatrixValue(MatrixValue[] matrixValues, MatrixValue newMatrixValue, bool append = false)
        {
            if (matrixValues == null)
            {
                matrixValues = new MatrixValue[1];
                matrixValues[0] = newMatrixValue;
            }
            else
            {
                bool exist = false;
                foreach (MatrixValue matrixValue in matrixValues)
                {
                    if (matrixValue.Name.Equals(newMatrixValue.Name))
                    {
                        if (append) matrixValue.Value += newMatrixValue.Value;
                        else matrixValue.Value = newMatrixValue.Value;
                        exist = true;
                    }
                }
                if (!exist)
                {
                    Array.Resize(ref matrixValues, matrixValues.Length + 1);
                    matrixValues[matrixValues.Length] = newMatrixValue;
                }
            }
            return matrixValues;
        }
        private void UpdateItems()
        {
            if(children.Count > 0)
            {
                Icon = children.First().Icon;
                Name = children.First().Name;
                Type = children.First().Type;
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
