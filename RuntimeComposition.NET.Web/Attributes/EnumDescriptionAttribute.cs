namespace RuntimeComposition.NET.Web.Attributes
{
    internal sealed class EnumDescriptionAttribute : Attribute
    {
        internal string? StringValue { get; }


        internal EnumDescriptionAttribute(string? value)
        {
            StringValue = value;
        }
    }
}
