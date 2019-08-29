using System;
using NUnit.Framework;

namespace reqparser.common.tests
{
    [TestFixture]
    public class TraceabilityGeneratorTests
    {
        [Test]
        public void GeneratesTraceabilityCorrectly()
        {
            Specification specification = new Specification(0, "");
            Requirement requirement = new Requirement(0, "");
            requirement.AddSpecification(specification);
            UserNeed userNeed = new UserNeed(0, "");
            userNeed.AddRequirement(requirement);

            string actualTraceabilityText = TraceabilityGenerator.Generate(new[] {userNeed});
            string expectedTraceabilityText = $"UN-000{Environment.NewLine}\tREQ-000{Environment.NewLine}\t\tSPEC-000";

            Assert.That(actualTraceabilityText, Is.EqualTo(expectedTraceabilityText));
        }
    }
}