using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Test
{
    [TestClass]
    public class DynamicParamsTests
    {
        private enum Test { ASD, QWE, ZXC };

        private class TestParser : DynamicParams<Test>.DynamicParser
        {
            public override bool CanCastToType(Test param, string input)
            {
                return param switch
                {
                    Test.ASD => int.TryParse(input, out _),
                    Test.QWE => bool.TryParse(input, out _),
                    Test.ZXC => long.TryParse(input, out _),
                    _ => throw new NotImplementedException()
                };
            }

            public override dynamic CastToType(Test param, string input)
            {
                return param switch
                {
                    Test.ASD => int.Parse(input),
                    Test.QWE => bool.Parse(input),
                    Test.ZXC => long.Parse(input),
                    _ => throw new NotImplementedException()
                };
            }

            public override string ValueTypeName(Test param)
            {
                return param switch
                {
                    Test.ASD => nameof(Int32),
                    Test.QWE => nameof(Boolean),
                    Test.ZXC => nameof(Int64),
                    _ => throw new NotImplementedException()
                };
            }
        }

        #region Constructor

        [TestMethod]
        public void Test_Constructor_SameKeyValuePairSeparatorAsPairSeparatorThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => new DynamicParams<Test>(':', ':'));
        }

        [TestMethod]
        public void Test_Constructor_NullDictionaryCreatesEmptyDynamicParams()
        {
            DynamicParams<Test> @params = new DynamicParams<Test>((Dictionary<Test, dynamic>)null);
            Assert.AreEqual(0, @params.Count);
        }

        #endregion

        #region MapParam

        [TestMethod]
        public void Test_MapParam_MapsAsExpected()
        {
            DynamicParams<Test> @params = new DynamicParams<Test>();
            @params.SetParam(Test.ASD, "asd");
            @params.MapParam(Test.QWE, Test.ASD);
            Assert.AreEqual("asd", @params.GetParam<string>(Test.ASD));
            Assert.AreEqual("asd", @params.GetParam<string>(Test.QWE));
        }

        [TestMethod]
        public void Test_MapParam_MapToMappedPropertiesMapsToBaseParam()
        {
            DynamicParams<Test> @params = new DynamicParams<Test>();
            @params.SetParam(Test.ASD, "asd");
            @params.MapParam(Test.QWE, Test.ASD);
            Assert.AreEqual("asd", @params.GetParam<string>(Test.ASD));
            Assert.AreEqual("asd", @params.GetParam<string>(Test.QWE));

            @params.MapParam(Test.ZXC, Test.QWE);
            Assert.AreEqual("asd", @params.GetParam<string>(Test.ZXC));
        }

        [TestMethod]
        public void Test_MapParam_CircularMapsThrowsException()
        {
            DynamicParams<Test> @params = new DynamicParams<Test>();
            @params.MapParam(Test.ASD, Test.QWE);
            Assert.ThrowsException<ArgumentException>(() => @params.MapParam(Test.QWE, Test.ASD));
        }

        #endregion

        #region HasParam

        [TestMethod]
        public void Test_HasParam_ContainsParamReturnsTrue()
        {
            DynamicParams<Test> @params = new DynamicParams<Test>();
            @params.SetParam(Test.ASD, 123);
            Assert.IsTrue(@params.HasParam(Test.ASD));
        }

        [TestMethod]
        public void Test_HasParam_DoesNotContainParamReturnsFalse()
        {
            DynamicParams<Test> @params = new DynamicParams<Test>();
            Assert.IsFalse(@params.HasParam(Test.ASD));
        }

        [TestMethod]
        public void Test_HasParam_ContainsMappedParamReturnsTrue()
        {
            DynamicParams<Test> @params = new DynamicParams<Test>();
            @params.SetParam(Test.ASD, 123);
            @params.MapParam(Test.QWE, Test.ASD);
            Assert.IsTrue(@params.HasParam(Test.QWE));
        }

        [TestMethod]
        public void Test_HasParam_NullValueWithAllowNullReturnsTrue()
        {
            DynamicParams<Test> @params = new DynamicParams<Test>();
            @params.SetParam(Test.ASD, (int?)null);
            Assert.IsTrue(@params.HasParam(Test.ASD, allowNull: true));
        }

        [TestMethod]
        public void Test_HasParam_NullValueWithDisallowNullReturnsFalse()
        {
            DynamicParams<Test> @params = new DynamicParams<Test>();
            @params.SetParam(Test.ASD, (int?)null);
            Assert.IsFalse(@params.HasParam(Test.ASD, allowNull: false));
        }

        #endregion

        #region SetParam

        [TestMethod]
        public void Test_SetParam_SettingTwiceOverridesOldValue()
        {
            DynamicParams<Test> @params = new DynamicParams<Test>();
            @params.SetParam(Test.ASD, 123);
            Assert.AreEqual(123, @params.GetParam<int>(Test.ASD));
            @params.SetParam(Test.ASD, 456);
            Assert.AreEqual(456, @params.GetParam<int>(Test.ASD));
        }

        #endregion

        #region GetParam

        [TestMethod]
        public void Test_GetParam_HasValueReturnsValue()
        {
            DynamicParams<Test> @params = new DynamicParams<Test>();
            @params.SetParam(Test.ASD, 123);
            Assert.AreEqual(123, @params.GetParam<int>(Test.ASD));
        }

        [TestMethod]
        public void Test_GetParam_DoesNotHaveValueThrowsArgumentException()
        {
            DynamicParams<Test> @params = new DynamicParams<Test>();
            Assert.IsFalse(@params.HasParam(Test.ASD));
            Assert.ThrowsException<ArgumentException>(() => @params.GetParam<int>(Test.ASD));
        }

        [TestMethod]
        public void Test_GetParam_HasNullValueButAllowNullReturnsNull()
        {
            DynamicParams<Test> @params = new DynamicParams<Test>();
            @params.SetParam(Test.ASD, (object)null);
            Assert.AreEqual(null, @params.GetParam<object>(Test.ASD, allowNull: true));
        }

        [TestMethod]
        public void Test_GetParam_HasNullValueAndDisallowNullThrowsArgumentException()
        {
            DynamicParams<Test> @params = new DynamicParams<Test>();
            @params.SetParam(Test.ASD, (object)null);
            Assert.ThrowsException<ArgumentException>(() => @params.GetParam<object>(Test.ASD, allowNull: false));
        }

        [TestMethod]
        public void Test_GetParam_IncorrectValueTypeThrowsArgumentException()
        {
            DynamicParams<Test> @params = new DynamicParams<Test>();
            @params.SetParam(Test.ASD, "NOT AN INT");
            Assert.ThrowsException<ArgumentException>(() => @params.GetParam<int>(Test.ASD));
        }

        #endregion

        #region GetParamOrDefault

        [TestMethod]
        public void Test_GetParamorDefault_HasValueReturnsValue()
        {
            DynamicParams<Test> @params = new DynamicParams<Test>();
            @params.SetParam(Test.ASD, 123);
            Assert.AreEqual(123, @params.GetParamOrDefault(Test.ASD, 0));
        }

        [TestMethod]
        public void Test_GetParamOrDefault_DoesNotHaveValueReturnsDefault()
        {
            DynamicParams<Test> @params = new DynamicParams<Test>();
            Assert.IsFalse(@params.HasParam(Test.ASD));
            Assert.AreEqual(123, @params.GetParamOrDefault(Test.ASD, 123));
        }

        [TestMethod]
        public void Test_GetParamOrDefault_HasNullValueReturnsNull()
        {
            DynamicParams<Test> @params = new DynamicParams<Test>();
            @params.SetParam(Test.ASD, (object)null);
            Assert.AreEqual(null, @params.GetParamOrDefault<object>(Test.ASD, null));
        }

        [TestMethod]
        public void Test_GetParamOrDefault_IncorrectValueTypeThrowsArgumentException()
        {
            DynamicParams<Test> @params = new DynamicParams<Test>();
            @params.SetParam(Test.ASD, "NOT AN INT");
            Assert.ThrowsException<ArgumentException>(() => @params.GetParamOrDefault(Test.ASD, 1));
        }

        [TestMethod]
        public void Test_GetParamOrDefault_HandlesNullableType()
        {
            DynamicParams<Test> @params = new DynamicParams<Test>();
            @params.SetParam(Test.ASD, 123);
            Assert.AreEqual(123, @params.GetParamOrDefault<int?>(Test.ASD));
        }

        #endregion

        #region Equals

        [TestMethod]
        public void Test_EqualsOverride_DynamicParams_SameValuesReturnsTrue()
        {
            DynamicParams<Test> p1 = new DynamicParams<Test>(
                new Tuple<Test, dynamic>(Test.ASD, 123),
                new Tuple<Test, dynamic>(Test.QWE, true));

            DynamicParams<Test> p2 = new DynamicParams<Test>(
                new Tuple<Test, dynamic>(Test.ASD, 123),
                new Tuple<Test, dynamic>(Test.QWE, true));

            Assert.IsTrue(p1.Equals(p2));
        }

        [TestMethod]
        public void Test_EqualsOverride_DynamicParams_DifferentValuesReturnsFalse()
        {
            DynamicParams<Test> p1 = new DynamicParams<Test>(
                new Tuple<Test, dynamic>(Test.ASD, 123),
                new Tuple<Test, dynamic>(Test.QWE, true));

            DynamicParams<Test> p2 = new DynamicParams<Test>();

            Assert.IsFalse(p1.Equals(p2));
        }

        [TestMethod]
        public void Test_EqualsOverride_Dictionary_SameValuesReturnsTrue()
        {
            DynamicParams<Test> p1 = new DynamicParams<Test>(
                new Tuple<Test, dynamic>(Test.ASD, 123),
                new Tuple<Test, dynamic>(Test.QWE, true));

            Dictionary<Test, dynamic> dict = new Dictionary<Test, dynamic>
            {
                { Test.ASD, 123 },
                { Test.QWE, true }
            };

            Assert.IsTrue(p1.Equals(dict));
        }

        [TestMethod]
        public void Test_EqualsOverride_Dictionary_DifferentValuesReturnsFalse()
        {
            DynamicParams<Test> p1 = new DynamicParams<Test>(
                new Tuple<Test, dynamic>(Test.ASD, 123),
                new Tuple<Test, dynamic>(Test.QWE, true));

            Dictionary<Test, dynamic> dict = new Dictionary<Test, dynamic>();

            Assert.IsFalse(p1.Equals(dict));
        }

        [TestMethod]
        public void Test_EqualsDictionary_SameValuesReturnsTrue()
        {
            DynamicParams<Test> @params = new DynamicParams<Test>(
                new Tuple<Test, dynamic>(Test.ASD, 123),
                new Tuple<Test, dynamic>(Test.QWE, true));

            Dictionary<Test, dynamic> dict = new Dictionary<Test, dynamic>
            {
                { Test.ASD, 123 },
                { Test.QWE, true }
            };

            Assert.IsTrue(@params.Equals(dict));
        }

        [TestMethod]
        public void Test_EqualsDictionary_DifferentValuesReturnsFalse()
        {
            DynamicParams<Test> @params = new DynamicParams<Test>(
                new Tuple<Test, dynamic>(Test.ASD, 123),
                new Tuple<Test, dynamic>(Test.QWE, true));

            Assert.IsFalse(@params.Equals(new Dictionary<Test, dynamic>()));
        }

        #endregion

        #region GetHashCode

        [TestMethod]
        public void Test_GetHashCode_SameValuesReturnsSameHashCode()
        {
            DynamicParams<Test> p1 = new DynamicParams<Test>(
                new Tuple<Test, dynamic>(Test.ASD, 123),
                new Tuple<Test, dynamic>(Test.QWE, true));

            DynamicParams<Test> p2 = new DynamicParams<Test>(
                new Tuple<Test, dynamic>(Test.ASD, 123),
                new Tuple<Test, dynamic>(Test.QWE, true));

            Assert.AreEqual(p1.GetHashCode(), p2.GetHashCode());
        }

        [TestMethod]
        public void Test_GetHashCode_DifferentValuesReturnsFalse()
        {
            DynamicParams<Test> @params = new DynamicParams<Test>(
                new Tuple<Test, dynamic>(Test.ASD, 123),
                new Tuple<Test, dynamic>(Test.QWE, true));

            Assert.IsFalse(@params.Equals(new Dictionary<Test, dynamic>()));
        }

        [TestMethod]
        public void Test_GetHashCode_ChangedParametersReturnsDifferentValues()
        {
            DynamicParams<Test> @params = new DynamicParams<Test>(
                new Tuple<Test, dynamic>(Test.ASD, 123),
                new Tuple<Test, dynamic>(Test.QWE, true));

            int firstHash = @params.GetHashCode();

            @params.SetParam(Test.ASD, 456);

            Assert.AreNotEqual(firstHash, @params.GetHashCode());
        }

        #endregion

        #region IsValidInput

        [TestMethod]
        public void Test_IsValidInput_ValidReturnsTrue()
        {
            Assert.IsTrue(DynamicParams<Test>.IsValidInput("ASD:123;QWE:true;ZXC:1234567890"));
        }

        [TestMethod]
        public void Test_IsValidInput_EndingPairSeparatorReturnsTrue()
        {
            Assert.IsTrue(DynamicParams<Test>.IsValidInput("ASD:123;QWE:true;ZXC:1234567890;"));
        }

        [TestMethod]
        public void Test_IsValidInput_StartingPairSeparatorReturnsTrue()
        {
            Assert.IsTrue(DynamicParams<Test>.IsValidInput(";ASD:123;QWE:true;ZXC:1234567890"));
        }

        [TestMethod]
        public void Test_IsValidInput_EmptyPairsReturnsTrue()
        {
            Assert.IsTrue(DynamicParams<Test>.IsValidInput("ASD:123;;;;;;;QWE:true"));
        }

        [TestMethod]
        public void Test_IsValidInput_NullStringReturnsTrue()
        {
            Assert.IsTrue(DynamicParams<Test>.IsValidInput(null));
        }

        [TestMethod]
        public void Test_IsValidInput_EmptyStringReturnsTrue()
        {
            Assert.IsTrue(DynamicParams<Test>.IsValidInput(string.Empty));
        }

        [TestMethod]
        public void Test_IsValidInput_WhitespaceStringReturnsTrue()
        {
            Assert.IsTrue(DynamicParams<Test>.IsValidInput("\r\n\t"));
        }

        [TestMethod]
        public void Test_IsValidInput_UnknownKeyStillReturnsTrue()
        {
            Assert.IsFalse(EnumHelpers.IsDefined<Test>("UNKNOWN_KEY"));
            Assert.IsTrue(DynamicParams<Test>.IsValidInput("UNKNOWN_KEY:123"));
        }

        [TestMethod]
        public void Test_IsValidInput_InvalidReturnsFalse()
        {
            Assert.IsFalse(DynamicParams<Test>.IsValidInput("INVALID"));
        }

        #endregion

        #region Parse

        private static readonly TestParser _parser = new TestParser();

        [TestMethod]
        public void Test_Parse_String_ValidReturnsParams()
        {
            DynamicParams<Test> @params = DynamicParams<Test>.Parse("ASD:123;QWE:true;ZXC:1234567890", _parser);
            Assert.AreEqual(3, @params.Count);
            Assert.IsTrue(@params.HasParam(Test.ASD));
            Assert.AreEqual(123, @params.GetParam<int>(Test.ASD));
            Assert.IsTrue(@params.HasParam(Test.QWE));
            Assert.AreEqual(true, @params.GetParam<bool>(Test.QWE));
            Assert.IsTrue(@params.HasParam(Test.ASD));
            Assert.AreEqual(1234567890, @params.GetParam<long>(Test.ZXC));
        }

        [TestMethod]
        public void Test_Parse_String_SameSeparatorCharactersThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => DynamicParams<Test>.Parse(string.Empty, _parser, ':', ':'));
        }

        [TestMethod]
        public void Test_Parse_String_NullStringReturnsEmptyParams()
        {
            DynamicParams<Test> @params = DynamicParams<Test>.Parse((string)null, _parser, ':', ';');
            Assert.AreEqual(0, @params.Count);
        }

        [TestMethod]
        public void Test_Parse_String_EmptyStringReturnsEmptyParams()
        {
            DynamicParams<Test> @params = DynamicParams<Test>.Parse(string.Empty, _parser, ':', ';');
            Assert.AreEqual(0, @params.Count);
        }

        [TestMethod]
        public void Test_Parse_String_WhitespaceStringReturnsEmptyParams()
        {
            DynamicParams<Test> @params = DynamicParams<Test>.Parse("\r\n\t", _parser, ':', ';');
            Assert.AreEqual(0, @params.Count);
        }

        [TestMethod]
        public void Test_Parse_String_DoesNotMatchRegexThrowsArgumentExcepion()
        {
            Assert.IsFalse(DynamicParams<Test>.IsValidInput("INVALID"));

            try
            {
                DynamicParams<Test>.Parse("INVALID", _parser);
                Assert.Fail($"{nameof(ArgumentException)} expected");
            }
            catch (ArgumentException argEx)
            {
                Assert.IsTrue(argEx.Message.Contains("regex"));
            }
        }

        [TestMethod]
        public void Test_Parse_String_UnknownKeyThrowsArgumentExcepion()
        {
            try
            {
                DynamicParams<Test>.Parse("UNKNOWN_KEY:123", _parser);
                Assert.Fail($"{nameof(ArgumentException)} expected");
            }
            catch (ArgumentException argEx)
            {
                Assert.IsTrue(argEx.Message.Contains("Unknown"));
            }
        }

        [TestMethod]
        public void Test_Parse_String_CannotCastToTypeThrowsAggregateException()
        {
            Assert.IsFalse(_parser.CanCastToType(Test.ASD, "NOT_AN_INT"));

            try
            {
                DynamicParams<Test>.Parse("ASD:NOT_AN_INT", _parser);
                Assert.Fail($"{nameof(AggregateException)} expected");
            }
            catch (AggregateException aggEx)
            {
                Assert.AreEqual(1, aggEx.InnerExceptions.Count);
                Exception ex = aggEx.InnerExceptions.First();
                Assert.IsTrue(ex is ArgumentException);
                Assert.IsTrue(ex.Message.Contains("NOT_AN_INT"));
            }
        }

        [TestMethod]
        public void Test_Parse_Dictionary_ValidReturnsParams()
        {
            Dictionary<Test, string> dict = new Dictionary<Test, string>
            {
                { Test.ASD, "123" },
                { Test.QWE, "true" },
                { Test.ZXC, "1234567890" }
            };

            DynamicParams<Test> @params = DynamicParams<Test>.Parse(dict, _parser);
            Assert.AreEqual(3, @params.Count);
            Assert.IsTrue(@params.HasParam(Test.ASD));
            Assert.AreEqual(123, @params.GetParam<int>(Test.ASD));
            Assert.IsTrue(@params.HasParam(Test.QWE));
            Assert.AreEqual(true, @params.GetParam<bool>(Test.QWE));
            Assert.IsTrue(@params.HasParam(Test.ASD));
            Assert.AreEqual(1234567890, @params.GetParam<long>(Test.ZXC));
        }

        [TestMethod]
        public void Test_Parse_Dictionary_EmptyDictionaryReturnsEmptyParams()
        {
            DynamicParams<Test> @params = DynamicParams<Test>.Parse(new Dictionary<Test, string>(), _parser);
            Assert.AreEqual(0, @params.Count);
        }

        [TestMethod]
        public void Test_Parse_Dictionary_NullDictionaryThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => DynamicParams<Test>.Parse((Dictionary<Test, string>)null, _parser));
        }

        [TestMethod]
        public void Test_Parse_Dictionary_CannotCastToTypeThrowsAggregateException()
        {
            Assert.IsFalse(_parser.CanCastToType(Test.ASD, "NOT_AN_INT"));

            Dictionary<Test, string> dict = new Dictionary<Test, string>
            {
                { Test.ASD, "NOT_AN_INT" }
            };

            try
            {
                DynamicParams<Test>.Parse(dict, _parser);
                Assert.Fail($"{nameof(AggregateException)} expected");
            }
            catch (AggregateException aggEx)
            {
                Assert.AreEqual(1, aggEx.InnerExceptions.Count);
                Exception ex = aggEx.InnerExceptions.First();
                Assert.IsTrue(ex is ArgumentException);
                Assert.IsTrue(ex.Message.Contains("NOT_AN_INT"));
            }
        }

        #endregion
    }
}
