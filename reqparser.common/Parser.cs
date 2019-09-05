using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace reqparser.common
{
    //todo 
    // - validate text blocks and find missing lines
    // - validate tree for missing/multi parents
    // - fix errors based on validation
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
        
        private IEnumerable<UserNeed> ParseBlocks(IEnumerable<IReadOnlyList<string>> _textBlocks)
        {
            List<UserNeed> userNeeds = new List<UserNeed>();
            List<(Requirement Requirement, string ParentLabel)> requirementsDetails = new List<(Requirement, string)>();
            List<(Specification Specification, string ParentLabel)> specificationsDetails = new List<(Specification, string)>();

            foreach (IReadOnlyList<string> textBlock in _textBlocks)
            {
                const string parentPrefix = "#### ";
                if (TryGetItemDetails(textBlock, $@"^### {"UN"}-[0-9]+$", parentPrefix, out (int Id, string Description, string ParentLabel) userNeedDetails))
                {
                    UserNeed userNeed = new UserNeed(userNeedDetails.Id, userNeedDetails.Description);
                    userNeeds.Add(userNeed);
                }
                else if (TryGetItemDetails(textBlock, $@"^### {"REQ"}-[0-9]+$", parentPrefix, out (int Id, string Description, string ParentLabel) requirementDetails))
                {
                    Requirement requirement = new Requirement(requirementDetails.Id, requirementDetails.Description);
                    requirementsDetails.Add((requirement, requirementDetails.ParentLabel));
                }
                else if (TryGetItemDetails(textBlock, $@"^### {"SPEC"}-[0-9]+$", parentPrefix, out (int Id, string Description, string ParentLabel) specificationDetails))
                {
                    Specification specification = new Specification(specificationDetails.Id, specificationDetails.Description);
                    specificationsDetails.Add((specification, specificationDetails.ParentLabel));
                }
                else
                {
                    ThrowError(-1, $"Failed to parse text block!");
                }
            }

            //associate items
            Requirement[] requirements = requirementsDetails.Select(_requirement => _requirement.Requirement).ToArray();

            foreach ((Specification specification, string parentLabel) in specificationsDetails)
            {
                Requirement requirement = requirements.FirstOrDefault(_requirement => _requirement.Label == parentLabel);
                requirement?.AddSpecification(specification);
            }

            foreach ((Requirement requirement, string parentLabel) in requirementsDetails)
            {
                UserNeed userNeed = userNeeds.FirstOrDefault(_userNeed => _userNeed.Label == parentLabel);
                userNeed?.AddRequirement(requirement);
            }

            return userNeeds;
        }

        private static bool TryGetItemDetails(IReadOnlyList<string> _lines, string _searchPattern,
            string _parentPrefix,
            out (int Id, string Description, string ParentLabel) _details)
        {
            _details = (0, "", "");

            string firstLine = _lines.First();

            Match specificationsMatch = Regex.Match(firstLine, _searchPattern);
            if (!specificationsMatch.Success)
            {
                return false;
            }

            _details.Id = int.Parse(specificationsMatch.Value.Split('-').Last());

            string parentLine = _lines[1];
            bool hasParent = parentLine.StartsWith(_parentPrefix);
            if (hasParent)
            {
                _details.ParentLabel = parentLine.Substring(_parentPrefix.Length, parentLine.Length - _parentPrefix.Length);
            }

            List<string> remainingText = _lines.Skip(hasParent ? 2 : 1).ToList();

            _details.Description = GetDescriptionFromTextBlock(remainingText);

            return true;
        }

        private static string GetDescriptionFromTextBlock(IEnumerable<string> _textBlock)
        {
            StringBuilder descriptionBuilder = new StringBuilder();
            foreach (string line in _textBlock)
            {
                if (line.StartsWith("#"))
                {
                    break;
                }

                descriptionBuilder.AppendLine(line);
            }

            string description = descriptionBuilder.ToString().Trim();
            return description;
        }

        public IEnumerable<UserNeed> Parse(string _text)
        {
            string[] lines = _text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            const string blockStart = "### ";
            string[] blockStartTexts = { "# ", "## ", blockStart };

            IEnumerable<IReadOnlyList<string>> textBlocks = TextUtilities.GetItemTextBlocks(lines, blockStartTexts);

            IEnumerable<IReadOnlyList<string>> itemBlocks = textBlocks.Where(_textBlock => _textBlock.First().StartsWith(blockStart));

            List<UserNeed> userNeeds = ParseBlocks(itemBlocks).ToList();

            userNeeds.SortByIdRecursive();

            return userNeeds;
        }

        private void ThrowError(int _lineIndex, string _message)
        {
            int lineNumber = _lineIndex + 1;
            m_errorHandler.ThrowError(lineNumber, _message);
        }
    }
}