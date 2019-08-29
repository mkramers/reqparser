using System.Collections.Generic;
using System.Text;

namespace reqparser.common
{
    public static class TraceabilityGenerator
    {
        public static string Generate(IEnumerable<UserNeed> _userNeeds)
        {
            StringBuilder traceabilityText = new StringBuilder();

            foreach (UserNeed userNeed in _userNeeds)
            {
                traceabilityText.AppendLine(userNeed.Label);

                foreach (Requirement requirement in userNeed.GetRequirements())
                {
                    string indentTextLines = TextUtilities.IndentTextLines(requirement.Label, 1);
                    traceabilityText.AppendLine(indentTextLines);

                    foreach (Specification specification in requirement.GetSpecifications())
                        traceabilityText.AppendLine(TextUtilities.IndentTextLines(specification.Label, 2));
                }
            }

            return traceabilityText.ToString().Trim();
        }
    }
}