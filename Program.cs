using Microsoft.EntityFrameworkCore;
using RestaurantApi.Models;
using RestaurantFromScratch.Data;
using RestaurantFromScratch.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<RestaurantContext>(options =>
    options.UseSqlite("Data Source=restaurant.db"));
builder.Services.AddScoped<OpeningHoursService>();
builder.Services.AddScoped<TableService>();
builder.Services.AddScoped<ReservationAvailabilityCalculator>();
builder.Services.AddScoped<ReservationService>();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
        .GetRequiredService<RestaurantContext>();

    // add seed

    if (!context.OpeningHours.Any())
    {
        context.OpeningHours.AddRange(
            new OpeningHour
            {
                DayOfWeek = DayOfWeek.Monday,
                OpenTime = new TimeOnly(12, 0),
                CloseTime = new TimeOnly(22, 0),
                IsClosed = false
            },
            new OpeningHour
            {
                DayOfWeek = DayOfWeek.Tuesday,
                OpenTime = new TimeOnly(12, 0),
                CloseTime = new TimeOnly(22, 0),
                IsClosed = false
            },
            new OpeningHour
            {
                DayOfWeek = DayOfWeek.Wednesday,
                OpenTime = new TimeOnly(12, 0),
                CloseTime = new TimeOnly(22, 0),
                IsClosed = false
            },
            new OpeningHour
            {
                DayOfWeek = DayOfWeek.Thursday,
                OpenTime = new TimeOnly(12, 0),
                CloseTime = new TimeOnly(22, 0),
                IsClosed = false
            },
            new OpeningHour
            {
                DayOfWeek = DayOfWeek.Friday,
                OpenTime = new TimeOnly(12, 0),
                CloseTime = new TimeOnly(23, 0),
                IsClosed = false
            },
            new OpeningHour
            {
                DayOfWeek = DayOfWeek.Saturday,
                OpenTime = new TimeOnly(12, 0),
                CloseTime = new TimeOnly(23, 0),
                IsClosed = false
            },
            new OpeningHour
            {
                DayOfWeek = DayOfWeek.Sunday,
                OpenTime = null,
                CloseTime = null,
                IsClosed = true
            }
        );

        context.SaveChanges();
    }
}

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

