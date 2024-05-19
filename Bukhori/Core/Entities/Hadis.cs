namespace Bukhori.Core.Entities
{
	public class Hadis
	{
		public int Id { get; set; }
		public int ChapterId { get; set; }
		public string FullText { get; set; }

		public Chapters Chapters { get; set; }
	}
}
