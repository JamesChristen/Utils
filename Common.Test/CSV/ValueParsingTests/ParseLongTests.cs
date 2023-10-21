using Common.CSV;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Common.Test.CSV.ValueParsingTests
{
    [TestClass]
    public class ParseLongTests
    {
        [TestMethod]
        public void Test_ParseLong_Valid_AsString()
        {
            Assert.AreEqual(123456789L, ValueParsers.ParseLong("123456789"));
        }

        [TestMethod]
        public void Test_ParseLong_Invalid_NonLongStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseLong("random_string"));
        }

        [TestMethod]
        public void Test_ParseLong_Invalid_NullStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseLong(null));
        }

        [TestMethod]
        public void Test_ParseLong_Invalid_EmptyStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseLong(string.Empty));
        }

        [TestMethod]
        public void Test_ParseLong_Invalid_WhitespaceStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseLong("    "));
        }

        [TestMethod]
        public void Test_ParseNullableLong_Valid_AsString()
        {
            Assert.AreEqual(123456789L, ValueParsers.ParseNullableLong("123456789"));
        }

        [TestMethod]
        public void Test_ParseNullableLong_Valid_NullStringReturnsNull()
        {
            Assert.AreEqual(null, ValueParsers.ParseNullableLong(null));
        }

        [TestMethod]
        public void Test_ParseNullableLong_Valid_EmptyStringReturnsNull()
        {
            Assert.AreEqual(null, ValueParsers.ParseNullableLong(string.Empty));
        }

        [TestMethod]
        public void Test_ParseNullableLong_Valid_WhitespaceStringReturnsNull()
        {
            Assert.AreEqual(null, ValueParsers.ParseNullableLong("    "));
        }

        [TestMethod]
        public void Test_ParseNullableLong_Invalid_NonLongStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseNullableLong("random_string"));
        }
    }
}
