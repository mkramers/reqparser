using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace reqparser.common
{
    public class Parser
    {
        private readonly IParserErrorHandler m_errorHandler;

        public Parser(IParserErrorHandler _errorHandler)
        {
            m_errorHandler = _errorHandler;
        }

        public Parser() : this(new ThrowingParserErrorHandler())
        {
        }

        public IEnumerable<UserNeed> Parse(string _text)
        {
            List<UserNeed> userNeeds = new List<UserNeed>();

            string[] lines = _text.Split(new[] {Environment.NewLine}, StringSplitOptions.None);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                Match userNeedMatch = Regex.Match(line, @"^### UN-[0-9]+$");
                if (userNeedMatch.Success)
                {
                    i++;

                    int id = int.Parse(userNeedMatch.Value.Split('-').Last());

                    StringBuilder descriptionBuilder = new StringBuilder();
                    while (i < lines.Length)
                    {
                        line = lines[i];
                        if (line.StartsWith("#"))
                        {
                            i--;
                            break;
                        }

                        descriptionBuilder.AppendLine(line);
                        i++;
                    }

                    UserNeed userNeed = new UserNeed(id, descriptionBuilder.ToString().Trim());
                    userNeeds.Add(userNeed);
                }

                Match requirementMatch = Regex.Match(line, @"^### REQ-[0-9]+$");
                if (requirementMatch.Success)
                {
                    int id = int.Parse(requirementMatch.Value.Split('-').Last());

                    //ensure next line is blank
                    i++;
                    line = lines[i];
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        ThrowError(i, "Expected empty line after requirement specifier");
                        return null;
                    }

                    //ensure next is userneed reference
                    i++;
                    line = lines[i];

                    Match parentUserNeedMatch = Regex.Match(line, @"^#### UN-[0-9]+$");
                    if (!parentUserNeedMatch.Success)
                    {
                        ThrowError(i, "Expected user need reference for requirement");
                        return null;
                    }

                    int parentUserNeedId = int.Parse(parentUserNeedMatch.Value.Split('-').Last());

                    UserNeed parentUserNeed = userNeeds.Find(_userNeed => _userNeed.Id == parentUserNeedId);
                    if (parentUserNeed == null)
                    {
                        ThrowError(i, "No parent user need exists for requirement");
                        return null;
                    }

                    i++;

                    StringBuilder descriptionBuilder = new StringBuilder();
                    while (i < lines.Length)
                    {
                        line = lines[i];
                        if (line.StartsWith("#"))
                        {
                            i--;
                            break;
                        }

                        descriptionBuilder.AppendLine(line);
                        i++;
                    }

                    Requirement requirement = new Requirement(id, descriptionBuilder.ToString().Trim());

                    parentUserNeed.AddRequirement(requirement);
                }
                
                Match specificationMatch = Regex.Match(line, @"^### SPEC-[0-9]+$");
                if (specificationMatch.Success)
                {
                    int id = int.Parse(specificationMatch.Value.Split('-').Last());

                    //ensure next line is blank
                    //i++;
                    line = lines[++i];
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        ThrowError(i, "Expected empty line after specification specifier");
                        return null;
                    }

                    //ensure next is requirement reference
                    line = lines[++i];

                    Match parentRequirementMatch = Regex.Match(line, @"^#### REQ-[0-9]+$");
                    if (!parentRequirementMatch.Success)
                    {
                        ThrowError(i, "Expected requirement reference for specification");
                        return null;
                    }

                    int parentRequirementId = int.Parse(parentRequirementMatch.Value.Split('-').Last());

                    List<Requirement> requirements = userNeeds.SelectMany(_userNeed => _userNeed.GetRequirements())
                        .Distinct().ToList();
                    Requirement parentRequirement =
                        requirements.Find(_requirement => _requirement.Id == parentRequirementId);
                    if (parentRequirement == null)
                    {
                        ThrowError(i, "No parent requirement exists for specification");
                        return null;
                    }

                    i++;

                    StringBuilder descriptionBuilder = new StringBuilder();
                    while (i < lines.Length)
                    {
                        line = lines[i];
                        if (line.StartsWith("#")) break;

                        descriptionBuilder.AppendLine(line);
                        i++;
                    }

                    Specification specification = new Specification(id, descriptionBuilder.ToString().Trim());

                    parentRequirement.AddSpecification(specification);
                }
            }

            return userNeeds;
        }

        private void ThrowError(int _lineNumber, string _message)
        {
            m_errorHandler.ThrowError(_lineNumber, _message);
        }
    }
}