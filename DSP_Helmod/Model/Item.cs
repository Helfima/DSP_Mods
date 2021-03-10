using DSP_Helmod.Classes;
using DSP_Helmod.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DSP_Helmod.Model
{
    public class Item : BaseItem, IItem
    {
        protected ItemProto proto;

        public Texture2D Icon
        {
            get { return proto.iconSprite.texture; }
        }

        public Item(int id)
        {
            this.id = id;
            this.proto = LDB.items.Select(id);
            if(this.proto != null)
            {
                this.name = proto.name;
            }
        }
        /// <summary>
        /// Use only for test
        /// </summary>
        /// <param name="name"></param>
        /// <param name="count"></param>
        public Item(string name, double count)
        {
            this.name = name;
            this.count = count;
        }
        public Item(string name, double count, ItemState state = ItemState.Normal)
        {
            this.name = name;
            this.count = count;
            this.state = state;
        }
        public Item(ItemProto proto, double count, ItemState state = ItemState.Normal)
        {
            this.id = proto.ID;
            this.proto = proto;
            this.name = proto.name;
            this.count = count;
            this.state = state;
        }
        public ItemProto Proto
        {
            get
            {
                if (proto == null) proto = LDB.items.Select(this.id);
                return proto;
            }
        }

        public List<IRecipe> Recipes
        {
            get
            {
                //HMLogger.Debug($"Try Item.Recipes");
                List<IRecipe> recipes = new List<IRecipe>();
                // probleme de cast sinon
                foreach(Recipe recipe in proto.recipes.Select(recipe => new Recipe(recipe, 1)).ToList())
                {
                    recipes.Add(recipe);
                }
                //HMLogger.Debug($"Item.Recipes:{recipes.Count}");
                return recipes;
            }
        }

        public double LogisticFlow
        {
            get
            {
                PrefabDesc prefabDesc = proto.prefabDesc;
                if (prefabDesc.isBelt)
                {
                    return prefabDesc.beltSpeed*6;
                }
                return 1;
            }
        }

        
        public bool Match(IItem other)
        {
            if (other == null || this.name == null) return false;
            return this.name.Equals(other.Name);
        }

        public IItem Clone(double factor = 1)
        {
            return new Item(proto, this.count * factor, this.state);
        }

    }
}
