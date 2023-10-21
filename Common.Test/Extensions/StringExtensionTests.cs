using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Common.Test.Extensions
{
    [TestClass]
    public class StringExtensionTests
    {
        #region RemoveWhiteSpace

        [TestMethod]
        public void Test_RemoveWhiteSpace_RemovesWhiteSpace()
        {
            string str = "ASD\n\r\t";
            string clean = str.RemoveWhiteSpace();
            Assert.AreEqual("ASD", clean);
        }

        [TestMethod]
        public void Test_RemoveWhiteSpace_NullReturnsNull()
        {
            string str = null;
            string clean = str.RemoveWhiteSpace();
            Assert.IsNull(clean);
        }

        [TestMethod]
        public void Test_RemoveWhiteSpace_EmptyReturnsEmpty()
        {
            string str = string.Empty;
            string clean = str.RemoveWhiteSpace();
            Assert.AreEqual(string.Empty, clean);
        }

        #endregion

        #region SqlClean

        [TestMethod]
        public void Test_SqlClean_CleansString()
        {
            string str = "TEST_NAME0123456789\n\r\t`¬!\"£$%^&*()-=+[{]};:'@#~,<.>/?\\| ";
            string clean = str.SqlClean();
            Assert.AreEqual("TEST_NAME0123456789", clean);
        }

        [TestMethod]
        public void Test_SqlClean_NullReturnsNull()
        {
            string str = null;
            string clean = str.SqlClean();
            Assert.AreEqual(null, clean);
        }

        [TestMethod]
        public void Test_SqlClean_EmptyReturnsEmpty()
        {
            string str = string.Empty;
            string clean = str.SqlClean();
            Assert.AreEqual(string.Empty, clean);
        }

        #endregion
    }
}
