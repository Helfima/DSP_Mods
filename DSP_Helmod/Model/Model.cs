using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSP_Helmod.Model
{
    public class Model
    {
        private int version = 1;
        private List<Sheet> sheets = new List<Sheet>();

        public int Version
        {
            get { return version; }
        }
        public List<Sheet> Sheets
        {
            get { return sheets; }
        }
    }
}
