using Common.CSV;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Common.Test.CSV.ValueParsingTests
{
    [TestClass]
    public class ParseDoubleTests
    {
        [TestMethod]
        public void Test_ParseDouble_Valid_AsString()
        {
            Assert.AreEqual(1.23d, ValueParsers.ParseDouble("1.23"));
        }

        [TestMethod]
        public void Test_ParseDouble_Invalid_NonDoubleStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseDouble("random_string"));
        }

        [TestMethod]
        public void Test_ParseDouble_Invalid_NullStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseDouble(null));
        }

        [TestMethod]
        public void Test_ParseDouble_Invalid_EmptyStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseDouble(string.Empty));
        }

        [TestMethod]
        public void Test_ParseDouble_Invalid_WhitespaceStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseDouble("    "));
        }

        [TestMethod]
        public void Test_ParseNullableDouble_Valid_AsString()
        {
            Assert.AreEqual(1.23d, ValueParsers.ParseNullableDouble("1.23"));
        }

        [TestMethod]
        public void Test_ParseNullableDouble_Valid_NullStringReturnsNull()
        {
            Assert.AreEqual(null, ValueParsers.ParseNullableDouble(null));
        }

        [TestMethod]
        public void Test_ParseNullableDouble_Valid_EmptyStringReturnsNull()
        {
            Assert.AreEqual(null, ValueParsers.ParseNullableDouble(string.Empty));
        }

        [TestMethod]
        public void Test_ParseNullableDouble_Valid_WhitespaceStringReturnsNull()
        {
            Assert.AreEqual(null, ValueParsers.ParseNullableDouble("    "));
        }

        [TestMethod]
        public void Test_ParseNullableDouble_Invalid_NonDoubleStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseNullableDouble("random_string"));
        }
    }
}
