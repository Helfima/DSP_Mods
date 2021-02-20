using DSP_Helmod.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DSP_Helmod.Model
{
    public class Recipe:Node
    {
        private RecipeProto proto;
        private Factory factory;

        public Recipe(RecipeProto proto)
        {
            this.proto = proto;
            UpdateItems();
        }
        public RecipeProto Proto
        {
            get
            {
                if (proto == null)
                {
                    proto = LDB.recipes.Select(Id);
                    UpdateItems();
                }
                return proto;
            }
        }

        public Factory Factory
        {
            get
            { 
                if(factory == null)
                {
                    List<ItemProto> items = LDB.items.dataArray.Where(item => item.typeString.Equals(proto.madeFromString)).ToList();
                    items.Sort(delegate (ItemProto item1, ItemProto item2)
                    {
                        return item1.prefabDesc.assemblerSpeed.CompareTo(item2.prefabDesc.assemblerSpeed);
                    });
                    
                    factory = new Factory(items.Last(), 1);
                }
                return factory; 
            }
        }

        private void UpdateItems()
        {
            this.Products = RecipeProtoHelper.GetProductItems(proto);
            this.Ingredients = RecipeProtoHelper.GetIngredientItems(proto);
            this.Icon = proto.iconSprite.texture;
            this.Id = proto.ID;
            this.Name = proto.name;
            this.Type = GetType().Name;
        }

    }
}
