using api.src.data;
using api.src.interfaces;
using api.src.repository;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

Env.Load();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<iUserRepository, UserRepository>();


string connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") ?? "Data Source-app.db";
builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlite(connectionString));

var app = builder.Build();

using ( var scope = app.Services.CreateScope()){

    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    DataSeeders.Initialize(services);

}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

