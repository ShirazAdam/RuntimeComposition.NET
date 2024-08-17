using RuntimeComposition.NET.Contracts;

namespace RuntimeComposition.NET.Services
{
    public class Japanese : ISomething
    {
        public string ReturnMessage()
        {
            return "ハローワールド";
        }
    }
}
