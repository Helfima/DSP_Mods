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
        public static List<int> Ids = new List<int>() { 1121, 2206, 2207 };
        private ItemProto proto;
        private Factory factory;

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
                case 1121:
                    Fractionator();
                    break;
                case 2206:
                    EnergyExchangerDischarge();
                    break;
                case 2207:
                    EnergyExchangerCharge();
                    break;
            }
        }
        /// <summary>
        /// Fractionator(2314): 100 Hydrogen(1120) => 99 Hydrogen(1120) + 1 Deuterium(1121)
        /// </summary>
        internal void Fractionator()
        {
            ItemProto fractionator = LDB.items.Select(2314);
            ItemProto deuterium = LDB.items.Select(1121);
            ItemProto hydrogen = LDB.items.Select(1120);
            this.proto = deuterium;
            this.factory = new Factory(fractionator, 1);
            this.Products.Clear();
            this.Products.Add(new Item(hydrogen, 99));
            this.Products.Add(new Item(deuterium, 1));
            this.Ingredients.Clear();
            this.Ingredients.Add(new Item(hydrogen, 100));
            this.Name = proto.name;
            this.Type = GetType().Name;
            this.Icon = proto.iconSprite.texture;
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
