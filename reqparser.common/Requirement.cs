using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace reqparser.common
{
    public class Requirement : ItemBase
    {
        private readonly List<Specification> m_specifications;

        public Requirement(int _id, string _description) : base(_id, _description, "REQ")
        {
            m_specifications = new List<Specification>();
        }

        private bool Equals(Requirement _other)
        {
            return base.Equals(_other) && m_specifications.SequenceEqual(_other.m_specifications);
        }

        public override bool Equals(object _obj)
        {
            if (ReferenceEquals(null, _obj)) return false;
            if (ReferenceEquals(this, _obj)) return true;
            if (_obj.GetType() != GetType()) return false;
            return Equals((Requirement) _obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ (m_specifications != null ? m_specifications.GetHashCode() : 0);
            }
        }

        public override void SortById()
        {
            Specification[] orderedSpecifications =
                m_specifications.OrderBy(_specification => _specification.Id).ToArray();

            m_specifications.Clear();
            m_specifications.AddRange(orderedSpecifications);

            foreach (Specification specification in orderedSpecifications)
            {
                specification.SortById();
            }
        }

        public void AddSpecification(Specification _specification)
        {
            m_specifications.Add(_specification);
        }

        public IEnumerable<Specification> GetSpecifications()
        {
            return new ReadOnlyCollection<Specification>(m_specifications);
        }
    }
}