using Common.CSV;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Common.Test.CSV.ValueParsingTests
{
    [TestClass]
    public class ValueParserTests
    {
        private enum Foo { ASD, QWE, ZXC }

        [TestMethod]
        public void Test_GetParsedValue_String()
        {
            string str = "test";
            string result = ValueParsers.GetParsedValue<string>(str);
            Assert.AreEqual(str, result);
        }

        [TestMethod]
        public void Test_GetParsedValue_String_NullReturnsNull()
        {
            Assert.AreEqual(null, ValueParsers.GetParsedValue<string>(null));
        }

        [TestMethod]
        public void Test_GetParsedValue_Bool()
        {
            Assert.AreEqual(true, ValueParsers.GetParsedValue<bool>("true"));
        }

        [TestMethod]
        public void Test_GetParsedValue_NullableBool()
        {
            Assert.AreEqual(true, ValueParsers.GetParsedValue<bool?>("true"));
            Assert.AreEqual(null, ValueParsers.GetParsedValue<bool?>(string.Empty));
        }

        [TestMethod]
        public void Test_GetParsedValue_Byte()
        {
            Assert.AreEqual((byte)1, ValueParsers.GetParsedValue<byte>("1"));
        }

        [TestMethod]
        public void Test_GetParsedValue_NullableByte()
        {
            Assert.AreEqual((byte)1, ValueParsers.GetParsedValue<byte?>("1"));
            Assert.AreEqual(null, ValueParsers.GetParsedValue<byte?>(string.Empty));
        }

        [TestMethod]
        public void Test_GetParsedValue_Char()
        {
            Assert.AreEqual('a', ValueParsers.GetParsedValue<char>("a"));
        }

        [TestMethod]
        public void Test_GetParsedValue_NullableChar()
        {
            Assert.AreEqual('a', ValueParsers.GetParsedValue<char?>("a"));
            Assert.AreEqual(null, ValueParsers.GetParsedValue<char?>(string.Empty));
        }

        [TestMethod]
        public void Test_GetParsedValue_DateTime()
        {
            Assert.AreEqual(new DateTime(2021, 9, 1), ValueParsers.GetParsedValue<DateTime>("2021-09-01"));
        }

        [TestMethod]
        public void Test_GetParsedValue_NullableDateTime()
        {
            Assert.AreEqual(new DateTime(2021, 9, 1), ValueParsers.GetParsedValue<DateTime?>("2021-09-01"));
            Assert.AreEqual(null, ValueParsers.GetParsedValue<DateTime?>(string.Empty));
        }

        [TestMethod]
        public void Test_GetParsedValue_Decimal()
        {
            Assert.AreEqual(1.23M, ValueParsers.GetParsedValue<decimal>("1.23"));
        }

        [TestMethod]
        public void Test_GetParsedValue_NullableDecimal()
        {
            Assert.AreEqual(1.23M, ValueParsers.GetParsedValue<decimal?>("1.23"));
            Assert.AreEqual(null, ValueParsers.GetParsedValue<decimal?>(string.Empty));
        }

        [TestMethod]
        public void Test_GetParsedValue_Double()
        {
            Assert.AreEqual(1.23d, ValueParsers.GetParsedValue<double>("1.23"));
        }

        [TestMethod]
        public void Test_GetParsedValue_NullableDouble()
        {
            Assert.AreEqual(1.23d, ValueParsers.GetParsedValue<double?>("1.23"));
            Assert.AreEqual(null, ValueParsers.GetParsedValue<double?>(string.Empty));
        }

        [TestMethod]
        public void Test_GetParsedValue_Float()
        {
            Assert.AreEqual(1.23f, ValueParsers.GetParsedValue<float>("1.23"));
        }

        [TestMethod]
        public void Test_GetParsedValue_NullableFloat()
        {
            Assert.AreEqual(1.23f, ValueParsers.GetParsedValue<float?>("1.23"));
            Assert.AreEqual(null, ValueParsers.GetParsedValue<float?>(string.Empty));
        }

        [TestMethod]
        public void Test_GetParsedValue_Int()
        {
            Assert.AreEqual(1, ValueParsers.GetParsedValue<int>("1"));
        }

        [TestMethod]
        public void Test_GetParsedValue_NullableInt()
        {
            Assert.AreEqual(1, ValueParsers.GetParsedValue<int?>("1"));
            Assert.AreEqual(null, ValueParsers.GetParsedValue<int?>(string.Empty));
        }

        [TestMethod]
        public void Test_GetParsedValue_Long()
        {
            Assert.AreEqual(123456789L, ValueParsers.GetParsedValue<long>("123456789"));
        }

        [TestMethod]
        public void Test_GetParsedValue_NullableLong()
        {
            Assert.AreEqual(123456789, ValueParsers.GetParsedValue<long?>("123456789"));
            Assert.AreEqual(null, ValueParsers.GetParsedValue<long?>(string.Empty));
        }

        [TestMethod]
        public void Test_GetParsedValue_Short()
        {
            Assert.AreEqual((short)1, ValueParsers.GetParsedValue<short>("1"));
        }

        [TestMethod]
        public void Test_GetParsedValue_NullableShort()
        {
            Assert.AreEqual((short)1, ValueParsers.GetParsedValue<short?>("1"));
            Assert.AreEqual(null, ValueParsers.GetParsedValue<short?>(string.Empty));
        }

        [TestMethod]
        public void Test_GetParsedValue_EnumThrowsException()
        {
            Assert.ThrowsException<NotImplementedException>(() => ValueParsers.GetParsedValue<Foo>("ASD"));
        }

        [TestMethod]
        public void Test_GetParsedValue_UnknownTypeThrowsException()
        {
            Assert.ThrowsException<NotImplementedException>(() => ValueParsers.GetParsedValue<ValueParserTests>("ASD"));
        }

        [TestMethod]
        public void Test_Dynamic_GetParsedValue_Bool()
        {
            Assert.AreEqual(true, ValueParsers.GetParsedValue("true", typeof(bool)));
        }

        [TestMethod]
        public void Test_Dynamic_GetParsedValue_NullableBool()
        {
            Assert.AreEqual(true, ValueParsers.GetParsedValue("true", typeof(bool?)));
            Assert.AreEqual((bool?)null, ValueParsers.GetParsedValue(string.Empty, typeof(bool?)));
        }

        [TestMethod]
        public void Test_Dynamic_GetParsedValue_Byte()
        {
            Assert.AreEqual((byte)1, ValueParsers.GetParsedValue("1", typeof(byte)));
        }

        [TestMethod]
        public void Test_Dynamic_GetParsedValue_NullableByte()
        {
            Assert.AreEqual((byte)1, ValueParsers.GetParsedValue("1", typeof(byte?)));
            Assert.AreEqual((byte?)null, ValueParsers.GetParsedValue(string.Empty, typeof(byte?)));
        }

        [TestMethod]
        public void Test_Dynamic_GetParsedValue_Char()
        {
            Assert.AreEqual('a', ValueParsers.GetParsedValue("a", typeof(char)));
        }

        [TestMethod]
        public void Test_Dynamic_GetParsedValue_NullableChar()
        {
            Assert.AreEqual('a', ValueParsers.GetParsedValue("a", typeof(char?)));
            Assert.AreEqual((char?)null, ValueParsers.GetParsedValue(string.Empty, typeof(char?)));
        }

        [TestMethod]
        public void Test_Dynamic_GetParsedValue_DateTime()
        {
            Assert.AreEqual(new DateTime(2021, 9, 1), ValueParsers.GetParsedValue("2021-09-01", typeof(DateTime)));
        }

        [TestMethod]
        public void Test_Dynamic_GetParsedValue_NullableDateTime()
        {
            Assert.AreEqual(new DateTime(2021, 9, 1), ValueParsers.GetParsedValue("2021-09-01", typeof(DateTime?)));
            Assert.AreEqual((DateTime?)null, ValueParsers.GetParsedValue(string.Empty, typeof(DateTime?)));
        }

        [TestMethod]
        public void Test_Dynamic_GetParsedValue_Decimal()
        {
            Assert.AreEqual(1.23M, ValueParsers.GetParsedValue("1.23", typeof(decimal)));
        }

        [TestMethod]
        public void Test_Dynamic_GetParsedValue_NullableDecimal()
        {
            Assert.AreEqual(1.23M, ValueParsers.GetParsedValue("1.23", typeof(decimal?)));
            Assert.AreEqual((decimal?)null, ValueParsers.GetParsedValue(string.Empty, typeof(decimal?)));
        }

        [TestMethod]
        public void Test_Dynamic_GetParsedValue_Double()
        {
            Assert.AreEqual(1.23d, ValueParsers.GetParsedValue("1.23", typeof(double)));
        }

        [TestMethod]
        public void Test_Dynamic_GetParsedValue_NullableDouble()
        {
            Assert.AreEqual(1.23d, ValueParsers.GetParsedValue("1.23", typeof(double?)));
            Assert.AreEqual((double?)null, ValueParsers.GetParsedValue(string.Empty, typeof(double?)));
        }

        [TestMethod]
        public void Test_Dynamic_GetParsedValue_Float()
        {
            Assert.AreEqual(1.23f, ValueParsers.GetParsedValue("1.23", typeof(float)));
        }

        [TestMethod]
        public void Test_Dynamic_GetParsedValue_NullableFloat()
        {
            Assert.AreEqual(1.23f, ValueParsers.GetParsedValue("1.23", typeof(float?)));
            Assert.AreEqual((float?)null, ValueParsers.GetParsedValue(string.Empty, typeof(float?)));
        }

        [TestMethod]
        public void Test_Dynamic_GetParsedValue_Int()
        {
            Assert.AreEqual(1, ValueParsers.GetParsedValue("1", typeof(int)));
        }

        [TestMethod]
        public void Test_Dynamic_GetParsedValue_NullableInt()
        {
            Assert.AreEqual(1, ValueParsers.GetParsedValue("1", typeof(int?)));
            Assert.AreEqual((int?)null, ValueParsers.GetParsedValue(string.Empty, typeof(int?)));
        }

        [TestMethod]
        public void Test_Dynamic_GetParsedValue_Long()
        {
            Assert.AreEqual(123456789L, ValueParsers.GetParsedValue("123456789", typeof(long)));
        }

        [TestMethod]
        public void Test_Dynamic_GetParsedValue_NullableLong()
        {
            Assert.AreEqual(123456789, ValueParsers.GetParsedValue("123456789", typeof(long?)));
            Assert.AreEqual((long?)null, ValueParsers.GetParsedValue(string.Empty, typeof(long?)));
        }

        [TestMethod]
        public void Test_Dynamic_GetParsedValue_Short()
        {
            Assert.AreEqual((short)1, ValueParsers.GetParsedValue("1", typeof(short)));
        }

        [TestMethod]
        public void Test_Dynamic_GetParsedValue_NullableShort()
        {
            Assert.AreEqual((short)1, ValueParsers.GetParsedValue("1", typeof(short?)));
            Assert.AreEqual((short?)null, ValueParsers.GetParsedValue(string.Empty, typeof(short?)));
        }

        [TestMethod]
        public void Test_Dynamic_GetParsedValue_EnumThrowsException()
        {
            Assert.ThrowsException<NotImplementedException>(() => ValueParsers.GetParsedValue("ASD", typeof(Foo)));
        }

        [TestMethod]
        public void Test_Dynamic_GetParsedValue_UnknownTypeThrowsException()
        {
            Assert.ThrowsException<NotImplementedException>(() => ValueParsers.GetParsedValue("ASD", typeof(ValueParserTests)));
        }
    }
}
