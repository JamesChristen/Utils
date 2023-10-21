using Common.CSV;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Common.Test.CSV.ValueParsingTests
{
    [TestClass]
    public class ParseShortTests
    {
        [TestMethod]
        public void Test_ParseShort_Valid_AsString()
        {
            Assert.AreEqual(123, ValueParsers.ParseShort("123"));
        }

        [TestMethod]
        public void Test_ParseShort_Invalid_NonShortStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseShort("random_string"));
        }

        [TestMethod]
        public void Test_ParseShort_Invalid_NullStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseShort(null));
        }

        [TestMethod]
        public void Test_ParseShort_Invalid_EmptyStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseShort(string.Empty));
        }

        [TestMethod]
        public void Test_ParseShort_Invalid_WhitespaceStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseShort("    "));
        }

        [TestMethod]
        public void Test_ParseNullableShort_Valid_AsString()
        {
            Assert.AreEqual((short)123, ValueParsers.ParseNullableShort("123"));
        }

        [TestMethod]
        public void Test_ParseNullableShort_Valid_NullStringReturnsNull()
        {
            Assert.AreEqual(null, ValueParsers.ParseNullableShort(null));
        }

        [TestMethod]
        public void Test_ParseNullableShort_Valid_EmptyStringReturnsNull()
        {
            Assert.AreEqual(null, ValueParsers.ParseNullableShort(string.Empty));
        }

        [TestMethod]
        public void Test_ParseNullableShort_Valid_WhitespaceStringReturnsNull()
        {
            Assert.AreEqual(null, ValueParsers.ParseNullableShort("    "));
        }

        [TestMethod]
        public void Test_ParseNullableShort_Invalid_NonShortStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseNullableShort("random_string"));
        }
    }
}
