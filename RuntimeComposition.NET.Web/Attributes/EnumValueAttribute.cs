namespace RuntimeComposition.NET.Web.Attributes
{
    internal sealed class EnumValueAttribute : Attribute
    {
        internal string StringValue { get; }

        internal EnumValueAttribute(string value)
        {
            StringValue = value;
        }
    }
}
