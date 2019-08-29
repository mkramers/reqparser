using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace reqparser.common
{
    public class UserNeed : ItemBase
    {
        private readonly List<Requirement> m_requirements;

        public UserNeed(int _id, string _description) : base(_id, _description, "UN")
        {
            m_requirements = new List<Requirement>();
        }

        private bool Equals(UserNeed _other)
        {
            return base.Equals(_other) && m_requirements.SequenceEqual(_other.m_requirements);
        }

        public override bool Equals(object _obj)
        {
            if (ReferenceEquals(null, _obj)) return false;
            if (ReferenceEquals(this, _obj)) return true;
            if (_obj.GetType() != GetType()) return false;
            return Equals((UserNeed) _obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ (m_requirements != null ? m_requirements.GetHashCode() : 0);
            }
        }

        public void AddRequirement(Requirement _requirement)
        {
            m_requirements.Add(_requirement);
        }

        public IEnumerable<Requirement> GetRequirements()
        {
            return new ReadOnlyCollection<Requirement>(m_requirements);
        }

        public override void SortById()
        {
            m_requirements.SortByIdRecursive();
        }
    }
}