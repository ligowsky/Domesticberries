using BitzArt.ApiExceptions.AspNetCore;
using Dberries;
using Dberries.Store.Persistence;
using Dberries.Store.Infrastructure;
using Dberries.Store.WebAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration, typeof(TConsumersAssemblyPointer).Assembly);

builder.Services.AddApiExceptionHandler();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddTelemetry();
builder.AddElasticLogging();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseApiExceptionHandler();

app.MapControllers();

app.Run();