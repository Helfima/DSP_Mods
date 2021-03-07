using DSP_Helmod.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DSP_Helmod.Model
{
    public interface IItem
    {
        int Id { get; set; }
        string Name { get; set; }
        double Count { get; set; }
        ItemState State { get; set; }
        double Flow { get; set; }
        Texture2D Icon { get; }
        string Type {get;}
        IItem Clone(double factor = 1);
    }
}
