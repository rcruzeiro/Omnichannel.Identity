using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Omnichannel.Identity.Platform.Infrastructure.IOC;
using Swashbuckle.AspNetCore.Swagger;

namespace Omnichannel.Identity.API
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
            ConfigureIOC(services);
            ConfigureAPI(services);
            ConfigureSwagger(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors("DefaultPolicy");
            app.UseStatusCodePages();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // .net core 2.2 health check
            app.UseHealthChecks("/status");

            // swagger
            app.UseSwagger(options =>
            {
                options.RouteTemplate = "docs/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("v1/swagger.json", "Omnichannel Wallet Service v1");
                options.RoutePrefix = "docs";
            });

            // rewrite swagger route path for lambda issues
            app.UseRewriter(new RewriteOptions().AddRedirect("(.*)docs$", "$1docs/index.html"));

            app.UseMvc();
        }

        private void ConfigureIOC(IServiceCollection services)
        {
            var module = new IdentityModule(services, Configuration);
        }

        private void ConfigureAPI(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("DefaultPolicy", builder =>
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials());
            }).AddMvc()
              .AddJsonOptions(options =>
              {
                  options.SerializerSettings.Culture = new CultureInfo("pt-BR");
                  options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
              })
              .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddHealthChecks();
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Title = "Omnichannel Identity Serivce",
                    Description = "Service for omnichannel user (identity) validation",
                    Version = "v1"
                });
                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
        }
    }
}
