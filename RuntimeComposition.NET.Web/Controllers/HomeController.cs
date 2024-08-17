using Microsoft.AspNetCore.Mvc;
using RuntimeComposition.NET.Contracts;
using RuntimeComposition.NET.Web.Extensions;
using RuntimeComposition.NET.Web.Keys;
using RuntimeComposition.NET.Web.Models;
using System.Diagnostics;

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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
