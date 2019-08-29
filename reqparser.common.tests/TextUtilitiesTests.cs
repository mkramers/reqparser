using System;
using NUnit.Framework;

namespace reqparser.common.tests
{
    [TestFixture]
    public class TextUtilitiesTests
    {
        [Test]
        public void IndentTextLinesCorrectly()
        {
            string text = $"A{Environment.NewLine}\tB{Environment.NewLine}\t\tC";

            string expectedPrefixedText = $"\tA{Environment.NewLine}\t\tB{Environment.NewLine}\t\t\tC";
            string actualPrefixedText = TextUtilities.IndentTextLines(text, 1);

            Assert.That(actualPrefixedText, Is.EqualTo(expectedPrefixedText));
        }

        [Test]
        public void PrefixesTextCorrectly()
        {
            const string prefix = "\t";

            string text = $"A{Environment.NewLine}\tB{Environment.NewLine}\t\tC";

            string expectedPrefixedText =
                $"{prefix}A{Environment.NewLine}{prefix}\tB{Environment.NewLine}{prefix}\t\tC";
            string actualPrefixedText = TextUtilities.PrefixTextLines(prefix, text);

            Assert.That(actualPrefixedText, Is.EqualTo(expectedPrefixedText));
        }
    }
}