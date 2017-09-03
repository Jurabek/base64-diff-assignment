using System;
using System.ComponentModel;
using System.Reflection;

namespace Base64Diff.Api.Helpers
{
    /// <summary>
    /// Provides extension methods for enumeration values.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Returns the description of an enumeration value, when the value is decorated with a <see cref="DescriptionAttribute" />.
        /// </summary>
        /// <param name="value">The enumeration value</param>.
        /// <returns>
        /// If the enumeration value is decorated by a <see cref="DescriptionAttribute"/>, the <see cref="DescriptionAttribute.Description"/> of that attribute; Otherwise, <b>null</b>.
        /// </returns>
        public static string GetDescription(this Enum value)
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            if (name == null) return null;
            var field = type.GetField(name);
            if (field == null) return null;
            var attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            return attr?.Description;
        }
    }
}
