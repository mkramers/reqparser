using System.Collections.Generic;
using System.IO;
using reqparser.common;

namespace reqparser
{
    internal class Program
    {
        private static void Main()
        {
            const string path = @"..\..\..\..\samples\reqs.md";

            string fileText = File.ReadAllText(path);

            Parser parser = new Parser();
            IEnumerable<UserNeed> userNeeds = parser.Parse(fileText);
        }
    }
}