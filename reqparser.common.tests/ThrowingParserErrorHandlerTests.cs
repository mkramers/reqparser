using System;
using NUnit.Framework;

namespace reqparser.common.tests
{
    [TestFixture]
    public class ThrowingParserErrorHandlerTests
    {
        [Test]
        public void Throws()
        {
            ThrowingParserErrorHandler throwingParserErrorHandler = new ThrowingParserErrorHandler();

            Assert.Throws<Exception>(() => throwingParserErrorHandler.ThrowError(0, "error"));
        }
    }
}