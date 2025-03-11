using System.Diagnostics;
using cash.hub.authentication.api.infra.EntityFramework;
using cash.hub.authentication.api.infra.OpenTelemetry;
using cash.hub.authentication.api.infra.Rest;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddInstrumentation(builder.Configuration);
builder.Services.AddEntityFrameworkConfiguration(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseEndpointConfiguration();
app.UseEntityFrameworkConfiguration();

app.Run();