using DSP_Helmod.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DSP_Helmod.Model
{
    public class RecipeCustom : Node, IRecipe
    {
        public static List<int> Ids = new List<int>() { 2206, 2207 };
        private ItemProto proto;
        private Factory factory;
        private double energy = 1.0;

        public RecipeCustom(int id)
        {
            this.Id = id;
            UpdateItems();
        }
        public RecipeCustom(int id, double count = 0)
        {
            this.Id = id;
            this.Count = count;
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
            get { return energy; }
        }

        public Factory Factory
        {
            get { return factory; }
            set { factory = value; }
        }

        public List<Factory> Factories
        {
            get { return new List<Factory>() { factory }; }
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
            switch (this.Id)
            {
                case 2206:
                    EnergyExchangerDischarge();
                    break;
                case 2207:
                    EnergyExchangerCharge();
                    break;
            }
        }
        /// <summary>
        /// Energy Exchanger(2209): 1 Accumulator Full(2207) => Accumulator(2206)
        /// </summary>
        internal void EnergyExchangerDischarge()
        {
            ItemProto energyExchanger = LDB.items.Select(2209);
            ItemProto accumulator = LDB.items.Select(2206);
            ItemProto accumulatorFull = LDB.items.Select(2207);
            this.proto = accumulatorFull;
            this.factory = new Factory(energyExchanger, 1);
            this.Products.Clear();
            this.Products.Add(new Item(accumulator, 1));
            this.Ingredients.Clear();
            this.Ingredients.Add(new Item(accumulatorFull, 1));
            this.Name = proto.name;
            this.Type = GetType().Name;
            this.Icon = proto.iconSprite.texture;
        }
        /// <summary>
        /// Energy Exchanger(2209): 1 Accumulator(2206) => Accumulator Full(2207)
        /// </summary>
        internal void EnergyExchangerCharge()
        {
            ItemProto energyExchanger = LDB.items.Select(2209);
            ItemProto accumulator = LDB.items.Select(2206);
            ItemProto accumulatorFull = LDB.items.Select(2207);
            this.proto = accumulator;
            this.factory = new Factory(energyExchanger, 1);
            this.Products.Clear();
            this.Products.Add(new Item(accumulatorFull, 1));
            this.Ingredients.Clear();
            this.Ingredients.Add(new Item(accumulator, 1));
            this.Name = proto.name;
            this.Type = GetType().Name;
            this.Icon = proto.iconSprite.texture;
        }

        public IRecipe Clone(double count = 1)
        {
            return new RecipeCustom(this.Id, count);
        }

    }
}
