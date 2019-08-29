using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace reqparser.common.tests
{
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

        private static IEnumerable FailureTestCases
        {
            // ReSharper disable once UnusedMember.Local
            get
            {
                yield return new TestCaseData("testcases/parentrequirementdoesnotexist.txt", 23);
                yield return new TestCaseData("testcases/norequirementforspecification.txt", 23);
                yield return new TestCaseData("testcases/notemptyafterspecificationspecifier.txt", 14);
                yield return new TestCaseData("testcases/notemptyafterrequirementspecifier.txt", 22);
                yield return new TestCaseData("testcases/nouserneedforrequirement.txt", 15);
                yield return new TestCaseData("testcases/parentuserneeddoesnotexist.txt", 15);
            }
        }

        [Test]
        [TestCaseSource(nameof(SampleTestCases))]
        public void ParsesCorrectly(string _textResourceName)
        {
            string sampleText = Helpers.GetEmbeddedResource(_textResourceName, Assembly.GetExecutingAssembly());

            IEnumerable<UserNeed> expectedUserNeeds = Helpers.CreateOrderedUserNeeds();

            Parser parser = new Parser();
            List<UserNeed> actualUserNeeds = parser.Parse(sampleText).ToList();

            Assert.That(actualUserNeeds, Is.EquivalentTo(expectedUserNeeds));
        }

        [Test]
        [TestCaseSource(nameof(FailureTestCases))]
        public void TestFailureMechanisms(string _textResourceName, int _expectedFailLine)
        {
            string sampleText = Helpers.GetEmbeddedResource(_textResourceName, Assembly.GetExecutingAssembly());

            PseudoParserErrorHandler errorHandler = new PseudoParserErrorHandler();
            Parser parser = new Parser(errorHandler);

            parser.Parse(sampleText);

            Assert.That(errorHandler.LineNumber, Is.EqualTo(_expectedFailLine));
        }
    }
}