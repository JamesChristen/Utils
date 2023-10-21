using Common.Sequences;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Common.Test.Sequences
{
    [TestClass]
    public class TimeSequenceTests
    {
        private static readonly SequenceOptions _options = new SequenceOptions() { IncludeWeekends = true };

        #region InRange

        [TestMethod]
        public void Test_InRange_ReturnsRange()
        {
            ITimeSequence seq = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 1M },
                { new DateTime(2022, 1, 2), 1M },
                { new DateTime(2022, 1, 3), 1M },
                { new DateTime(2022, 1, 4), 1M },
                { new DateTime(2022, 1, 5), 1M }
            };

            ITimeSequence result = seq.InRange(new DateTime(2022, 1, 2), new DateTime(2022, 1, 4));
            ITimeSequence expected = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 2), 1M },
                { new DateTime(2022, 1, 3), 1M },
                { new DateTime(2022, 1, 4), 1M }
            };

            Assert.That.AreEqual(expected, result);
        }

        [TestMethod]
        public void Test_InRange_StartDateAfterEndDateReturnsEmptySequence()
        {
            ITimeSequence seq = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 1M }
            };

            ITimeSequence result = seq.InRange(DateTime.MaxValue, DateTime.MinValue);
            ITimeSequence expected = new TimeSequence(_options);

            Assert.That.AreEqual(expected, result);
        }

        #endregion

        #region Absolute

        [TestMethod]
        public void Test_Absolute_AbsolutesEveryItem()
        {
            ITimeSequence seq = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 1M },
                { new DateTime(2022, 1, 2), -1M },
                { new DateTime(2022, 1, 3), -0M },
            };

            ITimeSequence expected = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 1M },
                { new DateTime(2022, 1, 2), 1M },
                { new DateTime(2022, 1, 3), 0M },
            };

            Assert.That.AreEqual(expected, seq.Absolute());
        }

        #endregion

        #region Negate

        [TestMethod]
        public void Test_Negate_NegatesEveryItem()
        {
            ITimeSequence seq = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 1M },
                { new DateTime(2022, 1, 2), -1M },
                { new DateTime(2022, 1, 3), 0M },
            };

            ITimeSequence expected = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), -1M },
                { new DateTime(2022, 1, 2), 1M },
                { new DateTime(2022, 1, 3), 0M },
            };

            Assert.That.AreEqual(expected, seq.Negate());
        }

        #endregion

        #region Add

        [TestMethod]
        public void Test_Add_SameKeys()
        {
            ITimeSequence seq = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 1M },
                { new DateTime(2022, 1, 2), -1M },
                { new DateTime(2022, 1, 3), 0M },
            };

            ITimeSequence toAdd = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 10M },
                { new DateTime(2022, 1, 2), 10M },
                { new DateTime(2022, 1, 3), 10M },
            };

            ITimeSequence expected = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 11M },
                { new DateTime(2022, 1, 2), 9M },
                { new DateTime(2022, 1, 3), 10M },
            };

            Assert.That.AreEqual(expected, seq.Add(toAdd));
        }

        [TestMethod]
        public void Test_Add_ExtraKeysInInputDoesNotCreateNewKeysInOutput()
        {
            ITimeSequence seq = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 1M },
                { new DateTime(2022, 1, 2), -1M },
            };

            ITimeSequence toAdd = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 10M },
                { new DateTime(2022, 1, 2), 10M },
                { new DateTime(2022, 1, 3), 10M },
            };

            ITimeSequence expected = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 11M },
                { new DateTime(2022, 1, 2), 9M },
            };

            Assert.That.AreEqual(expected, seq.Add(toAdd));
        }

        [TestMethod]
        public void Test_Add_MissingKeysInInputDefaultToAddingZero()
        {
            ITimeSequence seq = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 1M },
                { new DateTime(2022, 1, 2), -1M },
                { new DateTime(2022, 1, 3), 0M },
            };

            ITimeSequence toAdd = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 10M },
            };

            ITimeSequence expected = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 11M },
                { new DateTime(2022, 1, 2), -1M },
                { new DateTime(2022, 1, 3), 0M },
            };

            Assert.That.AreEqual(expected, seq.Add(toAdd));
        }

        [TestMethod]
        public void Test_Add_NullInputThrowsArgumentNullException()
        {
            ITimeSequence seq = new TimeSequence(_options);
            Assert.ThrowsException<ArgumentNullException>(() => seq.Add(null));
        }

        #endregion

        #region Subtract

        [TestMethod]
        public void Test_Subtract_SameKeys()
        {
            ITimeSequence seq = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 1M },
                { new DateTime(2022, 1, 2), -1M },
                { new DateTime(2022, 1, 3), 0M },
            };

            ITimeSequence toSubtract = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 10M },
                { new DateTime(2022, 1, 2), 10M },
                { new DateTime(2022, 1, 3), -10M },
            };

            ITimeSequence expected = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), -9M },
                { new DateTime(2022, 1, 2), -11M },
                { new DateTime(2022, 1, 3), 10M },
            };

            Assert.That.AreEqual(expected, seq.Subtract(toSubtract));
        }

        [TestMethod]
        public void Test_Subtract_ExtraKeysInInputDoesNotCreateNewKeysInOutput()
        {
            ITimeSequence seq = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 1M },
                { new DateTime(2022, 1, 2), -1M },
            };

            ITimeSequence toSubtract = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 10M },
                { new DateTime(2022, 1, 2), 10M },
                { new DateTime(2022, 1, 3), 10M },
            };

            ITimeSequence expected = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), -9M },
                { new DateTime(2022, 1, 2), -11M },
            };

            Assert.That.AreEqual(expected, seq.Subtract(toSubtract));
        }

        [TestMethod]
        public void Test_Subtract_MissingKeysInInputDefaultToAddingZero()
        {
            ITimeSequence seq = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 1M },
                { new DateTime(2022, 1, 2), -1M },
                { new DateTime(2022, 1, 3), 0M },
            };

            ITimeSequence toSubtract = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 10M },
            };

            ITimeSequence expected = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), -9M },
                { new DateTime(2022, 1, 2), -1M },
                { new DateTime(2022, 1, 3), 0M },
            };

            Assert.That.AreEqual(expected, seq.Subtract(toSubtract));
        }

        [TestMethod]
        public void Test_Subtract_NullInputThrowsArgumentNullException()
        {
            ITimeSequence seq = new TimeSequence(_options);
            Assert.ThrowsException<ArgumentNullException>(() => seq.Subtract(null));
        }

        #endregion

        #region DivideBy

        [TestMethod]
        public void Test_DivideBy_Denominator()
        {
            ITimeSequence seq = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 100M },
                { new DateTime(2022, 1, 2), 910M },
                { new DateTime(2022, 1, 3), 0M },
            };

            ITimeSequence expected = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 1M },
                { new DateTime(2022, 1, 2), 9.1M },
                { new DateTime(2022, 1, 3), 0M },
            };

            Assert.That.AreEqual(expected, seq.DivideBy(100));
        }

        [TestMethod]
        public void Test_DivideBy_Denominator_ZeroThrowsDivideByZeroException()
        {
            ITimeSequence seq = new TimeSequence(_options);
            Assert.ThrowsException<DivideByZeroException>(() => seq.DivideBy(0));
        }

        [TestMethod]
        public void Test_DivideBy_Seq_SameKeysNoZeroes()
        {
            ITimeSequence seq = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 1M },
                { new DateTime(2022, 1, 2), -1M },
                { new DateTime(2022, 1, 3), 0M },
            };

            ITimeSequence divide = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 10M },
                { new DateTime(2022, 1, 2), 10M },
                { new DateTime(2022, 1, 3), 10M },
            };

            ITimeSequence expected = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 0.1M },
                { new DateTime(2022, 1, 2), -0.1M },
                { new DateTime(2022, 1, 3), 0M },
            };

            Assert.That.AreEqual(expected, seq.DivideBy(divide));
        }

        [TestMethod]
        public void Test_DivideBy_Seq_SameKeysHasZeroesThrowsDivideByZeroException()
        {
            ITimeSequence seq = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 1M },
                { new DateTime(2022, 1, 2), -1M },
                { new DateTime(2022, 1, 3), 0M },
            };

            ITimeSequence divide = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 0M },
                { new DateTime(2022, 1, 2), 0M },
                { new DateTime(2022, 1, 3), 0M },
            };

            Assert.ThrowsException<DivideByZeroException>(() => seq.DivideBy(divide));
        }

        [TestMethod]
        public void Test_DivideBy_Seq_ExtraKeysInDivisorIgnoresExtraKeys()
        {
            ITimeSequence seq = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 0M },
                { new DateTime(2022, 1, 2), 1M },
                { new DateTime(2022, 1, 3), 2M },
            };

            ITimeSequence divide = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 1M },
                { new DateTime(2022, 1, 2), 1M },
                { new DateTime(2022, 1, 3), 1M },
                { new DateTime(2022, 1, 4), 1M }
            };

            ITimeSequence expected = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 0M },
                { new DateTime(2022, 1, 2), 1M },
                { new DateTime(2022, 1, 3), 2M },
            };

            Assert.That.AreEqual(expected, seq.DivideBy(divide));
        }

        [TestMethod]
        public void Test_DivideBy_Seq_DivisorHasMissingKeysThrowsArgumentException()
        {
            ITimeSequence seq = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 1M },
                { new DateTime(2022, 1, 2), -1M },
                { new DateTime(2022, 1, 3), 0M },
            };

            ITimeSequence divide = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 10M },
            };

            Assert.ThrowsException<ArgumentException>(() => seq.DivideBy(divide));
        }

        [TestMethod]
        public void Test_DivideBy_Seq_NullInputThrowsArgumentNullException()
        {
            ITimeSequence seq = new TimeSequence(_options);
            Assert.ThrowsException<ArgumentNullException>(() => seq.DivideBy(null));
        }

        #endregion

        #region MultiplyBy

        [TestMethod]
        public void Test_MultiplyBy_Multiplier()
        {
            ITimeSequence seq = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 1M },
                { new DateTime(2022, 1, 2), 9.1M },
                { new DateTime(2022, 1, 3), 0M },
            };

            ITimeSequence expected = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 100M },
                { new DateTime(2022, 1, 2), 910M },
                { new DateTime(2022, 1, 3), 0M },
            };

            Assert.That.AreEqual(expected, seq.MultiplyBy(100));
        }

        [TestMethod]
        public void Test_MultiplyBy_Seq_SameKeysNoZeroes()
        {
            ITimeSequence seq = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 1M },
                { new DateTime(2022, 1, 2), -1M },
                { new DateTime(2022, 1, 3), 0M },
            };

            ITimeSequence multiply = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 10M },
                { new DateTime(2022, 1, 2), 10M },
                { new DateTime(2022, 1, 3), 10M },
            };

            ITimeSequence expected = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 10M },
                { new DateTime(2022, 1, 2), -10M },
                { new DateTime(2022, 1, 3), 0M },
            };

            Assert.That.AreEqual(expected, seq.MultiplyBy(multiply));
        }

        [TestMethod]
        public void Test_MultiplyBy_Seq_MultiplierHasMissingKeysThrowsArgumentException()
        {
            ITimeSequence seq = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 1M },
                { new DateTime(2022, 1, 2), -1M },
                { new DateTime(2022, 1, 3), 0M },
            };

            ITimeSequence multiply = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 10M },
            };

            Assert.ThrowsException<ArgumentException>(() => seq.MultiplyBy(multiply));
        }

        [TestMethod]
        public void Test_MultiplyBy_Seq_ExtraKeysInMultiplierIgnoresExtraKeys()
        {
            ITimeSequence seq = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 0M },
                { new DateTime(2022, 1, 2), 1M },
                { new DateTime(2022, 1, 3), 2M },
            };

            ITimeSequence multiply = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 1M },
                { new DateTime(2022, 1, 2), 1M },
                { new DateTime(2022, 1, 3), 1M },
                { new DateTime(2022, 1, 4), 1M }
            };

            ITimeSequence expected = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 0M },
                { new DateTime(2022, 1, 2), 1M },
                { new DateTime(2022, 1, 3), 2M },
            };

            Assert.That.AreEqual(expected, seq.MultiplyBy(multiply));
        }

        [TestMethod]
        public void Test_MultiplyBy_Seq_NullInputThrowsArgumentNullException()
        {
            ITimeSequence seq = new TimeSequence(_options);
            Assert.ThrowsException<ArgumentNullException>(() => seq.MultiplyBy(null));
        }

        #endregion

        #region SquareRoot

        [TestMethod]
        public void Test_SquareRoot()
        {
            ITimeSequence seq = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 9M },
                { new DateTime(2022, 1, 2), 4M },
                { new DateTime(2022, 1, 3), 0M },
            };

            ITimeSequence expected = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 3M },
                { new DateTime(2022, 1, 2), 2M },
                { new DateTime(2022, 1, 3), 0M },
            };

            Assert.That.AreEqual(expected, seq.SquareRoot());
        }

        [TestMethod]
        public void Test_SquareRoot_NegativeValuesThrowsInvalidOperationException()
        {
            ITimeSequence seq = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), -1M },
            };

            Assert.ThrowsException<InvalidOperationException>(() => seq.SquareRoot());
        }

        #endregion

        private static readonly ITimeSequence _testSeq = new TimeSequence(_options)
        {
            { new DateTime(2022, 1, 1), 10 },
            { new DateTime(2022, 1, 2), 12 },
            { new DateTime(2022, 1, 3), 6 },
            { new DateTime(2022, 1, 4), 9 },
            { new DateTime(2022, 1, 5), 13 },
            { new DateTime(2022, 1, 6), 11 },
            { new DateTime(2022, 1, 7), 12 },
            { new DateTime(2022, 1, 8), 8 },
            { new DateTime(2022, 1, 9), 9 },
            { new DateTime(2022, 1, 10), 6 },
        };

        #region MeanExpanding

        [TestMethod]
        public void Test_MeanExpanding()
        {
            ITimeSequence expected = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 10M },
                { new DateTime(2022, 1, 2), 11M },
                { new DateTime(2022, 1, 3), 9.33333333333333M },
                { new DateTime(2022, 1, 4), 9.25M },
                { new DateTime(2022, 1, 5), 10M },
                { new DateTime(2022, 1, 6), 10.1666666666667M },
                { new DateTime(2022, 1, 7), 10.4285714285714M },
                { new DateTime(2022, 1, 8), 10.125M },
                { new DateTime(2022, 1, 9), 10M },
                { new DateTime(2022, 1, 10), 9.6M },

            };

            Assert.That.AreEqual(expected, _testSeq.MeanExpanding());
        }

        #endregion

        #region MeanWindowed

        [TestMethod]
        public void Test_MeanWindowed()
        {
            ITimeSequence expected = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 10M },
                { new DateTime(2022, 1, 2), 11M },
                { new DateTime(2022, 1, 3), 9.33333333333333M },
                { new DateTime(2022, 1, 4), 9.25M },
                { new DateTime(2022, 1, 5), 10M },
                { new DateTime(2022, 1, 6), 10.2M },
                { new DateTime(2022, 1, 7), 10.2M },
                { new DateTime(2022, 1, 8), 10.6M },
                { new DateTime(2022, 1, 9), 10.6M },
                { new DateTime(2022, 1, 10), 9.2M },
            };

            Assert.That.AreEqual(expected, _testSeq.MeanWindowed(new WindowLength(5)));
        }

        [TestMethod]
        public void Test_MeanWindowed_InfiniteReturnsMeanExpanding()
        {
            Assert.That.AreEqual(_testSeq.MeanExpanding(), _testSeq.MeanWindowed(WindowLength.Infinite));
        }

        #endregion

        #region StandardDeviationExpanding

        [TestMethod]
        public void Test_StandardDeviationExpanding()
        {
            ITimeSequence expected = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 2), 1.4142135623731M },
                { new DateTime(2022, 1, 3), 3.05505046330389M },
                { new DateTime(2022, 1, 4), 2.5M },
                { new DateTime(2022, 1, 5), 2.73861278752583M },
                { new DateTime(2022, 1, 6), 2.48327740429189M },
                { new DateTime(2022, 1, 7), 2.37045304088641M },
                { new DateTime(2022, 1, 8), 2.3566016694748M },
                { new DateTime(2022, 1, 9), 2.23606797749979M },
                { new DateTime(2022, 1, 10), 2.45854518861144M },
            };

            Assert.That.AreEqual(expected, _testSeq.StandardDeviationExpanding());
        }

        #endregion

        #region StandardDeviationWindowed

        [TestMethod]
        public void Test_StandardDeviationWindowed()
        {
            ITimeSequence expected = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 2), 1.4142135623731M },
                { new DateTime(2022, 1, 3), 3.05505046330389M },
                { new DateTime(2022, 1, 4), 2.5M },
                { new DateTime(2022, 1, 5), 2.73861278752583M },
                { new DateTime(2022, 1, 6), 2.77488738510232M },
                { new DateTime(2022, 1, 7), 2.77488738510232M },
                { new DateTime(2022, 1, 8), 2.07364413533278M },
                { new DateTime(2022, 1, 9), 2.07364413533278M },
                { new DateTime(2022, 1, 10), 2.38746727726266M },
            };

            Assert.That.AreEqual(expected, _testSeq.StandardDeviationWindowed(new WindowLength(5)));
        }

        [TestMethod]
        public void Test_StandardDeviationWindowed_InfiniteReturnsMeanExpanding()
        {
            Assert.That.AreEqual(_testSeq.StandardDeviationExpanding(), _testSeq.StandardDeviationWindowed(WindowLength.Infinite));
        }

        #endregion

        #region StandardDeviationPopulationExpanding

        [TestMethod]
        public void Test_StandardDeviationPopulationExpanding()
        {
            ITimeSequence expected = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 0M },
                { new DateTime(2022, 1, 2), 1M },
                { new DateTime(2022, 1, 3), 2.49443825784929M },
                { new DateTime(2022, 1, 4), 2.1650635094611M },
                { new DateTime(2022, 1, 5), 2.44948974278318M },
                { new DateTime(2022, 1, 6), 2.26691175145591M },
                { new DateTime(2022, 1, 7), 2.1946130708196M },
                { new DateTime(2022, 1, 8), 2.20439901106855M },
                { new DateTime(2022, 1, 9), 2.10818510677892M },
                { new DateTime(2022, 1, 10), 2.33238075793812M },
            };

            Assert.That.AreEqual(expected, _testSeq.StandardDeviationPopulationExpanding());
        }

        #endregion

        #region StandardDeviationPopulationWindowed

        [TestMethod]
        public void Test_StandardDeviationPopulationWindowed()
        {
            ITimeSequence expected = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 0M },
                { new DateTime(2022, 1, 2), 1M },
                { new DateTime(2022, 1, 3), 2.49443825784929M },
                { new DateTime(2022, 1, 4), 2.1650635094611M },
                { new DateTime(2022, 1, 5), 2.44948974278318M },
                { new DateTime(2022, 1, 6), 2.48193472919817M },
                { new DateTime(2022, 1, 7), 2.48193472919817M },
                { new DateTime(2022, 1, 8), 1.85472369909914M },
                { new DateTime(2022, 1, 9), 1.85472369909914M },
                { new DateTime(2022, 1, 10), 2.13541565040626M },
            };

            Assert.That.AreEqual(expected, _testSeq.StandardDeviationPopulationWindowed(new WindowLength(5)));
        }

        [TestMethod]
        public void Test_StandardDeviationPopulationWindowed_InfiniteReturnsMeanExpanding()
        {
            Assert.That.AreEqual(_testSeq.MeanExpanding(), _testSeq.MeanWindowed(WindowLength.Infinite));
        }

        #endregion

        #region VarianceExpanding

        [TestMethod]
        public void Test_VarianceExpanding()
        {
            ITimeSequence expected = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 2), 2M },
                { new DateTime(2022, 1, 3), 9.33333333333334M },
                { new DateTime(2022, 1, 4), 6.25M },
                { new DateTime(2022, 1, 5), 7.5M },
                { new DateTime(2022, 1, 6), 6.16666666666667M },
                { new DateTime(2022, 1, 7), 5.61904761904761M },
                { new DateTime(2022, 1, 8), 5.55357142857143M },
                { new DateTime(2022, 1, 9), 5M },
                { new DateTime(2022, 1, 10), 6.04444444444444M },
            };

            Assert.That.AreEqual(expected, _testSeq.VarianceExpanding());
        }

        #endregion

        #region VarianceWindowed

        [TestMethod]
        public void Test_VarianceWindowed()
        {
            ITimeSequence expected = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 2), 2M },
                { new DateTime(2022, 1, 3), 9.33333333333334M },
                { new DateTime(2022, 1, 4), 6.25M },
                { new DateTime(2022, 1, 5), 7.5M },
                { new DateTime(2022, 1, 6), 7.69999999999999M },
                { new DateTime(2022, 1, 7), 7.69999999999999M },
                { new DateTime(2022, 1, 8), 4.30000000000001M },
                { new DateTime(2022, 1, 9), 4.30000000000001M },
                { new DateTime(2022, 1, 10), 5.7M },
            };

            Assert.That.AreEqual(expected, _testSeq.VarianceWindowed(new WindowLength(5)));
        }

        [TestMethod]
        public void Test_VarianceWindowed_InfiniteReturnsMeanExpanding()
        {
            Assert.That.AreEqual(_testSeq.VarianceExpanding(), _testSeq.VarianceWindowed(WindowLength.Infinite));
        }

        #endregion

        #region VariancePopulationExpanding

        [TestMethod]
        public void Test_VariancePopulationExpanding()
        {
            ITimeSequence expected = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 0M },
                { new DateTime(2022, 1, 2), 1M },
                { new DateTime(2022, 1, 3), 6.22222222222222M },
                { new DateTime(2022, 1, 4), 4.6875M },
                { new DateTime(2022, 1, 5), 6M },
                { new DateTime(2022, 1, 6), 5.13888888888889M },
                { new DateTime(2022, 1, 7), 4.81632653061224M },
                { new DateTime(2022, 1, 8), 4.859375M },
                { new DateTime(2022, 1, 9), 4.44444444444444M },
                { new DateTime(2022, 1, 10), 5.44M },
            };

            Assert.That.AreEqual(expected, _testSeq.VariancePopulationExpanding());
        }

        #endregion

        #region VariancePopulationWindowed

        [TestMethod]
        public void Test_VariancePopulationWindowed()
        {
            ITimeSequence expected = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 0M },
                { new DateTime(2022, 1, 2), 1M },
                { new DateTime(2022, 1, 3), 6.22222222222222M },
                { new DateTime(2022, 1, 4), 4.6875M },
                { new DateTime(2022, 1, 5), 6M },
                { new DateTime(2022, 1, 6), 6.16M },
                { new DateTime(2022, 1, 7), 6.16M },
                { new DateTime(2022, 1, 8), 3.44M },
                { new DateTime(2022, 1, 9), 3.44M },
                { new DateTime(2022, 1, 10), 4.56M },
            };

            Assert.That.AreEqual(expected, _testSeq.VariancePopulationWindowed(new WindowLength(5)));
        }

        [TestMethod]
        public void Test_VariancePopulationWindowed_InfiniteReturnsMeanExpanding()
        {
            ITimeSequence expected = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 1), 0M },
                { new DateTime(2022, 1, 2), 1M },
                { new DateTime(2022, 1, 3), 6.22222222222222M },
                { new DateTime(2022, 1, 4), 4.6875M },
                { new DateTime(2022, 1, 5), 6M },
                { new DateTime(2022, 1, 6), 5.13888888888889M },
                { new DateTime(2022, 1, 7), 4.81632653061224M },
                { new DateTime(2022, 1, 8), 4.859375M },
                { new DateTime(2022, 1, 9), 4.44444444444444M },
                { new DateTime(2022, 1, 10), 5.44M },

            };

            Assert.That.AreEqual(expected, _testSeq.VariancePopulationWindowed(WindowLength.Infinite));
        }

        #endregion

        #region ZScoreExpanding

        [TestMethod]
        public void Test_ZScoreExpanding()
        {
            ITimeSequence expected = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 2), 0.707106781186547M },
                { new DateTime(2022, 1, 3), -1.09108945117996M },
                { new DateTime(2022, 1, 4), -0.1M },
                { new DateTime(2022, 1, 5), 1.09544511501033M },
                { new DateTime(2022, 1, 6), 0.335578027607012M },
                { new DateTime(2022, 1, 7), 0.662923308044504M },
                { new DateTime(2022, 1, 8), -0.901722182210616M },
                { new DateTime(2022, 1, 9), -0.447213595499958M },
                { new DateTime(2022, 1, 10), -1.46428059027593M },
            };

            Assert.That.AreEqual(expected, _testSeq.ZScoreExpanding());
        }

        #endregion

        #region ZScoreWindowed

        [TestMethod]
        public void Test_ZScoreWindowed()
        {
            ITimeSequence expected = new TimeSequence(_options)
            {
                { new DateTime(2022, 1, 2), 0.707106781186547M },
                { new DateTime(2022, 1, 3), -1.09108945117996M },
                { new DateTime(2022, 1, 4), -0.1M },
                { new DateTime(2022, 1, 5), 1.09544511501033M },
                { new DateTime(2022, 1, 6), 0.288299988062579M },
                { new DateTime(2022, 1, 7), 0.648674973140803M },
                { new DateTime(2022, 1, 8), -1.25383133764307M },
                { new DateTime(2022, 1, 9), -0.771588515472658M },
                { new DateTime(2022, 1, 10), -1.34033250653343M },
            };

            Assert.That.AreEqual(expected, _testSeq.ZScoreWindowed(new WindowLength(5)));
        }

        [TestMethod]
        public void Test_ZScoreWindowed_InfiniteReturnsMeanExpanding()
        {
            Assert.That.AreEqual(_testSeq.ZScoreExpanding(), _testSeq.ZScoreWindowed(WindowLength.Infinite));
        }

        #endregion

        #region Mean

        [TestMethod]
        public void Test_Mean_NullInputsThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => TimeSequence.Mean(null));
        }

        #endregion
    }

    internal static class TimeSequenceExtensions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void AreEqual(this Assert assert, ITimeSequence expected, ITimeSequence observed, bool checkOptions = true, decimal margin = 0.0001M)
        {
            if (expected == null && observed != null)
            {
                Assert.Fail("Expected null but observed not null");
            }
            if (expected != null && observed == null)
            {
                Assert.Fail("Expected not null but observed null");
            }
            if (expected == null && observed == null)
            {
                return;
            }

            Assert.AreEqual(expected.Length, observed.Length);
            Assert.IsTrue(expected.Keys.SequenceEqual(observed.Keys));

            foreach (DateTime key in expected.Keys)
            {
                Assert.IsTrue(expected[key].EqualsWithError(observed[key], margin), $"Difference on {key:dd/MM/yyyy}: Exp {expected[key]} -> Obs {observed[key]}");
            }

            if (checkOptions)
            {
                Assert.AreEqual(expected.Options, observed.Options);
            }
        }
    }
}
