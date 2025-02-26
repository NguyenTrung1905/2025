using _2025.Services.AuthAPI;
using _2025.Services.AuthAPI.Core;
using _2025.Services.AuthAPI.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

AppContext.SetSwitch("Npgsql.EnablelagacyTimestampBehavior", true); 

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddDbContextPool<IdentityContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Identity"));
});

// Add services to the container.
builder.Services.AddSingleton<AppSettings>();
builder.Services.AddScoped<IHelloService, HelloService>();

builder.Services.AddControllers();
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
