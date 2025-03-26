
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Bookify.Web.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}
		public DbSet<Category> Categories { get; set; }
		public DbSet<Author> Authors { get; set; }
		public DbSet<Book> Books { get; set; }
		public DbSet<BookCategory> BookCategories { get; set; }
		public DbSet<CopyBook> CopiesBook { get; set; }
		public DbSet<Subscriper> subscripers { get; set; }
		public DbSet<Area> areas { get; set; }
		public DbSet<Governorate> governorates { get; set; }
		public DbSet<Subcribtion> subcribtions { get; set; }
		public DbSet<Rental> Rentals { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.HasSequence<int>("Serial")
				.StartsAt(100);

			builder.Entity<CopyBook>()
				.Property(e => e.SerialNumber)
				.HasDefaultValueSql("NEXT VALUE FOR Serial");

			builder.Entity<BookCategory>().HasKey(e => new { e.BookId, e.CategoryId });

			var CascadeFks = builder.Model.GetEntityTypes().SelectMany(t => t.GetForeignKeys())
				.Where(fk => fk.DeleteBehavior == DeleteBehavior.Cascade && !fk.IsOwnership);
			foreach (var fk in CascadeFks)
			{
				fk.DeleteBehavior = DeleteBehavior.Restrict;
			}

			base.OnModelCreating(builder);
		}
	}
}
