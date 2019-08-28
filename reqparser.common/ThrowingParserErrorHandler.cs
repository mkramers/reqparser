using System;

namespace reqparser.common
{
    public class ThrowingParserErrorHandler : IParserErrorHandler
    {
        public void ThrowError(int _lineNumber, string _message)
        {
            throw new Exception($"Parsing failed at line {_lineNumber} => {_message}");
        }
    }
}