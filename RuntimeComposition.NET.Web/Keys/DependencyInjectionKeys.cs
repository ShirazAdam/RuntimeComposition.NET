using RuntimeComposition.NET.Web.Attributes;

namespace RuntimeComposition.NET.Web.Keys
{
    public sealed class DependencyInjectionKeys
    {
        public enum SomethingKeysEnumeration
        {
            [EnumDescription("Hello world in Arabic"), EnumValue("1")]
            Arabic,
            [EnumDescription("Hello world in Japanese"), EnumValue("2")]
            Japanese
        }
    }
}
