namespace Bukhori.Core.Entities
{
	public class User
	{
		public int Id { get; set; }
		public string TelegramId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }

		public ICollection<Article > Articles { get; set; }
		public ICollection<Comment> Comments { get; set; }
		public ICollection<Video> Videos { get; set; }
	}
}
