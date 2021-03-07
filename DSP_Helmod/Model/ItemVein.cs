using DSP_Helmod.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DSP_Helmod.Model
{
    public class ItemVein : BaseItem, IItem
    {
        protected VeinProto proto;
        public Texture2D Icon
        {
            get { return proto.iconSprite.texture; }
        }
        public ItemVein(VeinProto proto, double count, ItemState state = ItemState.Normal)
        {
            this.Id = proto.ID;
            this.proto = proto;
            this.Name = proto.name;
            this.Count = count;
            this.State = state;
        }
        public IItem Clone(double factor = 1)
        {
            return new ItemVein(proto, this.count * factor, this.state);
        }

    }
}
