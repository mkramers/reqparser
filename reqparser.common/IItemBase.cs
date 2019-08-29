namespace reqparser.common
{
    public interface IItemBase
    {
        int Id { get; }
        string Description { get; }
        string Label { get; }
        void SortById();
    }
}