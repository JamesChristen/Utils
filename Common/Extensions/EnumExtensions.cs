using System.Reflection;

namespace System
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the <typeparamref name="T"/> attribute of the enum value. Returns null if not found
        /// </summary>
        public static T GetAttributeOfType<T>(this Enum enumVal) where T : Attribute
        {
            Type type = enumVal.GetType();
            MemberInfo[] memInfo = type.GetMember(enumVal.ToString());
            object[] attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }

        /// <summary>
        /// Returns user friendly string in the format EnumType.EnumValue
        /// </summary>
        public static string ToLongString(this Enum enumVal)
        {
            return $"{enumVal.GetType().Name}.{enumVal}";
        }
    }
}
