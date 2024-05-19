using Bukhori.Core.Repository.Interfaces;
using Telegram.Bot;

namespace Bukhori.Services.BackGroundServices
{

	public class ReminderBackgroundService : BackgroundService
	{
		private TelegramBotClient _botClient;

		public ReminderBackgroundService(TelegramBotClient botClient, IServiceScopeFactory serviceScope)
		{
			_botClient = botClient;
			_scopeFactory = serviceScope;
		}
		private IVideoRepo _videoRepo;
		private ICommentRepo _commentRepo;
		private IArticleRepo _articleRepo;
		private IUserRepo _userRepo;
		private readonly IServiceScopeFactory _scopeFactory;
		private readonly string _apikey = "AIzaSyCL7Y8o3kgyU3w3I5mi6cSHsL0rTntCBIk";


		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			#region scope orqali servicelarni olib beradi
			using var scope = _scopeFactory.CreateAsyncScope();
			_videoRepo = scope.ServiceProvider.GetService<IVideoRepo>();
			_commentRepo = scope.ServiceProvider.GetService<ICommentRepo>();
			_articleRepo = scope.ServiceProvider.GetService<IArticleRepo>();
			_userRepo = scope.ServiceProvider.GetService<IUserRepo>();
			#endregion

			
		}

		private async ValueTask SendTextToUsers(string text, long chatId, ITelegramBotClient botClient, CancellationToken cancellationToken)
		{
			await _botClient.SendTextMessageAsync(
				chatId: chatId,
				text: text,
				cancellationToken: cancellationToken);
		}
	}

}
