using CJansson.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables(prefix: "APP_");

builder.Services.AddRouting(options =>
{
    options.AppendTrailingSlash = true;
    options.LowercaseUrls = true;
});

builder.Services.AddMvc();
builder.Services.AddHttpClient();

builder.Services.AddSingleton<IBlogService, BlogService>();
builder.Services.AddSingleton<IGitHubService, GitHubService>();
builder.Services.AddSingleton<IFileEventService, FileEventService>();
builder.Services.AddSingleton<IContentProcessorService, ContentProcessorService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
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
app.MapDefaultControllerRoute();

app.Services.GetRequiredService<IBlogService>().Build();
app.Services.GetRequiredService<IFileEventService>().Bind();

app.Run();