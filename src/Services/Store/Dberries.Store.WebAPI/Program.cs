using System.Text.Json.Serialization;
using BitzArt.ApiExceptions.AspNetCore;
using Dberries;
using Dberries.Store.Infrastructure;
using Dberries.Store.Persistence;
using Dberries.Store.WebAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistence(builder.Configuration);
builder.AddInfrastructure(typeof(IConsumersAssemblyPointer).Assembly);

builder.Services.AddApiExceptionHandler();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    );

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