using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Test.Extensions.Date
{
    [TestClass]
    public class RangeTests
    {
        [TestMethod]
        public void Test_RangeTo()
        {
            DateTime fromDate = new DateTime(2021, 4, 5); // Monday
            DateTime toDate = new DateTime(2021, 4, 9); // Friday
            IEnumerable<DateTime> expected = Enumerable.Range(0, 5).Select(x => fromDate.AddDays(x));

            IEnumerable<DateTime> dates = fromDate.RangeTo(toDate);
            Assert.IsTrue(Utils.AreEquivalent(expected, dates));
        }

        [TestMethod]
        public void Test_RangeTo_SingleDate()
        {
            DateTime fromDate = new DateTime(2021, 4, 5); // Monday
            IEnumerable<DateTime> expected = Enumerable.Range(0, 1).Select(x => fromDate.AddDays(x));

            IEnumerable<DateTime> dates = fromDate.RangeTo(fromDate);
            Assert.IsTrue(Utils.AreEquivalent(expected, dates));
        }

        [TestMethod]
        public void Test_RangeTo_ExcludeWeekend()
        {
            DateTime fromDate = new DateTime(2021, 4, 5); // Monday
            DateTime toDate = new DateTime(2021, 4, 12); // Monday
            IEnumerable<DateTime> expected =
                Enumerable.Range(0, 8)
                          .Select(x => fromDate.AddDays(x))
                          .Where(x => x.IsWeekday());

            IEnumerable<DateTime> dates = fromDate.RangeTo(toDate, includeWeekends: false);
            Assert.IsTrue(Utils.AreEquivalent(expected, dates));
        }

        [TestMethod]
        public void Test_RangeTo_IncludeWeekend()
        {
            DateTime fromDate = new DateTime(2021, 4, 5); // Monday
            DateTime toDate = new DateTime(2021, 4, 12); // Monday
            IEnumerable<DateTime> expected =
                Enumerable.Range(0, 8)
                          .Select(x => fromDate.AddDays(x));

            IEnumerable<DateTime> dates = fromDate.RangeTo(toDate, includeWeekends: true);
            Assert.IsTrue(Utils.AreEquivalent(expected, dates));
        }

        [TestMethod]
        public void Test_RangeTo_EndBeforeStartThrowsException()
        {
            DateTime fromDate = new DateTime(2021, 4, 5); // Monday
            DateTime toDate = new DateTime(2021, 4, 2); // Friday

            Assert.ThrowsException<ArgumentException>(() => fromDate.RangeTo(toDate).ToList());
        }

        [TestMethod]
        public void Test_RangeTo_DifferentTimeStampsThrowsException()
        {
            DateTime fromDate = new DateTime(2021, 4, 5, 12, 0, 0); // Monday
            DateTime toDate = new DateTime(2021, 4, 9); // Friday

            Assert.ThrowsException<ArgumentException>(() => fromDate.RangeTo(toDate).ToList());
        }

        [TestMethod]
        public void Test_RangeTo_KeepTimeStamp()
        {
            DateTime fromDate = new DateTime(2021, 4, 5, 12, 0, 0); // Monday
            DateTime toDate = new DateTime(2021, 4, 12, 12, 0, 0); // Monday
            IEnumerable<DateTime> expected =
                Enumerable.Range(0, 8)
                          .Select(x => fromDate.AddDays(x));

            IEnumerable<DateTime> dates = fromDate.RangeTo(toDate, includeWeekends: true);
            Assert.IsTrue(Utils.AreEquivalent(expected, dates));
        }

        [TestMethod]
        public void Test_RangeTo_DropTimeStamp()
        {
            DateTime fromDate = new DateTime(2021, 4, 5, 12, 0, 0); // Monday
            DateTime toDate = new DateTime(2021, 4, 12, 12, 0, 0); // Monday
            IEnumerable<DateTime> expected =
                Enumerable.Range(0, 8)
                          .Select(x => fromDate.AddDays(x).Date);

            IEnumerable<DateTime> dates = fromDate.RangeTo(toDate, includeWeekends: true, keepTimeStamp: false);
            Assert.IsTrue(Utils.AreEquivalent(expected, dates));
        }

        [TestMethod]
        public void Test_RangeFrom()
        {
            DateTime fromDate = new DateTime(2021, 4, 5); // Monday
            DateTime toDate = new DateTime(2021, 4, 9); // Friday
            IEnumerable<DateTime> expected = Enumerable.Range(0, 5).Select(x => fromDate.AddDays(x));

            IEnumerable<DateTime> dates = toDate.RangeFrom(fromDate);
            Assert.IsTrue(Utils.AreEquivalent(expected, dates));
        }

        [TestMethod]
        public void Test_RangeFrom_SingleDate()
        {
            DateTime fromDate = new DateTime(2021, 4, 5); // Monday
            IEnumerable<DateTime> expected = Enumerable.Range(0, 1).Select(x => fromDate.AddDays(x));

            IEnumerable<DateTime> dates = fromDate.RangeFrom(fromDate);
            Assert.IsTrue(Utils.AreEquivalent(expected, dates));
        }

        [TestMethod]
        public void Test_RangeFrom_ExcludeWeekend()
        {
            DateTime fromDate = new DateTime(2021, 4, 5); // Monday
            DateTime toDate = new DateTime(2021, 4, 12); // Monday
            IEnumerable<DateTime> expected =
                Enumerable.Range(0, 8)
                          .Select(x => fromDate.AddDays(x))
                          .Where(x => x.IsWeekday());

            IEnumerable<DateTime> dates = toDate.RangeFrom(fromDate, includeWeekends: false);
            Assert.IsTrue(Utils.AreEquivalent(expected, dates));
        }

        [TestMethod]
        public void Test_RangeFrom_IncludeWeekend()
        {
            DateTime fromDate = new DateTime(2021, 4, 5); // Monday
            DateTime toDate = new DateTime(2021, 4, 12); // Monday
            IEnumerable<DateTime> expected =
                Enumerable.Range(0, 8)
                          .Select(x => fromDate.AddDays(x));

            IEnumerable<DateTime> dates = toDate.RangeFrom(fromDate, includeWeekends: true);
            Assert.IsTrue(Utils.AreEquivalent(expected, dates));
        }

        [TestMethod]
        public void Test_RangeFrom_EndBeforeStartThrowsException()
        {
            DateTime fromDate = new DateTime(2021, 4, 5); // Monday
            DateTime toDate = new DateTime(2021, 4, 2); // Friday

            Assert.ThrowsException<ArgumentException>(() => toDate.RangeFrom(fromDate).ToList());
        }

        [TestMethod]
        public void Test_RangeFrom_DifferentTimeStampsThrowsException()
        {
            DateTime fromDate = new DateTime(2021, 4, 5, 12, 0, 0); // Monday
            DateTime toDate = new DateTime(2021, 4, 9); // Friday

            Assert.ThrowsException<ArgumentException>(() => toDate.RangeFrom(fromDate).ToList());
        }

        [TestMethod]
        public void Test_RangeFrom_KeepTimeStamp()
        {
            DateTime fromDate = new DateTime(2021, 4, 5, 12, 0, 0); // Monday
            DateTime toDate = new DateTime(2021, 4, 12, 12, 0, 0); // Monday
            IEnumerable<DateTime> expected =
                Enumerable.Range(0, 8)
                          .Select(x => fromDate.AddDays(x));

            IEnumerable<DateTime> dates = toDate.RangeFrom(fromDate, includeWeekends: true);
            Assert.IsTrue(Utils.AreEquivalent(expected, dates));
        }

        [TestMethod]
        public void Test_RangeFrom_DropTimeStamp()
        {
            DateTime fromDate = new DateTime(2021, 4, 5, 12, 0, 0); // Monday
            DateTime toDate = new DateTime(2021, 4, 12, 12, 0, 0); // Monday
            IEnumerable<DateTime> expected =
                Enumerable.Range(0, 8)
                          .Select(x => fromDate.AddDays(x).Date);

            IEnumerable<DateTime> dates = toDate.RangeFrom(fromDate, includeWeekends: true, keepTimeStamp: false);
            Assert.IsTrue(Utils.AreEquivalent(expected, dates));
        }
    }
}
