using Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Veritabaný baðlantýsýný yapýlandýrma
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS ayarý yapýlýyor, istek atacak porta izin veriliyor
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy",
        builder =>
        {
            builder.WithOrigins("https://localhost:7278", "https://localhost:another-origin.com")
       .AllowAnyHeader()
       .AllowAnyMethod();
        });
});

var app = builder.Build();

// CORS politikasýný kullan
app.UseCors("ReactPolicy");

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
