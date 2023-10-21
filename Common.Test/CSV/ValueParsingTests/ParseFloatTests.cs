using Common.CSV;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Common.Test.CSV.ValueParsingTests
{
    [TestClass]
    public class ParseFloatTests
    {
        [TestMethod]
        public void Test_ParseFloat_Valid_AsString()
        {
            Assert.AreEqual(1.23f, ValueParsers.ParseFloat("1.23"));
        }

        [TestMethod]
        public void Test_ParseFloat_Invalid_NonFloatStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseFloat("random_string"));
        }

        [TestMethod]
        public void Test_ParseFloat_Invalid_NullStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseFloat(null));
        }

        [TestMethod]
        public void Test_ParseFloat_Invalid_EmptyStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseFloat(string.Empty));
        }

        [TestMethod]
        public void Test_ParseFloat_Invalid_WhitespaceStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseFloat("    "));
        }

        [TestMethod]
        public void Test_ParseNullableFloat_Valid_AsString()
        {
            Assert.AreEqual(1.23f, ValueParsers.ParseNullableFloat("1.23"));
        }

        [TestMethod]
        public void Test_ParseNullableFloat_Valid_NullStringReturnsNull()
        {
            Assert.AreEqual(null, ValueParsers.ParseNullableFloat(null));
        }

        [TestMethod]
        public void Test_ParseNullableFloat_Valid_EmptyStringReturnsNull()
        {
            Assert.AreEqual(null, ValueParsers.ParseNullableFloat(string.Empty));
        }

        [TestMethod]
        public void Test_ParseNullableFloat_Valid_WhitespaceStringReturnsNull()
        {
            Assert.AreEqual(null, ValueParsers.ParseNullableFloat("    "));
        }

        [TestMethod]
        public void Test_ParseNullableFloat_Invalid_NonFloatStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseNullableFloat("random_string"));
        }
    }
}
