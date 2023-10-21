using Common.CSV;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Common.Test.CSV.ValueParsingTests
{
    [TestClass]
    public class ParseEnumTests
    {
        private enum Foo { ASD, QWE, ZXC }

        [TestMethod]
        public void Test_ParseEnum_Valid_AsString()
        {
            Assert.AreEqual(Foo.ASD, ValueParsers.ParseEnum<Foo>("ASD"));
        }

        [TestMethod]
        public void Test_ParseEnum_Valid_AsInt()
        {
            Assert.AreEqual(Foo.ASD, ValueParsers.ParseEnum<Foo>(((int)Foo.ASD).ToString()));
        }

        [TestMethod]
        public void Test_ParseEnum_Invalid_UndefinedValueThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseEnum<Foo>("UNDEFINED"));
        }

        [TestMethod]
        public void Test_ParseEnum_Invalid_NullStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseEnum<Foo>(null));
        }

        [TestMethod]
        public void Test_ParseEnum_Invalid_EmptyStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseEnum<Foo>(string.Empty));
        }

        [TestMethod]
        public void Test_ParseEnum_Invalid_WhitespaceStringThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseEnum<Foo>("    "));
        }

        [TestMethod]
        public void Test_ParseNullableEnum_Valid_AsString()
        {
            Assert.AreEqual(Foo.ASD, ValueParsers.ParseNullableEnum<Foo>("ASD"));
        }

        [TestMethod]
        public void Test_ParseNullableEnum_Valid_AsInt()
        {
            Assert.AreEqual(Foo.ASD, ValueParsers.ParseNullableEnum<Foo>(((int)Foo.ASD).ToString()));
        }

        [TestMethod]
        public void Test_ParseNullableEnum_Invalid_UndefinedValueThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => ValueParsers.ParseNullableEnum<Foo>("UNDEFINED"));
        }

        [TestMethod]
        public void Test_ParseNullableEnum_Valid_NullStringReturnsNull()
        {
            Assert.AreEqual(null, ValueParsers.ParseNullableEnum<Foo>(null));
        }

        [TestMethod]
        public void Test_ParseNullableEnum_Invalid_EmptyStringReturnsNull()
        {
            Assert.AreEqual(null, ValueParsers.ParseNullableEnum<Foo>(string.Empty));
        }

        [TestMethod]
        public void Test_ParseNullableEnum_Invalid_WhitespaceStringReturnsNull()
        {
            Assert.AreEqual(null, ValueParsers.ParseNullableEnum<Foo>("    "));
        }
    }
}
