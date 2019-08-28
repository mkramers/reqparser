namespace reqparser.common
{
    public interface IParserErrorHandler
    {
        void ThrowError(int _lineNumber, string _message);
    }
}