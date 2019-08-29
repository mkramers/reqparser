namespace reqparser.common
{
    public abstract class ItemBase : IItemBase
    {
        private readonly string m_prefix;

        protected ItemBase(int _id, string _description, string _prefix)
        {
            m_prefix = _prefix;
            Id = _id;
            Description = _description;
        }

        public int Id { get; }

        public string Description { get; }
        public string Label => $"{m_prefix}-{Id:000}";

        public abstract void SortById();

        protected bool Equals(ItemBase _other)
        {
            return Id == _other.Id && Description == _other.Description;
        }

        public override bool Equals(object _obj)
        {
            return Equals((ItemBase) _obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Id * 397) ^ (Description != null ? Description.GetHashCode() : 0);
            }
        }
    }
}