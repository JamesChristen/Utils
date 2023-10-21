using Common.Sequences;
using Common.Test;
using Common.Validation;
using Newtonsoft.Json.Linq;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    public static class AssertExtensions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void SeqAreEqual<T>(this Assert assert, IDateSequence<T> expected, IDateSequence<T> sequence)
        {
            if (expected is null && sequence is null)
            {
                return;
            }

            if (expected is null)
            {
                Assert.IsNull(sequence);
            }

            if (sequence is null)
            {
                Assert.IsNotNull(sequence);
            }

            Assert.AreEqual(expected.Length, sequence.Length);
            KeyValuePair<DateTime, T>[] exp = expected.ToArray();
            KeyValuePair<DateTime, T>[] arr = sequence.ToArray();

            for (int i = 0; i < exp.Length; i++)
            {
                Assert.AreEqual(exp[i].Key, arr[i].Key);
                Assert.AreEqual(exp[i].Value, arr[i].Value);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void ThrowsValidationException(this Assert assert, IValidatable validatable, string propertyName = "")
        {
            try
            {
                validatable.CheckValid();
                Assert.Fail();
            }
            catch (ValidationException ex)
            {
                if (!ex.InnerExceptions.Any(x => x.Message.Contains(propertyName)))
                {
                    Assert.Fail($"{nameof(ValidationException)} thrown, but none with {nameof(propertyName)} {propertyName}");
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void ThrowsValidationException(this Assert assert, Validate validate, string propertyName = "")
        {
            try
            {
                validate.Check();
                Assert.Fail();
            }
            catch (ValidationException ex)
            {
                if (!ex.InnerExceptions.Any(x => x.Message.Contains(propertyName)))
                {
                    Assert.Fail($"{nameof(ValidationException)} thrown, but none with {nameof(propertyName)} {propertyName}");
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void ThrowsValidationException(this Assert assert, Func<object> action, string propertyName = "")
        {
            try
            {
                action();
                Assert.Fail();
            }
            catch (ValidationException ex)
            {
                if (!ex.InnerExceptions.Any(x => x.Message.Contains(propertyName)))
                {
                    Assert.Fail($"{nameof(ValidationException)} thrown, but none with {nameof(propertyName)} {propertyName}");
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void ThrowsValidationException(this Assert assert, Action action, string propertyName = "")
        {
            try
            {
                action();
                Assert.Fail();
            }
            catch (ValidationException ex)
            {
                if (!ex.InnerExceptions.Any(x => x.Message.Contains(propertyName)))
                {
                    Assert.Fail($"{nameof(ValidationException)} thrown, but none with {nameof(propertyName)} {propertyName}");
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static async Task ThrowsExceptionAsyncEnumerable<TEx, TOut>(this Assert assert, Func<IAsyncEnumerable<TOut>> action, string propertyName = "")
            where TEx : Exception
        {
            try
            {
                await foreach (TOut output in action()) { }
                Assert.Fail();
            }
            catch (TEx) { }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void DoesNotThrowValidationException(this Assert assert, IValidatable validatable, string propertyName)
        {
            try
            {
                validatable.CheckValid();
            }
            catch (ValidationException ex)
            {
                if (!string.IsNullOrWhiteSpace(propertyName) && ex.InnerExceptions.Any(x => x.Message.Contains(propertyName)))
                {
                    Assert.Fail($"Validation threw exception with propertyName {propertyName}");
                }
                Assert.Fail($"Test incorrectly configured - validation errors: {string.Join(Environment.NewLine, ex.InnerExceptions.Select(x => x.Message))}");
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void DoesNotThrowValidationException(this Assert assert, Validate validate, string propertyName = "")
        {
            try
            {
                validate.Check();
            }
            catch (ValidationException ex)
            {
                if (!string.IsNullOrWhiteSpace(propertyName) && ex.InnerExceptions.Any(x => x.Message.Contains(propertyName)))
                {
                    Assert.Fail($"Validation threw exception with propertyName {propertyName}");
                }
                Assert.Fail($"Test incorrectly configured - validation errors: {string.Join(Environment.NewLine, ex.InnerExceptions.Select(x => x.Message))}");
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void DoesNotThrowValidationException(this Assert assert, Func<object> action, string propertyName = "")
        {
            try
            {
                action();
            }
            catch (ValidationException ex)
            {
                if (!string.IsNullOrWhiteSpace(propertyName) && ex.InnerExceptions.Any(x => x.Message.Contains(propertyName)))
                {
                    Assert.Fail($"Validation threw exception with propertyName {propertyName}");
                }
                Assert.Fail($"Test incorrectly configured - validation errors: {string.Join(Environment.NewLine, ex.InnerExceptions.Select(x => x.Message))}");
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void DoesNotThrowValidationException(this Assert assert, Action action, string propertyName = "")
        {
            try
            {
                action();
            }
            catch (ValidationException ex)
            {
                if (!string.IsNullOrWhiteSpace(propertyName) && ex.InnerExceptions.Any(x => x.Message.Contains(propertyName)))
                {
                    Assert.Fail($"Validation threw exception with propertyName {propertyName}");
                }
                Assert.Fail($"Test incorrectly configured - validation errors: {string.Join(Environment.NewLine, ex.InnerExceptions.Select(x => x.Message))}");
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void ThrowsValidationExceptionAsync(this Assert assert, Func<Task> action, string propertyName)
        {
            try
            {
                action().Wait();
                Assert.Fail();
            }
            catch (AggregateException ex)
            {
                if (ex.InnerException is ValidationException validEx)
                {
                    if (!validEx.InnerExceptions.Any(x => x.Message.Contains(propertyName)))
                    {
                        Assert.Fail($"{nameof(ValidationException)} thrown, but none with {nameof(propertyName)} {propertyName}");
                    }
                }
                else
                {
                    Assert.Fail();
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void ThrowsValidationExceptionAsync(this Assert assert, Task action, string propertyName)
        {
            try
            {
                action.Wait();
                Assert.Fail();
            }
            catch (AggregateException ex)
            {
                if (ex.InnerException is ValidationException validEx)
                {
                    if (!validEx.InnerExceptions.Any(x => x.Message.Contains(propertyName)))
                    {
                        Assert.Fail($"{nameof(ValidationException)} thrown, but none with {nameof(propertyName)} {propertyName}");
                    }
                }
                else
                {
                    Assert.Fail();
                }
            }
        }

        public static void DoesNotThrowException<T>(this Assert assert, Action action, string propertyName = "") where T : Exception
        {
            try
            {
                action();
            }
            catch (T ex)
            {
                Assert.Fail($"{nameof(T)} thrown: {ex.Message}");
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void JsonAreEquivalent(this Assert assert, string expected, string input)
        {
            JToken jsonExpected = JToken.Parse(expected);
            JToken jsonActual = JToken.Parse(input);
            Assert.IsTrue(JToken.DeepEquals(jsonExpected, jsonActual));
        }

        public static void JsonAreEquivalentFromFile(this Assert assert, string filepath, string input)
        {
            string json = Utils.ReadAllText(filepath);
            JsonAreEquivalent(assert, json, input);
        }

        public static void JsonAreEquivalentFromFile(this Assert assert, string dir, string filepath, string input)
        {
            filepath = Path.Combine(dir, filepath);
            string json = Utils.ReadAllText(filepath);
            JsonAreEquivalent(assert, json, input);
        }
    }
}
