using Api.Extensions;
using FastEndpoints;

using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));
builder.Services.AddCommonServices(builder.Configuration);

// Add MVC services
builder.Services.AddControllers();
builder.Services.AddCustomCors();
// Register modules
builder.Services.AddCatalogModule(builder.Configuration);
builder.Services.AddOrdersModule(builder.Configuration);
builder.Services.AddAccountingModule(builder.Configuration);
builder.Services.AddShippingModule(builder.Configuration);
builder.Services.AddNotificationsModule(builder.Configuration);
builder.Services.AddCustomersModule(builder.Configuration);
builder.Services.AddUsersModule(builder.Configuration);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();


var app = builder.Build();


//app.UseHttpsRedirection();
app.UseCustomCors();
app.UseSerilogRequestLogging();



// Use modules
app.UseCatalogModule();
app.UseOrdersModule();
app.UseAccountingModule();
app.UseShippingModule();
app.UseNotificationsModule();
app.UseCustomersModule();
app.UseUsersModule();

app.UseExceptionHandler(options => { });

// Add authentication & authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseFastEndpoints(cfg =>
{
  cfg.Endpoints.RoutePrefix = "api";
  cfg.Endpoints.ShortNames = true;
});

app.Run();
