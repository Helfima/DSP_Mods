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

        public static List<Item> GetProductItems(RecipeProto recipe)
        {
            return GetItems(recipe.Results, recipe.ResultCounts);
        }

        public static List<Item> GetIngredientItems(RecipeProto recipe)
        {
            return GetItems(recipe.Items, recipe.ItemCounts);
        }

        internal static List<Item> GetItems(int[] itemIds, int[] counts)
        {
            List<Item> items = new List<Item>();
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
