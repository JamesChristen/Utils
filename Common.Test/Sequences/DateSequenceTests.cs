using Common.Sequences;

namespace Common.Test.Sequences
{
    [TestClass]
    public class DateSequenceTests
    {
        private readonly DateTime _monday = new DateTime(2021, 5, 3);
        private readonly DateTime _tuesday = new DateTime(2021, 5, 4);

        [TestMethod]
        public void Test_OptionsNotNull()
        {
            Assert.IsNotNull(new DateSequence<int>().Options);
            Assert.IsNotNull(new DateSequence<int>(items: null).Options);
            Assert.IsNotNull(new DateSequence<int>(new SequenceOptions()).Options);
            Assert.IsNotNull(new DateSequence<int>(options: null).Options);
            Assert.IsNotNull(new DateSequence<int>(options: null, items: null).Options);
        }

        [TestMethod]
        public void Test_EmptySeq_HasNoKeys()
        {
            IDateSequence<int> seq = new DateSequence<int>();
            Assert.AreEqual(0, seq.Count());
            Assert.AreEqual(0, seq.Keys.Count());
            Assert.AreEqual(0, seq.Values.Count());
            Assert.AreEqual(null, seq.FirstKey);
            Assert.AreEqual(null, seq.LastKey);
        }

        [TestMethod]
        public void Test_EmptySeq_ReturnsDefaults()
        {
            IDateSequence<int> seq = new DateSequence<int>();
            Assert.AreEqual(-1, seq.GetValueOrDefault(DateTime.Today, -1));
            Assert.AreEqual(-1, seq.FirstValueOrDefault(-1));
            Assert.AreEqual(-1, seq.LastValueOrDefault(-1));
        }

        [TestMethod]
        public void Test_EmptySeq_IndexRetrieval_MissingDataThrowException_ThrowsException()
        {
            IDateSequence<int> seq = new DateSequence<int>();
            seq.Options.MissingData = MissingDataBehaviour.ThrowException;
            Assert.ThrowsException<KeyNotFoundException>(() => seq[DateTime.Today]);
        }

        [TestMethod]
        public void Test_EmptySeq_IndexRetrieval_MissingDataDefaultValue_ReturnsDefaultValue()
        {
            IDateSequence<int> seq = new DateSequence<int>();
            seq.Options.MissingData = MissingDataBehaviour.DefaultValue;
            Assert.AreEqual(default, seq[DateTime.Today]);
        }

        [TestMethod]
        public void Test_EmptySeq_IndexRetrieval_MissingDataBackToLast_ThrowsException()
        {
            IDateSequence<int> seq = new DateSequence<int>();
            seq.Options.MissingData = MissingDataBehaviour.BackToLast;
            Assert.ThrowsException<KeyNotFoundException>(() => seq[DateTime.Today]);
        }

        [TestMethod]
        public void Test_EmptySeq_IndexRetrieval_MissingDataForwardToNext_ThrowsException()
        {
            IDateSequence<int> seq = new DateSequence<int>();
            seq.Options.MissingData = MissingDataBehaviour.ForwardToNext;
            Assert.ThrowsException<KeyNotFoundException>(() => seq[DateTime.Today]);
        }

        [TestMethod]
        public void Test_EmptySeq_PrintsEmpty()
        {
            IDateSequence<int> seq = new DateSequence<int>();
            string output = string.Empty;
            seq.Print(str => output = str);
            Assert.AreEqual(string.Empty, output);
        }

        [TestMethod]
        public void Test_Add_AddToEmpty()
        {
            IDateSequence<int> seq = new DateSequence<int>();
            seq.Add(_monday, 1);

            Assert.AreEqual(1, seq.Count());
            Assert.AreEqual(1, seq[_monday]);
        }

        [TestMethod]
        public void Test_Add_AddDuplicateThrowsException()
        {
            IDateSequence<int> seq = new DateSequence<int>();
            seq.Add(_monday, 1);

            Assert.AreEqual(1, seq.Count());
            Assert.AreEqual(1, seq[_monday]);
            Assert.ThrowsException<ArgumentException>(() => seq.Add(_monday, 2));
            Assert.AreEqual(1, seq.Count());
            Assert.AreEqual(1, seq[_monday]);
        }

        [TestMethod]
        public void Test_AddOrUpdate_AddToEmpty()
        {
            IDateSequence<int> seq = new DateSequence<int>();
            seq.AddOrUpdate(_monday, 1);

            Assert.AreEqual(1, seq.Count());
            Assert.AreEqual(1, seq[_monday]);
        }

        [TestMethod]
        public void Test_AddOrUpdate_AddDuplicateUpdatesValue()
        {
            IDateSequence<int> seq = new DateSequence<int>();
            seq.Add(_monday, 1);

            Assert.AreEqual(1, seq.Count());
            Assert.AreEqual(1, seq[_monday]);

            seq.AddOrUpdate(_monday, 2);
            Assert.AreEqual(1, seq.Count());
            Assert.AreEqual(2, seq[_monday]);
        }

        [TestMethod]
        public void Test_AddRange_AddToEmpty()
        {
            DateTime date = new DateTime(2021, 5, 3); // Monday

            IDateSequence<int> seq = new DateSequence<int>();
            seq.AddRange(new Dictionary<DateTime, int>
            {
                { date, 1 },
                { date.AddDays(1), 2 }
            });

            Assert.AreEqual(2, seq.Count());
            Assert.AreEqual(1, seq[date]);
            Assert.AreEqual(2, seq[date.AddDays(1)]);
        }

        [TestMethod]
        public void Test_AddRange_AddDuplicateThrowsException()
        {
            IDateSequence<int> seq = new DateSequence<int>();
            seq.Add(_monday, 1);

            Assert.ThrowsException<ArgumentException>(() =>
            {
                seq.AddRange(new Dictionary<DateTime, int>
                {
                    { _monday, 1 }
                });
            });
        }

        [TestMethod]
        public void Test_AddOrUpdateRange_AddToEmpty()
        {
            IDateSequence<int> seq = new DateSequence<int>();
            seq.AddOrUpdateRange(new Dictionary<DateTime, int>
            {
                { _monday, 1 },
                { _monday.AddDays(1), 2 }
            });

            Assert.AreEqual(2, seq.Count());
            Assert.AreEqual(1, seq[_monday]);
            Assert.AreEqual(2, seq[_monday.AddDays(1)]);
        }

        [TestMethod]
        public void Test_AddOrUpdateRange_AddDuplicateUpdatesRecord()
        {
            DateTime date = new DateTime(2021, 5, 3); // Monday

            IDateSequence<int> seq = new DateSequence<int>();
            seq.Add(date, 1);

            Assert.AreEqual(1, seq.Count());
            Assert.AreEqual(1, seq[date]);

            seq.AddOrUpdateRange(new Dictionary<DateTime, int>
            {
                { date, 2 },
                { date.AddDays(1), 3 }
            });

            Assert.AreEqual(2, seq.Count());
            Assert.AreEqual(2, seq[date]);
            Assert.AreEqual(3, seq[date.AddDays(1)]);
        }

        [TestMethod]
        public void Test_FillMissingDates_Daily_EmptySeq()
        {
            SequenceOptions options = new SequenceOptions
            {
                AutoFillDates = true,
                IncludeWeekends = false,
                Frequency = SequenceFrequency.Daily
            };

            IDateSequence<int> seq = new DateSequence<int>(options);
            seq.FillMissingDates();

            Assert.AreEqual(0, seq.Count());
        }

        [TestMethod]
        public void Test_FillMissingDates_Daily_SingleItem()
        {
            SequenceOptions options = new SequenceOptions
            {
                AutoFillDates = true,
                IncludeWeekends = false,
                Frequency = SequenceFrequency.Daily
            };

            IDateSequence<int> seq = new DateSequence<int>(options);
            seq.Add(_monday, 1);
            seq.FillMissingDates();

            Assert.AreEqual(1, seq.Count());
        }

        [TestMethod]
        public void Test_FillMissingDates_Daily_Week()
        {
            SequenceOptions options = new SequenceOptions
            {
                AutoFillDates = true,
                IncludeWeekends = false,
                Frequency = SequenceFrequency.Daily
            };

            IDateSequence<int> seq = new DateSequence<int>(options)
            {
                { new DateTime(2021, 4, 5), 0 }, // Monday
                { new DateTime(2021, 4, 9), 0 }, // Friday
            };

            seq.FillMissingDates();
            Assert.AreEqual(5, seq.Count());

            IEnumerable<DateTime> expected = seq.FirstKey.Value.RangeTo(seq.LastKey.Value, includeWeekends: options.IncludeWeekends);
            Assert.IsTrue(Common.Test.Utils.AreEquivalent(expected, seq.Keys));
        }

        [TestMethod]
        public void Test_FillMissingDates_Daily_WeekendNotIncluded()
        {
            SequenceOptions options = new SequenceOptions
            {
                AutoFillDates = true,
                IncludeWeekends = false,
                Frequency = SequenceFrequency.Daily
            };

            IDateSequence<int> seq = new DateSequence<int>(options, new Dictionary<DateTime, int>
            {
                { new DateTime(2021, 4, 5), 0 }, // Monday
                { new DateTime(2021, 4, 12), 0 }, // Monday
            });

            seq.FillMissingDates();
            Assert.AreEqual(6, seq.Count());

            IEnumerable<DateTime> expected = seq.FirstKey.Value.RangeTo(seq.LastKey.Value, includeWeekends: options.IncludeWeekends);
            Assert.IsTrue(Common.Test.Utils.AreEquivalent(expected, seq.Keys));
        }

        [TestMethod]
        public void Test_FillMissingDates_Daily_WeekendIncluded()
        {
            SequenceOptions options = new SequenceOptions
            {
                AutoFillDates = true,
                IncludeWeekends = true,
                Frequency = SequenceFrequency.Daily
            };

            IDateSequence<int> seq = new DateSequence<int>(options, new Dictionary<DateTime, int>
            {
                { new DateTime(2021, 4, 5), 0 }, // Monday
                { new DateTime(2021, 4, 12), 0 }, // Monday
            });

            seq.FillMissingDates();
            Assert.AreEqual(8, seq.Count());

            IEnumerable<DateTime> expected = seq.FirstKey.Value.RangeTo(seq.LastKey.Value, includeWeekends: options.IncludeWeekends);
            Assert.IsTrue(Common.Test.Utils.AreEquivalent(expected, seq.Keys));
        }

        [TestMethod]
        public void Test_FillMissingDates_Daily_KeepTimestamp()
        {
            SequenceOptions options = new SequenceOptions
            {
                AutoFillDates = true,
                IncludeWeekends = false,
                Frequency = SequenceFrequency.Daily
            };

            IDateSequence<int> seq = new DateSequence<int>(options, new Dictionary<DateTime, int>
            {
                { new DateTime(2021, 4, 5, 12, 0, 0), 0 }, // Monday
                { new DateTime(2021, 4, 9, 12, 0, 0), 0 }, // Friday
            });

            seq.FillMissingDates();
            Assert.AreEqual(5, seq.Count());

            IEnumerable<DateTime> expected = seq.FirstKey.Value.RangeTo(seq.LastKey.Value, includeWeekends: options.IncludeWeekends);
            Assert.IsTrue(Common.Test.Utils.AreEquivalent(expected, seq.Keys));
        }

        [TestMethod]
        public void Test_SetStart_Empty()
        {
            IDateSequence<int> seq = new DateSequence<int>();
            seq.SetStart(_monday);

            Assert.AreEqual(1, seq.Count());
            Assert.AreEqual(default, seq[_monday]);
        }

        [TestMethod]
        public void Test_SetStart_MatchLow()
        {
            IDateSequence<int> seq = new DateSequence<int>()
            {
                { _monday, 1 }
            };
            seq.SetStart(_monday);

            Assert.AreEqual(1, seq.Count());
            Assert.AreEqual(1, seq[_monday]);
        }

        [TestMethod]
        public void Test_SetStart_NewLow()
        {
            IDateSequence<int> seq = new DateSequence<int>();
            seq.Add(_tuesday, 1);
            seq.SetStart(_monday);

            Assert.AreEqual(2, seq.Count());
            Assert.AreEqual(default, seq[_monday]);
            Assert.AreEqual(1, seq[_tuesday]);
        }

        [TestMethod]
        public void Test_SetStart_AboveLow()
        {
            IDateSequence<int> seq = new DateSequence<int>();
            seq.Add(_monday, 1);
            seq.SetStart(_tuesday);

            Assert.AreEqual(1, seq.Count());
            Assert.IsFalse(seq.ContainsKey(_monday));
            Assert.IsTrue(seq.ContainsKey(_tuesday));
            Assert.AreEqual(default, seq[_tuesday]);
        }

        [TestMethod]
        public void Test_SetStart_MiddleOfSet()
        {
            Dictionary<DateTime, int> items = new Dictionary<DateTime, int>
            {
                { new DateTime(2021, 4, 5), 1 },
                { new DateTime(2021, 4, 6), 2 },
                { new DateTime(2021, 4, 7), 3 },
                { new DateTime(2021, 4, 8), 4 },
                { new DateTime(2021, 4, 9), 5 },
            };
            IDateSequence<int> seq = new DateSequence<int>(items);
            seq.SetStart(new DateTime(2021, 4, 7));

            Assert.AreEqual(3, seq.Count());
            Assert.IsFalse(seq.ContainsKey(new DateTime(2021, 4, 5)));
            Assert.IsFalse(seq.ContainsKey(new DateTime(2021, 4, 6)));
            Assert.IsTrue(seq.ContainsKey(new DateTime(2021, 4, 7)));
            Assert.IsTrue(seq.ContainsKey(new DateTime(2021, 4, 8)));
            Assert.IsTrue(seq.ContainsKey(new DateTime(2021, 4, 9)));
            Assert.AreEqual(3, seq.Count());
            Assert.AreEqual(3, seq[new DateTime(2021, 4, 7)]);
        }

        [TestMethod]
        public void Test_SetEnd_Empty()
        {
            IDateSequence<int> seq = new DateSequence<int>();
            seq.SetEnd(_monday);

            Assert.AreEqual(1, seq.Count());
            Assert.AreEqual(default, seq[_monday]);
        }

        [TestMethod]
        public void Test_SetEnd_MatchHigh()
        {
            IDateSequence<int> seq = new DateSequence<int>()
            {
                { _monday, 1 }
            };
            seq.SetEnd(_monday);

            Assert.AreEqual(1, seq.Count());
            Assert.AreEqual(1, seq[_monday]);
        }

        [TestMethod]
        public void Test_SetEnd_NewHigh()
        {
            IDateSequence<int> seq = new DateSequence<int>();
            seq.Add(_monday, 1);
            seq.SetEnd(_tuesday);

            Assert.AreEqual(2, seq.Count());
            Assert.AreEqual(default, seq[_tuesday]);
            Assert.AreEqual(1, seq[_monday]);
        }

        [TestMethod]
        public void Test_SetEnd_BelowHigh()
        {
            IDateSequence<int> seq = new DateSequence<int>();
            seq.Add(_tuesday, 1);
            seq.SetEnd(_monday);

            Assert.AreEqual(1, seq.Count());
            Assert.IsFalse(seq.ContainsKey(_tuesday));
            Assert.IsTrue(seq.ContainsKey(_monday));
            Assert.AreEqual(default, seq[_monday]);
        }

        [TestMethod]
        public void Test_SetEnd_MiddleOfSet()
        {
            Dictionary<DateTime, int> items = new Dictionary<DateTime, int>
            {
                { new DateTime(2021, 4, 5), 1 },
                { new DateTime(2021, 4, 6), 2 },
                { new DateTime(2021, 4, 7), 3 },
                { new DateTime(2021, 4, 8), 4 },
                { new DateTime(2021, 4, 9), 5 },
            };
            IDateSequence<int> seq = new DateSequence<int>(items);
            seq.SetEnd(new DateTime(2021, 4, 7));

            Assert.AreEqual(3, seq.Count());
            Assert.IsTrue(seq.ContainsKey(new DateTime(2021, 4, 5)));
            Assert.IsTrue(seq.ContainsKey(new DateTime(2021, 4, 6)));
            Assert.IsTrue(seq.ContainsKey(new DateTime(2021, 4, 7)));
            Assert.IsFalse(seq.ContainsKey(new DateTime(2021, 4, 8)));
            Assert.IsFalse(seq.ContainsKey(new DateTime(2021, 4, 9)));
            Assert.AreEqual(3, seq.Count());
            Assert.AreEqual(3, seq[new DateTime(2021, 4, 7)]);
        }

        [TestMethod]
        public void Test_AutoFill_Add_Above()
        {
            SequenceOptions options = new SequenceOptions
            {
                AutoFillDates = true,
                IncludeWeekends = false,
                Frequency = SequenceFrequency.Daily
            };
            IDateSequence<int> seq = new DateSequence<int>(options);
            seq.Add(new DateTime(2021, 4, 5), 1); // Monday
            seq.Add(new DateTime(2021, 4, 9), 2); // Friday

            Assert.AreEqual(5, seq.Count());
            Assert.AreEqual(default, seq[new DateTime(2021, 4, 6)]);
            Assert.AreEqual(default, seq[new DateTime(2021, 4, 7)]);
            Assert.AreEqual(default, seq[new DateTime(2021, 4, 8)]);
        }

        [TestMethod]
        public void Test_AutoFill_Add_Below()
        {
            SequenceOptions options = new SequenceOptions
            {
                AutoFillDates = true,
                IncludeWeekends = false,
                Frequency = SequenceFrequency.Daily
            };
            IDateSequence<int> seq = new DateSequence<int>(options);
            seq.Add(new DateTime(2021, 4, 9), 1); // Friday
            seq.Add(new DateTime(2021, 4, 5), 2); // Monday

            Assert.AreEqual(5, seq.Count());
            Assert.AreEqual(default, seq[new DateTime(2021, 4, 6)]);
            Assert.AreEqual(default, seq[new DateTime(2021, 4, 7)]);
            Assert.AreEqual(default, seq[new DateTime(2021, 4, 8)]);
        }

        [TestMethod]
        public void Test_AutoFill_AddRange_Above()
        {
            SequenceOptions options = new SequenceOptions
            {
                AutoFillDates = true,
                IncludeWeekends = false,
                Frequency = SequenceFrequency.Daily
            };
            IDateSequence<int> seq = new DateSequence<int>(options);
            seq.Add(new DateTime(2021, 4, 5), 1); // Monday
            seq.AddRange(new Dictionary<DateTime, int>
            {
                { new DateTime(2021, 4, 9), 2 }, // Friday
            });

            Assert.AreEqual(5, seq.Count());
            Assert.AreEqual(default, seq[new DateTime(2021, 4, 6)]);
            Assert.AreEqual(default, seq[new DateTime(2021, 4, 7)]);
            Assert.AreEqual(default, seq[new DateTime(2021, 4, 8)]);
        }

        [TestMethod]
        public void Test_AutoFill_AddRange_Below()
        {
            SequenceOptions options = new SequenceOptions
            {
                AutoFillDates = true,
                IncludeWeekends = false,
                Frequency = SequenceFrequency.Daily
            };
            IDateSequence<int> seq = new DateSequence<int>(options);
            seq.Add(new DateTime(2021, 4, 9), 1); // Friday
            seq.AddRange(new Dictionary<DateTime, int>
            {
                { new DateTime(2021, 4, 5), 2 }, // Monday
            });

            Assert.AreEqual(5, seq.Count());
            Assert.AreEqual(default, seq[new DateTime(2021, 4, 6)]);
            Assert.AreEqual(default, seq[new DateTime(2021, 4, 7)]);
            Assert.AreEqual(default, seq[new DateTime(2021, 4, 8)]);
        }

        [TestMethod]
        public void Test_Transform_Empty()
        {
            IDateSequence<int> seq = new DateSequence<int>();
            IDateSequence<bool> transformed = seq.Transform(x => x % 2 == 0);

            Assert.AreEqual(0, transformed.Count());
        }

        [TestMethod]
        public void Test_Transform_Single()
        {
            IDateSequence<int> seq = new DateSequence<int>();
            seq.Add(_monday, 1);

            IDateSequence<bool> transformed = seq.Transform(x => x % 2 == 0);

            Assert.AreEqual(1, transformed.Count());
            Assert.AreEqual(false, transformed[_monday]);
        }

        [TestMethod]
        public void Test_Transform_MissingRecords()
        {
            IDateSequence<int> seq = new DateSequence<int>();
            seq.Add(new DateTime(2021, 4, 5), 1);
            seq.Add(new DateTime(2021, 4, 9), 5);
            seq.FillMissingDates();

            IDateSequence<bool> transformed = seq.Transform(x => x % 2 == 0);

            Assert.AreEqual(5, transformed.Count());
            Assert.AreEqual(false, transformed[new DateTime(2021, 4, 5)]);
        }

        [TestMethod]
        public void Test_Transform_ErrorsThrowAggregateException()
        {
            IDateSequence<Foo> seq = new DateSequence<Foo>();
            seq.Add(new DateTime(2021, 4, 5), null);

            Assert.ThrowsException<AggregateException>(() => seq.Transform(x => x.INT % 2 == 0));
        }

        private class Foo
        {
            public int INT { get; set; }
        }

        [TestMethod]
        public void Test_Chaining()
        {
            SequenceOptions options = new SequenceOptions
            {
                AutoFillDates = false,
                IncludeWeekends = false,
                Frequency = SequenceFrequency.Daily
            };
            IDateSequence<int> seq = new DateSequence<int>(options);
            IDateSequence<int> seq2 = seq.SetStart(new DateTime(2021, 4, 5)).SetEnd(new DateTime(2021, 4, 9)).FillMissingDates();
            Assert.AreSame(seq, seq2);

            IEnumerable<DateTime> dates = new DateTime(2021, 4, 5).RangeTo(new DateTime(2021, 4, 9));
            Assert.IsTrue(Common.Test.Utils.AreEquivalent(dates, seq.Keys));

            Assert.IsTrue(seq.Values.All(x => x == default));
        }

        [TestMethod]
        public void Test_Join()
        {
            IDateSequence<int> seq1 = DateSequence<int>.Empty;
            IDateSequence<int> seq2 = DateSequence<int>.Empty;
            IDateSequence<int> expected = DateSequence<int>.Empty;
            DateTime date = new DateTime(2022, 6, 27); // Monday
            for (int i = 0; i < 5; i++)
            {
                seq1.Add(date.AddDays(i), i);
                seq2.Add(date.AddDays(i), i);
                expected.Add(date.AddDays(i), i * i);
            }
            JoinOptions<int, int, int> joinOptions = new JoinOptions<int, int, int>(MissingDataBehaviour.DefaultValue, (x, y) => x * y);

            IDateSequence<int> result = seq1.Join(seq2, joinOptions);
            Assert.IsTrue(result.SequenceEqual(expected));
        }

        [TestMethod]
        public void Test_Join_NullJoinOptionsThrowsArgumentNullException()
        {
            IDateSequence<int> seq = DateSequence<int>.Empty;
            Assert.ThrowsException<ArgumentNullException>(() => seq.Join(DateSequence<int>.Empty, (JoinOptions<int, int, int>)null));
        }

        [TestMethod]
        public void Test_Join_NullSequenceThrowsArgumentNullException()
        {
            IDateSequence<int> seq = DateSequence<int>.Empty;
            JoinOptions<int, int, int> joinOptions = new JoinOptions<int, int, int>(MissingDataBehaviour.DefaultValue, (x, y) => x + y);
            Assert.ThrowsException<ArgumentNullException>(() => seq.Join(null, joinOptions));
        }

        [TestMethod]
        public void Test_Join_MissingValues()
        {
            IDateSequence<int> seq1 = DateSequence<int>.Empty;
            IDateSequence<int> seq2 = DateSequence<int>.Empty;
            IDateSequence<int> expected = DateSequence<int>.Empty;
            DateTime date = new DateTime(2022, 6, 27); // Monday
            for (int i = 0; i < 5; i++)
            {
                seq1.Add(date.AddDays(i), i);
                expected.Add(date.AddDays(i), 0);
            }
            JoinOptions<int, int, int> joinOptions = new JoinOptions<int, int, int>(MissingDataBehaviour.DefaultValue, (x, y) => x * y, defaultValue: 0);

            IDateSequence<int> result = seq1.Join(seq2, joinOptions);
            Assert.IsTrue(result.SequenceEqual(expected));
        }

        [TestMethod]
        public void Test_ShiftDates_TimeSpan()
        {
            IDateSequence<int> seq = new DateSequence<int>(
                new Dictionary<DateTime, int>()
                {
                    { new DateTime(2022, 8, 22), 1 },
                    { new DateTime(2022, 8, 23), 2 },
                    { new DateTime(2022, 8, 24), 3 },
                    { new DateTime(2022, 8, 25), 4 },
                    { new DateTime(2022, 8, 26), 5 }
                });

            IDateSequence<int> shifted = seq.ShiftDates(TimeSpan.FromHours(1));
            IDateSequence<int> expected = new DateSequence<int>(
                new Dictionary<DateTime, int>()
                {
                    { new DateTime(2022, 8, 22, 1, 0, 0), 1 },
                    { new DateTime(2022, 8, 23, 1, 0, 0), 2 },
                    { new DateTime(2022, 8, 24, 1, 0, 0), 3 },
                    { new DateTime(2022, 8, 25, 1, 0, 0), 4 },
                    { new DateTime(2022, 8, 26, 1, 0, 0), 5 }
                });

            Assert.IsTrue(shifted.SequenceEqual(expected));
        }
    }
}
