using Infrastructure.DataAccess.MySql;
using Infrastructure.DataAccess.Redis;
using Infrastructure.Services.BoziService;
using SharedModel.BoziService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

ConfigureMySql();

ConfigureRedis();

builder.Services.AddSingleton<IBoziService, BoziService>();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void ConfigureMySql()
{
    var connectionString = builder.Configuration.GetSection("MySql:ConnectionString").Value;
    if (connectionString != null)
    {
        var mySql = new MySqlDataContext(connectionString);
        builder.Services.AddSingleton(mySql);
    }
}

void ConfigureRedis()
{
    var connectionString = builder.Configuration.GetSection("Redis:ConnectionString").Value;
    if (connectionString != null)
    {
        var redis = new RedisDataContext(connectionString);
        builder.Services.AddSingleton(redis);
    }
}
