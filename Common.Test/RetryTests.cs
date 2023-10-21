using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace Common.Test
{
    [TestClass]
    public class RetryTests
    {
        [TestMethod]
        public void Test_NTimes_Func_NegativeNTimesThrowsValidationException()
        {
            Assert.That.ThrowsValidationException(() => Retry.NTimes(-1, () => true), "nTimes");
        }

        [TestMethod]
        public void Test_NTimes_Func_ZeroNTimesThrowsValidationException()
        {
            Assert.That.ThrowsValidationException(() => Retry.NTimes(0, () => true), "nTimes");
        }

        [TestMethod]
        public void Test_NTimes_Func_NullOperationThrowsValidationException()
        {
            Assert.That.ThrowsValidationException(() => Retry.NTimes(1, (Func<bool>)null), "operation");
        }

        [TestMethod]
        public void Test_NTimes_Func_OperationThrowingUnexpectedExceptionExitsEarly()
        {
            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                Retry.NTimes<bool, ArgumentException>(10, () => throw new InvalidOperationException(), TimeSpan.FromMinutes(1));
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
                sw.Stop();
                Assert.IsTrue(sw.ElapsedMilliseconds < TimeSpan.FromMinutes(10).TotalMilliseconds);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void Test_NTimes_Func_FailingNTimesThrowsMostRecentException()
        {
            int i = 0;
            try
            {
                Retry.NTimes<bool, ArgumentException>(10, () => throw new ArgumentException((++i).ToString()));
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.Message.Contains(i.ToString()));
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void Test_NTimes_Func_ReturnsValue()
        {
            bool result = Retry.NTimes<bool, ArgumentException>(3, () => true);
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Test_NTimes_Action_NegativeNTimesThrowsValidationException()
        {
            Assert.That.ThrowsValidationException(() => Retry.NTimes(-1, () => { }), "nTimes");
        }

        [TestMethod]
        public void Test_NTimes_Action_ZeroNTimesThrowsValidationException()
        {
            Assert.That.ThrowsValidationException(() => Retry.NTimes(0, () => { }), "nTimes");
        }

        [TestMethod]
        public void Test_NTimes_Action_NullOperationThrowsValidationException()
        {
            Assert.That.ThrowsValidationException(() => Retry.NTimes(1, null), "operation");
        }

        [TestMethod]
        public void Test_NTimes_Actoion_OperationThrowingUnexpectedExceptionExitsEarly()
        {
            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                Retry.NTimes<ArgumentException>(10, (Action)(() => { throw new InvalidOperationException(); }), TimeSpan.FromMinutes(1));
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
                sw.Stop();
                Assert.IsTrue(sw.ElapsedMilliseconds < TimeSpan.FromMinutes(10).TotalMilliseconds);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void Test_NTimes_Action_FailingNTimesThrowsMostRecentException()
        {
            int i = 0;
            try
            {
                Retry.NTimes<ArgumentException>(10, (Action)(() => throw new ArgumentException((++i).ToString())));
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.Message.Contains(i.ToString()));
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void Test_NTimes_Action_Returns()
        {
            int i = 0;
            Retry.NTimes<ArgumentException>(3, () => { i = 1; });
            Assert.AreEqual(1, i);
        }
    }
}
