using System;
using System.Collections.Generic;
using System.Linq;

namespace reqparser.common
{
    public static class ItemBaseExtensions
    {
        public static void SortByIdRecursive<T>(this List<T> _items) where T : IItemBase
        {
            List<T> orderedItems = _items.OrderBy(_item => _item.Id).ToList();

            _items.Clear();
            _items.AddRange(orderedItems);

            foreach (T orderedItem in orderedItems)
            {
                orderedItem.SortById();
            }
        }

        public static string GetDisplayString(this IItemBase _itemBase)
        {
            return _itemBase.Label;
        }
    }
}