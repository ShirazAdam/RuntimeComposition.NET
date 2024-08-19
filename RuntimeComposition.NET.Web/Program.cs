using RuntimeComposition.NET.Contracts;
using RuntimeComposition.NET.Services;
using RuntimeComposition.NET.Web.Extensions;
using RuntimeComposition.NET.Web.Keys;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

ConfigureDependencyInjection(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
return;

static void ConfigureDependencyInjection(IServiceCollection services)
{
    //The two implementations which have the same interface ISomething
    services.AddKeyedScoped<ISomething, Arabic>(DependencyInjectionKeys.SomethingKeysEnumeration.Arabic.ValueToStringValue());
    services.AddKeyedScoped<ISomething, Japanese>(DependencyInjectionKeys.SomethingKeysEnumeration.Japanese.ValueToStringValue());

    //DI factory to provide the correct implementation based on the value that is passed to it
    services.AddTransient<Func<string, ISomething>>(service => service.GetRequiredKeyedService<ISomething>);
}
