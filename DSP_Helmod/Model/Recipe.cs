using DSPHelmod.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DSP_Helmod.Model
{
    public class Recipe:Node
    {
        private RecipeProto proto;
        private Factory factory;

        public Recipe(RecipeProto proto)
        {
            this.proto = proto;
            UpdateItems();
        }
        public RecipeProto Proto
        {
            get
            {
                if (proto == null)
                {
                    proto = LDB.recipes.Select(Id);
                    UpdateItems();
                }
                return proto;
            }
        }

        public Factory Factory
        {
            get { return factory; }
        }

        private void UpdateItems()
        {
            this.Products = RecipeProtoHelper.GetProductItems(proto);
            this.Ingredients = RecipeProtoHelper.GetIngredientItems(proto);
            this.Icon = proto.iconSprite.texture;
            this.Id = proto.ID;
        }

    }
}
