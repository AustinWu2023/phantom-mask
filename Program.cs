using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using phantom_mask.Entities;
using phantom_mask.Services.Seeders;
using phantom_mask.Share.Options;
using phantom_mask.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();

// Add AppDbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 30)),
    mySqlOptions => mySqlOptions.EnableRetryOnFailure()
));

// µù¥U SeedDataOptions and SeedDataImporter
builder.Services.Configure<SeedDataOptions>(builder.Configuration.GetSection("SeedData"));
builder.Services.AddScoped<SeedDataImporter>();

builder.Services.AddScoped<IOpeningHourParser, OpeningHourParser>();

var app = builder.Build();

// °õ¦æ SeedDataImporter
using (var scope = app.Services.CreateScope()) {

    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    var seeder = scope.ServiceProvider.GetRequiredService<SeedDataImporter>();
    await seeder.ImportIfEmptyAsync();
}


app.UseFastEndpoints();
app.UseOpenApi();
app.UseSwaggerUi();

app.UseHttpsRedirection();

app.Run();

