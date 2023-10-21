using Common.CSV;

namespace Common.Test.CSV.ValueParsingTests
{
    [TestClass]
    public class ParseByteTests
    {
        [TestMethod]
        public void Test_ParseByte_Valid()
        {
            Assert.AreEqual((byte)1, ValueParsers.ParseByte("1"));
        }

        [TestMethod]
        public void Test_ParseByte_Invalid_NonByteStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseByte("random_string"));
        }

        [TestMethod]
        public void Test_ParseByte_Invalid_NullStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseByte(null));
        }

        [TestMethod]
        public void Test_ParseByte_Invalid_EmptyStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseByte(string.Empty));
        }

        [TestMethod]
        public void Test_ParseByte_Invalid_WhitespaceStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseByte("    "));
        }

        [TestMethod]
        public void Test_ParseNullableByte_Valid()
        {
            Assert.AreEqual((byte)1, ValueParsers.ParseNullableByte("1"));
        }

        [TestMethod]
        public void Test_ParseNullableByte_Valid_NullStringReturnsNull()
        {
            Assert.AreEqual(null, ValueParsers.ParseNullableByte(null));
        }

        [TestMethod]
        public void Test_ParseNullableByte_Valid_EmptyStringReturnsNull()
        {
            Assert.AreEqual(null, ValueParsers.ParseNullableByte(string.Empty));
        }

        [TestMethod]
        public void Test_ParseNullableByte_Valid_WhitespaceStringReturnsNull()
        {
            Assert.AreEqual(null, ValueParsers.ParseNullableByte("    "));
        }

        [TestMethod]
        public void Test_ParseNullableByte_Invalid_NonByteStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseNullableByte("random_string"));
        }
    }
}
