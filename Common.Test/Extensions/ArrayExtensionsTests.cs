using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Common.Test.Extensions
{
    [TestClass]
    public class ArrayExtensionsTests
    {
        private class Test
        {
            public int ASD { get; }
            public Test(int asd) { ASD = asd; }
            public override string ToString() => $"{ASD}";
        }

        [TestMethod]
        public void Test_Flatten()
        {
            Test[,] arr = new Test[,]
            {
                {  new Test(1), new Test(2), new Test(3) },
                {  new Test(4), new Test(5), new Test(6) }
            };

            Test[] flat = arr.Flatten();
            Assert.AreEqual(6, flat.Length);
            Assert.AreEqual(1, flat[0].ASD);
            Assert.AreEqual(2, flat[1].ASD);
            Assert.AreEqual(3, flat[2].ASD);
            Assert.AreEqual(4, flat[3].ASD);
            Assert.AreEqual(5, flat[4].ASD);
            Assert.AreEqual(6, flat[5].ASD);
        }

        [TestMethod]
        public void Test_Flatten_NullArrayThrowsArgumentNullException()
        {
            Test[,] arr = null;
            Assert.ThrowsException<ArgumentNullException>(() => arr.Flatten());
        }

        [TestMethod]
        public void Test_Flatten_OneDimensional()
        {
            Test[,] arr = new Test[,]
            {
                {  new Test(1), new Test(2), new Test(3) }
            };

            Test[] flat = arr.Flatten();
            Assert.AreEqual(3, flat.Length);
            Assert.AreEqual(1, flat[0].ASD);
            Assert.AreEqual(2, flat[1].ASD);
            Assert.AreEqual(3, flat[2].ASD);
        }

        [TestMethod]
        public void Test_Flatten_Empty()
        {
            Test[,] arr = new Test[0,0];

            Test[] flat = arr.Flatten();
            Assert.AreEqual(0, flat.Length);
        }

        [TestMethod]
        public void Test_GetColumn()
        {
            Test[,] arr = new Test[,]
            {
                {  new Test(1), new Test(2), new Test(3) },
                {  new Test(4), new Test(5), new Test(6) }
            };

            Test[] col = arr.GetColumn(0);
            Assert.AreEqual(2, col.Length);
            Assert.AreEqual(1, col[0].ASD);
            Assert.AreEqual(4, col[1].ASD);
        }

        [TestMethod]
        public void Test_GetColumn_NullArrayThrowsArgumentNullException()
        {
            Test[,] arr = null;
            Assert.ThrowsException<ArgumentNullException>(() => arr.GetColumn(0));
        }

        [TestMethod]
        public void Test_GetColumn_ColumnNumberOutOfRangeThrowsIndexOutOfRangeException()
        {
            Test[,] arr = new Test[,]
            {
                {  new Test(1), new Test(2), new Test(3) },
                {  new Test(4), new Test(5), new Test(6) }
            };

            Assert.ThrowsException<IndexOutOfRangeException>(() => arr.GetColumn(999));
        }

        [TestMethod]
        public void Test_GetRow()
        {
            Test[,] arr = new Test[,]
            {
                {  new Test(1), new Test(2), new Test(3) },
                {  new Test(4), new Test(5), new Test(6) }
            };

            Test[] row = arr.GetRow(0);
            Assert.AreEqual(3, row.Length);
            Assert.AreEqual(1, row[0].ASD);
            Assert.AreEqual(2, row[1].ASD);
            Assert.AreEqual(3, row[2].ASD);
        }

        [TestMethod]
        public void Test_GetRow_NullArrayThrowsArgumentNullException()
        {
            Test[,] arr = null;
            Assert.ThrowsException<ArgumentNullException>(() => arr.GetRow(0));
        }

        [TestMethod]
        public void Test_GetRow_RowNumberOutOfRangeThrowsIndexOutOfRangeException()
        {
            Test[,] arr = new Test[,]
            {
                {  new Test(1), new Test(2), new Test(3) },
                {  new Test(4), new Test(5), new Test(6) }
            };

            Assert.ThrowsException<IndexOutOfRangeException>(() => arr.GetRow(999));
        }
    }
}
