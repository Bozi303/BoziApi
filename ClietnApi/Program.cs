using ClietnApi.Services;
using Infrastructure.DataAccess.ElasticSearch;
using Infrastructure.DataAccess.MySql;
using Infrastructure.DataAccess.Redis;
using Infrastructure.Services.AuthService;
using Infrastructure.Services.BoziService;
using Infrastructure.Services.SmsService;
using Infrastructure.Services.SmsService.Model;
using Infrastructure.Services.SmsService.SmsServiceImplementation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SharedModel.BoziService;
using SixLaborsCaptcha.Mvc.Core;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


//Jwt configuration starts here
var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();

var fileManager = new FileManagerClient("https://localhost:7182", "/api/filemanager/addFile", "/api/filemanager/getFile");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
 .AddJwtBearer(options =>
 {
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         ValidIssuer = jwtIssuer,
         RequireExpirationTime = true,
         LogValidationExceptions = true,
         ValidAudience = jwtIssuer,
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
     };
 });
//Jwt configuration ends here

// configure my sql db
ConfigureMySql();

ConfigureRedis();

ConfigureSms();

builder.Services.AddElasticSearch(builder.Configuration);
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddSingleton(fileManager);
builder.Services.AddSingleton<IBoziService, BoziService>();

builder.Services.AddSixLabCaptcha(x =>
{
    x.DrawLines = 1;
    x.NoiseRate = 0;
});

builder.Services.AddControllers();
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors(c =>
{
    c.AllowAnyHeader();
    c.AllowAnyMethod();
    c.AllowAnyOrigin();
});

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

void ConfigureSms()
{
    var lineNumber = long.Parse(builder.Configuration.GetSection("SmsProvider:SmsIR:lineNumber").Value ?? "0");
    var token = builder.Configuration.GetSection("SmsProvider:SmsIR:token").Value;
    var sendApiUrl = builder.Configuration.GetSection("SmsProvider:SmsIR:sendApi").Value;
    var smsImplementation = new SmsIRImplementation(sendApiUrl, lineNumber, token);

    builder.Services.AddSingleton<ISmsService>(smsImplementation);

    builder.Services.AddScoped<TemplateService>();
}