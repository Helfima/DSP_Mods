using DSP_Helmod.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSP_Helmod.UI.Gui
{
    public class Callback
    {
        public delegate void ForSheet(Nodes sheet);
        public delegate void ForNode(INode node);
        public delegate void ForItem(IItem item);
        public delegate void ForVoid();
    }
}
