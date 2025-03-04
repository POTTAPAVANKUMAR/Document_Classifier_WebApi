using Document_Classifier_WebApi.Manager.Interface;
using Document_Classifier_WebApi.Manager;
using Document_Classifier_WebApi.Service.Interface;
using Document_Classifier_WebApi.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register services
builder.Services.AddScoped<IOcrService, OcrService>();
builder.Services.AddScoped<IPredictionService, PredictionService>();

// Register managers
builder.Services.AddScoped<IOcrManager, OcrManager>();
builder.Services.AddScoped<IPredictManager, PredictManager>();

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
