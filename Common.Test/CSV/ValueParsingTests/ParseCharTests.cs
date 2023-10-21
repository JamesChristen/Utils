using Common.CSV;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Common.Test.CSV.ValueParsingTests
{
    [TestClass]
    public class ParseCharTests
    {
        [TestMethod]
        public void Test_ParseChar_Valid_AsString()
        {
            Assert.AreEqual('a', ValueParsers.ParseChar("a"));
        }

        [TestMethod]
        public void Test_ParseChar_Valid_AsInt()
        {
            Assert.AreEqual('1', ValueParsers.ParseChar("1"));
        }

        [TestMethod]
        public void Test_ParseChar_Valid_AsWhitespace()
        {
            Assert.AreEqual('\n', ValueParsers.ParseChar("\n"));
        }

        [TestMethod]
        public void Test_ParseChar_Invalid_NullStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseChar(null));
        }

        [TestMethod]
        public void Test_ParseChar_Invalid_EmptyStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseChar(string.Empty));
        }

        [TestMethod]
        public void Test_ParseChar_Invalid_LongStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseChar("more_than_one_char"));
        }

        [TestMethod]
        public void Test_ParseNullableChar_Valid_AsString()
        {
            Assert.AreEqual('a', ValueParsers.ParseChar("a"));
        }

        [TestMethod]
        public void Test_ParseNullableChar_Valid_AsInt()
        {
            Assert.AreEqual('1', ValueParsers.ParseChar("1"));
        }

        [TestMethod]
        public void Test_ParseNullableChar_Valid_AsWhitespace()
        {
            Assert.AreEqual('\n', ValueParsers.ParseChar("\n"));
        }

        [TestMethod]
        public void Test_ParseNullableChar_Valid_NullStringReturnsNull()
        {
            Assert.AreEqual(null, ValueParsers.ParseNullableChar(null));
        }

        [TestMethod]
        public void Test_ParseNullableChar_Valid_EmptyStringReturnsNull()
        {
            Assert.AreEqual(null, ValueParsers.ParseNullableChar(string.Empty));
        }

        [TestMethod]
        public void Test_ParseNullableChar_Invalid_LongStringReturnsNull()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseNullableChar("more_than_one_char"));
        }
    }
}
