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
using CrmBox.Application.Interfaces.Chat;
using Microsoft.Extensions.Hosting;
using CrmBox.WebUI.Hubs;
using Microsoft.AspNetCore.Http.Features;
using CrmBox.WebUI.Helper.Twilio;
using CrmBox.WebUI.Helper;
using CrmBox.Application.Services.Chat;

var builder = WebApplication.CreateBuilder(args);
var provider = builder.Services.BuildServiceProvider();
var configuration = provider.GetRequiredService<IConfiguration>();

builder.Services.Configure<TwilioSettings>(configuration.GetSection("Twilio"));
builder.Services.Configure<EmailHelper>(configuration.GetSection("EmailSender"));



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

builder.Services.Configure<FormOptions>(x => x.ValueCountLimit = 1000000);

builder.Services.AddSingleton<IChatRoomService,
InMemoryChatRoomService>();

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
        options.Password.RequiredLength = 5;
        //options.Password.RequiredUniqueChars = 1;
        


        // Lockout settings.
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;

        // User settings.
        //options.User.AllowedUserNameCharacters =
        //"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        options.User.RequireUniqueEmail = true;

    })
    .AddEntityFrameworkStores<CrmBoxIdentityContext>()
            .AddDefaultTokenProviders().Services.AddMvc();

//Policy Rules
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("GetAllUsers",
        policy => policy.RequireClaim("Get All Users"));
    options.AddPolicy("ManageUserClaims",
        policy => policy.RequireClaim("Manage User Claims"));

    options.AddPolicy("AddUser",
    policy => policy.RequireClaim("Add User"));

    options.AddPolicy("UpdateUser",
    policy => policy.RequireClaim("Update User"));

    options.AddPolicy("DeleteUser",
    policy => policy.RequireClaim("Delete User"));

    options.AddPolicy("GetAllUserRoles",
    policy => policy.RequireClaim("Get All User Roles"));

    options.AddPolicy("AddUserRole",
    policy => policy.RequireClaim("Add User Role"));

    options.AddPolicy("UpdateUserRole",
    policy => policy.RequireClaim("Update User Role"));

    options.AddPolicy("DeleteUserRole",
    policy => policy.RequireClaim("Delete User Role"));

    options.AddPolicy("ChooseUserRole",
policy => policy.RequireClaim("Choose User Role"));

    options.AddPolicy("ManageUserClaims",
policy => policy.RequireClaim("Manage User Claims"));

    options.AddPolicy("GetAllCustomers",
policy => policy.RequireClaim("Get All Customers"));

    options.AddPolicy("AddCustomer",
policy => policy.RequireClaim("Add Customer"));


    options.AddPolicy("UpdateCustomer",
policy => policy.RequireClaim("Update Customer"));


    options.AddPolicy("DeleteCustomer",
policy => policy.RequireClaim("Delete Customer"));



    options.AddPolicy("SendSms",
policy => policy.RequireClaim("Send Sms"));

    options.AddPolicy("CustomerChatSupport",
        policy => policy.RequireClaim("Customer Chat Support"));

});

//Policy Rules


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

app.MapHub<ChatHub>("/chatHub");
app.MapHub<AgentHub>("/agentHub");
app.Run();

