using System;
using Xunit;

namespace TokenGeneratorTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var x = TokenGenerator.Data.Util.GenerateToken(2, 123456789);
            Assert.Equal("891234567", x.ToString());
        }
    }
}
