namespace reqparser.common
{
    public static class ItemBaseExtensions
    {
        public static string GetDisplayString(this ItemBase _itemBase)
        {
            string indentedDescription = TextUtilities.IndentTextLines(_itemBase.Description, 1);
            return $"{_itemBase.Label}\n{indentedDescription}";
        }
    }
}