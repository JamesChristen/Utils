using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Test
{
    [TestClass]
    public class EnumHelpersTests
    {
        private enum TestEnum
        {
            ASD, QWE, ZXC
        }

        [TestMethod]
        public void Test_All_ReturnsAll()
        {
            HashSet<TestEnum> values = EnumHelpers.All<TestEnum>().ToHashSet();
            HashSet<TestEnum> expected = new TestEnum[] { TestEnum.ASD, TestEnum.QWE, TestEnum.ZXC }.ToHashSet();
            Assert.IsTrue(values.SetEquals(expected));
        }

        [TestMethod]
        public void Test_IsDefined_DefinedValueReturnsTrue()
        {
            Assert.IsTrue(EnumHelpers.IsDefined<TestEnum>("ASD"));
        }

        [TestMethod]
        public void Test_IsDefined_DefinedValueReturnsTrue_CaseInvariant()
        {
            Assert.IsTrue(EnumHelpers.IsDefined<TestEnum>("aSd", ignoreCase: true));
        }

        [TestMethod]
        public void Test_IsDefined_UndefinedValueReturnsFalse()
        {
            Assert.IsFalse(EnumHelpers.IsDefined<TestEnum>("UNDEFINED"));
        }

        [TestMethod]
        public void Test_IsDefined_UndefinedValueReturnsFalse_CaseSensitive()
        {
            Assert.IsFalse(EnumHelpers.IsDefined<TestEnum>("aSD", ignoreCase: false));
        }

        [TestMethod]
        public void Test_Parse_DefinedValueReturnsEnum()
        {
            Assert.AreEqual(TestEnum.ASD, EnumHelpers.Parse<TestEnum>("ASD"));
        }

        [TestMethod]
        public void Test_Parse_DefinedValueReturnsEnum_CaseInvariant()
        {
            Assert.AreEqual(TestEnum.ASD, EnumHelpers.Parse<TestEnum>("aSd", ignoreCase: true));
        }

        [TestMethod]
        public void Test_Parse_UndefinedValueThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => EnumHelpers.Parse<TestEnum>("UNDEFINED"));
        }

        [TestMethod]
        public void Test_Parse_UndefinedValueThrowsArgumentException_CaseSensitive()
        {
            Assert.ThrowsException<ArgumentException>(() => EnumHelpers.Parse<TestEnum>("asD", ignoreCase: false));
        }

        [TestMethod]
        public void Test_ParseNullable_DefinedValueReturnsEnum()
        {
            Assert.AreEqual(TestEnum.ASD, EnumHelpers.ParseNullable<TestEnum>("ASD"));
        }

        [TestMethod]
        public void Test_ParseNullable_DefinedValueReturnsEnum_CaseInvariant()
        {
            Assert.AreEqual(TestEnum.ASD, EnumHelpers.ParseNullable<TestEnum>("aSd", ignoreCase: true));
        }

        [TestMethod]
        public void Test_ParseNullable_UndefinedValueReturnsNull()
        {
            Assert.AreEqual(null, EnumHelpers.ParseNullable<TestEnum>("UNDEFINED"));
        }

        [TestMethod]
        public void Test_ParseNullable_UndefinedValueReturnsNull_CaseSensitive()
        {
            Assert.AreEqual(null, EnumHelpers.ParseNullable<TestEnum>("UNDEFINED", ignoreCase: false));
        }
    }
}
