using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;
using Wikiled.Common.Utilities.Config;
using Wikiled.Server.Core.Errors;
using Wikiled.Server.Core.Helpers;
using Wikiled.Server.Core.Performance;
using Wikiled.Text.Parser.Ocr;
using Wikiled.Text.Parser.Readers;
using Wikiled.Text.Parser.Service.Logic;

namespace Wikiled.Text.Parser.Service
{
    public class Startup
    {
        private readonly ILoggerFactory loggerFactory;

        private readonly ILogger<Startup> logger;

        public Startup(ILoggerFactory loggerFactory, IHostingEnvironment env)
        {
            this.loggerFactory = loggerFactory;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            Env = env;
            logger = loggerFactory.CreateLogger<Startup>();
            Configuration.ChangeNlog();
            logger.LogInformation($"Starting: {Assembly.GetExecutingAssembly().GetName().Version}");
        }

        public IConfigurationRoot Configuration { get; }

        public IHostingEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseHsts();
            }

            app.UseCors("CorsPolicy");
            app.UseExceptionHandlingMiddleware();
            app.UseHttpStatusCodeExceptionMiddleware();
            app.UseMvc();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Needed to add this section, and....
            services.AddCors(
                options =>
                {
                    options.AddPolicy(
                        "CorsPolicy",
                        itemBuider => itemBuider.AllowAnyOrigin()
                                                .AllowAnyMethod()
                                                .AllowAnyHeader()
                                                .AllowCredentials());
                });

            // Add framework services.
            services.AddMvc(options => { });

            // needed to load configuration from appsettings.json
            services.AddOptions();
            services.RegisterConfiguration<DocumentsConfig>(Configuration.GetSection("documents"));

            // Create the container builder.
            var builder = new ContainerBuilder();
            SetupOther(builder);
            builder.Populate(services);
            var appContainer = builder.Build();
            services.AddHostedService<ResourceMonitoringService>();
            logger.LogInformation("Ready!");
            // Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(appContainer);
        }

        private void SetupOther(ContainerBuilder builder)
        {
            builder.RegisterInstance(new OcrImageParser(loggerFactory.CreateLogger<OcrImageParser>(), Path.Combine(Env.ContentRootPath, "tessdata"))).As<IOcrImageParser>();
            builder.RegisterType<ApplicationConfiguration>().As<IApplicationConfiguration>();
            builder.RegisterType<EnviromentHandler>().As<IEnviromentHandler>();
            builder.RegisterType<ParserFactory>().As<ITextParserFactory>();
        }
    }
}
