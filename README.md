## Zeus Caching
Zeus Caching helps using existing dotnet caching extensions easily in your ASP.NET Core Web API applications by simply putting a `[ZeusCache(...)]` attribute.

### CAUTION!
**This library doesn't respect the HTTP `Cache-Control` header and is not an alternative to Microsoft [Response Caching](https://www.nuget.org/packages/Microsoft.AspNetCore.ResponseCaching/). Also, this library does not check HTTP request for `Authorization` header and runs after Authentication/Authorization middleware so BE CAREFUL OF WHAT YOU ARE CACHING. If you want to cache authorized content please read [Tips For Caching Authorized Content](#authcaching) section.**

#### Install
Using dotnet CLI:
```
dotnet add package ZeusCaching
```

Or using nuget command
```
Install-Package ZeusCaching
```

#### How to use
First you need to add the configuration to the services in the `startup.cs` file:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddMemoryCache();
    // Or...
    // services.AddDistributedMemoryCache();
    
    // ...
    // Registers the ZeusCaching services
    services.AddZeusCaching();
    // ...
}
```
You can also define multiple caching profiles with different configurations:
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddMemoryCache();
    // Or...
    // services.AddDistributedMemoryCache();
    
    // ...
    // Registers the ZeusCaching services
    services.AddZeusCaching((builder) =>
    {
        builder.AddDefaultProfile(options =>
        {
            options.UseCachingPredicate((_, req) => !req.Path.StartsWithSegments("/private"));
            options.UseDistributedCachingAdapter();
            options.UseCacheKeyHandler((sp, ctx) =>
            {
                if (ctx.User.Identity.IsAuthenticated)
                {
                    return $"USER-{ctx.User.Identity.Name}-{ctx.Request.Path}";
                }
                else
                {
                    return ctx.Request.Path;
                }
            });
        });

        builder.AddNamedProfile("Wrapped", options =>
        {
            options.UseWrappingHandler(WrapCacheResult);
            options.UseInMemoryCachingAdapter();
            options.Disable();
        });
    });
    // ...
}
```

And finally, in your controller:
```csharp
using ZeusCaching;
using Microsoft.AspNetCore.Mvc;
// ...
namespace WebApi1
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        //...

        [HttpGet, ZeusCache(10) /*Uses the default cache profile*/]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("embedded"), ZeusCache(absoluteExpirationInSeconds: 10, profileName: "Wrapped")]
        public IEnumerable<WeatherForecast> Get2()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}

```

#### <a name="authcaching"></a> Tips For Caching Authorized Content

* Make sure the underlying caching store is secure when caching authenticated/authorized contents.
* Make sure you have implemented the `options.UseCacheKeyHandler` in a way that generates unique cache key for each user/client.
* When using the `options.UseCacheKeyHandler` to generate unique keys, make sure the client/user unique value is not being passed from query string or anywhere that is accessible by end-users. Also, if user can change their username in your application, using the username in cache key is not a good idea!
* Try clearing user cache when you revoke permissions or disabling user access to secured content or at least set lower expiration intervals to reduce the change of returning authenticated content for a long time!

#### Disabling Cache Globally

If you want to disable the ZeusCaching in application level call `builder.DisableGlobally()` in the `startup.cs` file or set environment variable `Zeus_CACHING_DISABLED` to `true` and restart the application.


#### Samples
Check out the [samples](/samples) folder for more information.

#### More
Feel free to open a new issue for any bug or a feature request.
