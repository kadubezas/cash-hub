using cash.hub.register.api.infra.EntityFramework;
using cash.hub.register.api.infra.Middleware;
using cash.hub.register.api.infra.OpenTelemetry;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInstrumentation(builder.Configuration);
builder.Services.AddEntityFrameworkConfiguration(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<TraceMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

app.UseEntityFrameworkConfiguration();

app.Run();