using DSP_Helmod.Classes;
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
            if (nodes == null) return;
            HMLogger.Debug($"Compute.Update({nodes.GetType()})");
            Time = nodes.Time;
            nodes.Count = 1;
            ComputeNode(nodes);
        }

        private void ComputeNode(Nodes nodes)
        {
            
            if (nodes == null || nodes.Children == null || nodes.Children.Count == 0) return;
            HMLogger.Debug($"Children:{nodes.Children.Count}");
            nodes.Objectives = null;
            nodes.CopyInputsToObjectives();
            foreach (INode node in nodes.Children)
            {
                if (node is Nodes)
                {
                    Nodes childNodes = (Nodes)node;
                    //childNodes.Count = 1;
                    //childNodes.Inputs = nodes.Inputs;
                    ComputeNode(childNodes);
                }
            }
            Matrix matrix = GetMatrix(nodes);
            // prepare des objectifs manquants
            if (nodes.Objectives == null)
            {
                List<IItem> items = nodes.Children.First().Products;
                nodes.Objectives = new MatrixValue[items.Count];
                for(int i = 0; i < items.Count; i++)
                {
                    IItem item = items[i];
                    nodes.Objectives[i] = new MatrixValue(item.GetType().Name, item.Name, item.Count);
                }
            }

            HMLogger.Debug(matrix.ToString());
            MatrixValue[] result = solver.Solve(matrix, nodes.Objectives);
            foreach (INode node in nodes.Children)
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
            ComputePower(nodes);
            ComputeInputOutput(nodes);
            HMLogger.Debug(solver.ToString());
        }

        private void ComputePower(Nodes nodes)
        {
            nodes.Power = 0;
            foreach (INode node in nodes.Children)
            {
                
                if(node is IRecipe)
                {
                    nodes.Power += node.Power;
                }
                else
                {
                    nodes.Power += node.Count * node.Power;
                }
            }
        }

        private void ComputeFactory(Nodes nodes)
        {
            foreach (INode node in nodes.Children)
            {
                if(node is IRecipe)
                {
                    IRecipe recipe = (IRecipe)node;
                    recipe.Factory.Count = recipe.Energy * recipe.Count / (recipe.Factory.Speed * Time);
                    HMLogger.Trace($"Factory.count (recipe.Name): recipe.Energy*recipe.Count/(recipe.Factory.Speed*Time)=recipe.Factory.Count");
                    HMLogger.Trace($"Factory.count ({recipe.Name}): {recipe.Energy}*{recipe.Count}/({recipe.Factory.Speed}*{Time}=={recipe.Factory.Count}");
                    node.Power = recipe.Factory.Count * recipe.Factory.Power;
                }
            }
        }

        private void ComputeInputOutput(Nodes nodes)
        {
            Dictionary<string, IItem> products = new Dictionary<string, IItem>();
            Dictionary<string, IItem> ingredients = new Dictionary<string, IItem>();
            foreach (INode node in nodes.Children)
            {
                foreach (IItem item in node.Products)
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
                foreach (IItem item in node.Ingredients)
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
            foreach(KeyValuePair<string, IItem> entry in ingredients)
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

        private Matrix GetMatrix(Nodes nodes)
        {
            List<MatrixHeader> rowHeaders = new List<MatrixHeader>();
            List<MatrixRow> rowDatas = new List<MatrixRow>();

            List<string> products = new List<string>();
            List<string> ingredients = new List<string>();
            foreach (INode node in nodes.Children)
            {
                rowHeaders.Add(new MatrixHeader(node.Type, node.Name));
                MatrixRow rowData = new MatrixRow(node.Type, node.Name);
                foreach(IItem item in node.Products)
                {
                    products.Add(item.Name);
                    rowData.AddValue(new MatrixValue(item.Type, item.Name, item.Count));
                }
                foreach (IItem item in node.Ingredients)
                {
                    ingredients.Add(item.Name);
                    rowData.AddValue(new MatrixValue(item.Type, item.Name, -item.Count));
                }
                rowDatas.Add(rowData);
            }
            // update status
            foreach (INode node in nodes.Children)
            {
                foreach (IItem item in node.Products)
                {
                    if (ingredients.Contains(item.Name))
                    {
                        item.State = ItemState.Residual;
                    }
                    else
                    {
                        item.State = ItemState.Main;
                    }
                }
                foreach (IItem item in node.Ingredients)
                {
                    if (products.Contains(item.Name))
                    {
                        item.State = ItemState.Residual;
                    }
                    else
                    {
                        item.State = ItemState.Main;
                    }
                }
            }
            Matrix matrix = new Matrix(rowHeaders.ToArray(), rowDatas.ToArray());
            return matrix;
        }

        /// <summary>
        /// Compute number of logistic item for product item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static IItem GetLogisticItem(IItem item)
        {
            int id = Settings.Instance.ItemIdLogistic;
            Item itemLogistic = Database.LogisticItems.FirstOrDefault(element => element.Id == id);
            if (itemLogistic == null) itemLogistic = Database.LogisticItems.First();
            IItem result = itemLogistic.Clone();
            result.Count = item.Flow / itemLogistic.LogisticFlow;
            return result;
        }
    }
}
