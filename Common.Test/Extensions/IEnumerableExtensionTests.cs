using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Test.Extensions
{
    [TestClass]
    public class IEnumerableExtensionTests
    {
        #region AsSingleEnumerable

        [TestMethod]
        public void Test_AsSingleEnumerable_NotNullSourceReturnsIEnumerable()
        {
            IEnumerable<int> e = 1.AsSingleEnumerable();
            Assert.AreEqual(1, e.Count());
            Assert.AreEqual(1, e.First());
        }

        private class TestClass { }

        [TestMethod]
        public void Test_AsSingleEnumerable_NullSourceReturnsEmptyIEnumerable()
        {
            IEnumerable<TestClass> e = ((TestClass)null).AsSingleEnumerable();
            Assert.AreEqual(0, e.Count());
        }

        #endregion

        #region ChunkEvenly

        [TestMethod]
        public void Test_ChunkEvenly_SingleChunk()
        {
            IEnumerable<int> ints = Enumerable.Range(0, 10);
            IEnumerable<IEnumerable<int>> chunked = ints.ChunkEvenly(100);

            IEnumerable<IEnumerable<int>> expected = new List<IEnumerable<int>> { Enumerable.Range(0, 10) };

            Assert.IsTrue(Utils.AreEquivalent(expected, chunked));
        }

        [TestMethod]
        public void Test_ChunkEvenly_InvalidChunkSizeThrowsException()
        {
            IEnumerable<int> ints = Enumerable.Range(0, 10);

            Assert.ThrowsException<ArgumentException>(() => ints.ChunkEvenly(0).ToList());
            Assert.ThrowsException<ArgumentException>(() => ints.ChunkEvenly(-1).ToList());
        }

        [TestMethod]
        public void Test_ChunkEvenly_ChunkSizeOne()
        {
            IEnumerable<int> ints = Enumerable.Range(0, 10);
            IEnumerable<IEnumerable<int>> chunked = ints.ChunkEvenly(1);

            IEnumerable<IEnumerable<int>> expected = Enumerable.Range(0, 10).Select(x => x.AsSingleEnumerable());

            Assert.IsTrue(Utils.AreEquivalent(expected, chunked));
        }

        [TestMethod]
        public void Test_ChunkEvenly_ChunkSizeExact()
        {
            IEnumerable<int> ints = Enumerable.Range(0, 10);
            IEnumerable<IEnumerable<int>> chunked = ints.ChunkEvenly(10);

            IEnumerable<IEnumerable<int>> expected = new List<IEnumerable<int>> { Enumerable.Range(0, 10) };

            Assert.IsTrue(Utils.AreEquivalent(expected, chunked));
        }

        [TestMethod]
        public void Test_ChunkEvenly_ManyChunks_Even()
        {
            IEnumerable<int> ints = Enumerable.Range(0, 10);
            IEnumerable<IEnumerable<int>> chunked = ints.ChunkEvenly(2);

            IEnumerable<IEnumerable<int>> expected = Enumerable.Range(0, 5).Select(x => new List<int> { x * 2, x * 2 + 1 });

            Assert.IsTrue(Utils.AreEquivalent(expected, chunked));
        }

        [TestMethod]
        public void Test_ChunkEvenly_ManyChunks_Uneven()
        {
            IEnumerable<int> ints = Enumerable.Range(0, 10);
            IEnumerable<IEnumerable<int>> chunked = ints.ChunkEvenly(4).ToList();

            IEnumerable<IEnumerable<int>> expected = new List<IEnumerable<int>>
            {
                new List<int> { 0, 1, 2, 3 },
                new List<int> { 4, 5, 6 },
                new List<int> { 7, 8, 9 }
            };

            Assert.IsTrue(Utils.AreEquivalent(expected, chunked));
        }

        [TestMethod]
        public void Test_ChunkEvenly_ManyChunks_UnevenDifferentGroupSizes()
        {
            IEnumerable<int> ints2 = Enumerable.Range(0, 9);
            IEnumerable<IEnumerable<int>> chunked2 = ints2.ChunkEvenly(4).ToList();

            IEnumerable<IEnumerable<int>> expected2 = new List<IEnumerable<int>>
            {
                new List<int> { 0, 1, 2 },
                new List<int> { 3, 4, 5 },
                new List<int> { 6, 7, 8 }
            };

            Assert.IsTrue(Utils.AreEquivalent(expected2, chunked2));
        }

        [TestMethod]
        public void Test_ChunkEvenly_NullSourceThrowsArgumentNullException()
        {
            IEnumerable<int> ints = null;
            Assert.ThrowsException<ArgumentNullException>(() => ints.Chunk(10).ToList());
        }

        #endregion

        #region DistinctBy

        [TestMethod]
        public void Test_DistinctBy_FiltersDownToDistinct()
        {
            IEnumerable<int> arr = new int[] { 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 1, 2 };
            IEnumerable<int> dis = arr.DistinctBy(x => x);
            int[] expected = new int[] { 1, 2, 3, 4, 5, 6 };
            Assert.IsTrue(expected.SequenceEqual(dis));
        }

        [TestMethod]
        public void Test_DistinctBy_NullSourceThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => ((int[])null).DistinctBy(x => x).ToList());
        }

        [TestMethod]
        public void Test_DistinctBy_EmptyIEnumerableReturnsEmpty()
        {
            Assert.AreEqual(0, Array.Empty<int>().DistinctBy(x => x).Count());
        }

        #endregion

        #region Flatten

        public class FlattenTest
        {
            public int ID { get; set; }
            public List<FlattenTest> Children { get; set; }

            public FlattenTest(int id, params FlattenTest[] items)
            {
                ID = id;
                Children = items.ToList();
            }
        }

        [TestMethod]
        public void Test_Flatten_SourceFlattened()
        {
            List<FlattenTest> source = new List<FlattenTest>
            {
                new FlattenTest(1, new FlattenTest(2, new FlattenTest(3)), new FlattenTest(4), new FlattenTest(5)),
                new FlattenTest(6, new FlattenTest(7, new FlattenTest(8, new FlattenTest(9)))),
                new FlattenTest(10)
            };

            IEnumerable<FlattenTest> flattened = source.Flatten(x => x.Children);
            Assert.AreEqual(10, flattened.Count());
            Assert.IsTrue(Enumerable.Range(1, 10).OrderBy(x => x).SequenceEqual(flattened.Select(x => x.ID).OrderBy(x => x)));
        }

        [TestMethod]
        public void Test_Flatten_NullSourceReturnsEmptyCollection()
        {
            IEnumerable<FlattenTest> flattened = ((FlattenTest[])null).Flatten(x => x.Children);
            Assert.AreEqual(0, flattened.Count());
        }

        [TestMethod]
        public void Test_Flatten_EmptySourceReturnsEmptyCollection()
        {
            IEnumerable<FlattenTest> flattened = Array.Empty<FlattenTest>().Flatten(x => x.Children);
            Assert.AreEqual(0, flattened.Count());
        }

        [TestMethod]
        public void Test_Flatten_NullElementSelectorThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Array.Empty<FlattenTest>().Flatten(null));
        }

        #endregion

        #region ForEach

        [TestMethod]
        public void Test_ForEach_LoopsThroughEachItem()
        {
            int count = 0;
            List<int> ints = new List<int>();
            int[] arr = new int[] { 1, 2, 3 };
            arr.ForEach(x =>
            {
                count++;
                ints.Add(x);
            });
            Assert.AreEqual(arr.Length, count);
            Assert.AreEqual(arr.Length, ints.Count);
            Assert.IsTrue(arr.SequenceEqual(ints));
        }

        [TestMethod]
        public void Test_ForEach_NullSourceDoesNotThrowArgumentNullException()
        {
            int count = 0;
            List<int> ints = new List<int>();
            int[] arr = null;

            try
            {
                arr.ForEach(x =>
                {
                    count++;
                    ints.Add(x);
                });
            }
            catch (ArgumentNullException)
            {
                Assert.Fail("Should not throw ArgumentNullException");
            }

            Assert.AreEqual(0, count);
            Assert.AreEqual(0, ints.Count);
        }

        [TestMethod]
        public void Test_ForEach_NullActionThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Array.Empty<int>().ForEach(null));
        }

        #endregion
    }
}
