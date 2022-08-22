using Autofac;
using Autofac.Extensions.DependencyInjection;
using CrmBox.Core.Domain.Identity;
using CrmBox.Infrastructure.Registrations;
using CrmBox.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Add Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(builder =>
    {
        builder.RegisterModule(new CrmBox.Infrastructure.Registrations.AutofacRegistration());

    });

//Add Serilog
Log.Logger = new LoggerConfiguration().CreateLogger();
builder.Host.UseSerilog(((ctx, lc) => lc
    .ReadFrom.Configuration(ctx.Configuration)
    .WriteTo.Console()));


builder.Services.AddControllers();
builder.Services.AddApplicationServices();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentity<AppUser, AppRole>(x =>
{
    x.Password.RequireUppercase = false;
    x.Password.RequireNonAlphanumeric = false;
    x.Password.RequireDigit = false;
    x.Password.RequireLowercase = false;
    x.Password.RequiredLength = 4;
})
    .AddEntityFrameworkStores<CrmBoxIdentityContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.AccessDeniedPath = "/Auth/AccessDenied";
});

//Add Database and Context
builder.Services.AddDbContext<CrmBoxContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Local"));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.UseAuthorization();

app.MapControllers();

app.Run();