using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace reqparser.common
{
    public static class TextUtilities
    {
        public static string PrefixTextLines(string _prefix, string _text)
        {
            string normalizedText = _text.NormalizeLineEndings();

            StringBuilder prefixedText = new StringBuilder();

            string[] lines = normalizedText.Split(new[] {Environment.NewLine}, StringSplitOptions.None);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                string lineEnding = i < lines.Length - 1 ? Environment.NewLine : string.Empty;
                prefixedText.Append($"{_prefix}{line}{lineEnding}");
            }

            return prefixedText.ToString();
        }

        public static string IndentTextLines(string _text, int _numberOfIndents)
        {
            const char indentCharacter = '\t';
            string indent = new string(indentCharacter, _numberOfIndents);

            return PrefixTextLines(indent, _text);
        }

        public static string NormalizeLineEndings(this string _text)
        {
            return _text.Replace("\r\n", "\n").Replace("\n", Environment.NewLine);
        }

        public static IEnumerable<IReadOnlyList<string>> GetItemTextBlocks(IEnumerable<string> _lines, IEnumerable<string> _blockStartTexts)
        {
            string[] lines = _lines as string[] ?? _lines.ToArray();

            List<List<string>> textBlocks = new List<List<string>>();

            List<string> currentTextBlock = null;

            string[] blockStartTexts = _blockStartTexts as string[] ?? _blockStartTexts.ToArray();

            foreach (string line in lines)
            {
                if (blockStartTexts.Any(_blockStartText => line.StartsWith(_blockStartText)))
                {
                    if (currentTextBlock != null)
                    {
                        textBlocks.Add(currentTextBlock);
                    }

                    currentTextBlock = new List<string> { line };
                }
                else
                {
                    currentTextBlock?.Add(line);
                }
            }

            if (currentTextBlock != null)
            {
                textBlocks.Add(currentTextBlock);
            }

            return textBlocks;
        }
    }
}