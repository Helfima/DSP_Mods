using DSP_Helmod.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DSP_Helmod.Model
{
    public class RecipeOcean : Node, IRecipe
    {
        private ItemProto proto;
        private Factory factory;

        public RecipeOcean(int id)
        {
            this.Id = id;
            IRecipe recipe = Database.SelectRecipe<RecipeOcean>(id);
            if (recipe is RecipeOcean)
            {
                RecipeOcean recipeOcean = (RecipeOcean)recipe;
                this.proto = recipeOcean.Proto;
                this.factory = new Factory(recipeOcean.Factory.Id);
                UpdateItems();
            }

        }

        public RecipeOcean(ItemProto proto, double count = 0)
        {
            this.proto = proto;
            this.Count = count;
            // 2306=Water Pump
            Factory factory = new Factory(2306);
            this.factory = factory;
            UpdateItems();
        }
        public RecipeOcean(ItemProto proto, Factory factory, double count = 0)
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
            get
            {
                if (factory == null)
                {
                    factory = Database.FactoriesOrbiter.First();
                }
                return factory; 
            }
            set
            {
                factory = value;
            }
        }

        public List<Factory> Factories
        {
            get
            {
                return Database.FactoriesOrbiter;
            }
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
            return new RecipeOcean(proto, new Factory(factory.Proto, factory.Count), count);
        }

    }
}
