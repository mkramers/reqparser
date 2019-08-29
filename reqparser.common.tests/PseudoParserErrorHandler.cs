namespace reqparser.common.tests
{
    public class PseudoParserErrorHandler : IParserErrorHandler
    {
        public int LineNumber { get; private set; }

        public void ThrowError(int _lineNumber, string _message)
        {
            LineNumber = _lineNumber;
        }
    }
}