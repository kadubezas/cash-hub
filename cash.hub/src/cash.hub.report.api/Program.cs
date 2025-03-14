using cash.hub.report.api.Infra.DependencyInjection;
using cash.hub.report.api.infra.EntityFramework;
using cash.hub.report.api.Infra.JwtConfig;
using cash.hub.report.api.infra.Middleware;
using cash.hub.report.api.Infra.RedisConfig;
using cash.hub.report.api.Infra.Rest;
using cash.hub.report.api.Infra.SwaggerConfig;

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
builder.Services.AddSwaggerConfiguration();
builder.Services.AddJwtConfiguration(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization(); 

app.UseMiddleware<TraceMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

app.UseEndpointConfiguration();

app.Run();

