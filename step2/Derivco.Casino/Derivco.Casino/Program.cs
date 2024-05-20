using Derivco.Casino.Repositories.Interfaces;
using Derivco.Casino.Repositories;
using Derivco.Casino.Services.Interfaces;
using Derivco.Casino.Services;
using Derivco.Casino;
using Microsoft.EntityFrameworkCore;
using System;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

if (Environment.OSVersion.Platform == PlatformID.Win32NT)
    builder.Logging.AddEventLog(options =>
    {
        options.SourceName = "Derivco.Casino";
        options.LogName = "Casino";
    });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped(typeof(IAppDBRepository), typeof(AppDBRepository));
builder.Services.AddScoped(typeof(IRouletteService), typeof(RouletteService));


builder.Services.AddTransient<IConfiguration>(sp =>
{
    IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
    configurationBuilder.AddJsonFile("appsettings.json");
    return configurationBuilder.Build();
});

builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CasinoConnectionString")));

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();



    app.UseDeveloperExceptionPage();

    app.UseAuthorization();

    app.UseMiddleware<ErrorHandlerMiddleware>();

    app.MapControllers();

    app.Run();
}
