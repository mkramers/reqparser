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
                traceabilityText.AppendLine(userNeed.GetDisplayString());

                foreach (Requirement requirement in userNeed.GetRequirements())
                {
                    string requirementText = requirement.GetDisplayString();
                    string indentTextLines = TextUtilities.IndentTextLines(requirementText, 1);
                    traceabilityText.AppendLine(indentTextLines);

                    foreach (Specification specification in requirement.GetSpecifications())
                    {
                        string specificationText = specification.GetDisplayString();
                        traceabilityText.AppendLine(TextUtilities.IndentTextLines(specificationText, 2));
                    }
                }
            }

            return traceabilityText.ToString();
        }
    }
}
