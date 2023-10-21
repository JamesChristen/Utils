using Common.CSV;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Common.Test.CSV.ValueParsingTests
{
    [TestClass]
    public class ParseDecimalTests
    {
        [TestMethod]
        public void Test_ParseDecimal_Valid_AsString()
        {
            Assert.AreEqual(1.234M, ValueParsers.ParseDecimal("1.234"));
        }

        [TestMethod]
        public void Test_ParseDecimal_Invalid_NonDecimalStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseDecimal("random_string"));
        }

        [TestMethod]
        public void Test_ParseDecimal_Invalid_NullStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseDecimal(null));
        }

        [TestMethod]
        public void Test_ParseDecimal_Invalid_EmptyStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseDecimal(string.Empty));
        }

        [TestMethod]
        public void Test_ParseDecimal_Invalid_WhitespaceStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseDecimal("    "));
        }

        [TestMethod]
        public void Test_ParseNullableDecimal_Valid_AsString()
        {
            Assert.AreEqual(1.234M, ValueParsers.ParseNullableDecimal("1.234"));
        }

        [TestMethod]
        public void Test_ParseNullableDecimal_Valid_NullStringReturnsNull()
        {
            Assert.AreEqual(null, ValueParsers.ParseNullableDecimal(null));
        }

        [TestMethod]
        public void Test_ParseNullableDecimal_Valid_EmptyStringReturnsNull()
        {
            Assert.AreEqual(null, ValueParsers.ParseNullableDecimal(string.Empty));
        }

        [TestMethod]
        public void Test_ParseNullableDecimal_Valid_WhitespaceStringReturnsNull()
        {
            Assert.AreEqual(null, ValueParsers.ParseNullableDecimal("    "));
        }

        [TestMethod]
        public void Test_ParseNullableDecimal_Invalid_NonDecimalStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseNullableDecimal("random_string"));
        }
    }
}
