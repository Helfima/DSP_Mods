using DSP_Helmod.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DSP_Helmod.UI.Gui
{
    public class HMLayoutOptions
    {
        public static int numProducts = 4;
        public static int numIngredients = 8;
        private static GUILayoutOption ColumnWidthAction = GUILayout.Width(100);
        private static GUILayoutOption ColumnWidthProduction = GUILayout.Width(40);
        private static GUILayoutOption ColumnWidthRecipe = GUILayout.Width(80);
        private static GUILayoutOption ColumnWidthPower = GUILayout.Width(80);
        private static GUILayoutOption ColumnWidthMachine = GUILayout.Width(80);
        private static GUILayoutOption ColumnWidthProducts = GUILayout.Width(5 + (45 + 2) * numProducts);
        private static GUILayoutOption ColumnWidthIngredients = GUILayout.Width(5 + (45 + 2) * numIngredients);
        public static GUILayoutOption[] GetDataCell(DataColumn column, bool logistic)
        {
            GUILayoutOption height = GUILayout.Height(73);
            if(logistic) height = GUILayout.Height(91);
            switch (column)
            {
                case DataColumn.Actions:
                    return new GUILayoutOption[] { ColumnWidthAction, height };
                case DataColumn.Production:
                    return new GUILayoutOption[] { ColumnWidthProduction, height };
                case DataColumn.Recipe:
                    return new GUILayoutOption[] { ColumnWidthRecipe, height };
                case DataColumn.Power:
                    return new GUILayoutOption[] { ColumnWidthPower, height };
                case DataColumn.Machine:
                    return new GUILayoutOption[] { ColumnWidthMachine, height };
                case DataColumn.Products:
                    return new GUILayoutOption[] { ColumnWidthProducts, height };
                case DataColumn.Ingredients:
                    return new GUILayoutOption[] { ColumnWidthIngredients, height };
            }
            return null;
        }
        public static GUILayoutOption[] BoxIcon = new GUILayoutOption[] { GUILayout.Height(70) };

        public static GUILayoutOption[] Icon50 = new GUILayoutOption[] { GUILayout.Height(50), GUILayout.Width(50) };
        public static GUILayoutOption[] Icon45 = new GUILayoutOption[] { GUILayout.Height(45), GUILayout.Width(45) };
        public static GUILayoutOption[] Icon30 = new GUILayoutOption[] { GUILayout.Height(30), GUILayout.Width(30) };
        public static GUILayoutOption[] Icon20 = new GUILayoutOption[] { GUILayout.Height(20), GUILayout.Width(20) };
        public static GUILayoutOption[] Icon15 = new GUILayoutOption[] { GUILayout.Height(15), GUILayout.Width(15) };

        public static GUILayoutOption[] Text45x15 = new GUILayoutOption[] { GUILayout.Height(15), GUILayout.Width(45) };
        public static GUILayoutOption[] Text30x15 = new GUILayoutOption[] { GUILayout.Height(15), GUILayout.Width(30) };


        public static GUILayoutOption[] ActionButton = new GUILayoutOption[] { GUILayout.Height(25), GUILayout.Width(25) };

        public static GUILayoutOption[] ListDetail = new GUILayoutOption[] { GUILayout.Width(250) };
        public static GUILayoutOption[] ScrollListDetail = new GUILayoutOption[] { GUILayout.Height(170) };

        public static GUILayoutOption[] ScrollNav = new GUILayoutOption[] { GUILayout.ExpandHeight(true), GUILayout.Width(140) };
        public static GUILayoutOption[] ScrollData = new GUILayoutOption[] { GUILayout.ExpandHeight(true)};
        public static GUILayoutOption[] ScrollChoose = new GUILayoutOption[] { GUILayout.Height(300) };

        public static GUILayoutOption[] ColumnAction = new GUILayoutOption[] { ColumnWidthAction };
        public static GUILayoutOption[] ColumnProduction = new GUILayoutOption[] { ColumnWidthProduction };
        public static GUILayoutOption[] ColumnRecipe = new GUILayoutOption[] { ColumnWidthRecipe };
        public static GUILayoutOption[] ColumnPower = new GUILayoutOption[] { ColumnWidthPower };
        public static GUILayoutOption[] ColumnMachine = new GUILayoutOption[] { ColumnWidthMachine };
        public static GUILayoutOption[] ColumnProducts = new GUILayoutOption[] { ColumnWidthProducts };
        public static GUILayoutOption[] ColumnIngredients = new GUILayoutOption[] { ColumnWidthIngredients };
    }
}
