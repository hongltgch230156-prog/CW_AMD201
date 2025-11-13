using Microsoft.EntityFrameworkCore;
using CrudCoursework.Data;
using CrudCoursework.CQRS.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database connection
builder.Services.AddDbContext<UrlDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register CQRS Handlers
builder.Services.AddScoped<GetAllUrlsHandler>();
builder.Services.AddScoped<GetUrlByIdHandler>();
builder.Services.AddScoped<CreateUrlHandler>();
builder.Services.AddScoped<UpdateUrlHandler>();
builder.Services.AddScoped<DeleteUrlHandler>();
builder.Services.AddScoped<DeleteAllUrlsHandler>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
