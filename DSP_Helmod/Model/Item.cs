using DSP_Helmod.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DSP_Helmod.Model
{
    public class Item
    {
        public int Id;
        public string Name;
        protected ItemProto proto;
        public double Count;
        public ItemState State = ItemState.Normal;

        /// <summary>
        /// Use only for test
        /// </summary>
        /// <param name="name"></param>
        /// <param name="count"></param>
        public Item(string name, double count)
        {
            this.Name = name;
            this.Count = count;
        }
        public Item(string name, double count, ItemState state = ItemState.Normal)
        {
            this.Name = name;
            this.Count = count;
            this.State = state;
        }
        public Item(ItemProto proto, double count, ItemState state = ItemState.Normal)
        {
            this.proto = proto;
            this.Name = proto.name;
            this.Count = count;
            this.State = state;
        }
        public ItemProto Proto
        {
            get
            {
                if (proto == null) proto = LDB.items.Select(Id);
                return proto;
            }
        }

        public Texture2D Icon
        {
            get
            {
                if (Proto != null) return proto.iconSprite.texture;
                return null;
            }
        }

        public List<Recipe> Recipes
        {
            get
            {
                return proto.recipes.Select(recipe => new Recipe(recipe, 1)).ToList();
            }
        }

        public string Type
        {
            get { return this.GetType().Name; }
        }
        public bool Match(Item other)
        {
            if (other == null || Name == null) return false;
            return Name.Equals(other.Name);
        }

        public Item Clone(double factor = 1)
        {
            return new Item(proto, Count * factor, State);
        }
    }
}
