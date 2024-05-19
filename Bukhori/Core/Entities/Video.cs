namespace Bukhori.Core.Entities
{
	public class Video
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string VideoUrl { get; set; }
		public int UserId { get; set; }

		public User User { get; set; }
	}
}
