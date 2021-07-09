using NUnit.Framework;
using TokenGenerator;

namespace TokenGeneratorTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Test1()
        {
            var x = TokenGenerator.Data.Util.GenerateToken(2,123456789);
            Assert.AreEqual(891234567, x);
            Assert.Pass();
        }
    }
}