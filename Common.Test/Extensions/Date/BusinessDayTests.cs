using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Common.Test.Extensions.Date
{
    [TestClass]
    public class BusinessDayTests
    {
        [TestMethod]
        public void Test_IsBusinessDay_Monday_NoHoliday()
        {
            DateTime monday = new DateTime(2021, 4, 5);
            Assert.IsTrue(monday.IsBusinessDay());
        }

        [TestMethod]
        public void Test_IsBusinessDay_Tuesday_NoHoliday()
        {
            DateTime tuesday = new DateTime(2021, 4, 6);
            Assert.IsTrue(tuesday.IsBusinessDay());
        }

        [TestMethod]
        public void Test_IsBusinessDay_Wednesday_NoHoliday()
        {
            DateTime wednesday = new DateTime(2021, 4, 7);
            Assert.IsTrue(wednesday.IsBusinessDay());
        }

        [TestMethod]
        public void Test_IsBusinessDay_Thursday_NoHoliday()
        {
            DateTime thursday = new DateTime(2021, 4, 8);
            Assert.IsTrue(thursday.IsBusinessDay());
        }

        [TestMethod]
        public void Test_IsBusinessDay_Friday_NoHoliday()
        {
            DateTime friday = new DateTime(2021, 4, 9);
            Assert.IsTrue(friday.IsBusinessDay());
        }

        [TestMethod]
        public void Test_IsBusinessDay_Saturday_NoHoliday()
        {
            DateTime saturday = new DateTime(2021, 4, 10);
            Assert.IsFalse(saturday.IsBusinessDay());
        }

        [TestMethod]
        public void Test_IsBusinessDay_Sunday_NoHoliday()
        {
            DateTime sunday = new DateTime(2021, 4, 11);
            Assert.IsFalse(sunday.IsBusinessDay());
        }

        [TestMethod]
        public void Test_IsBusinessDay_Monday_IsHoliday()
        {
            DateTime monday = new DateTime(2021, 4, 5);
            Assert.IsFalse(monday.IsBusinessDay(monday));
        }

        [TestMethod]
        public void Test_IsBusinessDay_Tuesday_IsHoliday()
        {
            DateTime tuesday = new DateTime(2021, 4, 6);
            Assert.IsFalse(tuesday.IsBusinessDay(tuesday));
        }

        [TestMethod]
        public void Test_IsBusinessDay_Wednesday_IsHoliday()
        {
            DateTime wednesday = new DateTime(2021, 4, 7);
            Assert.IsFalse(wednesday.IsBusinessDay(wednesday));
        }

        [TestMethod]
        public void Test_IsBusinessDay_Thursday_IsHoliday()
        {
            DateTime thursday = new DateTime(2021, 4, 8);
            Assert.IsFalse(thursday.IsBusinessDay(thursday));
        }

        [TestMethod]
        public void Test_IsBusinessDay_Friday_IsHoliday()
        {
            DateTime friday = new DateTime(2021, 4, 9);
            Assert.IsFalse(friday.IsBusinessDay(friday));
        }

        [TestMethod]
        public void Test_IsBusinessDay_Saturday_IsHoliday()
        {
            DateTime saturday = new DateTime(2021, 4, 10);
            Assert.IsFalse(saturday.IsBusinessDay(saturday));
        }

        [TestMethod]
        public void Test_IsBusinessDay_Sunday_IsHoliday()
        {
            DateTime sunday = new DateTime(2021, 4, 11);
            Assert.IsFalse(sunday.IsBusinessDay(sunday));
        }

        [TestMethod]
        public void Test_IsBusinessDay_Monday_NoHoliday_WithTimeStamp()
        {
            DateTime monday = new DateTime(2021, 4, 5, 12, 0, 0);
            Assert.IsTrue(monday.IsBusinessDay());
        }

        [TestMethod]
        public void Test_IsBusinessDay_Tuesday_NoHoliday_WithTimeStamp()
        {
            DateTime tuesday = new DateTime(2021, 4, 6, 12, 0, 0);
            Assert.IsTrue(tuesday.IsBusinessDay());
        }

        [TestMethod]
        public void Test_IsBusinessDay_Wednesday_NoHoliday_WithTimeStamp()
        {
            DateTime wednesday = new DateTime(2021, 4, 7, 12, 0, 0);
            Assert.IsTrue(wednesday.IsBusinessDay());
        }

        [TestMethod]
        public void Test_IsBusinessDay_Thursday_NoHoliday_WithTimeStamp()
        {
            DateTime thursday = new DateTime(2021, 4, 8, 12, 0, 0);
            Assert.IsTrue(thursday.IsBusinessDay());
        }

        [TestMethod]
        public void Test_IsBusinessDay_Friday_NoHoliday_WithTimeStamp()
        {
            DateTime friday = new DateTime(2021, 4, 9, 12, 0, 0);
            Assert.IsTrue(friday.IsBusinessDay());
        }

        [TestMethod]
        public void Test_IsBusinessDay_Saturday_NoHoliday_WithTimeStamp()
        {
            DateTime saturday = new DateTime(2021, 4, 10, 12, 0, 0);
            Assert.IsFalse(saturday.IsBusinessDay());
        }

        [TestMethod]
        public void Test_IsBusinessDay_Sunday_NoHoliday_WithTimeStamp()
        {
            DateTime sunday = new DateTime(2021, 4, 11, 12, 0, 0);
            Assert.IsFalse(sunday.IsBusinessDay());
        }

        [TestMethod]
        public void Test_IsBusinessDay_Monday_IsHoliday_WithTimeStamp()
        {
            DateTime monday = new DateTime(2021, 4, 5, 12, 0, 0);
            Assert.IsFalse(monday.IsBusinessDay(monday.Date));
        }

        [TestMethod]
        public void Test_IsBusinessDay_Tuesday_IsHoliday_WithTimeStamp()
        {
            DateTime tuesday = new DateTime(2021, 4, 6, 12, 0, 0);
            Assert.IsFalse(tuesday.IsBusinessDay(tuesday.Date));
        }

        [TestMethod]
        public void Test_IsBusinessDay_Wednesday_IsHoliday_WithTimeStamp()
        {
            DateTime wednesday = new DateTime(2021, 4, 7, 12, 0, 0);
            Assert.IsFalse(wednesday.IsBusinessDay(wednesday.Date));
        }

        [TestMethod]
        public void Test_IsBusinessDay_Thursday_IsHoliday_WithTimeStamp()
        {
            DateTime thursday = new DateTime(2021, 4, 8, 12, 0, 0);
            Assert.IsFalse(thursday.IsBusinessDay(thursday.Date));
        }

        [TestMethod]
        public void Test_IsBusinessDay_Friday_IsHoliday_WithTimeStamp()
        {
            DateTime friday = new DateTime(2021, 4, 9, 12, 0, 0);
            Assert.IsFalse(friday.IsBusinessDay(friday.Date));
        }

        [TestMethod]
        public void Test_IsBusinessDay_Saturday_IsHoliday_WithTimeStamp()
        {
            DateTime saturday = new DateTime(2021, 4, 10, 12, 0, 0);
            Assert.IsFalse(saturday.IsBusinessDay(saturday.Date));
        }

        [TestMethod]
        public void Test_IsBusinessDay_Sunday_IsHoliday_WithTimeStamp()
        {
            DateTime sunday = new DateTime(2021, 4, 11, 12, 0, 0);
            Assert.IsFalse(sunday.IsBusinessDay(sunday.Date));
        }

        [TestMethod]
        public void Test_AddBusinessDay_Zero_NoHolidayReturnsDate()
        {
            DateTime monday = new DateTime(2021, 4, 5);
            Assert.AreEqual(monday, monday.AddBusinessDays(0));
        }

        [TestMethod]
        public void Test_AddBusinessDay_Zero_WithHolidayReturnsDate()
        {
            DateTime monday = new DateTime(2021, 4, 5);
            Assert.AreEqual(monday, monday.AddBusinessDays(0, holidays: monday));
        }

        [TestMethod]
        public void Test_AddBusinessDay_Forward_NoHolidays_WeekdaysOnly()
        {
            DateTime monday = new DateTime(2021, 4, 5);
            DateTime tuesday = new DateTime(2021, 4, 6);
            Assert.AreEqual(tuesday, monday.AddBusinessDays(1));
        }

        [TestMethod]
        public void Test_AddBusinessDay_Forward_NoHolidays_OverWeekend()
        {
            DateTime friday = new DateTime(2021, 4, 9);
            DateTime expected = new DateTime(2021, 4, 12);
            Assert.AreEqual(expected, friday.AddBusinessDays(1));
        }

        [TestMethod]
        public void Test_AddBusinessDay_Forward_NoHolidays_OnWeekend()
        {
            DateTime saturday = new DateTime(2021, 4, 10);
            DateTime expected = new DateTime(2021, 4, 12);
            Assert.AreEqual(expected, saturday.AddBusinessDays(1));
        }

        [TestMethod]
        public void Test_AddBusinessDay_Forward_WithHolidays_WeekdaysOnly()
        {
            DateTime monday = new DateTime(2021, 4, 5);
            DateTime tuesday = new DateTime(2021, 4, 6);
            DateTime wednesday = new DateTime(2021, 4, 7);
            Assert.AreEqual(wednesday, monday.AddBusinessDays(1, holidays: tuesday));
        }

        [TestMethod]
        public void Test_AddBusinessDay_Forward_WithHolidays_OverWeekend()
        {
            DateTime friday = new DateTime(2021, 4, 9);
            DateTime monday = new DateTime(2021, 4, 12);
            DateTime expected = new DateTime(2021, 4, 13);
            Assert.AreEqual(expected, friday.AddBusinessDays(1, holidays: monday));
        }

        [TestMethod]
        public void Test_AddBusinessDay_Forward_WithHolidays_OnWeekend()
        {
            DateTime saturday = new DateTime(2021, 4, 10);
            DateTime monday = new DateTime(2021, 4, 12);
            DateTime expected = new DateTime(2021, 4, 13);
            Assert.AreEqual(expected, saturday.AddBusinessDays(1, holidays: monday));
        }

        [TestMethod]
        public void Test_AddBusinessDay_Forward_Multiple_NoHolidays_WeekdaysOnly()
        {
            DateTime monday = new DateTime(2021, 4, 5);
            DateTime expected = new DateTime(2021, 4, 7);
            Assert.AreEqual(expected, monday.AddBusinessDays(2));
        }

        [TestMethod]
        public void Test_AddBusinessDay_Forward_Multiple_NoHolidays_OverWeekend()
        {
            DateTime friday = new DateTime(2021, 4, 9);
            DateTime expected = new DateTime(2021, 4, 13);
            Assert.AreEqual(expected, friday.AddBusinessDays(2));
        }

        [TestMethod]
        public void Test_AddBusinessDay_Forward_Multiple_NoHolidays_OnWeekend()
        {
            DateTime saturday = new DateTime(2021, 4, 10);
            DateTime expected = new DateTime(2021, 4, 13);
            Assert.AreEqual(expected, saturday.AddBusinessDays(2));
        }

        [TestMethod]
        public void Test_AddBusinessDay_Forward_Multiple_WithHolidays_WeekdaysOnly()
        {
            DateTime monday = new DateTime(2021, 4, 5);
            DateTime tuesday = new DateTime(2021, 4, 6);
            DateTime expected = new DateTime(2021, 4, 8);
            Assert.AreEqual(expected, monday.AddBusinessDays(2, holidays: tuesday));
        }

        [TestMethod]
        public void Test_AddBusinessDay_Forward_Multiple_WithHolidays_OverWeekend()
        {
            DateTime friday = new DateTime(2021, 4, 9);
            DateTime monday = new DateTime(2021, 4, 12);
            DateTime expected = new DateTime(2021, 4, 14);
            Assert.AreEqual(expected, friday.AddBusinessDays(2, holidays: monday));
        }

        [TestMethod]
        public void Test_AddBusinessDay_Forward_Multiple_WithHolidays_OnWeekend()
        {
            DateTime saturday = new DateTime(2021, 4, 10);
            DateTime monday = new DateTime(2021, 4, 12);
            DateTime expected = new DateTime(2021, 4, 14);
            Assert.AreEqual(expected, saturday.AddBusinessDays(2, holidays: monday));
        }

        [TestMethod]
        public void Test_AddBusinessDay_Backward_NoHolidays_WeekdaysOnly()
        {
            DateTime tuesday = new DateTime(2021, 4, 6);
            DateTime expected = new DateTime(2021, 4, 5);
            Assert.AreEqual(expected, tuesday.AddBusinessDays(-1));
        }

        [TestMethod]
        public void Test_AddBusinessDay_Backward_NoHolidays_OverWeekend()
        {
            DateTime monday = new DateTime(2021, 4, 12);
            DateTime expected = new DateTime(2021, 4, 9);
            Assert.AreEqual(expected, monday.AddBusinessDays(-1));
        }

        [TestMethod]
        public void Test_AddBusinessDay_Backward_NoHolidays_OnWeekend()
        {
            DateTime saturday = new DateTime(2021, 4, 10);
            DateTime expected = new DateTime(2021, 4, 9);
            Assert.AreEqual(expected, saturday.AddBusinessDays(-1));
        }

        [TestMethod]
        public void Test_AddBusinessDay_Backward_WithHolidays_WeekdaysOnly()
        {
            DateTime wednesday = new DateTime(2021, 4, 7);
            DateTime tuesday = new DateTime(2021, 4, 6);
            DateTime expected = new DateTime(2021, 4, 5);
            Assert.AreEqual(expected, wednesday.AddBusinessDays(-1, holidays: tuesday));
        }

        [TestMethod]
        public void Test_AddBusinessDay_Backward_WithHolidays_OverWeekend()
        {
            DateTime monday = new DateTime(2021, 4, 12);
            DateTime friday = new DateTime(2021, 4, 9);
            DateTime expected = new DateTime(2021, 4, 8);
            Assert.AreEqual(expected, monday.AddBusinessDays(-1, holidays: friday));
        }

        [TestMethod]
        public void Test_AddBusinessDay_Backward_WithHolidays_OnWeekend()
        {
            DateTime saturday = new DateTime(2021, 4, 10);
            DateTime friday = new DateTime(2021, 4, 9);
            DateTime expected = new DateTime(2021, 4, 8);
            Assert.AreEqual(expected, saturday.AddBusinessDays(-1, holidays: friday));
        }

        [TestMethod]
        public void Test_AddBusinessDay_Backward_Multiple_NoHolidays_WeekdaysOnly()
        {
            DateTime wednesday = new DateTime(2021, 4, 7);
            DateTime expected = new DateTime(2021, 4, 5);
            Assert.AreEqual(expected, wednesday.AddBusinessDays(-2));
        }

        [TestMethod]
        public void Test_AddBusinessDay_Backward_Multiple_NoHolidays_OverWeekend()
        {
            DateTime monday = new DateTime(2021, 4, 12);
            DateTime expected = new DateTime(2021, 4, 8);
            Assert.AreEqual(expected, monday.AddBusinessDays(-2));
        }

        [TestMethod]
        public void Test_AddBusinessDay_Backward_Multiple_NoHolidays_OnWeekend()
        {
            DateTime saturday = new DateTime(2021, 4, 10);
            DateTime expected = new DateTime(2021, 4, 8);
            Assert.AreEqual(expected, saturday.AddBusinessDays(-2));
        }

        [TestMethod]
        public void Test_AddBusinessDay_Backward_Multiple_WithHolidays_WeekdaysOnly()
        {
            DateTime friday = new DateTime(2021, 4, 9);
            DateTime thursday = new DateTime(2021, 4, 8);
            DateTime expected = new DateTime(2021, 4, 6);
            Assert.AreEqual(expected, friday.AddBusinessDays(-2, holidays: thursday));
        }

        [TestMethod]
        public void Test_AddBusinessDay_Backward_Multiple_WithHolidays_OverWeekend()
        {
            DateTime monday = new DateTime(2021, 4, 12);
            DateTime friday = new DateTime(2021, 4, 9);
            DateTime expected = new DateTime(2021, 4, 7);
            Assert.AreEqual(expected, monday.AddBusinessDays(-2, holidays: friday));
        }

        [TestMethod]
        public void Test_AddBusinessDay_Backward_Multiple_WithHolidays_OnWeekend()
        {
            DateTime saturday = new DateTime(2021, 4, 10);
            DateTime friday = new DateTime(2021, 4, 9);
            DateTime expected = new DateTime(2021, 4, 7);
            Assert.AreEqual(expected, saturday.AddBusinessDays(-2, holidays: friday));
        }

        [TestMethod]
        public void Test_NextBusinessDay_Monday_NoHoliday()
        {
            DateTime monday = new DateTime(2021, 4, 5);
            DateTime expected = new DateTime(2021, 4, 6);
            Assert.AreEqual(expected, monday.NextBusinessDay());
        }

        [TestMethod]
        public void Test_NextBusinessDay_Tuesday_NoHoliday()
        {
            DateTime tuesday = new DateTime(2021, 4, 6);
            DateTime expected = new DateTime(2021, 4, 7);
            Assert.AreEqual(expected, tuesday.NextBusinessDay());
        }

        [TestMethod]
        public void Test_NextBusinessDay_Wednesday_NoHoliday()
        {
            DateTime wednesday = new DateTime(2021, 4, 7);
            DateTime expected = new DateTime(2021, 4, 8);
            Assert.AreEqual(expected, wednesday.NextBusinessDay());
        }

        [TestMethod]
        public void Test_NextBusinessDay_Thursday_NoHoliday()
        {
            DateTime thursday = new DateTime(2021, 4, 8);
            DateTime expected = new DateTime(2021, 4, 9);
            Assert.AreEqual(expected, thursday.NextBusinessDay());
        }

        [TestMethod]
        public void Test_NextBusinessDay_Friday_NoHoliday()
        {
            DateTime friday = new DateTime(2021, 4, 9);
            DateTime expected = new DateTime(2021, 4, 12);
            Assert.AreEqual(expected, friday.NextBusinessDay());
        }

        [TestMethod]
        public void Test_NextBusinessDay_Saturday_NoHoliday()
        {
            DateTime saturday = new DateTime(2021, 4, 10);
            DateTime expected = new DateTime(2021, 4, 12);
            Assert.AreEqual(expected, saturday.NextBusinessDay());
        }

        [TestMethod]
        public void Test_NextBusinessDay_Sunday_NoHoliday()
        {
            DateTime sunday = new DateTime(2021, 4, 11);
            DateTime expected = new DateTime(2021, 4, 12);
            Assert.AreEqual(expected, sunday.NextBusinessDay());
        }

        [TestMethod]
        public void Test_NextBusinessDay_Monday_IsHoliday()
        {
            DateTime monday = new DateTime(2021, 4, 5);
            DateTime holiday = new DateTime(2021, 4, 6);
            DateTime expected = new DateTime(2021, 4, 7);
            Assert.AreEqual(expected, monday.NextBusinessDay(holidays: holiday));
        }

        [TestMethod]
        public void Test_NextBusinessDay_Tuesday_IsHoliday()
        {
            DateTime tuesday = new DateTime(2021, 4, 6);
            DateTime holiday = new DateTime(2021, 4, 7);
            DateTime expected = new DateTime(2021, 4, 8);
            Assert.AreEqual(expected, tuesday.NextBusinessDay(holidays: holiday));
        }

        [TestMethod]
        public void Test_NextBusinessDay_Wednesday_IsHoliday()
        {
            DateTime wednesday = new DateTime(2021, 4, 7);
            DateTime holiday = new DateTime(2021, 4, 8);
            DateTime expected = new DateTime(2021, 4, 9);
            Assert.AreEqual(expected, wednesday.NextBusinessDay(holidays: holiday));
        }

        [TestMethod]
        public void Test_NextBusinessDay_Thursday_IsHoliday()
        {
            DateTime thursday = new DateTime(2021, 4, 8);
            DateTime holiday = new DateTime(2021, 4, 9);
            DateTime expected = new DateTime(2021, 4, 12);
            Assert.AreEqual(expected, thursday.NextBusinessDay(holidays: holiday));
        }

        [TestMethod]
        public void Test_NextBusinessDay_Friday_IsHoliday()
        {
            DateTime friday = new DateTime(2021, 4, 9);
            DateTime holiday = new DateTime(2021, 4, 12);
            DateTime expected = new DateTime(2021, 4, 13);
            Assert.AreEqual(expected, friday.NextBusinessDay(holidays: holiday));
        }

        [TestMethod]
        public void Test_NextBusinessDay_Saturday_IsHoliday()
        {
            DateTime saturday = new DateTime(2021, 4, 10);
            DateTime holiday = new DateTime(2021, 4, 12);
            DateTime expected = new DateTime(2021, 4, 13);
            Assert.AreEqual(expected, saturday.NextBusinessDay(holidays: holiday));
        }

        [TestMethod]
        public void Test_NextBusinessDay_Sunday_IsHoliday()
        {
            DateTime sunday = new DateTime(2021, 4, 11);
            DateTime holiday = new DateTime(2021, 4, 12);
            DateTime expected = new DateTime(2021, 4, 13);
            Assert.AreEqual(expected, sunday.NextBusinessDay(holidays: holiday));
        }

        [TestMethod]
        public void Test_NextBusinessDayOrDate_Monday_NoHoliday()
        {
            DateTime monday = new DateTime(2021, 4, 5);
            DateTime expected = new DateTime(2021, 4, 5);
            Assert.AreEqual(expected, monday.NextBusinessDayOrDate());
        }

        [TestMethod]
        public void Test_NextBusinessDayOrDate_Tuesday_NoHoliday()
        {
            DateTime tuesday = new DateTime(2021, 4, 6);
            DateTime expected = new DateTime(2021, 4, 6);
            Assert.AreEqual(expected, tuesday.NextBusinessDayOrDate());
        }

        [TestMethod]
        public void Test_NextBusinessDayOrDate_Wednesday_NoHoliday()
        {
            DateTime wednesday = new DateTime(2021, 4, 7);
            DateTime expected = new DateTime(2021, 4, 7);
            Assert.AreEqual(expected, wednesday.NextBusinessDayOrDate());
        }

        [TestMethod]
        public void Test_NextBusinessDayOrDate_Thursday_NoHoliday()
        {
            DateTime thursday = new DateTime(2021, 4, 8);
            DateTime expected = new DateTime(2021, 4, 8);
            Assert.AreEqual(expected, thursday.NextBusinessDayOrDate());
        }

        [TestMethod]
        public void Test_NextBusinessDayOrDate_Friday_NoHoliday()
        {
            DateTime friday = new DateTime(2021, 4, 9);
            DateTime expected = new DateTime(2021, 4, 9);
            Assert.AreEqual(expected, friday.NextBusinessDayOrDate());
        }

        [TestMethod]
        public void Test_NextBusinessDayOrDate_Saturday_NoHoliday()
        {
            DateTime saturday = new DateTime(2021, 4, 10);
            DateTime expected = new DateTime(2021, 4, 12);
            Assert.AreEqual(expected, saturday.NextBusinessDayOrDate());
        }

        [TestMethod]
        public void Test_NextBusinessDayOrDate_Sunday_NoHoliday()
        {
            DateTime sunday = new DateTime(2021, 4, 11);
            DateTime expected = new DateTime(2021, 4, 12);
            Assert.AreEqual(expected, sunday.NextBusinessDayOrDate());
        }

        [TestMethod]
        public void Test_NextBusinessDayOrDate_Monday_IsHoliday()
        {
            DateTime monday = new DateTime(2021, 4, 5);
            DateTime holiday = new DateTime(2021, 4, 5);
            DateTime expected = new DateTime(2021, 4, 6);
            Assert.AreEqual(expected, monday.NextBusinessDayOrDate(holidays: holiday));
        }

        [TestMethod]
        public void Test_NextBusinessDayOrDate_Tuesday_IsHoliday()
        {
            DateTime tuesday = new DateTime(2021, 4, 6);
            DateTime holiday = new DateTime(2021, 4, 6);
            DateTime expected = new DateTime(2021, 4, 7);
            Assert.AreEqual(expected, tuesday.NextBusinessDayOrDate(holidays: holiday));
        }

        [TestMethod]
        public void Test_NextBusinessDayOrDate_Wednesday_IsHoliday()
        {
            DateTime wednesday = new DateTime(2021, 4, 7);
            DateTime holiday = new DateTime(2021, 4, 7);
            DateTime expected = new DateTime(2021, 4, 8);
            Assert.AreEqual(expected, wednesday.NextBusinessDayOrDate(holidays: holiday));
        }

        [TestMethod]
        public void Test_NextBusinessDayOrDate_Thursday_IsHoliday()
        {
            DateTime thursday = new DateTime(2021, 4, 8);
            DateTime holiday = new DateTime(2021, 4, 8);
            DateTime expected = new DateTime(2021, 4, 9);
            Assert.AreEqual(expected, thursday.NextBusinessDayOrDate(holidays: holiday));
        }

        [TestMethod]
        public void Test_NextBusinessDayOrDate_Friday_IsHoliday()
        {
            DateTime friday = new DateTime(2021, 4, 9);
            DateTime holiday = new DateTime(2021, 4, 9);
            DateTime expected = new DateTime(2021, 4, 12);
            Assert.AreEqual(expected, friday.NextBusinessDayOrDate(holidays: holiday));
        }

        [TestMethod]
        public void Test_NextBusinessDayOrDate_Saturday_IsHoliday()
        {
            DateTime saturday = new DateTime(2021, 4, 10);
            DateTime holiday = new DateTime(2021, 4, 12);
            DateTime expected = new DateTime(2021, 4, 13);
            Assert.AreEqual(expected, saturday.NextBusinessDayOrDate(holidays: holiday));
        }

        [TestMethod]
        public void Test_NextBusinessDayOrDate_Sunday_IsHoliday()
        {
            DateTime sunday = new DateTime(2021, 4, 11);
            DateTime holiday = new DateTime(2021, 4, 12);
            DateTime expected = new DateTime(2021, 4, 13);
            Assert.AreEqual(expected, sunday.NextBusinessDayOrDate(holidays: holiday));
        }

        [TestMethod]
        public void Test_LastBusinessDay_Monday_NoHoliday()
        {
            DateTime monday = new DateTime(2021, 4, 5);
            DateTime expected = new DateTime(2021, 4, 2);
            Assert.AreEqual(expected, monday.LastBusinessDay());
        }

        [TestMethod]
        public void Test_LastBusinessDay_Tuesday_NoHoliday()
        {
            DateTime tuesday = new DateTime(2021, 4, 6);
            DateTime expected = new DateTime(2021, 4, 5);
            Assert.AreEqual(expected, tuesday.LastBusinessDay());
        }

        [TestMethod]
        public void Test_LastBusinessDay_Wednesday_NoHoliday()
        {
            DateTime wednesday = new DateTime(2021, 4, 7);
            DateTime expected = new DateTime(2021, 4, 6);
            Assert.AreEqual(expected, wednesday.LastBusinessDay());
        }

        [TestMethod]
        public void Test_LastBusinessDay_Thursday_NoHoliday()
        {
            DateTime thursday = new DateTime(2021, 4, 8);
            DateTime expected = new DateTime(2021, 4, 7);
            Assert.AreEqual(expected, thursday.LastBusinessDay());
        }

        [TestMethod]
        public void Test_LastBusinessDay_Friday_NoHoliday()
        {
            DateTime friday = new DateTime(2021, 4, 9);
            DateTime expected = new DateTime(2021, 4, 8);
            Assert.AreEqual(expected, friday.LastBusinessDay());
        }

        [TestMethod]
        public void Test_LastBusinessDay_Saturday_NoHoliday()
        {
            DateTime saturday = new DateTime(2021, 4, 10);
            DateTime expected = new DateTime(2021, 4, 9);
            Assert.AreEqual(expected, saturday.LastBusinessDay());
        }

        [TestMethod]
        public void Test_LastBusinessDay_Sunday_NoHoliday()
        {
            DateTime sunday = new DateTime(2021, 4, 11);
            DateTime expected = new DateTime(2021, 4, 9);
            Assert.AreEqual(expected, sunday.LastBusinessDay());
        }

        [TestMethod]
        public void Test_LastBusinessDay_Monday_IsHoliday()
        {
            DateTime monday = new DateTime(2021, 4, 5);
            DateTime holiday = new DateTime(2021, 4, 2);
            DateTime expected = new DateTime(2021, 4, 1);
            Assert.AreEqual(expected, monday.LastBusinessDay(holidays: holiday));
        }

        [TestMethod]
        public void Test_LastBusinessDay_Tuesday_IsHoliday()
        {
            DateTime tuesday = new DateTime(2021, 4, 6);
            DateTime holiday = new DateTime(2021, 4, 5);
            DateTime expected = new DateTime(2021, 4, 2);
            Assert.AreEqual(expected, tuesday.LastBusinessDay(holidays: holiday));
        }

        [TestMethod]
        public void Test_LastBusinessDay_Wednesday_IsHoliday()
        {
            DateTime wednesday = new DateTime(2021, 4, 7);
            DateTime holiday = new DateTime(2021, 4, 6);
            DateTime expected = new DateTime(2021, 4, 5);
            Assert.AreEqual(expected, wednesday.LastBusinessDay(holidays: holiday));
        }

        [TestMethod]
        public void Test_LastBusinessDay_Thursday_IsHoliday()
        {
            DateTime thursday = new DateTime(2021, 4, 8);
            DateTime holiday = new DateTime(2021, 4, 7);
            DateTime expected = new DateTime(2021, 4, 6);
            Assert.AreEqual(expected, thursday.LastBusinessDay(holidays: holiday));
        }

        [TestMethod]
        public void Test_LastBusinessDay_Friday_IsHoliday()
        {
            DateTime friday = new DateTime(2021, 4, 9);
            DateTime holiday = new DateTime(2021, 4, 8);
            DateTime expected = new DateTime(2021, 4, 7);
            Assert.AreEqual(expected, friday.LastBusinessDay(holidays: holiday));
        }

        [TestMethod]
        public void Test_LastBusinessDay_Saturday_IsHoliday()
        {
            DateTime saturday = new DateTime(2021, 4, 10);
            DateTime holiday = new DateTime(2021, 4, 9);
            DateTime expected = new DateTime(2021, 4, 8);
            Assert.AreEqual(expected, saturday.LastBusinessDay(holidays: holiday));
        }

        [TestMethod]
        public void Test_LastBusinessDay_Sunday_IsHoliday()
        {
            DateTime sunday = new DateTime(2021, 4, 11);
            DateTime holiday = new DateTime(2021, 4, 9);
            DateTime expected = new DateTime(2021, 4, 8);
            Assert.AreEqual(expected, sunday.LastBusinessDay(holidays: holiday));
        }

        [TestMethod]
        public void Test_LastBusinessDayOrDate_Monday_NoHoliday()
        {
            DateTime monday = new DateTime(2021, 4, 5);
            DateTime expected = new DateTime(2021, 4, 5);
            Assert.AreEqual(expected, monday.LastBusinessDayOrDate());
        }

        [TestMethod]
        public void Test_LastBusinessDayOrDate_Tuesday_NoHoliday()
        {
            DateTime tuesday = new DateTime(2021, 4, 6);
            DateTime expected = new DateTime(2021, 4, 6);
            Assert.AreEqual(expected, tuesday.LastBusinessDayOrDate());
        }

        [TestMethod]
        public void Test_LastBusinessDayOrDate_Wednesday_NoHoliday()
        {
            DateTime wednesday = new DateTime(2021, 4, 7);
            DateTime expected = new DateTime(2021, 4, 7);
            Assert.AreEqual(expected, wednesday.LastBusinessDayOrDate());
        }

        [TestMethod]
        public void Test_LastBusinessDayOrDate_Thursday_NoHoliday()
        {
            DateTime thursday = new DateTime(2021, 4, 8);
            DateTime expected = new DateTime(2021, 4, 8);
            Assert.AreEqual(expected, thursday.LastBusinessDayOrDate());
        }

        [TestMethod]
        public void Test_LastBusinessDayOrDate_Friday_NoHoliday()
        {
            DateTime friday = new DateTime(2021, 4, 9);
            DateTime expected = new DateTime(2021, 4, 9);
            Assert.AreEqual(expected, friday.LastBusinessDayOrDate());
        }

        [TestMethod]
        public void Test_LastBusinessDayOrDate_Saturday_NoHoliday()
        {
            DateTime saturday = new DateTime(2021, 4, 10);
            DateTime expected = new DateTime(2021, 4, 9);
            Assert.AreEqual(expected, saturday.LastBusinessDayOrDate());
        }

        [TestMethod]
        public void Test_LastBusinessDayOrDate_Sunday_NoHoliday()
        {
            DateTime sunday = new DateTime(2021, 4, 11);
            DateTime expected = new DateTime(2021, 4, 9);
            Assert.AreEqual(expected, sunday.LastBusinessDayOrDate());
        }

        [TestMethod]
        public void Test_LastBusinessDayOrDate_Monday_IsHoliday()
        {
            DateTime monday = new DateTime(2021, 4, 5);
            DateTime holiday = new DateTime(2021, 4, 5);
            DateTime expected = new DateTime(2021, 4, 2);
            Assert.AreEqual(expected, monday.LastBusinessDayOrDate(holidays: holiday));
        }

        [TestMethod]
        public void Test_LastBusinessDayOrDate_Tuesday_IsHoliday()
        {
            DateTime tuesday = new DateTime(2021, 4, 6);
            DateTime holiday = new DateTime(2021, 4, 6);
            DateTime expected = new DateTime(2021, 4, 5);
            Assert.AreEqual(expected, tuesday.LastBusinessDayOrDate(holidays: holiday));
        }

        [TestMethod]
        public void Test_LastBusinessDayOrDate_Wednesday_IsHoliday()
        {
            DateTime wednesday = new DateTime(2021, 4, 7);
            DateTime holiday = new DateTime(2021, 4, 7);
            DateTime expected = new DateTime(2021, 4, 6);
            Assert.AreEqual(expected, wednesday.LastBusinessDayOrDate(holidays: holiday));
        }

        [TestMethod]
        public void Test_LastBusinessDayOrDate_Thursday_IsHoliday()
        {
            DateTime thursday = new DateTime(2021, 4, 8);
            DateTime holiday = new DateTime(2021, 4, 8);
            DateTime expected = new DateTime(2021, 4, 7);
            Assert.AreEqual(expected, thursday.LastBusinessDayOrDate(holidays: holiday));
        }

        [TestMethod]
        public void Test_LastBusinessDayOrDate_Friday_IsHoliday()
        {
            DateTime friday = new DateTime(2021, 4, 9);
            DateTime holiday = new DateTime(2021, 4, 9);
            DateTime expected = new DateTime(2021, 4, 8);
            Assert.AreEqual(expected, friday.LastBusinessDayOrDate(holidays: holiday));
        }

        [TestMethod]
        public void Test_LastBusinessDayOrDate_Saturday_IsHoliday()
        {
            DateTime saturday = new DateTime(2021, 4, 10);
            DateTime holiday = new DateTime(2021, 4, 9);
            DateTime expected = new DateTime(2021, 4, 8);
            Assert.AreEqual(expected, saturday.LastBusinessDayOrDate(holidays: holiday));
        }

        [TestMethod]
        public void Test_LastBusinessDayOrDate_Sunday_IsHoliday()
        {
            DateTime sunday = new DateTime(2021, 4, 11);
            DateTime holiday = new DateTime(2021, 4, 9);
            DateTime expected = new DateTime(2021, 4, 8);
            Assert.AreEqual(expected, sunday.LastBusinessDayOrDate(holidays: holiday));
        }

        [TestMethod]
        public void Test_StartOfMonthBusinessDay_WeekdayStart_NoHoliday()
        {
            DateTime midMonth = new DateTime(2022, 3, 2);
            DateTime expected = new DateTime(2022, 3, 1);
            Assert.AreEqual(expected, midMonth.StartOfMonthBusinessDay());
        }

        [TestMethod]
        public void Test_StartOfMonthBusinessDay_WeekendStart_NoHoliday()
        {
            DateTime midMonth = new DateTime(2022, 1, 12);
            DateTime expected = new DateTime(2022, 1, 3);
            Assert.AreEqual(expected, midMonth.StartOfMonthBusinessDay());
        }

        [TestMethod]
        public void Test_StartOfMonthBusinessDay_WeekdayStart_IsHoliday()
        {
            DateTime midMonth = new DateTime(2022, 3, 2);
            DateTime holiday = new DateTime(2022, 3, 1);
            DateTime expected = new DateTime(2022, 3, 2);
            Assert.AreEqual(expected, midMonth.StartOfMonthBusinessDay(holiday));
        }

        [TestMethod]
        public void Test_StartOfMonthBusinessDay_WeekendStart_IsHoliday()
        {
            DateTime midMonth = new DateTime(2022, 1, 12);
            DateTime holiday = new DateTime(2022, 1, 3);
            DateTime expected = new DateTime(2022, 1, 4);
            Assert.AreEqual(expected, midMonth.StartOfMonthBusinessDay(holiday));
        }

        [TestMethod]
        public void Test_EndOfMonthBusinessDay_WeekdayEnd_NoHoliday()
        {
            DateTime midMonth = new DateTime(2022, 3, 2);
            DateTime expected = new DateTime(2022, 3, 31);
            Assert.AreEqual(expected, midMonth.EndOfMonthBusinessDay());
        }

        [TestMethod]
        public void Test_EndOfMonthBusinessDay_WeekendEnd_NoHoliday()
        {
            DateTime midMonth = new DateTime(2022, 4, 12);
            DateTime expected = new DateTime(2022, 4, 29);
            Assert.AreEqual(expected, midMonth.EndOfMonthBusinessDay());
        }

        [TestMethod]
        public void Test_EndOfMonthBusinessDay_WeekdayEnd_IsHoliday()
        {
            DateTime midMonth = new DateTime(2022, 3, 2);
            DateTime holiday = new DateTime(2022, 3, 31);
            DateTime expected = new DateTime(2022, 3, 30);
            Assert.AreEqual(expected, midMonth.EndOfMonthBusinessDay(holiday));
        }

        [TestMethod]
        public void Test_EndOfMonthBusinessDay_WeekendEnd_IsHoliday()
        {
            DateTime midMonth = new DateTime(2022, 4, 12);
            DateTime holiday = new DateTime(2022, 4, 29);
            DateTime expected = new DateTime(2022, 4, 28);
            Assert.AreEqual(expected, midMonth.EndOfMonthBusinessDay(holiday));
        }

        [TestMethod]
        public void Test_StartOfYearBusinessDay_WeekdayStart_NoHoliday()
        {
            DateTime date = new DateTime(2021, 1, 6);
            DateTime expected = new DateTime(2021, 1, 1);
            Assert.AreEqual(expected, date.StartOfYearBusinessDay());
        }

        [TestMethod]
        public void Test_StartOfYearBusinessDay_WeekendStart_NoHoliday()
        {
            DateTime date = new DateTime(2022, 1, 12);
            DateTime expected = new DateTime(2022, 1, 3);
            Assert.AreEqual(expected, date.StartOfYearBusinessDay());
        }

        [TestMethod]
        public void Test_StartOfYearBusinessDay_WeekdayStart_IsHoliday()
        {
            DateTime date = new DateTime(2021, 3, 2);
            DateTime holiday = new DateTime(2021, 1, 1);
            DateTime expected = new DateTime(2021, 1, 4);
            Assert.AreEqual(expected, date.StartOfYearBusinessDay(holiday));
        }

        [TestMethod]
        public void Test_StartOfYearBusinessDay_WeekendStart_IsHoliday()
        {
            DateTime date = new DateTime(2022, 1, 12);
            DateTime holiday = new DateTime(2022, 1, 3);
            DateTime expected = new DateTime(2022, 1, 4);
            Assert.AreEqual(expected, date.StartOfYearBusinessDay(holiday));
        }

        [TestMethod]
        public void Test_EndOfYearBusinessDay_WeekdayEnd_NoHoliday()
        {
            DateTime date = new DateTime(2021, 3, 2);
            DateTime expected = new DateTime(2021, 12, 31);
            Assert.AreEqual(expected, date.EndOfYearBusinessDay());
        }

        [TestMethod]
        public void Test_EndOfYearBusinessDay_WeekendEnd_NoHoliday()
        {
            DateTime date = new DateTime(2022, 4, 12);
            DateTime expected = new DateTime(2022, 12, 30);
            Assert.AreEqual(expected, date.EndOfYearBusinessDay());
        }

        [TestMethod]
        public void Test_EndOfYearBusinessDay_WeekdayEnd_IsHoliday()
        {
            DateTime date = new DateTime(2021, 3, 2);
            DateTime holiday = new DateTime(2021, 12, 31);
            DateTime expected = new DateTime(2021, 12, 30);
            Assert.AreEqual(expected, date.EndOfYearBusinessDay(holiday));
        }

        [TestMethod]
        public void Test_EndOfYearBusinessDay_WeekendEnd_IsHoliday()
        {
            DateTime date = new DateTime(2022, 4, 12);
            DateTime holiday = new DateTime(2022, 12, 30);
            DateTime expected = new DateTime(2022, 12, 29);
            Assert.AreEqual(expected, date.EndOfYearBusinessDay(holiday));
        }

        [TestMethod]
        public void Test_BusinessDaysUntil_CountsEndDayButNotStartDay_NoHoliday()
        {
            DateTime first = new DateTime(2022, 12, 22); // Thursday
            DateTime last = new DateTime(2022, 12, 23); // Friday
            int daysUntil = first.BusinessDaysUntil(last);
            Assert.AreEqual(1, daysUntil);
        }

        [TestMethod]
        public void Test_BusinessDaysUntil_OverWeekend_NoHoliday()
        {
            DateTime first = new DateTime(2022, 12, 22); // Thursday
            DateTime last = new DateTime(2022, 12, 29); // Friday
            int daysUntil = first.BusinessDaysUntil(last);
            Assert.AreEqual(5, daysUntil);
        }

        [TestMethod]
        public void Test_BusinessDaysUntil_SameDayReturnsZero_NoHoliday()
        {
            DateTime date = new DateTime(2022, 12, 22); // Thursday
            int daysUntil = date.BusinessDaysUntil(date);
            Assert.AreEqual(0, daysUntil);
        }

        [TestMethod]
        public void Test_BusinessDaysUntil_FirstDayAfterLastDayThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => DateTime.MaxValue.BusinessDaysUntil(DateTime.MinValue));
        }

        [TestMethod]
        public void Test_BusinessDaysUntil_HolidayOnEndDateReturnsZero_Holiday()
        {
            DateTime first = new DateTime(2022, 12, 22); // Thursday
            DateTime last = new DateTime(2022, 12, 23); // Friday
            int daysUntil = first.BusinessDaysUntil(last, bankHolidays: new DateTime[] { last });
            Assert.AreEqual(0, daysUntil);
        }
    }
}
