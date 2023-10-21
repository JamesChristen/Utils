using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Common.Test.Extensions
{
    [TestClass]
    public class IsOneOfExtensionTests
    {
        #region DefaultEquals

        [TestMethod]
        public void Test_DefaultEquals_NullArgumentThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => ((int?)null).IsOneOf(1, 2, 3));
        }

        [TestMethod]
        public void Test_DefaultEquals_ContainsReturnsTrue()
        {
            Assert.IsTrue(1.IsOneOf(1, 2, 3));
        }

        [TestMethod]
        public void Test_DefaultEquals_DoesNotContainReturnsFalse()
        {
            Assert.IsFalse(1.IsOneOf(4, 5, 6));
        }

        [TestMethod]
        public void Test_DefaultEquals_EmptyOptionsReturnsFalse()
        {
            Assert.IsFalse(1.IsOneOf());
        }

        [TestMethod]
        public void Test_DefaultEquals_Array_ContainsReturnsTrue()
        {
            Assert.IsTrue(1.IsOneOf(new int[] { 1, 2, 3 }));
        }

        [TestMethod]
        public void Test_DefaultEquals_Array_DoesNotContainReturnsFalse()
        {
            Assert.IsFalse(1.IsOneOf(new int[] { 4, 5, 6 }));
        }

        [TestMethod]
        public void Test_DefaultEquals_Array_EmptyOptionsReturnsFalse()
        {
            Assert.IsFalse(1.IsOneOf(new int[0]));
        }

        [TestMethod]
        public void Test_DefaultEquals_Array_NullArrayReturnsFalse()
        {
            Assert.IsFalse(1.IsOneOf((int[])null));
        }

        private class TestClass
        {
            public int ASD { get; set; }

            public TestClass(int asd) => ASD = asd;

            public override bool Equals(object obj) => obj is TestClass tc && tc.ASD == ASD;

            public override int GetHashCode() => ASD;
        }

        [TestMethod]
        public void Test_DefaultEquals_CustomClass_ContainsReturnsTrue()
        {
            Assert.IsTrue(new TestClass(1).IsOneOf(new TestClass(1), new TestClass(2), new TestClass(3)));
        }

        [TestMethod]
        public void Test_DefaultEquals_CustomClass_DoesNotContainReturnsFalse()
        {
            Assert.IsFalse(new TestClass(1).IsOneOf(new TestClass(4), new TestClass(5), new TestClass(6)));
        }

        [TestMethod]
        public void Test_DefaultEquals_CustomClass_EmptyOptionsReturnsFalse()
        {
            Assert.IsFalse(new TestClass(1).IsOneOf());
        }

        [TestMethod]
        public void Test_DefaultEquals_CustomClass_Array_ContainsReturnsTrue()
        {
            Assert.IsTrue(new TestClass(1).IsOneOf(new TestClass[] { new TestClass(1), new TestClass(2), new TestClass(3) }));
        }

        [TestMethod]
        public void Test_DefaultEquals_CustomClass_Array_DoesNotContainReturnsFalse()
        {
            Assert.IsFalse(new TestClass(1).IsOneOf(new TestClass[] { new TestClass(4), new TestClass(5), new TestClass(6) }));
        }

        [TestMethod]
        public void Test_DefaultEquals_CustomClass_Array_EmptyOptionsReturnsFalse()
        {
            Assert.IsFalse(new TestClass(1).IsOneOf(new TestClass[0]));
        }

        #endregion

        #region CustomComparer

        private class TestClassComparer : IEqualityComparer<TestClass>
        {
            public bool Equals([AllowNull] TestClass x, [AllowNull] TestClass y) => x != null && y != null && x.ASD == y.ASD;

            public int GetHashCode([DisallowNull] TestClass obj) => obj.ASD;
        }

        [TestMethod]
        public void Test_CustomComparer_NullArgumentThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => ((TestClass)null).IsOneOf(new TestClassComparer()));
        }

        [TestMethod]
        public void Test_CustomComparer_Array_NullArrayReturnsFalse()
        {
            Assert.IsFalse(new TestClass(1).IsOneOf(new TestClassComparer(), null));
        }

        [TestMethod]
        public void Test_CustomComparer_CustomClass_ContainsReturnsTrue()
        {
            Assert.IsTrue(new TestClass(1).IsOneOf(new TestClassComparer(), new TestClass(1), new TestClass(2), new TestClass(3)));
        }

        [TestMethod]
        public void Test_CustomComparer_CustomClass_DoesNotContainReturnsFalse()
        {
            Assert.IsFalse(new TestClass(1).IsOneOf(new TestClassComparer(), new TestClass(4), new TestClass(5), new TestClass(6)));
        }

        [TestMethod]
        public void Test_CustomComparer_CustomClass_EmptyOptionsReturnsFalse()
        {
            Assert.IsFalse(new TestClass(1).IsOneOf(new TestClassComparer()));
        }

        [TestMethod]
        public void Test_CustomComparer_CustomClass_Array_ContainsReturnsTrue()
        {
            Assert.IsTrue(new TestClass(1).IsOneOf(new TestClassComparer(), new TestClass[] { new TestClass(1), new TestClass(2), new TestClass(3) }));
        }

        [TestMethod]
        public void Test_CustomComparer_CustomClass_Array_DoesNotContainReturnsFalse()
        {
            Assert.IsFalse(new TestClass(1).IsOneOf(new TestClassComparer(), new TestClass[] { new TestClass(4), new TestClass(5), new TestClass(6) }));
        }

        [TestMethod]
        public void Test_CustomComparer_CustomClass_Array_EmptyOptionsReturnsFalse()
        {
            Assert.IsFalse(new TestClass(1).IsOneOf(new TestClassComparer(), new TestClass[0]));
        }

        #endregion
    }
}
