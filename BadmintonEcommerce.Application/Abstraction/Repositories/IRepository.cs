using System.Linq.Expressions;

namespace BadmintonEcommerce.Application.Abstraction.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> Get(
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        string includeProperties = "",
        int? pageIndex = null,
        int? pageSize = null
    );

    Task<int> Count(Expression<Func<TEntity, bool>> filter = null);

    TEntity? GetById(object id);
    void Insert(TEntity entity);
    Task Delete(object id);
    Task Delete(TEntity entity);
    Task Update(TEntity entity);
    Task SaveChangesAsync();
}