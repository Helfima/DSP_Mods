using DSP_Helmod.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DSP_Helmod.Model
{
    public class RecipeVein : Node, IRecipe
    {
        private VeinProto proto;
        private ItemProto itemProto;
        private Factory factory;

        public RecipeVein(int id)
        {
            this.Id = id;
            IRecipe recipe = Database.SelectRecipe<RecipeVein>(id);
            if (recipe is RecipeVein)
            {
                RecipeVein recipeVein = (RecipeVein)recipe;
                this.proto = recipeVein.Proto;
                this.factory = new Factory(recipeVein.Factory.Id);
                UpdateItems();
            }
        }

        public RecipeVein(VeinProto proto, double count = 0)
        {
            this.proto = proto;
            this.Count = count;
            UpdateItems();
            this.factory = (Factory)GetFactories().First().Clone();
        }

        public RecipeVein(VeinProto proto, Factory factory, double count = 0)
        {
            this.proto = proto;
            this.Count = count;
            this.factory = factory;
            UpdateItems();
        }

        public VeinProto Proto
        {
            get { return proto; }
        }
        public string MadeIn
        {
            get { return factory.TypeString; }
        }
        public int GridIndex
        {
            get { return 1; }
        }
        public ItemProto ItemProto
        {
            get { return itemProto; }
        }

        public double Energy
        {
            get { return proto.MiningTime/60.0; }
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
            return Database.FactoriesVein.Where(item => {
                if (itemProto.IsFluid)
                {
                    return item.IsOilMiner;
                }
                return item.IsVeinMiner;
            }).ToList();
        }
        private void UpdateItems()
        {
            this.Products.Clear();
            this.effects.Productivity = 1 / GameData.MiningCostRate;
            this.effects.Speed = 1 * GameData.MiningSpeedScale;
            ItemProto item = LDB.items.Select(proto.MiningItem);
            this.itemProto = item;
            this.Products.Add(new Item(item, 1));
            this.Ingredients.Clear();
            this.Ingredients.Add(new ItemVein(proto, 1));
            this.Id = proto.ID;
            this.Name = proto.name;
            this.Type = GetType().Name;
            this.Icon = proto.iconSprite.texture;
        }

        public IRecipe Clone(double count = 1)
        {
            return new RecipeVein(proto, new Factory(factory.Proto, factory.Count), count);
        }

    }
}
