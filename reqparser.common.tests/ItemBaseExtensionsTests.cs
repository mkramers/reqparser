using System.Collections.Generic;
using NUnit.Framework;

namespace reqparser.common.tests
{
    [TestFixture]
    public class ItemBaseExtensionsTests
    {
        [Test]
        public void SortsByIdRecursiveCorrectly()
        {
            IEnumerable<UserNeed> orderedUserNeeds = Helpers.CreateOrderedUserNeeds();

            List<UserNeed> sortedUserNeeds = Helpers.CreateUnOrderUserNeeds();
            sortedUserNeeds.SortByIdRecursive();

            Assert.That(sortedUserNeeds, Is.EquivalentTo(orderedUserNeeds));
        }
    }
}