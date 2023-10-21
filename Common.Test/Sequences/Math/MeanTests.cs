using Common.Sequences;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Common.Test.Sequences.Math
{
    [TestClass]
    public class MeanTests
    {
        private class Foo
        {
            public decimal? Dec { get; set; }

            public Foo(decimal? dec) { Dec = dec; }
        }

        #region Expanding

        [TestMethod]
        public void Test_ITimeSequence_MeanExpanding_EmptySeqReturnsEmpty()
        {
            ITimeSequence seq = TimeSequence.Empty;
            ITimeSequence result = seq.MeanExpanding();
            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        public void Test_ITimeSequence_MeanExpanding_SeqReturnsMean()
        {
            ITimeSequence seq = new TimeSequence()
            {
                { new DateTime(2022, 10, 24), 1M },
                { new DateTime(2022, 10, 25), 2M },
                { new DateTime(2022, 10, 26), 3M },
                { new DateTime(2022, 10, 27), 6M }
            };
            ITimeSequence result = seq.MeanExpanding();
            ITimeSequence expected = new TimeSequence()
            {
                { new DateTime(2022, 10, 24), 1M },
                { new DateTime(2022, 10, 25), 1.5M },
                { new DateTime(2022, 10, 26), 2M },
                { new DateTime(2022, 10, 27), 3M }
            };
            Assert.That.SeqAreEqual(expected, result);
        }

        #endregion

        #region Windowed

        [TestMethod]
        public void Test_ITimeSequence_MeanWindowed_EmptySeqReturnsEmpty()
        {
            ITimeSequence seq = TimeSequence.Empty;
            ITimeSequence result = seq.MeanWindowed(WindowLength.Infinite);
            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        public void Test_ITimeSequence_MeanWindowed_SeqReturnsMean_Infinite()
        {
            ITimeSequence seq = new TimeSequence()
            {
                { new DateTime(2022, 10, 24), 1M },
                { new DateTime(2022, 10, 25), 2M },
                { new DateTime(2022, 10, 26), 3M },
                { new DateTime(2022, 10, 27), 6M }
            };
            ITimeSequence result = seq.MeanWindowed(WindowLength.Infinite);
            ITimeSequence expected = new TimeSequence()
            {
                { new DateTime(2022, 10, 24), 1M },
                { new DateTime(2022, 10, 25), 1.5M },
                { new DateTime(2022, 10, 26), 2M },
                { new DateTime(2022, 10, 27), 3M }
            };
            Assert.That.SeqAreEqual(expected, result);
        }

        [TestMethod]
        public void Test_ITimeSequence_MeanWindowed_SeqReturnsMean_NonInfiniteWindow()
        {
            ITimeSequence seq = new TimeSequence()
            {
                { new DateTime(2022, 10, 24), 1M },
                { new DateTime(2022, 10, 25), 2M },
                { new DateTime(2022, 10, 26), 3M },
                { new DateTime(2022, 10, 27), 6M }
            };
            ITimeSequence result = seq.MeanWindowed(new WindowLength(2));
            ITimeSequence expected = new TimeSequence()
            {
                { new DateTime(2022, 10, 24), 1M },
                { new DateTime(2022, 10, 25), 1.5M },
                { new DateTime(2022, 10, 26), 2.5M },
                { new DateTime(2022, 10, 27), 4.5M }
            };
            Assert.That.SeqAreEqual(expected, result);
        }

        [TestMethod]
        public void Test_ITimeSequence_MeanWindowed_SeqReturnsMean_WindowLargerThanSequence()
        {
            ITimeSequence seq = new TimeSequence()
            {
                { new DateTime(2022, 10, 24), 1M },
                { new DateTime(2022, 10, 25), 2M },
                { new DateTime(2022, 10, 26), 3M },
                { new DateTime(2022, 10, 27), 6M }
            };
            ITimeSequence result = seq.MeanWindowed(new WindowLength(1000));
            ITimeSequence expected = new TimeSequence()
            {
                { new DateTime(2022, 10, 24), 1M },
                { new DateTime(2022, 10, 25), 1.5M },
                { new DateTime(2022, 10, 26), 2M },
                { new DateTime(2022, 10, 27), 3M }
            };
            Assert.That.SeqAreEqual(expected, result);
        }

        #endregion
    }
}
