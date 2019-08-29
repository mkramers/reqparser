using NUnit.Framework;

namespace reqparser.common.tests
{
    [TestFixture]
    public class RequirementTests
    {
        [Test]
        public void AreEqual()
        {
            Requirement a = new Requirement(0, "test");
            Requirement b = new Requirement(0, "test");

            bool isEqual = a.Equals(b);
            Assert.That(isEqual, Is.True);
        }

        [Test]
        public void HashCodesAreDifferent()
        {
            Requirement a = new Requirement(0, "test");
            Requirement b = new Requirement(0, "test2");

            int hashCodeA = a.GetHashCode();
            int hashCodeB = b.GetHashCode();

            Assert.That(hashCodeA, Is.Not.EqualTo(hashCodeB));
        }

        [Test]
        public void LabelIsCorrect()
        {
            Requirement a = new Requirement(0, "test");

            Assert.That(a.Label, Is.EqualTo("REQ-000"));
        }

        [Test]
        public void NullNotEqual()
        {
            Requirement a = new Requirement(0, "test");

            bool isEqual = a.Equals(null);
            Assert.That(isEqual, Is.False);
        }

        [Test]
        public void OtherTypeNotEqual()
        {
            Requirement a = new Requirement(0, "test");

            // ReSharper disable once SuspiciousTypeConversion.Global
            bool isEqual = a.Equals(0);
            Assert.That(isEqual, Is.False);
        }

        [Test]
        public void SameReferenceIsEqual()
        {
            Requirement a = new Requirement(0, "test");

            // ReSharper disable once EqualExpressionComparison
            bool isEqual = a.Equals(a);
            Assert.That(isEqual, Is.True);
        }
    }
}