using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Common.Test.Extensions
{
    [TestClass]
    public class EnumExtensionsTests
    {
        private sealed class TestAttribute : Attribute
        {
            public string Name { get; set; }

            public TestAttribute(string name)
            {
                Name = name;
            }
        }

        private enum TestEnum
        {
            [Test("Name")]
            ASD,

            QWE
        }

        [TestMethod]
        public void Test_GetAttributeOfType_ReturnsAttribute()
        {
            TestAttribute att = TestEnum.ASD.GetAttributeOfType<TestAttribute>();
            Assert.IsNotNull(att);
            Assert.AreEqual("Name", att.Name);
        }

        [TestMethod]
        public void Test_GetAttributeOfType_NoAttributeReturnsNull()
        {
            TestAttribute att = TestEnum.QWE.GetAttributeOfType<TestAttribute>();
            Assert.IsNull(att);
        }

        [TestMethod]
        public void Test_ToLongString()
        {
            Assert.AreEqual("TestEnum.ASD", TestEnum.ASD.ToLongString());
        }
    }
}
