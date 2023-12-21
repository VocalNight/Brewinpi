using BreweryApi.Models;
using BreweryApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<BreweryContext>(opt =>
opt.UseSqlServer(builder.Configuration.GetConnectionString("BreweryDatabase")));// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<BreweryService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev", builder =>
    {
        builder.WithOrigins("http://localhost:4200") // Update with your Angular app's URL
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = scope.ServiceProvider.GetRequiredService<BreweryContext>();
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();
}

app.UseCors("AllowAngularDev");
app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();
