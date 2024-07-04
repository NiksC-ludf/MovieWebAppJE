using Microsoft.Extensions.DependencyInjection;
using MovieWebApp.Services;
using VueCliMiddleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient<IOmdbService, OmdbService>();
builder.Services.AddTransient<IOmdbService>(sp =>
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var configuration = sp.GetRequiredService<IConfiguration>();
    var client = httpClientFactory.CreateClient();
    var apiKey = configuration["OmdbApiKey"];
    return new OmdbService(client, apiKey);
});
builder.Services.AddControllersWithViews();
builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "clientside/dist";
});
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:7086") // Vue dev server URL
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

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
app.UseCors();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapControllers();
});

app.UseSpa(spa =>
{
    spa.Options.SourcePath = "clientside";
    if(app.Environment.IsDevelopment())
    {
        spa.UseVueCli(npmScript: "serve");
    }
});

app.Run();
