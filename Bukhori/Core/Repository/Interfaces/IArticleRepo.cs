using Bukhori.Core.Dto;
using Bukhori.Core.Entities;

namespace Bukhori.Core.Repository.Interfaces
{
    public interface IArticleRepo
    {
        public ValueTask<bool> AddArticle(ArticleDto articleDto);
        public ValueTask<bool> UpdateArticle(Article article);
        public ValueTask<bool> DeleteArticle(int Id);
        public ValueTask<List<Article>> GetAllArticles();
        public ValueTask<Article> GetArticleById(int id);
    }
}
