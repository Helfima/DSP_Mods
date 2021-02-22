using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSP_Helmod.Model
{
    public class DataModel
    {
        private int version = 1;
        private List<Nodes> sheets = new List<Nodes>();

        public DataModel(int version = 1)
        {
            this.version = version;
        }
        public int Version
        {
            get { return version; }
        }
        public List<Nodes> Sheets
        {
            get { return sheets; }
        }

    }
}
