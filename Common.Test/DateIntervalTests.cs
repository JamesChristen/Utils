using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Common.Test
{
    [TestClass]
    public class DateIntervalTests
    {
        [TestMethod]
        public void Test_Constructor()
        {
            DateInterval interval = new DateInterval(DateTime.MinValue, DateTime.MaxValue);
            Assert.AreEqual(DateTime.MinValue, interval.Start);
            Assert.AreEqual(DateTime.MaxValue, interval.End);
        }

        [TestMethod]
        public void Test_Constructor_StartAfterEndSwapsDates()
        {
            DateInterval interval = new DateInterval(DateTime.MaxValue, DateTime.MinValue);
            Assert.AreEqual(DateTime.MinValue, interval.Start);
            Assert.AreEqual(DateTime.MaxValue, interval.End);
        }

        [TestMethod]
        public void Test_Constructor_SameDates()
        {
            DateInterval interval = new DateInterval(new DateTime(2022, 1, 1), new DateTime(2022, 1, 1));
            Assert.AreEqual(new DateTime(2022, 1, 1), interval.Start);
            Assert.AreEqual(new DateTime(2022, 1, 1), interval.End);
        }

        [TestMethod]
        public void Test_Contains_DateInRangeReturnsTrue()
        {
            DateInterval interval = new DateInterval(DateTime.MinValue, DateTime.MaxValue);
            Assert.IsTrue(interval.Contains(new DateTime(2022, 1, 1)));
        }

        [TestMethod]
        public void Test_Contains_DateOnBoundaryReturnsTrue()
        {
            DateInterval interval = new DateInterval(new DateTime(2022, 1, 1), new DateTime(2023, 1, 1));
            Assert.IsTrue(interval.Contains(new DateTime(2022, 1, 1)));
            Assert.IsTrue(interval.Contains(new DateTime(2023, 1, 1)));
        }

        [TestMethod]
        public void Test_Contains_DateOutOfRangeReturnsFalse()
        {
            DateInterval interval = new DateInterval(new DateTime(2022, 1, 1), new DateTime(2022, 1, 1));
            Assert.IsFalse(interval.Contains(DateTime.MinValue));
        }

        [TestMethod]
        public void Test_Intersects_SubsetReturnsTrue()
        {
            DateInterval interval = new DateInterval(DateTime.MinValue, DateTime.MaxValue);
            DateInterval input = new DateInterval(new DateTime(2022, 1, 1), new DateTime(2022, 1, 1));
            Assert.IsTrue(interval.Intersects(input));
        }

        [TestMethod]
        public void Test_Intersects_SupersetReturnsTrue()
        {
            DateInterval interval = new DateInterval(new DateTime(2022, 1, 1), new DateTime(2022, 1, 1));
            DateInterval input = new DateInterval(DateTime.MinValue, DateTime.MaxValue);
            Assert.IsTrue(interval.Intersects(input));
        }

        [TestMethod]
        public void Test_Intersects_OverlapsStartReturnsTrue()
        {
            DateInterval interval = new DateInterval(new DateTime(2022, 1, 1), DateTime.MaxValue);
            DateInterval input = new DateInterval(DateTime.MinValue, new DateTime(2099, 1, 1));
            Assert.IsTrue(interval.Intersects(input));
        }

        [TestMethod]
        public void Test_Intersects_OverlapsEndReturnsTrue()
        {
            DateInterval interval = new DateInterval(DateTime.MinValue, new DateTime(2022, 1, 1));
            DateInterval input = new DateInterval(new DateTime(2000, 1, 1), DateTime.MaxValue);
            Assert.IsTrue(interval.Intersects(input));
        }

        [TestMethod]
        public void Test_Intersects_IncludeEdgesTrue_EndDateEdgeMatchingReturnsTrue()
        {
            DateInterval interval = new DateInterval(DateTime.MinValue, new DateTime(2022, 1, 1));
            DateInterval input = new DateInterval(new DateTime(2022, 1, 1), DateTime.MaxValue);
            Assert.IsTrue(interval.Intersects(input, includeEdges: true));
        }

        [TestMethod]
        public void Test_Intersects_IncludeEdgesTrue_FromDateEdgeMatchingReturnsTrue()
        {
            DateInterval interval = new DateInterval(new DateTime(2022, 1, 1), DateTime.MaxValue);
            DateInterval input = new DateInterval(DateTime.MinValue, new DateTime(2022, 1, 1));
            Assert.IsTrue(interval.Intersects(input, includeEdges: true));
        }

        [TestMethod]
        public void Test_Intersects_IncludeEdgesFalse_EndDateEdgeMatchingReturnsFalse()
        {
            DateInterval interval = new DateInterval(DateTime.MinValue, new DateTime(2022, 1, 1));
            DateInterval input = new DateInterval(new DateTime(2022, 1, 1), DateTime.MaxValue);
            Assert.IsFalse(interval.Intersects(input, includeEdges: false));
        }

        [TestMethod]
        public void Test_Intersects_IncludeEdgesFalse_FromDateEdgeMatchingReturnsFalse()
        {
            DateInterval interval = new DateInterval(new DateTime(2022, 1, 1), DateTime.MaxValue);
            DateInterval input = new DateInterval(DateTime.MinValue, new DateTime(2022, 1, 1));
            Assert.IsFalse(interval.Intersects(input, includeEdges: false));
        }

        [TestMethod]
        public void Test_Intersects_OutOfRangeReturnsFalse()
        {
            DateInterval interval = new DateInterval(new DateTime(2022, 1, 1), new DateTime(2022, 1, 1));
            Assert.IsFalse(interval.Intersects(new DateInterval(new DateTime(2000, 1, 1), new DateTime(2000, 1, 1))));
            Assert.IsFalse(interval.Intersects(new DateInterval(new DateTime(2099, 1, 1), new DateTime(2099, 1, 1))));
        }
    }
}
