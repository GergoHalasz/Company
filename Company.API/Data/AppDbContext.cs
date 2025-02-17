using Microsoft.EntityFrameworkCore;
namespace Company.API.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
		public DbSet<Models.CompanyEntity> Companies { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Seed();
		}
	}
}
