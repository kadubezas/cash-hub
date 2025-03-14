using cash.hub.register.api.Infra.DependencyInjection;
using cash.hub.register.api.infra.EntityFramework;
using cash.hub.register.api.Infra.JwtConfig;
using cash.hub.register.api.infra.Middleware;
using cash.hub.register.api.infra.OpenTelemetry;
using cash.hub.register.api.Infra.Rest;
using cash.hub.register.api.Infra.SwaggerConfig;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInstrumentation(builder.Configuration);
builder.Services.AddEntityFrameworkConfiguration(builder.Configuration);
builder.Services.AddJwtConfiguration(builder.Configuration);
builder.Services.AddSwaggerConfiguration();
builder.Services.AddHealthChecks();
builder.Services.AddDomainConfiguration();
builder.Services.AddApplicationConfiguration();

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
app.UseEntityFrameworkConfiguration();

app.Run();