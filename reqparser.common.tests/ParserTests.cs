using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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
        public void OtherTypeNotEqual()
        {
            UserNeed a = new UserNeed(0, "test");

            // ReSharper disable once SuspiciousTypeConversion.Global
            bool isEqual = a.Equals(0);
            Assert.That(isEqual, Is.False);
        }

        [Test]
        public void NullNotEqual()
        {
            UserNeed a = new UserNeed(0, "test");
            
            bool isEqual = a.Equals(null);
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
        [Test]
        public void HashCodesAreDifferent()
        {
            UserNeed a = new UserNeed(0, "test");
            UserNeed b = new UserNeed(0, "test2");

            int hashCodeA = a.GetHashCode();
            int hashCodeB = b.GetHashCode();

            Assert.That(hashCodeA, Is.Not.EqualTo(hashCodeB));
        }
    }

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
        public void OtherTypeNotEqual()
        {
            Requirement a = new Requirement(0, "test");

            // ReSharper disable once SuspiciousTypeConversion.Global
            bool isEqual = a.Equals(0);
            Assert.That(isEqual, Is.False);
        }

        [Test]
        public void NullNotEqual()
        {
            Requirement a = new Requirement(0, "test");

            bool isEqual = a.Equals(null);
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
        [Test]
        public void HashCodesAreDifferent()
        {
            Requirement a = new Requirement(0, "test");
            Requirement b = new Requirement(0, "test2");

            int hashCodeA = a.GetHashCode();
            int hashCodeB = b.GetHashCode();

            Assert.That(hashCodeA, Is.Not.EqualTo(hashCodeB));
        }
    }

    [TestFixture]
    public class ThrowingParserErrorHandlerTests
    {
        [Test]
        public void Throws()
        {
            ThrowingParserErrorHandler throwingParserErrorHandler = new ThrowingParserErrorHandler();

            Assert.Throws<Exception>(() => throwingParserErrorHandler.ThrowError(0, "error"));
        }
    }

    [TestFixture]
    public class ParserTests
    {
        private static string GetEmbeddedResource(string _resourceName, Assembly _assembly)
        {
            _resourceName = FormatResourceName(_assembly, _resourceName);
            using (Stream resourceStream = _assembly.GetManifestResourceStream(_resourceName))
            {
                if (resourceStream == null)
                    return null;

                using (StreamReader reader = new StreamReader(resourceStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private static string FormatResourceName(Assembly _assembly, string _resourceName)
        {
            return _assembly.GetName().Name + "." + _resourceName.Replace(" ", "_")
                       .Replace("\\", ".")
                       .Replace("/", ".");
        }

        [Test]
        public void ParsesCorrectly()
        {
            string sampleText = GetEmbeddedResource("testcases/sample.txt", Assembly.GetExecutingAssembly());

            Specification specification = new Specification(1, "First spec");

            Requirement requirement = new Requirement(1, "First requirement");
            requirement.AddSpecification(specification);

            UserNeed userNeed = new UserNeed(1, "First user need");
            userNeed.AddRequirement(requirement);

            List<UserNeed> expectedUserNeeds = new List<UserNeed>
            {
                userNeed
            };

            Parser parser = new Parser();
            IEnumerable<UserNeed> actualUserNeeds = parser.Parse(sampleText);

            Assert.That(actualUserNeeds, Is.EquivalentTo(expectedUserNeeds));
        }
    }
}