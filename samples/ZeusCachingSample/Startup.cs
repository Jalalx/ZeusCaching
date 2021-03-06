using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ZeusCaching;
using System;
using Microsoft.AspNetCore.Http;
using ZeusCaching.Services;

namespace ZeusCachingSample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            // Or...
            services.AddDistributedMemoryCache();
            // Or for custom adapter:
            services.AddSingleton<ICachingAdapter>(new NaiveCachingAdapter());

            //services.AddZeusCaching();
            services.AddZeusCaching((builder) =>
            {
                // builder.DisableGlobally(),
                builder.AddDefaultProfile(options =>
                {
                    options.UseCachingPredicate((_, ctx) => !ctx.Request.Path.StartsWithSegments("/private"));
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

                builder.AddNamedProfile("WrappedProfile", options =>
                {
                    options.UseWrappingHandler(WrapCacheResult);
                    options.UseInMemoryCachingAdapter();
                    options.Enable();
                });

                builder.AddNamedProfile("DisabledProfile", options =>
                {
                    options.UseWrappingHandler(WrapCacheResult);
                    options.UseInMemoryCachingAdapter();
                    options.Disable();
                });

                builder.AddNamedProfile("CustomAdapterProfile", options =>
                {
                    options.UseCustomCachingAdapter();
                    options.Enable();
                });
            });

            services.AddControllers();
        }

        private object WrapCacheResult(IServiceProvider serviceProvider, HttpContext context, object content)
        {
            return new MyWrapper<object>
            {
                Message = "Wrapped",
                Success = true,
                Result = content
            };
        }

        internal class MyWrapper<T> where T : class
        {
            public T Result { get; set; }

            public bool Success { get; set; }

            public string Message { get; set; }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
