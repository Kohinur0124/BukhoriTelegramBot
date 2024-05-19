using Bukhori.Core.Dto;
using Bukhori.Core.Entities;

namespace Bukhori.Core.Repository.Interfaces
{
	public interface IUserRepo
	{
		public ValueTask<bool> AddUser(UserDto userDto);
		public ValueTask<bool> UpdateUser(User user);
		public ValueTask<bool> DeleteUser(int Id);
		public ValueTask<List<User>> GetAllUsers();
		public ValueTask<User> GetUserByTelegramId(string id);
	}
}
