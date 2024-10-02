using CleanArchitecture.Application.Behaviours;
using CleanArchitecture.Application.Services;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Repositories;
using CleanArchitecture.Persistance.Context;
using CleanArchitecture.Persistance.Repositories;
using CleanArchitecture.Persistance.Services;
using CleanArchitecture.WebApi.Middleware;
using FluentValidation;
using GenericRepository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<ICarService, CarService>(); // ICarService'den instance üretmeye çalýstýgýmýzda CarService instance'si türet demiþ oluyoruz.
builder.Services.AddTransient<ExceptionMiddleware>();

builder.Services.AddScoped<IUnitOfWork>(cfr => cfr.GetRequiredService<AppDbContext>());

builder.Services.AddScoped<ICarRepository, CarRepository>();

builder.Services.AddAutoMapper(typeof(CleanArchitecture.Persistance.AssemblyReference).Assembly);

string connectionString = builder.Configuration.GetConnectionString("SqlServer");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddControllers()
    .AddApplicationPart(typeof(
    CleanArchitecture.Presentation.AssemblyReference).Assembly); // mevcut uygulamama, baþka bir katmanda controllerlarýn devam edebileceðini söyledik.   

builder.Services.AddMediatR(cfr => cfr.RegisterServicesFromAssembly(typeof(CleanArchitecture.Application.AssemblyReference).Assembly));

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
builder.Services.AddValidatorsFromAssembly(typeof(CleanArchitecture.Application.AssemblyReference).Assembly);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddlewareExtension();  // kendi exception middleware'imiz

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
