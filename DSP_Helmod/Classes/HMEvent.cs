using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSP_Helmod.Classes
{
    public class HMEvent : EventArgs
    {
        public static event EventHandler<HMEvent> Handler;

        public static void SendEvent(object sender, HMEvent e)
        {
            if (Handler != null)
            {
                Handler(sender, e);
            }
        }

        private HMEventType type;
        private object item;
        public HMEvent(HMEventType type, object item)
        {
            this.type = type;
            this.item = item;
        }

        public HMEventType Type
        {
            get { return type; }
        }

        public object Item
        {
            get { return item; }
        }

        public T GetItem<T>() where T : class
        {
            if(item is T) return (T) item;
            return default(T);
        }

        
    }

    public enum HMEventType
    {
        LoadedModel,
        OpenClose,
        Update,
        AddRecipe,
        AddRecipeByProduct,
        AddRecipeByIngredient,
        AddItem,
        RemoveNode,
        ChooseRecipe,
        SwitchChooseRecipe,
        EditionProduct,
        EditionRecipe,
        UpdateSheet,
        UpNode,
        DownNode,
        UpLevelNode,
        DownLevelNode,
        ChangeLogisticItem,
        AddProperties,
        RemoveProperties
    }
}
