using FileManager.Models;
using FileManager.Services;

using System.Security.Cryptography;
using System.Text;

byte[] plaintext = Encoding.UTF8.GetBytes("Hello, World!");
byte[] key = Encoding.UTF8.GetBytes("MySecretKey12356");

using (Aes aes = Aes.Create())
{
    aes.Key = key;
    byte[] ciphertext = aes.EncryptCbc(plaintext, aes.IV);
    byte[] decryptedText = aes.DecryptCbc(ciphertext, aes.IV);

    Console.WriteLine($"Plaintext: {Encoding.UTF8.GetString(plaintext)}");
    Console.WriteLine($"Ciphertext: {BitConverter.ToString(ciphertext).Replace("-", "")}");
    Console.WriteLine($"Decrypted text: {Encoding.UTF8.GetString(decryptedText)}");
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IFileManager, FileManagerService>();

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
