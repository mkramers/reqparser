namespace reqparser.common
{
    public abstract class ItemBase
    {
        protected ItemBase(int _id, string _description)
        {
            Id = _id;
            Description = _description;
        }

        public int Id { get; }

        public string Description { get; }
        public abstract string Prefix { get; }
        public string Label => $"{Prefix}-{Id:000}";

        protected bool Equals(ItemBase _other)
        {
            return Id == _other.Id && Description == _other.Description;
        }

        public override bool Equals(object _obj)
        {
            if (ReferenceEquals(null, _obj)) return false;
            if (ReferenceEquals(this, _obj)) return true;
            if (_obj.GetType() != GetType()) return false;
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