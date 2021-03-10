using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSP_Helmod.Model
{
    public interface IRecipe : INode
    {
        double Energy { get; }
        Factory Factory { get; set; }
        List<Factory> Factories { get; }
        IRecipe Clone(double count = 1);
    }
}
