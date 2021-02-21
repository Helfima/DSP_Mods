using DSP_Helmod.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DSP_Helmod.Math
{
    public class Compute
    {
        public static double EPSILON = 0.01;
        private Solver solver;
        private int Time = 1;

        public Compute()
        {
            this.solver = new Solver();
        }
        public void Update(Nodes nodes)
        {
            //Debug.Log($"Compute.Update({nodes.GetType()})");
            Time = nodes.Time;
            ComputeNode(nodes);
        }

        private void ComputeNode(Nodes nodes)
        {
            if (nodes == null || nodes.Children.Count == 0) return;
            //Debug.Log($"Children:{nodes.Children.Count}");
            foreach (Node node in nodes.Children)
            {
                ComputeNode(node);
            }
            Matrix matrix = GetMatrix(nodes);
            // prepare des objectifs manquants
            if(nodes.Objectives == null)
            {
                List<Item> items = nodes.Children.First().Products;
                nodes.Objectives = new MatrixValue[items.Count];
                for(int i = 0; i < items.Count; i++)
                {
                    Item item = items[i];
                    nodes.Objectives[i] = new MatrixValue(item.GetType().Name, item.Name, item.Count);
                }
            }
                        
            //Debug.Log(matrix.ToString());
            MatrixValue[] result = solver.Solve(matrix, nodes.Objectives);
            foreach (Node node in nodes.Children)
            {
                foreach (MatrixValue value in result)
                {
                    if (node.Match(value))
                    {
                        node.Count = value.Value;
                        break;
                    }
                }
            }
            ComputeFactory(nodes);
            ComputeInputOutput(nodes);
            //Debug.Log(solver.ToString());
        }

        private void ComputeFactory(Nodes nodes)
        {
            foreach (Node node in nodes.Children)
            {
                if(node is Recipe)
                {
                    Recipe recipe = (Recipe)node;
                    recipe.Factory.Count = recipe.Energy * recipe.Count / (recipe.Factory.Speed * Time);
                    //Debug.Log($"Factory.count (recipe.Name): recipe.Energy*recipe.Count/(recipe.Factory.Speed*Time)=recipe.Factory.Count");
                    //Debug.Log($"Factory.count ({recipe.Name}): {recipe.Energy}*{recipe.Count}/({recipe.Factory.Speed}*{Time}=={recipe.Factory.Count}");
                    node.Power = recipe.Factory.Count * recipe.Factory.Power;
                }
            }
        }

        private void ComputeInputOutput(Nodes nodes)
        {
            Dictionary<string, Item> products = new Dictionary<string, Item>();
            Dictionary<string, Item> ingredients = new Dictionary<string, Item>();
            foreach (Node node in nodes.Children)
            {
                foreach (Item item in node.Products)
                {
                    if (products.ContainsKey(item.Name))
                    {
                        products[item.Name].Count += node.Count * item.Count;
                    }
                    else
                    {
                        products.Add(item.Name, item.Clone(node.Count));
                    }
                }
                foreach (Item item in node.Ingredients)
                {
                    if (ingredients.ContainsKey(item.Name))
                    {
                        ingredients[item.Name].Count += node.Count * item.Count;
                    }
                    else
                    {
                        ingredients.Add(item.Name, item.Clone(node.Count));
                    }
                }
            }

            //consomme les produits
            foreach(KeyValuePair<string, Item> entry in ingredients)
            {
                if (products.ContainsKey(entry.Key))
                {
                    double productCount = products[entry.Key].Count;
                    double ingredientCount = entry.Value.Count;
                    products[entry.Key].Count = System.Math.Max(0, products[entry.Key].Count - ingredientCount);
                    ingredients[entry.Key].Count = System.Math.Max(0, ingredients[entry.Key].Count - productCount);
                }
            }
            nodes.Products = products.Select(entry => entry.Value).ToList();
            nodes.Ingredients = ingredients.Select(entry => entry.Value).ToList();
        }

        private void ComputeNode(Node node)
        {

        }

        private Matrix GetMatrix(Nodes nodes)
        {
            List<MatrixHeader> rowHeaders = new List<MatrixHeader>();
            List<MatrixRow> rowDatas = new List<MatrixRow>();

            List<string> products = new List<string>();
            List<string> ingredients = new List<string>();
            foreach (Node node in nodes.Children)
            {
                rowHeaders.Add(new MatrixHeader(node.GetType().Name, node.Name));
                MatrixRow rowData = new MatrixRow(node.GetType().Name, node.Name);
                foreach(Item item in node.Products)
                {
                    products.Add(item.Name);
                    rowData.AddValue(new MatrixValue(item.GetType().Name, item.Name, item.Count));
                }
                foreach (Item item in node.Ingredients)
                {
                    ingredients.Add(item.Name);
                    rowData.AddValue(new MatrixValue(item.GetType().Name, item.Name, -item.Count));
                }
                rowDatas.Add(rowData);
            }
            // update status
            foreach (Node node in nodes.Children)
            {
                foreach (Item item in node.Products)
                {
                    if (ingredients.Contains(item.Name))
                    {
                        item.State = Classes.ItemState.Residual;
                    }
                    else
                    {
                        item.State = Classes.ItemState.Main;
                    }
                }
                foreach (Item item in node.Ingredients)
                {
                    if (products.Contains(item.Name))
                    {
                        item.State = Classes.ItemState.Residual;
                    }
                    else
                    {
                        item.State = Classes.ItemState.Main;
                    }
                }
            }
            Matrix matrix = new Matrix(rowHeaders.ToArray(), rowDatas.ToArray());
            return matrix;
        }
    }
}
