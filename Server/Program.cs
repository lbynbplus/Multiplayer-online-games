using Server.Domain;
using Server.Hubs;
using Server.Models;
using Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
//添加自定义服务
builder.Services.AddSingleton<IPlayerService, PlayerService>();
builder.Services.AddTransient<ILadder, Ladder>();
builder.Services.AddTransient<ISnake, Snake>();
builder.Services.AddTransient<ICard, Card>();
builder.Services.AddTransient<IDiceRoller, DiceRoller>();
builder.Services.AddTransient<IGameFactory, GameFactory>();
builder.Services.AddTransient<IGameService, GameService>();
builder.Services.Configure<GameConfig>(builder.Configuration.GetSection("GameConfig"));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapHub<ChatHub>("/ChatHub");

app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}