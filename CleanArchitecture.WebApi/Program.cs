using CleanArchitecture.Persistance.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string connectionString = builder.Configuration.GetConnectionString("SqlServer");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddControllers()
    .AddApplicationPart(typeof(
    CleanArchitecture.Presentation.AssemblyReference).Assembly); // mevcut uygulamama, baþka bir katmanda controllerlarýn devam edebileceðini söyledik.   

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

app.MapControllers();

app.Run();
