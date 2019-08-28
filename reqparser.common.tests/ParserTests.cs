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