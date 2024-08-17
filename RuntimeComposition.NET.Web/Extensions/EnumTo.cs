using RuntimeComposition.NET.Web.Attributes;

namespace RuntimeComposition.NET.Web.Extensions
{
    public static class EnumTo
    {
        public static string DescriptionToStringValue(this Enum value)
        {
            var type = value.GetType();

            var fieldInfo = type.GetField(value.ToString());

            return (fieldInfo != null && fieldInfo.GetCustomAttributes(
                typeof(EnumDescriptionAttribute), false) is EnumDescriptionAttribute[] { Length: > 0 } attributes ? attributes[0].StringValue : null) ?? string.Empty;
        }

        public static string ValueToStringValue(this Enum value)
        {
            var type = value.GetType();

            var fieldInfo = type.GetField(value.ToString());

            return (fieldInfo != null && fieldInfo.GetCustomAttributes(
                typeof(EnumValueAttribute), false) is EnumValueAttribute[] { Length: > 0 } attributes ? attributes[0].StringValue : null) ?? string.Empty;
        }

    }
}
