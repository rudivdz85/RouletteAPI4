using RouletteAPI4;
using System.Configuration;
using Dapper;
using System.Data.SQLite;
using System.Text.Json.Serialization;
using RouletteAPI4.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IBetRepository, BetRepository>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    return new BetRepository(connectionString);
});
builder.Services.AddSingleton<RouletteGame>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

ExceptionMiddleware.ConfigureExceptionHandler(app);

app.UseAuthorization();

app.MapControllers();

app.Run();
