using Bukhori.Core.Entities.Enums;

namespace Bukhori.Core.Dto
{
	public class ArticleDto
	{
		public string Title { get; set; }
		public LanguageEnums Languages { get; set; }
		public string ArticleUrl { get; set; }
		public int UserId { get; set; }
	}
}
