using DSP_Helmod.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DSP_Helmod.Model
{
    public class Recipe : Node, IRecipe
    {
        private RecipeProto proto;
        private Factory factory;
        public Recipe(int id)
        {
            this.Id = id;
            IRecipe irecipe = Database.SelectRecipe<Recipe>(id);
            if (irecipe is Recipe)
            {
                Recipe recipe = (Recipe)irecipe;
                this.proto = recipe.Proto;
                this.factory = new Factory(recipe.Factory.Id);
                UpdateItems();
            }
        }

        public Recipe(RecipeProto proto, double count = 0)
        {
            this.proto = proto;
            this.Count = count;
            List<Factory> factories = GetFactories();
            this.factory = (Factory)factories.First()?.Clone();
            UpdateItems();
        }
        public Recipe(RecipeProto proto, Factory factory, double count = 0)
        {
            this.proto = proto;
            this.Count = count;
            this.factory = factory;
            UpdateItems();
        }

        public RecipeProto Proto
        {
            get { return proto; }
        }
        public string MadeIn
        {
            get { return proto.madeFromString; }
        }
        public double Energy
        {
            get { return this.proto.TimeSpend/60.0; }
        }
        public int GridIndex
        {
            get { return this.proto.GridIndex; }
        }
        public Factory Factory
        {
            get { return factory; }
            set { factory = value; }
        }

        public List<Factory> Factories
        {
            get { return GetFactories(); }
        }

        internal List<Factory> GetFactories()
        {
            return Database.Factories.Where(item => item.TypeString.Equals(proto.madeFromString)).ToList();
        }

        private void UpdateItems()
        {
            this.Products = RecipeProtoHelper.GetProductItems(proto);
            this.Ingredients = RecipeProtoHelper.GetIngredientItems(proto);
            this.Id = proto.ID;
            this.Name = proto.name;
            this.Type = GetType().Name;
            this.Icon = proto.iconSprite.texture;
        }

        public IRecipe Clone(double count = 1)
        {
            return new Recipe(proto, new Factory(factory.Proto, factory.Count), count);
        }

    }
}
