using System.Linq.Expressions;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Infrastructure.Abstractions;
using BadmintonEcommerce.Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace BadmintonEcommerce.Infrastructure.Repositories;

public class Repository<TEntity>(ApplicationDbContext context) : IRepository<TEntity> where TEntity : class
{
    public async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "", int? pageIndex = null,
        int? pageSize = null)
    {
        IQueryable<TEntity> query = context.Set<TEntity>();
        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split(new char[] { ',' },
                     StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        if (pageIndex.HasValue && pageSize.HasValue)
        {
            int validPageIndex = pageIndex.Value > 0 ? pageIndex.Value - 1 : Constant.Queryable.Default.DefaultPageIndex;
            int validPageSize = pageSize.Value > 0 ? pageSize.Value : Constant.Queryable.Default.DefaultPageSize;

            query = query.Skip(validPageIndex * validPageSize).Take(validPageSize);
        }
            
        return query
            .AsNoTracking()
            .ToList();
    }

    public async Task<int> Count(Expression<Func<TEntity, bool>> filter = null)
    {
        IQueryable<TEntity> query = context.Set<TEntity>();
        query.Where(filter);

        return await query.CountAsync();
    }

    public TEntity? GetById(object id)
    {
        return context.Find<TEntity>(id);
    }

    public void Insert(TEntity entity)
    {
        context.Set<TEntity>().Add(entity);
    }

    public async Task Delete(object id)
    {
        TEntity? entityToDelete = await context.Set<TEntity>().FindAsync(id);
        Console.WriteLine(entityToDelete == null);
        if (entityToDelete != null) await Delete(entityToDelete);
    }

    public async Task Delete(TEntity entity)
    {
        if (context.Entry(entity).State == EntityState.Detached)
        {
            context.Set<TEntity>().Attach(entity);
        }
        
        context.Set<TEntity>().Remove(entity);
    }

    public async Task Update(TEntity entity)
    {
        context.Entry(entity).State = EntityState.Modified;
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}