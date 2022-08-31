using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using CrmBox.Application.Services.Customer;
using CrmBox.Core.Domain;
using CrmBox.Core.Domain.Identity;
using CrmBox.Infrastructure.Extensions.ExceptionHandler;
using CrmBox.Infrastructure.Registrations;
using CrmBox.Persistance.Context;
using CrmBox.WebUI.Models;
using crypto;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;
using Serilog;
using System.Data;
using System.Globalization;
using System.Reflection;
using Microsoft.Extensions.Hosting;
using CrmBox.WebUI.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
//Add Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(builder =>
    {
        builder.RegisterModule(new CrmBox.Infrastructure.Registrations.AutofacRegistration());
    });

builder.Services.AddClaimAuthorizationPolicies();


//Fluetn valdation
builder.Services.AddControllers().AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CustomerValidation>()
).ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);


//Add Serilog
Log.Logger = new LoggerConfiguration().CreateLogger();
builder.Host.UseSerilog(((ctx, lc) => lc
    .ReadFrom.Configuration(ctx.Configuration)
    .WriteTo.Console()));

//Add DbContext
builder.Services.AddDbContext<CrmBoxIdentityContext>();
builder.Services.AddDbContext<CrmBoxContext>();
builder.Services.AddDbContext<CrmBoxLogContext>();



//Add Cache
builder.Services.AddMemoryCache();


//Add Localization
builder.Services.AddLocalization(opt => { opt.ResourcesPath = "Resources"; });

builder.Services.AddMvc()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

builder.Services.Configure<RequestLocalizationOptions>(opt =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("tr"),
        new CultureInfo("en")
    };
    opt.DefaultRequestCulture = new RequestCulture("tr");
    opt.SupportedCultures = supportedCultures;
    opt.SupportedUICultures = supportedCultures;
});


// Add SignalR
builder.Services.AddSignalR();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
policy.AllowCredentials()
.AllowAnyHeader()
.AllowAnyMethod()
.SetIsOriginAllowed(x => true)));




//Add Identity
builder.Services.AddIdentity<AppUser, AppRole>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 6;
        options.Password.RequiredUniqueChars = 1;

        // Lockout settings.
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;

        // User settings.
        options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        options.User.RequireUniqueEmail = false;

    })
    .AddEntityFrameworkStores<CrmBoxIdentityContext>()
            .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = new PathString("/Auth/AccessDenied");
});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/Auth/Login";
    options.AccessDeniedPath = "/Auth/AccessDenied";
    options.SlidingExpiration = true;
});


var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandlerMiddleware();
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

}



app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseSession();
app.UseSerilogRequestLogging();
app.UseCors();
app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.UseRequestLocalization(((IApplicationBuilder)app).ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");
app.MapHub<UserHub>("/hubs/userCount");
app.MapHub<ChatHub>("/hubs/chat");
app.Run();

