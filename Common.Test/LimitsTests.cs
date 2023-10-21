using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Common.Test
{
    [TestClass]
    public class LimitsTests
    {
        #region IsValid

        [TestMethod]
        public void Limits_IsValid_BothLimits_Valid()
        {
            Limits limits = new Limits(10, 2, true, true);
            Assert.IsTrue(limits.IsValid());
        }

        [TestMethod]
        public void Limits_IsValid_OnlyLower_Valid()
        {
            Limits limits = new Limits(null, 2, null, true);
            Assert.IsTrue(limits.IsValid());
        }

        [TestMethod]
        public void Limits_IsValid_OnlyUpper_Valid()
        {
            Limits limits = new Limits(10, null, true, null);
            Assert.IsTrue(limits.IsValid());
        }

        [TestMethod]
        public void Limits_IsValid_NoLimits_Valid()
        {
            Limits limits = new Limits(null, null, null, null);
            Assert.IsTrue(limits.IsValid());
        }

        [TestMethod]
        public void Limits_IsValid_UpperLimitButNullInclusion_NotValid()
        {
            Limits limits = new Limits(10, 2, null, true);
            Assert.IsFalse(limits.IsValid());
        }

        [TestMethod]
        public void Limits_IsValid_UpperInclusionButNullLimit_NotValid()
        {
            Limits limits = new Limits(null, 2, true, true);
            Assert.IsFalse(limits.IsValid());
        }

        [TestMethod]
        public void Limits_IsValid_LowerLimitButNullInclusion_NotValid()
        {
            Limits limits = new Limits(10, 2, true, null);
            Assert.IsFalse(limits.IsValid());
        }

        [TestMethod]
        public void Limits_IsValid_LowerInclusionButNullLimit_NotValid()
        {
            Limits limits = new Limits(10, null, true, true);
            Assert.IsFalse(limits.IsValid());
        }

        [TestMethod]
        public void Limits_IsValid_SameLimitsInclusion_Valid()
        {
            Limits limits = new Limits(10, 10, true, true);
            Assert.IsTrue(limits.IsValid());
        }

        [TestMethod]
        public void Limits_IsValid_SameLimitsExclusion_NotValid()
        {
            Limits limits = new Limits(10, 10, false, false);
            Assert.IsFalse(limits.IsValid());
        }

        [TestMethod]
        public void Limits_IsValid_SameLimitsMismatchingInclusion_NotValid()
        {
            Limits limits = new Limits(10, 10, true, false);
            Assert.IsFalse(limits.IsValid());
        }

        [TestMethod]
        public void Limits_IsValid_LowerLimitGreaterThanUpperLimit_NotValid()
        {
            Limits limits = new Limits(10, 100, true, true);
            Assert.IsFalse(limits.IsValid());
        }

        #endregion

        #region IsInRange

        [TestMethod]
        public void Limits_IsInRange_LowerAndUpperLimit_Inclusion_ValueInRange_ReturnsTrue()
        {
            Limits limits = new Limits(10, 2, true, true);
            Assert.IsTrue(limits.IsInRange(5));
        }

        [TestMethod]
        public void Limits_IsInRange_LowerAndUpperLimit_Inclusion_ValueOnUpperLimit_ReturnsTrue()
        {
            Limits limits = new Limits(10, 2, true, true);
            Assert.IsTrue(limits.IsInRange(10));
        }

        [TestMethod]
        public void Limits_IsInRange_LowerAndUpperLimit_Inclusion_ValueOnLowerLimit_ReturnsTrue()
        {
            Limits limits = new Limits(10, 2, true, true);
            Assert.IsTrue(limits.IsInRange(2));
        }

        [TestMethod]
        public void Limits_IsInRange_LowerAndUpperLimit_Inclusion_ValueOutOfRange_ReturnsFalse()
        {
            Limits limits = new Limits(10, 2, true, true);
            Assert.IsFalse(limits.IsInRange(11));
        }

        [TestMethod]
        public void Limits_IsInRange_UpperLimitOnly_Inclusion_ValueUnderLimit_ReturnsTrue()
        {
            Limits limits = new Limits(10, null, true, null);
            Assert.IsTrue(limits.IsInRange(5));
        }

        [TestMethod]
        public void Limits_IsInRange_UpperLimitOnly_Inclusion_ValueOnLimit_ReturnsTrue()
        {
            Limits limits = new Limits(10, null, true, null);
            Assert.IsTrue(limits.IsInRange(10));
        }

        [TestMethod]
        public void Limits_IsInRange_UpperLimitOnly_Inclusion_ValueOverLimit_ReturnsFalse()
        {
            Limits limits = new Limits(10, null, true, null);
            Assert.IsFalse(limits.IsInRange(11));
        }

        [TestMethod]
        public void Limits_IsInRange_UpperLimitOnly_Exclusion_ValueUnderLimit_ReturnsTrue()
        {
            Limits limits = new Limits(10, null, false, null);
            Assert.IsTrue(limits.IsInRange(5));
        }

        [TestMethod]
        public void Limits_IsInRange_UpperLimitOnly_Exclusion_ValueOnLimit_ReturnsFalse()
        {
            Limits limits = new Limits(10, null, false, null);
            Assert.IsFalse(limits.IsInRange(10));
        }

        [TestMethod]
        public void Limits_IsInRange_UpperLimitOnly_Exclusion_ValueOverLimit_ReturnsFalse()
        {
            Limits limits = new Limits(10, null, false, null);
            Assert.IsFalse(limits.IsInRange(11));
        }

        [TestMethod]
        public void Limits_IsInRange_LowerLimitOnly_Inclusion_ValueOverLimit_ReturnsTrue()
        {
            Limits limits = new Limits(null, 10, null, true);
            Assert.IsTrue(limits.IsInRange(11));
        }

        [TestMethod]
        public void Limits_IsInRange_LowerLimitOnly_Inclusion_ValueOnLimit_ReturnsTrue()
        {
            Limits limits = new Limits(null, 10, null, true);
            Assert.IsTrue(limits.IsInRange(10));
        }

        [TestMethod]
        public void Limits_IsInRange_LowerLimitOnly_Inclusion_ValueUnderLimit_ReturnsFalse()
        {
            Limits limits = new Limits(null, 10, null, true);
            Assert.IsFalse(limits.IsInRange(5));
        }

        [TestMethod]
        public void Limits_IsInRange_LowerLimitOnly_Exclusion_ValueOverLimit_ReturnsTrue()
        {
            Limits limits = new Limits(null, 10, null, false);
            Assert.IsTrue(limits.IsInRange(11));
        }

        [TestMethod]
        public void Limits_IsInRange_LowerLimitOnly_Exclusion_ValueOnLimit_ReturnsFalse()
        {
            Limits limits = new Limits(null, 10, null, false);
            Assert.IsFalse(limits.IsInRange(10));
        }

        [TestMethod]
        public void Limits_IsInRange_LowerLimitOnly_Exclusion_ValueUnderLimit_ReturnsFalse()
        {
            Limits limits = new Limits(null, 10, null, false);
            Assert.IsFalse(limits.IsInRange(5));
        }

        [TestMethod]
        public void Limits_IsInRange_InvalidLimitsThrowsInvalidOperationException()
        {
            Limits limits = new Limits(10, 100, true, true);
            Assert.IsFalse(limits.IsValid());
            Assert.ThrowsException<InvalidOperationException>(() => limits.IsInRange(10));
        }

        #endregion

        #region Parsing

        [TestMethod]
        public void Limits_CanParse_BothLimitsInclusion() => Assert.IsTrue(Limits.CanParse("[0,1]", out _));

        [TestMethod]
        public void Limits_CanParse_BothLimitsUpperExclusionLowerInclusion() => Assert.IsTrue(Limits.CanParse("[0,1)", out _));

        [TestMethod]
        public void Limits_CanParse_BothLimitsUpperInclusionLowerExclusion() => Assert.IsTrue(Limits.CanParse("(0,1]", out _));

        [TestMethod]
        public void Limits_CanParse_BothLimitsExclusion() => Assert.IsTrue(Limits.CanParse("(0,1)", out _));

        [TestMethod]
        public void Limits_CanParse_DecimalLimits() => Assert.IsTrue(Limits.CanParse("[0.123,4.567)", out _));

        [TestMethod]
        public void Limits_CanParse_NoLimits() => Assert.IsTrue(Limits.CanParse(",", out _));

        [TestMethod]
        public void Limits_CanParse_HandlesWhitespace() => Assert.IsTrue(Limits.CanParse("[ 0\n ,\r 1 \t]", out _));

        [TestMethod]
        public void Limits_CanParse_BracketButNoLimitReturnsFalse() => Assert.IsFalse(Limits.CanParse("[0,]", out _));

        [TestMethod]
        public void Limits_CanParse_LimitButNoBracketReturnsFalse() => Assert.IsFalse(Limits.CanParse("[0,1", out _));

        [TestMethod]
        public void Limits_CanParse_NonNumericReturnsFalse() => Assert.IsFalse(Limits.CanParse("[0,asd]", out _));

        [TestMethod]
        public void Limits_CanParse_NoCommaSeparatorReturnsFalse() => Assert.IsFalse(Limits.CanParse("[0 1]", out _));

        [TestMethod]
        public void Limits_CanParse_ReturnsLimits()
        {
            bool canParse = Limits.CanParse("[0,1]", out Limits limits);
            Assert.IsTrue(canParse);
            Assert.IsNotNull(limits);
            Assert.IsTrue(limits.IsUpperValueIncluded.Value);
            Assert.IsTrue(limits.IsLowerValueIncluded.Value);
            Assert.AreEqual(0, limits.LowerLimit.Value);
            Assert.AreEqual(1, limits.UpperLimit.Value);
        }

        [TestMethod]
        public void Limits_Parse_InvalidInputThrowsArgumentException() => Assert.ThrowsException<ArgumentException>(() => Limits.Parse("something invalid"));

        [TestMethod]
        public void Limits_Parse_ReturnsLimits()
        {
            Limits limits = Limits.Parse("[0,1]");
            Assert.IsNotNull(limits);
            Assert.IsTrue(limits.IsUpperValueIncluded.Value);
            Assert.IsTrue(limits.IsLowerValueIncluded.Value);
            Assert.AreEqual(0, limits.LowerLimit.Value);
            Assert.AreEqual(1, limits.UpperLimit.Value);
        }

        [TestMethod]
        public void Limits_Parse_InvalidLimitsThrowsArgumentException() => Assert.ThrowsException<ArgumentException>(() => Limits.Parse("[1,0]"));

        #endregion

        #region ToString

        [TestMethod]
        public void Limits_ToString_InvalidLimitsPrependsWithInvalid()
        {
            Limits limits = new Limits(10, 100, true, true);
            Assert.IsFalse(limits.IsValid());
            Assert.IsTrue(limits.ToString().StartsWith("Invalid"));
        }

        [TestMethod]
        public void Limits_ToString_BothLimitsSet_BothIncluded() => Assert.AreEqual("[0,1]", new Limits(1, 0, true, true).ToString());

        [TestMethod]
        public void Limits_ToString_BothLimitsSet_UpperIncluded() => Assert.AreEqual("(0,1]", new Limits(1, 0, true, false).ToString());

        [TestMethod]
        public void Limits_ToString_BothLimitsSet_LowerIncluded() => Assert.AreEqual("[0,1)", new Limits(1, 0, false, true).ToString());

        [TestMethod]
        public void Limits_ToString_BothLimitsSet_NeitherIncluded() => Assert.AreEqual("(0,1)", new Limits(1, 0, false, false).ToString());

        [TestMethod]
        public void Limits_ToString_OnlyUpperSet_Included() => Assert.AreEqual(",1]", new Limits(1, null, true, null).ToString());

        [TestMethod]
        public void Limits_ToString_OnlyUpperSet_Excluded() => Assert.AreEqual(",1)", new Limits(1, null, false, null).ToString());

        [TestMethod]
        public void Limits_ToString_OnlyLowerSet_Included() => Assert.AreEqual("[0,", new Limits(null, 0, null, true).ToString());

        [TestMethod]
        public void Limits_ToString_OnlyLowerSet_Excluded() => Assert.AreEqual("(0,", new Limits(null, 0, null, false).ToString());

        [TestMethod]
        public void Limits_ToString_NeitherSet() => Assert.AreEqual(",", new Limits(null, null, null, null).ToString());

        #endregion
    }
}
