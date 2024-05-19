using Bukhori.Core.Data;
using Bukhori.Core.Dto;
using Bukhori.Core.Entities;
using Bukhori.Core.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Channels;

namespace Bukhori.Core.Repository
{
	public class VideoRepo : IVideoRepo
	{
		private AppDbContext _context;
        public VideoRepo(AppDbContext context)
        {
            _context = context;
        }
        public async ValueTask<bool> AddVideo(VideoDto videoDto)
		{
			try
			{

				var check = _context.Videos.FirstOrDefault(x => x.VideoUrl == videoDto.VideoUrl);
				if (check is null)
				{

					var chan = new Video()
					{
						Name = videoDto.Name,
						VideoUrl = videoDto.VideoUrl,
						UserId = videoDto.UserId,
					};
					await _context.Videos.AddAsync(chan);
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

		public async ValueTask<bool> DeleteVideo(int Id)
		{
			try
			{

				var check = _context.Videos.FirstOrDefault(x => x.Id == Id);
				if (check != null)
				{


					_context.Videos.Remove(check);
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

		public async ValueTask<List<Video>> GetAllVideos()
		{
			var channels = await _context.Videos.ToListAsync();
			return channels;
		}

		public async ValueTask<Video> GetVideoById(int id)
		{
			var check = _context.Videos.FirstOrDefault(x => x.Id == id);
			return check;
				
		}

		public async ValueTask<bool> UpdateVideo(Video video)
		{
			try
			{

				var updatechannel = await _context.Videos.FirstOrDefaultAsync(x => x.Id == video.Id);
				if (updatechannel is not null)
				{
					updatechannel.VideoUrl = video.VideoUrl;
					updatechannel.UserId = video.UserId;
					updatechannel.Name = video.Name;

				}
				_context.Videos.Update(updatechannel);
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
