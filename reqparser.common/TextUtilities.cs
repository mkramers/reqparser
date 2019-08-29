using System;
using System.Text;

namespace reqparser.common
{
    public static class TextUtilities
    {
        public static string PrefixTextLines(string _prefix, string _text)
        {
            StringBuilder prefixedText = new StringBuilder();

            string[] lines = _text.Split(new[] {Environment.NewLine}, StringSplitOptions.None);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                string lineEnding = i < lines.Length - 1 ? Environment.NewLine : string.Empty;
                prefixedText.Append($"{_prefix}{line}{lineEnding}");
            }

            return prefixedText.ToString();
        }
    }
}