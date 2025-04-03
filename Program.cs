using Document_Classifier_WebApi.Manager.Interface;
using Document_Classifier_WebApi.Manager;
using Document_Classifier_WebApi.Service.Interface;
using Document_Classifier_WebApi.Service;
using Hangfire;
using Hangfire.InMemory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register services
builder.Services.AddScoped<IOcrService, OcrService>();
builder.Services.AddScoped<IPredictionService, PredictionService>();
builder.Services.AddScoped<IBackgroundProcessService, BackgroundProcessService>(); // Register BackgroundProcessService

// Register managers
builder.Services.AddScoped<IOcrManager, OcrManager>();
builder.Services.AddScoped<IPredictManager, PredictManager>();

// Add Hangfire services.
builder.Services.AddHangfire(config =>
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
          .UseSimpleAssemblyNameTypeSerializer()
          .UseDefaultTypeSerializer()
          .UseInMemoryStorage()); // Updated to use InMemoryStorage
builder.Services.AddHangfireServer();

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

// Configure Hangfire dashboard.
app.UseHangfireDashboard();

// Schedule the background process to run every 3 minutes.
RecurringJob.AddOrUpdate<IBackgroundProcessService>("ExecuteBackgroundProcess", service => service.ExecuteAsync(), "*/1 * * * *"); // Runs every 3 minutes

app.Run();

