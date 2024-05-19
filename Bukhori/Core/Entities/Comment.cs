namespace Bukhori.Core.Entities
{
	public class Comment
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public string CommentText { get; set; }

		public User User { get; set; }
	}
}
