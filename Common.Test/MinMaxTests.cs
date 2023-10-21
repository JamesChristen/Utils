using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Common.Test
{
    [TestClass]
    public class MinMaxTests
    {
        #region DefaultComparer

        [TestMethod]
        public void Test_Max_Default_Decimal_FirstLargerReturnsMax()
        {
            Assert.AreEqual(123M, MinMax.Max(123M, 56M));
        }

        [TestMethod]
        public void Test_Max_Default_Decimal_SameValueReturnsMax()
        {
            Assert.AreEqual(123M, MinMax.Max(123M, 123M));
        }

        [TestMethod]
        public void Test_Max_Default_Decimal_SecondLargerReturnsMax()
        {
            Assert.AreEqual(123M, MinMax.Max(56M, 123M));
        }

        [TestMethod]
        public void Test_Max_Default_Int_FirstLargerReturnsMax()
        {
            Assert.AreEqual(123, MinMax.Max(123, 56));
        }

        [TestMethod]
        public void Test_Max_Default_Int_SameValueReturnsMax()
        {
            Assert.AreEqual(123, MinMax.Max(123, 123));
        }

        [TestMethod]
        public void Test_Max_Default_Int_SecondLargerReturnsMax()
        {
            Assert.AreEqual(123, MinMax.Max(56, 123));
        }

        [TestMethod]
        public void Test_Max_Default_Long_FirstLargerReturnsMax()
        {
            Assert.AreEqual(123L, MinMax.Max(123L, 56L));
        }

        [TestMethod]
        public void Test_Max_Default_Long_SameValueReturnsMax()
        {
            Assert.AreEqual(123L, MinMax.Max(123L, 123L));
        }

        [TestMethod]
        public void Test_Max_Default_Long_SecondLargerReturnsMax()
        {
            Assert.AreEqual(123L, MinMax.Max(56L, 123L));
        }

        [TestMethod]
        public void Test_Min_Default_Decimal_FirstSmallerReturnsMin()
        {
            Assert.AreEqual(56M, MinMax.Min(56M, 123M));
        }

        [TestMethod]
        public void Test_Min_Default_Decimal_SameValueReturnsMin()
        {
            Assert.AreEqual(123M, MinMax.Min(123M, 123M));
        }

        [TestMethod]
        public void Test_Min_Default_Decimal_SecondSmallerReturnsMin()
        {
            Assert.AreEqual(56M, MinMax.Min(123M, 56M));
        }

        [TestMethod]
        public void Test_Min_Default_Int_FirstSmallerReturnsMin()
        {
            Assert.AreEqual(56, MinMax.Min(56, 123));
        }

        [TestMethod]
        public void Test_Min_Default_Int_SameValueReturnsMin()
        {
            Assert.AreEqual(123, MinMax.Min(123, 123));
        }

        [TestMethod]
        public void Test_Min_Default_Int_SecondSmallerReturnsMin()
        {
            Assert.AreEqual(56, MinMax.Min(123, 56));
        }

        [TestMethod]
        public void Test_Min_Default_Long_FirstSmallerReturnsMin()
        {
            Assert.AreEqual(56L, MinMax.Min(56L, 123L));
        }

        [TestMethod]
        public void Test_Min_Default_Long_SameValueReturnsMin()
        {
            Assert.AreEqual(123L, MinMax.Min(123L, 123L));
        }

        [TestMethod]
        public void Test_Min_Default_Long_SecondSmallerReturnsMin()
        {
            Assert.AreEqual(56L, MinMax.Min(123L, 56L));
        }

        #endregion

        #region Custom Comparer

        private class Test
        {
            public int ASD { get; set; }

            public Test(int asd) => ASD = asd;
        }

        private class TestComparer : Comparer<Test>
        {
            public override int Compare([AllowNull] Test x, [AllowNull] Test y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }
                else if (x == null)
                {
                    return 1;
                }
                else if (y == null)
                {
                    return -1;
                }
                else
                {
                    return Comparer<int>.Default.Compare(x.ASD, y.ASD);
                }
            }
        }

        [TestMethod]
        public void Test_TestComparer()
        {
            TestComparer comp = new TestComparer();
            Assert.AreEqual(0, comp.Compare(null, null));
            Assert.AreEqual(1, comp.Compare(null, new Test(0)));
            Assert.AreEqual(-1, comp.Compare(new Test(0), null));
            Assert.AreEqual(0, comp.Compare(new Test(0), new Test(0)));
            Assert.AreEqual(-1, comp.Compare(new Test(0), new Test(1)));
            Assert.AreEqual(1, comp.Compare(new Test(1), new Test(0)));
        }

        [TestMethod]
        public void Test_Max_Custom_FirstLargerReturnsMax()
        {
            Test t1 = new Test(1);
            Test t2 = new Test(0);
            Assert.IsTrue(ReferenceEquals(t1, MinMax.Max(t1, t2, new TestComparer())));
        }

        [TestMethod]
        public void Test_Max_Custom_SameValueReturnsFirst()
        {
            Test t1 = new Test(1);
            Test t2 = new Test(1);
            Assert.IsTrue(ReferenceEquals(t1, MinMax.Max(t1, t2, new TestComparer())));
        }

        [TestMethod]
        public void Test_Max_Custom_SecondLargerReturnsMax()
        {
            Test t1 = new Test(0);
            Test t2 = new Test(1);
            Assert.IsTrue(ReferenceEquals(t2, MinMax.Max(t1, t2, new TestComparer())));
        }

        [TestMethod]
        public void Test_Min_Custom_FirstSmallerReturnsMin()
        {
            Test t1 = new Test(0);
            Test t2 = new Test(1);
            Assert.IsTrue(ReferenceEquals(t1, MinMax.Min(t1, t2, new TestComparer())));
        }

        [TestMethod]
        public void Test_Min_Custom_SameValueReturnsFirst()
        {
            Test t1 = new Test(1);
            Test t2 = new Test(1);
            Assert.IsTrue(ReferenceEquals(t1, MinMax.Min(t1, t2, new TestComparer())));
        }

        [TestMethod]
        public void Test_Min_Custom_SecondSmallerReturnsMin()
        {
            Test t1 = new Test(1);
            Test t2 = new Test(0);
            Assert.IsTrue(ReferenceEquals(t2, MinMax.Min(t1, t2, new TestComparer())));
        }

        #endregion

        #region NullableDateTime Comparer

        [TestMethod]
        public void Test_Max_NullableDateTime_BothNullReturnsNull()
        {
            Assert.AreEqual((DateTime?)null, MinMax.Max((DateTime?)null, (DateTime?)null));
        }

        [TestMethod]
        public void Test_Max_NullableDateTime_FirstNullReturnsSecond()
        {
            Assert.AreEqual(DateTime.MaxValue, MinMax.Max((DateTime?)null, DateTime.MaxValue));
        }

        [TestMethod]
        public void Test_Max_NullableDateTime_SecondNullReturnsFirst()
        {
            Assert.AreEqual(DateTime.MaxValue, MinMax.Max(DateTime.MaxValue, (DateTime?)null));
        }

        [TestMethod]
        public void Test_Max_NullableDateTime_ReturnsMax()
        {
            Assert.AreEqual(DateTime.MaxValue, MinMax.Max(DateTime.MinValue, DateTime.MaxValue));
        }

        [TestMethod]
        public void Test_Min_NullableDateTime_BothNullReturnsNull()
        {
            Assert.AreEqual((DateTime?)null, MinMax.Min((DateTime?)null, (DateTime?)null));
        }

        [TestMethod]
        public void Test_Min_NullableDateTime_FirstNullReturnsSecond()
        {
            Assert.AreEqual(DateTime.MaxValue, MinMax.Min((DateTime?)null, DateTime.MaxValue));
        }

        [TestMethod]
        public void Test_Min_NullableDateTime_SecondNullReturnsFirst()
        {
            Assert.AreEqual(DateTime.MaxValue, MinMax.Min(DateTime.MaxValue, (DateTime?)null));
        }

        [TestMethod]
        public void Test_Min_NullableDateTime_ReturnsMin()
        {
            Assert.AreEqual(DateTime.MinValue, MinMax.Min(DateTime.MinValue, DateTime.MaxValue));
        }

        #endregion
    }
}
