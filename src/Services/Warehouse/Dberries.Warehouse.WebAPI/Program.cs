using BitzArt.ApiExceptions.AspNetCore;
using Dberries;
using Dberries.Extensions;
using Dberries.Warehouse.Infrastructure;
using Dberries.Warehouse.Persistence;
using Dberries.Warehouse.Presentation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddInfrastructure();
builder.Services.AddMessaging(builder.Configuration);
builder.Services.AddPresentation();

builder.Services.AddApiExceptionHandler();

builder.Services.AddXApiKeyAuthorization(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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