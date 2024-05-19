using Bukhori.Core.Data;
using Bukhori.Core.Dto;
using Bukhori.Core.Entities;
using Bukhori.Core.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Bukhori.Core.Repository
{
	public class CommentRepo : ICommentRepo
	{
		private AppDbContext _context;
		public CommentRepo(AppDbContext context)
		{
			_context = context;
		}
		public async ValueTask<bool> AddComment(CommentDto commentDto)
		{
			try
			{

				var chan = new Comment()
					{
						UserId = commentDto.UserId,
						CommentText = commentDto.CommentText,
						
				};
					await _context.Comments.AddAsync(chan);
					await _context.SaveChangesAsync();
					return true;
				
			
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public async ValueTask<bool> DeleteComment(int Id)
		{
			try
			{

				var check = _context.Comments.FirstOrDefault(x => x.Id == Id);
				if (check != null)
				{


					_context.Comments.Remove(check);
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

		public async ValueTask<List<Comment>> GetAllComment()
		{
			var channels = await _context.Comments.ToListAsync();
			return channels;
		}

		public async ValueTask<Comment> GetCommentById(int id)
		{
			var check = _context.Comments.FirstOrDefault(x => x.Id == id);
			return check;

		}

		public async ValueTask<bool> UpdateComment(Comment comment)
		{
			try
			{

				var updatechannel = await _context.Comments.FirstOrDefaultAsync(x => x.Id == comment.Id);
				if (updatechannel is not null)
				{
					updatechannel.UserId = comment.UserId;
					updatechannel.CommentText = comment.CommentText;
					

				}
				_context.Comments.Update(updatechannel);
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
