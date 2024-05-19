using Bukhori.Core.Data;
using Bukhori.Core.Repository;
using Bukhori.Core.Repository.Interfaces;
using Bukhori.Services;
using Bukhori.Services.BackGroundServices;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Polling;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var token = builder.Configuration["token"];
builder.Services.AddSingleton(new TelegramBotClient(token));

builder.Services.AddHostedService<BotBackgroundService>();
builder.Services.AddHostedService<ReminderBackgroundService>();



builder.Services.AddDbContext<AppDbContext>(options =>
			options.UseSqlServer(builder.Configuration.
			GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IVideoRepo, VideoRepo>();
builder.Services.AddScoped<IArticleRepo, ArticleRepo>();
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<ICommentRepo, CommentRepo>();


builder.Services.AddSingleton<IUpdateHandler, UpdateHandlerService>();


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
