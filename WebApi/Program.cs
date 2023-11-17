using BLLLibrary.IService;
using BLLLibrary.Service;
using DataLibrary.ConnectionProvider;
using DataLibrary.UoW;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConnectionProvider, ConnectionProvider>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUsersService, UsersService>();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
