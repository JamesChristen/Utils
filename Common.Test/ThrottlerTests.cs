using Common.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Test
{
    [TestClass]
    public class ThrottlerTests
    {
        [TestMethod]
        public void Test_Validation_NegativeMaxThreadsThrowsArgumentException()
        {
            Assert.That.ThrowsValidationExceptionAsync(() => Throttler.RunInParallel(new string[0], x => Task.Run(() => x), -1), "maxThreads");
        }

        [TestMethod]
        public void Test_Validation_ZeroMaxThreadsThrowsArgumentException()
        {
            Assert.That.ThrowsValidationExceptionAsync(() => Throttler.RunInParallel(new string[0], x => Task.Run(() => x), 0), "maxThreads");
        }

        [TestMethod]
        public void Test_Validation_NullItemsThrowsArgumentException()
        {
            Assert.That.ThrowsValidationExceptionAsync(() => Throttler.RunInParallel((string[])null, x => Task.Run(() => x), 0), "items");
        }

        [TestMethod]
        public void Test_Validation_NullFuncThrowsArgumentException()
        {
            Assert.That.ThrowsValidationExceptionAsync(() => Throttler.RunInParallel(new string[0], (Func<string, Task<string>>)null, 0), "func");
        }

        [TestMethod]
        public void Test_Validation_NullConverterThrowsArgumentException()
        {
            Assert.That.ThrowsValidationExceptionAsync(() => Throttler.RunInParallel(new string[0], x => Task.Run(() => x), (Func<string, string, string>)null, 0), "converter");
        }

        [TestMethod]
        public async Task Test_RunInParallel()
        {
            string[] items = new string[] { "QWE", "ASD", "ZXC" };
            static Task<string> func(string x) => Task.FromResult(x);
            static string converter(string x, string y) => $"{x} => {y}";

            List<string> result = await Throttler.RunInParallel(items, func, converter);
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            Assert.IsTrue(new string[] { "ASD => ASD", "QWE => QWE", "ZXC => ZXC" }.SequenceEqual(result.OrderBy(x => x)));
        }

        [TestMethod]
        public async Task Test_RunInParallel_MoreItemsThanThreadsThrottlesTasks()
        {
            string[] items = new string[] { "QWE", "ASD", "ZXC" };
            static Task<string> func(string x) => Task.Run(() => { Thread.Sleep(100); return x; });
            static string converter(string x, string y) => $"{x} => {y}";

            Stopwatch sw = Stopwatch.StartNew();
            List<string> result = await Throttler.RunInParallel(items, func, converter, maxThreads: 1);
            sw.Stop();

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            Assert.IsTrue(new string[] { "ASD => ASD", "QWE => QWE", "ZXC => ZXC" }.SequenceEqual(result.OrderBy(x => x)));
            Assert.IsTrue(sw.ElapsedMilliseconds > 200);
        }

        [TestMethod]
        public async Task Test_RunInParallel_RequeuesCorrectly()
        {
            string[] items = new string[] { "QWE", "ASD", "ZXC", "QWE", "ASD", "ZXC", "QWE", "ASD", "ZXC", "QWE", "ASD", "ZXC" };
            static Task<string> func(string x) => Task.Run(() => { Thread.Sleep(100); return x; });
            static string converter(string x, string y) => $"{x} => {y}";

            Stopwatch sw = Stopwatch.StartNew();
            List<string> result = await Throttler.RunInParallel(items, func, converter);
            sw.Stop();

            Assert.IsNotNull(result);
            Assert.AreEqual(items.Length, result.Count);
            Assert.IsTrue(sw.ElapsedMilliseconds < 100 * items.Length);
        }

        [TestMethod]
        public void Test_RunInParallel_ExceptionInFuncThrowsAggregateException()
        {
            string[] items = new string[] { "QWE", "ASD", "ZXC" };
            static Task<string> func(string x) => Task.Run(() =>
            {
                return (string)null ?? throw new Exception("TEST");
            });
            static string converter(string x, string y) => x + y;

            Assert.ThrowsExceptionAsync<AggregateException>(async () => await Throttler.RunInParallel(items, func, converter, maxThreads: 1));
        }

        [TestMethod]
        public void Test_RunInParallel_ExceptionInConverterThrowsAggregateException()
        {
            string[] items = new string[] { "QWE", "ASD", "ZXC" };
            static Task<string> func(string x) => Task.Run(() => x);
            static string converter(string x, string y) => throw new NotImplementedException();

            Assert.ThrowsExceptionAsync<AggregateException>(async () => await Throttler.RunInParallel(items, func, converter, maxThreads: 1));
        }
    }
}
