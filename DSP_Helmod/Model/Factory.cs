using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSP_Helmod.Model
{
    public class Factory : Item
    {
        public Factory(int id) : base(id)
        {

        }
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

        public string TypeString
        {
            get { return proto.typeString; }
        }
        public bool IsOilMiner
        {
            get { return proto.prefabDesc.oilMiner; }
        }
        public bool IsVeinMiner
        {
            get { return proto.prefabDesc.veinMiner; }
        }
        internal double GetSpeed()
        {
            if (proto == null || proto.prefabDesc == null) return 1;
            if (proto.prefabDesc.isAssembler) return ((double)proto.prefabDesc.assemblerSpeed) / 10000;
            if (proto.prefabDesc.isLab) return ((double)proto.prefabDesc.labAssembleSpeed);
            if (proto.prefabDesc.veinMiner) return GameData.MiningSpeedScale * proto.prefabDesc.minerPeriod / 1000000.0;
            // for water pump
            if (proto.prefabDesc.minerPeriod != 0) return proto.prefabDesc.minerPeriod / (3600*60*4.0);
            return 1;
        }

        internal double GetPower()
        {
            if (proto == null || proto.prefabDesc == null) return 0;
            if (proto.prefabDesc.isPowerConsumer) return proto.prefabDesc.workEnergyPerTick * 60;
            return 0;
        }

        public new IItem Clone(double factor = 1)
        {
            return new Factory(proto, this.count * factor);
        }
    }
}
