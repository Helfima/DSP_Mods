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
        private ItemProto proto;
        public double Count;

        public Item(ItemProto proto, double count)
        {
            this.proto = proto;
            this.Count = count;
        }
        public ItemProto Proto
        {
            get
            {
                if (proto == null) proto = LDB.items.Select(Id);
                return proto;
            }
        }

        public new Texture2D Icon
        {
            get
            {
                if (Proto != null) return proto.iconSprite.texture;
                return null;
            }
        }
    }
}
