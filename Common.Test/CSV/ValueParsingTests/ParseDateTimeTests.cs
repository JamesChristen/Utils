using Common.CSV;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Common.Test.CSV.ValueParsingTests
{
    [TestClass]
    public class ParseDateTimeTests
    {
        [TestMethod]
        public void Test_ParseDateTime_Valid_AsString()
        {
            Assert.AreEqual(new DateTime(2021, 9, 1), ValueParsers.ParseDateTime("2021-09-01"));
        }

        [TestMethod]
        public void Test_ParseDateTime_Invalid_NonDateTimeStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseDateTime("random_string"));
        }

        [TestMethod]
        public void Test_ParseDateTime_Invalid_NullStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseDateTime(null));
        }

        [TestMethod]
        public void Test_ParseDateTime_Invalid_EmptyStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseDateTime(string.Empty));
        }

        [TestMethod]
        public void Test_ParseDateTime_Invalid_WhitespaceStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseDateTime("    "));
        }

        [TestMethod]
        public void Test_ParseNullableDateTime_Valid_AsString()
        {
            Assert.AreEqual(new DateTime(2021, 9, 1), ValueParsers.ParseNullableDateTime("2021-09-01"));
        }

        [TestMethod]
        public void Test_ParseNullableDateTime_Valid_NullStringReturnsNull()
        {
            Assert.AreEqual(null, ValueParsers.ParseNullableDateTime(null));
        }

        [TestMethod]
        public void Test_ParseNullableDateTime_Valid_EmptyStringReturnsNull()
        {
            Assert.AreEqual(null, ValueParsers.ParseNullableDateTime(string.Empty));
        }

        [TestMethod]
        public void Test_ParseNullableDateTime_Valid_WhitespaceStringReturnsNull()
        {
            Assert.AreEqual(null, ValueParsers.ParseNullableDateTime("    "));
        }

        [TestMethod]
        public void Test_ParseNullableDateTime_Invalid_NonDateTimeStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseNullableDateTime("random_string"));
        }
    }
}
