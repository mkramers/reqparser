using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace reqparser.common.tests
{
    public static class Helpers
    {
        public static IEnumerable<UserNeed> CreateOrderedUserNeeds()
        {
            Specification specification = new Specification(1, "First spec");
            Specification secondSpecification = new Specification(2, "Second spec");

            Requirement requirement = new Requirement(1, "First requirement");
            requirement.AddSpecification(specification);
            requirement.AddSpecification(secondSpecification);

            Requirement otherRequirement = new Requirement(2, "Second requirement");

            UserNeed userNeed = new UserNeed(1, "First user need");
            userNeed.AddRequirement(requirement);
            userNeed.AddRequirement(otherRequirement);
            return new[] { userNeed }.ToList();
        }

        public static List<UserNeed> CreateUnOrderUserNeeds()
        {
            Specification specification = new Specification(1, "First spec");
            Specification secondSpecification = new Specification(2, "Second spec");

            Requirement requirement = new Requirement(1, "First requirement");
            requirement.AddSpecification(secondSpecification);
            requirement.AddSpecification(specification);

            Requirement secondRequirement = new Requirement(2, "Second requirement");

            UserNeed userNeed = new UserNeed(1, "First user need");
            userNeed.AddRequirement(secondRequirement);
            userNeed.AddRequirement(requirement);
            return new[] { userNeed }.ToList();
        }

        public static string GetEmbeddedResource(string _resourceName, Assembly _assembly)
        {
            _resourceName = FormatResourceName(_assembly, _resourceName);
            using (Stream resourceStream = _assembly.GetManifestResourceStream(_resourceName))
            {
                if (resourceStream == null)
                {
                    throw new NullReferenceException($"Resource {_resourceName} not found");
                }

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
    }

    [TestFixture]
    public class ParserTests
    {
        private static IEnumerable SampleTestCases
        {
            // ReSharper disable once UnusedMember.Local
            get
            {
                yield return new TestCaseData("testcases/sample.txt");
                yield return new TestCaseData("testcases/outoforder.txt");
            }
        }

        [Test, TestCaseSource(nameof(SampleTestCases))]
        public void ParsesCorrectly(string _textResourceName)
        {
            string sampleText = Helpers.GetEmbeddedResource(_textResourceName, Assembly.GetExecutingAssembly());

            IEnumerable<UserNeed> expectedUserNeeds = Helpers.CreateOrderedUserNeeds();

            Parser parser = new Parser();
            List<UserNeed> actualUserNeeds = parser.Parse(sampleText).ToList();
            
            Assert.That(actualUserNeeds, Is.EquivalentTo(expectedUserNeeds));
        }

        private static IEnumerable FailureTestCases
        {
            // ReSharper disable once UnusedMember.Local
            get
            {
                yield return new TestCaseData("testcases/parentrequirementdoesnotexist.txt", 22);
                yield return new TestCaseData("testcases/norequirementforspecification.txt", 22);
                yield return new TestCaseData("testcases/notemptyafterspecificationspecifier.txt", 13);
                yield return new TestCaseData("testcases/notemptyafterrequirementspecifier.txt", 21);
                yield return new TestCaseData("testcases/nouserneedforrequirement.txt", 14);
                yield return new TestCaseData("testcases/parentuserneeddoesnotexist.txt", 14);
            }
        }

        [Test, TestCaseSource(nameof(FailureTestCases))]
        public void TestFailureMechanisms(string _textResourceName, int _expectedFailLine)
        {
            string sampleText = Helpers.GetEmbeddedResource(_textResourceName, Assembly.GetExecutingAssembly());

            PseudoParserErrorHandler errorHandler = new PseudoParserErrorHandler();
            Parser parser = new Parser(errorHandler);

            parser.Parse(sampleText);

            Assert.That(errorHandler.LineNumber, Is.EqualTo(_expectedFailLine));
        }
    }

    public class PseudoParserErrorHandler : IParserErrorHandler
    {
        public int LineNumber { get; private set; }

        public void ThrowError(int _lineNumber, string _message)
        {
            LineNumber = _lineNumber;
        }
    }

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