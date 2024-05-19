using Bukhori.Core.Entities.Enums;

namespace Bukhori.Core.Entities
{
	public class Article
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public LanguageEnums Languages { get; set; }
		public string ArticleUrl { get; set; }
		public int UserId { get; set; }

		public User User { get; set; }
	}
}
