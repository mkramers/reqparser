namespace reqparser.common
{
    public static class ItemBaseExtensions
    {
        public static string GetDisplayString(this ItemBase _itemBase)
        {
            return _itemBase.Label;
        }
    }
}