using Bukhori.Core.Dto;
using Bukhori.Core.Entities;

namespace Bukhori.Core.Repository.Interfaces
{
    public interface ICommentRepo
    {
        public ValueTask<bool> AddComment(CommentDto commentDto);
        public ValueTask<bool> UpdateComment(Comment comment);
        public ValueTask<bool> DeleteComment(int Id);
        public ValueTask<List<Comment>> GetAllComment();
        public ValueTask<Comment> GetCommentById(int id);
    }
}
