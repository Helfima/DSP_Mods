using DSP_Helmod.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DSP_Helmod.Helpers
{
    public static class RecipeProtoHelper
    {
        public static string GetTootip(RecipeProto recipe)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(recipe.name);
            stringBuilder.AppendLine(recipe.Type.ToString());
            stringBuilder.AppendLine($"Energy: {recipe.TimeSpend}");
            stringBuilder.AppendLine($"Made in: {recipe.madeFromString}");
            stringBuilder.AppendLine("Products:");
            for(int index = 0; index < recipe.ResultCounts.Length; index++)
            {
                int count = recipe.ResultCounts[index];
                int id = recipe.Results[index];
                ItemProto item = LDB.items.Select(id);
                stringBuilder.AppendLine($"{count}x{item.name}");
            }
            stringBuilder.AppendLine("Ingredients:");
            for (int index = 0; index < recipe.ItemCounts.Length; index++)
            {
                int count = recipe.ItemCounts[index];
                int id = recipe.Items[index];
                ItemProto item = LDB.items.Select(id);
                stringBuilder.AppendLine($"{count}x{item.name}");
            }
            return stringBuilder.ToString();
        }
        public static string GetTootip(IRecipe recipe)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(recipe.Name);
            //stringBuilder.AppendLine(recipe.Type.ToString());
            stringBuilder.AppendLine($"Energy: {recipe.Energy}");
            stringBuilder.AppendLine($"Made in: {recipe.Factory.Name}");
            stringBuilder.AppendLine("Products:");
            foreach (IItem item in recipe.Products)
            {
                stringBuilder.AppendLine($"{item.Count}x{item.Name}");
            }
            stringBuilder.AppendLine("Ingredients:");
            foreach (IItem item in recipe.Ingredients)
            {
                stringBuilder.AppendLine($"{item.Count}x{item.Name}");
            }
            return stringBuilder.ToString();
        }

        public static List<IItem> GetProductItems(RecipeProto recipe)
        {
            return GetItems(recipe.Results, recipe.ResultCounts);
        }

        public static List<IItem> GetIngredientItems(RecipeProto recipe)
        {
            return GetItems(recipe.Items, recipe.ItemCounts);
        }

        internal static List<IItem> GetItems(int[] itemIds, int[] counts)
        {
            List<IItem> items = new List<IItem>();
            for (int i = 0; i < counts.Length; i++)
            {
                double count = counts[i];
                int id = itemIds[i];
                ItemProto proto = LDB.items.Select(id);
                items.Add(new Item(proto, count));
            }
            return items;
        }
    }
}
