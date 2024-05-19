using Bukhori.Core.Dto;
using Bukhori.Core.Repository.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bukhori.Services
{

	public partial class UpdateHandlerService : IUpdateHandler
	{
		private IVideoRepo _videoRepo;
		private ICommentRepo _commentRepo;
		private IArticleRepo _articleRepo;
		private IUserRepo _userRepo;
		private readonly IServiceScopeFactory _scopeFactory;
		private readonly string _apikey = "AIzaSyCL7Y8o3kgyU3w3I5mi6cSHsL0rTntCBIk";

		public UpdateHandlerService(IServiceScopeFactory scopeFactory)
			=> _scopeFactory = scopeFactory;

		public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
		{
			#region scope orqali servicelarni olib ber
			using var scope = _scopeFactory.CreateAsyncScope();
			_videoRepo = scope.ServiceProvider.GetService<IVideoRepo>();
			_commentRepo = scope.ServiceProvider.GetService<ICommentRepo>();
			_articleRepo = scope.ServiceProvider.GetService<IArticleRepo>();
			_userRepo = scope.ServiceProvider.GetService<IUserRepo>();
			#endregion

		

				var updateHandler = update.Type switch
				{
					UpdateType.Message => HandleMessageAsync(botClient, update, cancellationToken),
					_ => HandleUnknownUpdateAsync(botClient, update, cancellationToken),
				};
				
				
				try
				{
					await updateHandler;
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			
		}

		public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
			=> throw new NotImplementedException();

		private Task HandleUnknownUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
			=> throw new NotImplementedException();




		private async Task HandleMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
		{
			var message = update.Message;

			var messageHandler = message.Type switch
			{
				MessageType.Text => HandleTextMessageAsync(botClient, update, cancellationToken),
				_ => HandleUnknownMessageAsync(botClient, update, cancellationToken),
			};

			try
			{
				await messageHandler;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		private async ValueTask HandleUnknownMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
			=> await SendTextMessageAsync("error", botClient, update, cancellationToken);

		private async ValueTask HandleTextMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
		{
			var text = update.Message.Text;
			var command = string.Empty;

			try
			{
				if (text.StartsWith("/start"))
				{
					await StartCommandAsync(botClient, update, cancellationToken);
				}
				else if (text.StartsWith("/videos"))
				{
					await VideosCommandAsync(botClient, update, cancellationToken);
				}

				else if (text.StartsWith("/articles"))
				{
					await ArticlesCommandAsync(botClient, update, cancellationToken);
				}
				else if (text.StartsWith("/comments"))
				{
					await CommantsCommandAsync(botClient, update, cancellationToken);
				}
				else if (text.StartsWith("/addvideo"))
				{
					await AddVideoCommandCommandAsync(botClient, update, cancellationToken);
				}
				else if (text.StartsWith("/editvideo"))
				{
					await EditVideoCommandCommandAsync(botClient, update, cancellationToken);
				}
				else if (text.StartsWith("/deletevideo"))
				{
					await DeleteVideoCommandCommandAsync(botClient, update, cancellationToken);
				}
				else if (text == "Add Video")
				{
					await AddVideoCommandAsync(botClient, update, cancellationToken);
				}
				else if (text == "Edit Video")
				{
					await EditVideoCommandAsync(botClient, update, cancellationToken);
				}
				else if (text == "Delete Video")
				{
					await DeleteVideoCommandAsync(botClient, update, cancellationToken);
				}
				else if (text == "Get Video")
				{
					await GetVideoCommandAsync(botClient, update, cancellationToken);
				}
			}

					catch (Exception ex)
			{
				await SendTextMessageAsync("Commandlarni to`g`ri kiriting !", botClient, update, cancellationToken);
			}
		}

		private async ValueTask UnknownCommandAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
			=> await SendTextMessageAsync("Unknown Command", botClient, update, cancellationToken);

		/*public async ValueTask AddCommandAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
		{
			await botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
			var query =  update.CallbackQuery.Data.Split("##").ToList();
			if (query[1].Length >0 && query[2].Length >0)
			{
				bool result = await _channelrepo.AddChannel(query[1], query[2]);
				bool result1 = await _channelrepo.AddUserChannel(query[1], query[3]);

				if(result && result1)
				await botClient.SendTextMessageAsync(
				chatId: update.CallbackQuery.From.Id,
				text: "Ushbu kanal shaxsiy obunalaringizga qo`shildi .",
				cancellationToken: cancellationToken
				);
				else
				{
					await botClient.SendTextMessageAsync(
					chatId: update.CallbackQuery.From.Id,
					text: "Obunalarga qo`shishda xatolik yuz berdi . Qaytadan  urinib ko`ring .",

					cancellationToken: cancellationToken
					);

				}
				
			}
			else
			{

				await botClient.SendTextMessageAsync(
				chatId: update.CallbackQuery.From.Id,
				text: "Obunalarga qo`shishda xatolik yuz berdi . Qaytadan  urinib ko`ring .",
				
				cancellationToken: cancellationToken
				); ; ;
			}

			


		}
*/
		private async ValueTask VideosCommandAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
		{
			ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
			{
				new KeyboardButton[] { "Add Video", "Edit Video" },
				new KeyboardButton[] { "Delete Video", "Get Video" },
			})
			{
				ResizeKeyboard = true
			};

			Message sentMessage = await botClient.SendTextMessageAsync(
				chatId: update.Message.Chat.Id,
				text: "Choose a response",
				replyMarkup: replyKeyboardMarkup,
				cancellationToken: cancellationToken);


		}
		private async ValueTask AddVideoCommandCommandAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
		{
			var s = "";
			try
			{
				var text = update.Message.Text;
				text = text.Replace("\\addvideo", "");
				var l = text.Split("#");
				var user = await _userRepo.GetUserByTelegramId(update.Message.From.Id.ToString());
				var newV = new VideoDto()
				{
					Name = l[0].Trim().ToString(),
					VideoUrl = l[1].Trim().ToString(),
					UserId = user.Id,
				};
				bool res =await _videoRepo.AddVideo(newV);
				
				if (res)
				{
					await botClient.SendStickerAsync(
						chatId: update.Message.Chat.Id,
						sticker: InputFile.FromUri("https://media.stickerswiki.app/gorobot/6173676.160.webp"),
						cancellationToken: cancellationToken);
						s = "Video Bazaga Muvofaqqiyatli qo`shildi !";
				}
				else
				{
					s = "Video qo`shishda xatolik bor .";
					await botClient.SendStickerAsync(
						chatId: update.Message.Chat.Id,
						sticker: InputFile.FromUri("https://media.stickerswiki.app/gorobot/6173693.160.webp"),
						cancellationToken: cancellationToken);
				}
			}
			catch
			{
				await botClient.SendStickerAsync(
					chatId: update.Message.Chat.Id,
					sticker: InputFile.FromUri("https://media.stickerswiki.app/gorobot/6173693.160.webp"),
					cancellationToken: cancellationToken);
				s = "Video qo`shishda xatolik bor .";
			}

			ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
			{
				new KeyboardButton[] { "Add Video", "Edit Video" },
				new KeyboardButton[] { "Delete Video", "Get Video" },

			})
			{
				ResizeKeyboard = true
			};



			Message sentMessage = await botClient.SendTextMessageAsync(
				chatId: update.Message.Chat.Id,
				text: s,
				replyMarkup: replyKeyboardMarkup,
				cancellationToken: cancellationToken);


		}
		private async ValueTask AddVideoCommandAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
		{
			ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
			{
				new KeyboardButton[] { "Add Video", "Edit Video" },
				new KeyboardButton[] { "Delete Video", "Get Video" },
			})
			{
				ResizeKeyboard = true
			};

			Message sentMessage = await botClient.SendTextMessageAsync(
				chatId: update.Message.Chat.Id,
				text: "Video nomi va linkini # bilan ajratib\n\\addvideo commandidan keyin kiriting kiriting :\nMasalan:" +
				"\\addvideo Nomi#Url",
				replyMarkup: replyKeyboardMarkup,
				cancellationToken: cancellationToken);


		}



		private async ValueTask EditVideoCommandCommandAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
		{
			var s = "";
			try
			{
				var text = update.Message.Text;
				text = text.Replace("/editvideo", "");
				var stt = text.Split("#");
				var user = await _userRepo.GetUserByTelegramId(update.Message.From.Id.ToString());
				var video = await _videoRepo.GetVideoById(int.Parse(stt[0].Trim()));
				video.VideoUrl = stt[2];
				video.Name = stt[1];
			
				bool res = await _videoRepo.UpdateVideo(video);

				if (res)
				{
					await botClient.SendStickerAsync(
						chatId: update.Message.Chat.Id,
						sticker: InputFile.FromUri("https://media.stickerswiki.app/gorobot/6173676.160.webp"),
						cancellationToken: cancellationToken);
					s = "Video O`zgartirildi !";
				}
				else
				{
					s = "Video o`zgartirishda xatolik bor .";
					await botClient.SendStickerAsync(
						chatId: update.Message.Chat.Id,
						sticker: InputFile.FromUri("https://media.stickerswiki.app/gorobot/6173693.160.webp"),
						cancellationToken: cancellationToken);
				}
			}
			catch
			{
				await botClient.SendStickerAsync(
					chatId: update.Message.Chat.Id,
					sticker: InputFile.FromUri("https://media.stickerswiki.app/gorobot/6173693.160.webp"),
					cancellationToken: cancellationToken);
				s = "Video o`zgartirishda xatolik bor .";
			}

			ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
			{
				new KeyboardButton[] { "Add Video", "Edit Video" },
				new KeyboardButton[] { "Delete Video", "Get Video" },

			})
			{
				ResizeKeyboard = true
			};



			Message sentMessage = await botClient.SendTextMessageAsync(
				chatId: update.Message.Chat.Id,
				text: s,
				replyMarkup: replyKeyboardMarkup,
				cancellationToken: cancellationToken);


		}
		private async ValueTask EditVideoCommandAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
		{
			ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
			{
				new KeyboardButton[] { "Add Video", "Edit Video" },
				new KeyboardButton[] { "Delete Video", "Get Video" },
			})
			{
				ResizeKeyboard = true
			};
			var videos = await _videoRepo.GetAllVideos();
			var user = await _userRepo.GetUserByTelegramId(update.Message.From.Id.ToString());

			var userVideos = videos.Where(x => x.UserId == user.Id).ToList();
			var st = "Siz joylagan videolaringiz \n";
			var ii = 0;
			foreach(var i in userVideos)
			{
				st += i.Id + " . " + i.Name + "\n"+i.VideoUrl+"\n";
			}
			Message sentMessage = await botClient.SendTextMessageAsync(
				chatId: update.Message.Chat.Id,
				text: st,
				replyMarkup: replyKeyboardMarkup,
				cancellationToken: cancellationToken);

			Message sentMessage1 = await botClient.SendTextMessageAsync(
				chatId: update.Message.Chat.Id,
				text: "Postni o`zgartirish uchun \n/editvideo commandidan keyin  # bilan video idsini , yangi nomini ,yangi linkini   kiriting .\n" +
				"Masalan:/editvideo Id#Name#Url",
				replyMarkup: replyKeyboardMarkup,
				cancellationToken: cancellationToken);


		}



		private async ValueTask DeleteVideoCommandCommandAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
		{
			var s = "";
			try
			{
				var text = update.Message.Text;
				text = text.Replace("/deletevideo", "");
				
				
				bool res = await _videoRepo.DeleteVideo(int.Parse(text.Trim()));

				if (res)
				{
					await botClient.SendStickerAsync(
						chatId: update.Message.Chat.Id,
						sticker: InputFile.FromUri("https://media.stickerswiki.app/gorobot/6173676.160.webp"),
						cancellationToken: cancellationToken);
					s = "Video O`chirildi !";
				}
				else
				{
					s = "Video o`chirishda xatolik bor .";
					await botClient.SendStickerAsync(
						chatId: update.Message.Chat.Id,
						sticker: InputFile.FromUri("https://media.stickerswiki.app/gorobot/6173693.160.webp"),
						cancellationToken: cancellationToken);
				}
			}
			catch
			{
				await botClient.SendStickerAsync(
					chatId: update.Message.Chat.Id,
					sticker: InputFile.FromUri("https://media.stickerswiki.app/gorobot/6173693.160.webp"),
					cancellationToken: cancellationToken);
				s = "Video o`chirishda xatolik bor .";
			}

			ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
			{
				new KeyboardButton[] { "Add Video", "Edit Video" },
				new KeyboardButton[] { "Delete Video", "Get Video" },

			})
			{
				ResizeKeyboard = true
			};



			Message sentMessage = await botClient.SendTextMessageAsync(
				chatId: update.Message.Chat.Id,
				text: s,
				replyMarkup: replyKeyboardMarkup,
				cancellationToken: cancellationToken);


		}
		private async ValueTask DeleteVideoCommandAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
		{
			ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
			{
				new KeyboardButton[] { "Add Video", "Edit Video" },
				new KeyboardButton[] { "Delete Video", "Get Video" },
			})
			{
				ResizeKeyboard = true
			};
			var videos = await _videoRepo.GetAllVideos();
			var user = await _userRepo.GetUserByTelegramId(update.Message.From.Id.ToString());

			var userVideos = videos.Where(x => x.UserId == user.Id).ToList();
			var st = "Siz joylagan videolaringiz \n";
			var ii = 0;
			foreach (var i in userVideos)
			{
				st += i.Id + " . " + i.Name + "\n" + i.VideoUrl + "\n";
			}
			Message sentMessage = await botClient.SendTextMessageAsync(
				chatId: update.Message.Chat.Id,
				text: st,
				replyMarkup: replyKeyboardMarkup,
				cancellationToken: cancellationToken);

			Message sentMessage1 = await botClient.SendTextMessageAsync(
				chatId: update.Message.Chat.Id,
				text: "Postni uchirish uchun \n/deletevideo commandidan keyin  video idsini kiriting .\n" +
				"Masalan:/deletevideo Id",
				replyMarkup: replyKeyboardMarkup,
				cancellationToken: cancellationToken);


		}

		private async ValueTask GetVideoCommandAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
		{
			ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
			{
				new KeyboardButton[] { "Add Video", "Edit Video" },
				new KeyboardButton[] { "Delete Video", "Get Video" },
			})
			{
				ResizeKeyboard = true
			};
			var videos = await _videoRepo.GetAllVideos();
			var user = await _userRepo.GetUserByTelegramId(update.Message.From.Id.ToString());

			var st = "Joylagan videolar : \n";
			var ii = 0;
			foreach (var i in videos)
			{
				st += i.Id + " . " + i.Name + "\n" + i.VideoUrl + "\n";
			}
			Message sentMessage = await botClient.SendTextMessageAsync(
				chatId: update.Message.Chat.Id,
				text: st,
				replyMarkup: replyKeyboardMarkup,
				cancellationToken: cancellationToken);

			Message sentMessage1 = await botClient.SendTextMessageAsync(
				chatId: update.Message.Chat.Id,
				text: "Postni uchirish uchun \n/deletevideo commandidan keyin  video idsini kiriting .\n" +
				"Masalan:/deletevideo Id",
				replyMarkup: replyKeyboardMarkup,
				cancellationToken: cancellationToken);


		}




		private async ValueTask ArticlesCommandAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
		{
			

			await SendTextMessageAsync("", botClient, update, cancellationToken);

		}

		private async ValueTask CommantsCommandAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
		{


			await SendTextMessageAsync("", botClient, update, cancellationToken);

		}


		private async ValueTask SendTextMessageAsync(string text, ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
		{
			await botClient.SendTextMessageAsync(
				chatId: update.Message.Chat.Id,
				text: text,
				parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,

				cancellationToken: cancellationToken
				); 
		}



		private async ValueTask StartCommandAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
		{
			var tgUser = update.Message.From;
			var newu = new UserDto()
			{
				FirstName = tgUser.FirstName,
				LastName = tgUser.LastName,
				TelegramId = tgUser.Id.ToString(),
			};
			await _userRepo.AddUser(newu);

			await SendTextMessageAsync($"{tgUser.FirstName} Botimizga Xush kelibsiz . " +
				$"\n/videos - Videolarni ko`rish" +
				$"\n/article - Maqolalarni ko`rish" +
				$"\n/comments - Commentlarni ko`rish", botClient, update, cancellationToken);

			await botClient.SendStickerAsync(
				chatId: update.Message.Chat.Id,
				sticker: InputFile.FromUri("https://media.stickerswiki.app/gorobot/6173692.512.webp"),
				cancellationToken: cancellationToken);

		}

	}
}
