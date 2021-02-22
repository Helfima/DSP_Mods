using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSP_Helmod.Classes
{
    public class ListExtensions
    {
        public static void Move<T>(List<T> list, int oldIndex, int newIndex)
        {
            var item = list[oldIndex];

            list.RemoveAt(oldIndex);

            if (newIndex > oldIndex) newIndex--;
            // the actual index could have shifted due to the removal

            list.Insert(newIndex, item);
        }

        public static void Move<T>(List<T> list, T item, int newIndex)
        {
            if (item != null)
            {
                var oldIndex = list.IndexOf(item);
                if (oldIndex > -1)
                {
                    list.RemoveAt(oldIndex);

                    if (newIndex > oldIndex) newIndex--;
                    // the actual index could have shifted due to the removal

                    list.Insert(newIndex, item);
                }
            }

        }

        public static void MoveStep<T>(List<T> list, T item, int step)
        {
            if (item != null)
            {
                var oldIndex = list.IndexOf(item);
                if (oldIndex > -1)
                {
                    if (step > 0)
                    {
                        list.RemoveAt(oldIndex);
                        if (oldIndex + step < list.Count)
                        {
                            list.Insert(oldIndex + step, item);
                        }
                        else
                        {
                            list.Add(item);
                        }
                    }
                    if (step < 0)
                    {
                        if (oldIndex + step >= 0)
                        {
                            list.Insert(oldIndex + step, item);
                        }
                        else
                        {
                            list.Insert(0, item);
                        }
                        list.RemoveAt(oldIndex+1);
                    }
                }
            }

        }
    }
}
