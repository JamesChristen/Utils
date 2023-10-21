using System;

namespace Common.Test.Sequences
{
    internal class HistoryItem
    {
        public DateTime KnownFrom { get; set; }

        public DateTime? KnownTo { get; set; }
        public int Value { get; set; }
        public DateTime RefDate { get; set; }
    }
}
