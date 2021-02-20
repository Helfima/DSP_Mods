using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSP_Helmod.Classes
{
    public class HMEventQueue
    {
        private object sender;
        private HMEvent e;

        public static List<HMEventQueue> queues = new List<HMEventQueue>();

        private HMEventQueue(object sender, HMEvent e)
        {
            this.sender = sender;
            this.e = e;
        }

        public static void EnQueue(object sender, HMEvent e)
        {
            queues.Add(new HMEventQueue(sender, e));
        }

        public static void DeQueue()
        {
            if (queues.Count > 0)
            {
                foreach (HMEventQueue queue in queues)
                {
                    HMEvent.SendEvent(queue.sender, queue.e);
                }
                queues.Clear();
            }
        }
    }
}
