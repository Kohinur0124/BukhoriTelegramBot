using Bukhori.Core.Data;
using Bukhori.Core.Dto;
using Bukhori.Core.Entities;
using Bukhori.Core.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bukhori.Core.Repository
{
	public class UserRepo : IUserRepo
	{
		private AppDbContext _context;
		public UserRepo(AppDbContext context)
		{
			_context = context;
		}
		public async ValueTask<bool> AddUser(UserDto userDto)
		{
			try
			{

				var check = _context.Users.FirstOrDefault(x => x.TelegramId == userDto.TelegramId);
				if (check is null)
				{

					var chan = new User()
					{
						TelegramId = userDto.TelegramId,
						FirstName = userDto.FirstName??" ",
						LastName = userDto.LastName??" ",
						
					};
					await _context.Users.AddAsync(chan);
					await _context.SaveChangesAsync();
					return true;
				}
				return false;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public async ValueTask<bool> DeleteUser(int Id)
		{
			try
			{

				var check = _context.Users.FirstOrDefault(x => x.Id == Id);
				if (check != null)
				{


					_context.Users.Remove(check);
					await _context.SaveChangesAsync();
					return true;
				}
				return false;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public async ValueTask<List<User>> GetAllUsers()
		{
			var channels = await _context.Users.ToListAsync();
			return channels;
		}

		public async ValueTask<User> GetUserByTelegramId(string id)
		{
			var check = _context.Users.FirstOrDefault(x => x.TelegramId == id);
			return check;

		}

		public async ValueTask<bool> UpdateUser(User user)
		{
			try
			{

				var updatechannel = await _context.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
				if (updatechannel is not null)
				{
					updatechannel.TelegramId = user.TelegramId;
					updatechannel.FirstName = user.FirstName;
					updatechannel.LastName = user.LastName;

				}
				_context.Users.Update(updatechannel);
				await _context.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}
	}
}
