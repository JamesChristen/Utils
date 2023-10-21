using Common.CSV;

namespace Common.Test.CSV.ValueParsingTests
{
    [TestClass]
    public class ParseBoolTests
    {
        [TestMethod]
        public void Test_ParseBool_Valid_AsString()
        {
            Assert.AreEqual(true, ValueParsers.ParseBool("true"));
            Assert.AreEqual(false, ValueParsers.ParseBool("false"));
        }

        [TestMethod]
        public void Test_ParseBool_Invalid_NonBoolStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseBool("random_string"));
        }

        [TestMethod]
        public void Test_ParseBool_Invalid_NullStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseBool(null));
        }

        [TestMethod]
        public void Test_ParseBool_Invalid_EmptyStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseBool(string.Empty));
        }

        [TestMethod]
        public void Test_ParseBool_Invalid_WhitespaceStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseBool("    "));
        }

        [TestMethod]
        public void Test_ParseNullableBool_Valid_AsString()
        {
            Assert.AreEqual(true, ValueParsers.ParseNullableBool("true"));
            Assert.AreEqual(false, ValueParsers.ParseNullableBool("false"));
        }

        [TestMethod]
        public void Test_ParseNullableBool_Valid_NullStringReturnsNull()
        {
            Assert.AreEqual(null, ValueParsers.ParseNullableBool(null));
        }

        [TestMethod]
        public void Test_ParseNullableBool_Valid_EmptyStringReturnsNull()
        {
            Assert.AreEqual(null, ValueParsers.ParseNullableBool(string.Empty));
        }

        [TestMethod]
        public void Test_ParseNullableBool_Valid_WhitespaceStringReturnsNull()
        {
            Assert.AreEqual(null, ValueParsers.ParseNullableBool("    "));
        }

        [TestMethod]
        public void Test_ParseNullableBool_Invalid_NonBoolStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseNullableBool("random_string"));
        }
    }
}
