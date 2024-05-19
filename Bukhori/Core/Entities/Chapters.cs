namespace Bukhori.Core.Entities
{
	public class Chapters
	{
		public int Id { get; set; }
		public int BookId { get; set; }
		public string Title { get; set; }

		public Book Book { get; set; }
		public ICollection<Hadis> Hadises { get; set; }
	}
}
