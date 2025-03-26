using System.Linq.Expressions;

namespace BookHaven.Web.Repository.BaseRepository
{
	public interface IBaseRepo<T> where T : class
	{
		public IQueryable<T> GetAll();
		public T GetById(int Id);
		public T FindMatch(Expression<Func<T, bool>> match, string[] Include = null);
		public IQueryable<T> FindAllMatch(Expression<Func<T, bool>> match, string[] Include = null);
		public T Create(T Entity);
		public T Update(T Entity);
		public T Delete(T Id);

	}
}