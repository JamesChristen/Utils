using Common.CSV;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Common.Test.CSV.ValueParsingTests
{
    [TestClass]
    public class ParseIntTests
    {
        [TestMethod]
        public void Test_ParseInt_Valid_AsString()
        {
            Assert.AreEqual(123, ValueParsers.ParseInt("123"));
        }

        [TestMethod]
        public void Test_ParseInt_Invalid_NonIntStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseInt("random_string"));
        }

        [TestMethod]
        public void Test_ParseInt_Invalid_NullStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseInt(null));
        }

        [TestMethod]
        public void Test_ParseInt_Invalid_EmptyStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseInt(string.Empty));
        }

        [TestMethod]
        public void Test_ParseInt_Invalid_WhitespaceStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseInt("    "));
        }

        [TestMethod]
        public void Test_ParseNullableInt_Valid_AsString()
        {
            Assert.AreEqual(123, ValueParsers.ParseNullableInt("123"));
        }

        [TestMethod]
        public void Test_ParseNullableInt_Valid_NullStringReturnsNull()
        {
            Assert.AreEqual(null, ValueParsers.ParseNullableInt(null));
        }

        [TestMethod]
        public void Test_ParseNullableInt_Valid_EmptyStringReturnsNull()
        {
            Assert.AreEqual(null, ValueParsers.ParseNullableInt(string.Empty));
        }

        [TestMethod]
        public void Test_ParseNullableInt_Valid_WhitespaceStringReturnsNull()
        {
            Assert.AreEqual(null, ValueParsers.ParseNullableInt("    "));
        }

        [TestMethod]
        public void Test_ParseNullableInt_Invalid_NonIntStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseNullableInt("random_string"));
        }
    }
}
