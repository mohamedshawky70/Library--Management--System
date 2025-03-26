using System.Linq.Expressions;

namespace BookHaven.Web.Repository.BaseRepository
{
	public class BaseRepo<T> : IBaseRepo<T> where T : class
	{
		private readonly ApplicationDbContext _context;

		public BaseRepo(ApplicationDbContext context)
		{
			_context = context;
		}
		public IQueryable<T> GetAll()
		{
			IQueryable<T> Entity = _context.Set<T>().AsNoTracking();//No tracking in db because i readOnly.
			return Entity;
		}
		/*public T GetById(int Id)
		{
			return _context.Set<T>().Find(Id);
		}*/
		public T GetById(int Id)
		{
			return _context.Set<T>().AsNoTracking().FirstOrDefault(e => EF.Property<int>(e, "Id") == Id);
		}

		public T FindMatch(Expression<Func<T, bool>> match, string[] Include = null)//Filter on server side===>filter then get filtration data not get data then filter
		{
			IQueryable<T> obj = _context.Set<T>();
			if (Include != null)
			{
				foreach (var item in Include)
				{
					obj = obj.Include(item);
				}
			}
			return obj.FirstOrDefault(match);
		}
		public IQueryable<T> FindAllMatch(Expression<Func<T, bool>> match, string[] Include = null)//Filter on server side===>filter then get filtration data not get data then filter
		{
			IQueryable<T> obj = _context.Set<T>();
			if (Include != null)
			{
				foreach (var item in Include)
				{
					obj = obj.Include(item);
				}
			}
			return obj.Where(match);
		}
		public T Create(T Entity)
		{
			_context.Set<T>().Add(Entity);
			//_context.SaveChanges();         Just once by unit of work
			return Entity;
		}
		public T Update(T Entity)
		{
			_context.Set<T>().Update(Entity);
			//_context.SaveChanges();          Just once by unit of work
			return Entity;
		}
		public T Delete(T Entity)
		{
			_context.Set<T>().Remove(Entity);
			return Entity;
		}


	}
}
