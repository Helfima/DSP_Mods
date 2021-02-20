using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSP_Helmod.Model
{
    public class Factory : Item
    {
        public Factory(string name, double count): base(name, count)
        {

        }
        public Factory(ItemProto proto, double count) : base(proto, count)
        {

        }
    }
}
