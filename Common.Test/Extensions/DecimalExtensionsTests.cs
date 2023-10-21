using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Common.Test.Extensions
{
    [TestClass]
    public class DecimalExtensionsTests
    {
        [TestMethod]
        public void Test_EqualsWithError_ExactReturnsTrue()
        {
            Assert.IsTrue(1.23456789M.EqualsWithError(1.23456789M));
        }

        [TestMethod]
        public void Test_EqualsWithError_WithinErrorReturnsTrue()
        {
            Assert.IsTrue(1.234M.EqualsWithError(1.235M, margin: 0.1M));
        }

        [TestMethod]
        public void Test_EqualsWithError_ExactlyOnErrorReturnsTrue()
        {
            Assert.IsTrue(1.234M.EqualsWithError(1.235M, 0.001M));
        }

        [TestMethod]
        public void Test_EqualsWithError_OutsideErrorReturnsFalse()
        {
            Assert.IsFalse(1.23456789M.EqualsWithError(decimal.MaxValue));
        }

        [TestMethod]
        public void Test_EqualsWithError_NegativeMarginThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => 1.23M.EqualsWithError(1.24M, margin: -1M));
        }
    }
}
