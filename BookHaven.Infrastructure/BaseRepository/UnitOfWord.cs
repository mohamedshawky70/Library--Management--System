using BookHaven.Application.Common.Intefaces;

namespace BookHaven.Web.Repository.BaseRepository
{
	public class UnitOfWord : IUnitOfWord
	{
		private readonly ApplicationDbContext _context;
		public IBaseRepo<Category> Categories { get; } //referance
		public IBaseRepo<Author> Authors { get; } //referance
		public IBaseRepo<Book> Books { get; } //referance
		public IBaseRepo<BookCategory> BookCategories { get; } //referance
		public IBaseRepo<CopyBook> CopiesBook { get; } //referance
		public IBaseRepo<Subscriper> Subscriper { get; } //referance
		public IBaseRepo<Area> Area { get; } //referance
		public IBaseRepo<Governorate> Governorate { get; } //referance
		public IBaseRepo<Rental> Rental { get; } //referance


		public UnitOfWord(ApplicationDbContext context)
		{
			_context = context;
			Categories = new BaseRepo<Category>(_context);
			Authors = new BaseRepo<Author>(_context);
			Books = new BaseRepo<Book>(_context);
			BookCategories = new BaseRepo<BookCategory>(_context);
			CopiesBook = new BaseRepo<CopyBook>(_context);
			Subscriper = new BaseRepo<Subscriper>(_context);
			Governorate = new BaseRepo<Governorate>(_context);
			Area = new BaseRepo<Area>(_context);
			Rental = new BaseRepo<Rental>(_context);
		}
		public int Commit()
		{
			return _context.SaveChanges();
		}
	}
}
