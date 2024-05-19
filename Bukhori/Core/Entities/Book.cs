namespace Bukhori.Core.Entities
{
	public class Book
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public int MaxCount { get; set; }
		public string Description { get; set; }

		public ICollection<Book> Books { get; set; }
	}
}
