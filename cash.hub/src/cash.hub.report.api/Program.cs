using cash.hub.report.api.Infra.DependencyInjection;
using cash.hub.report.api.infra.EntityFramework;
using cash.hub.report.api.Infra.RedisConfig;
using cash.hub.report.api.Infra.Rest;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRedisConfiguration(builder.Configuration);
builder.Services.AddHealthChecks();
builder.Services.AddDomainConfiguration();
builder.Services.AddApplicationConfiguration();
builder.Services.AddEntityFrameworkConfiguration(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseEndpointConfiguration();

app.Run();

