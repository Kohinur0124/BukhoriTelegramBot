using Bukhori.Core.Data;
using Bukhori.Core.Dto;
using Bukhori.Core.Entities;
using Bukhori.Core.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bukhori.Core.Repository
{
	public class ArticleRepo : IArticleRepo
	{
		private AppDbContext _context;
		public ArticleRepo(AppDbContext context)
		{
			_context = context;
		}
		public async ValueTask<bool> AddArticle(ArticleDto articleDto)
		{
			try
			{

				var check = _context.Articles.FirstOrDefault(x => x.ArticleUrl == articleDto.ArticleUrl);
				if (check is null)
				{

					var chan = new Article()
					{
						Title = articleDto.Title,
						Languages = articleDto.Languages,
						UserId = articleDto.UserId,
						ArticleUrl = articleDto.ArticleUrl,
					};
					await _context.Articles.AddAsync(chan);
					await _context.SaveChangesAsync();
					return true;
				}
				return false;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public async ValueTask<bool> DeleteArticle(int Id)
		{
			try
			{

				var check = _context.Articles.FirstOrDefault(x => x.Id == Id);
				if (check != null)
				{


					_context.Articles.Remove(check);
					await _context.SaveChangesAsync();
					return true;
				}
				return false;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public async ValueTask<List<Article>> GetAllArticles()
		{
			var channels = await _context.Articles.ToListAsync();
			return channels;
		}

		public async  ValueTask<Article> GetArticleById(int id)
		{
			var check = _context.Articles.FirstOrDefault(x => x.Id == id);
			return check;

		}

		public async ValueTask<bool> UpdateArticle(Article article)
		{
			try
			{

				var updatechannel = await _context.Articles.FirstOrDefaultAsync(x => x.Id == article.Id);
				if (updatechannel is not null)
				{
					updatechannel.ArticleUrl = article.ArticleUrl;
					updatechannel.UserId = article.UserId;
					updatechannel.Title = article.Title;
					updatechannel.Languages = article.Languages;

				}
				_context.Articles.Update(updatechannel);
				await _context.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}
	}
}
