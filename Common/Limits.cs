using Common.Validation;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace Common
{
    public class Limits : IValidatable
    {
        public decimal? UpperLimit { get; set; }
        public decimal? LowerLimit { get; set; }
        public bool? IsUpperValueIncluded { get; set; }
        public bool? IsLowerValueIncluded { get; set; }

        public Limits(decimal? upperLimit, decimal? lowerLimit, bool? isUpperValueIncluded, bool? isLowerValueIncluded)
        {
            UpperLimit = upperLimit;
            LowerLimit = lowerLimit;
            IsUpperValueIncluded = isUpperValueIncluded;
            IsLowerValueIncluded = isLowerValueIncluded;
        }

        public bool IsInRange(decimal value)
        {
            if (!IsValid())
            {
                throw new InvalidOperationException($"Limits ({ToString()}) invalid - cannot check in range");
            }

            if (UpperLimit.HasValue && LowerLimit.HasValue)
            {
                if (!(IsUpperValueIncluded.Value && IsLowerValueIncluded.Value && LowerLimit.Value <= value && value <= UpperLimit.Value)
                    && !(IsUpperValueIncluded.Value && LowerLimit.Value < value && value <= UpperLimit.Value)
                    && !(IsLowerValueIncluded.Value && LowerLimit.Value <= value && value < UpperLimit.Value)
                    && !(!IsLowerValueIncluded.Value && !IsUpperValueIncluded.Value && LowerLimit.Value < value && value < UpperLimit.Value))
                {
                    return false;
                }
            }
            else if (UpperLimit.HasValue)
            {
                if (!(IsUpperValueIncluded.Value && value <= UpperLimit.Value)
                    && !(!IsUpperValueIncluded.Value && value < UpperLimit.Value))
                {
                    return false;
                }
            }
            else if (LowerLimit.HasValue)
            {
                if (!(IsLowerValueIncluded.Value && LowerLimit.Value <= value)
                    && !(!IsLowerValueIncluded.Value && LowerLimit.Value < value))
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsValid()
        {
            if ((UpperLimit.HasValue && IsUpperValueIncluded == null)
                || (LowerLimit.HasValue && IsLowerValueIncluded == null)
                || (IsUpperValueIncluded.HasValue && UpperLimit == null)
                || (IsLowerValueIncluded.HasValue && LowerLimit == null))
            {
                return false; // Doesn't make sense to have an upper/lower limit and no brackets, or vice versa
            }

            if (UpperLimit.HasValue && LowerLimit.HasValue)
            {
                if (UpperLimit.Value == LowerLimit.Value)
                {
                    // If limits are the same both need to be included
                    // Doesn't make sense to have something like [0,0) or (0,0)
                    return IsUpperValueIncluded.Value && IsLowerValueIncluded.Value; 
                }
                return UpperLimit.Value > LowerLimit.Value;
            }

            return true;
        }

        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            if (!IsValid())
            {
                output.Append("Invalid: ");
            }

            if (LowerLimit.HasValue)
            {
                output.Append(IsLowerValueIncluded.Value ? "[" : "(");
                output.Append(LowerLimit.ToString());
            }
            output.Append(',');
            if (UpperLimit.HasValue)
            {
                output.Append(UpperLimit.ToString());
                output.Append(IsUpperValueIncluded.Value ? "]" : ")");
            }
            return output.ToString();
        }

        public string ValidationIdentifier => ToString();

        public void CheckValid()
        {
            Validate.That
                    .IsValidInput(IsValid(), x => x, $"UpperLimit has to be greater than or equal to LowerLimit")
                    .Check();
        }

        private static readonly Regex _regex = new Regex(@"^((\(|\[)\s*(\d+(\.\d+)*))?\s*,\s*((\d+(\.\d+)*)\s*(\)|\]))?$");

        public static bool CanParse(string input, out Limits limits)
        {
            if (_regex.IsMatch(input))
            {
                limits = Parse(input);
                return true;
            }
            limits = null;
            return false;
        }

        public static Limits Parse(string input)
        {
            if (!_regex.IsMatch(input))
            {
                throw new ArgumentException($"Limits input ({input}) does not match regex");
            }

            input = input.RemoveWhiteSpace();
            bool? isUpperValueIncluded = null;
            bool? isLowerValueIncluded = null;
            decimal? upperLimit = null;
            decimal? lowerLimit = null;

            string[] inputs = input.Split(",");

            if (!string.IsNullOrEmpty(inputs[0]))
            {
                isLowerValueIncluded = inputs[0][0] == '[';
                lowerLimit = decimal.Parse(inputs[0][1..]);
            }

            if (!string.IsNullOrEmpty(inputs[1]))
            {
                isUpperValueIncluded = inputs[1][^1] == ']';
                upperLimit = decimal.Parse(inputs[1][..^1]);
            }

            Limits limits = new Limits(upperLimit, lowerLimit, isUpperValueIncluded, isLowerValueIncluded);
            if (!limits.IsValid())
            {
                throw new ArgumentException($"Upper limit: {upperLimit} has to be greater than lower limit: {lowerLimit}");
            }

            return limits;
        }

        public class LimitsJsonConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(Limits);
            }

            public override bool CanRead => false;

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public override bool CanWrite => true;

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                writer.WriteValue(((Limits)value).ToString());
            }
        }
    }
}
