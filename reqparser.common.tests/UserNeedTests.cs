using NUnit.Framework;

namespace reqparser.common.tests
{
    [TestFixture]
    public class UserNeedTests
    {
        [Test]
        public void AreEqual()
        {
            UserNeed a = new UserNeed(0, "test");
            UserNeed b = new UserNeed(0, "test");

            bool isEqual = a.Equals(b);
            Assert.That(isEqual, Is.True);
        }

        [Test]
        public void HashCodesAreDifferent()
        {
            UserNeed a = new UserNeed(0, "test");
            UserNeed b = new UserNeed(0, "test2");

            int hashCodeA = a.GetHashCode();
            int hashCodeB = b.GetHashCode();

            Assert.That(hashCodeA, Is.Not.EqualTo(hashCodeB));
        }

        [Test]
        public void NullNotEqual()
        {
            UserNeed a = new UserNeed(0, "test");

            bool isEqual = a.Equals(null);
            Assert.That(isEqual, Is.False);
        }

        [Test]
        public void OtherTypeNotEqual()
        {
            UserNeed a = new UserNeed(0, "test");

            // ReSharper disable once SuspiciousTypeConversion.Global
            bool isEqual = a.Equals(0);
            Assert.That(isEqual, Is.False);
        }

        [Test]
        public void SameReferenceIsEqual()
        {
            UserNeed a = new UserNeed(0, "test");

            // ReSharper disable once EqualExpressionComparison
            bool isEqual = a.Equals(a);
            Assert.That(isEqual, Is.True);
        }
    }
}