﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSP_Helmod.Classes
{
    public enum FactorySelection
    {
        First,
        Last
    }

    public enum ItemState
    {
        Normal,
        Main,
        Residual,
        Overflow
    }

    public enum ItemColor
    {
        Normal,
        Blue,
        Green,
        Orange,
        Red,
        Yellow
    }

    public enum SelectorMode
    {
        Normal,
        Properties
    }

    public enum DataColumn
    {
        Actions,
        Ingredients,
        Machine,
        Power,
        Production,
        Products,
        Recipe
    }
}
