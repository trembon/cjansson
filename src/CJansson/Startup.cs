using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CJansson.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CJansson
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting(options => {
                options.AppendTrailingSlash = true;
                options.LowercaseUrls = true;
            });

            services.AddMvc();
            services.AddHttpClient();

            services.AddSingleton<IBlogService, BlogService>();
            services.AddSingleton<IGitHubService, GitHubService>();
            services.AddSingleton<IFileEventService, FileEventService>();
            services.AddSingleton<IContentProcessorService, ContentProcessorService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBlogService blogService, IFileEventService fileEventService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

            blogService.Build();
            fileEventService.Bind();
        }
    }
}
