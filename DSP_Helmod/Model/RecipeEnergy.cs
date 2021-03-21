using DSP_Helmod.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DSP_Helmod.Model
{
    public class RecipeEnergy : Node, IRecipe
    {
        private ItemProto proto;
        private Factory factory;

        public RecipeEnergy(int id)
        {
            this.Id = id;
            IRecipe recipe = Database.SelectRecipe<RecipeCustom>(id);
            if (recipe is RecipeCustom)
            {
                RecipeCustom recipeCustom = (RecipeCustom)recipe;
                this.proto = recipeCustom.Proto;
                this.factory = new Factory(recipeCustom.Factory.Id);
                UpdateItems();
            }
        }

        public RecipeEnergy(ItemProto proto, Factory factory, double count = 0)
        {
            this.proto = proto;
            this.Count = count;
            this.factory = factory;
            UpdateItems();
        }

        public ItemProto Proto
        {
            get { return proto;}
        }
        public string MadeIn
        {
            get { return factory.TypeString; }
        }
        public int GridIndex
        {
            get { return this.proto.GridIndex; }
        }
        public double Energy
        {
            get { return 1.0; }
        }

        public Factory Factory
        {
            get { return factory; }
            set { factory = value; }
        }

        public List<Factory> Factories
        {
            get { return Database.FactoriesOrbiter; }
        }

        public new List<IItem> Products
        {
            get { return products; }
            set { products = value; }
        }
        public new List<IItem> Ingredients
        {
            get { return ingredients; }
            set { ingredients = value; }
        }

        private void UpdateItems()
        {
            this.Products.Clear();
            this.Products.Add(new Item(proto, 1));
            this.Ingredients.Clear();
            this.Id = proto.ID;
            this.Name = proto.name;
            this.Type = GetType().Name;
            this.Icon = proto.iconSprite.texture;
        }

        public IRecipe Clone(double count = 1)
        {
            return new RecipeEnergy(proto, new Factory(factory.Proto, factory.Count), count);
        }

    }
}
