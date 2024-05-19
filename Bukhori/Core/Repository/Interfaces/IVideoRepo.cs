using Bukhori.Core.Dto;
using Bukhori.Core.Entities;

namespace Bukhori.Core.Repository.Interfaces
{
    public interface IVideoRepo
    {
		public ValueTask<bool> AddVideo(VideoDto videoDto);
		public ValueTask<bool> UpdateVideo(Video video);
		public ValueTask<bool> DeleteVideo(int Id);
		public ValueTask<List<Video>> GetAllVideos();
		public ValueTask<Video> GetVideoById(int id);
	}
}
