# RuntimeComposition.NET

[![.NET](https://github.com/ShirazAdam/RuntimeComposition.NET/actions/workflows/dotnet.yml/badge.svg)](https://github.com/ShirazAdam/RuntimeComposition.NET/actions/workflows/dotnet.yml)


- [https://gitlab.com/ShirazAdam/RuntimeComposition.NET](https://gitlab.com/ShirazAdam/RuntimeComposition.NET)
- [https://github.com/ShirazAdam/RuntimeComposition.NET](https://github.com/ShirazAdam/RuntimeComposition.NET) 

The above projects are a C# .NET 8 ASP.NET MVC Core web application implementation demonstrating the concept of registering multiple implementations against a single interface. They are based on the original work I did using C# .NET Framework 4.8.1 ASP.NET MVC web application located at:
- [https://gitlab.com/ShirazAdam/Runtime-Composition](https://gitlab.com/ShirazAdam/Runtime-Composition)
- [https://github.com/ShirazAdam/Runtime-Composition](https://github.com/ShirazAdam/Runtime-Composition)


This is an updated version for ASP.NET Core MVC (.NET 8) to showcase the same technique used in the ASP.NET MVC (.NET Framework 4.8.1).


I have seen a few articles suggesting to use an explicit delegate which I'm not a fan of, however, these articles inspired me to modernise my [previous solution at GitLab](https://gitlab.com/ShirazAdam/Runtime-Composition)/[previous solution at GitHub](https://github.com/ShirazAdam/Runtime-Composition) for .NET 8.


The articles I'm referring to are:

- https://code-maze.com/aspnetcore-register-multiple-interface-implementations/

- https://stackoverflow.com/questions/39174989/how-to-register-multiple-implementations-of-the-same-interface-in-asp-net-core


Let's begin by creating a new interface:
```csharp
namespace RuntimeComposition.NET.Contracts
{
    public interface ISomething
    {
        string ReturnMessage();
    }
}
```

Add the implementations:
```csharp
using RuntimeComposition.NET.Contracts;

namespace RuntimeComposition.NET.Services
{
    public class Arabic : ISomething
    {
        public string ReturnMessage()
        {
            return "مرحبا بالعالم";
        }
    }

    public class Japanese : ISomething
    {
        public string ReturnMessage()
        {
            return "ハローワールド";
        }
    }

}
```

This is optional. Create a new key class:
```csharp
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
```

This is optional. Create extensions for the above enums:
```csharp
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

    internal sealed class EnumValueAttribute : Attribute
    {
        internal string StringValue { get; }

        internal EnumValueAttribute(string value)
        {
            StringValue = value;
        }
    }
}
```


In your program.cs file, add the following:
```csharp
static void ConfigureDependencyInjection(IServiceCollection services)
{
    //The two implementations which have the same interface ISomething
    services.AddKeyedScoped<ISomething, Arabic>(DependencyInjectionKeys.SomethingKeysEnumeration.Arabic.ValueToStringValue());
    services.AddKeyedScoped<ISomething, Japanese>(DependencyInjectionKeys.SomethingKeysEnumeration.Japanese.ValueToStringValue());

    //DI factory to provide the correct implementation based on the value that is passed to it
    services.AddTransient<Func<string, ISomething>>(c => c.GetRequiredKeyedService<ISomething>);
}
```
AddKeyedScoped is important here as it allows us to assign a value to that specific registration. You can also use AddKeyedSingleton, AddKeyedTransient, etc. but it must be the Keyed variant registration for your dependency injection container.

Resolve the dependencies in the following manner:
```csharp
namespace RuntimeComposition.NET.Web.Controllers
{
    public class HomeController(Func<string, ISomething> something)
        : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var model = InitialiseModel();

            return View(model);
        }

        private static HomeViewModel InitialiseModel()
        {
            var model = new HomeViewModel()
            {
                Id = "0",
                Somethings =
                [
                    new CustomSelectList
                    {
                        Id = DependencyInjectionKeys.SomethingKeysEnumeration.Arabic.ValueToStringValue(),
                        Name = DependencyInjectionKeys.SomethingKeysEnumeration.Arabic.DescriptionToStringValue()
                    },

                    new CustomSelectList
                    {
                        Id = DependencyInjectionKeys.SomethingKeysEnumeration.Japanese.ValueToStringValue(),
                        Name = DependencyInjectionKeys.SomethingKeysEnumeration.Japanese.DescriptionToStringValue()
                    }
                ],
                Chosen = string.Empty
            };
            return model;
        }

        [HttpPost]
        public IActionResult Index(string id)
        {
            var myChoice = something(id);

            var model = InitialiseModel();

            model.Chosen = myChoice.ReturnMessage();

            return View(model);
        }
    }
}

```
