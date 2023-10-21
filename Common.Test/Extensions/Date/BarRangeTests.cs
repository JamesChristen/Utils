using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Test.Extensions.Date
{
    [TestClass]
    public class BarRangeTests
    {
        [TestMethod]
        public void Test_BarRangeTo()
        {
            DateTime fromDate = new DateTime(2021, 4, 5);
            DateTime toDate = new DateTime(2021, 4, 6);
            IEnumerable<DateTime> expected =
                GenerateBars(fromDate, toDate, 0, 15);

            IEnumerable<DateTime> dates = 
                fromDate.BarRangeTo(toDate, TimeSpan.FromMinutes(15), TimeSpan.Zero, TimeSpan.Zero);
            Assert.IsTrue(Utils.AreEquivalent(expected, dates));
        }

        [TestMethod]
        public void Test_BarRangeTo_SingleBar()
        {
            DateTime fromDate = new DateTime(2021, 4, 5, 12, 0, 0);
            DateTime toDate = new DateTime(2021, 4, 5, 12, 15, 0);
            IEnumerable<DateTime> expected =
                GenerateBars(fromDate, toDate, 0, 15);

            IEnumerable<DateTime> dates =
                fromDate.BarRangeTo(toDate, TimeSpan.FromMinutes(15), TimeSpan.Zero, TimeSpan.Zero);
            Assert.IsTrue(Utils.AreEquivalent(expected, dates));
        }

        [TestMethod]
        public void Test_BarRangeTo_SingleTime()
        {
            DateTime fromDate = new DateTime(2021, 4, 5);
            DateTime toDate = new DateTime(2021, 4, 5);
            IEnumerable<DateTime> expected =
                GenerateBars(fromDate, toDate, 0, 15);

            IEnumerable<DateTime> dates =
                fromDate.BarRangeTo(toDate, TimeSpan.FromMinutes(15), TimeSpan.Zero, TimeSpan.Zero);

            Assert.AreEqual(1, dates.Count());
            Assert.IsTrue(Utils.AreEquivalent(expected, dates));
        }

        [TestMethod]
        public void Test_BarRangeTo_ZeroBarTimeThrowsException()
        {
            DateTime fromDate = new DateTime(2021, 4, 5);
            DateTime toDate = new DateTime(2021, 4, 2);

            Assert.ThrowsException<ArgumentException>(
                () => fromDate.BarRangeTo(toDate, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero).ToList());
        }

        [TestMethod]
        public void Test_BarRangeTo_EndBeforeStartThrowsException()
        {
            DateTime fromDate = new DateTime(2021, 4, 5);
            DateTime toDate = new DateTime(2021, 4, 2);

            Assert.ThrowsException<ArgumentException>(
                () => fromDate.BarRangeTo(toDate, TimeSpan.FromMinutes(15), TimeSpan.Zero, TimeSpan.Zero).ToList());
        }

        [TestMethod]
        public void Test_BarRangeTo_UnevenBarsThrowsException()
        {
            DateTime fromDate = new DateTime(2021, 4, 5);
            DateTime toDate = new DateTime(2021, 4, 5, 0, 7, 0);

            Assert.ThrowsException<ArgumentException>(
                () => fromDate.BarRangeTo(toDate, TimeSpan.FromMinutes(15), TimeSpan.Zero, TimeSpan.Zero).ToList());
        }

        [TestMethod]
        public void Test_BarRangeTo_BarsDontFitInWindowThrowsException()
        {
            DateTime fromDate = new DateTime(2021, 4, 5, 9, 0, 0);
            DateTime toDate = new DateTime(2021, 4, 5, 16, 0, 0);

            Assert.ThrowsException<ArgumentException>(
                () => fromDate.BarRangeTo(toDate, TimeSpan.FromHours(3), TimeSpan.Zero, TimeSpan.Zero).ToList());
        }

        [TestMethod]
        public void Test_BarRangeTo_MultipleDays_IncludeWeekend()
        {
            DateTime fromDate = new DateTime(2021, 4, 9);
            DateTime toDate = new DateTime(2021, 4, 13);
            IEnumerable<DateTime> expected =
                GenerateBars(fromDate, toDate, 0, 15);

            IEnumerable<DateTime> dates =
                fromDate.BarRangeTo(toDate, TimeSpan.FromMinutes(15), TimeSpan.Zero, TimeSpan.Zero, includeWeekends: true);
            Assert.IsTrue(Utils.AreEquivalent(expected, dates));
        }

        [TestMethod]
        public void Test_BarRangeTo_MultipleDays_ExcludeWeekend()
        {
            DateTime fromDate = new DateTime(2021, 4, 9);
            DateTime toDate = new DateTime(2021, 4, 13);
            IEnumerable<DateTime> expected =
                GenerateBars(fromDate, toDate, 0, 15)
                .Where(x => x.IsWeekday());

            IEnumerable<DateTime> dates =
                fromDate.BarRangeTo(toDate, TimeSpan.FromMinutes(15), TimeSpan.Zero, TimeSpan.Zero, includeWeekends: false);
            Assert.IsTrue(Utils.AreEquivalent(expected, dates));
        }

        [TestMethod]
        public void Test_BarRangeTo_TradingHours()
        {
            DateTime fromDate = new DateTime(2021, 4, 5);
            DateTime toDate = new DateTime(2021, 4, 6);
            IEnumerable<DateTime> expected =
                GenerateBars(fromDate, toDate, 0, 15)
                .Where(x => x.TimeOfDay >= TimeSpan.FromHours(9) && x.TimeOfDay <= TimeSpan.FromHours(16));

            IEnumerable<DateTime> dates =
                fromDate.BarRangeTo(toDate, TimeSpan.FromMinutes(15), TimeSpan.FromHours(9), TimeSpan.FromHours(16));
            Assert.IsTrue(Utils.AreEquivalent(expected, dates));
        }

        [TestMethod]
        public void Test_BarRangeTo_TradingHours_MultipleDays_IncludeWeekend()
        {
            DateTime fromDate = new DateTime(2021, 4, 9);
            DateTime toDate = new DateTime(2021, 4, 13);
            IEnumerable<DateTime> expected =
                GenerateBars(fromDate, toDate, 0, 15)
                .Where(x => x.TimeOfDay >= TimeSpan.FromHours(9) && x.TimeOfDay <= TimeSpan.FromHours(16));

            IEnumerable<DateTime> dates =
                fromDate.BarRangeTo(toDate, TimeSpan.FromMinutes(15), TimeSpan.FromHours(9), TimeSpan.FromHours(16), includeWeekends: true);
            Assert.IsTrue(Utils.AreEquivalent(expected, dates));
        }

        [TestMethod]
        public void Test_BarRangeTo_TradingHours_MultipleDays_ExcludeWeekend()
        {
            DateTime fromDate = new DateTime(2021, 4, 9);
            DateTime toDate = new DateTime(2021, 4, 13);
            IEnumerable<DateTime> expected =
                GenerateBars(fromDate, toDate, 0, 15)
                .Where(x => x.IsWeekday())
                .Where(x => x.TimeOfDay >= TimeSpan.FromHours(9) && x.TimeOfDay <= TimeSpan.FromHours(16));

            IEnumerable<DateTime> dates =
                fromDate.BarRangeTo(toDate, TimeSpan.FromMinutes(15), TimeSpan.FromHours(9), TimeSpan.FromHours(16), includeWeekends: false);
            Assert.IsTrue(Utils.AreEquivalent(expected, dates));
        }

        private IEnumerable<DateTime> GenerateBars(DateTime from, DateTime to, int hours, int minutes, int seconds = 0, int milliseconds = 0)
        {
            while (from <= to)
            {
                yield return from;
                from = from.AddHours(hours).AddMinutes(minutes).AddSeconds(seconds).AddMilliseconds(milliseconds);
            }
        }
    }
}
