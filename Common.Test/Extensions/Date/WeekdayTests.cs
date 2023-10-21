using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Common.Test.Extensions.Date
{
    [TestClass]
    public class WeekdayTests
    {
        [TestMethod]
        public void Test_IsWeekday_Monday()
        {
            DateTime monday = new DateTime(2021, 4, 5);
            Assert.IsTrue(monday.IsWeekday());
        }

        [TestMethod]
        public void Test_IsWeekday_Tuesday()
        {
            DateTime tuesday = new DateTime(2021, 4, 6);
            Assert.IsTrue(tuesday.IsWeekday());
        }

        [TestMethod]
        public void Test_IsWeekday_Wednesday()
        {
            DateTime wednesday = new DateTime(2021, 4, 7);
            Assert.IsTrue(wednesday.IsWeekday());
        }

        [TestMethod]
        public void Test_IsWeekday_Thursday()
        {
            DateTime thursday = new DateTime(2021, 4, 8);
            Assert.IsTrue(thursday.IsWeekday());
        }

        [TestMethod]
        public void Test_IsWeekday_Friday()
        {
            DateTime friday = new DateTime(2021, 4, 9);
            Assert.IsTrue(friday.IsWeekday());
        }

        [TestMethod]
        public void Test_IsWeekday_Saturday()
        {
            DateTime saturday = new DateTime(2021, 4, 10);
            Assert.IsFalse(saturday.IsWeekday());
        }

        [TestMethod]
        public void Test_IsWeekday_Sunday()
        {
            DateTime sunday = new DateTime(2021, 4, 11);
            Assert.IsFalse(sunday.IsWeekday());
        }

        [TestMethod]
        public void Test_IsWeekday_Monday_WithTimeStamp()
        {
            DateTime monday = new DateTime(2021, 4, 5, 12, 0, 0);
            Assert.IsTrue(monday.IsWeekday());
        }

        [TestMethod]
        public void Test_IsWeekday_Tuesday_WithTimeStamp()
        {
            DateTime tuesday = new DateTime(2021, 4, 6, 12, 0, 0);
            Assert.IsTrue(tuesday.IsWeekday());
        }

        [TestMethod]
        public void Test_IsWeekday_Wednesday_WithTimeStamp()
        {
            DateTime wednesday = new DateTime(2021, 4, 7, 12, 0, 0);
            Assert.IsTrue(wednesday.IsWeekday());
        }

        [TestMethod]
        public void Test_IsWeekday_Thursday_WithTimeStamp()
        {
            DateTime thursday = new DateTime(2021, 4, 8, 12, 0, 0);
            Assert.IsTrue(thursday.IsWeekday());
        }

        [TestMethod]
        public void Test_IsWeekday_Friday_WithTimeStamp()
        {
            DateTime friday = new DateTime(2021, 4, 9, 12, 0, 0);
            Assert.IsTrue(friday.IsWeekday());
        }

        [TestMethod]
        public void Test_IsWeekday_Saturday_WithTimeStamp()
        {
            DateTime saturday = new DateTime(2021, 4, 10, 12, 0, 0);
            Assert.IsFalse(saturday.IsWeekday());
        }

        [TestMethod]
        public void Test_IsWeekday_Sunday_WithTimeStamp()
        {
            DateTime sunday = new DateTime(2021, 4, 11, 12, 0, 0);
            Assert.IsFalse(sunday.IsWeekday());
        }

        [TestMethod]
        public void Test_AddWeekday_Zero_ReturnsDate()
        {
            DateTime monday = new DateTime(2021, 4, 5);
            Assert.AreEqual(monday, monday.AddWeekdays(0));
        }

        [TestMethod]
        public void Test_AddWeekday_Forward_WeekdaysOnly()
        {
            DateTime monday = new DateTime(2021, 4, 5);
            DateTime tuesday = new DateTime(2021, 4, 6);
            Assert.AreEqual(tuesday, monday.AddWeekdays(1));
        }

        [TestMethod]
        public void Test_AddWeekday_Forward_OverWeekend()
        {
            DateTime friday = new DateTime(2021, 4, 9);
            DateTime expected = new DateTime(2021, 4, 12);
            Assert.AreEqual(expected, friday.AddWeekdays(1));
        }

        [TestMethod]
        public void Test_AddWeekday_Forward_OnWeekend()
        {
            DateTime saturday = new DateTime(2021, 4, 10);
            DateTime expected = new DateTime(2021, 4, 12);
            Assert.AreEqual(expected, saturday.AddWeekdays(1));
        }

        [TestMethod]
        public void Test_AddWeekday_Forward_Multiple_WeekdaysOnly()
        {
            DateTime monday = new DateTime(2021, 4, 5);
            DateTime expected = new DateTime(2021, 4, 7);
            Assert.AreEqual(expected, monday.AddWeekdays(2));
        }

        [TestMethod]
        public void Test_AddWeekday_Forward_Multiple_OverWeekend()
        {
            DateTime friday = new DateTime(2021, 4, 9);
            DateTime expected = new DateTime(2021, 4, 13);
            Assert.AreEqual(expected, friday.AddWeekdays(2));
        }

        [TestMethod]
        public void Test_AddWeekday_Forward_Multiple_OnWeekend()
        {
            DateTime saturday = new DateTime(2021, 4, 10);
            DateTime expected = new DateTime(2021, 4, 13);
            Assert.AreEqual(expected, saturday.AddWeekdays(2));
        }

        [TestMethod]
        public void Test_AddWeekday_Backward_WeekdaysOnly()
        {
            DateTime tuesday = new DateTime(2021, 4, 6);
            DateTime expected = new DateTime(2021, 4, 5);
            Assert.AreEqual(expected, tuesday.AddWeekdays(-1));
        }

        [TestMethod]
        public void Test_AddWeekday_Backward_OverWeekend()
        {
            DateTime monday = new DateTime(2021, 4, 12);
            DateTime expected = new DateTime(2021, 4, 9);
            Assert.AreEqual(expected, monday.AddWeekdays(-1));
        }

        [TestMethod]
        public void Test_AddWeekday_Backward_OnWeekend()
        {
            DateTime saturday = new DateTime(2021, 4, 10);
            DateTime expected = new DateTime(2021, 4, 9);
            Assert.AreEqual(expected, saturday.AddWeekdays(-1));
        }

        [TestMethod]
        public void Test_AddWeekday_Backward_Multiple_WeekdaysOnly()
        {
            DateTime wednesday = new DateTime(2021, 4, 7);
            DateTime expected = new DateTime(2021, 4, 5);
            Assert.AreEqual(expected, wednesday.AddWeekdays(-2));
        }

        [TestMethod]
        public void Test_AddWeekday_Backward_Multiple_OverWeekend()
        {
            DateTime monday = new DateTime(2021, 4, 12);
            DateTime expected = new DateTime(2021, 4, 8);
            Assert.AreEqual(expected, monday.AddWeekdays(-2));
        }

        [TestMethod]
        public void Test_AddWeekday_Backward_Multiple_OnWeekend()
        {
            DateTime saturday = new DateTime(2021, 4, 10);
            DateTime expected = new DateTime(2021, 4, 8);
            Assert.AreEqual(expected, saturday.AddWeekdays(-2));
        }

        [TestMethod]
        public void Test_NextWeekday_Monday()
        {
            DateTime monday = new DateTime(2021, 4, 5);
            DateTime expected = new DateTime(2021, 4, 6);
            Assert.AreEqual(expected, monday.NextWeekday());
        }

        [TestMethod]
        public void Test_NextWeekday_Tuesday()
        {
            DateTime tuesday = new DateTime(2021, 4, 6);
            DateTime expected = new DateTime(2021, 4, 7);
            Assert.AreEqual(expected, tuesday.NextWeekday());
        }

        [TestMethod]
        public void Test_NextWeekday_Wednesday()
        {
            DateTime wednesday = new DateTime(2021, 4, 7);
            DateTime expected = new DateTime(2021, 4, 8);
            Assert.AreEqual(expected, wednesday.NextWeekday());
        }

        [TestMethod]
        public void Test_NextWeekday_Thursday()
        {
            DateTime thursday = new DateTime(2021, 4, 8);
            DateTime expected = new DateTime(2021, 4, 9);
            Assert.AreEqual(expected, thursday.NextWeekday());
        }

        [TestMethod]
        public void Test_NextWeekday_Friday()
        {
            DateTime friday = new DateTime(2021, 4, 9);
            DateTime expected = new DateTime(2021, 4, 12);
            Assert.AreEqual(expected, friday.NextWeekday());
        }

        [TestMethod]
        public void Test_NextWeekday_Saturday()
        {
            DateTime saturday = new DateTime(2021, 4, 10);
            DateTime expected = new DateTime(2021, 4, 12);
            Assert.AreEqual(expected, saturday.NextWeekday());
        }

        [TestMethod]
        public void Test_NextWeekday_Sunday()
        {
            DateTime sunday = new DateTime(2021, 4, 11);
            DateTime expected = new DateTime(2021, 4, 12);
            Assert.AreEqual(expected, sunday.NextWeekday());
        }

        [TestMethod]
        public void Test_NextWeekdayOrDate_Monday()
        {
            DateTime monday = new DateTime(2021, 4, 5);
            DateTime expected = new DateTime(2021, 4, 5);
            Assert.AreEqual(expected, monday.NextWeekdayOrDate());
        }

        [TestMethod]
        public void Test_NextWeekdayOrDate_Tuesday()
        {
            DateTime tuesday = new DateTime(2021, 4, 6);
            DateTime expected = new DateTime(2021, 4, 6);
            Assert.AreEqual(expected, tuesday.NextWeekdayOrDate());
        }

        [TestMethod]
        public void Test_NextWeekdayOrDate_Wednesday()
        {
            DateTime wednesday = new DateTime(2021, 4, 7);
            DateTime expected = new DateTime(2021, 4, 7);
            Assert.AreEqual(expected, wednesday.NextWeekdayOrDate());
        }

        [TestMethod]
        public void Test_NextWeekdayOrDate_Thursday()
        {
            DateTime thursday = new DateTime(2021, 4, 8);
            DateTime expected = new DateTime(2021, 4, 8);
            Assert.AreEqual(expected, thursday.NextWeekdayOrDate());
        }

        [TestMethod]
        public void Test_NextWeekdayOrDate_Friday()
        {
            DateTime friday = new DateTime(2021, 4, 9);
            DateTime expected = new DateTime(2021, 4, 9);
            Assert.AreEqual(expected, friday.NextWeekdayOrDate());
        }

        [TestMethod]
        public void Test_NextWeekdayOrDate_Saturday()
        {
            DateTime saturday = new DateTime(2021, 4, 10);
            DateTime expected = new DateTime(2021, 4, 12);
            Assert.AreEqual(expected, saturday.NextWeekdayOrDate());
        }

        [TestMethod]
        public void Test_NextWeekdayOrDate_Sunday()
        {
            DateTime sunday = new DateTime(2021, 4, 11);
            DateTime expected = new DateTime(2021, 4, 12);
            Assert.AreEqual(expected, sunday.NextWeekdayOrDate());
        }

        [TestMethod]
        public void Test_LastWeekday_Monday()
        {
            DateTime monday = new DateTime(2021, 4, 5);
            DateTime expected = new DateTime(2021, 4, 2);
            Assert.AreEqual(expected, monday.LastWeekday());
        }

        [TestMethod]
        public void Test_LastWeekday_Tuesday()
        {
            DateTime tuesday = new DateTime(2021, 4, 6);
            DateTime expected = new DateTime(2021, 4, 5);
            Assert.AreEqual(expected, tuesday.LastWeekday());
        }

        [TestMethod]
        public void Test_LastWeekday_Wednesday()
        {
            DateTime wednesday = new DateTime(2021, 4, 7);
            DateTime expected = new DateTime(2021, 4, 6);
            Assert.AreEqual(expected, wednesday.LastWeekday());
        }

        [TestMethod]
        public void Test_LastWeekday_Thursday()
        {
            DateTime thursday = new DateTime(2021, 4, 8);
            DateTime expected = new DateTime(2021, 4, 7);
            Assert.AreEqual(expected, thursday.LastWeekday());
        }

        [TestMethod]
        public void Test_LastWeekday_Friday()
        {
            DateTime friday = new DateTime(2021, 4, 9);
            DateTime expected = new DateTime(2021, 4, 8);
            Assert.AreEqual(expected, friday.LastWeekday());
        }

        [TestMethod]
        public void Test_LastWeekday_Saturday()
        {
            DateTime saturday = new DateTime(2021, 4, 10);
            DateTime expected = new DateTime(2021, 4, 9);
            Assert.AreEqual(expected, saturday.LastWeekday());
        }

        [TestMethod]
        public void Test_LastWeekday_Sunday()
        {
            DateTime sunday = new DateTime(2021, 4, 11);
            DateTime expected = new DateTime(2021, 4, 9);
            Assert.AreEqual(expected, sunday.LastWeekday());
        }

        [TestMethod]
        public void Test_LastWeekdayOrDate_Monday()
        {
            DateTime monday = new DateTime(2021, 4, 5);
            DateTime expected = new DateTime(2021, 4, 5);
            Assert.AreEqual(expected, monday.LastWeekdayOrDate());
        }

        [TestMethod]
        public void Test_LastWeekdayOrDate_Tuesday()
        {
            DateTime tuesday = new DateTime(2021, 4, 6);
            DateTime expected = new DateTime(2021, 4, 6);
            Assert.AreEqual(expected, tuesday.LastWeekdayOrDate());
        }

        [TestMethod]
        public void Test_LastWeekdayOrDate_Wednesday()
        {
            DateTime wednesday = new DateTime(2021, 4, 7);
            DateTime expected = new DateTime(2021, 4, 7);
            Assert.AreEqual(expected, wednesday.LastWeekdayOrDate());
        }

        [TestMethod]
        public void Test_LastWeekdayOrDate_Thursday()
        {
            DateTime thursday = new DateTime(2021, 4, 8);
            DateTime expected = new DateTime(2021, 4, 8);
            Assert.AreEqual(expected, thursday.LastWeekdayOrDate());
        }

        [TestMethod]
        public void Test_LastWeekdayOrDate_Friday()
        {
            DateTime friday = new DateTime(2021, 4, 9);
            DateTime expected = new DateTime(2021, 4, 9);
            Assert.AreEqual(expected, friday.LastWeekdayOrDate());
        }

        [TestMethod]
        public void Test_LastWeekdayOrDate_Saturday()
        {
            DateTime saturday = new DateTime(2021, 4, 10);
            DateTime expected = new DateTime(2021, 4, 9);
            Assert.AreEqual(expected, saturday.LastWeekdayOrDate());
        }

        [TestMethod]
        public void Test_LastWeekdayOrDate_Sunday()
        {
            DateTime sunday = new DateTime(2021, 4, 11);
            DateTime expected = new DateTime(2021, 4, 9);
            Assert.AreEqual(expected, sunday.LastWeekdayOrDate());
        }

        [TestMethod]
        public void Test_StartOfMonth_WeekdayStart()
        {
            DateTime midMonth = new DateTime(2022, 3, 2);
            DateTime expected = new DateTime(2022, 3, 1);
            Assert.AreEqual(expected, midMonth.StartOfMonth());
        }

        [TestMethod]
        public void Test_StartOfMonth_WeekendStart()
        {
            DateTime midMonth = new DateTime(2022, 1, 12);
            DateTime expected = new DateTime(2022, 1, 1);
            Assert.AreEqual(expected, midMonth.StartOfMonth());
        }

        [TestMethod]
        public void Test_EndOfMonth_WeekdayEnd()
        {
            DateTime midMonth = new DateTime(2022, 3, 2);
            DateTime expected = new DateTime(2022, 3, 31);
            Assert.AreEqual(expected, midMonth.EndOfMonth());
        }

        [TestMethod]
        public void Test_EndOfMonth_WeekendEnd()
        {
            DateTime midMonth = new DateTime(2022, 4, 12);
            DateTime expected = new DateTime(2022, 4, 30);
            Assert.AreEqual(expected, midMonth.EndOfMonth());
        }

        [TestMethod]
        public void Test_StartOfYear_WeekdayStart()
        {
            DateTime date = new DateTime(2021, 1, 6);
            DateTime expected = new DateTime(2021, 1, 1);
            Assert.AreEqual(expected, date.StartOfYear());
        }

        [TestMethod]
        public void Test_StartOfYear_WeekendStart()
        {
            DateTime date = new DateTime(2022, 1, 12);
            DateTime expected = new DateTime(2022, 1, 1);
            Assert.AreEqual(expected, date.StartOfYear());
        }

        [TestMethod]
        public void Test_EndOfYear_WeekdayEnd()
        {
            DateTime date = new DateTime(2021, 3, 2);
            DateTime expected = new DateTime(2021, 12, 31);
            Assert.AreEqual(expected, date.EndOfYear());
        }

        [TestMethod]
        public void Test_EndOfYear_WeekendEnd()
        {
            DateTime date = new DateTime(2022, 4, 12);
            DateTime expected = new DateTime(2022, 12, 31);
            Assert.AreEqual(expected, date.EndOfYear());
        }

        [TestMethod]
        public void Test_NextDayOfWeek()
        {
            DateTime sat = new DateTime(2021, 8, 7);
            Assert.AreEqual(new DateTime(2021, 8, 8), sat.NextDayOfWeek(DayOfWeek.Sunday));
            Assert.AreEqual(new DateTime(2021, 8, 9), sat.NextDayOfWeek(DayOfWeek.Monday));
            Assert.AreEqual(new DateTime(2021, 8, 14), sat.NextDayOfWeek(DayOfWeek.Saturday));
        }

        [TestMethod]
        public void Test_NextDayOfWeekOrSame()
        {
            DateTime sat = new DateTime(2021, 8, 7);
            Assert.AreEqual(new DateTime(2021, 8, 8), sat.NextDayOfWeekOrSame(DayOfWeek.Sunday));
            Assert.AreEqual(new DateTime(2021, 8, 9), sat.NextDayOfWeekOrSame(DayOfWeek.Monday));
            Assert.AreEqual(new DateTime(2021, 8, 7), sat.NextDayOfWeekOrSame(DayOfWeek.Saturday));
        }
    }
}
