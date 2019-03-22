using System.Net;
using CitiesInfo.Interfaces;
using CitiesInfo.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace CitiesInfo
{
    public class Startup
    {

        public static IConfiguration Configuration;
        public static IConfigurationRoot _Configuration;

        public Startup(IHostingEnvironment environment, IConfiguration configuration)
        {
            // Used on Net Core 2.2
            Configuration = configuration;

            // Used on < Net Core 2.2
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appSettings.{environment.EnvironmentName}.json", optional: false, reloadOnChange: true);

            _Configuration = builder.Build();


        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddMvcOptions(o => o.OutputFormatters.Add(
                new XmlDataContractSerializerOutputFormatter()));
            //.AddJsonOptions(o => {
            //    if (o.SerializerSettings.ContractResolver != null)
            //    {
            //        DefaultContractResolver castedResolver = o.SerializerSettings.ContractResolver
            //        as DefaultContractResolver;

            //        castedResolver.NamingStrategy = null;
            //    }
            //});

            #if DEBUG
                services.AddTransient<IMailService, LocalMailService>();
            #else
                services.AddTransient<IMailService, CloudMailService>();
            #endif

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            loggerFactory.AddNLog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            } 
            else if (env.IsProduction())
            {
                app.UseExceptionHandler(option =>
                {
                    option.Run(
                    async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "text/html";
                        IExceptionHandlerFeature ex = context.Features.Get<IExceptionHandlerFeature>();
                        if (ex != null)
                        {
                            string err = $"<h1>Error: {ex.Error.Message}</h1>{ex.Error.StackTrace}";
                            await context.Response.WriteAsync(err).ConfigureAwait(false);
                        }
                    });
                });
            }
            else
            {
                app.UseExceptionHandler();
            }

            app.UseStatusCodePages();

            app.UseMvc();

            //app.Run((context) =>
            //{
            //    throw new Exception("Example exception");
            //});
            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}
