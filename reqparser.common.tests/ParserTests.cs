using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;

namespace reqparser.common.tests
{
    [TestFixture]
    public class ParserTests
    {
        private static string GetEmbeddedResource(string _resourceName, Assembly _assembly)
        {
            _resourceName = FormatResourceName(_assembly, _resourceName);
            using (Stream resourceStream = _assembly.GetManifestResourceStream(_resourceName))
            {
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

        [Test, TestCaseSource(nameof(FailureTestCases))]
        public void FailsWithoutEmptyLineAfterRequirementSpecifier(string _textResourceName, int _expectedFailLine)
        {
            string sampleText = GetEmbeddedResource(_textResourceName, Assembly.GetExecutingAssembly());

            PseudoParserErrorHandler errorHandler = new PseudoParserErrorHandler();
            Parser parser = new Parser(errorHandler);

            parser.Parse(sampleText);

            Assert.That(errorHandler.LineNumber, Is.EqualTo(_expectedFailLine));
        }

        public static IEnumerable FailureTestCases
        {
            get
            {
                yield return new TestCaseData("testcases/parentrequirementdoesnotexist.txt", 22).SetName("parentrequirementdoesnotexist");
                yield return new TestCaseData("testcases/norequirementforspecification.txt", 22).SetName("norequirementforspecification");
                yield return new TestCaseData("testcases/notemptyafterspecificationspecifier.txt", 13).SetName("notemptyafterspecificationspecifier");
                yield return new TestCaseData("testcases/notemptyafterrequirementspecifier.txt", 21).SetName("notemptyafterrequirementspecifier");
                yield return new TestCaseData("testcases/nouserneedforrequirement.txt", 14).SetName("nouserneedforrequirement");
                yield return new TestCaseData("testcases/parentuserneeddoesnotexist.txt", 14).SetName("parentuserneeddoesnotexist");
            }
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
}