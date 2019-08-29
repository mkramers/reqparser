using System;
using System.Collections.Generic;
using System.Linq;

namespace reqparser.common
{
    public static class ItemBaseExtensions
    {
        public static string GetDisplayString(this IItemBase _itemBase)
        {
            return _itemBase.Label;
        }
    }
}