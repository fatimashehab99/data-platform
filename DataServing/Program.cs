using DataPipeline;
using DataPipeline.DataAnalysis.Services;
using DataPipeline.DataCollection.Services;
using DataPipeline.Helpers.LocationService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDetection();
builder.Services.AddControllers();
builder.Services.AddCors();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.Configure<DatabaseConnecting>(builder.Configuration.GetSection("PageViewsDatabase"));
builder.Services.AddScoped<IDataCollectionService, DataCollectionService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IDataAnalyticsService, DataAnalyticsService>();
builder.Services.AddScoped<IDashboardStatisticsService, DashboardStatisticsService>();








var app = builder.Build();
app.UseCors(x => x.AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed(origin => true).AllowCredentials());

app.UseDetection();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    // app.UseSwaggerUI();
}
app.UseDefaultFiles();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
