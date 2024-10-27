using CleanArchitecture.WebApi.Middleware;
using CleanArchitecture.WebApi.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.InstallServices(builder.Configuration, typeof(IServiceInstaller).Assembly);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseMiddlewareExtension();  // kendi exception middleware'imiz

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
