using Microsoft.EntityFrameworkCore;
using proyecto_final_prog2.Application.Services;
using proyecto_final_prog2.Infrastructure;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDBContext>(options => {
    options.UseSqlite(
    builder.Configuration.GetConnectionString("DefaultConnection")/*, opt => { 
        opt.MigrationsAssembly("proyecto_final_prog2.Api"); 
    }*/);
});
builder.Services.AddScoped<ColumnService>();
builder.Services.AddScoped<TagService>();
builder.Services.AddScoped<TaskService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
           .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowCredentials()
           .WithOrigins("https://localhost:7158")); // Allow only this origin can also have multiple origins seperated with comma
           //.SetIsOriginAllowed(origin => true));// Allow any origin  

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
