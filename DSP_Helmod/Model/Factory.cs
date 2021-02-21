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

        public double Speed
        {
            get { return GetSpeed(); }
        }

        public double Power
        {
            get { return GetPower(); }
        }
        internal double GetSpeed()
        {
            if (proto == null || proto.prefabDesc == null) return 1;
            if (proto.prefabDesc.isAssembler) return ((double)proto.prefabDesc.assemblerSpeed) / 10000;
            if (proto.prefabDesc.isLab) return proto.prefabDesc.labAssembleSpeed;
            return 1;
        }

        internal double GetPower()
        {
            if (proto == null || proto.prefabDesc == null) return 0;
            if (proto.prefabDesc.isPowerConsumer) return proto.prefabDesc.workEnergyPerTick * 60;
            return 0;
        }
    }
}
