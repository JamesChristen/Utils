using Common.Sequences;

namespace Common.Test.Sequences
{
    [TestClass]
    public class JoinOptionsTests
    {
        [TestMethod]
        public void Test_JoinOptions_NullJoinBehaviourThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new JoinOptions<int, int, int>(MissingDataBehaviour.DefaultValue, null));
        }

        [TestMethod]
        public void Test_AllMissingDataBehavioursHaveImplementations()
        {
            IEnumerable<MissingDataBehaviour> mdb = EnumHelpers.All<MissingDataBehaviour>();
            foreach (MissingDataBehaviour m in mdb)
            {
                JoinOptions<int, int, int> joinOptions = new JoinOptions<int, int, int>(m, (x, y) => 0);
                try
                {
                    joinOptions.GetValue(DateTime.MaxValue, DateSequence<int>.Empty, DateSequence<int>.Empty);
                }
                catch (NotImplementedException)
                {
                    Assert.Fail($"{m.ToLongString()} has not implementation");
                }
                catch
                {
                }
            }
        }

        [TestMethod]
        public void Test_GetValue_BothContainDate()
        {
            JoinOptions<int, int, int> joinOptions = new JoinOptions<int, int, int>(MissingDataBehaviour.DefaultValue, (x, y) => x + y, 0);
            
            DateTime date = new DateTime(2022, 6, 27); // Monday
            IDateSequence<int> seq1 = DateSequence<int>.Empty;
            seq1.Add(date, 1);
            IDateSequence<int> seq2 = DateSequence<int>.Empty;
            seq2.Add(date, 2);

            int result = joinOptions.GetValue(date, seq1, seq2);
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void Test_GetValue_DefaultValue()
        {
            JoinOptions<int, int, int> joinOptions = new JoinOptions<int, int, int>(MissingDataBehaviour.DefaultValue, (x, y) => x + y, 0);

            DateTime date = new DateTime(2022, 6, 27); // Monday
            IDateSequence<int> seq1 = DateSequence<int>.Empty;
            seq1.Add(date, 1);
            IDateSequence<int> seq2 = DateSequence<int>.Empty;

            int result = joinOptions.GetValue(date, seq1, seq2);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Test_GetValue_ThrowException()
        {
            JoinOptions<int, int, int> joinOptions = new JoinOptions<int, int, int>(MissingDataBehaviour.ThrowException, (x, y) => x + y, 0);

            DateTime date = new DateTime(2022, 6, 27); // Monday
            IDateSequence<int> seq1 = DateSequence<int>.Empty;
            seq1.Add(date, 1);
            IDateSequence<int> seq2 = DateSequence<int>.Empty;

            Assert.ThrowsException<KeyNotFoundException>(() => joinOptions.GetValue(date, seq1, seq2));
        }

        [TestMethod]
        public void Test_GetValue_BackToLast_WithValue()
        {
            JoinOptions<int, int, int> joinOptions = new JoinOptions<int, int, int>(MissingDataBehaviour.BackToLast, (x, y) => x + y, 0);

            DateTime date = new DateTime(2022, 6, 27); // Monday
            IDateSequence<int> seq1 = DateSequence<int>.Empty;
            seq1.Add(date, 1);
            IDateSequence<int> seq2 = DateSequence<int>.Empty;
            seq2.Add(date, 2);

            int result = joinOptions.GetValue(date.AddDays(1), seq1, seq2);
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void Test_GetValue_BackToLast_WithDefaultValue()
        {
            int defaultValue = 10;
            JoinOptions<int, int, int> joinOptions = new JoinOptions<int, int, int>(MissingDataBehaviour.BackToLast, (x, y) => x + y, defaultValue);

            DateTime date = new DateTime(2022, 6, 27); // Monday
            IDateSequence<int> seq1 = DateSequence<int>.Empty;
            IDateSequence<int> seq2 = DateSequence<int>.Empty;

            int result = joinOptions.GetValue(date, seq1, seq2);
            Assert.AreEqual(defaultValue, result);
        }

        [TestMethod]
        public void Test_GetValue_ForwardToNext_WithValue()
        {
            JoinOptions<int, int, int> joinOptions = new JoinOptions<int, int, int>(MissingDataBehaviour.ForwardToNext, (x, y) => x + y, 0);

            DateTime date = new DateTime(2022, 6, 27); // Monday
            IDateSequence<int> seq1 = DateSequence<int>.Empty;
            seq1.Add(date, 1);
            IDateSequence<int> seq2 = DateSequence<int>.Empty;
            seq2.Add(date, 2);

            int result = joinOptions.GetValue(date.AddDays(-1), seq1, seq2);
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void Test_GetValue_ForwardToNext_WithDefaultValue()
        {
            int defaultValue = 10;
            JoinOptions<int, int, int> joinOptions = new JoinOptions<int, int, int>(MissingDataBehaviour.ForwardToNext, (x, y) => x + y, defaultValue);

            DateTime date = new DateTime(2022, 6, 27); // Monday
            IDateSequence<int> seq1 = DateSequence<int>.Empty;
            IDateSequence<int> seq2 = DateSequence<int>.Empty;

            int result = joinOptions.GetValue(date, seq1, seq2);
            Assert.AreEqual(defaultValue, result);
        }
    }
}
