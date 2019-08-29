using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

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
            return new[] {userNeed}.ToList();
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
            return new[] {userNeed}.ToList();
        }

        public static string GetEmbeddedResource(string _resourceName, Assembly _assembly)
        {
            _resourceName = FormatResourceName(_assembly, _resourceName);
            using (Stream resourceStream = _assembly.GetManifestResourceStream(_resourceName))
            {
                if (resourceStream == null) throw new NullReferenceException($"Resource {_resourceName} not found");

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
}