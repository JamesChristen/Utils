using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Common.Test.Extensions
{
    [TestClass]
    public class DictionaryExtensionTests
    {
        #region ToString

        [TestMethod]
        public void Test_ToString_NullDictionaryThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => ((Dictionary<int, int>)null).ToString(':', ';'));
        }

        [TestMethod]
        public void Test_ToString_EmptyDictionaryReturnsEmptyString()
        {
            Assert.AreEqual(string.Empty, new Dictionary<string, string>().ToString(':', ';'));
        }

        [TestMethod]
        public void Test_ToString_SingleValueReturnsExpected()
        {
            Assert.AreEqual("KEY:VALUE", new Dictionary<string, string> { { "KEY", "VALUE" } }.ToString(':', ';'));
        }

        [TestMethod]
        public void Test_ToString_MultipleValuesJoinsWithPairSeparator()
        {
            Assert.AreEqual("KEY1:VALUE1;KEY2:VALUE2", new Dictionary<string, string> { { "KEY1", "VALUE1" }, { "KEY2", "VALUE2" } }.ToString(':', ';'));
        }

        #endregion

        #region ContentEquals

        [TestMethod]
        public void Test_ContentEquals_SameValuesReturnsTrue()
        {
            Dictionary<string, int> dict1 = new Dictionary<string, int> { { "ASD", 1 }, { "QWE", 2 } };
            Dictionary<string, int> dict2 = new Dictionary<string, int> { { "ASD", 1 }, { "QWE", 2 } };
            Assert.IsTrue(dict1.ContentEquals(dict2));
        }

        [TestMethod]
        public void Test_ContentEquals_DifferentValuesReturnsFalse()
        {
            Dictionary<string, int> dict1 = new Dictionary<string, int> { { "ASD", 1 }, { "QWE", 2 } };
            Dictionary<string, int> dict2 = new Dictionary<string, int> { { "ASD", 2 }, { "QWE", 2} };
            Assert.IsFalse(dict1.ContentEquals(dict2));
        }

        [TestMethod]
        public void Test_ContentEquals_EmptyDictionariesReturnsTrue()
        {
            Dictionary<string, int> dict1 = new Dictionary<string, int>();
            Dictionary<string, int> dict2 = new Dictionary<string, int>();
            Assert.IsTrue(dict1.ContentEquals(dict2));
        }

        [TestMethod]
        public void Test_ContentEquals_SameDictionaryInstanceReturnsTrue()
        {
            Dictionary<string, int> dict1 = new Dictionary<string, int> { { "ASD", 1 }, { "QWE", 2 } };
            Assert.IsTrue(dict1.ContentEquals(dict1));
        }

        [TestMethod]
        public void Test_ContentEquals_NullDictionariesReturnsTrue()
        {
            Dictionary<string, int> dict1 = null;
            Dictionary<string, int> dict2 = null;
            Assert.IsTrue(dict1.ContentEquals(dict2));
        }

        [TestMethod]
        public void Test_ContentEquals_NullDictionaryContentEqualsEmptyDictionaryReturnsTrue()
        {
            Dictionary<string, int> dict1 = null;
            Dictionary<string, int> dict2 = new Dictionary<string, int>();
            Assert.IsTrue(dict1.ContentEquals(dict2));
        }

        [TestMethod]
        public void Test_ContentEquals_EmptyDictionaryContentEqualsNullDictionaryReturnsTrue()
        {
            Dictionary<string, int> dict1 = new Dictionary<string, int>();
            Dictionary<string, int> dict2 = null;
            Assert.IsTrue(dict1.ContentEquals(dict2));
        }

        #endregion

        #region ContentContains

        [TestMethod]
        public void Test_ContentContains_SameValuesReturnsTrue()
        {
            Dictionary<string, int> dict1 = new Dictionary<string, int> { { "ASD", 1 }, { "QWE", 2 } };
            Dictionary<string, int> dict2 = new Dictionary<string, int> { { "ASD", 1 }, { "QWE", 2 } };
            Assert.IsTrue(dict1.ContentContains(dict2));
        }

        [TestMethod]
        public void Test_ContentContains_DifferentValuesReturnsFalse()
        {
            Dictionary<string, int> dict1 = new Dictionary<string, int> { { "ASD", 1 }, { "QWE", 2 } };
            Dictionary<string, int> dict2 = new Dictionary<string, int> { { "ASD", 2 }, { "QWE", 2 } };
            Assert.IsFalse(dict1.ContentContains(dict2));
        }

        [TestMethod]
        public void Test_ContentContains_EmptyDictionariesReturnsTrue()
        {
            Dictionary<string, int> dict1 = new Dictionary<string, int>();
            Dictionary<string, int> dict2 = new Dictionary<string, int>();
            Assert.IsTrue(dict1.ContentContains(dict2));
        }

        [TestMethod]
        public void Test_ContentContains_SameDictionaryInstanceReturnsTrue()
        {
            Dictionary<string, int> dict1 = new Dictionary<string, int> { { "ASD", 1 }, { "QWE", 2 } };
            Assert.IsTrue(dict1.ContentContains(dict1));
        }

        [TestMethod]
        public void Test_ContentContains_NullDictionariesReturnsTrue()
        {
            Dictionary<string, int> dict1 = null;
            Dictionary<string, int> dict2 = null;
            Assert.IsTrue(dict1.ContentContains(dict2));
        }

        [TestMethod]
        public void Test_ContentContains_NullDictionaryContentContainsEmptyDictionaryReturnsTrue()
        {
            Dictionary<string, int> dict1 = null;
            Dictionary<string, int> dict2 = new Dictionary<string, int>();
            Assert.IsTrue(dict1.ContentContains(dict2));
        }

        [TestMethod]
        public void Test_ContentContains_EmptyDictionaryContentContainsNullDictionaryReturnsTrue()
        {
            Dictionary<string, int> dict1 = new Dictionary<string, int>();
            Dictionary<string, int> dict2 = null;
            Assert.IsTrue(dict1.ContentContains(dict2));
        }

        [TestMethod]
        public void Test_ContentContains_SecondDictionaryIsSubsetReturnsTrue()
        {
            Dictionary<string, int> dict1 = new Dictionary<string, int> { { "ASD", 1 }, { "QWE", 2 } };
            Dictionary<string, int> dict2 = new Dictionary<string, int> { { "ASD", 1 } };
            Assert.IsTrue(dict1.ContentContains(dict2));
        }

        [TestMethod]
        public void Test_ContentContains_SecondDictionaryIsSupersetReturnsFalse()
        {
            Dictionary<string, int> dict1 = new Dictionary<string, int> { { "ASD", 1 } };
            Dictionary<string, int> dict2 = new Dictionary<string, int> { { "ASD", 1 }, { "QWE", 2 } };
            Assert.IsFalse(dict1.ContentContains(dict2));
        }

        #endregion
    }
}
