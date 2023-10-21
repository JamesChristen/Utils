using Common.Sequences;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Test.Sequences
{
    [TestClass]
    public class HistorySequenceTests
    {
        [TestMethod]
        public void Test_Constructor()
        {
            List<HistoryItem> items = new List<HistoryItem>
            {
                new HistoryItem { RefDate = new DateTime(2021, 4, 5), KnownFrom = new DateTime(2021, 4, 5), KnownTo = new DateTime(2021, 4, 7), Value = 1 },
                new HistoryItem { RefDate = new DateTime(2021, 4, 5), KnownFrom = new DateTime(2021, 4, 7), KnownTo = null, Value = 2 },
                new HistoryItem { RefDate = new DateTime(2021, 4, 6), KnownFrom = new DateTime(2021, 4, 6), KnownTo = new DateTime(2021, 4, 7), Value = 3 },
                new HistoryItem { RefDate = new DateTime(2021, 4, 6), KnownFrom = new DateTime(2021, 4, 7), KnownTo = null, Value = 4 },
                new HistoryItem { RefDate = new DateTime(2021, 4, 7), KnownFrom = new DateTime(2021, 4, 7), KnownTo = null, Value = 5 }
            };

            Dictionary<DateTime, Dictionary<DateTime, HistoryItem>> data =
                items.GroupBy(x => x.RefDate)
                     .ToDictionary(x => x.Key, x => x.ToDictionary(y => y.KnownFrom, y => y));

            IHistorySequence<HistoryItem> seq = new HistorySequence<HistoryItem>(data);

            Assert.AreEqual(3, seq.Values.Count());

            Assert.IsTrue(seq.ContainsKey(new DateTime(2021, 4, 5)));
            Assert.AreEqual(2, seq[new DateTime(2021, 4, 5)].Latest.Value);

            Assert.IsTrue(seq.ContainsKey(new DateTime(2021, 4, 6)));
            Assert.AreEqual(4, seq[new DateTime(2021, 4, 6)].Latest.Value);

            Assert.IsTrue(seq.ContainsKey(new DateTime(2021, 4, 7)));
            Assert.AreEqual(5, seq[new DateTime(2021, 4, 7)].Latest.Value);
        }

        [TestMethod]
        public void Test_Constructor_NullItemsThrowsException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new HistorySequence<HistoryItem>(new SequenceOptions(), (Dictionary<DateTime, History<HistoryItem>>)null));
        }

        [TestMethod]
        public void Test_GetSequenceKnownOnDate_AfterSeq()
        {
            List<HistoryItem> items = new List<HistoryItem>
            {
                new HistoryItem { RefDate = new DateTime(2021, 4, 5), KnownFrom = new DateTime(2021, 4, 5), KnownTo = new DateTime(2021, 4, 7), Value = 1 },
                new HistoryItem { RefDate = new DateTime(2021, 4, 5), KnownFrom = new DateTime(2021, 4, 7), KnownTo = null, Value = 2 },
                new HistoryItem { RefDate = new DateTime(2021, 4, 6), KnownFrom = new DateTime(2021, 4, 6), KnownTo = new DateTime(2021, 4, 7), Value = 3 },
                new HistoryItem { RefDate = new DateTime(2021, 4, 6), KnownFrom = new DateTime(2021, 4, 7), KnownTo = null, Value = 4 },
                new HistoryItem { RefDate = new DateTime(2021, 4, 7), KnownFrom = new DateTime(2021, 4, 7), KnownTo = null, Value = 5 }
            };

            Dictionary<DateTime, Dictionary<DateTime, HistoryItem>> data =
                items.GroupBy(x => x.RefDate)
                     .ToDictionary(x => x.Key, x => x.ToDictionary(y => y.KnownFrom, y => y));

            IHistorySequence<HistoryItem> historySeq = new HistorySequence<HistoryItem>(data);
            IDateSequence<HistoryItem> seq = historySeq.GetSequenceKnownOnDate(new DateTime(2021, 4, 9));

            Assert.AreEqual(3, seq.Values.Count());

            Assert.IsTrue(seq.ContainsKey(new DateTime(2021, 4, 5)));
            Assert.AreEqual(2, seq[new DateTime(2021, 4, 5)].Value);

            Assert.IsTrue(seq.ContainsKey(new DateTime(2021, 4, 6)));
            Assert.AreEqual(4, seq[new DateTime(2021, 4, 6)].Value);

            Assert.IsTrue(seq.ContainsKey(new DateTime(2021, 4, 7)));
            Assert.AreEqual(5, seq[new DateTime(2021, 4, 7)].Value);
        }

        [TestMethod]
        public void Test_GetSequenceKnownOnDate_BeforeSeq()
        {
            List<HistoryItem> items = new List<HistoryItem>
            {
                new HistoryItem { RefDate = new DateTime(2021, 4, 5), KnownFrom = new DateTime(2021, 4, 5), KnownTo = new DateTime(2021, 4, 7), Value = 1 },
                new HistoryItem { RefDate = new DateTime(2021, 4, 5), KnownFrom = new DateTime(2021, 4, 7), KnownTo = null, Value = 2 },
                new HistoryItem { RefDate = new DateTime(2021, 4, 6), KnownFrom = new DateTime(2021, 4, 6), KnownTo = new DateTime(2021, 4, 7), Value = 3 },
                new HistoryItem { RefDate = new DateTime(2021, 4, 6), KnownFrom = new DateTime(2021, 4, 7), KnownTo = null, Value = 4 },
                new HistoryItem { RefDate = new DateTime(2021, 4, 7), KnownFrom = new DateTime(2021, 4, 7), KnownTo = null, Value = 5 }
            };

            Dictionary<DateTime, Dictionary<DateTime, HistoryItem>> data =
                items.GroupBy(x => x.RefDate)
                     .ToDictionary(x => x.Key, x => x.ToDictionary(y => y.KnownFrom, y => y));

            IHistorySequence<HistoryItem> historySeq = new HistorySequence<HistoryItem>(data);
            IDateSequence<HistoryItem> seq = historySeq.GetSequenceKnownOnDate(new DateTime(2021, 4, 1));

            Assert.AreEqual(0, seq.Values.Count());
        }

        [TestMethod]
        public void Test_GetSequenceKnownOnDate_MiddleSeq()
        {
            List<HistoryItem> items = new List<HistoryItem>
            {
                new HistoryItem { RefDate = new DateTime(2021, 4, 5), KnownFrom = new DateTime(2021, 4, 5), KnownTo = new DateTime(2021, 4, 7), Value = 1 },
                new HistoryItem { RefDate = new DateTime(2021, 4, 5), KnownFrom = new DateTime(2021, 4, 7), KnownTo = null, Value = 2 },
                new HistoryItem { RefDate = new DateTime(2021, 4, 6), KnownFrom = new DateTime(2021, 4, 6), KnownTo = new DateTime(2021, 4, 7), Value = 3 },
                new HistoryItem { RefDate = new DateTime(2021, 4, 6), KnownFrom = new DateTime(2021, 4, 7), KnownTo = null, Value = 4 },
                new HistoryItem { RefDate = new DateTime(2021, 4, 7), KnownFrom = new DateTime(2021, 4, 7), KnownTo = null, Value = 5 }
            };

            Dictionary<DateTime, Dictionary<DateTime, HistoryItem>> data =
                items.GroupBy(x => x.RefDate)
                     .ToDictionary(x => x.Key, x => x.ToDictionary(y => y.KnownFrom, y => y));

            IHistorySequence<HistoryItem> historySeq = new HistorySequence<HistoryItem>(data);
            IDateSequence<HistoryItem> seq = historySeq.GetSequenceKnownOnDate(new DateTime(2021, 4, 6));

            Assert.AreEqual(2, seq.Values.Count());

            Assert.IsTrue(seq.ContainsKey(new DateTime(2021, 4, 5)));
            Assert.AreEqual(1, seq[new DateTime(2021, 4, 5)].Value);

            Assert.IsTrue(seq.ContainsKey(new DateTime(2021, 4, 6)));
            Assert.AreEqual(3, seq[new DateTime(2021, 4, 6)].Value);

        }

        [TestMethod]
        public void Test_GetSequenceKnownOnDate_LongSeq()
        {
            int count = 10_000;
            DateTime start = new DateTime(1900, 1, 1);
            List<HistoryItem> items =
                Enumerable.Range(0, count)
                          .Select(x => new HistoryItem { RefDate = start.AddDays(x), KnownFrom = start.AddDays(x), Value = x })
                          .ToList();

            Dictionary<DateTime, Dictionary<DateTime, HistoryItem>> data =
                items.GroupBy(x => x.RefDate)
                     .ToDictionary(x => x.Key, x => x.ToDictionary(y => y.KnownFrom, y => y));

            SequenceOptions options = new SequenceOptions { IncludeWeekends = true };
            IHistorySequence<HistoryItem> historySeq = new HistorySequence<HistoryItem>(options, data);
            IDateSequence<HistoryItem> seq = historySeq.GetSequenceKnownOnDate(new DateTime(2021, 4, 6));

            Assert.AreEqual(count, seq.Values.Count());
        }
    }
}
