using DSP_Helmod.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DSP_Helmod.Model
{
    public interface INode
    {
        int Id { get; set; }
        int Index { get; set; }
        string Name { get; set; }
        string Type { get; set; }
        double Count { get; set; }
        double Power { get; set; }
        List<IItem> Products { get; set; }
        List<IItem> Ingredients { get; set; }
        Effects Effects { get; }
        Texture2D Icon { get; set; }
        Nodes Parent { get; set; }
        bool Match(MatrixValue other);
        double GetItemDeepCount(bool deep);
        double GetDeepCount(bool deep);
    }
}
