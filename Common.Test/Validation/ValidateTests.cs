using Common.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Test.Validation
{
    [TestClass]
    public class ValidateTests
    {
        [TestMethod]
        public void Test_IsNull_NullDoesNotThrowValidationException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.IsNull((ValidateTests)null, "name"), "name");
        }

        [TestMethod]
        public void Test_IsNull_NotNullThrowsValidationException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsNull("NOT NULL", "name"), "name");
        }

        [TestMethod]
        public void Test_IsNotNull_NotNullDoesNotThrowValidationException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.IsNotNull("NOT NULL", "name"), "name");
        }

        [TestMethod]
        public void Test_IsNotNull_NullThrowsValidationException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsNotNull((ValidateTests)null, "name"), "name");
        }

        [TestMethod]
        public void Test_Decimal_IsNegative_NegativeDoesNotThrowValidationException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.IsNegative(-1M, "name"), "name");
        }

        [TestMethod]
        public void Test_Decimal_IsNegative_ZeroThrowsValidationException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsNegative(0M, "name"), "name");
        }

        [TestMethod]
        public void Test_Decimal_IsNegative_PositiveThrowsValidationException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsNegative(1M, "name"), "name");
        }

        [TestMethod]
        public void Test_Decimal_IsNegativeOrZero_NegativeDoesNotThrowValidationException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.IsNegativeOrZero(-1M, "name"), "name");
        }

        [TestMethod]
        public void Test_Decimal_IsNegativeOrZero_ZeroDoesNotThrowValidationException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.IsNegativeOrZero(0M, "name"), "name");
        }

        [TestMethod]
        public void Test_Decimal_IsNegativeOrZero_PositiveThrowsValidationException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsNegativeOrZero(1M, "name"), "name");
        }

        [TestMethod]
        public void Test_Decimal_IsZero_NegativeThrowsValidationException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsZero(-1M, "name"), "name");
        }

        [TestMethod]
        public void Test_Decimal_IsZero_ZeroDoesNotThrowValidationException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.IsZero(0M, "name"), "name");
        }

        [TestMethod]
        public void Test_Decimal_IsZero_PositiveThrowsValidationException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsZero(1M, "name"), "name");
        }

        [TestMethod]
        public void Test_Decimal_IsPositive_NegativeThrowsValidationException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsPositive(-1M, "name"), "name");
        }

        [TestMethod]
        public void Test_Decimal_IsPositive_ZeroThrowsValidationException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsPositive(0M, "name"), "name");
        }

        [TestMethod]
        public void Test_Decimal_IsPositive_PositiveDoesNotThrowValidationException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.IsPositive(1M, "name"), "name");
        }

        [TestMethod]
        public void Test_Decimal_IsPositiveOrZero_NegativeThrowsValidationException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsPositiveOrZero(-1M, "name"), "name");
        }

        [TestMethod]
        public void Test_Decimal_IsPositiveOrZero_ZeroDoesNotThrowValidationException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.IsPositiveOrZero(0M, "name"), "name");
        }

        [TestMethod]
        public void Test_Decimal_IsPositiveOrZero_PositiveDoesNotThrowValidationException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.IsPositiveOrZero(1M, "name"), "name");
        }

        [TestMethod]
        public void Test_Long_IsNegative_NegativeDoesNotThrowValidationException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.IsNegative(-1L, "name"), "name");
        }

        [TestMethod]
        public void Test_Long_IsNegative_ZeroThrowsValidationException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsNegative(0L, "name"), "name");
        }

        [TestMethod]
        public void Test_Long_IsNegative_PositiveThrowsValidationException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsNegative(1L, "name"), "name");
        }

        [TestMethod]
        public void Test_Long_IsNegativeOrZero_NegativeDoesNotThrowValidationException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.IsNegativeOrZero(-1L, "name"), "name");
        }

        [TestMethod]
        public void Test_Long_IsNegativeOrZero_ZeroDoesNotThrowValidationException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.IsNegativeOrZero(0L, "name"), "name");
        }

        [TestMethod]
        public void Test_Long_IsNegativeOrZero_PositiveThrowsValidationException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsNegativeOrZero(1L, "name"), "name");
        }

        [TestMethod]
        public void Test_Long_IsZero_NegativeThrowsValidationException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsZero(-1L, "name"), "name");
        }

        [TestMethod]
        public void Test_Long_IsZero_ZeroDoesNotThrowValidationException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.IsZero(0L, "name"), "name");
        }

        [TestMethod]
        public void Test_Long_IsZero_PositiveThrowsValidationException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsZero(1L, "name"), "name");
        }

        [TestMethod]
        public void Test_Long_IsPositive_NegativeThrowsValidationException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsPositive(-1L, "name"), "name");
        }

        [TestMethod]
        public void Test_Long_IsPositive_ZeroThrowsValidationException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsPositive(0L, "name"), "name");
        }

        [TestMethod]
        public void Test_Long_IsPositive_PositiveDoesNotThrowValidationException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.IsPositive(1L, "name"), "name");
        }

        [TestMethod]
        public void Test_Long_IsPositiveOrZero_NegativeThrowsValidationException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsPositiveOrZero(-1L, "name"), "name");
        }

        [TestMethod]
        public void Test_Long_IsPositiveOrZero_ZeroDoesNotThrowValidationException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.IsPositiveOrZero(0L, "name"), "name");
        }

        [TestMethod]
        public void Test_Long_IsPositiveOrZero_PositiveDoesNotThrowValidationException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.IsPositiveOrZero(1L, "name"), "name");
        }

        [TestMethod]
        public void Test_Int_IsNegative_NegativeDoesNotThrowValidationException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.IsNegative(-1, "name"), "name");
        }

        [TestMethod]
        public void Test_Int_IsNegative_ZeroThrowsValidationException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsNegative(0, "name"), "name");
        }

        [TestMethod]
        public void Test_Int_IsNegative_PositiveThrowsValidationException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsNegative(1, "name"), "name");
        }

        [TestMethod]
        public void Test_Int_IsNegativeOrZero_NegativeDoesNotThrowValidationException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.IsNegativeOrZero(-1, "name"), "name");
        }

        [TestMethod]
        public void Test_Int_IsNegativeOrZero_ZeroDoesNotThrowValidationException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.IsNegativeOrZero(0, "name"), "name");
        }

        [TestMethod]
        public void Test_Int_IsNegativeOrZero_PositiveThrowsValidationException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsNegativeOrZero(1, "name"), "name");
        }

        [TestMethod]
        public void Test_Int_IsZero_NegativeThrowsValidationException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsZero(-1, "name"), "name");
        }

        [TestMethod]
        public void Test_Int_IsZero_ZeroDoesNotThrowValidationException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.IsZero(0, "name"), "name");
        }

        [TestMethod]
        public void Test_Int_IsZero_PositiveThrowsValidationException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsZero(1, "name"), "name");
        }

        [TestMethod]
        public void Test_Int_IsPositive_NegativeThrowsValidationException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsPositive(-1, "name"), "name");
        }

        [TestMethod]
        public void Test_Int_IsPositive_ZeroThrowsValidationException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsPositive(0, "name"), "name");
        }

        [TestMethod]
        public void Test_Int_IsPositive_PositiveDoesNotThrowValidationException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.IsPositive(1, "name"), "name");
        }

        [TestMethod]
        public void Test_Int_IsPositiveOrZero_NegativeThrowsValidationException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsPositiveOrZero(-1, "name"), "name");
        }

        [TestMethod]
        public void Test_Int_IsPositiveOrZero_ZeroDoesNotThrowValidationException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.IsPositiveOrZero(0, "name"), "name");
        }

        [TestMethod]
        public void Test_Int_IsPositiveOrZero_PositiveDoesNotThrowValidationException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.IsPositiveOrZero(1, "name"), "name");
        }

        [TestMethod]

        public void Test_String_IsNullOrEmpty_NullDoesNotThrowValidationException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.IsNullOrEmpty((string)null, "name"), "name");
        }

        [TestMethod]

        public void Test_String_IsNullOrEmpty_EmptyDoesNotThrowValidationException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.IsNullOrEmpty(string.Empty, "name"), "name");
        }

        [TestMethod]

        public void Test_String_IsNullOrEmpty_WhiteSpaceThrowsValidationException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsNullOrEmpty("\n", "name"), "name");
        }

        [TestMethod]

        public void Test_String_IsNullOrEmpty_NonWhiteSpaceThrowsValidationException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsNullOrEmpty("SOMETHING", "name"), "name");
        }

        [TestMethod]

        public void Test_String_IsNullOrWhiteSpace_NullDoesNotThrowValidationException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.IsNullOrWhiteSpace((string)null, "name"), "name");
        }

        [TestMethod]

        public void Test_String_IsNullOrWhiteSpace_EmptyDoesNotThrowValidationException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.IsNullOrWhiteSpace(string.Empty, "name"), "name");
        }

        [TestMethod]

        public void Test_String_IsNullOrWhiteSpace_WhiteSpaceDoesNotThrowValidationException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.IsNullOrWhiteSpace("\n", "name"), "name");
        }

        [TestMethod]

        public void Test_String_IsNullOrWhiteSpace_NonWhiteSpaceThrowsValidationException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsNullOrWhiteSpace("SOMETHING", "name"), "name");
        }

        [TestMethod]

        public void Test_String_IsNotNullOrEmpty_NullThrowsValidationException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsNotNullOrEmpty((string)null, "name"), "name");
        }

        [TestMethod]

        public void Test_String_IsNotNullOrEmpty_EmptyThrowsValidationException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsNotNullOrEmpty(string.Empty, "name"), "name");
        }

        [TestMethod]

        public void Test_String_IsNotNullOrEmpty_WhiteSpaceDoesNotThrowValidationException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.IsNotNullOrEmpty("\n", "name"), "name");
        }

        [TestMethod]

        public void Test_String_IsNotNullOrEmpty_NonWhiteSpaceDoesNotThrowValidationException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.IsNotNullOrEmpty("SOMETHING", "name"), "name");
        }

        [TestMethod]

        public void Test_String_IsNotNullOrWhiteSpace_NullThrowsValidationException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsNotNullOrWhiteSpace((string)null, "name"), "name");
        }

        [TestMethod]

        public void Test_String_IsNotNullOrWhiteSpace_EmptyThrowsValidationException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsNotNullOrWhiteSpace(string.Empty, "name"), "name");
        }

        [TestMethod]

        public void Test_String_IsNotNullOrWhiteSpace_WhiteSpaceThrowsValidationException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsNotNullOrWhiteSpace("\n", "name"), "name");
        }

        [TestMethod]

        public void Test_String_IsNotNullOrWhiteSpace_NonWhiteSpaceDoesNotThrowValidationException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.IsNotNullOrWhiteSpace("SOMETHING", "name"), "name");
        }

        [TestMethod]
        public void Test_Enumerable_IsNullOrEmpty_NullDoesNotThrowException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.IsNullOrEmpty((IEnumerable<int>)null, "name"), "name");
        }

        [TestMethod]
        public void Test_Enumerable_IsNullOrEmpty_EmptyDoesNotThrowException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.IsNullOrEmpty(Array.Empty<int>(), "name"), "name");
        }

        [TestMethod]
        public void Test_Enumerable_IsNullOrEmpty_NotEmptyThrowsException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsNullOrEmpty(new int[] { 0, 1 }, "name"), "name");
        }

        [TestMethod]
        public void Test_Enumerable_IsNotNullOrEmpty_NullThrowsException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsNotNullOrEmpty((IEnumerable<int>)null, "name"), "name");
        }

        [TestMethod]
        public void Test_Enumerable_IsNotNullOrEmpty_EmptyThrowsException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsNotNullOrEmpty(Array.Empty<int>(), "name"), "name");
        }

        [TestMethod]
        public void Test_Enumerable_IsNotNullOrEmpty_NotEmptyDoesNotThrowException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.IsNotNullOrEmpty(new int[] { 0, 1 }, "name"), "name");
        }

        [TestMethod]
        public void Test_IsTrue_TrueDoesNotThrowException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.IsTrue(true, "name"), "name");
        }

        [TestMethod]
        public void Test_IsTrue_FalseThrowsException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsTrue(false, "name"), "name");
        }

        [TestMethod]
        public void Test_IsFalse_TrueThrowsException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsFalse(true, "name"), "name");
        }

        [TestMethod]
        public void Test_IsFalse_FalseDoesNotThrowException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.IsFalse(false, "name"), "name");
        }

        [TestMethod]
        public void Test_AreEqual_TrueDoesNotThrowException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.AreEqual(true, true, "name"), "name");
        }

        [TestMethod]
        public void Test_AreEqual_FalseThrowsException()
        {
            Assert.That.ThrowsValidationException(Validate.That.AreEqual(false, true, "name"), "name");
        }

        [TestMethod]
        public void Test_AreEqual_OnlyNullInputArgumentThrowsArgumentNullException()
        {
            Assert.That.ThrowsValidationException(Validate.That.AreEqual(null, this, "name"), "input");
        }

        [TestMethod]
        public void Test_AreEqual_OnlyNullExpectedArgumentThrowsArgumentNullException()
        {
            Assert.That.ThrowsValidationException(Validate.That.AreEqual(this, null, "name"), "expected");
        }

        [TestMethod]
        public void Test_AreEqual_BothArgumentsNullDoesNotThrowException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.AreEqual<ValidateTests>(null, null, "name"), "name");
        }

        private class AreEqualTest
        {
            public override bool Equals(object obj) => obj is AreEqualTest;
            public override int GetHashCode() => 0;
        }

        [TestMethod]
        public void Test_AreEqual_EqualArgumentsDoesNotThrowException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.AreEqual(new AreEqualTest(), new AreEqualTest(), "name"), "name");
        }

        private class NotEqualTest
        {
            public override bool Equals(object obj) => false;
            public override int GetHashCode() => 0;
        }

        [TestMethod]
        public void Test_AreEqual_NotEqualArgumentsThrowsException()
        {
            Assert.That.ThrowsValidationException(Validate.That.AreEqual(new NotEqualTest(), new NotEqualTest(), "name"), "name");
        }

        [TestMethod]
        public void Test_AreEqual_Comparison_TrueDoesNotThrowException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.AreEqual(5, true, (i, b) => true, "name"), "name");
        }

        [TestMethod]
        public void Test_AreEqual_Comparison_FalseThrowsException()
        {
            Assert.That.ThrowsValidationException(Validate.That.AreEqual(5, true, (i, b) => false, "name"), "name");
        }

        [TestMethod]
        public void Test_AreEqual_Comparison_NullComparisonThrowsArgumentNullException()
        {
            Assert.That.ThrowsValidationException(Validate.That.AreEqual(5, true, null, "name"), "comparison");
        }

        [TestMethod]
        public void Test_IsValidInput_IsValidDoesNotThrowException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.IsValidInput(5, x => true, "name"), "name");
        }

        [TestMethod]
        public void Test_IsValidInput_NotValidThrowsException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsValidInput(5, x => false, "name"), "name");
        }

        [TestMethod]
        public void Test_IsValidInput_NullPredicateThrowsArgumentNullException()
        {
            Assert.That.ThrowsValidationException(Validate.That.IsValidInput(5, null, "name"), "predicate");
        }

        [TestMethod]
        public void Test_RegexMatches_IsMatchDoesNotThrowException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.RegexMatches("INPUT", "INPUT", "name"), "name");
        }

        [TestMethod]
        public void Test_RegexMatches_NoMatchThrowsException()
        {
            Assert.That.ThrowsValidationException(Validate.That.RegexMatches("INPUT", "NO MATCH", "name"), "name");
        }

        private class NotValid : IValidatable
        {
            public string ValidationIdentifier => nameof(NotValid);
            public void CheckValid() => Common.Validation.Validate.That.IsTrue(false, "name").Check();
        }

        private class IsValid : IValidatable
        {
            public string ValidationIdentifier => nameof(IsValid);
            public void CheckValid() => Common.Validation.Validate.That.IsTrue(true, "name").Check();
        }

        [TestMethod]
        public void Test_IsValid_ValidDoesNotThrowException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.IsValid(new IsValid()), nameof(IsValid));
        }

        [TestMethod]
        public void Test_IsValid_NotValidThrowsException()
        {
            IValidatable notValid = new NotValid();
            Assert.That.ThrowsValidationException(Validate.That.IsValid(notValid), notValid.ValidationIdentifier);
        }

        [TestMethod]
        public void Test_AreValid_EmptyCollectionDoesNotThrowException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.AreValid(), nameof(IsValid));
        }

        [TestMethod]
        public void Test_AreValid_NullCollectionDoesNotThrowException()
        {
            Assert.That.DoesNotThrowValidationException(Validate.That.AreValid((IEnumerable<IValidatable>)null), nameof(IsValid));
        }

        [TestMethod]
        public void Test_AreValid_AllValidDoesNotThrowException()
        {
            IValidatable[] arr = new IValidatable[] { new IsValid(), new IsValid() };
            Assert.That.DoesNotThrowValidationException(Validate.That.AreValid(arr), nameof(IsValid));
        }

        [TestMethod]
        public void Test_AreValid_AnyNotValidThrowsException()
        {
            IValidatable valid = new IsValid();
            IValidatable notValid = new NotValid();
            IValidatable[] arr = new IValidatable[] { valid, notValid };
            Assert.That.ThrowsValidationException(Validate.That.AreValid(arr), notValid.ValidationIdentifier);
        }

        [TestMethod]
        public void Test_ContainsKey_ContainsDoesNotThrowException()
        {
            Dictionary<int, string> dict = new Dictionary<int, string> { { 1, "STR" } };
            Assert.That.DoesNotThrowValidationException(Validate.That.ContainsKey(dict, 1, "name"), "name");
        }

        [TestMethod]
        public void Test_ContainsKey_NotContainsThrowsException()
        {
            Dictionary<int, string> dict = new Dictionary<int, string> { { 1, "STR" } };
            Assert.That.ThrowsValidationException(Validate.That.ContainsKey(dict, 0, "name"), "name");
        }

        [TestMethod]
        public void Test_ContainsKey_NullDictionaryThrowsException()
        {
            Dictionary<int, string> dict = null;
            Assert.That.ThrowsValidationException(Validate.That.ContainsKey(dict, 0, "name"), "dictionary");
        }

        [TestMethod]
        public void Test_Then_DoesNotRunChildTestIfParentFails()
        {
            try
            {
                Validate.That
                        .IsTrue(false, "parent")
                        .Then.IsTrue(true, "child")
                        .Check();

                Assert.Fail(); // Validation should fail and throw exception
            }
            catch (ValidationException ex)
            {
                Assert.IsTrue(ex.InnerExceptions.Any(x => x.Message.Contains("parent")));
                Assert.IsFalse(ex.InnerExceptions.Any(x => x.Message.Contains("child")));
            }
        }

        [TestMethod]
        public void Test_Then_RunsChildTestIfParentSucceeds()
        {
            try
            {
                Validate.That
                        .IsTrue(true, "parent")
                        .Then.IsTrue(false, "child")
                        .Check();

                Assert.Fail(); // Validation should fail and throw exception
            }
            catch (ValidationException ex)
            {
                Assert.IsFalse(ex.InnerExceptions.Any(x => x.Message.Contains("parent")));
                Assert.IsTrue(ex.InnerExceptions.Any(x => x.Message.Contains("child")));
            }
        }

        [TestMethod]
        public void Test_Then_ParentAndChildSucceed()
        {
            try
            {
                Validate.That
                        .IsTrue(true, "parent")
                        .Then.IsTrue(true, "child")
                        .Check();
            }
            catch (ValidationException)
            {
                Assert.Fail($"Validation failed");
            }
        }

        [TestMethod]
        public void Test_Then_FirstChildDoesNotPreventSecondChildFromRunning()
        {
            try
            {
                Validate.That
                        .IsTrue(true, "parent1")
                        .Then.IsTrue(false, "child1-1")
                             .IsTrue(true, "child2-2")
                        .Check();
            }
            catch (ValidationException ex)
            {
                Assert.IsTrue(ex.InnerExceptions.Count == 1);
                Assert.IsTrue(ex.InnerExceptions.Single().Message.Contains("child1-1"));
            }
        }

        [TestMethod]
        public void Test_Then_ThrowsInvalidOperationExceptionFromEmptyBranch()
        {
            Assert.ThrowsException<InvalidOperationException>(() => Validate.That.Then);
        }

        [TestMethod]
        public void Test_Root_ReturnsBranchToRoot()
        {
            try
            {
                Validate.That
                        .IsTrue(true, "parent1")
                        .Then.IsTrue(false, "child1-1")
                        .Root.IsTrue(true, "parent2")
                        .Check();
            }
            catch (ValidationException ex)
            {
                Assert.IsTrue(ex.InnerExceptions.Count == 1);
                Assert.IsTrue(ex.InnerExceptions.Single().Message.Contains("child1-1"));
            }
        }

        [TestMethod]
        public void Test_Back_ReturnsToPreviousBranch()
        {
            try
            {
                Validate.That
                        .IsTrue(true, "parent1")
                        .Then.IsTrue(true, "child1-1")
                             .Then.IsTrue(true, "granchild1-1-1")
                                  .Back.IsTrue(true, "child1-2")
                        .Check();
            }
            catch (ValidationException ex)
            {
                Assert.IsTrue(ex.InnerExceptions.Count == 1);
                Assert.IsTrue(ex.InnerExceptions.Single().Message.Contains("child1-1"));
            }
        }

        [TestMethod]
        public void Test_Back_ThrowsInvalidOperationExceptionFromEmptyBranch()
        {
            Assert.ThrowsException<InvalidOperationException>(() => Validate.That.Back);
        }

        [TestMethod]
        public void Test_If_Predicate_DoesNotRunChildIfPredicateFailed()
        {
            try
            {
                Validate.That
                        .If(() => false)
                        .IsTrue(false, "child")
                        .Check();
            }
            catch (ValidationException)
            {
                Assert.Fail($"Child test ran");
            }
        }

        [TestMethod]
        public void Test_If_Predicate_DoesRunChildIfPredicateSucceeded()
        {
            try
            {
                Validate.That
                        .If(() => true)
                        .IsTrue(false, "child")
                        .Check();
            }
            catch (ValidationException ex)
            {
                Assert.IsTrue(ex.InnerExceptions.Any(x => x.Message.Contains("child")));
            }
        }

        [TestMethod]
        public void Test_If_Predicate_RunsIfPredicateSucceeded()
        {
            Validate.That
                    .If(() => true)
                    .IsTrue(true, "child")
                    .Check();
        }

        [TestMethod]
        public void Test_If_Bool_DoesNotRunChildIfFalse()
        {
            try
            {
                Validate.That
                        .If(false)
                        .IsTrue(false, "child")
                        .Check();
            }
            catch (ValidationException)
            {
                Assert.Fail($"Child test ran");
            }
        }

        [TestMethod]
        public void Test_If_Bool_DoesRunChildIfTrue()
        {
            try
            {
                Validate.That
                        .If(true)
                        .IsTrue(false, "child")
                        .Check();
            }
            catch (ValidationException ex)
            {
                Assert.IsTrue(ex.InnerExceptions.Any(x => x.Message.Contains("child")));
            }
        }

        [TestMethod]
        public void Test_If_Bool_RunsIfTrue()
        {
            Validate.That
                    .If(() => true)
                    .IsTrue(true, "child")
                    .Check();
        }
    }
}
