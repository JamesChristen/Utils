using Common.Sequences;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Test.Sequences
{
    [TestClass]
    public class HistoryTests
    {
        [TestMethod]
        public void Test_NullDataThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new History<decimal>(null));
        }

        [TestMethod]
        public void Test_OutOfOrderBecomesSorted()
        {
            Dictionary<DateTime, decimal> items = new Dictionary<DateTime, decimal>()
            {
                { new DateTime(2021, 4, 7), 1M },
                { new DateTime(2021, 4, 6), 1M },
            };

            History<decimal> history = new History<decimal>(items);

            Assert.AreEqual(new DateTime(2021, 4, 6), history.Data.ElementAt(0).Key);
            Assert.AreEqual(new DateTime(2021, 4, 7), history.Data.ElementAt(1).Key);
        }

        [TestMethod]
        public void Test_GetValueKnownOn()
        {
            Dictionary<DateTime, decimal> items = new Dictionary<DateTime, decimal>()
            {
                { new DateTime(2021, 4, 5), 0 },
                { new DateTime(2021, 4, 6), 1 },
                { new DateTime(2021, 4, 7), 2 },
            };

            History<decimal> history = new History<decimal>(items);

            Assert.AreEqual(0, history.GetValueKnownOn(new DateTime(2021, 4, 5)));
            Assert.AreEqual(1, history.GetValueKnownOn(new DateTime(2021, 4, 6))); // Take latest value on known change time
            Assert.AreEqual(1, history.GetValueKnownOn(new DateTime(2021, 4, 6, 12, 0, 0)));
            Assert.AreEqual(2, history.GetValueKnownOn(new DateTime(2021, 4, 7)));
            Assert.AreEqual(2, history.GetValueKnownOn(new DateTime(2021, 4, 8)));
        }

        [TestMethod]
        public void Test_GetValueKnownOn_BeforeHistoryReturnsDefault()
        {
            Dictionary<DateTime, decimal> items = new Dictionary<DateTime, decimal>()
            {
                { new DateTime(2021, 4, 6), 1 },
                { new DateTime(2021, 4, 7), 2 },
            };

            History<decimal> history = new History<decimal>(items);

            Assert.AreEqual(default, history.GetValueKnownOn(new DateTime(2021, 4, 5)));
        }

        [TestMethod]
        public void Test_GetValueKnownOn_AfterHistoryReturnsLast()
        {
            Dictionary<DateTime, decimal> items = new Dictionary<DateTime, decimal>()
            {
                { new DateTime(2021, 4, 6), 1 },
                { new DateTime(2021, 4, 7), 2 },
            };

            History<decimal> history = new History<decimal>(items);

            Assert.AreEqual(2, history.GetValueKnownOn(new DateTime(2021, 4, 10)));
        }
    }
}
