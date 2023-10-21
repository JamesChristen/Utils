using Common.Sequences;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Common.Test.Sequences
{
    [TestClass]
    public class WindowLengthTests
    {
        [TestMethod]
        public void Test_Constructor_InfReturnsInfinite()
        {
            WindowLength len = WindowLength.Infinite;
            Assert.AreEqual(true, len.IsInfinite);
            Assert.AreEqual(0M, len.Length);
        }

        [TestMethod]
        public void Test_Constructor_DecimalReturnsLength()
        {
            WindowLength len = new WindowLength(123);
            Assert.AreEqual(false, len.IsInfinite);
            Assert.AreEqual(123, len.Length);
        }

        [TestMethod]
        public void Test_Constructor_ZeroLengthThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => new WindowLength(0));
        }

        [TestMethod]
        public void Test_Constructor_NegativeLengthThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => new WindowLength(-123));
        }

        [TestMethod]
        public void Test_StaticImplicitOperator_FromString_InfReturnsInfinite()
        {
            string str = "Inf";
            WindowLength len = str;
            Assert.AreEqual(true, len.IsInfinite);
            Assert.AreEqual(0M, len.Length);
        }

        [TestMethod]
        public void Test_StaticImplicitOperator_FromString_InfReturnsInfinite_CaseInvariant()
        {
            string str = "iNf";
            WindowLength len = str;
            Assert.AreEqual(true, len.IsInfinite);
            Assert.AreEqual(0M, len.Length);
        }

        [TestMethod]
        public void Test_StaticImplicitOperator_FromString_IntReturnsLength()
        {
            string str = "123";
            WindowLength len = str;
            Assert.AreEqual(false, len.IsInfinite);
            Assert.AreEqual(123, len.Length);
        }

        [TestMethod]
        public void Test_StaticImplicitOperator_FromString_ZeroLengthThrowsArgumentException()
        {
            string str = "0";
            Assert.ThrowsException<ArgumentException>(() => { WindowLength len = str; });
        }

        [TestMethod]
        public void Test_StaticImplicitOperator_FromString_NegativeLengthThrowsValidationException()
        {
            string str = "-10";
            Assert.ThrowsException<ArgumentException>(() => { WindowLength len = str; });
        }

        [TestMethod]
        public void Test_StaticImplicitOperator_ToString_ResultEqualsToString()
        {
            WindowLength len = new WindowLength(123);
            Assert.AreEqual("123", len.ToString());

            WindowLength inf = WindowLength.Infinite;
            Assert.AreEqual("Inf", inf.ToString());
        }

        [TestMethod]
        public void Test_CanParse_InfReturnsTrue()
        {
            bool result = WindowLength.CanParse("Inf", out WindowLength len);
            Assert.IsTrue(result);
            Assert.AreEqual(true, len.IsInfinite);
            Assert.AreEqual(0M, len.Length);
        }

        [TestMethod]
        public void Test_CanParse_InfReturnsTrue_CaseInvariant()
        {
            bool result = WindowLength.CanParse("iNf", out WindowLength len);
            Assert.IsTrue(result);
            Assert.AreEqual(true, len.IsInfinite);
            Assert.AreEqual(0M, len.Length);
        }

        [TestMethod]
        public void Test_CanParse_PositiveReturnsTrue()
        {
            bool result = WindowLength.CanParse("123", out WindowLength len);
            Assert.IsTrue(result);
            Assert.AreEqual(false, len.IsInfinite);
            Assert.AreEqual(123, len.Length);
        }

        [TestMethod]
        public void Test_CanParse_ZeroReturnsFalse()
        {
            bool result = WindowLength.CanParse("0", out _);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Test_CanParse_NegativeReturnsFalse()
        {
            bool result = WindowLength.CanParse("-123", out _);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Test_CanParse_NonIntegerReturnsFalse()
        {
            bool result = WindowLength.CanParse("123.456", out _);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Test_CanParse_NotInfOrDecimalReturnsFalse()
        {
            bool result = WindowLength.CanParse("something else", out _);
            Assert.IsFalse(result);
        }
    }
}
