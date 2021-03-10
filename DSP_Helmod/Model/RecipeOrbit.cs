using DSP_Helmod.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DSP_Helmod.Model
{
    public class RecipeOrbit : Node, IRecipe
    {
        private ItemProto proto;
        private Factory factory;

        public RecipeOrbit(int id)
        {
            this.Id = id;
            IRecipe recipe = Database.SelectRecipe<RecipeOrbit>(id);
            if (recipe is RecipeOrbit)
            {
                RecipeOrbit recipeOrbit = (RecipeOrbit)recipe;
                this.proto = recipeOrbit.Proto;
                this.factory = new Factory(recipeOrbit.Factory.Id);
                UpdateItems();
            }
        }

        public RecipeOrbit(ItemProto proto, double count = 0)
        {
            this.proto = proto;
            this.Count = count;
            // 2105=Orbital Collector
            Factory factory = new Factory(2105);
            this.factory = factory;
            UpdateItems();
        }

        public RecipeOrbit(ItemProto proto, Factory factory, double count = 0)
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
        
        public double Energy
        {
            get { return 1.0; }
        }

        public Factory Factory
        {
            get
            {
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
            return new RecipeOrbit(proto, new Factory(factory.Proto, factory.Count), count);
        }

    }
}
