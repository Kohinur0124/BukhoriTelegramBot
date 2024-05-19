using Bukhori.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bukhori.Core.Data
{
	public class AppDbContext:DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options)
		  : base(options) { }
		public DbSet<Book> Books { get; set; }
		public DbSet<Chapters> Chapters { get; set; }
		public DbSet<Hadis> Hadis { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Video> Videos { get; set; }
		public DbSet<Article> Articles { get; set; }
		public DbSet<Comment> Comments { get; set; }

	}
}
