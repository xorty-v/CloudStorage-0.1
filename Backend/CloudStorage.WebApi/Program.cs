using CloudStorage.Persistence;
using CloudStorage.Service;
using Minio;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var services = builder.Services;
var configuration = builder.Configuration;

services.AddMinio(options => options
    .WithEndpoint(configuration["MinIO:Endpoint"])
    .WithCredentials(configuration["MinIO:AccessKey"], configuration["MinIO:SecretKey"])
    .WithSSL(false)
    .Build());

services.AddService(configuration);
services.AddPersistence(configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.MapControllers();

app.Run();