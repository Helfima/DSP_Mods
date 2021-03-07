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
            this.proto = LDB.veins.Select(Id);
            UpdateItems();
        }

        public RecipeVein(VeinProto proto, double count = 0)
        {
            this.proto = proto;
            this.Count = count;
            UpdateItems();
        }

        public VeinProto Proto
        {
            get
            {
                if (proto == null)
                {
                    proto = LDB.veins.Select(Id);
                    UpdateItems();
                }
                return proto;
            }
        }
        public ItemProto ItemProto
        {
            get
            {
                return itemProto;
            }
        }

        public double Energy
        {
            get { return proto.MiningTime/60.0; }
        }

        public Factory Factory
        {
            get
            {
                if (factory == null)
                {
                    List<ItemProto> items = LDB.items.dataArray.Where(item => {
                        if (itemProto.IsFluid)
                        {
                            return item.prefabDesc.oilMiner;
                        }
                        return item.prefabDesc.veinMiner;
                        }).ToList();
                    factory = new Factory(items.First(), 1);
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
                List<ItemProto> items = LDB.items.dataArray.Where(item => {
                    if (itemProto.IsFluid)
                    {
                        return item.prefabDesc.oilMiner;
                    }
                    return item.prefabDesc.veinMiner;
                }).ToList();
                items.Sort(delegate (ItemProto item1, ItemProto item2)
                {
                    return item1.prefabDesc.assemblerSpeed.CompareTo(item2.prefabDesc.assemblerSpeed);
                });
                return items.Select(item => new Factory(item, 1)).ToList();
            }
        }

        private void UpdateItems()
        {
            this.Products = new List<IItem>();
            ItemProto item = LDB.items.Select(proto.MiningItem);
            this.itemProto = item;
            this.Products.Add(new Item(item, 1));
            this.Ingredients = new List<IItem>();
            this.Ingredients.Add(new ItemVein(proto, 1));
            this.Id = proto.ID;
            this.Name = proto.name;
            this.Type = GetType().Name;
            this.Icon = proto.iconSprite.texture;
        }

        public IRecipe Clone()
        {
            return new RecipeVein(proto, Count);
        }

        public IRecipe Clone(double value)
        {
            return new RecipeVein(proto, value);
        }

    }
}
