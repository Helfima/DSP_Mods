using DSP_Helmod.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DSP_Helmod.Model
{
    public abstract class BaseItem
    {
        protected int id;
        protected string name;
        protected double count;
        protected ItemState state = ItemState.Normal;
        protected double flow;

        protected Texture2D icon;

        public string Type
        {
            get { return this.GetType().Name; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public double Count
        {
            get { return count; }
            set { count = value; }
        }
        public ItemState State
        {
            get { return state; }
            set { state = value; }
        }
        public double Flow { 
            get { return flow; }
            set { flow = value; }
        }


    }
}
