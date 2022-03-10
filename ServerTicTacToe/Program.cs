using ServerTicTacToe.Interfaces;
using ServerTicTacToe.Models;
using ServerTicTacToe.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddEventLog();
    config.AddFile("Logs/TicTacToeServer-{Date}.log");
});
builder.Services.AddSingleton<IFileSystemProvider, FileSystemProvider>();
builder.Services.AddSingleton<JsonHandler>();
builder.Services.AddHostedService<FileHostedService>();
builder.Services.AddSingleton<GameSettings>(x =>
    new GameSettings(
        ArgumentHandler.TryParseInt(
            x.GetRequiredService<IConfiguration>()
            .GetSection("RoundTime")
            .Value),
        ArgumentHandler.TryParseInt(
            x.GetRequiredService<IConfiguration>()
            .GetSection("InactiveTime")
            .Value)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();


