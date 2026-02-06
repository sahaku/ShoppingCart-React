using Microsoft.EntityFrameworkCore;
using ShoppingCart_Api.Api;
using ShoppingCart_Api.DataLayer;
using ShoppingCart_Api.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ShoppingContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ShoppingContext")));
builder.Services.AddTransient<ProductService>();
builder.Services.AddTransient<ApiService>();
builder.Services.AddHttpClient();

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
       options.AddPolicy("AllowAll",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });

});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAll");

app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShoppingCart v1"); } );
app.MapControllers();

app.Run();
