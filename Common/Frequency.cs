using Common.Attributes;

namespace Common
{
    public enum Frequency
    {
        [SysName("-")]
        Undefined = 0,
        [SysName("D")]
        Daily = 1,
        [SysName("W")]
        Weekly = 2,
        [SysName("M")]
        Monthly = 3,
        [SysName("Q")]
        Quarterly = 4,
        [SysName("Y")]
        Yearly = 5
    }

    public static class FrequencyExtensions
    {
        /// <summary>
        /// Returns the single character code
        /// </summary>
        public static string Code(this Frequency freq)
        {
            return freq.GetAttributeOfType<SysName>()?.Name ?? freq.ToString();
        }
    }
}
