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
        private List<INode> children = new List<INode>();
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
        
        public List<INode> Children
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

        public void Add(INode node)
        {
            if (node == null) return; // TODO exception
            if (node is IRecipe && children.Count == 0) node.Count = 1;
            children.Add(node);
            node.Parent = this;
            UpdateItems();
        }
        public void Remove(INode node)
        {
            children.Remove(node);
            foreach(IItem product in node.Products)
            {
                RemoveInput(product);
            }
            UpdateItems();
        }

        public void UpLevelNode(INode node)
        {
            int index = children.IndexOf(node);
            children.RemoveAt(index);
            Nodes newNodes = new Nodes();
            newNodes.Add(node);
            newNodes.Parent = this;
            if (children.Count == 0)
            {
                children.Add(newNodes);
            }
            else
            {
                children.Insert(index, newNodes);
            }
        }
        public void DownLevelNode(INode node)
        {
            if (Parent != null)
            {
                int index = children.IndexOf(node);
                children.RemoveAt(index);
                int afterIndex = Parent.children.IndexOf(this);
                Parent.children.Insert(afterIndex+1, node);
                node.Parent = Parent;
            }
        }
        public void AddObjective(IItem item, double value)
        {
            objectives = AddMatrixValue(objectives, item, value, true);
        }
        public void SetInput(IItem item, double value)
        {
            inputs = AddMatrixValue(inputs, item, value, false);
        }
        public void RemoveInput(IItem item)
        {
            if(inputs != null)
            {
                inputs = inputs.Where(element => element.Name != item.Name).ToArray();
            }
        }

        public double GetInputValue(IItem item)
        {
            if (inputs != null)
            {
                foreach (MatrixValue input in inputs)
                {
                    if (input.Name == item.Name) return input.Value;
                }
            }
            return item.Count;
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
            objectives = null;
            if (inputs != null && inputs.Length > 0)
            {
                foreach (MatrixValue input in inputs)
                {
                    objectives = AddMatrixValue(objectives, input, true);
                }
            }
        }

        internal MatrixValue[] AddMatrixValue(MatrixValue[] matrixValues, IItem item, double value, bool append = false)
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
                    matrixValues[matrixValues.Length-1] = newMatrixValue;
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
                List<IItem> products = new List<IItem>();
                List<IItem> ingredients = new List<IItem>();
                foreach (INode node in children)
                {
                    foreach (IItem item in node.Products)
                    {
                        products.Add(item);
                    }
                    foreach (IItem item in node.Ingredients)
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
